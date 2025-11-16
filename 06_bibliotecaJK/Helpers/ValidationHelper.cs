using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using BibliotecaJK.BLL;

namespace BibliotecaJK.Helpers
{
    /// <summary>
    /// ValidationHelper - Utility class for creating reusable, composable validators
    /// Complements the existing Validadores class with modern, functional validation patterns
    /// </summary>
    /// <remarks>
    /// This class provides factory methods that return validator functions compatible with ValidatedTextBox
    /// and utility methods for validating entire forms. All validators return (bool IsValid, string ErrorMessage).
    ///
    /// Example usage:
    /// <code>
    /// var txtNome = new ValidatedTextBox
    /// {
    ///     FloatingLabel = "Nome",
    ///     Validator = ValidationHelper.Required("Nome")
    /// };
    ///
    /// var txtCPF = new ValidatedTextBox
    /// {
    ///     FloatingLabel = "CPF",
    ///     Validator = ValidationHelper.Composite(
    ///         ValidationHelper.Required("CPF"),
    ///         ValidationHelper.CPF()
    ///     )
    /// };
    /// </code>
    /// </remarks>
    public static class ValidationHelper
    {
        #region Basic Validators

        /// <summary>
        /// Creates a validator that checks for non-empty input
        /// </summary>
        /// <param name="fieldName">Name of the field being validated</param>
        /// <param name="customMessage">Custom error message (optional)</param>
        /// <returns>Validator function that checks if value is not empty</returns>
        /// <example>
        /// <code>
        /// var validator = ValidationHelper.Required("Nome");
        /// var (isValid, message) = validator("");  // (false, "Nome é obrigatório")
        /// </code>
        /// </example>
        public static Func<string, (bool IsValid, string ErrorMessage)> Required(
            string fieldName,
            string? customMessage = null)
        {
            return (value) =>
            {
                bool isValid = !string.IsNullOrWhiteSpace(value);
                string message = isValid ? string.Empty :
                    customMessage ?? $"{fieldName} é obrigatório";
                return (isValid, message);
            };
        }

        /// <summary>
        /// Creates a validator that checks minimum length
        /// </summary>
        /// <param name="length">Minimum length required</param>
        /// <param name="fieldName">Name of the field being validated</param>
        /// <param name="customMessage">Custom error message (optional)</param>
        /// <returns>Validator function that checks minimum length</returns>
        /// <example>
        /// <code>
        /// var validator = ValidationHelper.MinLength(3, "Nome");
        /// var (isValid, message) = validator("Ab");  // (false, "Nome deve ter no mínimo 3 caracteres")
        /// </code>
        /// </example>
        public static Func<string, (bool IsValid, string ErrorMessage)> MinLength(
            int length,
            string fieldName,
            string? customMessage = null)
        {
            return (value) =>
            {
                // Empty values pass - use Required() to enforce non-empty
                if (string.IsNullOrEmpty(value))
                    return (true, string.Empty);

                bool isValid = value.Length >= length;
                string message = isValid ? string.Empty :
                    customMessage ?? $"{fieldName} deve ter no mínimo {length} caracteres";
                return (isValid, message);
            };
        }

        /// <summary>
        /// Creates a validator that checks maximum length
        /// </summary>
        /// <param name="length">Maximum length allowed</param>
        /// <param name="fieldName">Name of the field being validated</param>
        /// <param name="customMessage">Custom error message (optional)</param>
        /// <returns>Validator function that checks maximum length</returns>
        /// <example>
        /// <code>
        /// var validator = ValidationHelper.MaxLength(50, "Nome");
        /// var (isValid, message) = validator(new string('A', 51));  // (false, "Nome deve ter no máximo 50 caracteres")
        /// </code>
        /// </example>
        public static Func<string, (bool IsValid, string ErrorMessage)> MaxLength(
            int length,
            string fieldName,
            string? customMessage = null)
        {
            return (value) =>
            {
                // Empty values pass
                if (string.IsNullOrEmpty(value))
                    return (true, string.Empty);

                bool isValid = value.Length <= length;
                string message = isValid ? string.Empty :
                    customMessage ?? $"{fieldName} deve ter no máximo {length} caracteres";
                return (isValid, message);
            };
        }

