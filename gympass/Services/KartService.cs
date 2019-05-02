using gympass.Interfaces;
using gympass.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gympass.Services
{
    public class KartService : IKartService
    {
        public async Task<List<KartRacing>> ObterKartLista(string[] registros)
        {
            try
            {
                return Task.FromResult(PrepararListaKart(registros)).Result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private List<KartRacing> PrepararListaKart(string[] registros)
        {
            try
            {
                List<KartRacing> kartRacings = new List<KartRacing>();

                for (int i = 1; i < registros.Length; i++)
                {
                    registros[i] = registros[i].Replace("\u0096", " ").Replace("\t\t", " ");
                    var linhasRegistros = registros[i].Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                    if(linhasRegistros.Length != 6)
                    {
                        throw new Exception("Formato do arquivo errado!");
                    }

                    KartRacing kart = new KartRacing();

                    for (int x = 0; x <= linhasRegistros.Length; x++)
                    {
                        switch (x)
                        {
                            case 0:
                                if (ContemLetras(linhasRegistros[x]))
                                    throw new Exception("Campo Hora no Formato Errado!");


                                kart.Hora = TimeSpan.Parse(linhasRegistros[x]);
                                break;

                            case 1:
                                if (ContemLetras(linhasRegistros[x]))
                                    throw new Exception("Campo NumeroPiloto no Formato Errado!");

                                kart.NumeroPiloto = Convert.ToInt32(linhasRegistros[x]);
                                break;

                            case 2:
                                kart.NomePiloto = linhasRegistros[x];
                                break;

                            case 3:
                                if (ContemLetras(linhasRegistros[x]))
                                    throw new Exception("Campo Volta no Formato Errado!");


                                kart.Volta = Convert.ToInt32(linhasRegistros[x]);
                                break;

                            case 4:
                                if (ContemLetras(linhasRegistros[x]))
                                    throw new Exception("Campo tempoVolta no Formato Errado!");


                                TimeSpan tempoVolta;
                                if (!TimeSpan.TryParse(linhasRegistros[x], out tempoVolta))
                                {
                                    if (linhasRegistros[x].Length == 8)
                                    {
                                        tempoVolta = TimeSpan.Parse("00:0" + linhasRegistros[x]);
                                    }
                                }

                                kart.TempoVolta = tempoVolta;
                                break;

                            case 5:
                                if(ContemLetras(linhasRegistros[x]))
                                    throw new Exception("Formato do arquivo errado!");

                                kart.VelocidadeMediaVolta = Convert.ToDouble(linhasRegistros[x]);
                                break;
                            default:
                                break;
                        }
                    }

                    kartRacings.Add(kart);
                }

                return kartRacings;
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        private bool ContemLetras(string texto)
        {
            if (texto.Where(c => char.IsLetter(c)).Count() > 0)
                return true;
            else
                return false;
        }

        private bool contemNumeros(string texto)
        {
            if (texto.Where(c => char.IsNumber(c)).Count() > 0)
                return true;
            else
                return false;
        }
    }
}
