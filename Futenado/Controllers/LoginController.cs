/* Copyright 2013 de Victor Ribeiro da Silva Eloy Este arquivo é parte do programa FUTECAMARA. O FUTECAMARA é um software livre; você pode redistribuí-lo e/ou modificá-lo dentro dos termos da GNU General Public License como publicada pela Fundação do Software Livre (FSF); na versão 3 da Licença. Este programa é distribuído na esperança que possa ser útil, mas SEM NENHUMA GARANTIA; sem uma garantia implícita de ADEQUAÇÃO a qualquer MERCADO ou APLICAÇÃO EM PARTICULAR. Veja a licença para maiores detalhes. Você deve ter recebido uma cópia da GNU General Public License, sob o título "LICENSE", junto com este programa, se não, acesse http://www.gnu.org/licenses/ */

using Facebook;
using Futenado.Components;
using Futenado.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;

namespace Futenado.Controllers
{
    public class LoginController : Controller
    {

        private EntityContext db = new EntityContext();

        //
        // GET: /Login/

        public ActionResult Index()
        {
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = ConfigurationManager.AppSettings["facebook_app_id"],
                client_secret = ConfigurationManager.AppSettings["facebook_app_secret"],
                redirect_uri = Util.getFacebookUrl(Url, Request.Url).AbsoluteUri,
                response_type = "code",
                scope = "email" // Add other permissions as needed
            });

            return Redirect(loginUrl.AbsoluteUri);
        }

        public ActionResult FacebookCallback(string code)
        {
            var fb = new FacebookClient();
            dynamic result = fb.Post("oauth/access_token", new
            {
                client_id = ConfigurationManager.AppSettings["facebook_app_id"],
                client_secret = ConfigurationManager.AppSettings["facebook_app_secret"],
                redirect_uri = Util.getFacebookUrl(Url, Request.Url).AbsoluteUri,
                code = code
            });

            var accessToken = result.access_token;


            Session["AccessToken"] = accessToken;

            fb.AccessToken = accessToken;


            dynamic me = fb.Get("me?fields=first_name,last_name,id,email");
            string email = me.email;

            //cria um usuario
            Usuario usuarioFb = new Usuario();
            usuarioFb.Email = me.email;
            usuarioFb.Nome = me.first_name;
            usuarioFb.Sobrenome = me.last_name;
            usuarioFb.IdFacebook = me.id;
            usuarioFb.UltimoLogin = DateTime.Now;

            string foto = GetPictureUrl(me.id);
            usuarioFb.FotoPerfil = foto;


          

            if (db.Usuarios.Count(_usuario => _usuario.IdFacebook == usuarioFb.IdFacebook) == 0)
            {
                db.Usuarios.Add(usuarioFb);
                db.SaveChanges();

                Time time = db.Times.FirstOrDefault(_time => _time.UsuarioID == usuarioFb.Id);

                if (time == null)
                {
                    time = new Time();
                    time.Deputados = new List<DeputadoTime>();
                    time.UsuarioID = usuarioFb.Id;
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
            }
            else
            {
                usuarioFb = db.Usuarios.Where(_usuario => _usuario.IdFacebook == usuarioFb.IdFacebook).First();
                usuarioFb.UltimoLogin = DateTime.Now;
                usuarioFb.FotoPerfil = foto;
                db.SaveChanges();
            }

            SessionManager.SetUsuario(usuarioFb);



            return RedirectToAction("Index", "Home");
        }

        private void PopularChave(Chave chave, int totalTimes)
        {
            Usuario bot = db.Usuarios.FirstOrDefault(_usuario => _usuario.IdFacebook == "bot");

            if (bot == null)
            {
                bot = new Usuario
                {
                    Nome = "jogador ",
                    Sobrenome = "do sistema",
                    IdFacebook = "bot",
                    UltimoLogin = DateTime.Now,
                    FotoPerfil = "~/Images/usuario.png"
                };
                db.Usuarios.Add(bot);
                db.SaveChanges();
            }

            for (int i = chave.Times.Count(); i <= totalTimes; i++)
            {
                Time time = new Time();
                time.Deputados = new List<DeputadoTime>();
                time.UsuarioID = bot.Id;
                time.Chave = chave;
                time.pathBrasao = "~/Content/Images/modeloBrasaoP.png";
                time.bot = true;

                time.Nome = "Computador " + i;

                db.Times.Add(time);
                db.SaveChanges();
                var deputados = db.Deputadoes.OrderBy(r => Guid.NewGuid()).Take(14).
                    ToList().ConvertAll(_deputado => new DeputadoTime { DeputadoID = _deputado.Id, TimeID = time.Id });
                time.Deputados.AddRange(deputados);
                db.SaveChanges();
            }
        }

        private string GetPictureUrl(string faceBookId)
        {
            WebResponse response = null;
            string pictureUrl = string.Empty;
            try
            {
                WebRequest request = WebRequest.Create(string.Format("https://graph.facebook.com/{0}/picture", faceBookId));
                response = request.GetResponse();
                pictureUrl = response.ResponseUri.ToString();
            }
            catch (Exception)
            {
                //? handle
            }
            finally
            {
                if (response != null) response.Close();
            }
            return pictureUrl;
        }

    }
}
