using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gympass.Models
{
    public class ResultadoCorrida
    {
        public int PosicaoChegada { get; set; }
        public int CodigoPiloto { get; set; }
        public string NomePiloto { get; set; }
        public int QtdVoltasCompletadas { get; set; }
        public string TempoTotalProva { get; set; }
        public VelocidadeMediaTodaCorrida VelocidadeMedia { get; set; }
        public string DiferencaChegada { get; set; }
        public RegistroCorrida MelhorVolta { get; set; }
        public RegistroCorrida MelhorVoltaCorrida { get; set; }
    }
}
