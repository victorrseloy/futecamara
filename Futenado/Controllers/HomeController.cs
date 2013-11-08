/* Copyright 2013 de Victor Ribeiro da Silva Eloy Este arquivo é parte do programa FUTECAMARA. O FUTECAMARA é um software livre; você pode redistribuí-lo e/ou modificá-lo dentro dos termos da GNU General Public License como publicada pela Fundação do Software Livre (FSF); na versão 3 da Licença. Este programa é distribuído na esperança que possa ser útil, mas SEM NENHUMA GARANTIA; sem uma garantia implícita de ADEQUAÇÃO a qualquer MERCADO ou APLICAÇÃO EM PARTICULAR. Veja a licença para maiores detalhes. Você deve ter recebido uma cópia da GNU General Public License, sob o título "LICENSE", junto com este programa, se não, acesse http://www.gnu.org/licenses/ */

using Futenado.Components;
using Futenado.Models;
using Futenado.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Configuration;
using System.IO;
using System.Data.Objects;

namespace Futenado.Controllers
{
    public class HomeController : Controller
    {

        private EntityContext db = new EntityContext();

        public ActionResult Index()
        {
            if (SessionManager.GetUsuario() != null)
            {
                return RedirectToAction("passo1");
            }

            return View();
        }

        public ActionResult passo1()
        {
            int idusuario = SessionManager.GetUsuario().Id;

            Time model = db.Times.Include(_time => _time.Deputados)
                    .FirstOrDefault(_time => _time.UsuarioID == idusuario);

            return View("tela1", model);
        }

        public ActionResult passo2()
        {
            int idusuario = SessionManager.GetUsuario().Id;

            Time model = db.Times.Include(_time => _time.Deputados)
                    .FirstOrDefault(_time => _time.UsuarioID == idusuario);

            return View("tela2", model);
        }

        public ActionResult passo3()
        {
            int idusuario = SessionManager.GetUsuario().Id;
            Time time = db.Times.Include(_time => _time.Deputados)
                    .FirstOrDefault(_time => _time.UsuarioID == idusuario);




            return View("tela3", time.Chave);
        }

        public ActionResult passo4()
        {
            int idusuario = SessionManager.GetUsuario().Id;
            Time time = db.Times.Include(_time => _time.Deputados)
                    .FirstOrDefault(_time => _time.UsuarioID == idusuario);
            var partidas = db.Partidas.Where(x => x.TimeDaCasaID == time.Id || x.TimeDeForaID == time.Id).OrderBy(x => x.DataDoJogo).ToList();
            ViewBag.time = time;
            if (partidas.Any())
            {
                return View("tela4", partidas);
            }
            else
            {
                GerarTabelaJogos(time.Chave);
                partidas = db.Partidas.Where(x => x.TimeDaCasaID == time.Id || x.TimeDeForaID == time.Id).OrderBy(x => x.DataDoJogo).ToList();
                return View("tela4", partidas);
            }

        }

