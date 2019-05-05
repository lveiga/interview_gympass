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
            try
            {
                var pilotos = kartRacings.GroupBy(x => x.NumeroPiloto)
                    .Select(x => x.ToList())
                    .ToList();

                return ObterPodioCorrida(pilotos);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter dados finais da corrida!");
            }
        }

        private static List<ResultadoCorrida> ObterPodioCorrida(List<List<KartRacing>> pilotos)
        {
            try
            {
                List<ResultadoCorrida> incompletos, completos;
                SegregarCompetidores(pilotos, out incompletos, out completos);

                List<ResultadoCorrida> resultadoFinalCorrida = ClassificacaoFinal(incompletos, completos);

                return resultadoFinalCorrida;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao calcular resultado final da corrida!");
            }
           
        }

        private static void SegregarCompetidores(List<List<KartRacing>> pilotos, out List<ResultadoCorrida> incompletos, out List<ResultadoCorrida> completos)
        {
            incompletos = new List<ResultadoCorrida>();
            completos = new List<ResultadoCorrida>();
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

                completos.Add(resultado);
            }
        }

        private static List<ResultadoCorrida> ClassificacaoFinal(List<ResultadoCorrida> incompletos, List<ResultadoCorrida> completos)
        {
            var resultadoFinalCorrida = completos.OrderBy(x => x.TempoTotalProva).ToList();

            int posicaoCorrida = 0;
            for (int i = 0; i < resultadoFinalCorrida.Count(); i++)
            {
                resultadoFinalCorrida[i].PosicaoChegada = i + 1;
                posicaoCorrida = resultadoFinalCorrida[i].PosicaoChegada;
            }

            incompletos = incompletos.OrderBy(x => x.TempoTotalProva).ToList();

            for (int i = 0; i < incompletos.Count(); i++)
            {
                incompletos[i].PosicaoChegada = posicaoCorrida + 1;
            }

            resultadoFinalCorrida.AddRange(incompletos);
            return resultadoFinalCorrida;
        }
    }
}
