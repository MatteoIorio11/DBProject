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
    using System.Collections.Generic;
    
    public partial class chirurgo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public chirurgo()
        {
            this.interventoes = new HashSet<intervento>();
            this.tipologias = new HashSet<tipologia>();
        }
    
        public int IdChirurgo { get; set; }
        public string Nome { get; set; }
        public string Cognome { get; set; }
        public string CodiceFiscale { get; set; }
        public System.DateTime DataNascita { get; set; }
        public string Genere { get; set; }
        public string NumeroDiTelefono { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<intervento> interventoes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tipologia> tipologias { get; set; }
        public override string ToString()
        {
            return this.Nome + " " + this.IdChirurgo;
        }
    }
}