        public ActionResult TabelaDeClassificacao(int id = 0)
        {
            //tenta pegar o id passado
            Chave minhaChave = null;

            if (id != 0)
            {
                minhaChave = db.Chaves.
              Include(_chave => _chave.Times).
               FirstOrDefault(_chave => _chave.Id == id);
            }

            else if (SessionManager.GetUsuario() != null)
            {
                int idJogador = SessionManager.GetUsuario().Id;
                id = db.Times.Where(_time => _time.UsuarioID == idJogador).Select(_time => _time.ChaveID).First();
                minhaChave = db.Chaves.
                    Include(_chave => _chave.Times).
                    FirstOrDefault(_chave => _chave.Id == id);
            }

            if (minhaChave == null)
            {
                return HttpNotFound();
            }

            List<ClassificacaoViewModel> classificacao = new List<ClassificacaoViewModel>();

            foreach (var time in minhaChave.Times)
            {
                var partidas = db.Partidas.Where(_partida => _partida.TimeDeForaID == time.Id || _partida.TimeDaCasaID == time.Id);
                var viewModel = new ClassificacaoViewModel();
                viewModel.CaminhoBrasao = time.pathBrasao;
                viewModel.NomeJogador = time.Usuario.Nome;
                viewModel.NomeTime = time.Nome;
                viewModel.Vitorias = partidas.Count(_partida =>
                    ((_partida.TimeDaCasaID == time.Id && _partida.PlacarTimeDaCasa > _partida.PlacarTimeDeFora) ||
                    (_partida.TimeDeForaID == time.Id && _partida.PlacarTimeDeFora > _partida.PlacarTimeDaCasa))
                    && _partida.JahOcorreu);

                viewModel.Empates = partidas.Count(_partida => (_partida.PlacarTimeDaCasa == _partida.PlacarTimeDeFora)
                    && _partida.JahOcorreu);

                viewModel.Derrotas = partidas.Count(_partida => _partida.JahOcorreu) - (viewModel.Vitorias + viewModel.Empates);
                classificacao.Add(viewModel);

                //somando gols que fiz em casa
                viewModel.Gols += partidas.
                    Where(_partida => _partida.TimeDaCasaID == time.Id)
                    .Sum(_x => _x.PlacarTimeDaCasa);

                //somando gols que fiz fora de casa
                viewModel.Gols += partidas.
                    Where(_partida => _partida.TimeDeForaID == time.Id)
                    .Sum(_x => _x.PlacarTimeDaCasa);
            }

            return View(classificacao.
                OrderByDescending(_classificacao => _classificacao.Pontos).
                ThenByDescending(_classificacao => _classificacao.Gols));


        }


        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult ListarDeputados()
        {
            var deputados = db.Deputadoes.ToList();
            deputados = deputados.OrderBy(_deputado => Guid.NewGuid()).ToList();

            SelecaoViewModel model = new SelecaoViewModel();
            model.TodosOsDeputados = deputados;

            if (SessionManager.GetUsuario() != null)
            {
                int idusuario = SessionManager.GetUsuario().Id;
                var deputadosParaCovnersao = db.Times.Include(_time => _time.Deputados)
                    .FirstOrDefault(_time => _time.UsuarioID == idusuario);

                if (deputadosParaCovnersao != null)
                {
                    model.MeuTime = deputadosParaCovnersao.Deputados.
                    ConvertAll(_deputadoTime => _deputadoTime.Deputado)
                    .ToList();
                }

                else
                {
                    model.MeuTime = new List<Deputado>();
                }

            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Upload(int? chunk, string name)
        {
            var fileUpload = Request.Files[0];
            var uploadPath = Server.MapPath("~/Uploads");
            chunk = chunk ?? 0;
            using (var fs = new FileStream(Path.Combine(uploadPath, name), chunk == 0 ? FileMode.Create : FileMode.Append))
            {
                var buffer = new byte[fileUpload.InputStream.Length];
                fileUpload.InputStream.Read(buffer, 0, buffer.Length);
                fs.Write(buffer, 0, buffer.Length);
            }
            return Content("chunk uploaded", "text/plain");
        }

        private bool GerarTabelaJogos(Chave chave)
        {
            int conta = 1;
            //0:Domingo
            //1:Quarta
            int nextGame = 0;
            DateTime AtualGame = DateTime.Now;

            List<DateTime> datasJogos = new List<DateTime>();
            if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday ||
                DateTime.Now.DayOfWeek == DayOfWeek.Monday ||
                DateTime.Now.DayOfWeek == DayOfWeek.Tuesday)
            {
                nextGame = 1;
            }

            //Numero de jogos por time
            while (conta <= 20)
            {
                AtualGame = GetNextGame(AtualGame, nextGame);
                datasJogos.Add(AtualGame);
                if (nextGame == 1)
                {
                    nextGame = 0;
                }
                else
                {
                    nextGame = 1;
                }
                conta++;
            }


            int j = 0;
            int casa = 0;
            int fora = 0;
            Random random = new Random();
            for (int i = 0; i < (chave.Times.Count - 1); i++)
            {
                j = i + 1;
                for (; j < chave.Times.Count; j++)
                {

                    if (random.NextDouble() < .5)
                    {
                        casa = i;
                        fora = j;
                    }
                    else
                    {
                        casa = j;
                        fora = i;
                    }
                    DateTime dataTemp = DateTime.Now;
                    foreach (DateTime date in datasJogos)
                    {
                        int tempCasaID = chave.Times[casa].Id;
                        int tempForaID = chave.Times[fora].Id;
                        var temp = db.Partidas.FirstOrDefault(_partida => (
                            _partida.TimeDaCasaID == tempCasaID ||
                            _partida.TimeDaCasaID == tempForaID ||
                            _partida.TimeDeForaID == tempCasaID ||
                            _partida.TimeDeForaID == tempForaID) &&
                            (EntityFunctions.TruncateTime(_partida.DataDoJogo) == EntityFunctions.TruncateTime(date)));

                        if (temp == null)
                        {
                            dataTemp = date;
                            break;
                        }

                    }
                    Partida partida = new Partida
                    {
                        TimeDaCasa = chave.Times[casa],
                        TimeDeFora = chave.Times[fora],
                        DataDoJogo = dataTemp,
                        PlacarTimeDaCasa = 0,
                        PlacarTimeDeFora = 0
                    };
                    db.Partidas.Add(partida);
                    db.SaveChanges();
                }
            }
            db.SaveChanges();

            return true;
        }

        private DateTime GetNextGame(DateTime anterior, int i)
        {
            //0:Domingo
            //1:Quarta
            int diaProcurado = 0;
            if (i == 0)
            {
                diaProcurado = (int)DayOfWeek.Sunday;
            }
            else
            {
                diaProcurado = (int)DayOfWeek.Wednesday;
            }
            int daysDiff = (diaProcurado - (int)anterior.DayOfWeek + 7) % 7;
            DateTime nextGame = anterior.AddDays(daysDiff);
            return nextGame;
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

    }
}
