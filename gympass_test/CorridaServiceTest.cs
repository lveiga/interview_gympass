using gympass.Controllers;
using gympass.Interfaces;
using gympass.Models;
using gympass.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace gympass_test
{
    [TestFixture]
    public class CorridaServiceTest
    {
        private Mock<ICorridaService> _corridaServiceMock;
        private Mock<IKartService> _kartServiceMock;
        private ICorridaService _corridaService;

        public CorridaServiceTest()
        {
            _corridaServiceMock = new Mock<ICorridaService>();
            _kartServiceMock = new Mock<IKartService>();
            _corridaService = new CorridaService();
        }

       

        [Test]
        public void RetornaExcecaoDadoListaKartNula()
        {
            Exception expectedExcetpion = null;
            List<KartRacing> racings = null;

            try
            {
                var resultado = _corridaService.ApresentarResultadoCorrida(racings).Result;
            }
            catch (Exception ex)
            {
                expectedExcetpion = ex;
            }

            Assert.IsNotNull(expectedExcetpion);
        }



        
    }
    
}
