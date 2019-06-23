namespace FSE.Assignment23.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("POMASTER")]
    public partial class POMASTER
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public POMASTER()
        {
            PODETAILs = new HashSet<PODETAIL>();
        }

        [Key]
        [StringLength(4)]
        public string PONO { get; set; }

        public DateTime? PODATE { get; set; }

        [StringLength(4)]
        public string SUPLNO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PODETAIL> PODETAILs { get; set; }

        public virtual SUPPLIER SUPPLIER { get; set; }
    }
}
