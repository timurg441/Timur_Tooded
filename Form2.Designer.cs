using System;
using Timur_Tooded;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Timur_Tooded
{
    partial class Form2
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.dataGridViewTooted = new System.Windows.Forms.DataGridView();
            this.txtNimetus = new System.Windows.Forms.TextBox();
            this.numKogus = new System.Windows.Forms.NumericUpDown();
            this.numHind = new System.Windows.Forms.NumericUpDown();
            this.comboKategooria = new System.Windows.Forms.ComboBox();
            this.picturePilt = new System.Windows.Forms.PictureBox();
            this.btnLisa = new System.Windows.Forms.Button();
            this.btnUuenda = new System.Windows.Forms.Button();
            this.btnKustuta = new System.Windows.Forms.Button();
            this.btnValiPilt = new System.Windows.Forms.Button();
            this.lblKogus = new System.Windows.Forms.Label();
            this.lblHind = new System.Windows.Forms.Label();

            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTooted)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numKogus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHind)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picturePilt)).BeginInit();
            this.SuspendLayout();

            // dataGridViewTooted
            this.dataGridViewTooted.Location = new System.Drawing.Point(12, 12);
            this.dataGridViewTooted.Name = "dataGridViewTooted";
            this.dataGridViewTooted.Size = new System.Drawing.Size(600, 300);
            this.dataGridViewTooted.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewTooted.MultiSelect = false;
            this.dataGridViewTooted.ReadOnly = true;
            this.dataGridViewTooted.AllowUserToAddRows = false;
            this.dataGridViewTooted.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewTooted_CellClick);

            // txtNimetus
            this.txtNimetus.Location = new System.Drawing.Point(630, 30);
            this.txtNimetus.Name = "txtNimetus";
            this.txtNimetus.Width = 200;
            this.txtNimetus.PlaceholderText = "Toote nimetus";

            // lblKogus
            this.lblKogus.AutoSize = true;
            this.lblKogus.Location = new System.Drawing.Point(630, 56);
            this.lblKogus.Name = "lblKogus";
            this.lblKogus.Text = "Kogus";

            // numKogus (целые числа, минимум 0)
            this.numKogus.Location = new System.Drawing.Point(630, 74);
            this.numKogus.Name = "numKogus";
            this.numKogus.Width = 200;
            this.numKogus.Minimum = 0;
            this.numKogus.Maximum = 100000;
            this.numKogus.DecimalPlaces = 0;

            // lblHind
            this.lblHind.AutoSize = true;
            this.lblHind.Location = new System.Drawing.Point(630, 98);
            this.lblHind.Name = "lblHind";
            this.lblHind.Text = "Hind €";

            // numHind (2 знака после запятой, минимум 0)
            this.numHind.Location = new System.Drawing.Point(630, 116);
            this.numHind.Name = "numHind";
            this.numHind.Width = 200;
            this.numHind.Minimum = 0;
            this.numHind.Maximum = 100000;
            this.numHind.DecimalPlaces = 2;

            // comboKategooria
            this.comboKategooria.Location = new System.Drawing.Point(630, 150);
            this.comboKategooria.Name = "comboKategooria";
            this.comboKategooria.Width = 200;
            this.comboKategooria.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            // picturePilt
            this.picturePilt.Location = new System.Drawing.Point(630, 190);
            this.picturePilt.Name = "picturePilt";
            this.picturePilt.Size = new System.Drawing.Size(150, 150);
            this.picturePilt.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picturePilt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picturePilt.TabStop = false;

            // btnValiPilt
            this.btnValiPilt.Location = new System.Drawing.Point(630, 350);
            this.btnValiPilt.Name = "btnValiPilt";
            this.btnValiPilt.Text = "Vali pilt";
            this.btnValiPilt.Click += new System.EventHandler(this.btnValiPilt_Click);

            // btnLisa
            this.btnLisa.Location = new System.Drawing.Point(630, 390);
            this.btnLisa.Name = "btnLisa";
            this.btnLisa.Text = "Lisa";
            this.btnLisa.Click += new System.EventHandler(this.btnLisa_Click);

            // btnUuenda
            this.btnUuenda.Location = new System.Drawing.Point(630, 430);
            this.btnUuenda.Name = "btnUuenda";
            this.btnUuenda.Text = "Uuenda";
            this.btnUuenda.Click += new System.EventHandler(this.btnUuenda_Click);

            // btnKustuta
            this.btnKustuta.Location = new System.Drawing.Point(630, 470);
            this.btnKustuta.Name = "btnKustuta";
            this.btnKustuta.Text = "Kustuta";
            this.btnKustuta.Click += new System.EventHandler(this.btnKustuta_Click);

            // Form2
            this.ClientSize = new System.Drawing.Size(860, 520);
            this.Controls.Add(this.dataGridViewTooted);
            this.Controls.Add(this.txtNimetus);
            this.Controls.Add(this.lblKogus);
            this.Controls.Add(this.numKogus);
            this.Controls.Add(this.lblHind);
            this.Controls.Add(this.numHind);
            this.Controls.Add(this.comboKategooria);
            this.Controls.Add(this.picturePilt);
            this.Controls.Add(this.btnValiPilt);
            this.Controls.Add(this.btnLisa);
            this.Controls.Add(this.btnUuenda);
            this.Controls.Add(this.btnKustuta);
            this.Name = "Form2";
            this.Text = "Toodete haldus";

            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTooted)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numKogus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHind)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picturePilt)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.DataGridView dataGridViewTooted;
        private System.Windows.Forms.TextBox txtNimetus;
        private System.Windows.Forms.NumericUpDown numKogus;
        private System.Windows.Forms.NumericUpDown numHind;
        private System.Windows.Forms.Label lblKogus;
        private System.Windows.Forms.Label lblHind;
        private System.Windows.Forms.ComboBox comboKategooria;
        private System.Windows.Forms.PictureBox picturePilt;
        private System.Windows.Forms.Button btnLisa;
        private System.Windows.Forms.Button btnUuenda;
        private System.Windows.Forms.Button btnKustuta;
        private System.Windows.Forms.Button btnValiPilt;
    }
}