using System;
using System.Windows.Forms;
using BibliotecaJK.Components;
using BibliotecaJK.Helpers;

namespace BibliotecaJK.Examples
{
    /// <summary>
    /// Example usage patterns for ValidationHelper
    /// This file demonstrates best practices for using validation helpers in forms
    /// </summary>
    /// <remarks>
    /// NOTE: This is an example/documentation file. Remove or exclude from build in production.
    /// </remarks>
    public class ValidationHelperExamples
    {
        #region Example 1: Basic Required Field Validation

        /// <summary>
        /// Example: Simple required field validation with ValidatedTextBox
        /// </summary>
        public static void Example1_RequiredField()
        {
            var txtNome = new ValidatedTextBox
            {
                FloatingLabel = "Nome do Aluno",
                Validator = ValidationHelper.Required("Nome"),
                ValidationEnabled = true
            };

            // The ValidatedTextBox will automatically show error when empty
            // and show success checkmark when filled
        }

        #endregion

        #region Example 2: Composite Validation (Multiple Rules)

        /// <summary>
        /// Example: Combining multiple validation rules
        /// </summary>
        public static void Example2_CompositeValidation()
        {
            // CPF field: required AND valid format
            var txtCPF = new ValidatedTextBox
            {
                FloatingLabel = "CPF",
                Validator = ValidationHelper.Composite(
                    ValidationHelper.Required("CPF"),
                    ValidationHelper.CPF()
                ),
                ValidationEnabled = true
            };

            // Email field: required AND valid email format
            var txtEmail = new ValidatedTextBox
            {
                FloatingLabel = "E-mail",
                Validator = ValidationHelper.Composite(
                    ValidationHelper.Required("E-mail"),
                    ValidationHelper.Email()
                ),
                ValidationEnabled = true
            };

            // Password field: required AND minimum length
            var txtSenha = new ValidatedTextBox
            {
                FloatingLabel = "Senha",
                Validator = ValidationHelper.Composite(
                    ValidationHelper.Required("Senha"),
                    ValidationHelper.MinLength(6, "Senha")
                ),
                ValidationEnabled = true,
                PasswordChar = '•'
            };
        }

        #endregion

        #region Example 3: ISBN Validation

        /// <summary>
        /// Example: ISBN validation for book registration
        /// </summary>
        public static void Example3_ISBNValidation()
        {
            var txtISBN = new ValidatedTextBox
            {
                FloatingLabel = "ISBN",
                Validator = ValidationHelper.Composite(
                    ValidationHelper.Required("ISBN"),
                    ValidationHelper.ISBN()
                ),
                ValidationEnabled = true,
                Hint = "ISBN-10 ou ISBN-13"
            };

            // Accepts both formats:
            // ISBN-10: 0-306-40615-2
            // ISBN-13: 978-3-16-148410-0
        }

        #endregion

        #region Example 4: Numeric Range Validation

        /// <summary>
        /// Example: Validating numeric input within a range
        /// </summary>
        public static void Example4_NumericRangeValidation()
        {
            // Quantity field: must be between 1 and 999
            var txtQuantidade = new ValidatedTextBox
            {
                FloatingLabel = "Quantidade",
                Validator = ValidationHelper.Composite(
                    ValidationHelper.Required("Quantidade"),
                    ValidationHelper.IntegerRange(1, 999, "Quantidade")
                ),
                ValidationEnabled = true
            };

            // Price field: must be between 0.01 and 9999.99
            var txtPreco = new ValidatedTextBox
            {
                FloatingLabel = "Preço",
                Validator = ValidationHelper.Composite(
                    ValidationHelper.Required("Preço"),
                    ValidationHelper.DecimalRange(0.01m, 9999.99m, "Preço")
                ),
                ValidationEnabled = true,
                Hint = "Ex: 29.90"
            };
        }

        #endregion

        #region Example 5: Custom Pattern Validation

