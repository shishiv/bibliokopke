using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using BibliotecaJK.Model;
using BibliotecaJK.BLL;
using BibliotecaJK.DAL;
using BibliotecaJK.Components;
using BibliotecaJK.Helpers;

namespace BibliotecaJK.Forms
{
    /// <summary>
    /// Formulário de Cadastro de Alunos com Wizard Multi-Step
    /// Uses WizardControl for guided registration flow
    /// </summary>
    public partial class FormCadastroAluno : Form
    {
        private readonly Funcionario _funcionarioLogado;
        private readonly AlunoService _alunoService;
        private readonly AlunoDAL _alunoDAL;
        private int? _alunoEmEdicaoId;

        // Wizard control
        private WizardControl _wizard = null!;

        // Step 1: Dados Pessoais
        private Panel _step1Panel = null!;
        private ValidatedTextBox _txtNome = null!;
        private ValidatedTextBox _txtCPF = null!;
        private HelpIcon _helpCPF = null!;

        // Step 2: Contato
        private Panel _step2Panel = null!;
        private ValidatedTextBox _txtEmail = null!;
        private ModernTextBox _txtTelefone = null!;
        private HelpIcon _helpEmail = null!;

        // Step 3: Informações Acadêmicas
        private Panel _step3Panel = null!;
        private ValidatedTextBox _txtMatricula = null!;
        private ModernTextBox _txtTurma = null!;
        private HelpIcon _helpMatricula = null!;

        public FormCadastroAluno(Funcionario funcionario)
        {
            _funcionarioLogado = funcionario;
            _alunoService = new AlunoService();
            _alunoDAL = new AlunoDAL();

            InitializeComponent();

            // Restore form state
            FormStateManager.RestoreFormState(this);

            // Handle form closing to save state
            this.FormClosing += (s, e) => FormStateManager.SaveFormState(this);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // FormCadastroAluno
            this.Name = "FormCadastroAluno";
            this.ClientSize = new Size(900, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Cadastro de Aluno - Wizard";
            this.BackColor = ThemeManager.Background.LightDefault;
            this.MinimumSize = new Size(800, 600);

            // Create wizard steps
            CreateStep1_DadosPessoais();
            CreateStep2_Contato();
            CreateStep3_InformacoesAcademicas();

            // Initialize Wizard Control
            _wizard = new WizardControl
            {
                Dock = DockStyle.Fill
            };

            // Add steps to wizard
            _wizard.AddStep(
                "Dados Pessoais",
                "Informações básicas do aluno",
                _step1Panel,
                ValidateStep1
            );

            _wizard.AddStep(
                "Contato",
                "Informações de contato",
                _step2Panel,
                ValidateStep2
            );

            _wizard.AddStep(
                "Informações Acadêmicas",
                "Matrícula e dados escolares",
                _step3Panel,
                ValidateStep3
            );

            // Wire up wizard completion event
            _wizard.WizardCompleted += Wizard_Completed;

            // Add wizard to form
            this.Controls.Add(_wizard);

            this.ResumeLayout(false);

            // Set focus to first field
            this.Load += (s, e) => _txtNome.Focus();
        }

        /// <summary>
        /// Creates Step 1: Dados Pessoais (Personal Information)
        /// </summary>
        private void CreateStep1_DadosPessoais()
        {
            _step1Panel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(ThemeManager.Spacing.XL),
                BackColor = Color.Transparent
            };

            int yPos = 20;
            int labelWidth = 150;
            int controlWidth = 400;
            int spacing = 100;

            // Nome Completo (Required)
            var lblNome = new Label
            {
                Text = "Nome Completo *",
                Location = new Point(20, yPos),
                Size = new Size(labelWidth, 25),
                Font = ThemeManager.Typography.Body1,
                TextAlign = ContentAlignment.MiddleLeft
            };
            _step1Panel.Controls.Add(lblNome);

            _txtNome = new ValidatedTextBox
            {
                FloatingLabel = "Nome Completo",
                Location = new Point(labelWidth + 40, yPos - 5),
                Size = new Size(controlWidth, 80),
                MaxLength = 100,
                ValidationEnabled = true
            };
            _txtNome.SetRequiredValidator("Nome completo é obrigatório");
            _step1Panel.Controls.Add(_txtNome);

            yPos += spacing;

            // CPF (Required with validation)
            var lblCPF = new Label
            {
                Text = "CPF *",
                Location = new Point(20, yPos),
                Size = new Size(labelWidth, 25),
                Font = ThemeManager.Typography.Body1,
                TextAlign = ContentAlignment.MiddleLeft
            };
            _step1Panel.Controls.Add(lblCPF);

            _txtCPF = new ValidatedTextBox
            {
                FloatingLabel = "CPF",
                Location = new Point(labelWidth + 40, yPos - 5),
                Size = new Size(250, 80),
                MaxLength = 14,
                ValidationEnabled = true
            };
            _txtCPF.Validator = (text) =>
            {
                if (string.IsNullOrWhiteSpace(text))
                    return (false, "CPF é obrigatório");

                if (!Validadores.ValidarCPF(text))
                    return (false, "CPF inválido");

                return (true, string.Empty);
            };
            _txtCPF.TextChanged += (s, e) => FormatarCPF();
            _step1Panel.Controls.Add(_txtCPF);

            // Help icon for CPF
            _helpCPF = HelpIcon.Create(
                "Digite o CPF no formato XXX.XXX.XXX-XX ou apenas números. A validação é feita automaticamente.",
                "Ajuda - CPF"
            );
            _helpCPF.Location = new Point(labelWidth + 40 + 250 + 10, yPos);
            _step1Panel.Controls.Add(_helpCPF);

        }

