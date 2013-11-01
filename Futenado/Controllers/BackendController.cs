/* Copyright 2013 de Victor Ribeiro da Silva Eloy Este arquivo é parte do programa FUTECAMARA. O FUTECAMARA é um software livre; você pode redistribuí-lo e/ou modificá-lo dentro dos termos da GNU General Public License como publicada pela Fundação do Software Livre (FSF); na versão 3 da Licença. Este programa é distribuído na esperança que possa ser útil, mas SEM NENHUMA GARANTIA; sem uma garantia implícita de ADEQUAÇÃO a qualquer MERCADO ou APLICAÇÃO EM PARTICULAR. Veja a licença para maiores detalhes. Você deve ter recebido uma cópia da GNU General Public License, sob o título "LICENSE", junto com este programa, se não, acesse http://www.gnu.org/licenses/ */

using Futenado.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;

namespace Futenado.Controllers
{
    public class BackendController : Controller
    {
        //
        // GET: /Backend/

        private EntityContext db = new EntityContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AtualizarDeputados()
        {
            //DeputadosSoapClient deputadosWS = new DeputadosSoapClient();
            //XmlNode resposta = deputadosWS.ObterDeputados();

            var client = new RestClient("http://www.camara.gov.br/SitCamaraWS/Deputados.asmx");
            var request = new RestRequest("/ObterDeputados", Method.GET);
            request.RequestFormat = DataFormat.Xml;
            var queryResult = client.Execute(request);

            XDocument documento = XDocument.Parse(queryResult.Content);
            IEnumerable<XElement> deputados = documento.Descendants("deputado");

            //jah estou com os deputados
            foreach (var deputado in deputados)
            {

                Deputado deputadoWS = new Deputado();
                deputadoWS.Nome = deputado.Descendants("nome").FirstOrDefault().Value;
                deputadoWS.ideCadastro = Convert.ToInt32(deputado.Descendants("ideCadastro").FirstOrDefault().Value);
                deputadoWS.UrlFoto = deputado.Descendants("urlFoto").FirstOrDefault().Value;
                deputadoWS.Matricula = deputado.Descendants("matricula").FirstOrDefault().Value;
                deputadoWS.Partido = deputado.Descendants("partido").FirstOrDefault().Value;


                if (db.Deputadoes.Count(_deputado => _deputado.ideCadastro == deputadoWS.ideCadastro) == 0)
                {
                    db.Deputadoes.Add(deputadoWS);
                }



            }

            db.SaveChanges();

            return Content("ok");
        }

        public ActionResult CalcularAssiduidade()
        {
            var deputados = db.Deputadoes;
            DateTime agora = DateTime.Now;
            DateTime mesPassado = agora.AddMonths(-1);

            foreach (var deputado in deputados)
            {
                try
                {

                    var client = new RestClient("http://www.camara.gov.br/SitCamaraWS/sessoesreunioes.asmx");
                    var request = new RestRequest("/ListarPresencasParlamentar", Method.GET);
                    request.AddParameter("dataIni", mesPassado.ToString("dd/MM/yyyy"));
                    request.AddParameter("dataFim", agora.ToString("dd/MM/yyyy"));
                    request.AddParameter("numMatriculaParlamentar", deputado.Matricula);
                    request.RequestFormat = DataFormat.Xml;
                    var queryResult = client.Execute(request);

                    XDocument documento = XDocument.Parse(queryResult.Content);
                    IEnumerable<XElement> frequncias = documento.Descendants("frequencianoDia");
                    int presencas = 0;
                    float total = 0;
                    foreach (var frequencia in frequncias)
                    {
                        total += 1;
                        if (frequencia.Value == "Presença")
                        {
                            presencas++;
                        }
                    }


                    deputado.Assiduidade = (presencas / total) * 10;

                }
                catch { }

            }

            db.SaveChanges();

            return Content("ok");
        }

