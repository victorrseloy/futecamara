using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Futenado.ViewModels
{
    public class ClassificacaoViewModel
    {
        public string NomeTime { get; set; }
        public string NomeJogador { get; set; }
        public string CaminhoBrasao { get; set; }
        public int Vitorias { get; set; }
        public int Derrotas { get; set; }
        public int Empates { get; set; }
        public int Jogos { get { return Vitorias + Derrotas + Empates; } }
        public int Pontos
        {
            get { return Vitorias * 3 + Empates; }
        }
        public int Gols { get; set; }

    }
}