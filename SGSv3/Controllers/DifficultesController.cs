using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SGSv3.Models;

namespace SGSv3.Controllers
{
    public class DifficultesController : Controller
    {
        private H2016_ProjetWeb_101_Equipe1Entities db = new H2016_ProjetWeb_101_Equipe1Entities();

        public ActionResult _Create()
        {
            return PartialView();
        }

        public ActionResult IndexPartial(int? noStage, int? noSem)
        {
            var difficultes = db.Difficultes.Include(d => d.Detail_Semaine).Where(t => t.noSTAGE == noStage).Where(t => t.noSemaine == noSem);
            return PartialView("Index", difficultes.ToList());
        }

        // GET: Difficultes
        public ActionResult Index()
        {
            var difficultes = db.Difficultes.Include(d => d.Detail_Semaine);
            return View(difficultes.ToList());
        }

        // GET: Difficultes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Difficulte difficulte = db.Difficultes.Find(id);
            if (difficulte == null)
            {
                return HttpNotFound();
            }
            return View(difficulte);
        }

        // GET: Difficultes/Create
        public ActionResult Create()
        {
            ViewBag.noSTAGE = new SelectList(db.Detail_Semaine, "noSTAGE", "synthese");
            return View();
        }

        // POST: Difficultes/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "noDIFFIC,noSTAGE,noSemaine,description,moyen,resultat")] Difficulte difficulte)
        {
            if (ModelState.IsValid)
            {
                db.Difficultes.Add(difficulte);
                db.SaveChanges();
                return RedirectToAction("Details", "Detail_Semaine", new { idStage = difficulte.noSTAGE, idSemaine = difficulte.noSemaine });
            }
            else
            {
                ModelState.AddModelError("", "Modèle invalide.");
            }

            ViewBag.noSTAGE = new SelectList(db.Detail_Semaine, "noSTAGE", "synthese", difficulte.noSTAGE);
            return RedirectToAction("Details", "Detail_Semaine", new { idStage = difficulte.noSTAGE, idSemaine = difficulte.noSemaine });
        }

        // GET: Difficultes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Difficulte difficulte = db.Difficultes.Find(id);
            if (difficulte == null)
            {
                return HttpNotFound();
            }
            ViewBag.noSTAGE = new SelectList(db.Detail_Semaine, "noSTAGE", "synthese", difficulte.noSTAGE);
            return PartialView(difficulte);
        }

        // POST: Difficultes/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "noDIFFIC,noSTAGE,noSemaine,description,moyen,resultat")] Difficulte difficulte)
        {
            if (ModelState.IsValid)
            {
                db.Entry(difficulte).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Detail_Semaine", new { idStage = difficulte.noSTAGE, idSemaine = difficulte.noSemaine });
            }
            ViewBag.noSTAGE = new SelectList(db.Detail_Semaine, "noSTAGE", "synthese", difficulte.noSTAGE);
            return View(difficulte);
        }

        // GET: Difficultes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Difficulte difficulte = db.Difficultes.Find(id);
            if (difficulte == null)
            {
                return HttpNotFound();
            }
            return PartialView("Delete", difficulte);
        }

        // POST: Difficultes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Difficulte difficulte = db.Difficultes.Find(id);
            db.Difficultes.Remove(difficulte);
            db.SaveChanges();
            return RedirectToAction("Details", "Detail_Semaine", new { idStage = difficulte.noSTAGE, idSemaine = difficulte.noSemaine });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
