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
    public bool LogiValja { get; private set; }   // kasutaja vajutas "Logi välja"
    public bool Vahetatud { get; private set; }   // kasutaja vahetas kontot

    private Label lblKorvInfo = new();
    private readonly RoundButton btnKonto = new();
    private readonly ContextMenuStrip kontoMenyy = new();

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

        // vasakul üleval: konto nupp, vajutades tuleb kontode menüü
        btnKonto.Text = $"{AktiivneKasutaja.Nimi}  ▾";
        btnKonto.Bounds = new Rectangle(20, 17, 240, 36);
        btnKonto.Raadius = 18;
        btnKonto.Varv = Color.White;
        btnKonto.ForeColor = Stiil.Punane;
        btnKonto.Click += (s, e) => NaitaKontoMenyyd();
        pais.Controls.Add(btnKonto);

        pais.Controls.Add(new Label
        {
            Text = "Pood",
            ForeColor = Color.White,
            BackColor = Color.Transparent,
            Font = new Font("Segoe UI", 20f, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(290, 16)
        });
        pais.Controls.Add(new Label
        {
            Text = AktiivneKasutaja.OnAdmin ? "Administraator" : "Ostja",
            ForeColor = Color.White,
            BackColor = Color.Transparent,
            Font = new Font("Segoe UI", 11f),
            Bounds = new Rectangle(500, 22, 256, 28),
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

        // admin näeb ainult tootehaldust, tavakasutaja ainult ostja asju
        var kaardid = new List<Kaart>();
        if (AktiivneKasutaja.OnAdmin)
        {
            kaardid.Add(TeeKaart("⚙", "Tootehaldus", "Lisa, muuda ja kustuta tooteid",
                () => Ava(new Form2()), out _));
        }
        else
        {
            kaardid.Add(TeeKaart("🛍", "Pood", "Vaata tooteid ja lisa korvi",
                () => Ava(new Form3()), out _));
            kaardid.Add(TeeKaart("🧾", "Ostukorv ja arve", "",
                () => Ava(new ArveForm()), out lblKorvInfo));
        }

        Controls.Add(pais);
        Controls.Add(alguse);

        // kaardid keskele
        int kokku = kaardid.Count * 220 + (kaardid.Count - 1) * 30;
        int x = (ClientSize.Width - kokku) / 2;
        foreach (var kaart in kaardid)
        {
            kaart.Location = new Point(x, 150);
            Controls.Add(kaart);
            x += 250;
        }

        UuendaKorv();
    }

    // ---------- Kontode menüü ----------

    private void NaitaKontoMenyyd()
    {
        kontoMenyy.Items.Clear();
        kontoMenyy.Font = new Font("Segoe UI", 10.5f);

        foreach (var konto in AktiivneKasutaja.Kontod)
        {
            var valitud = konto;   // iga kliki jaoks oma koopia
            var rida = new ToolStripMenuItem($"{konto.Nimi}  ({konto.Roll})")
            {
                Checked = konto.Id == AktiivneKasutaja.Id
            };
            rida.Click += (s, e) => VahetaKonto(valitud);
            kontoMenyy.Items.Add(rida);
        }

        kontoMenyy.Items.Add(new ToolStripSeparator());
        var valja = new ToolStripMenuItem("Logi välja");
        valja.Click += (s, e) =>
        {
            Ostukorv.Tyhjenda();
            AktiivneKasutaja.LogiValja();
            LogiValja = true;
            Close();
        };
        kontoMenyy.Items.Add(valja);

        kontoMenyy.Show(btnKonto, new Point(0, btnKonto.Height));
    }

    private void VahetaKonto(Konto konto)
    {
        if (konto.Id == AktiivneKasutaja.Id) return;   // see konto on juba aktiivne

        AktiivneKasutaja.Vaheta(konto);
        Ostukorv.Tyhjenda();   // ostukorv kuulub ostjale, uuel kontol algab tühjalt
        Vahetatud = true;
        Close();               // LoginForm avab uue menüü uue kontoga
    }

    // ---------- Menüükaardid ----------

    private Kaart TeeKaart(string ikoon, string pealkiri, string alapealkiri,
                           Action ava, out Label alaSilt)
    {
        var kaart = new Kaart { Bounds = new Rectangle(0, 150, 220, 250), Raadius = 14 };

        var ikoonSilt = new Label
        {
            Text = ikoon,
            Font = new Font("Segoe UI Emoji", 36f),
            ForeColor = Stiil.Punane,
            BackColor = Color.Transparent,
            Bounds = new Rectangle(0, 35, 220, 80),
            TextAlign = ContentAlignment.MiddleCenter
        };
        var pealkiriSilt = new Label
        {
            Text = pealkiri,
            Font = new Font("Segoe UI", 14f, FontStyle.Bold),
            ForeColor = Stiil.Tekst,
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

        // punane raam kui hiir on kaardi peal
        void Uuenda() => kaart.SeaEsile(kaart.ClientRectangle.Contains(kaart.PointToClient(Cursor.Position)));

        foreach (Control c in new Control[] { kaart, ikoonSilt, pealkiriSilt, alaSilt })
        {
            c.Cursor = Cursors.Hand;
            c.MouseEnter += (s, e) => Uuenda();
            c.MouseLeave += (s, e) => Uuenda();
            c.Click += (s, e) => ava();
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
