/* Copyright 2013 de Victor Ribeiro da Silva Eloy Este arquivo é parte do programa FUTECAMARA. O FUTECAMARA é um software livre; você pode redistribuí-lo e/ou modificá-lo dentro dos termos da GNU General Public License como publicada pela Fundação do Software Livre (FSF); na versão 3 da Licença. Este programa é distribuído na esperança que possa ser útil, mas SEM NENHUMA GARANTIA; sem uma garantia implícita de ADEQUAÇÃO a qualquer MERCADO ou APLICAÇÃO EM PARTICULAR. Veja a licença para maiores detalhes. Você deve ter recebido uma cópia da GNU General Public License, sob o título "LICENSE", junto com este programa, se não, acesse http://www.gnu.org/licenses/ */

using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;

namespace Futenado.Models
{
    public class Partida
    {
        public int Id { get; set; }
        public int TimeDaCasaID { get; set; }
        public int TimeDeForaID { get; set; }
        public int PlacarTimeDaCasa { get; set; }
        public int PlacarTimeDeFora { get; set; }
        public DateTime DataDoJogo { get; set; }
        public bool JahOcorreu { get; set; }

        public virtual Time TimeDaCasa { get; set; }
        public virtual Time TimeDeFora { get; set; }

        /*
         * Seria legal implementa quem faz os gols
         * mas por ora não teremos isto
         * */


    }
}
