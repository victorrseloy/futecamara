/* Copyright 2013 de Victor Ribeiro da Silva Eloy Este arquivo é parte do programa FUTECAMARA. O FUTECAMARA é um software livre; você pode redistribuí-lo e/ou modificá-lo dentro dos termos da GNU General Public License como publicada pela Fundação do Software Livre (FSF); na versão 3 da Licença. Este programa é distribuído na esperança que possa ser útil, mas SEM NENHUMA GARANTIA; sem uma garantia implícita de ADEQUAÇÃO a qualquer MERCADO ou APLICAÇÃO EM PARTICULAR. Veja a licença para maiores detalhes. Você deve ter recebido uma cópia da GNU General Public License, sob o título "LICENSE", junto com este programa, se não, acesse http://www.gnu.org/licenses/ */

using Futenado.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Futenado.Components
{
    public static class HtmlExtensions
    {
        public static IHtmlString CheckDeputado(this HtmlHelper html, List<Deputado> time, int idDeputado)
        {
            if (time != null)
            {

                switch (time.Count(_deputado => _deputado.Id == idDeputado))
                {
                    case 0:
                        return new HtmlString("");
                    case 1:
                        return new HtmlString("check-deputado-marcado");
                    //default:
                        //throw new InvalidOperationException("o time nao pode ter o mesmo deputado mais de uma vez");
                }

            }

            return new HtmlString("");
        }

    }
}