        public ActionResult CalcularExperiencia()
        {
            var deputados = db.Deputadoes;
            

            foreach (var deputado in deputados)
            {
                try
                {
                    var client = new RestClient("http://www.camara.gov.br/SitCamaraWS/Deputados.asmx");
                    var request = new RestRequest("/ObterDetalhesDeputado", Method.GET);
                    request.AddParameter("ideCadastro", deputado.ideCadastro);
                    request.AddParameter("numLegislatura", "");
                    request.RequestFormat = DataFormat.Xml;
                    var queryResult = client.Execute(request);

                    //periodoExercicio

                    XDocument documento = XDocument.Parse(queryResult.Content);
                    IEnumerable<XElement> periodos = documento.Descendants("periodoExercicio");
                    int meusPeriodos = 0;

                    foreach (var qualquerBosta in periodos)
                    {
                        meusPeriodos++;
                    }

                    deputado.Experiencia = meusPeriodos;
                }
                catch { }

                //eu tava cansado pra kralho qndo fiz isso me deem um desconto
            }


            db.SaveChanges();
            return Content("ok");
        }

        public ActionResult CalcularComissoes()
        {
            var deputados = db.Deputadoes;


            foreach (var deputado in deputados)
            {
                try
                {
                    var client = new RestClient("http://www.camara.gov.br/SitCamaraWS/Deputados.asmx");
                    var request = new RestRequest("/ObterDetalhesDeputado", Method.GET);
                    request.AddParameter("ideCadastro", deputado.ideCadastro);
                    request.AddParameter("numLegislatura", "");
                    request.RequestFormat = DataFormat.Xml;
                    var queryResult = client.Execute(request);

                    //periodoExercicio

                    XDocument documento = XDocument.Parse(queryResult.Content);
                    IEnumerable<XElement> periodos = documento.Descendants("comissao");
                    int meusPeriodos = 0;

                    foreach (var qualquerBosta in periodos)
                    {
                        meusPeriodos++;
                    }

                    deputado.Comissoes = meusPeriodos;
                }
                catch { }

                //eu tava cansado pra kralho qndo fiz isso me deem um desconto
            }


            db.SaveChanges();
            return Content("ok");
        }


        public ActionResult ExecutarRodada(DateTime data)
        {
            Random rnd = new Random();
            var partidas = db.Partidas.Where(_partida => !_partida.JahOcorreu && _partida.DataDoJogo < data);
            
            float xpMax = db.Deputadoes.Max(p => p.Experiencia);
            float comissoesMax = db.Deputadoes.Max(p => p.Comissoes);

            foreach (var partida in partidas)
            {
                partida.JahOcorreu = true;

                var deputadosTimeCasa = partida.TimeDaCasa.Deputados.ConvertAll(x => x.Deputado);
                var deputadosTimeFora = partida.TimeDeFora.Deputados.ConvertAll(x => x.Deputado);

                float ProbabilidadeGolCasa = 
                    ((deputadosTimeCasa.Sum(x => x.Assiduidade) / deputadosTimeCasa.Count())
                    +
                    ((deputadosTimeCasa.Sum(x => x.Comissoes)/comissoesMax) / deputadosTimeCasa.Count())
                     +
                    ((deputadosTimeCasa.Sum(x => x.Experiencia) / xpMax) / deputadosTimeCasa.Count()))/3
                    ;

                var ProbabilidadeGolFora = ((deputadosTimeFora.Sum(x => x.Assiduidade) / deputadosTimeFora.Count())
                    +
                    ((deputadosTimeFora.Sum(x => x.Comissoes) / comissoesMax) / deputadosTimeFora.Count())
                     +
                    ((deputadosTimeFora.Sum(x => x.Experiencia) / xpMax) / deputadosTimeFora.Count())) / 3;

                for (int i = 0; i < 5; i++)
                {
                    float val = (float)(rnd.NextDouble());
                    if (ProbabilidadeGolCasa > val)
                    {
                        partida.PlacarTimeDaCasa++;
                    }
                }

                for (int i = 0; i < 5; i++)
                {
                    float val = (float)(rnd.NextDouble());
                    if (ProbabilidadeGolFora > val)
                    {
                        partida.PlacarTimeDeFora++;
                    }
                }
            }

            db.SaveChanges();
            return Content("ok");
        }


       
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

    }
}
