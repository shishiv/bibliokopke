using BibliotecaJK.Utilitarios;
using BibliotecaJK.Modelos;
using BibliotecaJK.Servicos;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace BibliotecaJK
{
    public partial class Reservas : Form
    {
        private readonly Funcionario _usuarioLogado;
        private readonly ServicoReserva _servicoReserva = new();
        private readonly ServicoLivro _servicoLivro = new();
        private readonly BindingSource _bindingSource = new();
        private Reserva? _selecionada;

        public Reservas(Funcionario usuarioLogado)
        {
            _usuarioLogado = usuarioLogado;
            InitializeComponent();
            txt_tituloautor.ReadOnly = true;
            ConfigurarGrid();
            ConfigurarPlaceholders();
            RegistrarEventos();
        }

        private void ConfigurarPlaceholders()
        {
            // Placeholder para Matrícula
            txt_matriculaAluno.Text = "Digite a matrícula do aluno...";
            txt_matriculaAluno.ForeColor = Color.Gray;
            txt_matriculaAluno.Enter += (s, e) =>
            {
                if (txt_matriculaAluno.Text == "Digite a matrícula do aluno...")
                {
                    txt_matriculaAluno.Text = "";
                    txt_matriculaAluno.ForeColor = Color.Black;
                }
            };
            txt_matriculaAluno.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txt_matriculaAluno.Text))
                {
                    txt_matriculaAluno.Text = "Digite a matrícula do aluno...";
                    txt_matriculaAluno.ForeColor = Color.Gray;
                }
            };

            // Placeholder para ISBN/Código
            txt_codigolivro.Text = "Digite o ISBN ou código do livro...";
            txt_codigolivro.ForeColor = Color.Gray;
            txt_codigolivro.Enter += (s, e) =>
            {
                if (txt_codigolivro.Text == "Digite o ISBN ou código do livro...")
                {
                    txt_codigolivro.Text = "";
                    txt_codigolivro.ForeColor = Color.Black;
                }
            };
            txt_codigolivro.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txt_codigolivro.Text))
                {
                    txt_codigolivro.Text = "Digite o ISBN ou código do livro...";
                    txt_codigolivro.ForeColor = Color.Gray;
                }
                else
                {
                    // Quando sai do campo, busca informações do livro
                    ExibirResumoLivro(txt_codigolivro.Text.Trim());
                }
            };
        }

        private void RegistrarEventos()
        {
            btn_confirmarReserva.Click += btn_confirmarReserva_Click;
            btn_cancelarReserva.Click += btn_cancelarReserva_Click;
            btn_carregarReservas.Click += (_, _) => CarregarReservas();
            btn_voltar.Click += (_, _) => Close();
            dgv_reservas.SelectionChanged += dgv_reservas_SelectionChanged;
        }

        private void ConfigurarGrid()
        {
            dgv_reservas.AutoGenerateColumns = false;
            dgv_reservas.BackgroundColor = SystemColors.ControlLight;
            dgv_reservas.Columns.Clear();
            dgv_reservas.Columns.AddRange(new DataGridViewColumn[]
            {
                new DataGridViewTextBoxColumn { DataPropertyName = nameof(Reserva.Id), Visible = false },
                new DataGridViewTextBoxColumn { DataPropertyName = nameof(Reserva.IdLivro), HeaderText = "Livro", Width = 80 },
                new DataGridViewTextBoxColumn { DataPropertyName = nameof(Reserva.DataReserva), HeaderText = "Data", Width = 120, DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" } },
                new DataGridViewTextBoxColumn { DataPropertyName = nameof(Reserva.Status), HeaderText = "Status", Width = 100 }
            });

            dgv_reservas.DataSource = _bindingSource;
        }

        private void btn_confirmarReserva_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_matriculaAluno.Text) || txt_matriculaAluno.Text == "Digite a matrícula do aluno...")
            {
                MessageBox.Show("Campo obrigatório: Matrícula", "Reservas", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_matriculaAluno.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txt_codigolivro.Text) || txt_codigolivro.Text == "Digite o ISBN ou código do livro...")
            {
                MessageBox.Show("Campo obrigatório: Código do Livro", "Reservas", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_codigolivro.Focus();
                return;
            }

            try
            {
                var reserva = _servicoReserva.CriarReserva(
                    txt_matriculaAluno.Text.Trim(),
                    txt_codigolivro.Text.Trim(),
                    dtp_dataParaReserva.Value,
                    _usuarioLogado.Id);

                MessageBox.Show("Reserva criada com sucesso!", "Reservas", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ExibirResumoLivro(txt_codigolivro.Text.Trim());
                CarregarReservas();
            }
            catch (ExcecaoValidacao ex)
            {
                MessageBox.Show(ex.Message, "Reservas", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao criar reserva: {ex.Message}", "Reservas", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_cancelarReserva_Click(object? sender, EventArgs e)
        {
            if (_selecionada == null)
            {
                MessageBox.Show("Selecione uma reserva.", "Reservas", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirmacao = MessageBox.Show("Deseja cancelar a reserva selecionada?", "Reservas", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmacao != DialogResult.Yes)
            {
                return;
            }

            try
            {
                _servicoReserva.Cancelar(_selecionada.Id, _usuarioLogado.Id);
                MessageBox.Show("Reserva cancelada.", "Reservas", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CarregarReservas();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao cancelar reserva: {ex.Message}", "Reservas", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgv_reservas_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgv_reservas.CurrentRow?.DataBoundItem is Reserva reserva)
            {
                _selecionada = reserva;
            }
        }

        private void CarregarReservas()
        {
            if (string.IsNullOrWhiteSpace(txt_matriculaAluno.Text) || txt_matriculaAluno.Text == "Digite a matrícula do aluno...")
            {
                MessageBox.Show("Campo obrigatório: Matrícula", "Reservas", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txt_matriculaAluno.Focus();
                return;
            }

            try
            {
                var reservas = _servicoReserva.ListarPorAluno(txt_matriculaAluno.Text.Trim()).ToList();
                _bindingSource.DataSource = reservas;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar reservas: {ex.Message}", "Reservas", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExibirResumoLivro(string codigo)
        {
            var livro = _servicoLivro.ObterPorCodigo(codigo);
            if (livro != null)
            {
                txt_tituloautor.Text = $"{livro.Titulo} - {livro.Autor}";
            }
        }
    }
}
