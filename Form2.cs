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
using System.Globalization;

namespace Timur_Tooded;

public partial class Form2 : Form
{
    private static readonly Color Hall = Color.FromArgb(230, 230, 230);

    private readonly ToodedContext _db = new();
    private string? valitudPilt;

    private readonly DataGridView dataGridViewTooted = new();
    private readonly TextBox txtNimetus = new(), txtKogus = new(), txtHind = new();
    private readonly ComboBox comboKategooria = new();
    private readonly PictureBox picturePilt = new();
    private readonly RoundButton btnValiPilt = new(), btnLisa = new(), btnUuenda = new(),
                                 btnKustuta = new(), btnTyhjenda = new();

    public Form2()
    {
        InitializeComponent();
        LooLiides();
        LaeKategooriad();
        LaeTooted();

        // turvalisus: ainult admin saab muuta
        if (!AktiivneKasutaja.OnAdmin)
            btnLisa.Enabled = btnUuenda.Enabled = btnKustuta.Enabled = btnValiPilt.Enabled = false;
    }

    private void LooLiides()
    {
        Stiil.Vorm(this);
        Text = "Toodete haldus";
        ClientSize = new Size(900, 620);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        StartPosition = FormStartPosition.CenterScreen;

        // punane pealkirjariba
        var pais = new Panel { Bounds = new Rectangle(0, 0, 900, 56), BackColor = Stiil.Punane };
        pais.Controls.Add(new Label
        {
            Text = "Toodete haldus",
            ForeColor = Color.White,
            BackColor = Color.Transparent,
            Font = new Font("Segoe UI", 18f, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(15, 10)
        });

        StiiliTabel();

        // paremal: vorm
        const int x = 626, w = 258;

        var nimiKast = Stiil.Sisend(txtNimetus, x, 72, w, "Toote nimetus", varv: Color.White);
        var kogusKast = Stiil.Sisend(txtKogus, x, 126, w, "Kogus", varv: Color.White);
        var hindKast = Stiil.Sisend(txtHind, x, 180, w, "Hind €, nt 2,50", varv: Color.White);

        var katKast = new Kaart { Bounds = new Rectangle(x, 234, w, 44), Raadius = 8 };
        comboKategooria.DropDownStyle = ComboBoxStyle.DropDownList;
        comboKategooria.FlatStyle = FlatStyle.Flat;
        comboKategooria.BackColor = Color.White;
        comboKategooria.Font = new Font("Segoe UI", 11f);
        comboKategooria.SetBounds(12, 7, w - 24, 30);
        katKast.Controls.Add(comboKategooria);

        var piltKast = new Kaart { Bounds = new Rectangle(x, 288, w, 120), Raadius = 8 };
        picturePilt.Bounds = new Rectangle(8, 8, w - 16, 104);
        picturePilt.SizeMode = PictureBoxSizeMode.Zoom;
        picturePilt.BackColor = Color.Transparent;
        piltKast.Controls.Add(picturePilt);

        SeaNupp(btnValiPilt, "Vali pilt", x, 418, w, btnValiPilt_Click, hall: true);
        SeaNupp(btnLisa, "Lisa", x, 462, w, btnLisa_Click);
        SeaNupp(btnUuenda, "Uuenda", x, 506, 125, btnUuenda_Click);
        SeaNupp(btnKustuta, "Kustuta", x + 133, 506, 125, btnKustuta_Click);
        btnKustuta.Varv = Stiil.TumePunane;
        SeaNupp(btnTyhjenda, "Tühjenda väljad", x, 550, w, (s, e) => TyhjendaVorm(), hall: true);

        Controls.AddRange(new Control[] { pais, dataGridViewTooted, nimiKast, kogusKast, hindKast,
            katKast, piltKast, btnValiPilt, btnLisa, btnUuenda, btnKustuta, btnTyhjenda });
    }

    private void SeaNupp(RoundButton nupp, string tekst, int x, int y, int laius, EventHandler kasitleja, bool hall = false)
    {
        nupp.Text = tekst;
        nupp.SetBounds(x, y, laius, 38);
        if (hall)
        {
            nupp.Varv = Hall;
            nupp.ForeColor = Stiil.Tekst;
        }
        nupp.Click += kasitleja;
    }

    // tabel poe stiilis: punane päis, valged read, hele punane valik
    private void StiiliTabel()
    {
        var g = dataGridViewTooted;
        g.Bounds = new Rectangle(16, 72, 590, 532);
        g.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        g.MultiSelect = false;
        g.ReadOnly = true;
        g.AllowUserToAddRows = false;
        g.AllowUserToResizeRows = false;
        g.CellClick += dataGridViewTooted_CellClick;

        g.BorderStyle = BorderStyle.None;
        g.BackgroundColor = Stiil.Taust;
        g.GridColor = Color.FromArgb(228, 228, 228);
        g.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        g.RowHeadersVisible = false;
        g.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        g.EnableHeadersVisualStyles = false;
        g.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
        g.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        g.ColumnHeadersHeight = 38;
        g.ColumnHeadersDefaultCellStyle.BackColor = Stiil.Punane;
        g.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        g.ColumnHeadersDefaultCellStyle.SelectionBackColor = Stiil.Punane;
        g.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10f, FontStyle.Bold);

        g.RowTemplate.Height = 32;
        g.DefaultCellStyle.BackColor = Color.White;
        g.DefaultCellStyle.ForeColor = Stiil.Tekst;
        g.DefaultCellStyle.SelectionBackColor = Color.FromArgb(253, 228, 229);
        g.DefaultCellStyle.SelectionForeColor = Stiil.Tekst;
        g.DefaultCellStyle.Padding = new Padding(4, 0, 4, 0);
    }

