using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SGSv3.Models;
using System.Threading.Tasks;
using System.Web.Security;
using SGSv3.Models.Views;
using SGSv3;
using System.Net.Mail;

namespace SGSv3.Controllers
{
    public class AccesController : Controller
    {

        private H2016_ProjetWeb_101_Equipe1Entities db = new H2016_ProjetWeb_101_Equipe1Entities();

        // GET: Acces
        public ActionResult Connexion(string ReturnUrl)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Connexion(AccesConnexion modele, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (Authentifier(modele.courriel, modele.mdp))
                {
                    var utilisateur = db.Utilisateurs.FirstOrDefault(u => u.courrielUTIL == modele.courriel);
                    FormsAuthentication.SetAuthCookie(utilisateur.noUTIL.ToString(), false);
                    Session["nomUTIL"] = utilisateur.nomUTIL + ", " + utilisateur.prenomUTIL;

                    if (returnUrl != null)
                        return Redirect(returnUrl);
                    else
                    {
                        if (utilisateur.noTYPE == 1)
                        {
                            return RedirectToAction("Index", "Detail_Semaine");
                        }
                        else
                        {
                            return RedirectToAction("Index", "Stages");
                        }
                    }
                        
                }
                else
                {
                    //L'authentification a échoué
                    ModelState.AddModelError("", "Identifiants invalides");
                }
            }

            // Les champs ne sont pas bien remplis
            return View(modele);
        }

        public Boolean Authentifier(String courriel, String mdp)
        {
            bool resultat = false;
            var utilisateur = db.Utilisateurs.FirstOrDefault(u => u.courrielUTIL == courriel && u.dateSuppression == null);
            if (utilisateur != null && PasswordHash.ValidatePassword(mdp, utilisateur.mdp))
            {
                resultat = true;
            }
            return resultat;
        }

        public ActionResult Deconnexion()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Connexion", "Acces");
        }

        public ActionResult ModifierMDP()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult ModifierMDP(AccesModifierMDP modele)
        {
            if (ModelState.IsValid)
            {
                if (modele.nouveauMDP == modele.confirmerNouveauMDP)
                {
                    // modifier le mot de passe
                    var utilisateur = db.Utilisateurs.FirstOrDefault(u => u.noUTIL.ToString() == User.Identity.Name);
                    utilisateur.mdp = PasswordHash.CreateHash(modele.nouveauMDP);
                    db.Entry(utilisateur).State = EntityState.Modified;
                    db.SaveChanges();

                    // rediriger à une page plus appropriée...
                    if (utilisateur.noTYPE == 1)
                    {
                        return RedirectToAction("Index", "Detail_Semaine", new { noEtudiant = utilisateur.noUTIL });
                    }
                    else
                    {
                        return RedirectToAction("Index", "Stages");
                    }
                }
                else
                {
                    // les mots de passe entrés ne correspondent pas
                    ModelState.AddModelError("", "Les mots de passe entrés ne correspondent pas");
                }
            }

            // Les champs ne sont pas bien remplis
            return View(modele);
        }

        //
        // GET: /Acces/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Acces/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(AccesForgotPassword modele, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (ValiderCourriel(modele.courriel))
                {
                    String mdp = PasswordHash.RandomString(10);
                    String mdpHash = PasswordHash.CreateHash(mdp);
                    var utilisateur = db.Utilisateurs.FirstOrDefault(u => u.courrielUTIL == modele.courriel);
                    utilisateur.mdp = mdpHash;
                    db.Entry(utilisateur).State = EntityState.Modified;
                    db.SaveChanges();

                    try
                    {
                        MailMessage message = new MailMessage("reinitialisation-mdp@cgmatane.qc.ca", modele.courriel, "Réinitialisation de votre MDP (SGS)", "Votre mot de passe a été réinitialisé.\n\nVotre nouveau mot de passe est : " + mdp + "\nPensez à changer de mot de passe si besoin.");
                        Courriel.envoyerCourriel(message);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Le mail n'a pas pu être envoyé. \n\n" + e.ToString());
                    }

                    if (returnUrl != null)
                        return Redirect(returnUrl);
                    else
                    {
                        if (utilisateur.noTYPE == 1)
                        {
                            return RedirectToAction("Index", "Detail_Semaine", new { noEtudiant = utilisateur.noUTIL });
                        }
                        else
                        {
                            return RedirectToAction("Index", "Stages");
                        }
                    }
                }
                else
                {
                    //L'authentification a échoué
                    ModelState.AddModelError("", "Courriel invalide");
                }
            }

            // Les champs ne sont pas bien remplis
            return View(modele);
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