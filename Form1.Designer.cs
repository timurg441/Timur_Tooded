namespace Tooded
{
    partial class Form1
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtHind = new System.Windows.Forms.TextBox();
            this.txtKogus = new System.Windows.Forms.TextBox();
            this.txtNimetus = new System.Windows.Forms.TextBox();
            this.cmbKategooria = new System.Windows.Forms.ComboBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnLisaKategooria = new System.Windows.Forms.Button();
            this.btnKustutaKategooria = new System.Windows.Forms.Button();
            this.btnOtsiFail = new System.Windows.Forms.Button();
            this.btnLisa = new System.Windows.Forms.Button();
            this.btnUuenda = new System.Windows.Forms.Button();
            this.btnKustuta = new System.Windows.Forms.Button();
            this.dataGridTooted = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridTooted)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.label1.Location = new System.Drawing.Point(52, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(142, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Toode nimetus";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.label2.Location = new System.Drawing.Point(142, 133);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 25);
            this.label2.TabIndex = 1;
            this.label2.Text = "Hind";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.label3.Location = new System.Drawing.Point(125, 80);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 25);
            this.label3.TabIndex = 2;
            this.label3.Text = "Kogus";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.label4.Location = new System.Drawing.Point(87, 184);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(107, 25);
            this.label4.TabIndex = 3;
            this.label4.Text = "Kategooria";
            // 
            // txtHind
            // 
            this.txtHind.Location = new System.Drawing.Point(224, 138);
            this.txtHind.Name = "txtHind";
            this.txtHind.Size = new System.Drawing.Size(100, 20);
            this.txtHind.TabIndex = 4;
            // 
            // txtKogus
            // 
            this.txtKogus.Location = new System.Drawing.Point(224, 85);
            this.txtKogus.Name = "txtKogus";
            this.txtKogus.Size = new System.Drawing.Size(100, 20);
            this.txtKogus.TabIndex = 5;
            // 
            // txtNimetus
            // 
            this.txtNimetus.Location = new System.Drawing.Point(224, 33);
            this.txtNimetus.Name = "txtNimetus";
            this.txtNimetus.Size = new System.Drawing.Size(100, 20);
            this.txtNimetus.TabIndex = 6;
            // 
            // cmbKategooria
            // 
            this.cmbKategooria.FormattingEnabled = true;
            this.cmbKategooria.Location = new System.Drawing.Point(215, 188);
            this.cmbKategooria.Name = "cmbKategooria";
            this.cmbKategooria.Size = new System.Drawing.Size(121, 21);
            this.cmbKategooria.TabIndex = 7;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(360, 33);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(250, 176);
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            // 
            // btnLisaKategooria
            // 
            this.btnLisaKategooria.Location = new System.Drawing.Point(31, 239);
            this.btnLisaKategooria.Name = "btnLisaKategooria";
            this.btnLisaKategooria.Size = new System.Drawing.Size(105, 47);
            this.btnLisaKategooria.TabIndex = 9;
            this.btnLisaKategooria.Text = "Lisa kategooria";
            this.btnLisaKategooria.UseVisualStyleBackColor = true;
            this.btnLisaKategooria.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnKustutaKategooria
            // 
            this.btnKustutaKategooria.Location = new System.Drawing.Point(156, 239);
            this.btnKustutaKategooria.Name = "btnKustutaKategooria";
            this.btnKustutaKategooria.Size = new System.Drawing.Size(105, 47);
            this.btnKustutaKategooria.TabIndex = 10;
            this.btnKustutaKategooria.Text = "Kustuta kategooria";
            this.btnKustutaKategooria.UseVisualStyleBackColor = true;
            // 
            // btnOtsiFail
            // 
            this.btnOtsiFail.Location = new System.Drawing.Point(278, 239);
            this.btnOtsiFail.Name = "btnOtsiFail";
            this.btnOtsiFail.Size = new System.Drawing.Size(105, 47);
            this.btnOtsiFail.TabIndex = 11;
            this.btnOtsiFail.Text = "Otsi fail";
            this.btnOtsiFail.UseVisualStyleBackColor = true;
            // 
            // btnLisa
            // 
            this.btnLisa.Location = new System.Drawing.Point(409, 239);
            this.btnLisa.Name = "btnLisa";
            this.btnLisa.Size = new System.Drawing.Size(105, 47);
            this.btnLisa.TabIndex = 12;
            this.btnLisa.Text = "Lisa";
            this.btnLisa.UseVisualStyleBackColor = true;
            // 
            // btnUuenda
            // 
            this.btnUuenda.Location = new System.Drawing.Point(540, 239);
            this.btnUuenda.Name = "btnUuenda";
            this.btnUuenda.Size = new System.Drawing.Size(105, 47);
            this.btnUuenda.TabIndex = 13;
            this.btnUuenda.Text = "Uuenda";
            this.btnUuenda.UseVisualStyleBackColor = true;
            // 
            // btnKustuta
            // 
            this.btnKustuta.Location = new System.Drawing.Point(667, 239);
            this.btnKustuta.Name = "btnKustuta";
            this.btnKustuta.Size = new System.Drawing.Size(105, 47);
            this.btnKustuta.TabIndex = 14;
            this.btnKustuta.Text = "Kustuta";
            this.btnKustuta.UseVisualStyleBackColor = true;
            // 
            // dataGridTooted
            // 
            this.dataGridTooted.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridTooted.Location = new System.Drawing.Point(31, 292);
            this.dataGridTooted.Name = "dataGridTooted";
            this.dataGridTooted.Size = new System.Drawing.Size(741, 156);
            this.dataGridTooted.TabIndex = 15;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dataGridTooted);
            this.Controls.Add(this.btnKustuta);
            this.Controls.Add(this.btnUuenda);
            this.Controls.Add(this.btnLisa);
            this.Controls.Add(this.btnOtsiFail);
            this.Controls.Add(this.btnKustutaKategooria);
            this.Controls.Add(this.btnLisaKategooria);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.cmbKategooria);
            this.Controls.Add(this.txtNimetus);
            this.Controls.Add(this.txtKogus);
            this.Controls.Add(this.txtHind);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridTooted)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtHind;
        private System.Windows.Forms.TextBox txtKogus;
        private System.Windows.Forms.TextBox txtNimetus;
        private System.Windows.Forms.ComboBox cmbKategooria;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnLisaKategooria;
        private System.Windows.Forms.Button btnKustutaKategooria;
        private System.Windows.Forms.Button btnOtsiFail;
        private System.Windows.Forms.Button btnLisa;
        private System.Windows.Forms.Button btnUuenda;
        private System.Windows.Forms.Button btnKustuta;
        private System.Windows.Forms.DataGridView dataGridTooted;
    }
}

