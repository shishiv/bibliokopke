using BibliotecaJK.Utilitarios;
using BibliotecaJK.Modelos;
using BibliotecaJK.Servicos;
using System;
using System.Windows.Forms;

namespace BibliotecaJK
{
    public partial class Devolucao : Form
    {
        private readonly Funcionario _usuarioLogado;
        private readonly ServicoEmprestimo _servicoEmprestimo = new();
        private readonly ServicoLivro _servicoLivro = new();

        public Devolucao(Funcionario usuarioLogado)
        {
            _usuarioLogado = usuarioLogado;
            InitializeComponent();
            txt_tituloautor.ReadOnly = true;
            ConfigurarPlaceholders();
            btn_confirmarDevolucao.Click += btn_confirmarDevolucao_Click;
            btn_voltar.Click += (_, _) => Close();
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

        private void btn_confirmarDevolucao_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_matriculaAluno.Text) || txt_matriculaAluno.Text == "Digite a matrícula do aluno...")
            {
                MessageBox.Show("Campo obrigatório: Matrícula", "Devolução", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_matriculaAluno.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txt_codigolivro.Text) || txt_codigolivro.Text == "Digite o ISBN ou código do livro...")
            {
                MessageBox.Show("Campo obrigatório: Código do Livro", "Devolução", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_codigolivro.Focus();
                return;
            }

            try
            {
                var devolucao = _servicoEmprestimo.RegistrarDevolucao(
                    txt_matriculaAluno.Text.Trim(),
                    txt_codigolivro.Text.Trim(),
                    DateTime.Today,
                    _usuarioLogado.Id);

                var mensagem = devolucao.Multa > 0
                    ? $"Devolução registrada com multa de R$ {devolucao.Multa:F2}."
                    : "Devolução registrada com sucesso!";
                MessageBox.Show(mensagem, "Devolução", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ExibirResumoLivro(txt_codigolivro.Text.Trim());
                LimparCampos();
                txt_matriculaAluno.Focus();
            }
            catch (ExcecaoValidacao ex)
            {
                MessageBox.Show(ex.Message, "Devolução", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao registrar devolução: {ex.Message}", "Devolução", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void LimparCampos()
        {
            txt_matriculaAluno.Clear();
            txt_codigolivro.Clear();
            txt_tituloautor.Clear();
        }
    }
}
