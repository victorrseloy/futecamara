﻿/* Copyright 2013 de Victor Ribeiro da Silva Eloy Este arquivo é parte do programa FUTECAMARA. O FUTECAMARA é um software livre; você pode redistribuí-lo e/ou modificá-lo dentro dos termos da GNU General Public License como publicada pela Fundação do Software Livre (FSF); na versão 3 da Licença. Este programa é distribuído na esperança que possa ser útil, mas SEM NENHUMA GARANTIA; sem uma garantia implícita de ADEQUAÇÃO a qualquer MERCADO ou APLICAÇÃO EM PARTICULAR. Veja a licença para maiores detalhes. Você deve ter recebido uma cópia da GNU General Public License, sob o título "LICENSE", junto com este programa, se não, acesse http://www.gnu.org/licenses/ */

using Futenado.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Futenado.Components
{
    public static class SessionManager
    {
        public static void SetUsuario(Usuario usuario)
        {
            HttpContext.Current.Session["USUARIO"] = usuario;
        }

        public static Usuario GetUsuario()
        {
            return HttpContext.Current.Session["USUARIO"] as Usuario;
        }
    }
}