﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApplication2.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class hospitaldbEntities : DbContext
    {
        public hospitaldbEntities()
            : base("name=hospitaldbEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<appartenenza> appartenenzas { get; set; }
        public virtual DbSet<assegnazionecura> assegnazionecuras { get; set; }
        public virtual DbSet<assiste> assistes { get; set; }
        public virtual DbSet<assistenza> assistenzas { get; set; }
        public virtual DbSet<chirurgo> chirurgoes { get; set; }
        public virtual DbSet<cura> curas { get; set; }
        public virtual DbSet<effettua> effettuas { get; set; }
        public virtual DbSet<esecuzione> esecuziones { get; set; }
        public virtual DbSet<infermiere> infermieres { get; set; }
        public virtual DbSet<intervento> interventoes { get; set; }
        public virtual DbSet<intervento_tipologia> intervento_tipologia { get; set; }
    }
}