        /// <summary>
        /// Creates a validator that checks if length is within a range
        /// </summary>
        /// <param name="min">Minimum length</param>
        /// <param name="max">Maximum length</param>
        /// <param name="fieldName">Name of the field being validated</param>
        /// <param name="customMessage">Custom error message (optional)</param>
        /// <returns>Validator function that checks length range</returns>
        /// <example>
        /// <code>
        /// var validator = ValidationHelper.LengthRange(3, 20, "Matrícula");
        /// var (isValid, message) = validator("AB");  // (false, "Matrícula deve ter entre 3 e 20 caracteres")
        /// </code>
        /// </example>
        public static Func<string, (bool IsValid, string ErrorMessage)> LengthRange(
            int min,
            int max,
            string fieldName,
            string? customMessage = null)
        {
            return (value) =>
            {
                // Empty values pass
                if (string.IsNullOrEmpty(value))
                    return (true, string.Empty);

                bool isValid = value.Length >= min && value.Length <= max;
                string message = isValid ? string.Empty :
                    customMessage ?? $"{fieldName} deve ter entre {min} e {max} caracteres";
                return (isValid, message);
            };
        }

        #endregion

        #region Pattern Validators

        /// <summary>
        /// Creates a validator that checks if value matches a regex pattern
        /// </summary>
        /// <param name="regex">Regular expression pattern</param>
        /// <param name="fieldName">Name of the field being validated</param>
        /// <param name="invalidMessage">Message to display when pattern doesn't match</param>
        /// <returns>Validator function that checks regex pattern</returns>
        /// <example>
        /// <code>
        /// var validator = ValidationHelper.Pattern(@"^\d{5}-\d{3}$", "CEP", "CEP deve estar no formato 00000-000");
        /// var (isValid, message) = validator("12345678");  // (false, "CEP deve estar no formato 00000-000")
        /// </code>
        /// </example>
        public static Func<string, (bool IsValid, string ErrorMessage)> Pattern(
            string regex,
            string fieldName,
            string invalidMessage)
        {
            return (value) =>
            {
                // Empty values pass
                if (string.IsNullOrEmpty(value))
                    return (true, string.Empty);

                try
                {
                    bool isValid = Regex.IsMatch(value, regex);
                    return (isValid, isValid ? string.Empty : invalidMessage);
                }
                catch (Exception ex)
                {
                    return (false, $"Erro na validação de {fieldName}: {ex.Message}");
                }
            };
        }

        #endregion

        #region Domain-Specific Validators (Integration with Validadores)

        /// <summary>
        /// Creates a validator for email format using Validadores.ValidarEmail()
        /// </summary>
        /// <param name="fieldName">Name of the field (defaults to "E-mail")</param>
        /// <param name="customMessage">Custom error message (optional)</param>
        /// <returns>Validator function that checks email format</returns>
        /// <example>
        /// <code>
        /// var validator = ValidationHelper.Email();
        /// var (isValid, message) = validator("usuario@exemplo.com");  // (true, "")
        /// var (isValid2, message2) = validator("invalid-email");  // (false, "E-mail inválido")
        /// </code>
        /// </example>
        public static Func<string, (bool IsValid, string ErrorMessage)> Email(
            string fieldName = "E-mail",
            string? customMessage = null)
        {
            return (value) =>
            {
                // Empty values pass - use Required() to enforce non-empty
                if (string.IsNullOrWhiteSpace(value))
                    return (true, string.Empty);

                bool isValid = Validadores.ValidarEmail(value);
                string message = isValid ? string.Empty :
                    customMessage ?? $"{fieldName} inválido";
                return (isValid, message);
            };
        }

        /// <summary>
        /// Creates a validator for CPF format using Validadores.ValidarCPF()
        /// </summary>
        /// <param name="fieldName">Name of the field (defaults to "CPF")</param>
        /// <param name="customMessage">Custom error message (optional)</param>
        /// <returns>Validator function that checks CPF format and check digits</returns>
        /// <example>
        /// <code>
        /// var validator = ValidationHelper.CPF();
        /// var (isValid, message) = validator("123.456.789-09");  // Check if valid CPF
        /// </code>
        /// </example>
        public static Func<string, (bool IsValid, string ErrorMessage)> CPF(
            string fieldName = "CPF",
            string? customMessage = null)
        {
            return (value) =>
            {
                // Empty values pass - use Required() to enforce non-empty
                if (string.IsNullOrWhiteSpace(value))
                    return (true, string.Empty);

                bool isValid = Validadores.ValidarCPF(value);
                string message = isValid ? string.Empty :
                    customMessage ?? $"{fieldName} inválido";
                return (isValid, message);
            };
        }

