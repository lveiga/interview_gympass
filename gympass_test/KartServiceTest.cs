using gympass.Interfaces;
using gympass.Services;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace gympass_test
{
    [TestFixture]
    public class KartServiceTest
    {
        private IKartService _kartService;

        public KartServiceTest()
        {
            _kartService = new KartService();
            
        }

        [Test]
        public void RetornaListaKartCorretamentDadoArquivoLog()
        {
            string[] linhas = ObterTextoLogCorridaTeste().Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            var resultado = _kartService.ObterKartLista(linhas).Result;

            Assert.IsTrue(resultado.Count > 0);
        }

        [Test]
        public void ReturnoListaZeradaDadoNenhumRegistroEncontradoNoArquivo()
        {
            var registrosNulos = new string[0];
            var resultado = _kartService.ObterKartLista(registrosNulos);

            Assert.IsFalse(resultado.Result.Count > 0);
        }

        [Test]
        public void RetornarExcecaoDadoRegistrosSemRetirarCaracteresEspeciais()
        {
            string[] linhas = ObterTextoLogCorridaTesteComCaracterEspecial().Split(new[] { Environment.NewLine },StringSplitOptions.None);
            Exception expectedExcetpion = null;

            try
            {
                var resultado = _kartService.ObterKartLista(linhas).Result;
            }
            catch (Exception ex)
            {
                expectedExcetpion = ex;
            }

            Assert.IsNotNull(expectedExcetpion);
        }

        [Test]
        public void RetornaErroArquivoDadoArquivoComFormatoErrado()
        {
            string[] linhas = ObterTextoLogCorridaSemVoltasTeste().Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            Exception expectedExcetpion = null;

            try
            {
                var resultado = _kartService.ObterKartLista(linhas).Result;
            }
            catch (Exception ex)
            {
                expectedExcetpion = ex;
            }

            Assert.IsTrue(expectedExcetpion.Message.Contains("Formato do arquivo errado!"));
        }

        [Test]
        public void RetornaErroArquivoComHoraErradoDadoLogComHoraErrada()
        {
            string[] linhas = ObterTextoLogCorridaTesteComHoraErrada().Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            Exception expectedExcetpion = null;

            try
            {
                var resultado = _kartService.ObterKartLista(linhas).Result;
            }
            catch (Exception ex)
            {
                expectedExcetpion = ex;
            }

            Assert.IsTrue(expectedExcetpion.Message.Contains("Campo Hora no Formato Errado!"));
        }

        [Test]
        public void RetornaErroArquivoComNumeroPilotoErradoDadoLogComNumeroPilotoErrado()
        {
            string[] linhas = ObterTextoLogCorridaTesteComNumeroPilotoErrado().Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            Exception expectedExcetpion = null;

            try
            {
                var resultado = _kartService.ObterKartLista(linhas).Result;
            }
            catch (Exception ex)
            {
                expectedExcetpion = ex;
            }

            Assert.IsTrue(expectedExcetpion.Message.Contains("Campo NumeroPiloto no Formato Errado!"));
        }

        [Test]
        public void RetornaErroArquivoComVoltaErradaDadoLogComVoltaErrado()
        {
            string[] linhas = ObterTextoLogCorridaTesteComVoltaErrada().Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            Exception expectedExcetpion = null;

            try
            {
                var resultado = _kartService.ObterKartLista(linhas).Result;
            }
            catch (Exception ex)
            {
                expectedExcetpion = ex;
            }

            Assert.IsTrue(expectedExcetpion.Message.Contains("Campo Volta no Formato Errado!"));
        }

        [Test]
        public void RetornaErroArquivoComTempoVoltaErradaDadoLogComTempoVoltaErrado()
        {
            string[] linhas = ObterTextoLogCorridaTesteComTempoVoltaErrada().Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            Exception expectedExcetpion = null;

            try
            {
                var resultado = _kartService.ObterKartLista(linhas).Result;
            }
            catch (Exception ex)
            {
                expectedExcetpion = ex;
            }

            Assert.IsTrue(expectedExcetpion.Message.Contains("Campo tempoVolta no Formato Errado!"));
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

        private string ObterTextoLogCorridaSemVoltasTeste()
        {
            return @"Hora                               Piloto              Tempo Volta       Velocidade média da volta
                        23:49:08.277      038  F.MASSA                    1:02.852                        44,275
                        23:49:10.858      033  R.BARRICHELLO              1:04.352                        43,243
                        23:49:11.075      002  K.RAIKKONEN                1:04.108                        43,408
                        23:49:12.667      023  M.WEBBER                   1:04.414                        43,202
                        23:49:30.976      015  F.ALONSO                   1:18.456			               35,47
                        23:50:11.447      038  F.MASSA                    1:03.170                        44,053
                        23:50:14.860      033  R.BARRICHELLO              1:04.002                        43,48
                        23:50:15.057      002  K.RAIKKONEN                1:03.982                        43,493
                        23:50:17.472      023  M.WEBBER                   1:04.805                        42,941
                        23:50:37.987      015  F.ALONSO                   1:07.011			               41,528
                        23:51:14.216      038  F.MASSA                    1:02.769                        44,334
                        23:51:18.576      033  R.BARRICHELLO		       1:03.716                        43,675
                        23:51:19.044      002  K.RAIKKONEN                1:03.987                        43,49
                        23:51:21.759      023  M.WEBBER                   1:04.287                        43,287
                        23:51:46.691      015  F.ALONSO                   1:08.704			               40,504
                        23:52:01.796      011  S.VETTEL                   3:31.315			               13,169
                        23:52:17.003      038  F.MASS                     1:02.787                        44,321
                        23:52:22.586      033  R.BARRICHELLO		       1:04.010                        43,474
                        23:52:22.120      002  K.RAIKKONEN                1:03.076                        44,118
                        23:52:25.975      023  M.WEBBER                   1:04.216                        43,335
                        23:53:06.741      015  F.ALONSO                   1:20.050			               34,763
                        23:53:39.660      011  S.VETTEL                   1:37.864			               28,435
                        23:54:57.757      011  S.VETTEL                   1:18.097			               35,633";
        }

        private string ObterTextoLogCorridaTesteComHoraErrada()
        {
            return @"Hora                               Piloto             Nº Volta   Tempo Volta       Velocidade média da volta
                        MM23:49:08.277      038  F.MASSA                           1		MM1:02.852                        44,275";
        }

        private string ObterTextoLogCorridaTesteComNumeroPilotoErrado()
        {
            return @"Hora                               Piloto             Nº Volta   Tempo Volta       Velocidade média da volta
                        23:49:08.277      MM038  F.MASSA                           1		MM1:02.852                        44,275";
        }

        private string ObterTextoLogCorridaTesteComVoltaErrada()
        {
            return @"Hora                               Piloto             Nº Volta   Tempo Volta       Velocidade média da volta
                        23:49:08.277      038  F.MASSA                           MM1		MM1:02.852                        44,275";
        }

        private string ObterTextoLogCorridaTesteComTempoVoltaErrada()
        {
            return @"Hora                               Piloto             Nº Volta   Tempo Volta       Velocidade média da volta
                        23:49:08.277      038  F.MASSA                           1		MM1:02.852                        44,275";
        }

        private string ObterTextoLogCorridaTesteComCaracterEspecial()
        {
            return @"Hora                               Piloto             Nº Volta   Tempo Volta       Velocidade média da volta
                        23:49:08.277      038\u0096  F.MASSA                           1		\t\t1:02.852                        44,275";
        }

        
        
    }
}
