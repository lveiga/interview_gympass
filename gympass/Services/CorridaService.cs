using gympass.Interfaces;
using gympass.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gympass.Services
{
    public class CorridaService : ICorridaService
    {
        public async Task<List<ResultadoCorrida>> ApresentarResultadoCorrida(List<KartRacing> kartRacings)
        {
            return Task.FromResult(ObterResultadoFinal(kartRacings)).Result;
        }

        private List<ResultadoCorrida> ObterResultadoFinal(List<KartRacing> kartRacings)
        {
            var pilotos = kartRacings.GroupBy(x => x.NumeroPiloto)
                    .Select(x => x.ToList())
                    .ToList();

            return ObterPodioCorrida(pilotos);
        }

        private static List<ResultadoCorrida> ObterPodioCorrida(List<List<KartRacing>> pilotos)
        {
            List<ResultadoCorrida> incompletos = new List<ResultadoCorrida>();
            List<ResultadoCorrida> resultadoFinalCorrida = new List<ResultadoCorrida>();

            foreach (var piloto in pilotos)
            {
                ResultadoCorrida resultado = new ResultadoCorrida();
                if (piloto.Count() < 4)
                {
                    resultado.NomePiloto = piloto[0].NomePiloto;
                    resultado.CodigoPiloto = piloto[0].NumeroPiloto;
                    resultado.QtdVoltasCompletadas = piloto.Count();
                    resultado.TempoTotalProva = new TimeSpan(piloto.Sum(p => p.TempoVolta.Ticks)).ToString();
                    incompletos.Add(resultado);

                    break;
                }

                resultado.NomePiloto = piloto[0].NomePiloto;
                resultado.CodigoPiloto = piloto[0].NumeroPiloto;
                resultado.QtdVoltasCompletadas = piloto.Count();
                resultado.TempoTotalProva = new TimeSpan(piloto.Sum(p => p.TempoVolta.Ticks)).ToString();

                resultadoFinalCorrida.Add(resultado);
            }

            resultadoFinalCorrida = resultadoFinalCorrida.OrderBy(x => x.TempoTotalProva).ToList();
            int ultimaPosicao = 0;
            for (int i = 0; i < resultadoFinalCorrida.Count(); i++)
            {
                resultadoFinalCorrida[i].PosicaoChegada = i + 1;
                ultimaPosicao = resultadoFinalCorrida[i].PosicaoChegada;
            }

            incompletos = incompletos.OrderBy(x => x.TempoTotalProva).ToList();

            for (int i = 0; i < incompletos.Count(); i++)
            {
                incompletos[i].PosicaoChegada = ultimaPosicao + 1;
            }

            resultadoFinalCorrida.AddRange(incompletos);
            return resultadoFinalCorrida;
        }
    }
}
