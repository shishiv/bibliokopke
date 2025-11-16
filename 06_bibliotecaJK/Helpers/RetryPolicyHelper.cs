using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;
using Npgsql;

namespace BibliotecaJK.Helpers
{
    /// <summary>
    /// Backoff strategy for retry delays
    /// </summary>
    public enum BackoffStrategy
    {
        /// <summary>
        /// Linear backoff - same delay each time
        /// </summary>
        Linear,

        /// <summary>
        /// Exponential backoff - doubles each time (2^n)
        /// </summary>
        Exponential,

        /// <summary>
        /// Fibonacci backoff - follows fibonacci sequence
        /// </summary>
        Fibonacci
    }

    /// <summary>
    /// Configuration for retry behavior
    /// </summary>
    public class RetryPolicy
    {
        /// <summary>
        /// Maximum number of retry attempts
        /// </summary>
        public int MaxRetries { get; set; } = 3;

        /// <summary>
        /// Initial delay in milliseconds
        /// </summary>
        public int InitialDelayMs { get; set; } = 1000;

        /// <summary>
        /// Maximum delay in milliseconds (caps calculated delays)
        /// </summary>
        public int MaxDelayMs { get; set; } = 30000;

        /// <summary>
        /// Backoff strategy to use for calculating delays
        /// </summary>
        public BackoffStrategy BackoffStrategy { get; set; } = BackoffStrategy.Exponential;

        /// <summary>
        /// List of exception types that should trigger a retry
        /// </summary>
        public List<Type> RetryOn { get; set; } = new List<Type>();

        /// <summary>
        /// Optional callback invoked on each retry attempt
        /// Parameters: (exception, attemptNumber)
        /// </summary>
        public Action<Exception, int>? OnRetry { get; set; }

        /// <summary>
        /// Enable jitter to prevent thundering herd problem
        /// </summary>
        public bool EnableJitter { get; set; } = true;

        /// <summary>
        /// Jitter factor (0.0 to 1.0) - percentage of randomization to apply
        /// </summary>
        public double JitterFactor { get; set; } = 0.25;
    }

    /// <summary>
    /// Static helper class for executing operations with retry logic
    /// Provides comprehensive retry policies with backoff strategies and exception filtering
    /// </summary>
    public static class RetryPolicyHelper
    {
        private static readonly Random _random = new Random();

        /// <summary>
        /// Executes a synchronous operation with retry logic
        /// </summary>
        /// <typeparam name="T">Return type of the operation</typeparam>
        /// <param name="operation">The operation to execute</param>
        /// <param name="policy">Retry policy configuration</param>
        /// <returns>Result of the operation</returns>
        /// <exception cref="ArgumentNullException">Thrown when operation or policy is null</exception>
        public static T ExecuteWithRetry<T>(Func<T> operation, RetryPolicy policy)
        {
            if (operation == null)
                throw new ArgumentNullException(nameof(operation));
            if (policy == null)
                throw new ArgumentNullException(nameof(policy));

            int attemptNumber = 0;
            Exception? lastException = null;

            while (attemptNumber <= policy.MaxRetries)
            {
                try
                {
                    return operation();
                }
                catch (Exception ex)
                {
                    lastException = ex;

                    // Check if we should retry this exception
                    if (!ShouldRetry(ex, policy))
                    {
                        throw;
                    }

                    // Check if we have retries left
                    if (attemptNumber >= policy.MaxRetries)
                    {
                        throw;
                    }

                    // Invoke retry callback if provided
                    policy.OnRetry?.Invoke(ex, attemptNumber + 1);

                    // Calculate and apply delay
                    int delay = CalculateDelay(attemptNumber, policy);
                    System.Threading.Thread.Sleep(delay);

                    attemptNumber++;
                }
            }

            // This should never be reached, but just in case
            if (lastException != null)
                throw lastException;

            throw new InvalidOperationException("Retry logic error: no exception but operation failed");
        }