        /// <summary>
        /// Example: Custom regex pattern validation
        /// </summary>
        public static void Example5_PatternValidation()
        {
            // Phone number validation (Brazilian format)
            var txtTelefone = new ValidatedTextBox
            {
                FloatingLabel = "Telefone",
                Validator = ValidationHelper.Composite(
                    ValidationHelper.Required("Telefone"),
                    ValidationHelper.Pattern(
                        @"^\(\d{2}\)\s?\d{4,5}-\d{4}$",
                        "Telefone",
                        "Telefone deve estar no formato (99) 99999-9999"
                    )
                ),
                ValidationEnabled = true,
                Hint = "(99) 99999-9999"
            };

            // CEP validation
            var txtCEP = new ValidatedTextBox
            {
                FloatingLabel = "CEP",
                Validator = ValidationHelper.Pattern(
                    @"^\d{5}-\d{3}$",
                    "CEP",
                    "CEP deve estar no formato 00000-000"
                ),
                ValidationEnabled = true,
                Hint = "00000-000"
            };
        }

        #endregion

        #region Example 6: Date Range Validation

        /// <summary>
        /// Example: Date validation with min/max range
        /// </summary>
        public static void Example6_DateRangeValidation()
        {
            // Birth date: must be between 1900 and today
            var txtDataNascimento = new ValidatedTextBox
            {
                FloatingLabel = "Data de Nascimento",
                Validator = ValidationHelper.Composite(
                    ValidationHelper.Required("Data de Nascimento"),
                    ValidationHelper.DateRange(
                        new DateTime(1900, 1, 1),
                        DateTime.Today,
                        "Data de Nascimento"
                    )
                ),
                ValidationEnabled = true,
                Hint = "dd/MM/yyyy"
            };
        }

        #endregion

        #region Example 7: Custom Validation Logic

        /// <summary>
        /// Example: Custom validation with business rules
        /// </summary>
        public static void Example7_CustomValidation()
        {
            // Book code: must start with "LIV-"
            var txtCodigoLivro = new ValidatedTextBox
            {
                FloatingLabel = "Código do Livro",
                Validator = ValidationHelper.Composite(
                    ValidationHelper.Required("Código"),
                    ValidationHelper.Custom(
                        value => value.StartsWith("LIV-"),
                        "Código deve começar com 'LIV-'"
                    ),
                    ValidationHelper.MinLength(8, "Código")
                ),
                ValidationEnabled = true,
                Hint = "LIV-0001"
            };

            // Student enrollment: custom format validation
            var txtMatricula = new ValidatedTextBox
            {
                FloatingLabel = "Matrícula",
                Validator = ValidationHelper.Composite(
                    ValidationHelper.Required("Matrícula"),
                    ValidationHelper.Matricula()
                ),
                ValidationEnabled = true
            };
        }

        #endregion

        #region Example 8: Form-Level Validation (Standard Controls)

        /// <summary>
        /// Example: Validating entire form with standard controls
        /// </summary>
        public class ExampleForm : Form
        {
            private TextBox txtNome = new TextBox();
            private TextBox txtEmail = new TextBox();
            private TextBox txtCPF = new TextBox();
            private Button btnSalvar = new Button();

            public ExampleForm()
            {
                btnSalvar.Click += BtnSalvar_Click;
            }

            private void BtnSalvar_Click(object? sender, EventArgs e)
            {
                // Validate all fields at once
                var errors = ValidationHelper.ValidateForm(
                    (txtNome, ValidationHelper.Required("Nome")),
                    (txtEmail, ValidationHelper.Composite(
                        ValidationHelper.Required("E-mail"),
                        ValidationHelper.Email()
                    )),
                    (txtCPF, ValidationHelper.Composite(
                        ValidationHelper.Required("CPF"),
                        ValidationHelper.CPF()
                    ))
                );

                // Check if there are any errors
                if (errors.Count > 0)
                {
                    ValidationHelper.ShowValidationErrors(this, errors);
                    return; // Don't proceed with save
                }

                // All validations passed - proceed with save
                SaveData();
            }

            private void SaveData()
            {
                // Implementation here
                MessageBox.Show("Dados salvos com sucesso!", "Sucesso");
            }

