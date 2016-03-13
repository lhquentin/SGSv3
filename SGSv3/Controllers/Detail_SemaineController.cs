using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SGSv3.Models;
using System.Net.Mail;

namespace SGSv3.Controllers
{
    [Authorize(Roles = "Coordonnateur , Enseignant, Etudiant, Tuteur")]
    public class Detail_SemaineController : Controller
    {
        private H2016_ProjetWeb_101_Equipe1Entities db = new H2016_ProjetWeb_101_Equipe1Entities();
        
        // GET: Detail_Semaine/Index
        public ActionResult Index(int? noEtudiant, int? noStage)
        {
            if(noStage == null)
            {
                DateTime date = DateTime.Now;
                if(User.IsInRole("Etudiant"))
                {
                    var detail_Semaine = db.Detail_Semaine.Include(d => d.Stage).Where(d => d.Stage.noStagiaire.ToString() == User.Identity.Name).Where(d => d.Stage.dateDebut.Year == DateTime.Now.Year);
                    return View(detail_Semaine.ToList());
                }
                else
                {
                    var detail_Semaine = db.Detail_Semaine.Include(d => d.Stage).Where(d => d.Stage.noStagiaire == noEtudiant).Where(d => d.Stage.dateDebut.Year == DateTime.Now.Year);
                    return View(detail_Semaine.ToList());
                }
            }
            else
            {
                var detail_Semaine = db.Detail_Semaine.Include(d => d.Stage).Where(d => d.Stage.noSTAGE == noStage);
                return View(detail_Semaine.ToList());
            }
        }