        /// <summary>
        /// Executes an asynchronous operation with retry logic
        /// </summary>
        /// <typeparam name="T">Return type of the operation</typeparam>
        /// <param name="operation">The async operation to execute</param>
        /// <param name="policy">Retry policy configuration</param>
        /// <returns>Task containing the result of the operation</returns>
        /// <exception cref="ArgumentNullException">Thrown when operation or policy is null</exception>
        public static async Task<T> ExecuteWithRetryAsync<T>(Func<Task<T>> operation, RetryPolicy policy)
        {
            if (operation == null)
                throw new ArgumentNullException(nameof(operation));
            if (policy == null)
                throw new ArgumentNullException(nameof(policy));

            int attemptNumber = 0;
            Exception? lastException = null;

            while (attemptNumber <= policy.MaxRetries)
            {
                try
                {
                    return await operation();
                }
                catch (Exception ex)
                {
                    lastException = ex;

                    // Check if we should retry this exception
                    if (!ShouldRetry(ex, policy))
                    {
                        throw;
                    }

                    // Check if we have retries left
                    if (attemptNumber >= policy.MaxRetries)
                    {
                        throw;
                    }

                    // Invoke retry callback if provided
                    policy.OnRetry?.Invoke(ex, attemptNumber + 1);

                    // Calculate and apply delay
                    int delay = CalculateDelay(attemptNumber, policy);
                    await Task.Delay(delay);

                    attemptNumber++;
                }
            }

            // This should never be reached, but just in case
            if (lastException != null)
                throw lastException;

            throw new InvalidOperationException("Retry logic error: no exception but operation failed");
        }

        /// <summary>
        /// Executes a synchronous void operation with retry logic
        /// </summary>
        /// <param name="operation">The operation to execute</param>
        /// <param name="policy">Retry policy configuration</param>
        /// <exception cref="ArgumentNullException">Thrown when operation or policy is null</exception>
        public static void ExecuteWithRetry(Action operation, RetryPolicy policy)
        {
            if (operation == null)
                throw new ArgumentNullException(nameof(operation));
            if (policy == null)
                throw new ArgumentNullException(nameof(policy));

            ExecuteWithRetry(() =>
            {
                operation();
                return true; // Dummy return value
            }, policy);
        }

        /// <summary>
        /// Executes an asynchronous void operation with retry logic
        /// </summary>
        /// <param name="operation">The async operation to execute</param>
        /// <param name="policy">Retry policy configuration</param>
        /// <returns>Task representing the async operation</returns>
        /// <exception cref="ArgumentNullException">Thrown when operation or policy is null</exception>
        public static async Task ExecuteWithRetryAsync(Func<Task> operation, RetryPolicy policy)
        {
            if (operation == null)
                throw new ArgumentNullException(nameof(operation));
            if (policy == null)
                throw new ArgumentNullException(nameof(policy));

            await ExecuteWithRetryAsync(async () =>
            {
                await operation();
                return true; // Dummy return value
            }, policy);
        }

        /// <summary>
        /// Creates a default retry policy with sensible defaults
        /// Retries on: IOException, TimeoutException, NpgsqlException
        /// 3 retries, exponential backoff with jitter
        /// </summary>
        /// <returns>Default retry policy</returns>
        public static RetryPolicy CreateDefaultPolicy()
        {
            return new RetryPolicy
            {
                MaxRetries = 3,
                InitialDelayMs = 1000,
                MaxDelayMs = 30000,
                BackoffStrategy = BackoffStrategy.Exponential,
                EnableJitter = true,
                RetryOn = new List<Type>
                {
                    typeof(IOException),
                    typeof(TimeoutException),
                    typeof(NpgsqlException)
                }
            };
        }

