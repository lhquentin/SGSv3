﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré à partir d'un modèle.
//
//     Des modifications manuelles apportées à ce fichier peuvent conduire à un comportement inattendu de votre application.
//     Les modifications manuelles apportées à ce fichier sont remplacées si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SGSv3.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class H2016_ProjetWeb_101_Equipe1Entities : DbContext
    {
        public H2016_ProjetWeb_101_Equipe1Entities()
            : base("name=H2016_ProjetWeb_101_Equipe1Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Apprentissage> Apprentissages { get; set; }
        public virtual DbSet<Detail_Semaine> Detail_Semaine { get; set; }
        public virtual DbSet<Difficulte> Difficultes { get; set; }
        public virtual DbSet<Entreprise> Entreprises { get; set; }
        public virtual DbSet<Stage> Stages { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<Tache> Taches { get; set; }
        public virtual DbSet<Type_Utilisateur> Type_Utilisateur { get; set; }
        public virtual DbSet<Utilisateur> Utilisateurs { get; set; }
    }
}
