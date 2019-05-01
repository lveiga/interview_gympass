using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using gympass.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace gympass.Controllers
{
    public class UploadController : Controller
    {
        IHostingEnvironment _appEnvironment;
        public UploadController(IHostingEnvironment env)
        {
            _appEnvironment = env;
        }
        
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> LoadDefaultLog()
        {
            try
            {
                string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Files\LogDefault.txt");
                string[] files = System.IO.File.ReadAllLines(path, Encoding.GetEncoding("iso-8859-1"));

                List<KartRacing> kartRacings = new List<KartRacing>();

                for (int i = 1; i < files.Length; i++)
                {
                   
                    files[i] = files[i].Replace("\u0096", " ").Replace("\t\t"," ");
                    var registros = files[i].Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                    KartRacing kart = new KartRacing();

                    for (int x = 0; x <= registros.Length; x++)
                    {
                        switch (x)
                        {
                            case 0:
                                kart.Hora = TimeSpan.Parse(registros[x]);
                                break;
                            case 1:
                                kart.NumeroPiloto = Convert.ToInt32(registros[x]);
                                break;
                            case 2:
                                kart.NomePiloto = registros[x];
                                break;
                            case 3:
                                kart.Volta = Convert.ToInt32(registros[x]);
                                break;
                            case 4:
                                TimeSpan tempoVolta;
                                if(!TimeSpan.TryParse(registros[x], out tempoVolta))
                                {
                                    if(registros[x].Length == 8)
                                    {
                                        tempoVolta = TimeSpan.Parse("00:0" + registros[x]);
                                    }
                                }

                                kart.TempoVolta = tempoVolta;
                                break;
                            case 5:
                                kart.VelocidadeMediaVolta= Convert.ToDouble(registros[x]);
                                break;
                            default:
                                break;
                        }
                    }

                    kartRacings.Add(kart);
                }

                var pilotos = kartRacings.GroupBy(x => x.NumeroPiloto)
                        .Select(x => x.ToList())
                        .ToList();


                List<ResultRace> incompletos = new List<ResultRace>();
                List<ResultRace> completosRaces = new List<ResultRace>();

                foreach (var piloto in pilotos)
                {
                    ResultRace result = new ResultRace();
                    if(piloto.Count() < 4)
                    {
                        result.NomePiloto = piloto[0].NomePiloto;
                        result.CodigoPiloto = piloto[0].NumeroPiloto;
                        result.QtdVoltasCompletadas = piloto.Count();
                        result.TempoTotalProva = new TimeSpan(piloto.Sum(p => p.TempoVolta.Ticks)).ToString();
                        incompletos.Add(result);

                        break;
                    }

                    result.NomePiloto = piloto[0].NomePiloto;
                    result.CodigoPiloto = piloto[0].NumeroPiloto;
                    result.QtdVoltasCompletadas = piloto.Count();
                    result.TempoTotalProva = new TimeSpan(piloto.Sum(p => p.TempoVolta.Ticks)).ToString();

                    completosRaces.Add(result);
                }

                completosRaces = completosRaces.OrderBy(x => x.TempoTotalProva).ToList();

                for (int i = 0; i < completosRaces.Count(); i++)
                {
                    completosRaces[i].PosicaoChegada = i + 1;
                }

                incompletos = incompletos.OrderBy(x => x.TempoTotalProva).ToList();

                for (int i = 0; i < incompletos.Count(); i++)
                {
                    incompletos[i].PosicaoChegada = i + 1;
                }

                completosRaces.AddRange(incompletos);

            }
            catch (Exception ex)
            {
                throw;
            }

            return Ok(JsonConvert.SerializeObject(new KartRacing()));
        }


        public async Task<IActionResult> EnviarArquivo(List<IFormFile> arquivos)
        {
            long tamanhoArquivos = arquivos.Sum(f => f.Length);
            var caminhoArquivo = Path.GetTempFileName();

            foreach (var arquivo in arquivos)
            {
                //verifica se existem arquivos 
                if (arquivo == null || arquivo.Length == 0)
                {
                    //retorna a viewdata com erro
                    ViewData["Erro"] = "Error: Arquivo(s) não selecionado(s)";
                    return View(ViewData);
                }
                // < define a pasta onde vamos salvar os arquivos >
                string pasta = "Arquivos_Usuario";
                // Define um nome para o arquivo enviado incluindo o sufixo obtido de milesegundos
                string nomeArquivo = "Usuario_arquivo_" + DateTime.Now.Millisecond.ToString();
                //verifica qual o tipo de arquivo : jpg, gif, png, pdf ou tmp
                if (arquivo.FileName.Contains(".jpg"))
                    nomeArquivo += ".jpg";
                else if (arquivo.FileName.Contains(".gif"))
                    nomeArquivo += ".gif";
                else if (arquivo.FileName.Contains(".png"))
                    nomeArquivo += ".png";
                else if (arquivo.FileName.Contains(".pdf"))
                    nomeArquivo += ".pdf";
                else
                    nomeArquivo += ".tmp";
                //< obtém o caminho físico da pasta wwwroot >
                string caminho_WebRoot = _appEnvironment.WebRootPath;
                // monta o caminho onde vamos salvar o arquivo : 
                // ~\wwwroot\Arquivos\Arquivos_Usuario\Recebidos
                string caminhoDestinoArquivo = caminho_WebRoot + "\\Arquivos\\" + pasta + "\\";
                // incluir a pasta Recebidos e o nome do arquivo enviado : 
                // ~\wwwroot\Arquivos\Arquivos_Usuario\Recebidos\
                string caminhoDestinoArquivoOriginal = caminhoDestinoArquivo + "\\Recebidos\\" + nomeArquivo;
                //copia o arquivo para o local de destino original
                using (var stream = new FileStream(caminhoDestinoArquivoOriginal, FileMode.Create))
                {
                    await arquivo.CopyToAsync(stream);
                }
            }
            //monta a ViewData que será exibida na view como resultado do envio 
            ViewData["Resultado"] = $"{arquivos.Count} arquivos foram enviados ao servidor, " +
             $"com tamanho total de : {tamanhoArquivos} bytes";
            //retorna a viewdata
            return View(ViewData);
        }
    }
}