        /// <summary>
        /// Creates a policy specialized for database operations
        /// 5 retries, exponential backoff with jitter
        /// Retries only on transient database errors
        /// </summary>
        /// <returns>Database retry policy</returns>
        public static RetryPolicy CreateDatabasePolicy()
        {
            return new RetryPolicy
            {
                MaxRetries = 5,
                InitialDelayMs = 500,
                MaxDelayMs = 10000,
                BackoffStrategy = BackoffStrategy.Exponential,
                EnableJitter = true,
                RetryOn = new List<Type>
                {
                    typeof(NpgsqlException),
                    typeof(TimeoutException),
                    typeof(InvalidOperationException) // Connection pool exhaustion, etc.
                },
                OnRetry = (ex, attempt) =>
                {
                    // Log database retry attempts
                    Console.WriteLine($"[RetryPolicy] Database operation failed (attempt {attempt}): {ex.Message}");
                }
            };
        }

        /// <summary>
        /// Creates a policy specialized for network operations
        /// 3 retries, exponential backoff with jitter
        /// Retries on: WebException, HttpRequestException, SocketException
        /// </summary>
        /// <returns>Network retry policy</returns>
        public static RetryPolicy CreateNetworkPolicy()
        {
            return new RetryPolicy
            {
                MaxRetries = 3,
                InitialDelayMs = 2000,
                MaxDelayMs = 20000,
                BackoffStrategy = BackoffStrategy.Exponential,
                EnableJitter = true,
                RetryOn = new List<Type>
                {
                    typeof(WebException),
                    typeof(HttpRequestException),
                    typeof(SocketException),
                    typeof(TimeoutException)
                },
                OnRetry = (ex, attempt) =>
                {
                    Console.WriteLine($"[RetryPolicy] Network operation failed (attempt {attempt}): {ex.Message}");
                }
            };
        }

        /// <summary>
        /// Creates a policy specialized for file I/O operations
        /// 5 retries, linear backoff
        /// Retries on: IOException, UnauthorizedAccessException
        /// </summary>
        /// <returns>File I/O retry policy</returns>
        public static RetryPolicy CreateFileIOPolicy()
        {
            return new RetryPolicy
            {
                MaxRetries = 5,
                InitialDelayMs = 500,
                MaxDelayMs = 5000,
                BackoffStrategy = BackoffStrategy.Linear,
                EnableJitter = false, // Linear backoff works well without jitter for file I/O
                RetryOn = new List<Type>
                {
                    typeof(IOException),
                    typeof(UnauthorizedAccessException)
                },
                OnRetry = (ex, attempt) =>
                {
                    Console.WriteLine($"[RetryPolicy] File I/O operation failed (attempt {attempt}): {ex.Message}");
                }
            };
        }

        /// <summary>
        /// Calculates the delay for the current retry attempt based on the backoff strategy
        /// </summary>
        /// <param name="attemptNumber">Current attempt number (0-based)</param>
        /// <param name="policy">Retry policy configuration</param>
        /// <returns>Delay in milliseconds, capped at MaxDelayMs</returns>
        public static int CalculateDelay(int attemptNumber, RetryPolicy policy)
        {
            if (policy == null)
                throw new ArgumentNullException(nameof(policy));

            int baseDelay;

            switch (policy.BackoffStrategy)
            {
                case BackoffStrategy.Linear:
                    baseDelay = policy.InitialDelayMs * (attemptNumber + 1);
                    break;

                case BackoffStrategy.Exponential:
                    // 2^attemptNumber * InitialDelayMs
                    baseDelay = (int)(Math.Pow(2, attemptNumber) * policy.InitialDelayMs);
                    break;

                case BackoffStrategy.Fibonacci:
                    // Fibonacci sequence: 1, 1, 2, 3, 5, 8, 13, 21...
                    int fibNumber = CalculateFibonacci(attemptNumber + 1);
                    baseDelay = fibNumber * policy.InitialDelayMs;
                    break;

                default:
                    baseDelay = policy.InitialDelayMs;
                    break;
            }

            // Cap at maximum delay
            baseDelay = Math.Min(baseDelay, policy.MaxDelayMs);

            // Apply jitter if enabled
            if (policy.EnableJitter && policy.JitterFactor > 0)
            {
                // Calculate jitter range: +/- (baseDelay * jitterFactor)
                int jitterRange = (int)(baseDelay * policy.JitterFactor);
                int jitter = _random.Next(-jitterRange, jitterRange + 1);
                baseDelay = Math.Max(0, baseDelay + jitter);
            }

            return baseDelay;
        }

