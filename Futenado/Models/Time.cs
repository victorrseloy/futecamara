/* Copyright 2013 de Victor Ribeiro da Silva Eloy Este arquivo é parte do programa FUTECAMARA. O FUTECAMARA é um software livre; você pode redistribuí-lo e/ou modificá-lo dentro dos termos da GNU General Public License como publicada pela Fundação do Software Livre (FSF); na versão 3 da Licença. Este programa é distribuído na esperança que possa ser útil, mas SEM NENHUMA GARANTIA; sem uma garantia implícita de ADEQUAÇÃO a qualquer MERCADO ou APLICAÇÃO EM PARTICULAR. Veja a licença para maiores detalhes. Você deve ter recebido uma cópia da GNU General Public License, sob o título "LICENSE", junto com este programa, se não, acesse http://www.gnu.org/licenses/ */

using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using System.Collections.Generic;

namespace Futenado.Models
{
    public class Time
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string pathBrasao { get; set; }
        public int UsuarioID { get; set; }
        public int ChaveID { get; set; }
        public bool bot { get; set; }
        public virtual Chave Chave { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual List<DeputadoTime> Deputados { get; set; }

    }
}
