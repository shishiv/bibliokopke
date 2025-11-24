namespace BibliotecaJK
{
    partial class CadastrarLivro
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panel1 = new Panel();
            txt_localizacao = new TextBox();
            lbl_localizacao = new Label();
            txt_estoque = new TextBox();
            txt_editora = new TextBox();
            txt_autor = new TextBox();
            txt_codigolivro = new TextBox();
            txt_ano = new TextBox();
            txt_titulo = new TextBox();
            lbl_qtdEstoque = new Label();
            lbl_genero = new Label();
            lbl_titulo = new Label();
            lbl_autor = new Label();
            lbl_ano = new Label();
            lbl_codigoLivro = new Label();
            btn_excluir = new Button();
            btn_limpar = new Button();
            btn_editar = new Button();
            btn_salvar = new Button();
            lbl_cadastrarlivros = new Label();
            pictureBox2 = new PictureBox();
            btn_voltar = new Button();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.ControlDarkDark;
            panel1.Controls.Add(txt_localizacao);
            panel1.Controls.Add(lbl_localizacao);
            panel1.Controls.Add(txt_estoque);
            panel1.Controls.Add(txt_editora);
            panel1.Controls.Add(txt_autor);
            panel1.Controls.Add(txt_codigolivro);
            panel1.Controls.Add(txt_ano);
            panel1.Controls.Add(txt_titulo);
            panel1.Controls.Add(lbl_qtdEstoque);
            panel1.Controls.Add(lbl_genero);
            panel1.Controls.Add(lbl_titulo);
            panel1.Controls.Add(lbl_autor);
            panel1.Controls.Add(lbl_ano);
            panel1.Controls.Add(lbl_codigoLivro);
            panel1.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            panel1.Location = new Point(147, 49);
            panel1.Margin = new Padding(4, 3, 4, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(670, 136);
            panel1.TabIndex = 1;
            // 
            // txt_localizacao
            // 
            txt_localizacao.Location = new Point(359, 94);
            txt_localizacao.Margin = new Padding(4, 3, 4, 3);
            txt_localizacao.Name = "txt_localizacao";
            txt_localizacao.Size = new Size(116, 25);
            txt_localizacao.TabIndex = 15;
            // 
            // lbl_localizacao
            // 
            lbl_localizacao.AutoSize = true;
            lbl_localizacao.ForeColor = SystemColors.Control;
            lbl_localizacao.Location = new Point(355, 71);
            lbl_localizacao.Margin = new Padding(4, 0, 4, 0);
            lbl_localizacao.Name = "lbl_localizacao";
            lbl_localizacao.Size = new Size(78, 17);
            lbl_localizacao.TabIndex = 16;
            lbl_localizacao.Text = "Localização";
            lbl_localizacao.TextAlign = ContentAlignment.TopRight;
            // 
            // txt_estoque
            // 
            txt_estoque.Location = new Point(197, 94);
            txt_estoque.Margin = new Padding(4, 3, 4, 3);
            txt_estoque.Name = "txt_estoque";
            txt_estoque.Size = new Size(116, 25);
            txt_estoque.TabIndex = 6;
            // 
            // txt_editora
            //
            txt_editora.Location = new Point(486, 31);
            txt_editora.Margin = new Padding(4, 3, 4, 3);
            txt_editora.Name = "txt_editora";
            txt_editora.Size = new Size(153, 25);
            txt_editora.TabIndex = 4;
            // 
            // txt_autor
            // 
            txt_autor.Location = new Point(260, 31);
            txt_autor.Margin = new Padding(4, 3, 4, 3);
            txt_autor.Name = "txt_autor";
            txt_autor.Size = new Size(174, 25);
            txt_autor.TabIndex = 2;
            // 
            // txt_codigolivro
            // 
            txt_codigolivro.Location = new Point(31, 94);
            txt_codigolivro.Margin = new Padding(4, 3, 4, 3);
            txt_codigolivro.Name = "txt_codigolivro";
            txt_codigolivro.Size = new Size(123, 25);
            txt_codigolivro.TabIndex = 3;
            // 
            // txt_ano
            // 
            txt_ano.Location = new Point(523, 94);
            txt_ano.Margin = new Padding(4, 3, 4, 3);
            txt_ano.Name = "txt_ano";
            txt_ano.Size = new Size(116, 25);
            txt_ano.TabIndex = 5;
            // 
            // txt_titulo
            // 
            txt_titulo.Location = new Point(31, 31);
            txt_titulo.Margin = new Padding(4, 3, 4, 3);
            txt_titulo.Name = "txt_titulo";
            txt_titulo.Size = new Size(180, 25);
            txt_titulo.TabIndex = 1;
            // 
            // lbl_qtdEstoque
            // 
            lbl_qtdEstoque.AutoSize = true;
            lbl_qtdEstoque.ForeColor = SystemColors.Control;
            lbl_qtdEstoque.Location = new Point(194, 71);
            lbl_qtdEstoque.Margin = new Padding(4, 0, 4, 0);
            lbl_qtdEstoque.Name = "lbl_qtdEstoque";
            lbl_qtdEstoque.Size = new Size(88, 17);
            lbl_qtdEstoque.TabIndex = 8;
            lbl_qtdEstoque.Text = "Quantidade Total";
            lbl_qtdEstoque.TextAlign = ContentAlignment.TopRight;
            // 
            // lbl_genero
            // 
            lbl_genero.AutoSize = true;
            lbl_genero.ForeColor = SystemColors.Control;
            lbl_genero.Location = new Point(482, 8);
            lbl_genero.Margin = new Padding(4, 0, 4, 0);
            lbl_genero.Name = "lbl_genero";
            lbl_genero.Size = new Size(52, 17);
            lbl_genero.TabIndex = 7;
            lbl_genero.Text = "Editora";
            lbl_genero.TextAlign = ContentAlignment.TopRight;
            // 
            // lbl_titulo
            // 
            lbl_titulo.AutoSize = true;
            lbl_titulo.ForeColor = SystemColors.Control;
            lbl_titulo.Location = new Point(28, 8);
            lbl_titulo.Margin = new Padding(4, 0, 4, 0);
            lbl_titulo.Name = "lbl_titulo";
            lbl_titulo.Size = new Size(45, 17);
            lbl_titulo.TabIndex = 6;
            lbl_titulo.Text = "Titulo";
            lbl_titulo.TextAlign = ContentAlignment.TopRight;
            // 
            // lbl_autor
            // 
            lbl_autor.AutoSize = true;
            lbl_autor.ForeColor = SystemColors.Control;
            lbl_autor.Location = new Point(257, 8);
            lbl_autor.Margin = new Padding(4, 0, 4, 0);
            lbl_autor.Name = "lbl_autor";
            lbl_autor.Size = new Size(43, 17);
            lbl_autor.TabIndex = 3;
            lbl_autor.Text = "Autor";
            lbl_autor.TextAlign = ContentAlignment.TopRight;
            // 
            // lbl_ano
            // 
            lbl_ano.AutoSize = true;
            lbl_ano.ForeColor = SystemColors.Control;
            lbl_ano.Location = new Point(523, 74);
            lbl_ano.Margin = new Padding(4, 0, 4, 0);
            lbl_ano.Name = "lbl_ano";
            lbl_ano.Size = new Size(33, 17);
            lbl_ano.TabIndex = 2;
            lbl_ano.Text = "Ano";
            lbl_ano.TextAlign = ContentAlignment.TopRight;
            // 
            // lbl_codigoLivro
            // 
            lbl_codigoLivro.AutoSize = true;
            lbl_codigoLivro.ForeColor = SystemColors.Control;
            lbl_codigoLivro.Location = new Point(28, 70);
            lbl_codigoLivro.Margin = new Padding(4, 0, 4, 0);
            lbl_codigoLivro.Name = "lbl_codigoLivro";
            lbl_codigoLivro.Size = new Size(37, 17);
            lbl_codigoLivro.TabIndex = 1;
            lbl_codigoLivro.Text = "ISBN";
            lbl_codigoLivro.TextAlign = ContentAlignment.TopRight;
            // 
            // btn_excluir
            // 
            btn_excluir.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_excluir.ForeColor = Color.Black;
            btn_excluir.Location = new Point(271, 455);
            btn_excluir.Margin = new Padding(4, 3, 4, 3);
            btn_excluir.Name = "btn_excluir";
            btn_excluir.Size = new Size(104, 40);
            btn_excluir.TabIndex = 10;
            btn_excluir.Text = "Excluir";
            btn_excluir.UseVisualStyleBackColor = true;
            // 
            // btn_limpar
            // 
            btn_limpar.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_limpar.ForeColor = Color.Black;
            btn_limpar.Location = new Point(538, 455);
            btn_limpar.Margin = new Padding(4, 3, 4, 3);
            btn_limpar.Name = "btn_limpar";
            btn_limpar.Size = new Size(120, 40);
            btn_limpar.TabIndex = 8;
            btn_limpar.Text = "🆕 Limpar Campos";
            btn_limpar.UseVisualStyleBackColor = true;
            // 
            // btn_editar
            // 
            btn_editar.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_editar.ForeColor = Color.Black;
            btn_editar.Location = new Point(405, 455);
            btn_editar.Margin = new Padding(4, 3, 4, 3);
            btn_editar.Name = "btn_editar";
            btn_editar.Size = new Size(104, 40);
            btn_editar.TabIndex = 12;
            btn_editar.Text = "Editar";
            btn_editar.UseVisualStyleBackColor = true;
            // 
            // btn_salvar
            // 
            btn_salvar.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_salvar.ForeColor = Color.Black;
            btn_salvar.Location = new Point(665, 455);
            btn_salvar.Margin = new Padding(4, 3, 4, 3);
            btn_salvar.Name = "btn_salvar";
            btn_salvar.Size = new Size(104, 40);
            btn_salvar.TabIndex = 7;
            btn_salvar.Text = "Salvar";
            btn_salvar.UseVisualStyleBackColor = true;
            // 
            // lbl_cadastrarlivros
            // 
            lbl_cadastrarlivros.AutoSize = true;
            lbl_cadastrarlivros.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbl_cadastrarlivros.ForeColor = SystemColors.ControlText;
            lbl_cadastrarlivros.Location = new Point(394, 16);
            lbl_cadastrarlivros.Margin = new Padding(4, 0, 4, 0);
            lbl_cadastrarlivros.Name = "lbl_cadastrarlivros";
            lbl_cadastrarlivros.Size = new Size(124, 21);
            lbl_cadastrarlivros.TabIndex = 14;
            lbl_cadastrarlivros.Text = "Cadastrar Livro";
            // 
            // pictureBox2
            // 
            pictureBox2.Image = Properties.Resources.bibliokopke;
            pictureBox2.Location = new Point(2, 1);
            pictureBox2.Margin = new Padding(4, 3, 4, 3);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(134, 122);
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.TabIndex = 5;
            pictureBox2.TabStop = false;
            // 
            // btn_voltar
            // 
            btn_voltar.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_voltar.ForeColor = Color.Black;
            btn_voltar.Location = new Point(140, 455);
            btn_voltar.Margin = new Padding(4, 3, 4, 3);
            btn_voltar.Name = "btn_voltar";
            btn_voltar.Size = new Size(104, 40);
            btn_voltar.TabIndex = 28;
            btn_voltar.Text = "Voltar";
            btn_voltar.UseVisualStyleBackColor = true;
            // 
            // CadastrarLivro
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlDark;
            ClientSize = new Size(964, 537);
            Controls.Add(btn_voltar);
            Controls.Add(lbl_cadastrarlivros);
            Controls.Add(btn_salvar);
            Controls.Add(btn_editar);
            Controls.Add(btn_limpar);
            Controls.Add(btn_excluir);
            Controls.Add(pictureBox2);
            Controls.Add(panel1);
            Margin = new Padding(4, 3, 4, 3);
            Name = "CadastrarLivro";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "CadastrarLivro";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label lbl_qtdEstoque;
        private System.Windows.Forms.Label lbl_genero;
        private System.Windows.Forms.Label lbl_titulo;
        private System.Windows.Forms.Label lbl_autor;
        private System.Windows.Forms.Label lbl_ano;
        private System.Windows.Forms.Label lbl_codigoLivro;
        private System.Windows.Forms.Button btn_excluir;
        private System.Windows.Forms.Button btn_limpar;
        private System.Windows.Forms.Button btn_editar;
        private System.Windows.Forms.Button btn_salvar;
        private System.Windows.Forms.TextBox txt_estoque;
        private System.Windows.Forms.TextBox txt_editora;
        private System.Windows.Forms.TextBox txt_autor;
        private System.Windows.Forms.TextBox txt_codigolivro;
        private System.Windows.Forms.TextBox txt_ano;
        private System.Windows.Forms.TextBox txt_titulo;
        private System.Windows.Forms.Label lbl_cadastrarlivros;
        private System.Windows.Forms.Button btn_voltar;
        private System.Windows.Forms.TextBox txt_localizacao;
        private System.Windows.Forms.Label lbl_localizacao;
    }
}
