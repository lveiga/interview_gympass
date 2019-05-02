﻿using System;
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
        
        private readonly IKartService _kartService;
        private readonly ICorridaService _corridaServices;

        public UploadController( IKartService kartService, ICorridaService corridaServices)
        {
            _kartService = kartService;
            _corridaServices = corridaServices;
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
                string[] linhasRegistroLogKart = System.IO.File.ReadAllLines(path, Encoding.GetEncoding("iso-8859-1"));

                var karts = await _kartService.ObterKartLista(linhasRegistroLogKart);

                return Ok(JsonConvert.SerializeObject(await _corridaServices.ApresentarResultadoCorrida(karts)));
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        public async Task<IActionResult> EnviarArquivo(IFormFile arquivo)
        {
            var caminhoArquivo = Path.GetTempFileName();

            if (!ArquivoTexto(arquivo))
                return BadRequest(_mensagemErro);

            string[] linhasRegistroLogKart = new string[] { };
            using (var reader = new StreamReader(arquivo.OpenReadStream(), Encoding.GetEncoding("iso-8859-1")))
            {
                while (reader.Peek() >= 0)
                    linhasRegistroLogKart[reader.Peek()] = (await reader.ReadLineAsync());
            }

            var karts = await _kartService.ObterKartLista(linhasRegistroLogKart);

            return Ok(JsonConvert.SerializeObject(await _corridaServices.ApresentarResultadoCorrida(karts)));
        }


        private bool ArquivoTexto(IFormFile file)
        {
            bool respota = true;
             
            if (file == null || file.Length == 0)
            {
                respota = false;
                _mensagemErro = "Nenhum Arquivo Selecionado";
            }

            if (!file.FileName.Contains(".txt"))
            {
                respota = false;
                _mensagemErro = "Apenas arquivo texto";
            }

            return respota;
        }
    }
}