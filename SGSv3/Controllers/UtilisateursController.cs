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
using System.Text;
using SGSv3;

namespace SGSv3.Controllers
{
    [Authorize(Roles = "Coordonnateur")]
    public class UtilisateursController : Controller
    {
        private H2016_ProjetWeb_101_Equipe1Entities db = new H2016_ProjetWeb_101_Equipe1Entities();

        // GET: Utilisateurs
        public ActionResult Index()
        {
            var utilisateurs = db.Utilisateurs.Include(u => u.Type_Utilisateur).Where(u => u.dateSuppression == null);
            return View(utilisateurs.ToList());
        }

        // GET: Utilisateurs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Utilisateur utilisateur = db.Utilisateurs.Find(id);
            if (utilisateur == null)
            {
                return HttpNotFound();
            }
            return View(utilisateur);
        }

        // GET: Utilisateurs/Create
        public ActionResult Create()
        {
            ViewBag.noTYPE = new SelectList(db.Type_Utilisateur, "noTYPE", "libelleTYPE");
            return View();
        }

        // POST: Utilisateurs/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "noUTIL,nomUTIL,prenomUTIL,courrielUTIL,telUTIL,noTYPE,mdp,dateSuppression")] Utilisateur utilisateur)
        {
            if (ModelState.IsValid)
            {
                if (!ValiderCourriel(utilisateur.courrielUTIL))
                {
                    String mdp = PasswordHash.RandomString(10);

                    var type = db.Type_Utilisateur.FirstOrDefault(t => t.noTYPE == utilisateur.noTYPE);

                    String mdpHash = PasswordHash.CreateHash(mdp);
                    utilisateur.mdp = mdpHash;

                    db.Utilisateurs.Add(utilisateur);
                    db.SaveChanges();

                    try
                    {
                        MailMessage message = new MailMessage("no-reply@cgmatane.qc.ca", utilisateur.courrielUTIL, "Système de Gestion de Stage (SGS)", "Bienvenu " + utilisateur.nomUTIL + " " + utilisateur.prenomUTIL + ",\n\nVous avez été rajouté au module de gestion de stage en tant que " + type.libelleTYPE + ". \n\nVotre nouveau mot de passe est le suivant :\n" + mdp + "\n\nPensez à changer de mot de passe si besoin.");
                        Courriel.envoyerCourriel(message);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Le mail n'a pas pu être envoyé. \n\n" + e.ToString());
                    }

                    return RedirectToAction("Index");
                }
                else
                {
                    //L'authentification a échoué
                    ModelState.AddModelError("", "Courriel déjà existant.");
                }
                
            }

            ViewBag.noTYPE = new SelectList(db.Type_Utilisateur, "noTYPE", "libelleTYPE", utilisateur.noTYPE);

            return View(utilisateur);
        }

        // GET: Utilisateurs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Utilisateur utilisateur = db.Utilisateurs.Find(id);
            if (utilisateur == null)
            {
                return HttpNotFound();
            }
            ViewBag.noTYPE = new SelectList(db.Type_Utilisateur, "noTYPE", "libelleTYPE", utilisateur.noTYPE);
            return View(utilisateur);
        }

        // POST: Utilisateurs/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "noUTIL,nomUTIL,prenomUTIL,courrielUTIL,telUTIL,noTYPE,dateSuppression")] Utilisateur utilisateur)
        {
            if (ModelState.IsValid)
            {
                db.Entry(utilisateur).State = EntityState.Modified;
                db.Entry(utilisateur).Property(u => u.mdp).IsModified = false;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.noTYPE = new SelectList(db.Type_Utilisateur, "noTYPE", "libelleTYPE", utilisateur.noTYPE);
            return View(utilisateur);
        }

        // GET: Utilisateurs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Utilisateur utilisateur = db.Utilisateurs.Find(id);
            if (utilisateur == null)
            {
                return HttpNotFound();
            }
            return View(utilisateur);
        }

        // POST: Utilisateurs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Utilisateur utilisateur = db.Utilisateurs.Find(id);
            //db.Utilisateurs.Remove(utilisateur);
            utilisateur.dateSuppression = DateTime.Now;
            db.Entry(utilisateur).State = EntityState.Modified;
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

        public Boolean ValiderCourriel(String courriel)
        {
            bool resultat = false;
            var utilisateur = db.Utilisateurs.FirstOrDefault(u => u.courrielUTIL == courriel && u.dateSuppression == null);
            if (utilisateur != null)
            {
                resultat = true;
            }
            return resultat;
        }
    }
}
