/* Copyright 2013 de Victor Ribeiro da Silva Eloy Este arquivo é parte do programa FUTECAMARA. O FUTECAMARA é um software livre; você pode redistribuí-lo e/ou modificá-lo dentro dos termos da GNU General Public License como publicada pela Fundação do Software Livre (FSF); na versão 3 da Licença. Este programa é distribuído na esperança que possa ser útil, mas SEM NENHUMA GARANTIA; sem uma garantia implícita de ADEQUAÇÃO a qualquer MERCADO ou APLICAÇÃO EM PARTICULAR. Veja a licença para maiores detalhes. Você deve ter recebido uma cópia da GNU General Public License, sob o título "LICENSE", junto com este programa, se não, acesse http://www.gnu.org/licenses/ */

using Futenado.Components;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Web;


[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(SampleAspNetTimer), "Start")]
public static class SampleAspNetTimer
{
    private static readonly Timer _timer = new Timer(OnTimerElapsed);
    private static readonly JobHost _jobHost = new JobHost();

    public static void Start()
    {
        //dispara a cada 5 horas
        _timer.Change(TimeSpan.Zero, TimeSpan.FromMilliseconds(1000*60*60*5));
    }

    private static void OnTimerElapsed(object sender)
    {
        _jobHost.DoWork(() =>
        {

            {
                //faz uma requisicao para mim msm.
                var client = new RestClient(ConfigurationManager.AppSettings["host"]);
                var request = new RestRequest("/Backend/ExecutarRodada", Method.GET);
                request.AddParameter("data", DateTime.Now.ToString("MM-dd-yyy"));
                request.RequestFormat = DataFormat.Xml;
                var queryResult = client.Execute(request);
            }


            {
                //faz uma requisicao para mim msm.
                var client = new RestClient(ConfigurationManager.AppSettings["host"]);
                var request = new RestRequest("/Backend/CalcularAssiduidade", Method.GET);
                request.RequestFormat = DataFormat.Xml;
                var queryResult = client.Execute(request);
            }

        });
    }
}
