using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using BibliotecaJK.Components;

namespace BibliotecaJK.Helpers
{
    /// <summary>
    /// AsyncOperationHelper - Static helper class for running async operations with UI feedback
    /// Provides methods for async/await operations with loading panels, progress dialogs, and error handling
    /// </summary>
    public static class AsyncOperationHelper
    {
        /// <summary>
        /// Run async operation with loading panel overlay and error handling
        /// </summary>
        /// <typeparam name="T">Return type of the operation</typeparam>
        /// <param name="parent">Parent form to show loading panel on</param>
        /// <param name="operation">Async operation to execute</param>
        /// <param name="message">Loading message to display</param>
        /// <returns>Result of the operation</returns>
        public static async Task<T> RunAsync<T>(Form parent, Func<Task<T>> operation, string message = "Processando...")
        {
            LoadingPanel? loadingPanel = null;

            try
            {
                // Create and show loading panel
                loadingPanel = new LoadingPanel
                {
                    Mensagem = message,
                    Dock = DockStyle.Fill
                };

                // Add to parent form on UI thread
                if (parent.InvokeRequired)
                {
                    parent.Invoke(new Action(() =>
                    {
                        parent.Controls.Add(loadingPanel);
                        loadingPanel.BringToFront();
                        loadingPanel.Show();
                    }));
                }
                else
                {
                    parent.Controls.Add(loadingPanel);
                    loadingPanel.BringToFront();
                    loadingPanel.Show();
                }

                // Execute async operation
                var result = await operation();

                return result;
            }
            catch (Exception ex)
            {
                // Show error dialog with smart suggestions
                if (parent.InvokeRequired)
                {
                    parent.Invoke(new Action(() =>
                    {
                        ErrorDialog.Show(parent, "Erro na Operação", ex.Message, ex);
                    }));
                }
                else
                {
                    ErrorDialog.Show(parent, "Erro na Operação", ex.Message, ex);
                }

                throw; // Re-throw to allow caller to handle if needed
            }
            finally
            {
                // Hide and remove loading panel
                if (loadingPanel != null)
                {
                    if (parent.InvokeRequired)
                    {
                        parent.Invoke(new Action(() =>
                        {
                            loadingPanel.Hide();
                            parent.Controls.Remove(loadingPanel);
                            loadingPanel.Dispose();
                        }));
                    }
                    else
                    {
                        loadingPanel.Hide();
                        parent.Controls.Remove(loadingPanel);
                        loadingPanel.Dispose();
                    }
                }
            }
        }

        /// <summary>
        /// Run async operation (void return) with loading panel overlay and error handling
        /// </summary>
        /// <param name="parent">Parent form to show loading panel on</param>
        /// <param name="operation">Async operation to execute</param>
        /// <param name="message">Loading message to display</param>
        public static async Task RunAsync(Form parent, Func<Task> operation, string message = "Processando...")
        {
            LoadingPanel? loadingPanel = null;

            try
            {
                // Create and show loading panel
                loadingPanel = new LoadingPanel
                {
                    Mensagem = message,
                    Dock = DockStyle.Fill
                };

                // Add to parent form on UI thread
                if (parent.InvokeRequired)
                {
                    parent.Invoke(new Action(() =>
                    {
                        parent.Controls.Add(loadingPanel);
                        loadingPanel.BringToFront();
                        loadingPanel.Show();
                    }));
                }
                else
                {
                    parent.Controls.Add(loadingPanel);
                    loadingPanel.BringToFront();
                    loadingPanel.Show();
                }

                // Execute async operation
                await operation();
            }
            catch (Exception ex)
            {
                // Show error dialog with smart suggestions
                if (parent.InvokeRequired)
                {
                    parent.Invoke(new Action(() =>
                    {
                        ErrorDialog.Show(parent, "Erro na Operação", ex.Message, ex);
                    }));
                }
                else
                {
                    ErrorDialog.Show(parent, "Erro na Operação", ex.Message, ex);
                }

                throw; // Re-throw to allow caller to handle if needed
            }
            finally
            {
                // Hide and remove loading panel
                if (loadingPanel != null)
                {
                    if (parent.InvokeRequired)
                    {
                        parent.Invoke(new Action(() =>
                        {
                            loadingPanel.Hide();
                            parent.Controls.Remove(loadingPanel);
                            loadingPanel.Dispose();
                        }));
                    }
                    else
                    {
                        loadingPanel.Hide();
                        parent.Controls.Remove(loadingPanel);
                        loadingPanel.Dispose();
                    }
                }
            }
        }

