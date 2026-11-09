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

public partial class LoginForm : Form
{
    private readonly TextBox txtEmail = new(), txtParool = new();
    private readonly RoundButton btnLogin = new();
    private readonly Label lblVeateade = new();

    public LoginForm()
    {
        InitializeComponent();
        Stiil.Vorm(this);
        BackColor = Color.White;
        Text = "Sisselogimine";
        ClientSize = new Size(820, 500);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        StartPosition = FormStartPosition.CenterScreen;

        // vasak punane külg
        var vasak = new Panel { Bounds = new Rectangle(0, 0, 340, 500), BackColor = Stiil.Punane };
        vasak.Controls.Add(new Label
        {
            Text = "Pood",
            ForeColor = Color.White,
            BackColor = Color.Transparent,
            Font = new Font("Segoe UI", 44f, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(36, 170)
        });
        vasak.Controls.Add(new Label
        {
            Text = "Värsked tooted\nkoju või järele",
            ForeColor = Color.White,
            BackColor = Color.Transparent,
            Font = new Font("Segoe UI", 15f),
            AutoSize = true,
            Location = new Point(42, 265)
        });

        // parem pool: sisselogimine
        var tervitus = new Label
        {
            Text = "Tere tulemast!",
            Font = new Font("Segoe UI", 24f, FontStyle.Bold),
            ForeColor = Stiil.Tekst,
            AutoSize = true,
            Location = new Point(396, 95)
        };
        var alapealkiri = new Label
        {
            Text = "Logi sisse oma kontole",
            Font = new Font("Segoe UI", 11f),
            ForeColor = Stiil.HallTekst,
            AutoSize = true,
            Location = new Point(400, 148)
        };

        var emailKast = Stiil.Sisend(txtEmail, 400, 200, 360, "E-post");
        var paroolKast = Stiil.Sisend(txtParool, 400, 260, 360, "Parool", true);   // näitab punkte

        lblVeateade.SetBounds(400, 312, 360, 24);
        lblVeateade.ForeColor = Color.Red;

        btnLogin.SetBounds(400, 345, 360, 46);
        btnLogin.Text = "Logi sisse";
        btnLogin.Click += btnLogin_Click;

        AcceptButton = btnLogin;   // Enter-klahv vajutab nuppu

        Controls.AddRange(new Control[] { vasak, tervitus, alapealkiri, emailKast, paroolKast, lblVeateade, btnLogin });
    }

    private void btnLogin_Click(object? sender, EventArgs e)
    {
        string email = txtEmail.Text.Trim().ToLower();

        using var db = new ToodedContext();
        var kasutaja = db.Kasutajad.FirstOrDefault(k => k.Email == email);

        if (kasutaja == null || !PasswordHelper.Verify(txtParool.Text, kasutaja.ParoolHash))
        {
            lblVeateade.Text = "Vale e-post või parool.";
            txtParool.Clear();
            return;
        }

        AktiivneKasutaja.Nimi = kasutaja.Nimi;
        AktiivneKasutaja.Roll = kasutaja.Roll;

        var menyy = new FormMain();
        menyy.FormClosed += (s, a) => Close();   // menüü sulgemine lõpetab programmi
        menyy.Show();
        Hide();
    }
}
