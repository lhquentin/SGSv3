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
    public class ApprentissagesController : Controller
    {
        private H2016_ProjetWeb_101_Equipe1Entities db = new H2016_ProjetWeb_101_Equipe1Entities();

        public ActionResult _Create()
        {
            return PartialView();
        }

        public ActionResult IndexPartial(int? noStage, int? noSem)
        {
            var apprentissages = db.Apprentissages.Include(a => a.Detail_Semaine).Where(t => t.noSTAGE == noStage).Where(t => t.noSemaine == noSem);
            return PartialView("Index", apprentissages.ToList());
        }

        // GET: Apprentissages
        public ActionResult Index()
        {
            var apprentissages = db.Apprentissages.Include(a => a.Detail_Semaine);
            return View(apprentissages.ToList());
        }

        // GET: Apprentissages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Apprentissage apprentissage = db.Apprentissages.Find(id);
            if (apprentissage == null)
            {
                return HttpNotFound();
            }
            return View(apprentissage);
        }

        // GET: Apprentissages/Create
        public ActionResult Create()
        {
            ViewBag.noSTAGE = new SelectList(db.Detail_Semaine, "noSTAGE", "synthese");
            return View();
        }

        // POST: Apprentissages/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "noAPPR,noSTAGE,noSemaine,date,description")] Apprentissage apprentissage)
        {
            if (ModelState.IsValid)
            {
                db.Apprentissages.Add(apprentissage);
                db.SaveChanges();
                return RedirectToAction("Details", "Detail_Semaine", new { idStage = apprentissage.noSTAGE, idSemaine = apprentissage.noSemaine });
            }
            else
            {
                ModelState.AddModelError("", "Modèle invalide.");
            }

            ViewBag.noSTAGE = new SelectList(db.Detail_Semaine, "noSTAGE", "synthese", apprentissage.noSTAGE);
            return RedirectToAction("Details", "Detail_Semaine", new { idStage = apprentissage.noSTAGE, idSemaine = apprentissage.noSemaine });
        }

        // GET: Apprentissages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Apprentissage apprentissage = db.Apprentissages.Find(id);
            if (apprentissage == null)
            {
                return HttpNotFound();
            }
            ViewBag.noSTAGE = new SelectList(db.Detail_Semaine, "noSTAGE", "synthese", apprentissage.noSTAGE);
            return PartialView(apprentissage);
        }

        // POST: Apprentissages/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "noAPPR,noSTAGE,noSemaine,date,description")] Apprentissage apprentissage)
        {
            if (ModelState.IsValid)
            {
                db.Entry(apprentissage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Detail_Semaine", new { idStage = apprentissage.noSTAGE, idSemaine = apprentissage.noSemaine });
            }
            ViewBag.noSTAGE = new SelectList(db.Detail_Semaine, "noSTAGE", "synthese", apprentissage.noSTAGE);
            return View(apprentissage);
        }

        // GET: Apprentissages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Apprentissage apprentissage = db.Apprentissages.Find(id);
            if (apprentissage == null)
            {
                return HttpNotFound();
            }
            return PartialView("Delete", apprentissage);
        }

        // POST: Apprentissages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Apprentissage apprentissage = db.Apprentissages.Find(id);
            db.Apprentissages.Remove(apprentissage);
            db.SaveChanges();
            return RedirectToAction("Details", "Detail_Semaine", new { idStage = apprentissage.noSTAGE, idSemaine = apprentissage.noSemaine });
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
