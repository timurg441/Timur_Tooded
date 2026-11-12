using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timur_Tooded;

// Üks konto, mille vahel saab kiiresti vahetada
public record Konto(int Id, string Nimi, string Roll);

public static class AktiivneKasutaja
{
    public static int Id { get; set; }
    public static string Nimi { get; set; } = "";
    public static string Roll { get; set; } = "";
    public static bool OnAdmin => Roll == "admin";

    // kontod, mida selles sessioonis saab valida (admin: admin + kasutaja, tavakasutaja: ainult tema ise)
    public static List<Konto> Kontod { get; } = new();

    // teeb valitud konto aktiivseks
    public static void Vaheta(Konto konto)
    {
        Id = konto.Id;
        Nimi = konto.Nimi;
        Roll = konto.Roll;
    }

    public static void LogiValja()
    {
        Id = 0;
        Nimi = "";
        Roll = "";
        Kontod.Clear();
    }
}