        /// <summary>
        /// Creates Step 2: Contato (Contact Information)
        /// </summary>
        private void CreateStep2_Contato()
        {
            _step2Panel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(ThemeManager.Spacing.XL),
                BackColor = Color.Transparent
            };

            int yPos = 20;
            int labelWidth = 150;
            int controlWidth = 400;
            int spacing = 100;

            // Email (Optional but validated)
            var lblEmail = new Label
            {
                Text = "E-mail",
                Location = new Point(20, yPos),
                Size = new Size(labelWidth, 25),
                Font = ThemeManager.Typography.Body1,
                TextAlign = ContentAlignment.MiddleLeft
            };
            _step2Panel.Controls.Add(lblEmail);

            _txtEmail = new ValidatedTextBox
            {
                FloatingLabel = "E-mail",
                Location = new Point(labelWidth + 40, yPos - 5),
                Size = new Size(controlWidth, 80),
                MaxLength = 100,
                ValidationEnabled = true
            };
            _txtEmail.Validator = (text) =>
            {
                if (string.IsNullOrWhiteSpace(text))
                    return (true, string.Empty); // Optional field

                if (!Validadores.ValidarEmail(text))
                    return (false, "E-mail inválido");

                return (true, string.Empty);
            };
            _step2Panel.Controls.Add(_txtEmail);

            // Help icon for Email
            _helpEmail = HelpIcon.Create(
                "E-mail opcional para contato. Exemplo: aluno@escola.com.br",
                "Ajuda - E-mail"
            );
            _helpEmail.Location = new Point(labelWidth + 40 + controlWidth + 10, yPos);
            _step2Panel.Controls.Add(_helpEmail);

            yPos += spacing;

            // Telefone (Optional with mask)
            var lblTelefone = new Label
            {
                Text = "Telefone",
                Location = new Point(20, yPos),
                Size = new Size(labelWidth, 25),
                Font = ThemeManager.Typography.Body1,
                TextAlign = ContentAlignment.MiddleLeft
            };
            _step2Panel.Controls.Add(lblTelefone);

            _txtTelefone = new ModernTextBox
            {
                FloatingLabel = "Telefone",
                Location = new Point(labelWidth + 40, yPos - 5),
                Size = new Size(250, 56),
                MaxLength = 15
            };
            // Apply phone mask formatting on text changed
            _txtTelefone.TextChanged += (s, e) => FormatarTelefone();
            _step2Panel.Controls.Add(_txtTelefone);
        }

