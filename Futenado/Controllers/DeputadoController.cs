/* Copyright 2013 de Victor Ribeiro da Silva Eloy Este arquivo é parte do programa FUTECAMARA. O FUTECAMARA é um software livre; você pode redistribuí-lo e/ou modificá-lo dentro dos termos da GNU General Public License como publicada pela Fundação do Software Livre (FSF); na versão 3 da Licença. Este programa é distribuído na esperança que possa ser útil, mas SEM NENHUMA GARANTIA; sem uma garantia implícita de ADEQUAÇÃO a qualquer MERCADO ou APLICAÇÃO EM PARTICULAR. Veja a licença para maiores detalhes. Você deve ter recebido uma cópia da GNU General Public License, sob o título "LICENSE", junto com este programa, se não, acesse http://www.gnu.org/licenses/ */

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Futenado.Models;

namespace Futenado.Controllers
{
    public class DeputadoController : Controller
    {
        private EntityContext db = new EntityContext();

        //
        // GET: /Deputado/

        public ActionResult Index()
        {
            return View(db.Deputadoes.ToList());
        }

        //
        // GET: /Deputado/Details/5

        public ActionResult Details(int id = 0)
        {
            Deputado deputado = db.Deputadoes.Find(id);
            if (deputado == null)
            {
                return HttpNotFound();
            }
            return View(deputado);
        }

        //
        // GET: /Deputado/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Deputado/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Deputado deputado)
        {
            if (ModelState.IsValid)
            {
                db.Deputadoes.Add(deputado);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(deputado);
        }

        //
        // GET: /Deputado/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Deputado deputado = db.Deputadoes.Find(id);
            if (deputado == null)
            {
                return HttpNotFound();
            }
            return View(deputado);
        }

        //
        // POST: /Deputado/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Deputado deputado)
        {
            if (ModelState.IsValid)
            {
                db.Entry(deputado).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(deputado);
        }

        //
        // GET: /Deputado/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Deputado deputado = db.Deputadoes.Find(id);
            if (deputado == null)
            {
                return HttpNotFound();
            }
            return View(deputado);
        }

        //
        // POST: /Deputado/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Deputado deputado = db.Deputadoes.Find(id);
            db.Deputadoes.Remove(deputado);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}