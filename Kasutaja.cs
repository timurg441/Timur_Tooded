using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timur_Tooded
{
    public class Kasutaja
    {
        public int Id { get; set; }
        public string Nimi { get; set; } = "";
        public string Email { get; set; } = "";
        public string ParoolHash { get; set; } = "";
        public string Roll { get; set; } = "Müüja";
    }
}
