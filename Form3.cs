using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Timur_Tooded
{
    public partial class Form3 : Form
    {
        private readonly ToodedContext _db;

        public Form3()
        {
            InitializeComponent();
            _db = new ToodedContext();
            LaeKategooriadJaPildid();
        }
        private Image LaePilt(string failinimi)
        {
            if (string.IsNullOrEmpty(failinimi)) return null;

            string tee = Path.Combine(Application.StartupPath, "Images", failinimi);
            if (!File.Exists(tee)) return null;

            using var ms = new MemoryStream(File.ReadAllBytes(tee));
            return new Bitmap(Image.FromStream(ms));
        }

        private void LaeKategooriadJaPildid()
        {
            tabPood.TabPages.Clear();

            var kategooriad = _db.Kategooriatabel.ToList();

            foreach (var kat in kategooriad)
            {
                var page = new TabPage(kat.Kategooria_nimetus);
                page.AutoScroll = true;

                var tooted = _db.Toodetabel
                                .Where(t => t.KategooriaId == kat.Id)
                                .ToList();

                int x = 10;
                int y = 10;
                int count = 0;

                foreach (var t in tooted)
                {
                    Panel p = new Panel();
                    p.Width = 150;
                    p.Height = 200;
                    p.BorderStyle = BorderStyle.FixedSingle;

                    PictureBox pb = new PictureBox();
                    pb.Width = 140;
                    pb.Height = 140;
                    pb.SizeMode = PictureBoxSizeMode.Zoom;
                    pb.Location = new Point(5, 5);
                    pb.Image = LaePilt(t.Pilt);

                    Label lbl = new Label();
                    lbl.Text = t.Toodenimetus;
                    lbl.AutoSize = false;
                    lbl.TextAlign = ContentAlignment.MiddleCenter;
                    lbl.Location = new Point(5, 150);
                    lbl.Width = 140;
                    lbl.Height = 40;

                    pb.Click += (s, e) =>
                    {
                        MessageBox.Show($"Toode: {t.Toodenimetus}\nHind: {t.Hind} €\nLaos: {t.Kogus} tk");
                    };

                    p.Controls.Add(pb);
                    p.Controls.Add(lbl);

                    p.Location = new Point(x, y);
                    page.Controls.Add(p);

                    x += 160;
                    count++;

                    if (count % 4 == 0)
                    {
                        x = 10;
                        y += 210;
                    }
                }

                tabPood.TabPages.Add(page);
            }
        }
    }
}
