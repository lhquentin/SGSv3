using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Security.Principal;
using SGSv3.Models;
using System.Web.Security;

namespace SGSv3
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private H2016_ProjetWeb_101_Equipe1Entities db = new H2016_ProjetWeb_101_Equipe1Entities();

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs args)
        {
            if (Context.User != null)
            {
                // récupérer le(s) rôle/type(s) associé(s) à l’utilisateur qui vient de s’authentifier
                // (remplacer la ligne suivante par une requête qui récupère le(s) role/type(s) de l'utilisateur
                // d'après la valeur de Context.User.Identity.Name)
                // --> dans notre application, il n'y aura qu'un seul role (Coordonnateur, Superviseur, Entreprise ou Étudiant)
                var utilisateur = db.Utilisateurs.FirstOrDefault(u => u.noUTIL.ToString() == User.Identity.Name);
                List<String> roles = new List<string>() { utilisateur.Type_Utilisateur.libelleTYPE };

                // on remplace le cookie d'authentification qui avait été créé par notre propre cookie, qui contient le(s) role(s) de l'utilisateur
                GenericPrincipal gp = new GenericPrincipal(Context.User.Identity, roles.ToArray());
                Context.User = gp;
            }
        }
    }
}
