using gympass.Interfaces;
using gympass.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gympass.Services
{
    public class BonusService : IBonusService
    {
        private const int voltasCorridaCompleta = 4;

        public ResultadoCorrida MelhorVoltaPiloto(ResultadoCorrida resultadoCorridaIndividual, List<RegistroCorrida> registrosCorrida)
        {
            var melhorVolta = registrosCorrida.Where(p => p.NumeroPiloto == resultadoCorridaIndividual.CodigoPiloto).OrderBy(x => x.TempoVolta).FirstOrDefault();
            resultadoCorridaIndividual.MelhorVolta = melhorVolta;
            return resultadoCorridaIndividual;
        }

        public ResultadoCorrida MelhorVoltaCorrida(ResultadoCorrida resultadoCorridaIndividual, List<RegistroCorrida> registrosCorrida)
        {
            var melhorVoltaCorrida = registrosCorrida.OrderBy(x => x.TempoVolta).FirstOrDefault();
            resultadoCorridaIndividual.MelhorVoltaCorrida = melhorVoltaCorrida;
            return resultadoCorridaIndividual;
        }

        public ResultadoCorrida VelocidadeMedia(ResultadoCorrida resultadoCorridaIndividual, List<RegistroCorrida> registrosCorrida)
        {
            var qtdVoltasRegistradas = registrosCorrida.Count();
            double velocidadeMedia = 0;

            var registrosIndividuais = registrosCorrida.Where(x => Convert.ToInt32(x.NumeroPiloto) == resultadoCorridaIndividual.CodigoPiloto).ToList();
            registrosIndividuais.ForEach(x => velocidadeMedia = x.VelocidadeMediaVolta + x.VelocidadeMediaVolta / qtdVoltasRegistradas);

            VelocidadeMediaTodaCorrida velocidadeMediaCorrida = new VelocidadeMediaTodaCorrida();
            velocidadeMediaCorrida.Velocidade = velocidadeMedia.ToString();
            velocidadeMediaCorrida.NomePiloto = resultadoCorridaIndividual.NomePiloto;

            resultadoCorridaIndividual.VelocidadeMedia = velocidadeMediaCorrida;
            return resultadoCorridaIndividual;
        }


    }
}