        /// <summary>
        /// Run async operation with progress dialog and progress reporting
        /// </summary>
        /// <typeparam name="T">Return type of the operation</typeparam>
        /// <param name="parent">Parent form for the progress dialog</param>
        /// <param name="operation">Async operation that accepts IProgress<int> for progress reporting</param>
        /// <param name="title">Title of the progress dialog</param>
        /// <param name="isCancellable">Whether the operation can be cancelled</param>
        /// <returns>Result of the operation</returns>
        public static async Task<T> RunWithProgress<T>(
            Form parent,
            Func<IProgress<int>, Task<T>> operation,
            string title,
            bool isCancellable = false)
        {
            ProgressDialog? progressDialog = null;

            try
            {
                // Create progress dialog
                progressDialog = new ProgressDialog(title)
                {
                    IsCancellable = isCancellable
                };

                // Show dialog non-blocking
                progressDialog.Show(parent);

                // Create progress reporter
                var progress = progressDialog.CreateProgressReporter();

                // Execute async operation with progress reporting
                var result = await operation(progress);

                // Complete and close dialog
                progressDialog.Complete("Operação concluída!");

                return result;
            }
            catch (OperationCanceledException)
            {
                // User cancelled - show toast notification
                ToastNotification.Warning("Operação cancelada pelo usuário");
                throw;
            }
            catch (Exception ex)
            {
                // Show error dialog
                ErrorDialog.Show(parent, "Erro na Operação", ex.Message, ex);
                throw;
            }
            finally
            {
                // Ensure dialog is closed
                if (progressDialog != null && !progressDialog.IsDisposed)
                {
                    progressDialog.Close();
                    progressDialog.Dispose();
                }
            }
        }

        /// <summary>
        /// Run async operation with detailed progress reporting (progress + status message)
        /// </summary>
        /// <typeparam name="T">Return type of the operation</typeparam>
        /// <param name="parent">Parent form for the progress dialog</param>
        /// <param name="operation">Async operation that accepts IProgress<(int, string)> for detailed progress reporting</param>
        /// <param name="title">Title of the progress dialog</param>
        /// <param name="isCancellable">Whether the operation can be cancelled</param>
        /// <returns>Result of the operation</returns>
        public static async Task<T> RunWithDetailedProgress<T>(
            Form parent,
            Func<IProgress<(int progress, string status)>, Task<T>> operation,
            string title,
            bool isCancellable = false)
        {
            ProgressDialog? progressDialog = null;

            try
            {
                // Create progress dialog
                progressDialog = new ProgressDialog(title)
                {
                    IsCancellable = isCancellable
                };

                // Show dialog non-blocking
                progressDialog.Show(parent);

                // Create detailed progress reporter
                var progress = progressDialog.CreateDetailedProgressReporter();

                // Execute async operation with progress reporting
                var result = await operation(progress);

                // Complete and close dialog
                progressDialog.Complete("Operação concluída!");

                return result;
            }
            catch (OperationCanceledException)
            {
                // User cancelled - show toast notification
                ToastNotification.Warning("Operação cancelada pelo usuário");
                throw;
            }
            catch (Exception ex)
            {
                // Show error dialog
                ErrorDialog.Show(parent, "Erro na Operação", ex.Message, ex);
                throw;
            }
            finally
            {
                // Ensure dialog is closed
                if (progressDialog != null && !progressDialog.IsDisposed)
                {
                    progressDialog.Close();
                    progressDialog.Dispose();
                }
            }
        }

