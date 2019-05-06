using gympass.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gympass.Interfaces
{
    public interface IBonusService
    {
        ResultadoCorrida MelhorVoltaPiloto(ResultadoCorrida resultadoCorridaIndividual, List<RegistroCorrida> registrosCorrida);
        ResultadoCorrida MelhorVoltaCorrida(ResultadoCorrida resultadoCorridaIndividual, List<RegistroCorrida> registrosCorrida);
        ResultadoCorrida VelocidadeMedia(ResultadoCorrida resultadoCorridaIndividual, List<RegistroCorrida> registrosCorrida);
    }
}
