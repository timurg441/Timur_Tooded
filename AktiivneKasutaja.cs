using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timur_Tooded;
public static class AktiivneKasutaja
{
    public static string Nimi { get; set; } = "";
    public static string Roll { get; set; } = "";
    public static bool OnAdmin => Roll == "admin";
}
