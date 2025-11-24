namespace BibliotecaJK
{
    partial class Form1
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtSenha = new System.Windows.Forms.TextBox();
            this.txtUsuario = new System.Windows.Forms.TextBox();
            this.lbl_senha = new System.Windows.Forms.Label();
            this.lbl_usuario = new System.Windows.Forms.Label();
            this.btnEntrar = new System.Windows.Forms.Button();
            this.btnSair = new System.Windows.Forms.Button();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.lbl_logintela = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.panel1.Controls.Add(this.txtSenha);
            this.panel1.Controls.Add(this.txtUsuario);
            this.panel1.Controls.Add(this.lbl_senha);
            this.panel1.Controls.Add(this.lbl_usuario);
            this.panel1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel1.Location = new System.Drawing.Point(258, 174);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(304, 167);
            this.panel1.TabIndex = 0;
            // 
            // txtSenha
            //
            this.txtSenha.Location = new System.Drawing.Point(20, 107);
            this.txtSenha.Name = "txtSenha";
            this.txtSenha.Size = new System.Drawing.Size(270, 25);
            this.txtSenha.TabIndex = 2;
            // 
            // txtUsuario
            //
            this.txtUsuario.Location = new System.Drawing.Point(20, 51);
            this.txtUsuario.Name = "txtUsuario";
            this.txtUsuario.Size = new System.Drawing.Size(270, 25);
            this.txtUsuario.TabIndex = 1;
            // 
            // lbl_senha
            // 
            this.lbl_senha.AutoSize = true;
            this.lbl_senha.ForeColor = System.Drawing.SystemColors.Control;
            this.lbl_senha.Location = new System.Drawing.Point(17, 85);
            this.lbl_senha.Name = "lbl_senha";
            this.lbl_senha.Size = new System.Drawing.Size(45, 17);
            this.lbl_senha.TabIndex = 1;
            this.lbl_senha.Text = "Senha";
            this.lbl_senha.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lbl_usuario
            // 
            this.lbl_usuario.AutoSize = true;
            this.lbl_usuario.ForeColor = System.Drawing.SystemColors.Control;
            this.lbl_usuario.Location = new System.Drawing.Point(17, 31);
            this.lbl_usuario.Name = "lbl_usuario";
            this.lbl_usuario.Size = new System.Drawing.Size(55, 17);
            this.lbl_usuario.TabIndex = 0;
            this.lbl_usuario.Text = "Usuário";
            this.lbl_usuario.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // btnEntrar
            //
            this.btnEntrar.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEntrar.Location = new System.Drawing.Point(473, 402);
            this.btnEntrar.Name = "btnEntrar";
            this.btnEntrar.Size = new System.Drawing.Size(89, 35);
            this.btnEntrar.TabIndex = 3;
            this.btnEntrar.Text = "Entrar";
            this.btnEntrar.UseVisualStyleBackColor = true;
            this.btnEntrar.Click += new System.EventHandler(this.btnEntrar_Click);
            // 
            // btnSair
            //
            this.btnSair.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSair.Location = new System.Drawing.Point(258, 402);
            this.btnSair.Name = "btnSair";
            this.btnSair.Size = new System.Drawing.Size(89, 35);
            this.btnSair.TabIndex = 4;
            this.btnSair.Text = "Fechar";
            this.btnSair.UseVisualStyleBackColor = true;
            this.btnSair.Click += new System.EventHandler(this.btnSair_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::BibliotecaJK.Properties.Resources.bibliokopke;
            this.pictureBox2.Location = new System.Drawing.Point(353, 5);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(115, 106);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 4;
            this.pictureBox2.TabStop = false;
            // 
            // lbl_logintela
            // 
            this.lbl_logintela.AutoSize = true;
            this.lbl_logintela.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_logintela.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lbl_logintela.Location = new System.Drawing.Point(358, 140);
            this.lbl_logintela.Name = "lbl_logintela";
            this.lbl_logintela.Size = new System.Drawing.Size(105, 21);
            this.lbl_logintela.TabIndex = 5;
            this.lbl_logintela.Text = "Faça o Login";
            // 
            // Form1
            //
            this.AcceptButton = this.btnEntrar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ClientSize = new System.Drawing.Size(821, 481);
            this.Controls.Add(this.lbl_logintela);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.btnSair);
            this.Controls.Add(this.btnEntrar);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "BiblioKopke - Login";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lbl_usuario;
        private System.Windows.Forms.TextBox txtSenha;
        private System.Windows.Forms.TextBox txtUsuario;
        private System.Windows.Forms.Label lbl_senha;
        private System.Windows.Forms.Button btnEntrar;
        private System.Windows.Forms.Button btnSair;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label lbl_logintela;
    }
}

