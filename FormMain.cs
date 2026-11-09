using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Timur_Tooded;

public partial class FormMain : Form
{
    private Label lblKorvInfo = new();

    public FormMain()
    {
        InitializeComponent();
        Stiil.Vorm(this);
        Text = $"Peamenüü – {AktiivneKasutaja.Nimi}";
        ClientSize = new Size(780, 440);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        StartPosition = FormStartPosition.CenterScreen;

        // punane pealkirjariba
        var pais = new Panel { Bounds = new Rectangle(0, 0, 780, 70), BackColor = Stiil.Punane };
        pais.Controls.Add(new Label
        {
            Text = "Pood",
            ForeColor = Color.White,
            BackColor = Color.Transparent,
            Font = new Font("Segoe UI", 20f, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(24, 16)
        });
        pais.Controls.Add(new Label
        {
            Text = $"Tere, {AktiivneKasutaja.Nimi}  •  {AktiivneKasutaja.Roll}",
            ForeColor = Color.White,
            BackColor = Color.Transparent,
            Font = new Font("Segoe UI", 11f),
            Bounds = new Rectangle(380, 22, 376, 28),
            TextAlign = ContentAlignment.MiddleRight
        });

        var alguse = new Label
        {
            Text = "Mida soovid teha?",
            Font = new Font("Segoe UI", 16f, FontStyle.Bold),
            ForeColor = Stiil.Tekst,
            AutoSize = true,
            Location = new Point(30, 92)
        };

        // kolm suurt kaarti
        var pood = TeeKaart("🛍", "Pood", "Vaata tooteid ja lisa korvi", 30, true, () => Ava(new Form3()), out _);
        var arve = TeeKaart("🧾", "Ostukorv ja arve", "", 280, true, () => Ava(new ArveForm()), out lblKorvInfo);
        var haldus = TeeKaart("⚙", "Tootehaldus",
            AktiivneKasutaja.OnAdmin ? "Lisa, muuda ja kustuta tooteid" : "Ainult administraatorile",
            530, AktiivneKasutaja.OnAdmin, () => Ava(new Form2()), out _);

        Controls.AddRange(new Control[] { pais, alguse, pood, arve, haldus });

        UuendaKorv();
    }

    // üks menüükaart; lubatud=false -> hall ja ei reageeri
    private Kaart TeeKaart(string ikoon, string pealkiri, string alapealkiri, int x, bool lubatud,
                           Action ava, out Label alaSilt)
    {
        var kaart = new Kaart { Bounds = new Rectangle(x, 150, 220, 250), Raadius = 14 };
        if (!lubatud) kaart.Taustavarv = Color.FromArgb(235, 235, 235);

        var ikoonSilt = new Label
        {
            Text = ikoon,
            Font = new Font("Segoe UI Emoji", 36f),
            ForeColor = lubatud ? Stiil.Punane : Color.Gray,
            BackColor = Color.Transparent,
            Bounds = new Rectangle(0, 35, 220, 80),
            TextAlign = ContentAlignment.MiddleCenter
        };
        var pealkiriSilt = new Label
        {
            Text = pealkiri,
            Font = new Font("Segoe UI", 14f, FontStyle.Bold),
            ForeColor = lubatud ? Stiil.Tekst : Color.Gray,
            BackColor = Color.Transparent,
            Bounds = new Rectangle(0, 125, 220, 32),
            TextAlign = ContentAlignment.MiddleCenter
        };
        alaSilt = new Label
        {
            Text = alapealkiri,
            Font = new Font("Segoe UI", 10f),
            ForeColor = Stiil.HallTekst,
            BackColor = Color.Transparent,
            Bounds = new Rectangle(15, 162, 190, 60),
            TextAlign = ContentAlignment.TopCenter
        };

        kaart.Controls.AddRange(new Control[] { ikoonSilt, pealkiriSilt, alaSilt });

        if (lubatud)
        {
            // punane raam kui hiir on kaardi peal
            void Uuenda() => kaart.SeaEsile(kaart.ClientRectangle.Contains(kaart.PointToClient(Cursor.Position)));

            foreach (Control c in new Control[] { kaart, ikoonSilt, pealkiriSilt, alaSilt })
            {
                c.Cursor = Cursors.Hand;
                c.MouseEnter += (s, e) => Uuenda();
                c.MouseLeave += (s, e) => Uuenda();
                c.Click += (s, e) => ava();
            }
        }
        return kaart;
    }

    private void UuendaKorv()
    {
        int tk = Ostukorv.Ridad.Sum(r => r.Kogus);
        lblKorvInfo.Text = tk == 0 ? "Ostukorv on tühi" : $"Korvis {tk} tk\nKokku {Ostukorv.Kokku:F2} €";
    }

    private void Ava(Form vorm)
    {
        using (vorm)   // vorm vabastatakse pärast sulgemist
            vorm.ShowDialog();

        UuendaKorv();   // korv võis muutuda
    }
}
