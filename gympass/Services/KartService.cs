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
            return Task.FromResult(PrepararListaKart(registros)).Result;
        }

       

        private List<KartRacing> PrepararListaKart(string[] registros)
        {
            List<KartRacing> kartRacings = new List<KartRacing>();

            
            for (int i = 1; i < registros.Length; i++)
            {
                registros[i] = registros[i].Replace("\u0096", " ").Replace("\t\t", " ");
                var linhasRegistros = registros[i].Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                KartRacing kart = new KartRacing();

                for (int x = 0; x <= linhasRegistros.Length; x++)
                {
                    switch (x)
                    {
                        case 0:
                            kart.Hora = TimeSpan.Parse(linhasRegistros[x]);
                            break;
                        case 1:
                            kart.NumeroPiloto = Convert.ToInt32(linhasRegistros[x]);
                            break;
                        case 2:
                            kart.NomePiloto = linhasRegistros[x];
                            break;
                        case 3:
                            kart.Volta = Convert.ToInt32(linhasRegistros[x]);
                            break;
                        case 4:
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
    }
}