        /// <summary>
        /// Creates a validator for ISBN format using Validadores.ValidarISBN()
        /// Validates both ISBN-10 and ISBN-13 formats
        /// </summary>
        /// <param name="fieldName">Name of the field (defaults to "ISBN")</param>
        /// <param name="customMessage">Custom error message (optional)</param>
        /// <returns>Validator function that checks ISBN format</returns>
        /// <example>
        /// <code>
        /// var validator = ValidationHelper.ISBN();
        /// var (isValid, message) = validator("978-3-16-148410-0");  // Check if valid ISBN
        /// </code>
        /// </example>
        public static Func<string, (bool IsValid, string ErrorMessage)> ISBN(
            string fieldName = "ISBN",
            string? customMessage = null)
        {
            return (value) =>
            {
                // Empty values pass - use Required() to enforce non-empty
                if (string.IsNullOrWhiteSpace(value))
                    return (true, string.Empty);

                bool isValid = Validadores.ValidarISBN(value);
                string message = isValid ? string.Empty :
                    customMessage ?? $"{fieldName} inválido";
                return (isValid, message);
            };
        }

        /// <summary>
        /// Creates a validator for student enrollment number (matrícula) using Validadores.ValidarMatricula()
        /// </summary>
        /// <param name="fieldName">Name of the field (defaults to "Matrícula")</param>
        /// <param name="customMessage">Custom error message (optional)</param>
        /// <returns>Validator function that checks matrícula format (3-20 alphanumeric characters)</returns>
        /// <example>
        /// <code>
        /// var validator = ValidationHelper.Matricula();
        /// var (isValid, message) = validator("ALU2023001");  // (true, "")
        /// </code>
        /// </example>
        public static Func<string, (bool IsValid, string ErrorMessage)> Matricula(
            string fieldName = "Matrícula",
            string? customMessage = null)
        {
            return (value) =>
            {
                // Empty values pass - use Required() to enforce non-empty
                if (string.IsNullOrWhiteSpace(value))
                    return (true, string.Empty);

                bool isValid = Validadores.ValidarMatricula(value);
                string message = isValid ? string.Empty :
                    customMessage ?? $"{fieldName} inválida (deve ter entre 3 e 20 caracteres alfanuméricos)";
                return (isValid, message);
            };
        }

        #endregion

        #region Numeric Validators

        /// <summary>
        /// Creates a validator that checks if value is numeric
        /// </summary>
        /// <param name="fieldName">Name of the field being validated</param>
        /// <param name="customMessage">Custom error message (optional)</param>
        /// <returns>Validator function that checks if value can be parsed as decimal</returns>
        /// <example>
        /// <code>
        /// var validator = ValidationHelper.Numeric("Preço");
        /// var (isValid, message) = validator("123.45");  // (true, "")
        /// var (isValid2, message2) = validator("abc");  // (false, "Preço deve ser um número")
        /// </code>
        /// </example>
        public static Func<string, (bool IsValid, string ErrorMessage)> Numeric(
            string fieldName,
            string? customMessage = null)
        {
            return (value) =>
            {
                // Empty values pass
                if (string.IsNullOrWhiteSpace(value))
                    return (true, string.Empty);

                bool isValid = decimal.TryParse(value, out _);
                string message = isValid ? string.Empty :
                    customMessage ?? $"{fieldName} deve ser um número";
                return (isValid, message);
            };
        }

