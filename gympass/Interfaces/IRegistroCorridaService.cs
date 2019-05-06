using gympass.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gympass.Interfaces
{
    public interface IRegistroCorridaService
    {
        Task<List<RegistroCorrida>> ObterRegistrosCorrida(string[] registros);
    }
}
