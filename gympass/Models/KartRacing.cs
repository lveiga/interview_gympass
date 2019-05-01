using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gympass.Models
{
    public class KartRacing
    {
        public TimeSpan Hora { get; set; }
        public int Volta { get; set; }
        public TimeSpan TempoVolta { get; set; }
        public double VelocidadeMediaVolta { get; set; }
        public string NomePiloto { get; set; }
        public int NumeroPiloto { get; set; }
    }
}
