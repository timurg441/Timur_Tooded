using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Linq;

namespace Timur_Tooded
{
    public partial class Form2 : Form
    {
        private readonly ToodedContext _db;
        private string valitudPilt = "";

        public Form2()
        {
            InitializeComponent();
            _db = new ToodedContext();
            LaeKategooriad();
            LaeTooted();
        }

        private void LaeKategooriad()
        {
            if (!_db.Kategooriatabel.Any())
            {
                _db.Kategooriatabel.Add(new Kategooria { Kategooria_nimetus = "Toidutooted", Kirjeldus = "" });
                _db.Kategooriatabel.Add(new Kategooria { Kategooria_nimetus = "Joogid", Kirjeldus = "" });
                _db.Kategooriatabel.Add(new Kategooria { Kategooria_nimetus = "Maiustused", Kirjeldus = "" });
                _db.SaveChanges();
            }

            comboKategooria.DataSource = _db.Kategooriatabel.ToList();
            comboKategooria.DisplayMember = "Kategooria_nimetus";
            comboKategooria.ValueMember = "Id";
        }

        private void LaeTooted()
        {
            dataGridViewTooted.DataSource = _db.Toodetabel
                .Select(t => new
                {
                    t.Id,
                    t.Toodenimetus,
                    t.Kogus,
                    t.Hind,
                    Kategooria = t.Kategooria.Kategooria_nimetus,
                    t.Pilt
                })
                .ToList();
        }
        private string PiltideKaust()
        {
            string kaust = Path.Combine(Application.StartupPath, "Images");
            Directory.CreateDirectory(kaust);
            return kaust;
        }
        private void NaitaPilt(string failiTee)
        {
            picturePilt.Image = null;

            if (string.IsNullOrEmpty(failiTee) || !File.Exists(failiTee)) return;

            using var ms = new MemoryStream(File.ReadAllBytes(failiTee));
            picturePilt.Image = new Bitmap(Image.FromStream(ms));
        }
        private string SalvestaPilt()
        {
            if (valitudPilt == "") return "";

            string nimi = Path.GetFileName(valitudPilt);
            string uusTee = Path.Combine(PiltideKaust(), nimi);

            if (!File.Exists(uusTee))
                File.Copy(valitudPilt, uusTee);

            return nimi;
        }

        private void btnValiPilt_Click(object sender, EventArgs e)
        {
            using var ofd = new OpenFileDialog();
            ofd.Filter = "Pildid|*.jpg;*.png;*.jpeg";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                valitudPilt = ofd.FileName;
                NaitaPilt(valitudPilt);
            }
        }
        private void Tuhjenda()
        {
            txtNimetus.Text = "";
            numKogus.Value = 0;
            numHind.Value = 0;
            picturePilt.Image = null;
            valitudPilt = "";
        }

        private void btnLisa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNimetus.Text))
            {
                MessageBox.Show("Sisesta toote nimetus!");
                return;
            }

            var uus = new Toode
            {
                Toodenimetus = txtNimetus.Text,
                Kogus = (int)numKogus.Value,
                Hind = numHind.Value,
                KategooriaId = (int)comboKategooria.SelectedValue,
                Pilt = SalvestaPilt()
            };

            _db.Toodetabel.Add(uus);
            _db.SaveChanges();
            LaeTooted();
            Tuhjenda();
        }

        private void dataGridViewTooted_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            int id = (int)dataGridViewTooted.Rows[e.RowIndex].Cells["Id"].Value;
            var toode = _db.Toodetabel.Find(id);

            txtNimetus.Text = toode.Toodenimetus;
            numKogus.Value = toode.Kogus;
            numHind.Value = toode.Hind;
            comboKategooria.SelectedValue = toode.KategooriaId;

            valitudPilt = "";
            NaitaPilt(Path.Combine(PiltideKaust(), toode.Pilt ?? ""));
        }

        private void btnUuenda_Click(object sender, EventArgs e)
        {
            if (dataGridViewTooted.CurrentRow == null)
            {
                MessageBox.Show("Vali tabelist toode!");
                return;
            }

            int id = (int)dataGridViewTooted.CurrentRow.Cells["Id"].Value;
            var toode = _db.Toodetabel.Find(id);

            toode.Toodenimetus = txtNimetus.Text;
            toode.Kogus = (int)numKogus.Value;
            toode.Hind = numHind.Value;
            toode.KategooriaId = (int)comboKategooria.SelectedValue;
            if (valitudPilt != "")
                toode.Pilt = SalvestaPilt();

            _db.SaveChanges();
            LaeTooted();
            Tuhjenda();
        }

        private void btnKustuta_Click(object sender, EventArgs e)
        {
            if (dataGridViewTooted.CurrentRow == null)
            {
                MessageBox.Show("Vali tabelist toode!");
                return;
            }

            int id = (int)dataGridViewTooted.CurrentRow.Cells["Id"].Value;
            var toode = _db.Toodetabel.Find(id);

            _db.Toodetabel.Remove(toode);
            _db.SaveChanges();
            LaeTooted();
            Tuhjenda();
        }
    }
}