        /// <summary>
        /// Creates a validator that checks if value is an integer within a range
        /// </summary>
        /// <param name="min">Minimum value (inclusive)</param>
        /// <param name="max">Maximum value (inclusive)</param>
        /// <param name="fieldName">Name of the field being validated</param>
        /// <param name="customMessage">Custom error message (optional)</param>
        /// <returns>Validator function that checks integer range</returns>
        /// <example>
        /// <code>
        /// var validator = ValidationHelper.IntegerRange(1, 100, "Quantidade");
        /// var (isValid, message) = validator("50");  // (true, "")
        /// var (isValid2, message2) = validator("150");  // (false, "Quantidade deve estar entre 1 e 100")
        /// </code>
        /// </example>
        public static Func<string, (bool IsValid, string ErrorMessage)> IntegerRange(
            int min,
            int max,
            string fieldName,
            string? customMessage = null)
        {
            return (value) =>
            {
                // Empty values pass
                if (string.IsNullOrWhiteSpace(value))
                    return (true, string.Empty);

                if (!int.TryParse(value, out int intValue))
                {
                    return (false, $"{fieldName} deve ser um número inteiro");
                }

                bool isValid = intValue >= min && intValue <= max;
                string message = isValid ? string.Empty :
                    customMessage ?? $"{fieldName} deve estar entre {min} e {max}";
                return (isValid, message);
            };
        }

        /// <summary>
        /// Creates a validator that checks if value is a decimal within a range
        /// </summary>
        /// <param name="min">Minimum value (inclusive)</param>
        /// <param name="max">Maximum value (inclusive)</param>
        /// <param name="fieldName">Name of the field being validated</param>
        /// <param name="customMessage">Custom error message (optional)</param>
        /// <returns>Validator function that checks decimal range</returns>
        /// <example>
        /// <code>
        /// var validator = ValidationHelper.DecimalRange(0.01m, 9999.99m, "Preço");
        /// var (isValid, message) = validator("150.50");  // (true, "")
        /// </code>
        /// </example>
        public static Func<string, (bool IsValid, string ErrorMessage)> DecimalRange(
            decimal min,
            decimal max,
            string fieldName,
            string? customMessage = null)
        {
            return (value) =>
            {
                // Empty values pass
                if (string.IsNullOrWhiteSpace(value))
                    return (true, string.Empty);

                if (!decimal.TryParse(value, out decimal decimalValue))
                {
                    return (false, $"{fieldName} deve ser um número");
                }

                bool isValid = decimalValue >= min && decimalValue <= max;
                string message = isValid ? string.Empty :
                    customMessage ?? $"{fieldName} deve estar entre {min:N2} e {max:N2}";
                return (isValid, message);
            };
        }

        #endregion

        #region Date Validators

        /// <summary>
        /// Creates a validator that checks if value is a valid date
        /// </summary>
        /// <param name="fieldName">Name of the field being validated</param>
        /// <param name="customMessage">Custom error message (optional)</param>
        /// <returns>Validator function that checks if value can be parsed as DateTime</returns>
        /// <example>
        /// <code>
        /// var validator = ValidationHelper.Date("Data de Nascimento");
        /// var (isValid, message) = validator("01/01/2000");  // (true, "")
        /// </code>
        /// </example>
        public static Func<string, (bool IsValid, string ErrorMessage)> Date(
            string fieldName,
            string? customMessage = null)
        {
            return (value) =>
            {
                // Empty values pass
                if (string.IsNullOrWhiteSpace(value))
                    return (true, string.Empty);

                bool isValid = DateTime.TryParse(value, out _);
                string message = isValid ? string.Empty :
                    customMessage ?? $"{fieldName} deve ser uma data válida";
                return (isValid, message);
            };
        }

        /// <summary>
        /// Creates a validator that checks if date is within a range
        /// </summary>
        /// <param name="min">Minimum date (inclusive)</param>
        /// <param name="max">Maximum date (inclusive)</param>
        /// <param name="fieldName">Name of the field being validated</param>
        /// <param name="customMessage">Custom error message (optional)</param>
        /// <returns>Validator function that checks date range</returns>
        /// <example>
        /// <code>
        /// var min = new DateTime(1900, 1, 1);
        /// var max = DateTime.Today;
        /// var validator = ValidationHelper.DateRange(min, max, "Data de Nascimento");
        /// </code>
        /// </example>
        public static Func<string, (bool IsValid, string ErrorMessage)> DateRange(
            DateTime min,
            DateTime max,
            string fieldName,
            string? customMessage = null)
        {
            return (value) =>
            {
                // Empty values pass
                if (string.IsNullOrWhiteSpace(value))
                    return (true, string.Empty);

                if (!DateTime.TryParse(value, out DateTime dateValue))
                {
                    return (false, $"{fieldName} deve ser uma data válida");
                }

                bool isValid = dateValue >= min && dateValue <= max;
                string message = isValid ? string.Empty :
                    customMessage ?? $"{fieldName} deve estar entre {min:dd/MM/yyyy} e {max:dd/MM/yyyy}";
                return (isValid, message);
            };
        }

