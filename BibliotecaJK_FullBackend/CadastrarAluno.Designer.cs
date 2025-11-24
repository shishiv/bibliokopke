namespace BibliotecaJK
{
    partial class CadastrarAluno
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
            btn_salvar = new Button();
            btn_editar = new Button();
            btn_limpar = new Button();
            btn_excluir = new Button();
            panel1 = new Panel();
            txt_cpf = new TextBox();
            lbl_cpf = new Label();
            txt_CursoTurma = new TextBox();
            txt_nomeAluno = new TextBox();
            txt_email = new TextBox();
            txt_telefone = new TextBox();
            txt_matricula = new TextBox();
            lbl_cursoTurma = new Label();
            lbl_nome = new Label();
            lbl_matricula = new Label();
            lbl_telefone = new Label();
            lbl_email = new Label();
            lbl_cadastraraluno = new Label();
            pictureBox2 = new PictureBox();
            btn_voltar = new Button();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            SuspendLayout();
            // 
            // btn_salvar
            // 
            btn_salvar.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_salvar.ForeColor = Color.Black;
            btn_salvar.Location = new Point(702, 473);
            btn_salvar.Margin = new Padding(4, 3, 4, 3);
            btn_salvar.Name = "btn_salvar";
            btn_salvar.Size = new Size(104, 40);
            btn_salvar.TabIndex = 6;
            btn_salvar.Text = "Salvar";
            btn_salvar.UseVisualStyleBackColor = true;
            // 
            // btn_editar
            // 
            btn_editar.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_editar.ForeColor = Color.Black;
            btn_editar.Location = new Point(438, 473);
            btn_editar.Margin = new Padding(4, 3, 4, 3);
            btn_editar.Name = "btn_editar";
            btn_editar.Size = new Size(104, 40);
            btn_editar.TabIndex = 18;
            btn_editar.Text = "Editar";
            btn_editar.UseVisualStyleBackColor = true;
            // 
            // btn_limpar
            // 
            btn_limpar.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_limpar.ForeColor = Color.Black;
            btn_limpar.Location = new Point(570, 473);
            btn_limpar.Margin = new Padding(4, 3, 4, 3);
            btn_limpar.Name = "btn_limpar";
            btn_limpar.Size = new Size(120, 40);
            btn_limpar.TabIndex = 7;
            btn_limpar.Text = "🆕 Limpar Campos";
            btn_limpar.UseVisualStyleBackColor = true;
            // 
            // btn_excluir
            // 
            btn_excluir.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_excluir.ForeColor = Color.Black;
            btn_excluir.Location = new Point(301, 473);
            btn_excluir.Margin = new Padding(4, 3, 4, 3);
            btn_excluir.Name = "btn_excluir";
            btn_excluir.Size = new Size(104, 40);
            btn_excluir.TabIndex = 16;
            btn_excluir.Text = "Excluir";
            btn_excluir.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.ControlDarkDark;
            panel1.Controls.Add(txt_cpf);
            panel1.Controls.Add(lbl_cpf);
            panel1.Controls.Add(txt_CursoTurma);
            panel1.Controls.Add(txt_nomeAluno);
            panel1.Controls.Add(txt_email);
            panel1.Controls.Add(txt_telefone);
            panel1.Controls.Add(txt_matricula);
            panel1.Controls.Add(lbl_cursoTurma);
            panel1.Controls.Add(lbl_nome);
            panel1.Controls.Add(lbl_matricula);
            panel1.Controls.Add(lbl_telefone);
            panel1.Controls.Add(lbl_email);
            panel1.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            panel1.Location = new Point(183, 57);
            panel1.Margin = new Padding(4, 3, 4, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(609, 129);
            panel1.TabIndex = 14;
            // 
            // txt_cpf
            //
            txt_cpf.Location = new Point(428, 90);
            txt_cpf.Margin = new Padding(4, 3, 4, 3);
            txt_cpf.Name = "txt_cpf";
            txt_cpf.Size = new Size(164, 25);
            txt_cpf.TabIndex = 5;
            // 
            // lbl_cpf
            // 
            lbl_cpf.AutoSize = true;
            lbl_cpf.ForeColor = SystemColors.Control;
            lbl_cpf.Location = new Point(428, 67);
            lbl_cpf.Margin = new Padding(4, 0, 4, 0);
            lbl_cpf.Name = "lbl_cpf";
            lbl_cpf.Size = new Size(31, 17);
            lbl_cpf.TabIndex = 14;
            lbl_cpf.Text = "CPF";
            lbl_cpf.TextAlign = ContentAlignment.TopRight;
            // 
            // txt_CursoTurma
            //
            txt_CursoTurma.Location = new Point(432, 33);
            txt_CursoTurma.Margin = new Padding(4, 3, 4, 3);
            txt_CursoTurma.Name = "txt_CursoTurma";
            txt_CursoTurma.Size = new Size(160, 25);
            txt_CursoTurma.TabIndex = 3;
            // 
            // txt_nomeAluno
            //
            txt_nomeAluno.Location = new Point(230, 33);
            txt_nomeAluno.Margin = new Padding(4, 3, 4, 3);
            txt_nomeAluno.Name = "txt_nomeAluno";
            txt_nomeAluno.Size = new Size(176, 25);
            txt_nomeAluno.TabIndex = 2;
            // 
            // txt_email
            //
            txt_email.Location = new Point(227, 90);
            txt_email.Margin = new Padding(4, 3, 4, 3);
            txt_email.Name = "txt_email";
            txt_email.Size = new Size(179, 25);
            txt_email.TabIndex = 4;
            // 
            // txt_telefone
            // 
            txt_telefone.Location = new Point(31, 90);
            txt_telefone.Margin = new Padding(4, 3, 4, 3);
            txt_telefone.Name = "txt_telefone";
            txt_telefone.Size = new Size(166, 25);
            txt_telefone.TabIndex = 10;
            // 
            // txt_matricula
            //
            txt_matricula.Location = new Point(31, 33);
            txt_matricula.Margin = new Padding(4, 3, 4, 3);
            txt_matricula.Name = "txt_matricula";
            txt_matricula.Size = new Size(166, 25);
            txt_matricula.TabIndex = 1;
            // 
            // lbl_cursoTurma
            // 
            lbl_cursoTurma.AutoSize = true;
            lbl_cursoTurma.ForeColor = SystemColors.Control;
            lbl_cursoTurma.Location = new Point(428, 10);
            lbl_cursoTurma.Margin = new Padding(4, 0, 4, 0);
            lbl_cursoTurma.Name = "lbl_cursoTurma";
            lbl_cursoTurma.Size = new Size(88, 17);
            lbl_cursoTurma.TabIndex = 7;
            lbl_cursoTurma.Text = "Curso/Turma";
            lbl_cursoTurma.TextAlign = ContentAlignment.TopRight;
            // 
            // lbl_nome
            // 
            lbl_nome.AutoSize = true;
            lbl_nome.ForeColor = SystemColors.Control;
            lbl_nome.Location = new Point(227, 10);
            lbl_nome.Margin = new Padding(4, 0, 4, 0);
            lbl_nome.Name = "lbl_nome";
            lbl_nome.Size = new Size(45, 17);
            lbl_nome.TabIndex = 6;
            lbl_nome.Text = "Nome";
            lbl_nome.TextAlign = ContentAlignment.TopRight;
            // 
            // lbl_matricula
            // 
            lbl_matricula.AutoSize = true;
            lbl_matricula.ForeColor = SystemColors.Control;
            lbl_matricula.Location = new Point(28, 10);
            lbl_matricula.Margin = new Padding(4, 0, 4, 0);
            lbl_matricula.Name = "lbl_matricula";
            lbl_matricula.Size = new Size(66, 17);
            lbl_matricula.TabIndex = 3;
            lbl_matricula.Text = "Matricula";
            lbl_matricula.TextAlign = ContentAlignment.TopRight;
            // 
            // lbl_telefone
            // 
            lbl_telefone.AutoSize = true;
            lbl_telefone.ForeColor = SystemColors.Control;
            lbl_telefone.Location = new Point(28, 67);
            lbl_telefone.Margin = new Padding(4, 0, 4, 0);
            lbl_telefone.Name = "lbl_telefone";
            lbl_telefone.Size = new Size(61, 17);
            lbl_telefone.TabIndex = 2;
            lbl_telefone.Text = "Telefone";
            lbl_telefone.TextAlign = ContentAlignment.TopRight;
            // 
            // lbl_email
            // 
            lbl_email.AutoSize = true;
            lbl_email.ForeColor = SystemColors.Control;
            lbl_email.Location = new Point(230, 70);
            lbl_email.Margin = new Padding(4, 0, 4, 0);
            lbl_email.Name = "lbl_email";
            lbl_email.Size = new Size(42, 17);
            lbl_email.TabIndex = 1;
            lbl_email.Text = "Email";
            lbl_email.TextAlign = ContentAlignment.TopRight;
            // 
            // lbl_cadastraraluno
            // 
            lbl_cadastraraluno.AutoSize = true;
            lbl_cadastraraluno.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbl_cadastraraluno.ForeColor = SystemColors.ControlText;
            lbl_cadastraraluno.Location = new Point(401, 17);
            lbl_cadastraraluno.Margin = new Padding(4, 0, 4, 0);
            lbl_cadastraraluno.Name = "lbl_cadastraraluno";
            lbl_cadastraraluno.Size = new Size(132, 21);
            lbl_cadastraraluno.TabIndex = 20;
            lbl_cadastraraluno.Text = "Cadastrar Aluno";
            // 
            // pictureBox2
            // 
            pictureBox2.Image = Properties.Resources.bibliokopke;
            pictureBox2.Location = new Point(1, 1);
            pictureBox2.Margin = new Padding(4, 3, 4, 3);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(134, 122);
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.TabIndex = 15;
            pictureBox2.TabStop = false;
            // 
            // btn_voltar
            // 
            btn_voltar.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_voltar.ForeColor = Color.Black;
            btn_voltar.Location = new Point(161, 473);
            btn_voltar.Margin = new Padding(4, 3, 4, 3);
            btn_voltar.Name = "btn_voltar";
            btn_voltar.Size = new Size(104, 40);
            btn_voltar.TabIndex = 28;
            btn_voltar.Text = "Voltar";
            btn_voltar.UseVisualStyleBackColor = true;
            // 
            // CadastrarAluno
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlDark;
            ClientSize = new Size(961, 539);
            Controls.Add(btn_voltar);
            Controls.Add(lbl_cadastraraluno);
            Controls.Add(btn_salvar);
            Controls.Add(btn_editar);
            Controls.Add(btn_limpar);
            Controls.Add(btn_excluir);
            Controls.Add(pictureBox2);
            Controls.Add(panel1);
            Margin = new Padding(4, 3, 4, 3);
            Name = "CadastrarAluno";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "CadastrarAluno";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_salvar;
        private System.Windows.Forms.Button btn_editar;
        private System.Windows.Forms.Button btn_limpar;
        private System.Windows.Forms.Button btn_excluir;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txt_CursoTurma;
        private System.Windows.Forms.TextBox txt_nomeAluno;
        private System.Windows.Forms.TextBox txt_email;
        private System.Windows.Forms.TextBox txt_telefone;
        private System.Windows.Forms.TextBox txt_matricula;
        private System.Windows.Forms.Label lbl_cursoTurma;
        private System.Windows.Forms.Label lbl_nome;
        private System.Windows.Forms.Label lbl_matricula;
        private System.Windows.Forms.Label lbl_telefone;
        private System.Windows.Forms.Label lbl_email;
        private System.Windows.Forms.Label lbl_cadastraraluno;
        private System.Windows.Forms.Button btn_voltar;
        private System.Windows.Forms.TextBox txt_cpf;
        private System.Windows.Forms.Label lbl_cpf;
    }
}
