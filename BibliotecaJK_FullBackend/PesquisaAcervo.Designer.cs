namespace BibliotecaJK
{
    partial class PesquisaAcervo
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
            btn_buscar = new Button();
            btn_detalhes = new Button();
            btn_voltar = new Button();
            pictureBox2 = new PictureBox();
            panel1 = new Panel();
            dataGridView1 = new DataGridView();
            txt_pesquisa = new TextBox();
            lbl_pesquisar = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // lbl_pesquisaracervo
            // 
            lbl_pesquisaracervo.AutoSize = true;
            lbl_pesquisaracervo.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbl_pesquisaracervo.ForeColor = SystemColors.ControlText;
            lbl_pesquisaracervo.Location = new Point(421, 10);
            lbl_pesquisaracervo.Margin = new Padding(4, 0, 4, 0);
            lbl_pesquisaracervo.Name = "lbl_pesquisaracervo";
            lbl_pesquisaracervo.Size = new Size(141, 21);
            lbl_pesquisaracervo.TabIndex = 33;
            lbl_pesquisaracervo.Text = "Pesquisar Acervo";
            // 
            // btn_buscar
            // 
            btn_buscar.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_buscar.ForeColor = Color.Black;
            btn_buscar.Location = new Point(549, 39);
            btn_buscar.Margin = new Padding(4, 3, 4, 3);
            btn_buscar.Name = "btn_buscar";
            btn_buscar.Size = new Size(103, 39);
            btn_buscar.TabIndex = 32;
            btn_buscar.Text = "Pesquisar";
            btn_buscar.UseVisualStyleBackColor = true;
            // 
            // btn_detalhes
            // 
            btn_detalhes.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_detalhes.ForeColor = Color.Black;
            btn_detalhes.Location = new Point(657, 467);
            btn_detalhes.Margin = new Padding(4, 3, 4, 3);
            btn_detalhes.Name = "btn_detalhes";
            btn_detalhes.Size = new Size(104, 40);
            btn_detalhes.TabIndex = 31;
            btn_detalhes.Text = "Detalhes";
            btn_detalhes.UseVisualStyleBackColor = true;
            // 
            // btn_voltar
            // 
            btn_voltar.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_voltar.ForeColor = Color.Black;
            btn_voltar.Location = new Point(778, 467);
            btn_voltar.Margin = new Padding(4, 3, 4, 3);
            btn_voltar.Name = "btn_voltar";
            btn_voltar.Size = new Size(104, 40);
            btn_voltar.TabIndex = 30;
            btn_voltar.Text = "Voltar";
            btn_voltar.UseVisualStyleBackColor = true;
            // 
            // pictureBox2
            // 
            pictureBox2.Image = Properties.Resources.bibliokopke;
            pictureBox2.Location = new Point(1, 1);
            pictureBox2.Margin = new Padding(4, 3, 4, 3);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(134, 122);
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.TabIndex = 28;
            pictureBox2.TabStop = false;
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.ControlDarkDark;
            panel1.Controls.Add(dataGridView1);
            panel1.Controls.Add(btn_buscar);
            panel1.Controls.Add(txt_pesquisa);
            panel1.Controls.Add(lbl_pesquisar);
            panel1.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            panel1.Location = new Point(167, 51);
            panel1.Margin = new Padding(4, 3, 4, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(682, 391);
            panel1.TabIndex = 27;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(33, 129);
            dataGridView1.Margin = new Padding(4, 3, 4, 3);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(619, 240);
            dataGridView1.TabIndex = 33;
            // 
            // txt_pesquisa
            // 
            txt_pesquisa.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txt_pesquisa.ForeColor = Color.Gray;
            txt_pesquisa.Location = new Point(35, 51);
            txt_pesquisa.Margin = new Padding(4, 3, 4, 3);
            txt_pesquisa.Name = "txt_pesquisa";
            txt_pesquisa.Size = new Size(485, 22);
            txt_pesquisa.TabIndex = 9;
            txt_pesquisa.Text = "(Digite título, autor ou ISBN)";
            // 
            // lbl_pesquisar
            // 
            lbl_pesquisar.AutoSize = true;
            lbl_pesquisar.ForeColor = SystemColors.Control;
            lbl_pesquisar.Location = new Point(33, 28);
            lbl_pesquisar.Margin = new Padding(4, 0, 4, 0);
            lbl_pesquisar.Name = "lbl_pesquisar";
            lbl_pesquisar.Size = new Size(67, 17);
            lbl_pesquisar.TabIndex = 3;
            lbl_pesquisar.Text = "Pesquisar";
            lbl_pesquisar.TextAlign = ContentAlignment.TopRight;
            // 
            // PesquisaAcervo
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlDark;
            ClientSize = new Size(960, 528);
            Controls.Add(lbl_pesquisaracervo);
            Controls.Add(btn_detalhes);
            Controls.Add(btn_voltar);
            Controls.Add(pictureBox2);
            Controls.Add(panel1);
            Margin = new Padding(4, 3, 4, 3);
            Name = "PesquisaAcervo";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "PesquisaAcervo";
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_pesquisaracervo;
        private System.Windows.Forms.Button btn_buscar;
        private System.Windows.Forms.Button btn_detalhes;
        private System.Windows.Forms.Button btn_voltar;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txt_pesquisa;
        private System.Windows.Forms.Label lbl_pesquisar;
        private System.Windows.Forms.DataGridView dataGridView1;
    }
}