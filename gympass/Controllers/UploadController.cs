using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using gympass.Interfaces;
using gympass.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace gympass.Controllers
{
    public class UploadController : Controller
    {
        private string _mensagemErro = string.Empty;
        
        private readonly IRegistroCorridaService _registroService;
        private readonly ICorridaService _corridaServices;
        private readonly IBonusService _bonusService;

        public UploadController( IRegistroCorridaService kartService, ICorridaService corridaServices, IBonusService bonusService)
        {
            _registroService = kartService;
            _corridaServices = corridaServices;
            _bonusService = bonusService;
        }
        
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> CarregarArquivoPadrao()
        {
            try
            {
                string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Files\LogDefault.txt");
                string[] linhasRegistroLogKart = System.IO.File.ReadAllLines(path, Encoding.GetEncoding("iso-8859-1"));

                var registrosCorrida = await _registroService.ObterRegistrosCorrida(linhasRegistroLogKart);

                var resultadoCorrida = await _corridaServices.ApresentarResultadoCorrida(registrosCorrida);

                if(resultadoCorrida.Count > 0)
                    Bonus(registrosCorrida, resultadoCorrida);

                return Ok(JsonConvert.SerializeObject(resultadoCorrida));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

      

        public async Task<IActionResult> CarregarArquivo(IFormFile arquivo)
        {
            try
            {
                if (!ArquivoTexto(arquivo))
                    return BadRequest(_mensagemErro);

                List<string> linhasRegistroLogKart = new List<string>();
                using (var reader = new StreamReader(arquivo.OpenReadStream(), Encoding.GetEncoding("iso-8859-1")))
                {
                    while (reader.Peek() >= 0)
                        linhasRegistroLogKart.Add(await reader.ReadLineAsync());
                }

                string[] registrosArquivo = linhasRegistroLogKart.Select(i => i.ToString()).ToArray();

                var registrosCorrida = await _registroService.ObterRegistrosCorrida(registrosArquivo);

                var resultadoCorrida = await _corridaServices.ApresentarResultadoCorrida(registrosCorrida);

                if (resultadoCorrida.Count > 0)
                    Bonus(registrosCorrida, resultadoCorrida);

                return Ok(JsonConvert.SerializeObject(resultadoCorrida));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public bool ArquivoTexto(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                _mensagemErro = "Nenhum Arquivo Selecionado ou Formato Incorreto!";
                return false;
            }

            if (!file.FileName.Contains(".txt"))
            {
                _mensagemErro = "Apenas arquivo texto";
                return false;
            }

            return true; ;
        }

        private void Bonus(List<RegistroCorrida> registrosCorrida, List<ResultadoCorrida> resultadoCorrida)
        {
            for (int i = 0; i < resultadoCorrida.Count(); i++)
            {
                resultadoCorrida[i] = _bonusService.MelhorVoltaPiloto(resultadoCorrida[i], registrosCorrida);
                resultadoCorrida[i] = _bonusService.VelocidadeMedia(resultadoCorrida[i], registrosCorrida);
            }

            resultadoCorrida[0] = _bonusService.MelhorVoltaCorrida(resultadoCorrida[0], registrosCorrida);
        }
    }
}