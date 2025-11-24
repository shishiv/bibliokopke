namespace BibliotecaJK
{
    partial class Reservas
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
            lbl_pesquisaracervo = new Label();
            btn_voltar = new Button();
            pictureBox2 = new PictureBox();
            panel1 = new Panel();
            label1 = new Label();
            dtp_dataParaReserva = new DateTimePicker();
            btn_cancelarReserva = new Button();
            dgv_reservas = new DataGridView();
            btn_confirmarReserva = new Button();
            btn_carregarReservas = new Button();
            lbl_hintGrid = new Label();
            lbl_tituloAutor = new Label();
            lbl_codigoLivro = new Label();
            txt_tituloautor = new TextBox();
            txt_codigolivro = new TextBox();
            txt_matriculaAluno = new TextBox();
            lbl_matriculaAluno = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgv_reservas).BeginInit();
            SuspendLayout();
            // 
            // lbl_pesquisaracervo
            // 
            lbl_pesquisaracervo.AutoSize = true;
            lbl_pesquisaracervo.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbl_pesquisaracervo.ForeColor = SystemColors.ControlText;
            lbl_pesquisaracervo.Location = new Point(420, 9);
            lbl_pesquisaracervo.Margin = new Padding(4, 0, 4, 0);
            lbl_pesquisaracervo.Name = "lbl_pesquisaracervo";
            lbl_pesquisaracervo.Size = new Size(70, 21);
            lbl_pesquisaracervo.TabIndex = 46;
            lbl_pesquisaracervo.Text = "Reserva";
            // 
            // btn_voltar
            // 
            btn_voltar.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_voltar.ForeColor = Color.Black;
            btn_voltar.Location = new Point(198, 471);
            btn_voltar.Margin = new Padding(4, 3, 4, 3);
            btn_voltar.Name = "btn_voltar";
            btn_voltar.Size = new Size(104, 40);
            btn_voltar.TabIndex = 45;
            btn_voltar.Text = "Voltar";
            btn_voltar.UseVisualStyleBackColor = true;
            // 
            // pictureBox2
            // 
            pictureBox2.Image = Properties.Resources.bibliokopke;
            pictureBox2.Location = new Point(0, 0);
            pictureBox2.Margin = new Padding(4, 3, 4, 3);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(134, 122);
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.TabIndex = 44;
            pictureBox2.TabStop = false;
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.ControlDarkDark;
            panel1.Controls.Add(label1);
            panel1.Controls.Add(dtp_dataParaReserva);
            panel1.Controls.Add(dgv_reservas);
            panel1.Controls.Add(lbl_hintGrid);
            panel1.Controls.Add(btn_carregarReservas);
            panel1.Controls.Add(lbl_tituloAutor);
            panel1.Controls.Add(lbl_codigoLivro);
            panel1.Controls.Add(txt_tituloautor);
            panel1.Controls.Add(txt_codigolivro);
            panel1.Controls.Add(txt_matriculaAluno);
            panel1.Controls.Add(lbl_matriculaAluno);
            panel1.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            panel1.Location = new Point(198, 55);
            panel1.Margin = new Padding(4, 3, 4, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(550, 396);
            panel1.TabIndex = 43;
            // 
            // label1
            //
            label1.AutoSize = true;
            label1.ForeColor = SystemColors.Control;
            label1.Location = new Point(38, 191);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(120, 17);
            label1.TabIndex = 49;
            label1.Text = "Data da Reserva:";
            label1.TextAlign = ContentAlignment.TopRight;
            // 
            // dtp_dataParaReserva
            //
            dtp_dataParaReserva.CalendarTitleBackColor = SystemColors.GradientActiveCaption;
            dtp_dataParaReserva.CalendarTitleForeColor = SystemColors.ControlText;
            dtp_dataParaReserva.Location = new Point(160, 187);
            dtp_dataParaReserva.Margin = new Padding(4, 3, 4, 3);
            dtp_dataParaReserva.Name = "dtp_dataParaReserva";
            dtp_dataParaReserva.Size = new Size(360, 25);
            dtp_dataParaReserva.TabIndex = 48;
            // 
            // btn_cancelarReserva
            // 
            btn_cancelarReserva.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_cancelarReserva.ForeColor = Color.Black;
            btn_cancelarReserva.Location = new Point(340, 470);
            btn_cancelarReserva.Margin = new Padding(4, 3, 4, 3);
            btn_cancelarReserva.Name = "btn_cancelarReserva";
            btn_cancelarReserva.Size = new Size(187, 39);
            btn_cancelarReserva.TabIndex = 47;
            btn_cancelarReserva.Text = "Cancelar Reserva";
            btn_cancelarReserva.UseVisualStyleBackColor = true;
            // 
            // dgv_reservas
            // 
            dgv_reservas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgv_reservas.GridColor = SystemColors.ControlText;
            dgv_reservas.Location = new Point(35, 225);
            dgv_reservas.Margin = new Padding(4, 3, 4, 3);
            dgv_reservas.Name = "dgv_reservas";
            dgv_reservas.Size = new Size(485, 156);
            dgv_reservas.TabIndex = 47;
            // 
            // btn_confirmarReserva
            //
            btn_confirmarReserva.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_confirmarReserva.ForeColor = Color.Black;
            btn_confirmarReserva.Location = new Point(561, 471);
            btn_confirmarReserva.Margin = new Padding(4, 3, 4, 3);
            btn_confirmarReserva.Name = "btn_confirmarReserva";
            btn_confirmarReserva.Size = new Size(187, 39);
            btn_confirmarReserva.TabIndex = 35;
            btn_confirmarReserva.Text = "Reservar";
            btn_confirmarReserva.UseVisualStyleBackColor = true;
            //
            // btn_carregarReservas
            //
            btn_carregarReservas.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_carregarReservas.ForeColor = Color.Black;
            btn_carregarReservas.Location = new Point(305, 35);
            btn_carregarReservas.Margin = new Padding(4, 3, 4, 3);
            btn_carregarReservas.Name = "btn_carregarReservas";
            btn_carregarReservas.Size = new Size(150, 28);
            btn_carregarReservas.TabIndex = 50;
            btn_carregarReservas.Text = "🔍 Buscar Reservas";
            btn_carregarReservas.UseVisualStyleBackColor = true;
            //
            // lbl_hintGrid
            //
            lbl_hintGrid.AutoSize = true;
            lbl_hintGrid.Font = new Font("Segoe UI", 8F, FontStyle.Italic, GraphicsUnit.Point, 0);
            lbl_hintGrid.ForeColor = Color.LightGray;
            lbl_hintGrid.Location = new Point(38, 205);
            lbl_hintGrid.Margin = new Padding(4, 0, 4, 0);
            lbl_hintGrid.Name = "lbl_hintGrid";
            lbl_hintGrid.Size = new Size(170, 13);
            lbl_hintGrid.TabIndex = 51;
            lbl_hintGrid.Text = "Selecione o livro desejado";
            // 
            // lbl_tituloAutor
            // 
            lbl_tituloAutor.AutoSize = true;
            lbl_tituloAutor.ForeColor = SystemColors.Control;
            lbl_tituloAutor.Location = new Point(33, 127);
            lbl_tituloAutor.Margin = new Padding(4, 0, 4, 0);
            lbl_tituloAutor.Name = "lbl_tituloAutor";
            lbl_tituloAutor.Size = new Size(86, 17);
            lbl_tituloAutor.TabIndex = 13;
            lbl_tituloAutor.Text = "Titulo/Autor";
            lbl_tituloAutor.TextAlign = ContentAlignment.TopRight;
            // 
            // lbl_codigoLivro
            // 
            lbl_codigoLivro.AutoSize = true;
            lbl_codigoLivro.ForeColor = SystemColors.Control;
            lbl_codigoLivro.Location = new Point(33, 73);
            lbl_codigoLivro.Margin = new Padding(4, 0, 4, 0);
            lbl_codigoLivro.Name = "lbl_codigoLivro";
            lbl_codigoLivro.Size = new Size(107, 17);
            lbl_codigoLivro.TabIndex = 12;
            lbl_codigoLivro.Text = "Código do Livro";
            lbl_codigoLivro.TextAlign = ContentAlignment.TopRight;
            // 
            // txt_tituloautor
            // 
            txt_tituloautor.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txt_tituloautor.ForeColor = SystemColors.ControlDark;
            txt_tituloautor.Location = new Point(35, 150);
            txt_tituloautor.Margin = new Padding(4, 3, 4, 3);
            txt_tituloautor.Name = "txt_tituloautor";
            txt_tituloautor.Size = new Size(485, 22);
            txt_tituloautor.TabIndex = 11;
            txt_tituloautor.Text = "Campo para digitar título ou autor...";
            // 
            // txt_codigolivro
            // 
            txt_codigolivro.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txt_codigolivro.ForeColor = SystemColors.ControlDark;
            txt_codigolivro.Location = new Point(35, 96);
            txt_codigolivro.Margin = new Padding(4, 3, 4, 3);
            txt_codigolivro.Name = "txt_codigolivro";
            txt_codigolivro.Size = new Size(485, 22);
            txt_codigolivro.TabIndex = 10;
            txt_codigolivro.Text = "Campo para digitar código do livro...";
            // 
            // txt_matriculaAluno
            // 
            txt_matriculaAluno.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txt_matriculaAluno.ForeColor = SystemColors.ControlDark;
            txt_matriculaAluno.Location = new Point(35, 38);
            txt_matriculaAluno.Margin = new Padding(4, 3, 4, 3);
            txt_matriculaAluno.Name = "txt_matriculaAluno";
            txt_matriculaAluno.Size = new Size(485, 22);
            txt_matriculaAluno.TabIndex = 9;
            txt_matriculaAluno.Text = "Campo para digitar matrícula do aluno...";
            // 
            // lbl_matriculaAluno
            // 
            lbl_matriculaAluno.AutoSize = true;
            lbl_matriculaAluno.ForeColor = SystemColors.Control;
            lbl_matriculaAluno.Location = new Point(33, 15);
            lbl_matriculaAluno.Margin = new Padding(4, 0, 4, 0);
            lbl_matriculaAluno.Name = "lbl_matriculaAluno";
            lbl_matriculaAluno.Size = new Size(107, 17);
            lbl_matriculaAluno.TabIndex = 3;
            lbl_matriculaAluno.Text = "Matricula Aluno";
            lbl_matriculaAluno.TextAlign = ContentAlignment.TopRight;
            // 
            // Reservas
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlDark;
            ClientSize = new Size(960, 524);
            Controls.Add(lbl_pesquisaracervo);
            Controls.Add(btn_voltar);
            Controls.Add(btn_cancelarReserva);
            Controls.Add(btn_confirmarReserva);
            Controls.Add(pictureBox2);
            Controls.Add(panel1);
            Margin = new Padding(4, 3, 4, 3);
            Name = "Reservas";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Reservas";
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgv_reservas).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_pesquisaracervo;
        private System.Windows.Forms.Button btn_voltar;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btn_confirmarReserva;
        private System.Windows.Forms.Button btn_carregarReservas;
        private System.Windows.Forms.Label lbl_hintGrid;
        private System.Windows.Forms.Label lbl_tituloAutor;
        private System.Windows.Forms.Label lbl_codigoLivro;
        private System.Windows.Forms.TextBox txt_tituloautor;
        private System.Windows.Forms.TextBox txt_codigolivro;
        private System.Windows.Forms.TextBox txt_matriculaAluno;
        private System.Windows.Forms.Label lbl_matriculaAluno;
        private System.Windows.Forms.DataGridView dgv_reservas;
        private System.Windows.Forms.Button btn_cancelarReserva;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtp_dataParaReserva;
    }
}