using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;

namespace Timur_Tooded;

public partial class ArveForm : Form
{
    private readonly FlowLayoutPanel flowKorv = new();   // siin on ostukorvi read
    private readonly Label lblTuhi = new();
    private readonly Label lblKokku = new();
    private readonly Label lblOlek = new();
    private readonly TextBox txtEmail = new();
    private readonly RoundButton btnKinnita = new(), btnTuhjenda = new(), btnAva = new(), btnSaadaEmail = new();
    private string? viimanePdf;     // viimati loodud arve täistee
    private bool ostuTehtud;        // kas ost on juba kinnitatud
    private decimal makstud;        // kinnitatud ostu summa

    public ArveForm()
    {
        InitializeComponent();
        Stiil.Vorm(this);
        Text = "Ostukorv";
        ClientSize = new Size(860, 540);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        StartPosition = FormStartPosition.CenterScreen;

        // punane pealkirjariba
        var pais = new Panel { Bounds = new Rectangle(0, 0, 860, 56), BackColor = Stiil.Punane };
        pais.Controls.Add(new Label
        {
            Text = "Ostukorv",
            ForeColor = Color.White,
            BackColor = Color.Transparent,
            Font = new Font("Segoe UI", 18f, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(15, 10)
        });

        // vasakul: kõik ostukorvi read allapoole
        flowKorv.Bounds = new Rectangle(20, 76, 505, 440);
        flowKorv.FlowDirection = FlowDirection.TopDown;
        flowKorv.WrapContents = false;
        flowKorv.AutoScroll = true;
        flowKorv.BackColor = Stiil.Taust;

        lblTuhi.Bounds = flowKorv.Bounds;
        lblTuhi.BackColor = Stiil.Taust;
        lblTuhi.Font = new Font("Segoe UI", 14f);
        lblTuhi.TextAlign = ContentAlignment.MiddleCenter;

        // paremal: summa ja nupud
        var kokkuKaart = new Kaart { Bounds = new Rectangle(545, 76, 295, 440), Raadius = 12 };

        kokkuKaart.Controls.Add(new Label
        {
            Text = "Kokku maksta",
            ForeColor = Stiil.HallTekst,
            BackColor = Color.Transparent,
            AutoSize = true,
            Location = new Point(20, 18)
        });

        lblKokku.Bounds = new Rectangle(20, 40, 255, 44);
        lblKokku.BackColor = Color.Transparent;
        lblKokku.Font = new Font("Segoe UI", 22f, FontStyle.Bold);
        lblKokku.ForeColor = Stiil.Punane;
        kokkuKaart.Controls.Add(lblKokku);

        SeaNupp(kokkuKaart, btnKinnita, "Kinnita ost ja loo arve", 95, 48, btnKinnita_Click);
        SeaNupp(kokkuKaart, btnTuhjenda, "Tühjenda ostukorv", 151, 38, btnTuhjenda_Click);
        btnTuhjenda.Varv = Color.FromArgb(240, 240, 240);   // teisejärguline nupp: hall
        btnTuhjenda.ForeColor = Stiil.Tekst;

        kokkuKaart.Controls.Add(new Label
        {
            Text = "Pärast ostu kinnitamist:",
            ForeColor = Stiil.HallTekst,
            BackColor = Color.Transparent,
            AutoSize = true,
            Location = new Point(20, 215)
        });

        SeaNupp(kokkuKaart, btnAva, "Ava arve (PDF)", 240, 40, btnAva_Click);
        btnAva.Varv = Color.FromArgb(240, 240, 240);
        btnAva.ForeColor = Stiil.Tekst;

        var emailKast = Stiil.Sisend(txtEmail, 20, 290, 255, "Saaja e-post");
        emailKast.BackColor = Color.White;   // nurkade taust = kaardi värv
        kokkuKaart.Controls.Add(emailKast);

        SeaNupp(kokkuKaart, btnSaadaEmail, "Saada e-postiga", 346, 44, btnSaadaEmail_Click);

        btnAva.Enabled = false;
        btnSaadaEmail.Enabled = false;

        lblOlek.Bounds = new Rectangle(20, 398, 255, 30);
        lblOlek.BackColor = Color.Transparent;
        lblOlek.ForeColor = Color.FromArgb(46, 158, 79);
        lblOlek.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
        kokkuKaart.Controls.Add(lblOlek);

        Controls.AddRange(new Control[] { pais, flowKorv, lblTuhi, kokkuKaart });
        lblTuhi.BringToFront();

        NaitaKorvi();
    }

    private void SeaNupp(Control vanem, RoundButton nupp, string tekst, int y, int korgus, EventHandler kasitleja)
    {
        nupp.Text = tekst;
        nupp.SetBounds(20, y, 255, korgus);
        nupp.Click += kasitleja;
        vanem.Controls.Add(nupp);
    }

    // ---------- Ostukorvi read ----------

    // joonistab kõik read uuesti (kasutatakse alguses ja kui rida kustutatakse)
    private void NaitaKorvi()
    {
        foreach (Control c in flowKorv.Controls)
            foreach (var pb in c.Controls.OfType<PictureBox>())
                pb.Image?.Dispose();
        while (flowKorv.Controls.Count > 0)
            flowKorv.Controls[0].Dispose();

        foreach (var rida in Ostukorv.Ridad)
            flowKorv.Controls.Add(LooRida(rida));

        UuendaKokku();
    }

    // üks rida: pilt, nimi, hind, - kogus +, summa, kustuta
    private Kaart LooRida(OstukorviRida rida)
    {
        var kaart = new Kaart { Size = new Size(480, 76), Margin = new Padding(0, 0, 0, 8), Raadius = 10 };

        var pilt = new PictureBox
        {
            Bounds = new Rectangle(10, 8, 60, 60),
            SizeMode = PictureBoxSizeMode.Zoom,
            BackColor = Color.Transparent,
            Image = Kaustad.LaePilt(rida.Toode.Pilt)
        };
        var nimi = new Label
        {
            Bounds = new Rectangle(80, 8, 170, 36),
            Text = rida.Toode.Toodenimetus,
            Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
            BackColor = Color.Transparent,
            AutoEllipsis = true
        };
        var hind = new Label
        {
            Bounds = new Rectangle(80, 46, 170, 20),
            Text = $"{rida.Toode.Hind:F2} € / tk",
            ForeColor = Stiil.HallTekst,
            BackColor = Color.Transparent
        };

        var btnMiinus = LooMiniNupp("−", 255);
        var btnPlus = LooMiniNupp("+", 319);
        var btnKustuta = LooMiniNupp("✕", 442);

        var lblKogus = new Label
        {
            Bounds = new Rectangle(287, 23, 30, 30),
            TextAlign = ContentAlignment.MiddleCenter,
            Font = new Font("Segoe UI", 11f, FontStyle.Bold),
            BackColor = Color.Transparent
        };
        var lblSumma = new Label
        {
            Bounds = new Rectangle(352, 23, 82, 30),
            TextAlign = ContentAlignment.MiddleRight,
            Font = new Font("Segoe UI", 11f, FontStyle.Bold),
            ForeColor = Stiil.Punane,
            BackColor = Color.Transparent
        };

        // uuendab selle rea numbrid ja nuppude olekud
        void Uuenda()
        {
            lblKogus.Text = rida.Kogus.ToString();
            lblSumma.Text = $"{rida.Summa:F2} €";
            btnMiinus.Enabled = rida.Kogus > 1;                  // alla 1 ei saa
            btnPlus.Enabled = rida.Kogus < rida.Toode.Kogus;     // rohkem kui laos on, ei saa
            UuendaKokku();
        }

        btnMiinus.Click += (s, e) => { rida.Kogus--; Uuenda(); };
        btnPlus.Click += (s, e) => { rida.Kogus++; Uuenda(); };
        btnKustuta.Click += (s, e) => { Ostukorv.Ridad.Remove(rida); NaitaKorvi(); };

        kaart.Controls.AddRange(new Control[] { pilt, nimi, hind, btnMiinus, lblKogus, btnPlus, lblSumma, btnKustuta });
        Uuenda();
        return kaart;
    }

    private RoundButton LooMiniNupp(string tekst, int x)
    {
        return new RoundButton
        {
            Text = tekst,
            Bounds = new Rectangle(x, 23, 30, 30),
            Raadius = 15,
            Varv = Color.FromArgb(240, 240, 240),
            ForeColor = Stiil.Tekst,
            Font = new Font("Segoe UI", 11f, FontStyle.Bold)
        };
    }

    // kogusumma ja nuppude olekud
    private void UuendaKokku()
    {
        bool onRidu = Ostukorv.Ridad.Count > 0;

        lblKokku.Text = ostuTehtud ? $"{makstud:F2} €" : $"{Ostukorv.Kokku:F2} €";
        btnKinnita.Enabled = onRidu && !ostuTehtud;
        btnTuhjenda.Enabled = onRidu && !ostuTehtud;

        lblTuhi.Visible = !onRidu;
        lblTuhi.Text = ostuTehtud ? "✓ Ost kinnitatud!\nArve on valmis." : "Ostukorv on tühi";
        lblTuhi.ForeColor = ostuTehtud ? Color.FromArgb(46, 158, 79) : Stiil.HallTekst;
    }

    private void btnTuhjenda_Click(object? sender, EventArgs e)
    {
        Ostukorv.Tyhjenda();
        NaitaKorvi();
    }

    // ---------- Ost ----------

    // kontrollime laoseisu, teeme PDF-i ja vähendame laoseisu
    private void btnKinnita_Click(object? sender, EventArgs e)
    {
        if (Ostukorv.Ridad.Count == 0)
        {
            MessageBox.Show("Ostukorv on tühi.");
            return;
        }

        decimal kokku = Ostukorv.Kokku;

        try
        {
            using var db = new ToodedContext();

            // 1) kas laos jagub kõike?
            foreach (var rida in Ostukorv.Ridad)
            {
                var toode = db.Toodetabel.Find(rida.Toode.Id);
                if (toode == null || toode.Kogus < rida.Kogus)
                {
                    MessageBox.Show($"{rida.Toode.Toodenimetus}: laos pole piisavalt!");
                    return;
                }
            }

            // 2) arve PDF
            LooPdf();

            // 3) laoseis väheneb
            foreach (var rida in Ostukorv.Ridad)
                db.Toodetabel.Find(rida.Toode.Id)!.Kogus -= rida.Kogus;
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Viga: " + ex.Message);
            return;
        }

        // ost on tehtud: korv tühjaks, nüüd saab arve avada või saata
        ostuTehtud = true;
        makstud = kokku;
        Ostukorv.Tyhjenda();
        NaitaKorvi();

        btnAva.Enabled = true;
        btnSaadaEmail.Enabled = true;
        lblOlek.Text = "✓ Ost kinnitatud!";
    }

    // PDF
    private void LooPdf()
    {
        viimanePdf = Path.Combine(Kaustad.Arved, $"Arve_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");

        using var dokument = new PdfDocument();
        PdfPage leht = dokument.AddPage();

        using (XGraphics gfx = XGraphics.FromPdfPage(leht))
        {
            var pealkiri = new XFont("Arial", 20, XFontStyle.Bold);
            var tavaline = new XFont("Arial", 12);
            var paks = new XFont("Arial", 14, XFontStyle.Bold);

            gfx.DrawString("ARVE", pealkiri, XBrushes.Black,
                new XRect(0, 30, leht.Width, 30), XStringFormats.TopCenter);
            gfx.DrawString($"Kuupäev: {DateTime.Now:dd.MM.yyyy}", tavaline, XBrushes.Black, 40, 90);
            gfx.DrawString($"Ostja: {AktiivneKasutaja.Nimi}", tavaline, XBrushes.Black, 40, 112);

            double y = 150;
            foreach (var rida in Ostukorv.Ridad)
            {
                gfx.DrawString(rida.ToString(), tavaline, XBrushes.Black, 40, y);
                y += 22;
            }
            gfx.DrawString($"Kokku: {Ostukorv.Kokku:F2} €", paks, XBrushes.Black, 40, y + 20);
        }

        dokument.Save(viimanePdf);
    }

    // arve avamine vaikimisi PDF-programmis
    private void btnAva_Click(object? sender, EventArgs e)
    {
        if (viimanePdf == null || !File.Exists(viimanePdf))
        {
            MessageBox.Show("Arvet ei leitud.");
            return;
        }

        System.Diagnostics.Process.Start(
            new System.Diagnostics.ProcessStartInfo(viimanePdf) { UseShellExecute = true });
    }

    // E-post
    private void btnSaadaEmail_Click(object? sender, EventArgs e)
    {
        if (viimanePdf == null || !File.Exists(viimanePdf))
        {
            MessageBox.Show("Arvet ei leitud.");
            return;
        }

        string saaja = txtEmail.Text.Trim();
        if (!MailAddress.TryCreate(saaja, out _))
        {
            MessageBox.Show("Sisesta korrektne e-posti aadress.");
            return;
        }

        const string saatja = "1ame.xyz07@gmail.com";
        const string rakenduseParool = "";

        try
        {
            // using vabastab manuse, muidu jääb PDF-fail lukku
            using var kiri = new MailMessage(saatja, saaja, "Arve", "Arve on manuses.");
            kiri.Attachments.Add(new Attachment(viimanePdf));

            using var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential(saatja, rakenduseParool),
                EnableSsl = true
            };
            smtp.Send(kiri);

            MessageBox.Show("Arve saadetud!");
            Close();   // tagasi poodi
        }
        catch (Exception ex)
        {
            MessageBox.Show("Viga: " + ex.Message);
        }
    }
}