        /// <summary>
        /// Checks if the given exception type should trigger a retry
        /// </summary>
        /// <param name="ex">Exception to check</param>
        /// <param name="policy">Retry policy configuration</param>
        /// <returns>True if the exception should trigger a retry, false otherwise</returns>
        public static bool ShouldRetry(Exception ex, RetryPolicy policy)
        {
            if (ex == null || policy == null)
                return false;

            if (policy.RetryOn == null || policy.RetryOn.Count == 0)
                return true; // Retry all exceptions if no filter specified

            // Check if the exception type matches any in the retry list
            Type exceptionType = ex.GetType();
            foreach (var retryType in policy.RetryOn)
            {
                if (retryType.IsAssignableFrom(exceptionType))
                    return true;
            }

            // Check for NpgsqlException and filter transient errors
            if (ex is NpgsqlException npgsqlEx)
            {
                return IsTransientNpgsqlError(npgsqlEx);
            }

            return false;
        }

        /// <summary>
        /// Calculates the nth Fibonacci number (1-based index)
        /// </summary>
        /// <param name="n">Position in Fibonacci sequence (1-based)</param>
        /// <returns>Fibonacci number at position n</returns>
        private static int CalculateFibonacci(int n)
        {
            if (n <= 0) return 0;
            if (n == 1 || n == 2) return 1;

            int prev = 1, curr = 1;
            for (int i = 3; i <= n; i++)
            {
                int next = prev + curr;
                prev = curr;
                curr = next;
            }
            return curr;
        }

        /// <summary>
        /// Determines if an NpgsqlException represents a transient error that can be retried
        /// </summary>
        /// <param name="ex">The NpgsqlException to check</param>
        /// <returns>True if the error is transient and can be retried</returns>
        private static bool IsTransientNpgsqlError(NpgsqlException ex)
        {
            // Transient error codes from PostgreSQL
            // These are errors that may succeed if retried
            var transientErrorCodes = new HashSet<string>
            {
                "53000", // insufficient_resources
                "53100", // disk_full
                "53200", // out_of_memory
                "53300", // too_many_connections
                "53400", // configuration_limit_exceeded
                "57P03", // cannot_connect_now
                "58000", // system_error
                "58030", // io_error
                "40001", // serialization_failure
                "40P01", // deadlock_detected
                "08000", // connection_exception
                "08003", // connection_does_not_exist
                "08006", // connection_failure
                "08001", // sqlclient_unable_to_establish_sqlconnection
                "08004", // sqlserver_rejected_establishment_of_sqlconnection
            };

            // Check if the error code matches any transient errors
            if (ex.SqlState != null && transientErrorCodes.Contains(ex.SqlState))
            {
                return true;
            }

            // Also retry on timeout-related errors
            if (ex.Message != null && (
                ex.Message.Contains("timeout") ||
                ex.Message.Contains("time out") ||
                ex.Message.Contains("connection pool") ||
                ex.Message.Contains("too many clients")))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Creates a custom retry policy with specified parameters
        /// </summary>
        /// <param name="maxRetries">Maximum number of retry attempts</param>
        /// <param name="initialDelayMs">Initial delay in milliseconds</param>
        /// <param name="strategy">Backoff strategy to use</param>
        /// <param name="retryOn">Exception types to retry on (null = retry all)</param>
        /// <returns>Custom retry policy</returns>
        public static RetryPolicy CreateCustomPolicy(
            int maxRetries = 3,
            int initialDelayMs = 1000,
            BackoffStrategy strategy = BackoffStrategy.Exponential,
            List<Type>? retryOn = null)
        {
            return new RetryPolicy
            {
                MaxRetries = maxRetries,
                InitialDelayMs = initialDelayMs,
                MaxDelayMs = 30000,
                BackoffStrategy = strategy,
                EnableJitter = strategy == BackoffStrategy.Exponential,
                RetryOn = retryOn ?? new List<Type>()
            };
        }
    }
}
