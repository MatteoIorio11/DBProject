//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Hospital.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class HospitalEntities : DbContext
    {
        public HospitalEntities()
            : base("name=HospitalEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<chirurgo> chirurgoes { get; set; }
        public virtual DbSet<cura> curas { get; set; }
        public virtual DbSet<infermiere> infermieres { get; set; }
        public virtual DbSet<intervento> interventoes { get; set; }
        public virtual DbSet<medicina> medicinas { get; set; }
        public virtual DbSet<medico> medicos { get; set; }
        public virtual DbSet<paziente> pazientes { get; set; }
        public virtual DbSet<referto> refertoes { get; set; }
        public virtual DbSet<tipologia> tipologias { get; set; }
        public virtual DbSet<visita> visitas { get; set; }
    }
}
