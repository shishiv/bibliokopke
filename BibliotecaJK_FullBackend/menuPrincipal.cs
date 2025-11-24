using BibliotecaJK.Modelos;
using BibliotecaJK.Servicos;
using System;
using System.Windows.Forms;

namespace BibliotecaJK
{
    public partial class menuPrincipal : Form
    {
        private readonly Funcionario _usuarioLogado;
        private readonly ServicoPainel _servicoPainel = new();

        public menuPrincipal(Funcionario usuarioLogado)
        {
            _usuarioLogado = usuarioLogado ?? throw new ArgumentNullException(nameof(usuarioLogado));
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            lbl_usuarioLogado.Text = $"Usuário: {_usuarioLogado.Nome}";
            AtualizarPainel();
        }

        private void AtualizarPainel()
        {
            try
            {
                var resumo = _servicoPainel.ObterResumo();
                lbl_dashboardLivros.Text = $"Livros: {resumo.TotalLivros}";
                lbl_dashboardAlunos.Text = $"Alunos: {resumo.TotalAlunos}";
                lbl_dashboardLivrosEmprestados.Text = $"Livros Emprestados: {resumo.LivrosEmprestados}";
                lbl_dashboardReservas.Text = $"Reservas: {resumo.ReservasAtivas}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Não foi possível carregar o painel: {ex.Message}", "Painel", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btn_cadastrarAluno_Click(object? sender, EventArgs e)
        {
            AbrirModal(new CadastrarAluno(_usuarioLogado));
        }

        private void btn_cadastrarLivro_Click(object? sender, EventArgs e)
        {
            AbrirModal(new CadastrarLivro(_usuarioLogado));
        }

        private void btn_cadastrarFuncionario_Click(object? sender, EventArgs e)
        {
            AbrirModal(new CadastrarFuncionario(_usuarioLogado));
        }

        private void btn_emprestimo_Click(object? sender, EventArgs e)
        {
            AbrirModal(new EmprestimoDevolucao(_usuarioLogado));
        }

        private void btn_devolucao_Click(object? sender, EventArgs e)
        {
            AbrirModal(new Devolucao(_usuarioLogado));
        }

        private void btn_reserva_Click(object? sender, EventArgs e)
        {
            AbrirModal(new Reservas(_usuarioLogado));
        }

        private void btn_pesquisarAcervo_Click(object? sender, EventArgs e)
        {
            AbrirModal(new PesquisaAcervo(_usuarioLogado));
        }

        private void button6_Click(object? sender, EventArgs e)
        {
            Close();
        }

        private void AbrirModal(Form form)
        {
            using (form)
            {
                form.ShowDialog();
            }
            AtualizarPainel();
        }
    }
}