        #endregion

        #region Custom & Composite Validators

        /// <summary>
        /// Creates a custom validator from a predicate function
        /// </summary>
        /// <param name="predicate">Function that returns true if value is valid</param>
        /// <param name="errorMessage">Error message to display when invalid</param>
        /// <returns>Validator function using the custom predicate</returns>
        /// <example>
        /// <code>
        /// var validator = ValidationHelper.Custom(
        ///     value => value.StartsWith("BIB-"),
        ///     "Código deve começar com BIB-"
        /// );
        /// </code>
        /// </example>
        public static Func<string, (bool IsValid, string ErrorMessage)> Custom(
            Func<string, bool> predicate,
            string errorMessage)
        {
            return (value) =>
            {
                // Empty values pass unless predicate explicitly rejects them
                if (string.IsNullOrWhiteSpace(value))
                    return (true, string.Empty);

                try
                {
                    bool isValid = predicate(value);
                    return (isValid, isValid ? string.Empty : errorMessage);
                }
                catch (Exception ex)
                {
                    return (false, $"Erro na validação: {ex.Message}");
                }
            };
        }

        /// <summary>
        /// Combines multiple validators into a composite validator
        /// Returns the first error found, or success if all validators pass
        /// </summary>
        /// <param name="validators">Array of validator functions to combine</param>
        /// <returns>Composite validator that executes all validators in sequence</returns>
        /// <example>
        /// <code>
        /// var validator = ValidationHelper.Composite(
        ///     ValidationHelper.Required("CPF"),
        ///     ValidationHelper.CPF()
        /// );
        ///
        /// // This will first check if required, then check CPF format
        /// var (isValid, message) = validator("123.456.789-09");
        /// </code>
        /// </example>
        public static Func<string, (bool IsValid, string ErrorMessage)> Composite(
            params Func<string, (bool IsValid, string ErrorMessage)>[] validators)
        {
            return (value) =>
            {
                foreach (var validator in validators)
                {
                    var (isValid, errorMessage) = validator(value);
                    if (!isValid)
                    {
                        return (false, errorMessage);
                    }
                }
                return (true, string.Empty);
            };
        }

        #endregion

        #region Form Validation Utilities

        /// <summary>
        /// Validates multiple controls in a form
        /// </summary>
        /// <param name="validations">Array of tuples containing control and its validator</param>
        /// <returns>Dictionary mapping controls to their error messages (only invalid controls)</returns>
        /// <example>
        /// <code>
        /// var errors = ValidationHelper.ValidateForm(
        ///     (txtNome, ValidationHelper.Required("Nome")),
        ///     (txtEmail, ValidationHelper.Composite(
        ///         ValidationHelper.Required("E-mail"),
        ///         ValidationHelper.Email()
        ///     )),
        ///     (txtCPF, ValidationHelper.CPF())
        /// );
        ///
        /// if (errors.Count > 0)
        /// {
        ///     ValidationHelper.ShowValidationErrors(this, errors);
        ///     return;
        /// }
        /// </code>
        /// </example>
        public static Dictionary<Control, string> ValidateForm(
            params (Control control, Func<string, (bool IsValid, string ErrorMessage)> validator)[] validations)
        {
            var errors = new Dictionary<Control, string>();

            foreach (var (control, validator) in validations)
            {
                if (control == null || validator == null)
                    continue;

                string value = control.Text;
                var (isValid, errorMessage) = validator(value);

                if (!isValid)
                {
                    errors[control] = errorMessage;
                    HighlightInvalidControl(control);
                }
                else
                {
                    ClearControlHighlight(control);
                }
            }

            return errors;
        }