        /// <summary>
        /// Run async operation with optimistic UI update
        /// Applies the optimistic update immediately, then executes the async operation.
        /// If the operation fails, the rollback action is executed.
        /// </summary>
        /// <typeparam name="T">Return type of the operation</typeparam>
        /// <param name="optimisticUpdate">Action to apply immediately (optimistic update)</param>
        /// <param name="operation">Async operation to execute</param>
        /// <param name="rollback">Action to execute if operation fails (rollback optimistic update)</param>
        /// <returns>Result of the operation</returns>
        public static async Task<T> RunOptimistic<T>(
            Action optimisticUpdate,
            Func<Task<T>> operation,
            Action rollback)
        {
            if (optimisticUpdate == null)
                throw new ArgumentNullException(nameof(optimisticUpdate));
            if (operation == null)
                throw new ArgumentNullException(nameof(operation));
            if (rollback == null)
                throw new ArgumentNullException(nameof(rollback));

            try
            {
                // Apply optimistic update immediately
                optimisticUpdate();

                // Execute async operation
                var result = await operation();

                return result;
            }
            catch (Exception)
            {
                // Rollback optimistic update on failure
                try
                {
                    rollback();
                }
                catch (Exception rollbackEx)
                {
                    // Log rollback failure but don't throw - original exception is more important
                    System.Diagnostics.Debug.WriteLine($"Rollback failed: {rollbackEx.Message}");
                }

                // Re-throw original exception
                throw;
            }
        }

        /// <summary>
        /// Run async operation with optimistic UI update (void return)
        /// </summary>
        /// <param name="optimisticUpdate">Action to apply immediately (optimistic update)</param>
        /// <param name="operation">Async operation to execute</param>
        /// <param name="rollback">Action to execute if operation fails (rollback optimistic update)</param>
        public static async Task RunOptimistic(
            Action optimisticUpdate,
            Func<Task> operation,
            Action rollback)
        {
            if (optimisticUpdate == null)
                throw new ArgumentNullException(nameof(optimisticUpdate));
            if (operation == null)
                throw new ArgumentNullException(nameof(operation));
            if (rollback == null)
                throw new ArgumentNullException(nameof(rollback));

            try
            {
                // Apply optimistic update immediately
                optimisticUpdate();

                // Execute async operation
                await operation();
            }
            catch (Exception)
            {
                // Rollback optimistic update on failure
                try
                {
                    rollback();
                }
                catch (Exception rollbackEx)
                {
                    // Log rollback failure but don't throw - original exception is more important
                    System.Diagnostics.Debug.WriteLine($"Rollback failed: {rollbackEx.Message}");
                }

                // Re-throw original exception
                throw;
            }
        }

        /// <summary>
        /// Run async operation with retry logic
        /// </summary>
        /// <typeparam name="T">Return type of the operation</typeparam>
        /// <param name="operation">Async operation to execute</param>
        /// <param name="maxRetries">Maximum number of retry attempts</param>
        /// <param name="delayMilliseconds">Delay between retries in milliseconds</param>
        /// <returns>Result of the operation</returns>
        public static async Task<T> RunWithRetry<T>(
            Func<Task<T>> operation,
            int maxRetries = 3,
            int delayMilliseconds = 1000)
        {
            if (operation == null)
                throw new ArgumentNullException(nameof(operation));
            if (maxRetries < 0)
                throw new ArgumentOutOfRangeException(nameof(maxRetries), "Max retries must be non-negative");
            if (delayMilliseconds < 0)
                throw new ArgumentOutOfRangeException(nameof(delayMilliseconds), "Delay must be non-negative");

            Exception? lastException = null;

            for (int attempt = 0; attempt <= maxRetries; attempt++)
            {
                try
                {
                    return await operation();
                }
                catch (Exception ex)
                {
                    lastException = ex;

                    // Don't retry on last attempt
                    if (attempt < maxRetries)
                    {
                        System.Diagnostics.Debug.WriteLine($"Operation failed (attempt {attempt + 1}/{maxRetries + 1}): {ex.Message}");
                        await Task.Delay(delayMilliseconds);
                    }
                }
            }

            // All retries exhausted - throw last exception
            throw lastException!;
        }
    }
}
