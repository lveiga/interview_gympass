﻿using gympass.Enums;
using gympass.Interfaces;
using gympass.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gympass.Services
{
    public class RegistroCorridaService : IRegistroCorridaService
    {
        private string colunasIncorretas = string.Empty;
        private const int colunasArquivo = 6;
        private const int timeSpanIncompleto = 8;

        public async Task<List<RegistroCorrida>> ObterRegistrosCorrida(string[] registros)
        {
            try
            {
                return await Task.FromResult(PrepararListaRegistros(registros));
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private List<RegistroCorrida> PrepararListaRegistros(string[] registrosArquivo)
        {
            try
            {
                List<RegistroCorrida> registrosCorrida = new List<RegistroCorrida>();

                for (int linhaTual = 1; linhaTual < registrosArquivo.Length; linhaTual++)
                {
                    registrosArquivo[linhaTual] = registrosArquivo[linhaTual].Replace("\u0096", " ").Replace("\t\t", " ");
                    var kartRegistros = registrosArquivo[linhaTual].Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                    if(kartRegistros.Length != colunasArquivo)
                        throw new Exception("Formato do arquivo errado!");

                    registrosCorrida.Add(CriarRegistro(kartRegistros, linhaTual));
                }

                return registrosCorrida;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private RegistroCorrida CriarRegistro(string[] kartRegistros, int linhaAtual)
        {
            RegistroCorrida kart = new RegistroCorrida();

            for (int registro = 0; registro <= kartRegistros.Length; registro++)
            {
                switch (registro)
                {
                    case (int)FormatoArquivoCorrida.Hora:
                        if (ContemLetras(kartRegistros[registro]))
                            throw new Exception("Campo Hora no Formato Errado!");


                        kart.Hora = TimeSpan.Parse(kartRegistros[registro]);
                        break;

                    case (int)FormatoArquivoCorrida.NumeroPiloto:
                        if (ContemLetras(kartRegistros[registro]))
                            throw new Exception("Campo NumeroPiloto no Formato Errado!");

                        kart.NumeroPiloto = Convert.ToInt32(kartRegistros[registro]);
                        break;

                    case (int)FormatoArquivoCorrida.NomePiloto:
                        kart.NomePiloto = kartRegistros[registro];
                        break;

                    case (int)FormatoArquivoCorrida.Volta:
                        if (ContemLetras(kartRegistros[registro]))
                            throw new Exception("Campo Volta no Formato Errado!");


                        kart.Volta = Convert.ToInt32(kartRegistros[registro]);
                        break;

                    case (int)FormatoArquivoCorrida.TempoVolta:
                        if (ContemLetras(kartRegistros[registro]))
                            throw new Exception("Campo tempoVolta no Formato Errado!");


                        TimeSpan tempoVolta;
                        if (!TimeSpan.TryParse(kartRegistros[registro], out tempoVolta))
                        {
                            if (kartRegistros[registro].Length == timeSpanIncompleto)
                            {
                                tempoVolta = TimeSpan.Parse("00:0" + kartRegistros[registro]);
                            }
                        }

                        kart.TempoVolta = tempoVolta;
                        break;

                    case (int)FormatoArquivoCorrida.VelocidadeMediaVolta:
                        if (ContemLetras(kartRegistros[registro]))
                            throw new Exception("Formato do arquivo errado!");

                        kart.VelocidadeMediaVolta = Convert.ToDouble(kartRegistros[registro]);
                        break;
                    default:
                        break;
                }

                if(registro == colunasArquivo - 1)
                {
                    if (!VerificaKartEstaValido(kart))
                        throw new Exception("Formato Incorreto! verificar linha " + (linhaAtual + 1).ToString() + "Campos: " + colunasIncorretas);
                }
            }

            return kart;
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

        private bool VerificaKartEstaValido(RegistroCorrida kart)
        {
            string colunasIncorretas = string.Empty;

            if(kart.Hora == new TimeSpan())
            {
                colunasIncorretas += "  Hora  ";
            }

            if (string.IsNullOrEmpty(kart.NomePiloto))
            {
                colunasIncorretas += "  NumeroPiloto  ";
            }

            if (string.IsNullOrEmpty(kart.NomePiloto))
            {
                colunasIncorretas += "  NomePiloto  ";
            }

            if (kart.Volta == 0)
            {
                colunasIncorretas += "  Volta  ";
            }

            if(kart.TempoVolta == new TimeSpan())
            {
                colunasIncorretas += "  TempoVolta  ";
            }

            if(kart.VelocidadeMediaVolta == 0)
            {
                colunasIncorretas += "  VelocidadeMediaVolta  ";
            }

            if (!string.IsNullOrEmpty(colunasIncorretas))
            {
                colunasIncorretas = this.colunasIncorretas;
                return false;
            }
                

            return true;
        }
    }
}
