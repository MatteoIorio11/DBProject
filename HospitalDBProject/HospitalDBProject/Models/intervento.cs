//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HospitalDBProject.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class intervento
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public intervento()
        {
            this.chirurgoes = new HashSet<chirurgo>();
            this.infermieres = new HashSet<infermiere>();
            this.tipologias = new HashSet<tipologia>();
        }
    
        public int IdIntervento { get; set; }
        public int IdReferto { get; set; }
        public System.DateTime Giorno { get; set; }
        public System.TimeSpan OraInizio { get; set; }
        public System.TimeSpan OraFine { get; set; }
        public string Descrizione { get; set; }
        public int IdPaziente { get; set; }
    
        public virtual paziente paziente { get; set; }
        public virtual referto referto { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<chirurgo> chirurgoes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<infermiere> infermieres { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tipologia> tipologias { get; set; }
    }
}
