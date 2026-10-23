using Microsoft.EntityFrameworkCore;

namespace Timur_Tooded;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        Kaustad.Loo();

        using (var db = new ToodedContext())
        {
            db.Database.Migrate();
            LooAlgkasutajad(db);
            LooAlgtooted(db);
        }

        Application.Run(new LoginForm());
    }

    private static void LooAlgkasutajad(ToodedContext db)
    {
        if (db.Kasutajad.Any()) return;

        db.Kasutajad.AddRange(
            new Kasutaja
            {
                Nimi = "Admin",
                Email = "admin@admin.ee",
                ParoolHash = PasswordHelper.Hash("admin123"),
                Roll = "admin"
            },
            new Kasutaja
            {
                Nimi = "Kasutaja",
                Email = "kasutaja@pood.ee",
                ParoolHash = PasswordHelper.Hash("kasutaja123"),
                Roll = "kasutaja"
            });
        db.SaveChanges();
    }

    private static void LooAlgtooted(ToodedContext db)
    {
        // nimi, hind, kogus, kategooriaId, pildi failinimi, pildi URL (t¸hi = pilti pole)
        var andmed = new (string Nimi, decimal Hind, int Kogus, int Kat, string Fail, string Url)[]
{
    // Joogid (Selver, pilte pole)
    ("Aloe Vera jook Premium ALEO 1,5 L", 4.19m, 30, 1, "", ""),
    ("Allikavesi Vittel 1,5 L",           1.79m, 50, 1, "", ""),
    ("Mineraalvesi Evian 1,5 L",          2.29m, 40, 1, "", ""),
    ("Mineraalvesi Evian 500 ml",         1.29m, 60, 1, "", ""),

    // Joogid (Lidl)
    ("Solevita apelsinimahl 100%", 1.49m, 30, 1, "lidl_apelsinimahl.png",
        "https://www.lidl.ee/assets/gcp50ad638c18624b3ebf9cdf0444eae2a0.png"),

    // Puu- ja kˆˆgiviljad (Lidl)
    ("Hele viinamari, seemneteta 500 g", 1.49m, 25, 2, "lidl_viinamari.png",
        "https://www.lidl.ee/assets/gcpf2ce96a14c614b938a532d9c11ed0945.png"),
    ("J‰‰salat 300 g",                   0.99m, 20, 2, "lidl_jaasalat.png",
        "https://www.lidl.ee/assets/gcpd501194f19ec43eab017eb181a88bffe.png"),
    ("Roheline avokaado",                0.89m, 35, 2, "lidl_avokaado.png",
        "https://www.lidl.ee/assets/gcp02ee8c4730094e1e9513b36ee27b0008.png"),
    ("Kiivi Gold",                       0.55m, 0,  2, "lidl_kiivi.png",
        "https://www.lidl.ee/assets/gcp8cf69292b5c34f66a4612a2c97046beb.png"),
    ("Hispaania mango Osteen",           1.69m, 15, 2, "lidl_mango.png",
        "https://www.lidl.ee/assets/gcp9b9c0374bf2e4bb49284c3195fed9456.png"),

    // Piimatooted ja munad (Lidl) - hinnad ligikaudsed!
    ("Keefir, hellus",  1.09m, 30, 4, "lidl_keefir.png",
        "https://www.lidl.ee/assets/gcp83eab88f5b5d41bc98297fa69784972c.png"),
    ("Kanamunad",       2.49m, 40, 4, "lidl_kanamunad.png",
        "https://www.lidl.ee/assets/gcp588976fa77ad495292546af59a600a72.png"),

    // Liha (Lidl) - hinnad ligikaudsed!
    ("Broileri veerand",               3.99m, 20, 5, "lidl_broileri.png",
        "https://www.lidl.ee/assets/gcp6e0df745a03d4ddfa8af5ce2c021e256.png"),
    ("Tastino rukkirost, viilutatud",  1.79m, 25, 5, "lidl_rukkirost.png",
        "https://www.lidl.ee/assets/gcpf484ffffabbd47928f487df7afcf112c.png"),

    // Pagaritooted (Lidl) - hind ligikaudne!
    ("Tume leib, viilutatud", 1.29m, 30, 6, "lidl_leib.png",
        "https://www.lidl.ee/assets/gcp1e50abff2f5d4660833f91ed7de4fc73.png"),

    // Muu (Lidl) - hinnad ligikaudsed!
    ("Freshona kikerherned",                 0.89m, 40, 7, "lidl_kikerherned.png",
        "https://www.lidl.ee/assets/gcp455e7ebc97a1418bbc794a6f78344d81.png"),
    ("Hellmann's majonees",                  2.19m, 30, 7, "lidl_majonees.png",
        "https://www.lidl.ee/assets/gcp17fef4b60ad940a7a1396ffe6b4c6190.png"),
    ("Max's World koeratoit veiseliha ja kartuliga", 1.99m, 25, 7, "lidl_koeratoit.png",
        "https://www.lidl.ee/assets/gcpccb7be28791b40cdaa14ce9b03b99447.png"),
};

        using var http = new HttpClient();
        http.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0");

        foreach (var a in andmed)
        {
            var toode = db.Toodetabel.FirstOrDefault(t => t.Toodenimetus == a.Nimi);

            if (toode == null)
            {
                toode = new Toode
                {
                    Toodenimetus = a.Nimi,
                    Hind = a.Hind,
                    Kogus = a.Kogus,
                    KategooriaId = a.Kat
                };
                db.Toodetabel.Add(toode);
            }

            // pilt lisatakse ainult siis, kui tootel pilti veel pole
            if (string.IsNullOrEmpty(toode.Pilt) && a.Url != "")
                toode.Pilt = LaadiPilt(http, a.Url, a.Fail);
        }
        db.SaveChanges();
    }
    private static string? LaadiPilt(HttpClient http, string url, string fail)
    {
        string alus = url.Contains('.') ? url[..url.LastIndexOf('.')] : url;
        string[] variandid = { url, alus + ".jpg", alus + ".png" };

        foreach (var v in variandid)
        {
            try
            {
                byte[] baidid = http.GetByteArrayAsync(v).GetAwaiter().GetResult();
                using (var ms = new MemoryStream(baidid))
                using (new Bitmap(ms)) { }   // viskab vea, kui formaat ei ole toetatud
                File.WriteAllBytes(Path.Combine(Kaustad.Pildid, fail), baidid);
                return fail;
            }
            catch { /* proovime j‰rgmist varianti */ }
        }
        return null;   // toode j‰‰b ilma pildita, programm ei kuku
    }
}