        /// <summary>
        /// Creates Step 3: Informações Acadêmicas (Academic Information)
        /// </summary>
        private void CreateStep3_InformacoesAcademicas()
        {
            _step3Panel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(ThemeManager.Spacing.XL),
                BackColor = Color.Transparent
            };

            int yPos = 20;
            int labelWidth = 150;
            int controlWidth = 400;
            int spacing = 100;

            // Matrícula (Required)
            var lblMatricula = new Label
            {
                Text = "Matrícula *",
                Location = new Point(20, yPos),
                Size = new Size(labelWidth, 25),
                Font = ThemeManager.Typography.Body1,
                TextAlign = ContentAlignment.MiddleLeft
            };
            _step3Panel.Controls.Add(lblMatricula);

            _txtMatricula = new ValidatedTextBox
            {
                FloatingLabel = "Matrícula",
                Location = new Point(labelWidth + 40, yPos - 5),
                Size = new Size(250, 80),
                MaxLength = 20,
                ValidationEnabled = true
            };
            _txtMatricula.Validator = (text) =>
            {
                if (string.IsNullOrWhiteSpace(text))
                    return (false, "Matrícula é obrigatória");

                if (!Validadores.ValidarMatricula(text))
                    return (false, "Matrícula inválida (3-20 caracteres alfanuméricos)");

                return (true, string.Empty);
            };
            _step3Panel.Controls.Add(_txtMatricula);

            // Help icon for Matrícula
            _helpMatricula = HelpIcon.Create(
                "Matrícula deve conter de 3 a 20 caracteres alfanuméricos. Exemplo: 2024001, ALU123",
                "Ajuda - Matrícula"
            );
            _helpMatricula.Location = new Point(labelWidth + 40 + 250 + 10, yPos);
            _step3Panel.Controls.Add(_helpMatricula);

            yPos += spacing;

            // Turma
            var lblTurma = new Label
            {
                Text = "Turma",
                Location = new Point(20, yPos),
                Size = new Size(labelWidth, 25),
                Font = ThemeManager.Typography.Body1,
                TextAlign = ContentAlignment.MiddleLeft
            };
            _step3Panel.Controls.Add(lblTurma);