        // GET: Detail_Semaine/Details/5
        public ActionResult Details(int? idStage, int? idSemaine)
        {
            if (idStage == null || idSemaine == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detail_Semaine detail_Semaine = db.Detail_Semaine.Find(idStage, idSemaine);
            if (detail_Semaine == null)
            {
                return HttpNotFound();
            }
            return View(detail_Semaine);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DetailsEtudiant([Bind(Include = "noSTAGE,noSemaine,synthese,horaire,envMateriel,envLogiciel")] Detail_Semaine detail_Semaine_Etudiant, String submit)
        {
            if (ModelState.IsValid)
            {
                var donneesBDD = db.Detail_Semaine.FirstOrDefault(d => d.noSTAGE == detail_Semaine_Etudiant.noSTAGE && d.noSemaine == detail_Semaine_Etudiant.noSemaine);
                switch (submit)
                {
                    case "Soumettre":
                        donneesBDD.soumisCahier = true;
                        var stage = db.Stages.FirstOrDefault(s => s.noSTAGE == detail_Semaine_Etudiant.noSTAGE);
                        var utilisateur = db.Utilisateurs.FirstOrDefault(u => u.noUTIL == stage.noSuperviseur);
                        try
                        {
                            MailMessage message = new MailMessage("no-reply@cgmatane.qc.ca", utilisateur.courrielUTIL, "Cahier de " + utilisateur.nomPrenom + " (semaine " + detail_Semaine_Etudiant.noSemaine + ")", utilisateur.nomPrenom + " a soumis son cahier pour la semaine " + detail_Semaine_Etudiant.noSemaine + ".");
                            Courriel.envoyerCourriel(message);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Le mail n'a pas pu être envoyé. \n\n" + e.ToString());
                        }
                        break;
                    case "Désoumettre":
                        donneesBDD.soumisCahier = false;
                        break;
                }
                db.Entry(donneesBDD).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { idStage = detail_Semaine_Etudiant.noSTAGE, idSemaine = detail_Semaine_Etudiant.noSemaine });
            }
            ViewBag.noSTAGE = new SelectList(db.Stages, "noSTAGE", "description", detail_Semaine_Etudiant.noSTAGE);
            return View(detail_Semaine_Etudiant);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DetailsSuperviseur([Bind(Include = "noSTAGE,noSemaine,commSuperviseur")] Detail_Semaine detail_Semaine_Superviseur, String submit)
        {
            if (ModelState.IsValid)
            {
                var donneesBDD = db.Detail_Semaine.FirstOrDefault(d => d.noSTAGE == detail_Semaine_Superviseur.noSTAGE && d.noSemaine == detail_Semaine_Superviseur.noSemaine);
                switch (submit)
                {
                    case "Soumettre":
                        donneesBDD.soumisSuperviseur = true;
                        var stage = db.Stages.FirstOrDefault(s => s.noSTAGE == detail_Semaine_Superviseur.noSTAGE);
                        var utilisateur = db.Utilisateurs.FirstOrDefault(u => u.noUTIL == stage.noStagiaire);
                        try
                        {
                            MailMessage message = new MailMessage("no-reply@cgmatane.qc.ca", utilisateur.courrielUTIL, "Commentaire du superviseur (semaine " + detail_Semaine_Superviseur.noSemaine + ")", "Votre superviseur a soumis le commentaire de la semaine " + detail_Semaine_Superviseur.noSemaine + ": \n\n" + detail_Semaine_Superviseur.commSuperviseur);
                            Courriel.envoyerCourriel(message);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Le mail n'a pas pu être envoyé. \n\n" + e.ToString());
                        }
                    break;
                    case "Désoumettre":
                    donneesBDD.soumisSuperviseur = false;
                    break;
                }
                donneesBDD.commSuperviseur = detail_Semaine_Superviseur.commSuperviseur;
                db.Entry(donneesBDD).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { idStage = detail_Semaine_Superviseur.noSTAGE, idSemaine = detail_Semaine_Superviseur.noSemaine });
            }
            ViewBag.noSTAGE = new SelectList(db.Stages, "noSTAGE", "description", detail_Semaine_Superviseur.noSTAGE);
            return View(detail_Semaine_Superviseur);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DetailsTuteur([Bind(Include = "noSTAGE,noSemaine,commTuteur")] Detail_Semaine detail_Semaine_Tuteur, String submit)
        {
            if (ModelState.IsValid)
            {
                var donneesBDD = db.Detail_Semaine.FirstOrDefault(d => d.noSTAGE == detail_Semaine_Tuteur.noSTAGE && d.noSemaine == detail_Semaine_Tuteur.noSemaine);
                switch (submit)
                {
                    case "Soumettre":
                        donneesBDD.soumisTuteur = true;
                        var stage = db.Stages.FirstOrDefault(s => s.noSTAGE == detail_Semaine_Tuteur.noSTAGE);
                        var utilisateur1 = db.Utilisateurs.FirstOrDefault(u => u.noUTIL == stage.noStagiaire);
                        var utilisateur2 = db.Utilisateurs.FirstOrDefault(u => u.noUTIL == stage.noSuperviseur);
                        try
                        {
                            MailMessage message1 = new MailMessage("no-reply@cgmatane.qc.ca", utilisateur1.courrielUTIL, "Commentaire du tuteur (semaine " + detail_Semaine_Tuteur.noSemaine + ")", "Votre tuteur a soumis le commentaire de la semaine " + detail_Semaine_Tuteur.noSemaine + ": \n\n" + detail_Semaine_Tuteur.commTuteur);
                            Courriel.envoyerCourriel(message1);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Le mail n'a pas pu être envoyé. \n\n" + e.ToString());
                        }
                        try
                        {
                            MailMessage message2 = new MailMessage("no-reply@cgmatane.qc.ca", utilisateur2.courrielUTIL, "Commentaire du tuteur (semaine " + detail_Semaine_Tuteur.noSemaine + ")", "Le tuteur a soumis le commentaire de la semaine " + detail_Semaine_Tuteur.noSemaine + ": \n\n" + detail_Semaine_Tuteur.commTuteur);
                            Courriel.envoyerCourriel(message2);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Le mail n'a pas pu être envoyé. \n\n" + e.ToString());
                        }
                     break;
                     case "Désoumettre":
                     donneesBDD.soumisTuteur = false;
                     break;
                }
                donneesBDD.commTuteur = detail_Semaine_Tuteur.commTuteur;
                db.Entry(donneesBDD).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { idStage = detail_Semaine_Tuteur.noSTAGE, idSemaine = detail_Semaine_Tuteur.noSemaine });
            }
            ViewBag.noSTAGE = new SelectList(db.Stages, "noSTAGE", "description", detail_Semaine_Tuteur.noSTAGE);
            return View(detail_Semaine_Tuteur);
        }

        // GET: Detail_Semaine/Create
        public ActionResult Create()
        {
            ViewBag.noSTAGE = new SelectList(db.Stages, "noSTAGE", "description");
            return View();
        }

        // POST: Detail_Semaine/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "noSTAGE,noSemaine,synthese,horaire,envMateriel,envLogiciel,commSuperviseur,commTuteur,soumisCahier,soumisSuperviseur,soumisTuteur")] Detail_Semaine detail_Semaine)
        {
            if (ModelState.IsValid)
            {
                db.Detail_Semaine.Add(detail_Semaine);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.noSTAGE = new SelectList(db.Stages, "noSTAGE", "description", detail_Semaine.noSTAGE);
            return View(detail_Semaine);
        }

        // GET: Detail_Semaine/Edit/5
        public ActionResult Edit(int? idStage, int? idSemaine)
        {
            if (idStage == null || idSemaine == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detail_Semaine detail_Semaine = db.Detail_Semaine.Find(idStage, idSemaine);
            if (detail_Semaine == null)
            {
                return HttpNotFound();
            }
            ViewBag.noSTAGE = new SelectList(db.Stages, "noSTAGE", "description", detail_Semaine.noSTAGE);
            return View(detail_Semaine);
        }

        // POST: Detail_Semaine/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "noSTAGE,noSemaine,synthese,horaire,envMateriel,envLogiciel,commSuperviseur,commTuteur,soumisCahier,soumisSuperviseur,soumisTuteur")] Detail_Semaine detail_Semaine)
        {
            if (ModelState.IsValid)
            {
                db.Entry(detail_Semaine).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Detail_Semaine", new { idStage = detail_Semaine.noSTAGE, idSemaine = detail_Semaine.noSemaine });
            }
            ViewBag.noSTAGE = new SelectList(db.Stages, "noSTAGE", "description", detail_Semaine.noSTAGE);
            return View(detail_Semaine);
        }

        // GET: Detail_Semaine/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detail_Semaine detail_Semaine = db.Detail_Semaine.Find(id);
            if (detail_Semaine == null)
            {
                return HttpNotFound();
            }
            return View(detail_Semaine);
        }

        // POST: Detail_Semaine/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Detail_Semaine detail_Semaine = db.Detail_Semaine.Find(id);
            db.Detail_Semaine.Remove(detail_Semaine);
            db.SaveChanges();
            return RedirectToAction("Index");
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
