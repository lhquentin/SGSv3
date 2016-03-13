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
    [Authorize(Roles = "Coordonnateur , Enseignant , Tuteur")]
    public class StagesController : Controller
    {
        private H2016_ProjetWeb_101_Equipe1Entities db = new H2016_ProjetWeb_101_Equipe1Entities();

        // GET: Stages
        public ActionResult Index()
        {
            var stages = db.Stages.Include(s => s.Entreprise).Include(s => s.Utilisateur).Include(s => s.Utilisateur1).Include(s => s.Utilisateur2).Where(s =>s.dateSuppression == null).OrderBy(s => s.dateDebut);
            var annee = from stage in db.Stages
                        orderby stage.dateDebut.Year.ToString()
                        select stage.dateDebut.Year.ToString().Union("Toutes");
            return View(stages.ToList());
        }

        // GET: Stages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stage stage = db.Stages.Find(id);
            if (stage == null)
            {
                return HttpNotFound();
            }
            return View(stage);
        }

        public ActionResult _AjoutEntreprise()
        {
            return View("_AjoutEntreprise");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _AjoutEntreprise([Bind(Include = "nomENT,adresseENT,villeENT,cpENT,telENT,faxENT,courrielENT,dateSuppression")] Entreprise entreprise)
        {
            if (ModelState.IsValid)
            {
                db.Entreprises.Add(entreprise);
                db.SaveChanges();
                return RedirectToAction("Create");
            }

            return View();
        }

        public ActionResult _AjoutTuteur()
        {
            return View("_AjoutTuteur");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _AjoutTuteur([Bind(Include = "nomUTIL,prenomUTIL,courrielUTIL,telUTIL,noTYPE,mdp,dateSuppression")] Utilisateur utilisateur)
        {
            if (ModelState.IsValid)
            {
                if (!ValiderCourriel(utilisateur.courrielUTIL))
                {
                    String mdp = PasswordHash.RandomString(10);

                    String mdpHash = PasswordHash.CreateHash(mdp);
                    utilisateur.mdp = mdpHash;
                    utilisateur.noTYPE = 4;
                    var type = db.Type_Utilisateur.FirstOrDefault(t => t.noTYPE == utilisateur.noTYPE);
                    db.Utilisateurs.Add(utilisateur);
                    db.SaveChanges();

                    try
                    {
                        MailMessage message = new MailMessage("no-reply@cgmatane.qc.ca", utilisateur.courrielUTIL, "Système de Gestion de Stage (SGS)", "Bienvenue " + utilisateur.nomUTIL + " " + utilisateur.prenomUTIL + ",\n\nVous avez été rajouté au module de gestion de stage en tant que " + type.libelleTYPE + ". \n\nVotre nouveau mot de passe est le suivant :\n" + mdp + "\n\nPensez à changer de mot de passe si besoin.");
                        Courriel.envoyerCourriel(message);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Le mail n'a pas pu être envoyé. \n\n" + e.ToString());
                    }

                    return RedirectToAction("Create");
                }
                else
                {
                    //L'authentification a échoué
                    ModelState.AddModelError("", "Courriel déjà existant.");
                }

            }

            ViewBag.noTYPE = new SelectList(db.Type_Utilisateur, "noTYPE", "libelleTYPE", utilisateur.noTYPE);

            return View();
        }

        // GET: Stages/Create
        public ActionResult Create()
        {
            var StagiaireEnvoi = from utilisateur in db.Utilisateurs
                                 where utilisateur.noTYPE == 1
                                 where !(from stage in db.Stages
                                         select stage.noStagiaire)
                                         .Contains(utilisateur.noUTIL)
                                 orderby utilisateur.nomUTIL
                                 select utilisateur;

            var Superviseur = db.Utilisateurs.Where(s => s.noTYPE == 2 || s.noTYPE == 3).OrderBy(s => s.nomUTIL);
            var Tuteur = db.Utilisateurs.Where(s => s.noTYPE == 4).OrderBy(s => s.nomUTIL);
            

            ViewBag.noENT = new SelectList(db.Entreprises.OrderBy(e => e.nomENT), "noENT", "nomENT");
            ViewBag.noStagiaire = new SelectList(StagiaireEnvoi, "noUTIL", "NomPrenom");
            ViewBag.noSuperviseur = new SelectList(Superviseur, "noUTIL", "NomPrenom");
            ViewBag.noTuteur = new SelectList(Tuteur, "noUTIL", "NomPrenom");
            return View();
        }

        // POST: Stages/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "noSTAGE,noStagiaire,noSuperviseur,noTuteur,noENT,dateDebut,nbSemaine,description")] Stage stage)
        {
            if (ModelState.IsValid)
            {

                var date = stage.dateDebut; 
                db.Stages.Add(stage);
                

                for (var i = 1; i < stage.nbSemaine + 1; i++)
                {
                    Detail_Semaine Detail_Semaine = new Detail_Semaine();
                    Detail_Semaine.noSTAGE = stage.noSTAGE;
                    Detail_Semaine.noSemaine = i;
                    
                    db.Entry(Detail_Semaine).State = EntityState.Added;
                    date = date.AddDays(7);
                    Detail_Semaine.dateDebut = date;
                    db.SaveChanges();
                }
                db.SaveChanges();
                TempData["Msg"] = "Les données ont été enregistrées";
                return RedirectToAction("Index");
            }

            ViewBag.noENT = new SelectList(db.Entreprises.Where(e => e.dateSuppression == null).OrderBy(e => e.nomENT), "noENT", "nomENT", stage.noENT);
            ViewBag.noStagiaire = new SelectList(db.Utilisateurs.Where(u => u.dateSuppression == null).OrderBy(u => u.nomUTIL), "noUTIL", "nomUTIL", stage.noStagiaire);
            ViewBag.noSuperviseur = new SelectList(db.Utilisateurs.Where(u => u.dateSuppression == null).OrderBy(u => u.nomUTIL), "noUTIL", "nomUTIL", stage.noSuperviseur);
            ViewBag.noTuteur = new SelectList(db.Utilisateurs.Where(u => u.dateSuppression == null).OrderBy(u => u.nomUTIL), "noUTIL", "nomUTIL", stage.noTuteur);
            return View(stage);
        }

        // GET: Stages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stage stage = db.Stages.Find(id);
            if (stage == null)
            {
                return HttpNotFound();
            }

            var Stagiaire = db.Utilisateurs.Where(u => u.dateSuppression == null).Where(s => s.noTYPE == 1).OrderBy(u => u.nomUTIL);
            var Superviseur = db.Utilisateurs.Where(u => u.dateSuppression == null).Where(s => s.noTYPE == 2 || s.noTYPE == 3).OrderBy(u => u.nomUTIL);
            var Tuteur = db.Utilisateurs.Where(u => u.dateSuppression == null).Where(s => s.noTYPE == 4).OrderBy(u => u.nomUTIL);

            ViewBag.noENT = new SelectList(db.Entreprises.OrderBy(e => e.nomENT), "noENT", "nomENT", stage.noENT);
            ViewBag.noStagiaire = new SelectList(Stagiaire, "noUTIL", "NomPrenom", stage.noStagiaire);
            ViewBag.noSuperviseur = new SelectList(Superviseur, "noUTIL", "NomPrenom", stage.noSuperviseur);
            ViewBag.noTuteur = new SelectList(Tuteur, "noUTIL", "NomPrenom", stage.noTuteur);
            return View(stage);
        }

        // POST: Stages/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "noSTAGE,noStagiaire,noSuperviseur,noTuteur,noENT,dateDebut,nbSemaine,description,dateSuppression")] Stage stage)
        {
            if (ModelState.IsValid)
            {
                Boolean verification = true;
                var semaines = db.Detail_Semaine.Where(d => d.noSTAGE == stage.noSTAGE).OrderBy(d => d.noSemaine);
                foreach(var semaine in semaines)
                {
                    var apprentissages = db.Apprentissages.Where(a => a.noSTAGE == stage.noSTAGE && a.noSemaine == semaine.noSemaine);
                    var difficultes = db.Difficultes.Where(d => d.noSTAGE == stage.noSTAGE && d.noSemaine == semaine.noSemaine);
                    var taches = db.Taches.Where(t => t.noSTAGE == stage.noSTAGE && t.noSemaine == semaine.noSemaine);
                    
                    Boolean verifApprentissage = false;
                    Boolean verifDifficulte = false;
                    Boolean verifTache = false;

                    if(apprentissages != null)
                    {
                        foreach (var apprentissage in apprentissages)
                        {
                            if (apprentissage.date != null) { verifApprentissage = true; }
                        }
                    }

                    if (difficultes != null)
                    {
                        foreach (var difficulte in difficultes)
                        {
                            if (difficulte.description != null) { verifDifficulte = true; }
                        }
                    }

                    if (taches != null)
                    {
                        foreach (var tache in taches)
                        {
                            if (tache.date != null) { verifTache = true; }
                        }
                    }

                    if (semaine.noSemaine > stage.nbSemaine && ((semaine.synthese != null) ||
                        (semaine.horaire != null) || (semaine.envMateriel != null) ||
                        (semaine.envLogiciel != null) || (semaine.commSuperviseur != null) ||
                        (semaine.commTuteur != null) || verifApprentissage ||
                        verifDifficulte || verifTache))
                    {
                        verification = false;
                    }
                    else if (semaine.noSemaine > stage.nbSemaine)
                    {
                        db.Detail_Semaine.Remove(semaine);
                    }
                }
                var max = db.Detail_Semaine.Where(d => d.noSTAGE == stage.noSTAGE).Max(d => d.noSemaine);
                var derniereSemaine = db.Detail_Semaine.FirstOrDefault(d => d.noSTAGE == stage.noSTAGE && d.noSemaine == max);
                if (derniereSemaine.noSemaine < stage.nbSemaine)
                {
                    var date = derniereSemaine.dateDebut;
                    for (int i = 1; i <= (stage.nbSemaine - derniereSemaine.noSemaine); i++)
                    {
                        Detail_Semaine Detail_Semaine = new Detail_Semaine();
                        Detail_Semaine.noSTAGE = stage.noSTAGE;
                        Detail_Semaine.noSemaine = derniereSemaine.noSemaine + i;
                        db.Entry(Detail_Semaine).State = EntityState.Added;
                        date = date.AddDays(7);
                        Detail_Semaine.dateDebut = date;
                        db.SaveChanges();
                    }
                }
                if(verification)
                {
                    db.Entry(stage).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["Msg"] = "Les données ont été modifiées";
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Des semaines sont déjà remplies et ne peuvent être supprimées.");
                }
            }
            ViewBag.noENT = new SelectList(db.Entreprises.OrderBy(e => e.nomENT), "noENT", "nomENT", stage.noENT);
            ViewBag.noStagiaire = new SelectList(db.Utilisateurs.OrderBy(u => u.nomUTIL), "noUTIL", "nomUTIL", stage.noStagiaire);
            ViewBag.noSuperviseur = new SelectList(db.Utilisateurs.OrderBy(u => u.nomUTIL), "noUTIL", "nomUTIL", stage.noSuperviseur);
            ViewBag.noTuteur = new SelectList(db.Utilisateurs.OrderBy(u => u.nomUTIL), "noUTIL", "nomUTIL", stage.noTuteur);
            return View(stage);
        }

        // GET: Stages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stage stage = db.Stages.Find(id);
            if (stage == null)
            {
                return HttpNotFound();
            }
            return View(stage);
        }

        // POST: Stages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Stage stage = db.Stages.Find(id);
            stage.dateSuppression = DateTime.Now;
            db.Entry(stage).State = EntityState.Modified;
            db.SaveChanges();
            TempData["Msg"] = "Les données ont été supprimées";
            return RedirectToAction("Index");
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
