using BibliotecaJK.Utilitarios;
using BibliotecaJK.Modelos;
using BibliotecaJK.Servicos;
using System;
using System.Windows.Forms;

namespace BibliotecaJK
{
    public partial class Form1 : Form
    {
        private readonly ServicoAutenticacao _servicoAutenticacao = new();

        public Form1()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            txtSenha.UseSystemPasswordChar = true;
            AcceptButton = btnEntrar;
            CancelButton = btnSair;
        }

        private void btnEntrar_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsuario.Text))
            {
                MessageBox.Show("Campo obrigatório: Usuário", "Autenticação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsuario.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtSenha.Text))
            {
                MessageBox.Show("Campo obrigatório: Senha", "Autenticação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSenha.Focus();
                return;
            }

            try
            {
                var usuario = _servicoAutenticacao.Autenticar(txtUsuario.Text.Trim(), txtSenha.Text.Trim());
                AbrirMenu(usuario);
            }
            catch (ExcecaoValidacao ex)
            {
                MessageBox.Show(ex.Message, "Autenticação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro inesperado ao tentar autenticar: {ex.Message}", "Autenticação", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSair_Click(object? sender, EventArgs e)
        {
            Close();
        }

        private void AbrirMenu(Funcionario usuario)
        {
            Hide();
            using (var menu = new menuPrincipal(usuario))
            {
                menu.ShowDialog();
            }
            txtSenha.Clear();
            txtUsuario.Focus();
            Show();
        }
    }
}