            _txtTurma = new ModernTextBox
            {
                FloatingLabel = "Turma",
                Location = new Point(labelWidth + 40, yPos - 5),
                Size = new Size(controlWidth, 56),
                MaxLength = 50
            };
            _step3Panel.Controls.Add(_txtTurma);
        }

        /// <summary>
        /// Validates Step 1: Dados Pessoais
        /// </summary>
        private bool ValidateStep1()
        {
            // Trigger validation on all fields
            _txtNome.ValidateInput();
            _txtCPF.ValidateInput();

            // Check if all required fields are valid
            return _txtNome.IsValid && _txtCPF.IsValid;
        }

        /// <summary>
        /// Validates Step 2: Contato
        /// </summary>
        private bool ValidateStep2()
        {
            // Trigger validation on email
            _txtEmail.ValidateInput();

            // Email is optional but must be valid if provided
            return _txtEmail.IsValid;
        }

        /// <summary>
        /// Validates Step 3: Informações Acadêmicas
        /// </summary>
        private bool ValidateStep3()
        {
            // Trigger validation on matrícula
            _txtMatricula.ValidateInput();

            // Check if matrícula is valid
            return _txtMatricula.IsValid;
        }

        /// <summary>
        /// Handles wizard completion - saves the student data
        /// </summary>
        private async void Wizard_Completed(object? sender, EventArgs e)
        {
            try
            {
                // Collect data from all wizard steps
                var aluno = new Aluno
                {
                    // Step 1: Dados Pessoais
                    Nome = _txtNome.Text.Trim(),
                    CPF = _txtCPF.Text.Trim(),

                    // Step 2: Contato
                    Email = string.IsNullOrWhiteSpace(_txtEmail.Text) ? null : _txtEmail.Text.Trim(),
                    Telefone = string.IsNullOrWhiteSpace(_txtTelefone.Text) ? null : _txtTelefone.Text.Trim(),

                    // Step 3: Informações Acadêmicas
                    Matricula = _txtMatricula.Text.Trim(),
                    Turma = string.IsNullOrWhiteSpace(_txtTurma.Text) ? null : _txtTurma.Text.Trim()
                };

                // Use AsyncOperationHelper to save with loading indicator
                await AsyncOperationHelper.RunAsync(this, async () =>
                {
                    await Task.Run(() =>
                    {
                        ResultadoOperacao resultado;

                        if (_alunoEmEdicaoId.HasValue)
                        {
                            // Update existing student
                            aluno.Id = _alunoEmEdicaoId.Value;
                            resultado = _alunoService.AtualizarAluno(aluno, _funcionarioLogado.Id);
                        }
                        else
                        {
                            // Create new student
                            resultado = _alunoService.CadastrarAluno(aluno, _funcionarioLogado.Id);
                        }

                        if (!resultado.Sucesso)
                        {
                            throw new Exception(resultado.Mensagem);
                        }

                        // Invoke on UI thread to show success message
                        this.Invoke(new Action(() =>
                        {
                            ToastNotification.Success(resultado.Mensagem);
                        }));
                    });
                }, "Salvando aluno...");

                // Close the form on success
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                // Error is already shown by AsyncOperationHelper via ErrorDialog
                // Just log for debugging
                System.Diagnostics.Debug.WriteLine($"Error saving student: {ex.Message}");
            }
        }

        /// <summary>
        /// Formats CPF with mask (XXX.XXX.XXX-XX)
        /// </summary>
        private void FormatarCPF()
        {
            string cpf = new string(_txtCPF.Text.Where(char.IsDigit).ToArray());

            if (cpf.Length == 11)
            {
                // Don't trigger TextChanged event during formatting
                _txtCPF.TextChanged -= (s, e) => FormatarCPF();
                _txtCPF.Text = $"{cpf.Substring(0, 3)}.{cpf.Substring(3, 3)}.{cpf.Substring(6, 3)}-{cpf.Substring(9, 2)}";
                _txtCPF.TextChanged += (s, e) => FormatarCPF();
            }
        }

        /// <summary>
        /// Formats phone with mask (XX) XXXXX-XXXX
        /// </summary>
        private void FormatarTelefone()
        {
            string telefone = new string(_txtTelefone.Text.Where(char.IsDigit).ToArray());

            if (telefone.Length == 11)
            {
                // Don't trigger TextChanged event during formatting
                _txtTelefone.TextChanged -= (s, e) => FormatarTelefone();
                _txtTelefone.Text = $"({telefone.Substring(0, 2)}) {telefone.Substring(2, 5)}-{telefone.Substring(7, 4)}";
                _txtTelefone.TextChanged += (s, e) => FormatarTelefone();
            }
            else if (telefone.Length == 10)
            {
                // Landline format
                _txtTelefone.TextChanged -= (s, e) => FormatarTelefone();
                _txtTelefone.Text = $"({telefone.Substring(0, 2)}) {telefone.Substring(2, 4)}-{telefone.Substring(6, 4)}";
                _txtTelefone.TextChanged += (s, e) => FormatarTelefone();
            }
        }

        /// <summary>
        /// Clears all wizard fields and resets to step 1
        /// </summary>
        private void LimparCampos()
        {
            // Step 1
            _txtNome.Clear();
            _txtCPF.Clear();

            // Step 2
            _txtEmail.Clear();
            _txtTelefone.Clear();

            // Step 3
            _txtMatricula.Clear();
            _txtTurma.Clear();

            // Reset wizard to first step
            _wizard.GoToStep(0);

            // Clear editing state
            _alunoEmEdicaoId = null;
        }

        /// <summary>
        /// Disposes resources
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _wizard?.Dispose();
                _txtNome?.Dispose();
                _txtCPF?.Dispose();
                _txtEmail?.Dispose();
                _txtTelefone?.Dispose();
                _txtMatricula?.Dispose();
                _txtTurma?.Dispose();
                _helpCPF?.Dispose();
                _helpEmail?.Dispose();
                _helpMatricula?.Dispose();
                _step1Panel?.Dispose();
                _step2Panel?.Dispose();
                _step3Panel?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
