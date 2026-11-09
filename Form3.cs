using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Microsoft.EntityFrameworkCore;

namespace Timur_Tooded;

public partial class Form3 : Form
{
    private readonly ToodedContext _db = new();
    private readonly FlowLayoutPanel kategooriaRiba = new();
    private readonly FlowLayoutPanel tootedPaneel = new();
    private readonly RoundButton btnKorv = new();
    private Kategooria? valitudKat;   // praegu valitud kategooria (null = kõik)
    private List<Kategooria> kategooriad = new();

    // otsing ja sorteerimine
    private readonly TextBox txtOtsing = new();
    private readonly RoundButton btnSort = new();
    private int sortRezhiim;   // 0 = nime järgi, 1 = odavamast, 2 = kallimast

    // teade "toode lisati ostukorvi"
    private readonly Kaart teade = new();
    private readonly Label lblTeade = new();
    private readonly System.Windows.Forms.Timer teateTaimer = new() { Interval = 2200 };

    public Form3()
    {
        InitializeComponent();
        Stiil.Vorm(this);
        Text = "Pood";
        ClientSize = new Size(990, 650);

        // toodete ala (täidab ülejäänud koha, seepärast lisatakse esimesena)
        tootedPaneel.Dock = DockStyle.Fill;
        tootedPaneel.AutoScroll = true;
        tootedPaneel.Padding = new Padding(10);
        tootedPaneel.BackColor = Stiil.Taust;
        Controls.Add(tootedPaneel);

        // kategooriate nupud
        kategooriaRiba.Dock = DockStyle.Top;
        kategooriaRiba.Height = 58;
        kategooriaRiba.Padding = new Padding(10, 12, 10, 0);
        kategooriaRiba.BackColor = Stiil.Taust;
        Controls.Add(kategooriaRiba);

        // otsing + sorteerimine (kategooriate riba kohal)
        var otsinguRiba = new Panel { Dock = DockStyle.Top, Height = 62, BackColor = Stiil.Taust };
        var otsingKast = Stiil.Sisend(txtOtsing, 10, 9, 420, "Otsi toodet nime järgi", varv: Color.White);
        txtOtsing.TextChanged += (s, e) => NaitaTooted();   // tulemused uuenevad trükkimise ajal

        btnSort.Bounds = new Rectangle(442, 9, 230, 44);
        btnSort.Raadius = 8;
        btnSort.Click += (s, e) =>
        {
            sortRezhiim = (sortRezhiim + 1) % 3;   // sorteerimata -> odavamast -> kallimast
            UuendaSortNupp();
            NaitaTooted();
        };
        otsinguRiba.Controls.AddRange(new Control[] { otsingKast, btnSort });
        UuendaSortNupp();
        Controls.Add(otsinguRiba);

        // punane riba üleval
        var pais = new Panel { Dock = DockStyle.Top, Height = 56, BackColor = Stiil.Punane };
        pais.Controls.Add(new Label
        {
            Text = "Pood",
            ForeColor = Color.White,
            BackColor = Color.Transparent,
            Font = new Font("Segoe UI", 18f, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(15, 10)
        });
        // ostukorvi nupp: avab korvi, seal saab ostu kinnitada
        btnKorv.Text = "Ostukorv";
        btnKorv.Size = new Size(270, 36);
        btnKorv.Top = 10;
        btnKorv.Raadius = 18;
        btnKorv.Varv = Color.White;
        btnKorv.ForeColor = Stiil.Punane;
        btnKorv.Click += btnKorv_Click;
        pais.Controls.Add(btnKorv);

        // nupp jääb paremale serva, ka kui aken muutub
        pais.Resize += (s, e) => btnKorv.Left = pais.Width - btnKorv.Width - 15;
        Controls.Add(pais);
        pais.Width = ClientSize.Width;   // käivitab Resize ja paneb nupu õigesse kohta

        // teade paremal all, peidetud kuni vaja
        teade.Size = new Size(420, 54);
        teade.Location = new Point(ClientSize.Width - 440, ClientSize.Height - 74);
        teade.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        teade.Visible = false;
        lblTeade.Bounds = new Rectangle(14, 0, 392, 54);
        lblTeade.Font = new Font("Segoe UI", 11f, FontStyle.Bold);
        lblTeade.ForeColor = Color.White;
        lblTeade.BackColor = Color.Transparent;
        lblTeade.TextAlign = ContentAlignment.MiddleLeft;
        lblTeade.AutoEllipsis = true;
        teade.Controls.Add(lblTeade);
        Controls.Add(teade);

        teateTaimer.Tick += (s, e) => { teateTaimer.Stop(); teade.Visible = false; };

        LaeKategooriad();
        UuendaKorv();
        ValiKategooria(null);   // alguses näidatakse kõiki tooteid
    }

    private void LaeKategooriad()
    {
        kategooriad = _db.Kategooriatabel
            .Include(k => k.Tooded)
            .OrderBy(k => k.Kategooria_nimetus)
            .ToList();
    }

    private void btnKorv_Click(object? sender, EventArgs e)
    {
        using (var arve = new ArveForm())
            arve.ShowDialog();

        // laoseis võis ostu tõttu muutuda, seepärast loeme andmed uuesti
        _db.ChangeTracker.Clear();
        LaeKategooriad();
        int? id = valitudKat?.Id;
        ValiKategooria(kategooriad.FirstOrDefault(k => k.Id == id));
        UuendaKorv();
    }

    // näitab teadet 2 sekundit (roheline = õnnestus, punane = viga)
    private void NaitaTeadet(string tekst, bool viga)
    {
        Color varv = viga ? Stiil.TumePunane : Color.FromArgb(46, 158, 79);
        teade.Taustavarv = varv;
        teade.Raamivarv = varv;
        lblTeade.Text = tekst;
        teade.Visible = true;
        teade.BringToFront();
        teade.Invalidate();

        teateTaimer.Stop();
        teateTaimer.Start();   // iga uus teade alustab aja otsast
    }

    private void UuendaKorv()
    {
        int tk = Ostukorv.Ridad.Sum(r => r.Kogus);
        btnKorv.Text = tk == 0 ? "Ostukorv" : $"Ostukorv  ({tk} tk  •  {Ostukorv.Kokku:F2} €)";
        btnKorv.Invalidate();
    }

    // sorteerimisnupu tekst ja värv: aktiivne sorteerimine on punane
    private void UuendaSortNupp()
    {
        btnSort.Text = sortRezhiim switch
        {
            1 => "Hind: odavamast ↑",
            2 => "Hind: kallimast ↓",
            _ => "Sorteeri hinna järgi"
        };
        btnSort.Varv = sortRezhiim == 0 ? Color.White : Stiil.Punane;
        btnSort.ForeColor = sortRezhiim == 0 ? Stiil.Tekst : Color.White;
        btnSort.Invalidate();
    }

    // null = kõik tooted
    private void ValiKategooria(Kategooria? valitud)
    {
        valitudKat = valitud;

        // kategooriate nupud: valitud on punane, ülejäänud valged
        while (kategooriaRiba.Controls.Count > 0)
            kategooriaRiba.Controls[0].Dispose();
        kategooriaRiba.Controls.Add(LooNupp("Kõik", valitud == null, () => ValiKategooria(null)));
        foreach (var kat in kategooriad)
        {
            var k = kat;
            kategooriaRiba.Controls.Add(LooNupp(k.Kategooria_nimetus, valitud?.Id == k.Id, () => ValiKategooria(k)));
        }

        NaitaTooted();
    }

    // joonistab tooted: kategooria + otsing + sorteerimine
    private void NaitaTooted()
    {
        tootedPaneel.SuspendLayout();

        // vanad kaardid ära
        foreach (Control c in tootedPaneel.Controls)
            foreach (var pb in c.Controls.OfType<PictureBox>())
                pb.Image?.Dispose();
        while (tootedPaneel.Controls.Count > 0)
            tootedPaneel.Controls[0].Dispose();

        IEnumerable<Toode> tooted = valitudKat == null
            ? kategooriad.SelectMany(k => k.Tooded)
            : valitudKat.Tooded;

        string otsing = txtOtsing.Text.Trim();
        if (otsing.Length > 0)
            tooted = tooted.Where(t => t.Toodenimetus.Contains(otsing, StringComparison.CurrentCultureIgnoreCase));

        tooted = sortRezhiim switch
        {
            1 => tooted.OrderBy(t => t.Hind).ThenBy(t => t.Toodenimetus),
            2 => tooted.OrderByDescending(t => t.Hind).ThenBy(t => t.Toodenimetus),
            _ => tooted.OrderBy(t => t.Toodenimetus)
        };

        var nimekiri = tooted.ToList();
        foreach (var toode in nimekiri)
            tootedPaneel.Controls.Add(LooTootekaart(toode));

        if (nimekiri.Count == 0)
        {
            tootedPaneel.Controls.Add(new Label
            {
                Text = "Midagi ei leitud",
                AutoSize = true,
                Font = new Font("Segoe UI", 14f),
                ForeColor = Stiil.HallTekst,
                Margin = new Padding(10)
            });
        }

        tootedPaneel.ResumeLayout();
    }

    private RoundButton LooNupp(string tekst, bool valitud, Action klikk)
    {
        var nupp = new RoundButton
        {
            Text = tekst,
            Size = new Size(120, 34),
            Raadius = 17,
            Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
            Varv = valitud ? Stiil.Punane : Color.White,
            ForeColor = valitud ? Color.White : Stiil.Tekst,
            Margin = new Padding(0, 0, 8, 0)
        };
        nupp.Click += (s, e) => klikk();
        return nupp;
    }

    private Kaart LooTootekaart(Toode t)
    {
        var kaart = new Kaart { Size = new Size(175, 260), Margin = new Padding(8) };

        var pilt = new PictureBox
        {
            Bounds = new Rectangle(10, 10, 155, 130),
            SizeMode = PictureBoxSizeMode.Zoom,
            BackColor = Color.Transparent,
            Image = Kaustad.LaePilt(t.Pilt)
        };

        var nimi = new Label
        {
            Bounds = new Rectangle(10, 145, 155, 36),
            Text = t.Toodenimetus,
            BackColor = Color.Transparent
        };

        // hind suurelt ja punaselt
        var hind = new Label
        {
            Bounds = new Rectangle(10, 183, 155, 28),
            Text = $"{t.Hind:F2} €",
            ForeColor = Stiil.Punane,
            BackColor = Color.Transparent,
            Font = new Font("Segoe UI", 13f, FontStyle.Bold)
        };

        var kogus = new NumericUpDown
        {
            Bounds = new Rectangle(10, 221, 60, 27),
            Minimum = 1,
            Maximum = Math.Max(1, t.Kogus),   // rohkem kui laos ei saa valida
            Value = 1
        };

        var nupp = new RoundButton
        {
            Bounds = new Rectangle(78, 218, 87, 34),
            Text = t.Kogus > 0 ? "Lisa korvi" : "Otsas",
            Enabled = t.Kogus > 0
        };
        nupp.Click += (s, e) =>
        {
            int juba = Ostukorv.Ridad.FirstOrDefault(r => r.Toode.Id == t.Id)?.Kogus ?? 0;
            int soovitud = (int)kogus.Value;

            // rohkem kui laos on, ei saa ostukorvi panna
            if (juba + soovitud > t.Kogus)
            {
                NaitaTeadet($"Laos on ainult {t.Kogus} tk (korvis juba {juba})", true);
                return;
            }

            Ostukorv.Lisa(t, soovitud);
            UuendaKorv();   // ostukorvi summa ülal uueneb
            NaitaTeadet($"✓ {t.Toodenimetus} ({soovitud} tk) lisati ostukorvi", false);
            kogus.Value = 1;
        };

        pilt.Click += (s, e) =>
            MessageBox.Show($"{t.Toodenimetus}\nHind: {t.Hind:F2} €\nLaos: {t.Kogus} tk");

        kaart.Controls.AddRange(new Control[] { pilt, nimi, hind, kogus, nupp });
        return kaart;
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        teateTaimer.Dispose();
        _db.Dispose();
        base.OnFormClosed(e);
    }
}