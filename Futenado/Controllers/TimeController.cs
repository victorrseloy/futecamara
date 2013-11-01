/* Copyright 2013 de Victor Ribeiro da Silva Eloy Este arquivo é parte do programa FUTECAMARA. O FUTECAMARA é um software livre; você pode redistribuí-lo e/ou modificá-lo dentro dos termos da GNU General Public License como publicada pela Fundação do Software Livre (FSF); na versão 3 da Licença. Este programa é distribuído na esperança que possa ser útil, mas SEM NENHUMA GARANTIA; sem uma garantia implícita de ADEQUAÇÃO a qualquer MERCADO ou APLICAÇÃO EM PARTICULAR. Veja a licença para maiores detalhes. Você deve ter recebido uma cópia da GNU General Public License, sob o título "LICENSE", junto com este programa, se não, acesse http://www.gnu.org/licenses/ */

using Futenado.Components;
using Futenado.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Futenado.Controllers
{
    public class TimeController : Controller
    {
        //
        // GET: /Time/

        private EntityContext db = new EntityContext();

        
        public ActionResult Index()
        {

            return View();

        }

        public ActionResult Add(int idDeputado)
        {

            int idUsuario = SessionManager.GetUsuario().Id;
            Time time = db.Times.FirstOrDefault(_time => _time.UsuarioID == idUsuario);

            if (time == null)
            {
                time = new Time();
                time.Deputados = new List<DeputadoTime>();
                time.UsuarioID = idUsuario;
                db.Times.Add(time);


                //criando a chave
                int timesPorChave = Convert.ToInt32(ConfigurationManager.AppSettings["times_por_chave"]);
                Chave chave = db.Chaves.FirstOrDefault(_chave => _chave.Times.Count() > timesPorChave);
                //se nao há nenhuma chave livre
                if (chave == null)
                {
                    chave = new Chave();
                    db.Chaves.Add(chave);
                    PopularChave(chave, 11);
                }

                chave.Times.Add(time);
                

                db.SaveChanges();
            }

            Deputado deputado = db.Deputadoes.First(_deputado => _deputado.Id == idDeputado);
            DeputadoTime deputadoTime = new DeputadoTime() { DeputadoID = deputado.Id, TimeID = time.Id };

            time.Deputados.Add(deputadoTime);

            
            db.SaveChanges();

            return Content("ok");
        }

        public ActionResult Remove(int idDeputado)
        {
            int idUsuario = SessionManager.GetUsuario().Id;
            Time time = db.Times.FirstOrDefault(_time => _time.UsuarioID == idUsuario);

            if (time != null)
            {
                DeputadoTime deputado = db.DeputadosTime.First(_deputadoTime => _deputadoTime.DeputadoID == idDeputado 
                    && _deputadoTime.TimeID == time.Id);
                try
                {
                    db.DeputadosTime.Remove(deputado);
                    time.Deputados.Remove(deputado);
                    db.SaveChanges();
                }
                catch { }

               
                
            }


            return Content("ok");
           
        }

        public ActionResult MudarNome(string nome)
        {
            int idUsuario = SessionManager.GetUsuario().Id;
            Time time = db.Times.FirstOrDefault(_time => _time.UsuarioID == idUsuario);
            if (time != null)
            {
                time.Nome = nome;
                db.SaveChanges();
            }

            return Content("ok");
        }

        public ActionResult AddBrasao(string img)
        {
            int idUsuario = SessionManager.GetUsuario().Id;
            Time time = db.Times.FirstOrDefault(_time => _time.UsuarioID == idUsuario);

            if (time != null)
            {
                time.pathBrasao = "~/Uploads/" + img;
                db.SaveChanges();
            }
            else
            {
                time = new Time();
                time.Deputados = new List<DeputadoTime>();
                time.UsuarioID = idUsuario;
                time.pathBrasao = "~/Uploads/" + img;
                db.Times.Add(time);


                //criando a chave
                int timesPorChave = Convert.ToInt32(ConfigurationManager.AppSettings["times_por_chave"]);
                Chave chave = db.Chaves.FirstOrDefault(_chave => _chave.Times.Count() > timesPorChave);
                //se nao há nenhuma chave livre
                if (chave == null)
                {
                    chave = new Chave();
                    db.Chaves.Add(chave);
                    PopularChave(chave, 11);
                }

                chave.Times.Add(time);
                db.SaveChanges();
            }

            return Content("ok");
        }

        public ActionResult SetXY(int idDeputadoTime, int x, int y)
        {
            DeputadoTime deputado = db.DeputadosTime.First(_dp => _dp.Id == idDeputadoTime);
            deputado.PosicaoX = x;
            deputado.PosicaoY = y;
            db.SaveChanges();

            return Content("ok");
        }

        private void PopularChave(Chave chave,int totalTimes)
        {
            Usuario bot = db.Usuarios.FirstOrDefault(_usuario => _usuario.IdFacebook == "bot");

            if (bot == null)
            {
                bot = new Usuario { 
                    Nome = "jogador ",
                    Sobrenome = "do sistema",
                    IdFacebook = "bot",
                    UltimoLogin = DateTime.Now,
                    FotoPerfil = "~/Images/usuario.png"
                };
                db.Usuarios.Add(bot);
                db.SaveChanges();
            }

            int j = 1;
            for (int i = chave.Times.Count(); i <= totalTimes; i++)
            {
                Time time = new Time();
                time.Deputados = new List<DeputadoTime>();
                time.UsuarioID = bot.Id;
                time.Chave = chave;
                time.pathBrasao = "~/Content/Images/brasao" + (int)(j) + ".jpg";
               
                time.bot = true;

                time.Nome = "Computador " + i;

                db.Times.Add(time);
                db.SaveChanges();
                var deputados = db.Deputadoes.OrderBy(r => Guid.NewGuid()).Take(14).
                    ToList().ConvertAll(_deputado => new DeputadoTime { DeputadoID = _deputado.Id, TimeID = time.Id });
                time.Deputados.AddRange(deputados);
                db.SaveChanges();
                j++;
            }
        }


        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

    }
}