            private void LimparFormulario()
            {
                // Clear all validation errors
                ValidationHelper.ClearValidationErrors(this);

                // Clear field values
                txtNome.Clear();
                txtEmail.Clear();
                txtCPF.Clear();
            }
        }

        #endregion

        #region Example 9: Optional Field Validation

        /// <summary>
        /// Example: Optional fields that validate format when filled
        /// </summary>
        public static void Example9_OptionalFieldValidation()
        {
            // Email is optional, but if filled, must be valid
            // Notice: NO Required() validator
            var txtEmailOpcional = new ValidatedTextBox
            {
                FloatingLabel = "E-mail (opcional)",
                Validator = ValidationHelper.Email(),
                ValidationEnabled = true
            };

            // Phone is optional, but if filled, must match pattern
            var txtTelefoneOpcional = new ValidatedTextBox
            {
                FloatingLabel = "Telefone (opcional)",
                Validator = ValidationHelper.Pattern(
                    @"^\(\d{2}\)\s?\d{4,5}-\d{4}$",
                    "Telefone",
                    "Telefone deve estar no formato (99) 99999-9999"
                ),
                ValidationEnabled = true
            };
        }

        #endregion

        #region Example 10: Dynamic Validation State

        /// <summary>
        /// Example: Enabling/disabling validation dynamically
        /// </summary>
        public static void Example10_DynamicValidation()
        {
            var chkCadastrarEmail = new CheckBox { Text = "Cadastrar e-mail?" };
            var txtEmail = new ValidatedTextBox
            {
                FloatingLabel = "E-mail",
                Validator = ValidationHelper.Composite(
                    ValidationHelper.Required("E-mail"),
                    ValidationHelper.Email()
                ),
                ValidationEnabled = false, // Start disabled
                Enabled = false
            };

            // Enable validation when checkbox is checked
            chkCadastrarEmail.CheckedChanged += (s, e) =>
            {
                bool cadastrar = chkCadastrarEmail.Checked;
                txtEmail.Enabled = cadastrar;
                txtEmail.ValidationEnabled = cadastrar;

                if (!cadastrar)
                {
                    txtEmail.Clear();
                }
            };
        }

        #endregion

        #region Example 11: Validate on Blur Only

        /// <summary>
        /// Example: Validating only when user leaves the field (better UX for some fields)
        /// </summary>
        public static void Example11_ValidateOnBlurOnly()
        {
            // Only show errors after user finishes typing and moves to next field
            var txtNome = new ValidatedTextBox
            {
                FloatingLabel = "Nome Completo",
                Validator = ValidationHelper.Composite(
                    ValidationHelper.Required("Nome"),
                    ValidationHelper.MinLength(3, "Nome")
                ),
                ValidationEnabled = true,
                ValidateOnBlurOnly = true // Only validate on blur, not on every keystroke
            };

            // Good for long inputs where real-time validation is distracting
            var txtDescricao = new ValidatedTextBox
            {
                FloatingLabel = "Descrição",
                Validator = ValidationHelper.Composite(
                    ValidationHelper.Required("Descrição"),
                    ValidationHelper.MinLength(10, "Descrição"),
                    ValidationHelper.MaxLength(500, "Descrição")
                ),
                ValidationEnabled = true,
                ValidateOnBlurOnly = true,
                Multiline = true
            };
        }

        #endregion

        #region Example 12: Integration with Business Rules

        /// <summary>
        /// Example: Combining ValidationHelper with business logic
        /// </summary>
        public class FormCadastroAluno : Form
        {
            private ValidatedTextBox txtNome = new ValidatedTextBox();
            private ValidatedTextBox txtCPF = new ValidatedTextBox();
            private ValidatedTextBox txtMatricula = new ValidatedTextBox();
            private ValidatedTextBox txtEmail = new ValidatedTextBox();
            private ValidatedTextBox txtTurma = new ValidatedTextBox();

            public FormCadastroAluno()
            {
                ConfigureValidators();
            }

