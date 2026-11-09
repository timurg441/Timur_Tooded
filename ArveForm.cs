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
    private readonly ListBox listOstukorv = new();
    private readonly Label lblKokku = new();
    private readonly Label lblTuhi = new();
    private readonly TextBox txtEmail = new();
    private readonly RoundButton btnEemalda = new(), btnGenPDF = new(), btnSaadaEmail = new();
    private string? viimanePdf;   // viimati loodud arve täistee

    public ArveForm()
    {
        InitializeComponent();
        Stiil.Vorm(this);
        Text = "Ostukorv ja arve";
        ClientSize = new Size(720, 480);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        StartPosition = FormStartPosition.CenterScreen;

        // punane pealkirjariba
        var pais = new Panel { Bounds = new Rectangle(0, 0, 720, 56), BackColor = Stiil.Punane };
        pais.Controls.Add(new Label
        {
            Text = "Ostukorv ja arve",
            ForeColor = Color.White,
            BackColor = Color.Transparent,
            Font = new Font("Segoe UI", 18f, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(15, 10)
        });

        // valge kaart, mille sees on ostukorvi nimekiri
        var kaart = new Kaart { Bounds = new Rectangle(20, 76, 440, 330), Raadius = 12 };

        listOstukorv.SetBounds(10, 10, 420, 310);
        listOstukorv.BorderStyle = BorderStyle.None;
        listOstukorv.BackColor = Color.White;
        listOstukorv.DrawMode = DrawMode.OwnerDrawFixed;
        listOstukorv.ItemHeight = 36;
        listOstukorv.DrawItem += listOstukorv_DrawItem;

        lblTuhi.Text = "Ostukorv on tühi";
        lblTuhi.ForeColor = Stiil.HallTekst;
        lblTuhi.Font = new Font("Segoe UI", 12f);
        lblTuhi.TextAlign = ContentAlignment.MiddleCenter;
        lblTuhi.SetBounds(10, 10, 420, 310);

        kaart.Controls.Add(lblTuhi);
        kaart.Controls.Add(listOstukorv);

        // summa suurelt punaselt
        lblKokku.SetBounds(20, 420, 440, 40);
        lblKokku.Font = new Font("Segoe UI", 20f, FontStyle.Bold);
        lblKokku.ForeColor = Stiil.Punane;

        var emailKast = Stiil.Sisend(txtEmail, 480, 76, 220, "Saaja e-post", false, Color.White);

        SeaNupp(btnEemalda, "Eemalda valitud rida", 144, btnEemalda_Click);
        btnEemalda.Varv = Color.White;          // teisejärguline nupp: valge
        btnEemalda.ForeColor = Stiil.Tekst;
        SeaNupp(btnGenPDF, "Genereeri PDF", 214, btnGenPDF_Click);
        SeaNupp(btnSaadaEmail, "Saada e-postiga", 270, btnSaadaEmail_Click);

        Controls.AddRange(new Control[] { pais, kaart, lblKokku, emailKast,
            btnEemalda, btnGenPDF, btnSaadaEmail });

        NaitaKorvi();
    }

    private void SeaNupp(RoundButton nupp, string tekst, int y, EventHandler kasitleja)
    {
        nupp.Text = tekst;
        nupp.SetBounds(480, y, 220, 44);
        nupp.Click += kasitleja;
    }

    // iga rida joonistatakse ise: valitud rida on heleroosa
    private void listOstukorv_DrawItem(object? sender, DrawItemEventArgs e)
    {
        if (e.Index < 0) return;

        bool valitud = (e.State & DrawItemState.Selected) != 0;
        using (var taust = new SolidBrush(valitud ? Color.FromArgb(253, 226, 227) : Color.White))
            e.Graphics.FillRectangle(taust, e.Bounds);

        TextRenderer.DrawText(e.Graphics, listOstukorv.Items[e.Index].ToString(), Font,
            new Rectangle(e.Bounds.X + 8, e.Bounds.Y, e.Bounds.Width - 8, e.Bounds.Height),
            Stiil.Tekst, TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);

        // õhuke joon ridade vahele
        using var joon = new Pen(Color.FromArgb(235, 235, 235));
        e.Graphics.DrawLine(joon, e.Bounds.Left, e.Bounds.Bottom - 1, e.Bounds.Right, e.Bounds.Bottom - 1);
    }

    private void NaitaKorvi()
    {
        listOstukorv.Items.Clear();
        foreach (var rida in Ostukorv.Ridad)
            listOstukorv.Items.Add(rida);   // ListBox kuvab OstukorviRida.ToString()
        lblKokku.Text = $"Kokku: {Ostukorv.Kokku:F2} €";
        lblTuhi.Visible = Ostukorv.Ridad.Count == 0;
    }

    private void btnEemalda_Click(object? sender, EventArgs e)
    {
        if (listOstukorv.SelectedItem is OstukorviRida rida)
        {
            Ostukorv.Ridad.Remove(rida);
            NaitaKorvi();
        }
    }

    // PDF
    private void btnGenPDF_Click(object? sender, EventArgs e)
    {
        if (Ostukorv.Ridad.Count == 0)
        {
            MessageBox.Show("Ostukorv on tühi.");
            return;
        }

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

            double y = 130;
            foreach (var rida in Ostukorv.Ridad)
            {
                gfx.DrawString(rida.ToString(), tavaline, XBrushes.Black, 40, y);
                y += 22;
            }
            gfx.DrawString($"Kokku: {Ostukorv.Kokku:F2} €", paks, XBrushes.Black, 40, y + 20);
        }

        dokument.Save(viimanePdf);
        MessageBox.Show($"PDF on loodud:\n{viimanePdf}");
    }

    // E-post
    private void btnSaadaEmail_Click(object? sender, EventArgs e)
    {
        if (viimanePdf == null || !File.Exists(viimanePdf))
        {
            MessageBox.Show("Genereeri enne PDF.");
            return;
        }

        string saaja = txtEmail.Text.Trim();
        if (!MailAddress.TryCreate(saaja, out _))
        {
            MessageBox.Show("Sisesta korrektne e-posti aadress.");
            return;
        }

        const string saatja = "1ame.xyz07@gmail.com";
        const string rakenduseParool = "emez sqqn rvcf ktqi";

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
            Ostukorv.Tyhjenda();
            NaitaKorvi();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Viga: " + ex.Message);
        }
    }
}
