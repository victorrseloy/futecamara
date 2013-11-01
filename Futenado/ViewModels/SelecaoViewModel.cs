using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Futenado.Models;

namespace Futenado.ViewModels
{
    public class SelecaoViewModel
    {
        public SelecaoViewModel()
        {
            MeuTime = new List<Deputado>();
            TodosOsDeputados = new List<Deputado>();
        }

        public List<Deputado> MeuTime { get; set; }
        public List<Deputado> TodosOsDeputados { get; set; }

    }
}