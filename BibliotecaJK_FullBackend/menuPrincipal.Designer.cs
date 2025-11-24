namespace BibliotecaJK
{
    partial class menuPrincipal
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
            this.btn_devolucao = new System.Windows.Forms.Button();
            this.btn_cadastrarLivro = new System.Windows.Forms.Button();
            this.btn_cadastrarAluno = new System.Windows.Forms.Button();
            this.btn_emprestimo = new System.Windows.Forms.Button();
            this.btn_cadastrarFuncionario = new System.Windows.Forms.Button();
            this.btn_sair = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn_reserva = new System.Windows.Forms.Button();
            this.btn_pesquisarAcervo = new System.Windows.Forms.Button();
            this.lbl_menuprincipal = new System.Windows.Forms.Label();
            this.lbl_usuarioLogado = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.gbx_dashboard = new System.Windows.Forms.GroupBox();
            this.lbl_dashboardReservas = new System.Windows.Forms.Label();
            this.lbl_dashboardLivrosEmprestados = new System.Windows.Forms.Label();
            this.lbl_dashboardAlunos = new System.Windows.Forms.Label();
            this.lbl_dashboardLivros = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.gbx_dashboard.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_devolucao
            //
            this.btn_devolucao.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_devolucao.ForeColor = System.Drawing.Color.Black;
            this.btn_devolucao.Location = new System.Drawing.Point(20, 146);
            this.btn_devolucao.Name = "btn_devolucao";
            this.btn_devolucao.Size = new System.Drawing.Size(191, 35);
            this.btn_devolucao.TabIndex = 3;
            this.btn_devolucao.Text = "Devolução";
            this.btn_devolucao.UseVisualStyleBackColor = true;
            this.btn_devolucao.Click += new System.EventHandler(this.btn_devolucao_Click);
            // 
            // btn_cadastrarLivro
            //
            this.btn_cadastrarLivro.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_cadastrarLivro.ForeColor = System.Drawing.Color.Black;
            this.btn_cadastrarLivro.Location = new System.Drawing.Point(288, 26);
            this.btn_cadastrarLivro.Name = "btn_cadastrarLivro";
            this.btn_cadastrarLivro.Size = new System.Drawing.Size(191, 35);
            this.btn_cadastrarLivro.TabIndex = 4;
            this.btn_cadastrarLivro.Text = "Cadastrar Livro";
            this.btn_cadastrarLivro.UseVisualStyleBackColor = true;
            this.btn_cadastrarLivro.Click += new System.EventHandler(this.btn_cadastrarLivro_Click);
            // 
            // btn_cadastrarAluno
            //
            this.btn_cadastrarAluno.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_cadastrarAluno.ForeColor = System.Drawing.Color.Black;
            this.btn_cadastrarAluno.Location = new System.Drawing.Point(288, 85);
            this.btn_cadastrarAluno.Name = "btn_cadastrarAluno";
            this.btn_cadastrarAluno.Size = new System.Drawing.Size(191, 35);
            this.btn_cadastrarAluno.TabIndex = 5;
            this.btn_cadastrarAluno.Text = "Cadastrar Aluno";
            this.btn_cadastrarAluno.UseVisualStyleBackColor = true;
            this.btn_cadastrarAluno.Click += new System.EventHandler(this.btn_cadastrarAluno_Click);
            // 
            // btn_emprestimo
            //
            this.btn_emprestimo.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_emprestimo.ForeColor = System.Drawing.Color.Black;
            this.btn_emprestimo.Location = new System.Drawing.Point(20, 85);
            this.btn_emprestimo.Name = "btn_emprestimo";
            this.btn_emprestimo.Size = new System.Drawing.Size(191, 35);
            this.btn_emprestimo.TabIndex = 7;
            this.btn_emprestimo.Text = "Empréstimo";
            this.btn_emprestimo.UseVisualStyleBackColor = true;
            this.btn_emprestimo.Click += new System.EventHandler(this.btn_emprestimo_Click);
            // 
            // btn_cadastrarFuncionario
            //
            this.btn_cadastrarFuncionario.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_cadastrarFuncionario.ForeColor = System.Drawing.Color.Black;
            this.btn_cadastrarFuncionario.Location = new System.Drawing.Point(288, 146);
            this.btn_cadastrarFuncionario.Name = "btn_cadastrarFuncionario";
            this.btn_cadastrarFuncionario.Size = new System.Drawing.Size(191, 35);
            this.btn_cadastrarFuncionario.TabIndex = 8;
            this.btn_cadastrarFuncionario.Text = "Cadastrar Funcionário";
            this.btn_cadastrarFuncionario.UseVisualStyleBackColor = true;
            this.btn_cadastrarFuncionario.Click += new System.EventHandler(this.btn_cadastrarFuncionario_Click);
            // 
            // btn_sair
            //
            this.btn_sair.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_sair.ForeColor = System.Drawing.Color.Black;
            this.btn_sair.Location = new System.Drawing.Point(569, 426);
            this.btn_sair.Name = "btn_sair";
            this.btn_sair.Size = new System.Drawing.Size(89, 35);
            this.btn_sair.TabIndex = 9;
            this.btn_sair.Text = "Sair";
            this.btn_sair.UseVisualStyleBackColor = true;
            this.btn_sair.Click += new System.EventHandler(this.button6_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.panel1.Controls.Add(this.btn_reserva);
            this.panel1.Controls.Add(this.btn_pesquisarAcervo);
            this.panel1.Controls.Add(this.btn_cadastrarLivro);
            this.panel1.Controls.Add(this.btn_cadastrarAluno);
            this.panel1.Controls.Add(this.btn_devolucao);
            this.panel1.Controls.Add(this.btn_emprestimo);
            this.panel1.Controls.Add(this.btn_cadastrarFuncionario);
            this.panel1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel1.Location = new System.Drawing.Point(159, 145);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(499, 248);
            this.panel1.TabIndex = 10;
            // 
            // btn_reserva
            //
            this.btn_reserva.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_reserva.ForeColor = System.Drawing.Color.Black;
            this.btn_reserva.Location = new System.Drawing.Point(20, 196);
            this.btn_reserva.Name = "btn_reserva";
            this.btn_reserva.Size = new System.Drawing.Size(191, 35);
            this.btn_reserva.TabIndex = 10;
            this.btn_reserva.Text = "Reserva";
            this.btn_reserva.UseVisualStyleBackColor = true;
            this.btn_reserva.Click += new System.EventHandler(this.btn_reserva_Click);
            // 
            // btn_pesquisarAcervo
            //
            this.btn_pesquisarAcervo.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_pesquisarAcervo.ForeColor = System.Drawing.Color.Black;
            this.btn_pesquisarAcervo.Location = new System.Drawing.Point(20, 26);
            this.btn_pesquisarAcervo.Name = "btn_pesquisarAcervo";
            this.btn_pesquisarAcervo.Size = new System.Drawing.Size(191, 35);
            this.btn_pesquisarAcervo.TabIndex = 9;
            this.btn_pesquisarAcervo.Text = "Pesquisar Acervo";
            this.btn_pesquisarAcervo.UseVisualStyleBackColor = true;
            this.btn_pesquisarAcervo.Click += new System.EventHandler(this.btn_pesquisarAcervo_Click);
            // 
            // lbl_menuprincipal
            //
            this.lbl_menuprincipal.AutoSize = true;
            this.lbl_menuprincipal.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_menuprincipal.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lbl_menuprincipal.Location = new System.Drawing.Point(369, 9);
            this.lbl_menuprincipal.Name = "lbl_menuprincipal";
            this.lbl_menuprincipal.Size = new System.Drawing.Size(126, 21);
            this.lbl_menuprincipal.TabIndex = 11;
            this.lbl_menuprincipal.Text = "Menu Principal";
            //
            // lbl_usuarioLogado
            //
            this.lbl_usuarioLogado.AutoSize = true;
            this.lbl_usuarioLogado.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_usuarioLogado.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lbl_usuarioLogado.Location = new System.Drawing.Point(600, 12);
            this.lbl_usuarioLogado.Name = "lbl_usuarioLogado";
            this.lbl_usuarioLogado.Size = new System.Drawing.Size(100, 19);
            this.lbl_usuarioLogado.TabIndex = 13;
            this.lbl_usuarioLogado.Text = "Usuário: ";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::BibliotecaJK.Properties.Resources.bibliokopke;
            this.pictureBox2.Location = new System.Drawing.Point(3, 2);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(115, 106);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 10;
            this.pictureBox2.TabStop = false;
            // 
            // gbx_dashboard
            // 
            this.gbx_dashboard.Controls.Add(this.lbl_dashboardReservas);
            this.gbx_dashboard.Controls.Add(this.lbl_dashboardLivrosEmprestados);
            this.gbx_dashboard.Controls.Add(this.lbl_dashboardAlunos);
            this.gbx_dashboard.Controls.Add(this.lbl_dashboardLivros);
            this.gbx_dashboard.ForeColor = System.Drawing.SystemColors.ControlText;
            this.gbx_dashboard.Location = new System.Drawing.Point(159, 48);
            this.gbx_dashboard.Name = "gbx_dashboard";
            this.gbx_dashboard.Size = new System.Drawing.Size(499, 81);
            this.gbx_dashboard.TabIndex = 12;
            this.gbx_dashboard.TabStop = false;
            this.gbx_dashboard.Text = "Resumo da Biblioteca";
            // 
            // lbl_dashboardReservas
            // 
            this.lbl_dashboardReservas.AutoSize = true;
            this.lbl_dashboardReservas.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_dashboardReservas.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lbl_dashboardReservas.Location = new System.Drawing.Point(266, 45);
            this.lbl_dashboardReservas.Name = "lbl_dashboardReservas";
            this.lbl_dashboardReservas.Size = new System.Drawing.Size(103, 21);
            this.lbl_dashboardReservas.TabIndex = 16;
            this.lbl_dashboardReservas.Text = "Reservas: 12";
            // 
            // lbl_dashboardLivrosEmprestados
            // 
            this.lbl_dashboardLivrosEmprestados.AutoSize = true;
            this.lbl_dashboardLivrosEmprestados.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_dashboardLivrosEmprestados.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lbl_dashboardLivrosEmprestados.Location = new System.Drawing.Point(266, 16);
            this.lbl_dashboardLivrosEmprestados.Name = "lbl_dashboardLivrosEmprestados";
            this.lbl_dashboardLivrosEmprestados.Size = new System.Drawing.Size(183, 21);
            this.lbl_dashboardLivrosEmprestados.TabIndex = 15;
            this.lbl_dashboardLivrosEmprestados.Text = "Livros Emprestados: 48";
            // 
            // lbl_dashboardAlunos
            // 
            this.lbl_dashboardAlunos.AutoSize = true;
            this.lbl_dashboardAlunos.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_dashboardAlunos.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lbl_dashboardAlunos.Location = new System.Drawing.Point(60, 45);
            this.lbl_dashboardAlunos.Name = "lbl_dashboardAlunos";
            this.lbl_dashboardAlunos.Size = new System.Drawing.Size(98, 21);
            this.lbl_dashboardAlunos.TabIndex = 14;
            this.lbl_dashboardAlunos.Text = "Alunos: 120";
            // 
            // lbl_dashboardLivros
            // 
            this.lbl_dashboardLivros.AutoSize = true;
            this.lbl_dashboardLivros.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_dashboardLivros.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lbl_dashboardLivros.Location = new System.Drawing.Point(60, 16);
            this.lbl_dashboardLivros.Name = "lbl_dashboardLivros";
            this.lbl_dashboardLivros.Size = new System.Drawing.Size(90, 21);
            this.lbl_dashboardLivros.TabIndex = 13;
            this.lbl_dashboardLivros.Text = "Livros: 150";
            // 
            // menuPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ClientSize = new System.Drawing.Size(817, 473);
            this.Controls.Add(this.gbx_dashboard);
            this.Controls.Add(this.lbl_usuarioLogado);
            this.Controls.Add(this.lbl_menuprincipal);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btn_sair);
            this.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.Name = "menuPrincipal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "menuPrincipal";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.gbx_dashboard.ResumeLayout(false);
            this.gbx_dashboard.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_devolucao;
        private System.Windows.Forms.Button btn_cadastrarLivro;
        private System.Windows.Forms.Button btn_cadastrarAluno;
        private System.Windows.Forms.Button btn_emprestimo;
        private System.Windows.Forms.Button btn_cadastrarFuncionario;
        private System.Windows.Forms.Button btn_sair;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btn_pesquisarAcervo;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label lbl_menuprincipal;
        private System.Windows.Forms.Label lbl_usuarioLogado;
        private System.Windows.Forms.Button btn_reserva;
        private System.Windows.Forms.GroupBox gbx_dashboard;
        private System.Windows.Forms.Label lbl_dashboardReservas;
        private System.Windows.Forms.Label lbl_dashboardLivrosEmprestados;
        private System.Windows.Forms.Label lbl_dashboardAlunos;
        private System.Windows.Forms.Label lbl_dashboardLivros;
    }
}