        /// <summary>
        /// Shows validation errors using MessageBox
        /// </summary>
        /// <param name="form">The form containing the controls</param>
        /// <param name="errors">Dictionary of controls and their error messages</param>
        /// <example>
        /// <code>
        /// var errors = ValidationHelper.ValidateForm(...);
        /// if (errors.Count > 0)
        /// {
        ///     ValidationHelper.ShowValidationErrors(this, errors);
        /// }
        /// </code>
        /// </example>
        public static void ShowValidationErrors(Form form, Dictionary<Control, string> errors)
        {
            if (errors == null || errors.Count == 0)
                return;

            // Build error message
            var errorMessages = errors.Select(kvp =>
            {
                string controlName = !string.IsNullOrEmpty(kvp.Key.Name)
                    ? kvp.Key.Name.Replace("txt", "").Replace("cmb", "").Replace("dtp", "")
                    : "Campo";
                return $"• {kvp.Value}";
            });

            string message = "Por favor, corrija os seguintes erros:\n\n" +
                           string.Join("\n", errorMessages);

            MessageBox.Show(
                message,
                "Erros de Validação",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            );

            // Focus first invalid control
            if (errors.Count > 0)
            {
                var firstInvalidControl = errors.Keys.First();
                firstInvalidControl.Focus();
            }
        }

        /// <summary>
        /// Clears all validation error indicators from a form
        /// </summary>
        /// <param name="form">The form to clear validation errors from</param>
        /// <example>
        /// <code>
        /// ValidationHelper.ClearValidationErrors(this);
        /// </code>
        /// </example>
        public static void ClearValidationErrors(Form form)
        {
            if (form == null)
                return;

            // Clear all control highlights
            foreach (Control control in GetAllControls(form))
            {
                ClearControlHighlight(control);
            }
        }

        /// <summary>
        /// Highlights a control with an error state (red border)
        /// </summary>
        /// <param name="control">The control to highlight</param>
        /// <example>
        /// <code>
        /// ValidationHelper.HighlightInvalidControl(txtNome);
        /// </code>
        /// </example>
        public static void HighlightInvalidControl(Control control)
        {
            if (control == null)
                return;

            // Store original border style in Tag if not already stored
            if (control.Tag == null || !(control.Tag is ControlValidationState))
            {
                control.Tag = new ControlValidationState
                {
                    OriginalBackColor = control.BackColor,
                    WasHighlighted = false
                };
            }

            var state = control.Tag as ControlValidationState;
            if (state != null && !state.WasHighlighted)
            {
                state.WasHighlighted = true;

                // Apply error styling based on control type
                if (control is TextBox textBox)
                {
                    textBox.BackColor = Color.FromArgb(255, 245, 245); // Light red background
                }
                else if (control is ComboBox comboBox)
                {
                    comboBox.BackColor = Color.FromArgb(255, 245, 245);
                }
                else if (control is DateTimePicker dateTimePicker)
                {
                    dateTimePicker.BackColor = Color.FromArgb(255, 245, 245);
                }
            }
        }

        /// <summary>
        /// Clears error highlighting from a control
        /// </summary>
        /// <param name="control">The control to clear highlighting from</param>
        private static void ClearControlHighlight(Control control)
        {
            if (control == null)
                return;

            if (control.Tag is ControlValidationState state && state.WasHighlighted)
            {
                // Restore original background color
                control.BackColor = state.OriginalBackColor;
                state.WasHighlighted = false;
            }
        }

        /// <summary>
        /// Gets all controls recursively from a container
        /// </summary>
        private static IEnumerable<Control> GetAllControls(Control container)
        {
            var controls = new List<Control>();

            foreach (Control control in container.Controls)
            {
                controls.Add(control);
                if (control.HasChildren)
                {
                    controls.AddRange(GetAllControls(control));
                }
            }

            return controls;
        }

        #endregion

        #region Helper Classes

        /// <summary>
        /// Internal class to store control validation state
        /// </summary>
        private class ControlValidationState
        {
            public Color OriginalBackColor { get; set; }
            public bool WasHighlighted { get; set; }
        }

        #endregion
    }
}
