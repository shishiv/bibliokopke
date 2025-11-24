namespace BibliotecaJK
{
    partial class Devolucao
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
            this.lbl_pesquisaracervo = new System.Windows.Forms.Label();
            this.btn_voltar = new System.Windows.Forms.Button();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn_confirmarDevolucao = new System.Windows.Forms.Button();
            this.lbl_tituloAutor = new System.Windows.Forms.Label();
            this.lbl_codigoLivro = new System.Windows.Forms.Label();
            this.txt_tituloautor = new System.Windows.Forms.TextBox();
            this.txt_codigolivro = new System.Windows.Forms.TextBox();
            this.txt_matriculaAluno = new System.Windows.Forms.TextBox();
            this.lbl_matriculaAluno = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbl_pesquisaracervo
            // 
            this.lbl_pesquisaracervo.AutoSize = true;
            this.lbl_pesquisaracervo.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_pesquisaracervo.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lbl_pesquisaracervo.Location = new System.Drawing.Point(361, 8);
            this.lbl_pesquisaracervo.Name = "lbl_pesquisaracervo";
            this.lbl_pesquisaracervo.Size = new System.Drawing.Size(92, 21);
            this.lbl_pesquisaracervo.TabIndex = 42;
            this.lbl_pesquisaracervo.Text = "Devolução";
            // 
            // btn_voltar
            // 
            this.btn_voltar.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_voltar.ForeColor = System.Drawing.Color.Black;
            this.btn_voltar.Location = new System.Drawing.Point(553, 408);
            this.btn_voltar.Name = "btn_voltar";
            this.btn_voltar.Size = new System.Drawing.Size(89, 35);
            this.btn_voltar.TabIndex = 41;
            this.btn_voltar.Text = "Voltar";
            this.btn_voltar.UseVisualStyleBackColor = true;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::BibliotecaJK.Properties.Resources.bibliokopke;
            this.pictureBox2.Location = new System.Drawing.Point(1, 0);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(115, 106);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 40;
            this.pictureBox2.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.panel1.Controls.Add(this.btn_confirmarDevolucao);
            this.panel1.Controls.Add(this.lbl_tituloAutor);
            this.panel1.Controls.Add(this.lbl_codigoLivro);
            this.panel1.Controls.Add(this.txt_tituloautor);
            this.panel1.Controls.Add(this.txt_codigolivro);
            this.panel1.Controls.Add(this.txt_matriculaAluno);
            this.panel1.Controls.Add(this.lbl_matriculaAluno);
            this.panel1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel1.Location = new System.Drawing.Point(171, 98);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(471, 261);
            this.panel1.TabIndex = 39;
            // 
            // btn_confirmarDevolucao
            // 
            this.btn_confirmarDevolucao.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_confirmarDevolucao.ForeColor = System.Drawing.Color.Black;
            this.btn_confirmarDevolucao.Location = new System.Drawing.Point(255, 204);
            this.btn_confirmarDevolucao.Name = "btn_confirmarDevolucao";
            this.btn_confirmarDevolucao.Size = new System.Drawing.Size(191, 34);
            this.btn_confirmarDevolucao.TabIndex = 35;
            this.btn_confirmarDevolucao.Text = "Confirmar Devolução";
            this.btn_confirmarDevolucao.UseVisualStyleBackColor = true;
            // 
            // lbl_tituloAutor
            // 
            this.lbl_tituloAutor.AutoSize = true;
            this.lbl_tituloAutor.ForeColor = System.Drawing.SystemColors.Control;
            this.lbl_tituloAutor.Location = new System.Drawing.Point(28, 138);
            this.lbl_tituloAutor.Name = "lbl_tituloAutor";
            this.lbl_tituloAutor.Size = new System.Drawing.Size(86, 17);
            this.lbl_tituloAutor.TabIndex = 13;
            this.lbl_tituloAutor.Text = "Titulo/Autor";
            this.lbl_tituloAutor.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lbl_codigoLivro
            //
            this.lbl_codigoLivro.AutoSize = true;
            this.lbl_codigoLivro.ForeColor = System.Drawing.SystemColors.Control;
            this.lbl_codigoLivro.Location = new System.Drawing.Point(28, 80);
            this.lbl_codigoLivro.Name = "lbl_codigoLivro";
            this.lbl_codigoLivro.Size = new System.Drawing.Size(165, 17);
            this.lbl_codigoLivro.TabIndex = 12;
            this.lbl_codigoLivro.Text = "ISBN ou Código do Livro";
            this.lbl_codigoLivro.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txt_tituloautor
            //
            this.txt_tituloautor.BackColor = System.Drawing.Color.LightGray;
            this.txt_tituloautor.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_tituloautor.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.txt_tituloautor.Location = new System.Drawing.Point(30, 158);
            this.txt_tituloautor.Name = "txt_tituloautor";
            this.txt_tituloautor.Size = new System.Drawing.Size(416, 22);
            this.txt_tituloautor.TabIndex = 11;
            this.txt_tituloautor.Text = "Campo para digitar título ou autor...";
            // 
            // txt_codigolivro
            // 
            this.txt_codigolivro.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_codigolivro.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.txt_codigolivro.Location = new System.Drawing.Point(30, 100);
            this.txt_codigolivro.Name = "txt_codigolivro";
            this.txt_codigolivro.Size = new System.Drawing.Size(416, 22);
            this.txt_codigolivro.TabIndex = 10;
            this.txt_codigolivro.Text = "Campo para digitar código do livro...";
            // 
            // txt_matriculaAluno
            // 
            this.txt_matriculaAluno.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_matriculaAluno.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.txt_matriculaAluno.Location = new System.Drawing.Point(30, 44);
            this.txt_matriculaAluno.Name = "txt_matriculaAluno";
            this.txt_matriculaAluno.Size = new System.Drawing.Size(416, 22);
            this.txt_matriculaAluno.TabIndex = 9;
            this.txt_matriculaAluno.Text = "Campo para digitar matrícula do aluno...";
            // 
            // lbl_matriculaAluno
            //
            this.lbl_matriculaAluno.AutoSize = true;
            this.lbl_matriculaAluno.ForeColor = System.Drawing.SystemColors.Control;
            this.lbl_matriculaAluno.Location = new System.Drawing.Point(28, 24);
            this.lbl_matriculaAluno.Name = "lbl_matriculaAluno";
            this.lbl_matriculaAluno.Size = new System.Drawing.Size(125, 17);
            this.lbl_matriculaAluno.TabIndex = 3;
            this.lbl_matriculaAluno.Text = "Matrícula do Aluno";
            this.lbl_matriculaAluno.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // Devolucao
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ClientSize = new System.Drawing.Size(823, 456);
            this.Controls.Add(this.lbl_pesquisaracervo);
            this.Controls.Add(this.btn_voltar);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.panel1);
            this.Name = "Devolucao";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Devolucao";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_pesquisaracervo;
        private System.Windows.Forms.Button btn_voltar;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lbl_tituloAutor;
        private System.Windows.Forms.Label lbl_codigoLivro;
        private System.Windows.Forms.TextBox txt_tituloautor;
        private System.Windows.Forms.TextBox txt_codigolivro;
        private System.Windows.Forms.TextBox txt_matriculaAluno;
        private System.Windows.Forms.Label lbl_matriculaAluno;
        private System.Windows.Forms.Button btn_confirmarDevolucao;
    }
}