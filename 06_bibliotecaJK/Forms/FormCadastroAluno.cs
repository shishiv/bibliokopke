using System;
using System.Linq;
using System.Windows.Forms;
using BibliotecaJK.Model;
using BibliotecaJK.BLL;
using BibliotecaJK.DAL;

namespace BibliotecaJK.Forms
{
    /// <summary>
    /// Formulário de Cadastro de Alunos (CRUD Completo)
    /// </summary>
    public partial class FormCadastroAluno : Form
    {
        private readonly Funcionario _funcionarioLogado;
        private readonly AlunoService _alunoService;
        private readonly AlunoDAL _alunoDAL;
        private int? _alunoEmEdicaoId;

        public FormCadastroAluno(Funcionario funcionario)
        {
            _funcionarioLogado = funcionario;
            _alunoService = new AlunoService();
            _alunoDAL = new AlunoDAL();

            InitializeComponent();
            CarregarGrid();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // FormCadastroAluno
            this.ClientSize = new System.Drawing.Size(1000, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Cadastro de Alunos";
            this.BackColor = System.Drawing.Color.WhiteSmoke;

            // Título
            var lblTitulo = new Label
            {
                Text = "CADASTRO DE ALUNOS",
                Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.DarkSlateBlue,
                Location = new System.Drawing.Point(20, 15),
                Size = new System.Drawing.Size(960, 30)
            };
            this.Controls.Add(lblTitulo);

            // Panel de Formulário
            var pnlForm = new Panel
            {
                Location = new System.Drawing.Point(20, 60),
                Size = new System.Drawing.Size(960, 200),
                BackColor = System.Drawing.Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Nome
            pnlForm.Controls.Add(new Label
            {
                Text = "Nome Completo *",
                Location = new System.Drawing.Point(20, 20),
                Size = new System.Drawing.Size(120, 20)
            });
            txtNome = new TextBox
            {
                Location = new System.Drawing.Point(150, 18),
                Size = new System.Drawing.Size(350, 25),
                MaxLength = 100
            };
            pnlForm.Controls.Add(txtNome);

            // CPF
            pnlForm.Controls.Add(new Label
            {
                Text = "CPF *",
                Location = new System.Drawing.Point(520, 20),
                Size = new System.Drawing.Size(80, 20)
            });
            txtCPF = new TextBox
            {
                Location = new System.Drawing.Point(610, 18),
                Size = new System.Drawing.Size(150, 25),
                MaxLength = 14
            };
            txtCPF.Leave += (s, e) => FormatarCPF();
            pnlForm.Controls.Add(txtCPF);

            // Matrícula
            pnlForm.Controls.Add(new Label
            {
                Text = "Matrícula *",
                Location = new System.Drawing.Point(20, 60),
                Size = new System.Drawing.Size(120, 20)
            });
            txtMatricula = new TextBox
            {
                Location = new System.Drawing.Point(150, 58),
                Size = new System.Drawing.Size(150, 25),
                MaxLength = 20
            };
            pnlForm.Controls.Add(txtMatricula);

            // Turma
            pnlForm.Controls.Add(new Label
            {
                Text = "Turma",
                Location = new System.Drawing.Point(320, 60),
                Size = new System.Drawing.Size(80, 20)
            });
            txtTurma = new TextBox
            {
                Location = new System.Drawing.Point(410, 58),
                Size = new System.Drawing.Size(90, 25),
                MaxLength = 50
            };
            pnlForm.Controls.Add(txtTurma);

            // Telefone
            pnlForm.Controls.Add(new Label
            {
                Text = "Telefone",
                Location = new System.Drawing.Point(520, 60),
                Size = new System.Drawing.Size(80, 20)
            });
            txtTelefone = new TextBox
            {
                Location = new System.Drawing.Point(610, 58),
                Size = new System.Drawing.Size(150, 25),
                MaxLength = 15
            };
            pnlForm.Controls.Add(txtTelefone);

            // Email
            pnlForm.Controls.Add(new Label
            {
                Text = "E-mail",
                Location = new System.Drawing.Point(20, 100),
                Size = new System.Drawing.Size(120, 20)
            });
            txtEmail = new TextBox
            {
                Location = new System.Drawing.Point(150, 98),
                Size = new System.Drawing.Size(350, 25),
                MaxLength = 100
            };
            pnlForm.Controls.Add(txtEmail);

            // Botões
            btnNovo = new Button
            {
                Text = "Novo",
                Location = new System.Drawing.Point(150, 145),
                Size = new System.Drawing.Size(100, 35),
                BackColor = System.Drawing.Color.MediumSeaGreen,
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnNovo.FlatAppearance.BorderSize = 0;
            btnNovo.Click += BtnNovo_Click;
            pnlForm.Controls.Add(btnNovo);

            btnSalvar = new Button
            {
                Text = "Salvar",
                Location = new System.Drawing.Point(260, 145),
                Size = new System.Drawing.Size(100, 35),
                BackColor = System.Drawing.Color.DarkSlateBlue,
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnSalvar.FlatAppearance.BorderSize = 0;
            btnSalvar.Click += BtnSalvar_Click;
            pnlForm.Controls.Add(btnSalvar);

            btnCancelar = new Button
            {
                Text = "Cancelar",
                Location = new System.Drawing.Point(370, 145),
                Size = new System.Drawing.Size(100, 35),
                BackColor = System.Drawing.Color.Gray,
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Enabled = false
            };
            btnCancelar.FlatAppearance.BorderSize = 0;
            btnCancelar.Click += BtnCancelar_Click;
            pnlForm.Controls.Add(btnCancelar);

            this.Controls.Add(pnlForm);

            // Grid de Alunos
            dgvAlunos = new DataGridView
            {
                Location = new System.Drawing.Point(20, 280),
                Size = new System.Drawing.Size(960, 320),
                BackgroundColor = System.Drawing.Color.White,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            dgvAlunos.CellDoubleClick += DgvAlunos_CellDoubleClick;

            this.Controls.Add(dgvAlunos);

            // Botões de Ação
            var btnEditar = new Button
            {
                Text = "Editar",
                Location = new System.Drawing.Point(710, 610),
                Size = new System.Drawing.Size(90, 30),
                BackColor = System.Drawing.Color.SteelBlue,
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnEditar.FlatAppearance.BorderSize = 0;
            btnEditar.Click += BtnEditar_Click;
            this.Controls.Add(btnEditar);

            var btnExcluir = new Button
            {
                Text = "Excluir",
                Location = new System.Drawing.Point(810, 610),
                Size = new System.Drawing.Size(90, 30),
                BackColor = System.Drawing.Color.Crimson,
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnExcluir.FlatAppearance.BorderSize = 0;
            btnExcluir.Click += BtnExcluir_Click;
            this.Controls.Add(btnExcluir);

            var btnFechar = new Button
            {
                Text = "Fechar",
                Location = new System.Drawing.Point(910, 610),
                Size = new System.Drawing.Size(70, 30),
                BackColor = System.Drawing.Color.Gray,
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnFechar.FlatAppearance.BorderSize = 0;
            btnFechar.Click += (s, e) => this.Close();
            this.Controls.Add(btnFechar);

            this.ResumeLayout(false);
        }

        private TextBox txtNome = new TextBox();
        private TextBox txtCPF = new TextBox();
        private TextBox txtMatricula = new TextBox();
        private TextBox txtTurma = new TextBox();
        private TextBox txtTelefone = new TextBox();
        private TextBox txtEmail = new TextBox();
        private Button btnNovo = new Button();
        private Button btnSalvar = new Button();
        private Button btnCancelar = new Button();
        private DataGridView dgvAlunos = new DataGridView();

        private void CarregarGrid()
        {
            try
            {
                var alunos = _alunoDAL.Listar();

                dgvAlunos.DataSource = alunos.Select(a => new
                {
                    a.Id,
                    a.Nome,
                    a.CPF,
                    a.Matricula,
                    a.Turma,
                    a.Telefone,
                    a.Email
                }).ToList();

                if (dgvAlunos.Columns.Count > 0)
                {
                    dgvAlunos.Columns["Id"].Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar alunos: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormatarCPF()
        {
            string cpf = new string(txtCPF.Text.Where(char.IsDigit).ToArray());
            if (cpf.Length == 11)
            {
                txtCPF.Text = $"{cpf.Substring(0, 3)}.{cpf.Substring(3, 3)}.{cpf.Substring(6, 3)}-{cpf.Substring(9, 2)}";
            }
        }

        private void BtnNovo_Click(object? sender, EventArgs e)
        {
            LimparCampos();
            _alunoEmEdicaoId = null;
            btnCancelar.Enabled = true;
            txtNome.Focus();
        }

        private void BtnSalvar_Click(object? sender, EventArgs e)
        {
            try
            {
                var aluno = new Aluno
                {
                    Nome = txtNome.Text.Trim(),
                    CPF = txtCPF.Text.Trim(),
                    Matricula = txtMatricula.Text.Trim(),
                    Turma = string.IsNullOrWhiteSpace(txtTurma.Text) ? null : txtTurma.Text.Trim(),
                    Telefone = string.IsNullOrWhiteSpace(txtTelefone.Text) ? null : txtTelefone.Text.Trim(),
                    Email = string.IsNullOrWhiteSpace(txtEmail.Text) ? null : txtEmail.Text.Trim()
                };

                ResultadoOperacao resultado;

                if (_alunoEmEdicaoId.HasValue)
                {
                    // Atualização
                    aluno.Id = _alunoEmEdicaoId.Value;
                    resultado = _alunoService.AtualizarAluno(aluno, _funcionarioLogado.Id);
                }
                else
                {
                    // Novo cadastro
                    resultado = _alunoService.CadastrarAluno(aluno, _funcionarioLogado.Id);
                }

                if (resultado.Sucesso)
                {
                    MessageBox.Show(resultado.Mensagem, "Sucesso",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimparCampos();
                    CarregarGrid();
                    _alunoEmEdicaoId = null;
                    btnCancelar.Enabled = false;
                }
                else
                {
                    MessageBox.Show(resultado.Mensagem, "Atenção",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao salvar aluno: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancelar_Click(object? sender, EventArgs e)
        {
            LimparCampos();
            _alunoEmEdicaoId = null;
            btnCancelar.Enabled = false;
        }

        private void BtnEditar_Click(object? sender, EventArgs e)
        {
            if (dgvAlunos.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecione um aluno para editar.", "Atenção",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int alunoId = Convert.ToInt32(dgvAlunos.SelectedRows[0].Cells["Id"].Value);
            var aluno = _alunoDAL.ObterPorId(alunoId);

            if (aluno != null)
            {
                txtNome.Text = aluno.Nome;
                txtCPF.Text = aluno.CPF;
                txtMatricula.Text = aluno.Matricula;
                txtTurma.Text = aluno.Turma ?? "";
                txtTelefone.Text = aluno.Telefone ?? "";
                txtEmail.Text = aluno.Email ?? "";

                _alunoEmEdicaoId = aluno.Id;
                btnCancelar.Enabled = true;
                txtNome.Focus();
            }
        }

        private void DgvAlunos_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                BtnEditar_Click(sender, EventArgs.Empty);
            }
        }

        private void BtnExcluir_Click(object? sender, EventArgs e)
        {
            if (dgvAlunos.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecione um aluno para excluir.", "Atenção",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int alunoId = Convert.ToInt32(dgvAlunos.SelectedRows[0].Cells["Id"].Value);
            string nomeAluno = dgvAlunos.SelectedRows[0].Cells["Nome"].Value.ToString() ?? "";

            var confirmacao = MessageBox.Show(
                $"Deseja realmente excluir o aluno '{nomeAluno}'?",
                "Confirmação",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmacao == DialogResult.Yes)
            {
                var resultado = _alunoService.ExcluirAluno(alunoId, _funcionarioLogado.Id);

                if (resultado.Sucesso)
                {
                    MessageBox.Show(resultado.Mensagem, "Sucesso",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimparCampos();
                    CarregarGrid();
                }
                else
                {
                    MessageBox.Show(resultado.Mensagem, "Atenção",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void LimparCampos()
        {
            txtNome.Clear();
            txtCPF.Clear();
            txtMatricula.Clear();
            txtTurma.Clear();
            txtTelefone.Clear();
            txtEmail.Clear();
        }
    }
}
