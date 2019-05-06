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
        private static string _mensagemErro = string.Empty;
        private const int numeroVoltasCorrida = 4;

        public async Task<List<ResultadoCorrida>> ApresentarResultadoCorrida(List<RegistroCorrida> registrosCorrida)
        {
            return await Task.FromResult(ObterResultadoFinal(registrosCorrida));
        }

        private List<ResultadoCorrida> ObterResultadoFinal(List<RegistroCorrida> registrosCorrida)
        {
            try
            {
                var registrosPorPilotos = registrosCorrida.GroupBy(x => x.NumeroPiloto)
                    .Select(x => x.ToList())
                    .ToList();

                return ObterClassificacaoFinal(registrosPorPilotos);
            }
            catch (Exception)
            {
                if (string.IsNullOrEmpty(_mensagemErro))
                    throw new Exception("Erro ao obter dados finais da corrida!");

                throw new Exception(_mensagemErro);
            }
        }

        private static List<ResultadoCorrida> ObterClassificacaoFinal(List<List<RegistroCorrida>> pilotos)
        {
            try
            {
                List<ResultadoCorrida> incompletos, completos;
                SegregarCompetidores(pilotos, out incompletos, out completos);

                List<ResultadoCorrida> resultadoFinalCorrida = ClassificacaoFinal(incompletos, completos);

                return resultadoFinalCorrida;
            }
            catch (Exception)
            {
                if(string.IsNullOrEmpty(_mensagemErro))
                    _mensagemErro = "Erro ao calcular resultado final da corrida!";

                throw;
            }
           
        }

        private static void SegregarCompetidores(List<List<RegistroCorrida>> pilotos, out List<ResultadoCorrida> incompletos, out List<ResultadoCorrida> completos)
        {
            incompletos = new List<ResultadoCorrida>();
            completos = new List<ResultadoCorrida>();
            foreach (var piloto in pilotos)
            {

                if(piloto.Count > numeroVoltasCorrida)
                {
                    _mensagemErro = "Formato Incorreto! O piloto: " + piloto[0].NomePiloto + " Possui número de voltas além do permitido";
                    throw new Exception();
                }
                    

                ResultadoCorrida resultado = new ResultadoCorrida();

                if (piloto.Count() < numeroVoltasCorrida)
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
