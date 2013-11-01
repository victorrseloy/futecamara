/* Copyright 2013 de Victor Ribeiro da Silva Eloy Este arquivo é parte do programa FUTECAMARA. O FUTECAMARA é um software livre; você pode redistribuí-lo e/ou modificá-lo dentro dos termos da GNU General Public License como publicada pela Fundação do Software Livre (FSF); na versão 3 da Licença. Este programa é distribuído na esperança que possa ser útil, mas SEM NENHUMA GARANTIA; sem uma garantia implícita de ADEQUAÇÃO a qualquer MERCADO ou APLICAÇÃO EM PARTICULAR. Veja a licença para maiores detalhes. Você deve ter recebido uma cópia da GNU General Public License, sob o título "LICENSE", junto com este programa, se não, acesse http://www.gnu.org/licenses/ */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CsQuery;
using Futenado.ViewModels;

namespace Futenado.Components
{
    public static class Util
    {
        public static Uri getFacebookUrl(UrlHelper url, Uri requestUrl)
        {
            var uriBuilder = new UriBuilder(requestUrl);
            uriBuilder.Query = null;
            uriBuilder.Fragment = null;
            uriBuilder.Path = url.Action("FacebookCallback");
            return uriBuilder.Uri;
        }

        public static List<NoticiaViewModel> GetNoticias()
        {
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;
            string downloadString = client.DownloadString("http://www2.camara.leg.br/");
            CQ dom = CQ.Create(downloadString);
            var content = dom.Select(".listaEstilo-1 li a").ToList();
            List<NoticiaViewModel> retorno = new List<NoticiaViewModel>();

            foreach (var i in content)
            {
                var texto = i.Cq().Html();
                var temp = texto.Split('-');
                var href = i.Cq().Attr("href");
                retorno.Add(new NoticiaViewModel
                {
                    texto = WebUtility.HtmlDecode(temp[1]),
                    data = temp[0],
                    href = href
                });
            }
            return retorno;
        }
       
    }
}