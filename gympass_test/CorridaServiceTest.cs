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
        private Mock<IRegistroCorridaService> _registroCorridaServiceMock;
        private IRegistroCorridaService _registroCorridaService;
        private ICorridaService _corridaService;

        public CorridaServiceTest()
        {
            _corridaServiceMock = new Mock<ICorridaService>();
            _registroCorridaServiceMock = new Mock<IRegistroCorridaService>();
            _corridaService = new CorridaService();
            _registroCorridaService = new RegistroCorridaService();
        }

       [Test]
       public void DeveRetornaSucessoDadoArquivoFormatoCorreto()
        {
            List<RegistroCorrida> registrosCorrida = ObterListaRegistrosFormatoCorreto();
            var resultado = _corridaService.ApresentarResultadoCorrida(registrosCorrida).Result;

            Assert.IsTrue(resultado.Count > 0);
        }

        [Test]
        public void DeveRetornaExcecaoDadoListaKartNula()
        {
            Exception expectedExcetpion = null;
            List<RegistroCorrida> racings = null;

            try
            {
                var resultado = _corridaService.ApresentarResultadoCorrida(racings).Result;
            }
            catch (Exception ex)
            {
                expectedExcetpion = ex;
            }

            Assert.IsNotNull(expectedExcetpion);
            Assert.IsTrue(expectedExcetpion.Message.Contains("Erro ao obter dados finais da corrida!"));
        }

        [Test]
        public void DeveRetornaExcecaoDadoListaPilotoComVoltasAlemPermitido()
        {
            Exception expectedExcetpion = null;

            try
            {
                string[] linhas = ObterTextoLogCorridaTesteComMaisVoltas().Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                var registros = _registroCorridaService.ObterRegistrosCorrida(linhas).Result;

                var resultado = _corridaService.ApresentarResultadoCorrida(registros).Result;
            }
            catch (Exception ex)
            {
                expectedExcetpion = ex;
            }

            Assert.IsNotNull(expectedExcetpion);
            Assert.IsTrue(expectedExcetpion.Message.Contains("Formato Incorreto! O piloto:"));
        }

        private string ObterTextoLogCorridaTesteComMaisVoltas()
        {
            return @"Hora                               Piloto             Nº Volta   Tempo Volta       Velocidade média da volta
                        23:49:08.277      038  F.MASSA                           1		1:02.852                        44,275
                        23:49:10.858      033  R.BARRICHELLO                     1		1:04.352                        43,243
                        23:49:11.075      002  K.RAIKKONEN                       1             1:04.108                        43,408
                        23:49:12.667      023  M.WEBBER                          1		1:04.414                        43,202
                        23:49:30.976      015  F.ALONSO                          1		1:18.456			35,47
                        23:50:11.447      038  F.MASSA                           2		1:03.170                        44,053
                        23:50:14.860      033  R.BARRICHELLO                     2		1:04.002                        43,48
                        23:50:15.057      002  K.RAIKKONEN                       2             1:03.982                        43,493
                        23:50:17.472      023  M.WEBBER                          2		1:04.805                        42,941
                        23:50:37.987      015  F.ALONSO                          2		1:07.011			41,528
                        23:51:14.216      038  F.MASSA                           3		1:02.769                        44,334
                        23:51:18.576      033  R.BARRICHELLO		          3		1:03.716                        43,675
                        23:51:19.044      002  K.RAIKKONEN                       3		1:03.987                        43,49
                        23:51:21.759      023  M.WEBBER                          3		1:04.287                        43,287
                        23:51:46.691      015  F.ALONSO                          3		1:08.704			40,504
                        23:52:01.796      011  S.VETTEL                          1		3:31.315			13,169
                        23:52:17.003      038  F.MASS                            4		1:02.787                        44,321
                        23:52:22.586      033  R.BARRICHELLO		          4		1:04.010                        43,474
                        23:52:22.120      002  K.RAIKKONEN                       4		1:03.076                        44,118
                        23:52:25.975      023  M.WEBBER                          4		1:04.216                        43,335
                        23:53:06.741      015  F.ALONSO                          4		1:20.050			34,763
                        23:53:39.660      011  S.VETTEL                          2		1:37.864			28,435
                        23:54:57.757      011  S.VETTEL                          3		1:18.097			35,633
                        23:49:08.277      038  F.MASSA                           1		2:02.852                        44,275";
        }

        private string ObterTextoLogCorridaTeste()
        {
            return @"Hora                               Piloto             Nº Volta   Tempo Volta       Velocidade média da volta
                        23:49:08.277      038  F.MASSA                           1		1:02.852                        44,275
                        23:49:10.858      033  R.BARRICHELLO                     1		1:04.352                        43,243
                        23:49:11.075      002  K.RAIKKONEN                       1             1:04.108                        43,408
                        23:49:12.667      023  M.WEBBER                          1		1:04.414                        43,202
                        23:49:30.976      015  F.ALONSO                          1		1:18.456			35,47
                        23:50:11.447      038  F.MASSA                           2		1:03.170                        44,053
                        23:50:14.860      033  R.BARRICHELLO                     2		1:04.002                        43,48
                        23:50:15.057      002  K.RAIKKONEN                       2             1:03.982                        43,493
                        23:50:17.472      023  M.WEBBER                          2		1:04.805                        42,941
                        23:50:37.987      015  F.ALONSO                          2		1:07.011			41,528
                        23:51:14.216      038  F.MASSA                           3		1:02.769                        44,334
                        23:51:18.576      033  R.BARRICHELLO		          3		1:03.716                        43,675
                        23:51:19.044      002  K.RAIKKONEN                       3		1:03.987                        43,49
                        23:51:21.759      023  M.WEBBER                          3		1:04.287                        43,287
                        23:51:46.691      015  F.ALONSO                          3		1:08.704			40,504
                        23:52:01.796      011  S.VETTEL                          1		3:31.315			13,169
                        23:52:17.003      038  F.MASS                            4		1:02.787                        44,321
                        23:52:22.586      033  R.BARRICHELLO		          4		1:04.010                        43,474
                        23:52:22.120      002  K.RAIKKONEN                       4		1:03.076                        44,118
                        23:52:25.975      023  M.WEBBER                          4		1:04.216                        43,335
                        23:53:06.741      015  F.ALONSO                          4		1:20.050			34,763
                        23:53:39.660      011  S.VETTEL                          2		1:37.864			28,435
                        23:54:57.757      011  S.VETTEL                          3		1:18.097			35,633";
        }

        private List<RegistroCorrida> ObterListaRegistrosFormatoCorreto()
        {
            string[] linhas = ObterTextoLogCorridaTeste().Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            var registros = _registroCorridaService.ObterRegistrosCorrida(linhas).Result;
            return registros;
        }

    }
    
}
