//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace veri_tabanı_entity_framwork_çalışmaları
{
    using System;
    using System.Collections.Generic;
    
    public partial class TBLDERS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TBLDERS()
        {
            this.TBLNOTLAR = new HashSet<TBLNOTLAR>();
        }
    
        public int DERSID { get; set; }
        public string DERSAD { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TBLNOTLAR> TBLNOTLAR { get; set; }
    }
}
