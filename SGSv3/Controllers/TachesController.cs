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
    public class TachesController : Controller
    {
        private H2016_ProjetWeb_101_Equipe1Entities db = new H2016_ProjetWeb_101_Equipe1Entities();

        public ActionResult _Create()
        {
            return PartialView();
        }

        public ActionResult IndexPartial(int? noStage, int? noSem)
        {
            var taches = db.Taches.Include(t => t.Detail_Semaine).Where(t => t.noSTAGE == noStage).Where(t => t.noSemaine == noSem);
            return PartialView("Index", taches.ToList());
        }

        // GET: Taches
        public ActionResult Index()
        {
            var taches = db.Taches.Include(t => t.Detail_Semaine);
            return View(taches.ToList());
        }

        // GET: Taches/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tache tache = db.Taches.Find(id);
            if (tache == null)
            {
                return HttpNotFound();
            }
            return View(tache);
        }

        // GET: Taches/Create
        public ActionResult Create()
        {
            ViewBag.noSTAGE = new SelectList(db.Detail_Semaine, "noSTAGE", "synthese");
            return View();
        }

        // POST: Taches/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "noTACHE,noSTAGE,noSemaine,date,duree,description")] Tache tache)
        {
            if (ModelState.IsValid)
            {
                db.Taches.Add(tache);
                db.SaveChanges();
                return RedirectToAction("Details", "Detail_Semaine", new { idStage = tache.noSTAGE, idSemaine = tache.noSemaine });
            }
            else
            {
                ModelState.AddModelError("", "Modèle invalide.");
            }

            ViewBag.noSTAGE = new SelectList(db.Detail_Semaine, "noSTAGE", "synthese", tache.noSTAGE);
            return RedirectToAction("Details", "Detail_Semaine", new { idStage = tache.noSTAGE, idSemaine = tache.noSemaine });
        }

        // GET: Taches/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tache tache = db.Taches.Find(id);
            if (tache == null)
            {
                return HttpNotFound();
            }
            ViewBag.noSTAGE = new SelectList(db.Detail_Semaine, "noSTAGE", "synthese", tache.noSTAGE);
            return PartialView(tache);
        }

        // POST: Taches/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "noTACHE,noSTAGE,noSemaine,date,duree,description")] Tache tache)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tache).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Detail_Semaine", new { idStage = tache.noSTAGE, idSemaine = tache.noSemaine });
            }
            ViewBag.noSTAGE = new SelectList(db.Detail_Semaine, "noSTAGE", "synthese", tache.noSTAGE);
            return View(tache);
        }

        // GET: Taches/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tache tache = db.Taches.Find(id);
            if (tache == null)
            {
                return HttpNotFound();
            }
            return PartialView("Delete",tache);
        }

        // POST: Taches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tache tache = db.Taches.Find(id);
            db.Taches.Remove(tache);
            db.SaveChanges();
            return RedirectToAction("Details", "Detail_Semaine", new { idStage = tache.noSTAGE, idSemaine = tache.noSemaine });
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
