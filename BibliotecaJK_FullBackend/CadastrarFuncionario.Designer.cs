namespace BibliotecaJK
{
    partial class CadastrarFuncionario
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
            cmb_perfil = new ComboBox();
            lbl_perfil = new Label();
            txt_cargo = new TextBox();
            txt_nomeFuncionario = new TextBox();
            txt_senha = new TextBox();
            txt_login = new TextBox();
            txt_cpfFuncionario = new TextBox();
            lbl_cargo = new Label();
            lbl_nomeFuncionario = new Label();
            lbl_cpfFuncionario = new Label();
            lbl_login = new Label();
            lbl_senha = new Label();
            lbl_logintela = new Label();
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
            btn_salvar.Location = new Point(696, 471);
            btn_salvar.Margin = new Padding(4, 3, 4, 3);
            btn_salvar.Name = "btn_salvar";
            btn_salvar.Size = new Size(104, 40);
            btn_salvar.TabIndex = 25;
            btn_salvar.Text = "Salvar";
            btn_salvar.UseVisualStyleBackColor = true;
            // 
            // btn_editar
            // 
            btn_editar.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_editar.ForeColor = Color.Black;
            btn_editar.Location = new Point(565, 470);
            btn_editar.Margin = new Padding(4, 3, 4, 3);
            btn_editar.Name = "btn_editar";
            btn_editar.Size = new Size(104, 40);
            btn_editar.TabIndex = 24;
            btn_editar.Text = "Editar";
            btn_editar.UseVisualStyleBackColor = true;
            // 
            // btn_limpar
            // 
            btn_limpar.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_limpar.ForeColor = Color.Black;
            btn_limpar.Location = new Point(428, 470);
            btn_limpar.Margin = new Padding(4, 3, 4, 3);
            btn_limpar.Name = "btn_limpar";
            btn_limpar.Size = new Size(120, 40);
            btn_limpar.TabIndex = 23;
            btn_limpar.Text = "🆕 Limpar Campos";
            btn_limpar.UseVisualStyleBackColor = true;
            // 
            // btn_excluir
            // 
            btn_excluir.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_excluir.ForeColor = Color.Black;
            btn_excluir.Location = new Point(294, 471);
            btn_excluir.Margin = new Padding(4, 3, 4, 3);
            btn_excluir.Name = "btn_excluir";
            btn_excluir.Size = new Size(104, 40);
            btn_excluir.TabIndex = 22;
            btn_excluir.Text = "Excluir";
            btn_excluir.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.ControlDarkDark;
            panel1.Controls.Add(cmb_perfil);
            panel1.Controls.Add(lbl_perfil);
            panel1.Controls.Add(txt_cargo);
            panel1.Controls.Add(txt_nomeFuncionario);
            panel1.Controls.Add(txt_senha);
            panel1.Controls.Add(txt_login);
            panel1.Controls.Add(txt_cpfFuncionario);
            panel1.Controls.Add(lbl_cargo);
            panel1.Controls.Add(lbl_nomeFuncionario);
            panel1.Controls.Add(lbl_cpfFuncionario);
            panel1.Controls.Add(lbl_login);
            panel1.Controls.Add(lbl_senha);
            panel1.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            panel1.Location = new Point(205, 56);
            panel1.Margin = new Padding(4, 3, 4, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(550, 132);
            panel1.TabIndex = 20;
            // 
            // cmb_perfil
            // 
            cmb_perfil.DropDownStyle = ComboBoxStyle.DropDownList;
            cmb_perfil.FormattingEnabled = true;
            cmb_perfil.Items.AddRange(new object[] { "ADMIN", "BIBLIOTECARIO", "OPERADOR" });
            cmb_perfil.Location = new Point(405, 87);
            cmb_perfil.Margin = new Padding(4, 3, 4, 3);
            cmb_perfil.Name = "cmb_perfil";
            cmb_perfil.Size = new Size(116, 25);
            cmb_perfil.TabIndex = 15;
            // 
            // lbl_perfil
            // 
            lbl_perfil.AutoSize = true;
            lbl_perfil.ForeColor = SystemColors.Control;
            lbl_perfil.Location = new Point(401, 64);
            lbl_perfil.Margin = new Padding(4, 0, 4, 0);
            lbl_perfil.Name = "lbl_perfil";
            lbl_perfil.Size = new Size(41, 17);
            lbl_perfil.TabIndex = 14;
            lbl_perfil.Text = "Perfil";
            lbl_perfil.TextAlign = ContentAlignment.TopRight;
            // 
            // txt_cargo
            // 
            txt_cargo.Location = new Point(405, 32);
            txt_cargo.Margin = new Padding(4, 3, 4, 3);
            txt_cargo.Name = "txt_cargo";
            txt_cargo.Size = new Size(116, 25);
            txt_cargo.TabIndex = 13;
            // 
            // txt_nomeFuncionario
            // 
            txt_nomeFuncionario.Location = new Point(219, 32);
            txt_nomeFuncionario.Margin = new Padding(4, 3, 4, 3);
            txt_nomeFuncionario.Name = "txt_nomeFuncionario";
            txt_nomeFuncionario.Size = new Size(116, 25);
            txt_nomeFuncionario.TabIndex = 12;
            // 
            // txt_senha
            // 
            txt_senha.Location = new Point(219, 87);
            txt_senha.Margin = new Padding(4, 3, 4, 3);
            txt_senha.Name = "txt_senha";
            txt_senha.Size = new Size(116, 25);
            txt_senha.TabIndex = 11;
            // 
            // txt_login
            // 
            txt_login.Location = new Point(31, 87);
            txt_login.Margin = new Padding(4, 3, 4, 3);
            txt_login.Name = "txt_login";
            txt_login.Size = new Size(116, 25);
            txt_login.TabIndex = 10;
            // 
            // txt_cpfFuncionario
            // 
            txt_cpfFuncionario.Location = new Point(31, 32);
            txt_cpfFuncionario.Margin = new Padding(4, 3, 4, 3);
            txt_cpfFuncionario.Name = "txt_cpfFuncionario";
            txt_cpfFuncionario.Size = new Size(116, 25);
            txt_cpfFuncionario.TabIndex = 9;
            // 
            // lbl_cargo
            // 
            lbl_cargo.AutoSize = true;
            lbl_cargo.ForeColor = SystemColors.Control;
            lbl_cargo.Location = new Point(401, 9);
            lbl_cargo.Margin = new Padding(4, 0, 4, 0);
            lbl_cargo.Name = "lbl_cargo";
            lbl_cargo.Size = new Size(44, 17);
            lbl_cargo.TabIndex = 7;
            lbl_cargo.Text = "Cargo";
            lbl_cargo.TextAlign = ContentAlignment.TopRight;
            // 
            // lbl_nomeFuncionario
            // 
            lbl_nomeFuncionario.AutoSize = true;
            lbl_nomeFuncionario.ForeColor = SystemColors.Control;
            lbl_nomeFuncionario.Location = new Point(216, 9);
            lbl_nomeFuncionario.Margin = new Padding(4, 0, 4, 0);
            lbl_nomeFuncionario.Name = "lbl_nomeFuncionario";
            lbl_nomeFuncionario.Size = new Size(45, 17);
            lbl_nomeFuncionario.TabIndex = 6;
            lbl_nomeFuncionario.Text = "Nome";
            lbl_nomeFuncionario.TextAlign = ContentAlignment.TopRight;
            // 
            // lbl_cpfFuncionario
            // 
            lbl_cpfFuncionario.AutoSize = true;
            lbl_cpfFuncionario.ForeColor = SystemColors.Control;
            lbl_cpfFuncionario.Location = new Point(33, 9);
            lbl_cpfFuncionario.Margin = new Padding(4, 0, 4, 0);
            lbl_cpfFuncionario.Name = "lbl_cpfFuncionario";
            lbl_cpfFuncionario.Size = new Size(31, 17);
            lbl_cpfFuncionario.TabIndex = 3;
            lbl_cpfFuncionario.Text = "CPF";
            lbl_cpfFuncionario.TextAlign = ContentAlignment.TopRight;
            // 
            // lbl_login
            // 
            lbl_login.AutoSize = true;
            lbl_login.ForeColor = SystemColors.Control;
            lbl_login.Location = new Point(33, 63);
            lbl_login.Margin = new Padding(4, 0, 4, 0);
            lbl_login.Name = "lbl_login";
            lbl_login.Size = new Size(43, 17);
            lbl_login.TabIndex = 2;
            lbl_login.Text = "Login";
            lbl_login.TextAlign = ContentAlignment.TopRight;
            // 
            // lbl_senha
            // 
            lbl_senha.AutoSize = true;
            lbl_senha.ForeColor = SystemColors.Control;
            lbl_senha.Location = new Point(216, 63);
            lbl_senha.Margin = new Padding(4, 0, 4, 0);
            lbl_senha.Name = "lbl_senha";
            lbl_senha.Size = new Size(45, 17);
            lbl_senha.TabIndex = 1;
            lbl_senha.Text = "Senha";
            lbl_senha.TextAlign = ContentAlignment.TopRight;
            // 
            // lbl_logintela
            // 
            lbl_logintela.AutoSize = true;
            lbl_logintela.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbl_logintela.ForeColor = SystemColors.ControlText;
            lbl_logintela.Location = new Point(391, 16);
            lbl_logintela.Margin = new Padding(4, 0, 4, 0);
            lbl_logintela.Name = "lbl_logintela";
            lbl_logintela.Size = new Size(184, 21);
            lbl_logintela.TabIndex = 26;
            lbl_logintela.Text = "Cadastrar Funcionarios";
            // 
            // pictureBox2
            // 
            pictureBox2.Image = Properties.Resources.bibliokopke;
            pictureBox2.Location = new Point(1, 0);
            pictureBox2.Margin = new Padding(4, 3, 4, 3);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(134, 122);
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.TabIndex = 21;
            pictureBox2.TabStop = false;
            // 
            // btn_voltar
            // 
            btn_voltar.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_voltar.ForeColor = Color.Black;
            btn_voltar.Location = new Point(160, 471);
            btn_voltar.Margin = new Padding(4, 3, 4, 3);
            btn_voltar.Name = "btn_voltar";
            btn_voltar.Size = new Size(104, 40);
            btn_voltar.TabIndex = 27;
            btn_voltar.Text = "Voltar";
            btn_voltar.UseVisualStyleBackColor = true;
            // 
            // CadastrarFuncionario
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlDark;
            ClientSize = new Size(959, 537);
            Controls.Add(btn_voltar);
            Controls.Add(lbl_logintela);
            Controls.Add(btn_salvar);
            Controls.Add(btn_editar);
            Controls.Add(btn_limpar);
            Controls.Add(btn_excluir);
            Controls.Add(pictureBox2);
            Controls.Add(panel1);
            Margin = new Padding(4, 3, 4, 3);
            Name = "CadastrarFuncionario";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "CadastrarFuncionario";
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
        private System.Windows.Forms.TextBox txt_cargo;
        private System.Windows.Forms.TextBox txt_nomeFuncionario;
        private System.Windows.Forms.TextBox txt_senha;
        private System.Windows.Forms.TextBox txt_login;
        private System.Windows.Forms.TextBox txt_cpfFuncionario;
        private System.Windows.Forms.Label lbl_cargo;
        private System.Windows.Forms.Label lbl_nomeFuncionario;
        private System.Windows.Forms.Label lbl_cpfFuncionario;
        private System.Windows.Forms.Label lbl_login;
        private System.Windows.Forms.Label lbl_senha;
        private System.Windows.Forms.Label lbl_logintela;
        private System.Windows.Forms.Button btn_voltar;
        private System.Windows.Forms.ComboBox cmb_perfil;
        private System.Windows.Forms.Label lbl_perfil;
    }
}
