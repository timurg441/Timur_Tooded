using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;

namespace Timur_Tooded;

// Ühine disain: värvid, ümarad nupud ja kaardid
public static class Stiil
{
    public static readonly Color Punane = ColorTranslator.FromHtml("#F01319");
    public static readonly Color TumePunane = ColorTranslator.FromHtml("#C70F14");
    public static readonly Color Taust = ColorTranslator.FromHtml("#F5F5F5");
    public static readonly Color Tekst = ColorTranslator.FromHtml("#333333");
    public static readonly Color HallTekst = ColorTranslator.FromHtml("#777777");

    // vormi taust, värv ja font
    public static void Vorm(Form vorm)
    {
        vorm.BackColor = Taust;
        vorm.ForeColor = Tekst;
        vorm.Font = new Font("Segoe UI", 9.5f);
    }

    // ümardatud nurkadega kujund
    public static GraphicsPath Umar(Rectangle r, int raadius)
    {
        int d = raadius * 2;
        var tee = new GraphicsPath();
        tee.AddArc(r.X, r.Y, d, d, 180, 90);
        tee.AddArc(r.Right - d, r.Y, d, d, 270, 90);
        tee.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
        tee.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
        tee.CloseFigure();
        return tee;
    }

    // ümar sisestuskast: TextBox pannakse kasti sisse, vormile lisatakse KAST (mitte TextBox)
    public static Kaart Sisend(TextBox tb, int x, int y, int laius, string vihje, bool parool = false, Color? varv = null)
    {
        Color taust = varv ?? Taust;   // sisestuskasti värv
        var kast = new Kaart { Bounds = new Rectangle(x, y, laius, 44), Raadius = 8, Taustavarv = taust };

        tb.BorderStyle = BorderStyle.None;
        tb.BackColor = taust;
        tb.Font = new Font("Segoe UI", 11f);
        tb.PlaceholderText = vihje;
        tb.UseSystemPasswordChar = parool;
        tb.SetBounds(12, 12, laius - 24, 24);

        kast.Controls.Add(tb);
        kast.Click += (s, e) => tb.Focus();
        return kast;
    }
}

// Ümarate nurkadega kaart (valge, õhukese raamiga)
public class Kaart : Panel
{
    public int Raadius { get; set; } = 12;
    public Color Taustavarv { get; set; } = Color.White;
    public Color Raamivarv { get; set; } = Color.FromArgb(228, 228, 228);
    public bool Esile { get; set; }   // hiir peal -> punane raam

    public Kaart()
    {
        DoubleBuffered = true;
        ResizeRedraw = true;
    }

    public void SeaEsile(bool v)
    {
        Esile = v;
        Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        using var tee = Stiil.Umar(new Rectangle(0, 0, Width - 1, Height - 1), Raadius);
        using var pintsel = new SolidBrush(Taustavarv);
        using var pliiats = new Pen(Esile ? Stiil.Punane : Raamivarv, Esile ? 2 : 1);
        g.FillPath(pintsel, tee);
        g.DrawPath(pliiats, tee);
    }
}

// Ümar nupp (punane, hiire peal tumedam)
public class RoundButton : Button
{
    public int Raadius { get; set; } = 8;
    public Color Varv { get; set; } = Stiil.Punane;
    private bool hiirPeal;

    public RoundButton()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint |
                 ControlStyles.OptimizedDoubleBuffer, true);
        Font = new Font("Segoe UI", 10f, FontStyle.Bold);
        ForeColor = Color.White;
        Cursor = Cursors.Hand;
    }

    protected override void OnMouseEnter(EventArgs e) { hiirPeal = true; Invalidate(); base.OnMouseEnter(e); }
    protected override void OnMouseLeave(EventArgs e) { hiirPeal = false; Invalidate(); base.OnMouseLeave(e); }
    protected override void OnEnabledChanged(EventArgs e) { Invalidate(); base.OnEnabledChanged(e); }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        // nurkade taust = vanema värv
        Color taust = Parent is Kaart k ? k.Taustavarv : (Parent?.BackColor ?? SystemColors.Control);
        g.Clear(taust);

        Color varv = !Enabled ? Color.FromArgb(210, 210, 210)
                   : hiirPeal ? ControlPaint.Dark(Varv, 0.08f)
                   : Varv;

        using var tee = Stiil.Umar(new Rectangle(0, 0, Width - 1, Height - 1), Raadius);
        using var pintsel = new SolidBrush(varv);
        g.FillPath(pintsel, tee);

        TextRenderer.DrawText(g, Text, Font, ClientRectangle, ForeColor,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
    }
}