            private void ConfigureValidators()
            {
                // Nome: required, 3-100 characters
                txtNome.Validator = ValidationHelper.Composite(
                    ValidationHelper.Required("Nome"),
                    ValidationHelper.LengthRange(3, 100, "Nome")
                );
                txtNome.ValidationEnabled = true;

                // CPF: required, valid format
                txtCPF.Validator = ValidationHelper.Composite(
                    ValidationHelper.Required("CPF"),
                    ValidationHelper.CPF()
                );
                txtCPF.ValidationEnabled = true;

                // Matrícula: required, valid format (3-20 alphanumeric)
                txtMatricula.Validator = ValidationHelper.Composite(
                    ValidationHelper.Required("Matrícula"),
                    ValidationHelper.Matricula()
                );
                txtMatricula.ValidationEnabled = true;

                // Email: required, valid format
                txtEmail.Validator = ValidationHelper.Composite(
                    ValidationHelper.Required("E-mail"),
                    ValidationHelper.Email()
                );
                txtEmail.ValidationEnabled = true;

                // Turma: required, custom format (e.g., "3A", "9B")
                txtTurma.Validator = ValidationHelper.Composite(
                    ValidationHelper.Required("Turma"),
                    ValidationHelper.Pattern(
                        @"^[1-9][A-Z]$",
                        "Turma",
                        "Turma deve estar no formato 1A, 2B, etc."
                    )
                );
                txtTurma.ValidationEnabled = true;
            }

            private bool ValidarFormulario()
            {
                // Check all ValidatedTextBox controls
                var controls = new[] { txtNome, txtCPF, txtMatricula, txtEmail, txtTurma };
                bool allValid = true;

                foreach (var control in controls)
                {
                    control.ValidateInput();
                    if (!control.IsValid)
                    {
                        allValid = false;
                    }
                }

                if (!allValid)
                {
                    MessageBox.Show(
                        "Por favor, corrija os erros nos campos destacados.",
                        "Validação",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                }

                return allValid;
            }
        }

        #endregion

        #region Example 13: Reusable Validator Factory

        /// <summary>
        /// Example: Creating reusable validator configurations
        /// </summary>
        public static class CommonValidators
        {
            public static Func<string, (bool, string)> NomePessoa =>
                ValidationHelper.Composite(
                    ValidationHelper.Required("Nome"),
                    ValidationHelper.LengthRange(3, 100, "Nome"),
                    ValidationHelper.Pattern(
                        @"^[a-zA-ZÀ-ÿ\s]+$",
                        "Nome",
                        "Nome deve conter apenas letras"
                    )
                );

            public static Func<string, (bool, string)> CPFCompleto =>
                ValidationHelper.Composite(
                    ValidationHelper.Required("CPF"),
                    ValidationHelper.CPF()
                );

            public static Func<string, (bool, string)> EmailCompleto =>
                ValidationHelper.Composite(
                    ValidationHelper.Required("E-mail"),
                    ValidationHelper.Email()
                );

            public static Func<string, (bool, string)> SenhaForte =>
                ValidationHelper.Composite(
                    ValidationHelper.Required("Senha"),
                    ValidationHelper.MinLength(8, "Senha", "Senha deve ter no mínimo 8 caracteres"),
                    ValidationHelper.Custom(
                        value => value.Any(char.IsUpper) && value.Any(char.IsLower) && value.Any(char.IsDigit),
                        "Senha deve conter maiúsculas, minúsculas e números"
                    )
                );

            public static Func<string, (bool, string)> TelefoneOpcional =>
                ValidationHelper.Pattern(
                    @"^\(\d{2}\)\s?\d{4,5}-\d{4}$",
                    "Telefone",
                    "Formato: (99) 99999-9999"
                );
        }

        // Usage:
        public static void Example13_UseCommonValidators()
        {
            var txtNome = new ValidatedTextBox
            {
                FloatingLabel = "Nome",
                Validator = CommonValidators.NomePessoa,
                ValidationEnabled = true
            };

            var txtCPF = new ValidatedTextBox
            {
                FloatingLabel = "CPF",
                Validator = CommonValidators.CPFCompleto,
                ValidationEnabled = true
            };
        }

        #endregion
    }
}