    private void LaeKategooriad()
    {
        comboKategooria.DataSource = _db.Kategooriatabel.OrderBy(k => k.Kategooria_nimetus).ToList();
        comboKategooria.DisplayMember = "Kategooria_nimetus";
        comboKategooria.ValueMember = "Id";
    }

    private void LaeTooted()
    {
        var g = dataGridViewTooted;
        g.DataSource = _db.Toodetabel
            .Select(t => new
            {
                t.Id,
                t.Toodenimetus,
                t.Kogus,
                t.Hind,
                t.KategooriaId,
                Kategooria = t.Kategooria.Kategooria_nimetus,
                t.Pilt
            })
            .ToList();

        // tehnilised veerud peidus (andmed jäävad alles, neid loetakse koodis)
        g.Columns["Id"]!.Visible = false;
        g.Columns["KategooriaId"]!.Visible = false;
        g.Columns["Pilt"]!.Visible = false;

        g.Columns["Toodenimetus"]!.HeaderText = "Nimetus";
        g.Columns["Toodenimetus"]!.FillWeight = 220;
        g.Columns["Kogus"]!.FillWeight = 60;
        g.Columns["Hind"]!.HeaderText = "Hind (€)";
        g.Columns["Hind"]!.DefaultCellStyle.Format = "F2";
        g.Columns["Hind"]!.FillWeight = 70;
        g.Columns["Kategooria"]!.FillWeight = 110;
    }

    private void btnValiPilt_Click(object? sender, EventArgs e)
    {
        using var ofd = new OpenFileDialog { Filter = "Pildid|*.jpg;*.jpeg;*.png" };
        if (ofd.ShowDialog() != DialogResult.OK) return;

        valitudPilt = ofd.FileName;
        using var ajutine = Image.FromFile(valitudPilt);
        picturePilt.Image = new Bitmap(ajutine);
    }

    private bool LoeVorm(Toode toode)
    {
        if (string.IsNullOrWhiteSpace(txtNimetus.Text))
        { MessageBox.Show("Sisesta toote nimetus."); return false; }

        if (!int.TryParse(txtKogus.Text, out int kogus) || kogus < 0)
        { MessageBox.Show("Kogus peab olema täisarv (0 või rohkem)."); return false; }

        if (!decimal.TryParse(txtHind.Text.Replace(',', '.'), NumberStyles.Number,
                              CultureInfo.InvariantCulture, out decimal hind)
            || hind < 0)
        { MessageBox.Show("Hind peab olema arv, nt 2,50."); return false; }

        if (comboKategooria.SelectedValue is not int kategooriaId)
        { MessageBox.Show("Vali kategooria."); return false; }

        toode.Toodenimetus = txtNimetus.Text.Trim();
        toode.Kogus = kogus;
        toode.Hind = hind;
        toode.KategooriaId = kategooriaId;

        if (valitudPilt != null)
        {
            toode.Pilt = Path.GetFileName(valitudPilt);
            string siht = Path.Combine(Kaustad.Pildid, toode.Pilt);
            if (!string.Equals(Path.GetFullPath(valitudPilt), siht, StringComparison.OrdinalIgnoreCase))
                File.Copy(valitudPilt, siht, true);
        }
        return true;
    }

    private int? ValitudId()
    {
        if (dataGridViewTooted.SelectedRows.Count == 0)
        {
            MessageBox.Show("Vali tabelist toode.");
            return null;
        }
        return (int)dataGridViewTooted.SelectedRows[0].Cells["Id"].Value;
    }

    private void TyhjendaVorm()
    {
        txtNimetus.Clear();
        txtKogus.Clear();
        txtHind.Clear();
        valitudPilt = null;
        picturePilt.Image = null;
    }

    private void btnLisa_Click(object? sender, EventArgs e)
    {
        var uus = new Toode();
        if (!LoeVorm(uus)) return;

        _db.Toodetabel.Add(uus);
        _db.SaveChanges();
        TyhjendaVorm();
        LaeTooted();
    }

    private void dataGridViewTooted_CellClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        var rida = dataGridViewTooted.Rows[e.RowIndex];

        txtNimetus.Text = rida.Cells["Toodenimetus"].Value?.ToString();
        txtKogus.Text = rida.Cells["Kogus"].Value?.ToString();
        txtHind.Text = rida.Cells["Hind"].Value?.ToString();
        comboKategooria.SelectedValue = rida.Cells["KategooriaId"].Value;

        valitudPilt = null;
        picturePilt.Image = Kaustad.LaePilt(rida.Cells["Pilt"].Value?.ToString());
    }

    private void btnUuenda_Click(object? sender, EventArgs e)
    {
        if (ValitudId() is not int id) return;
        var toode = _db.Toodetabel.Find(id);
        if (toode == null || !LoeVorm(toode)) return;

        _db.SaveChanges();
        LaeTooted();
    }

    private void btnKustuta_Click(object? sender, EventArgs e)
    {
        if (ValitudId() is not int id) return;
        if (MessageBox.Show("Kas kustutan valitud toote?", "Kinnitus",
                MessageBoxButtons.YesNo) != DialogResult.Yes) return;

        var toode = _db.Toodetabel.Find(id);
        if (toode == null) return;

        _db.Toodetabel.Remove(toode);
        _db.SaveChanges();
        TyhjendaVorm();
        LaeTooted();
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        _db.Dispose();
        base.OnFormClosed(e);
    }
}