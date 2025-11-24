using BibliotecaJK.Modelos;
using BibliotecaJK.Servicos;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace BibliotecaJK
{
    public partial class PesquisaAcervo : Form
    {
        private readonly ServicoLivro _servicoLivro = new();
        private readonly BindingSource _bindingSource = new();

        public PesquisaAcervo(Funcionario _)
        {
            InitializeComponent();
            ConfigurarGrid();
            ConfigurarPlaceholder();
            RegistrarEventos();
            CarregarLivros();
        }

        private void ConfigurarPlaceholder()
        {
            txt_pesquisa.Text = "(Digite título, autor ou ISBN)";
            txt_pesquisa.ForeColor = System.Drawing.Color.Gray;
        }

        private void RegistrarEventos()
        {
            btn_buscar.Click += (_, _) => CarregarLivros(txt_pesquisa.Text.Trim());
            btn_detalhes.Click += btn_detalhes_Click;
            btn_voltar.Click += (_, _) => Close();
            txt_pesquisa.Enter += txt_pesquisa_Enter;
            txt_pesquisa.Leave += txt_pesquisa_Leave;
            txt_pesquisa.KeyPress += txt_pesquisa_KeyPress;
        }

        private void txt_pesquisa_Enter(object? sender, EventArgs e)
        {
            if (txt_pesquisa.Text == "(Digite título, autor ou ISBN)" && txt_pesquisa.ForeColor == System.Drawing.Color.Gray)
            {
                txt_pesquisa.Text = "";
                txt_pesquisa.ForeColor = System.Drawing.SystemColors.WindowText;
            }
        }

        private void txt_pesquisa_Leave(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_pesquisa.Text))
            {
                txt_pesquisa.Text = "(Digite título, autor ou ISBN)";
                txt_pesquisa.ForeColor = System.Drawing.Color.Gray;
            }
        }

        private void txt_pesquisa_KeyPress(object? sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                btn_buscar.PerformClick();
            }
        }

        private void ConfigurarGrid()
        {
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.BackgroundColor = SystemColors.ControlLight;
            dataGridView1.Columns.Clear();
            dataGridView1.Columns.AddRange(new DataGridViewColumn[]
            {
                new DataGridViewTextBoxColumn { DataPropertyName = nameof(Livro.Titulo), HeaderText = "Título", Width = 200 },
                new DataGridViewTextBoxColumn { DataPropertyName = nameof(Livro.Autor), HeaderText = "Autor", Width = 150 },
                new DataGridViewTextBoxColumn { DataPropertyName = nameof(Livro.ISBN), HeaderText = "ISBN", Width = 120 },
                new DataGridViewTextBoxColumn { DataPropertyName = nameof(Livro.QuantidadeDisponivel), HeaderText = "Disponível", Width = 90 }
            });

            dataGridView1.DataSource = _bindingSource;
        }

        private void CarregarLivros(string? termo = null)
        {
            try
            {
                // Ignora o placeholder ao buscar
                if (termo == "(Digite título, autor ou ISBN)")
                {
                    termo = null;
                }
                _bindingSource.DataSource = _servicoLivro.Listar(termo).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar o acervo: {ex.Message}", "Acervo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_detalhes_Click(object? sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow?.DataBoundItem is not Livro livro)
            {
                MessageBox.Show("Selecione um livro.", "Acervo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var detalhes = $"Título: {livro.Titulo}\nAutor: {livro.Autor}\nEditora: {livro.Editora}\nISBN: {livro.ISBN}\nAno: {livro.AnoPublicacao}\nDisponíveis: {livro.QuantidadeDisponivel}";
            MessageBox.Show(detalhes, "Detalhes do Livro", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
