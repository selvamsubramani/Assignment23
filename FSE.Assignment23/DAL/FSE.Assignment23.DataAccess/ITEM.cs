namespace FSE.Assignment23.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ITEM")]
    public partial class ITEM
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ITEM()
        {
            PODETAILs = new HashSet<PODETAIL>();
        }

        [Key]
        [StringLength(4)]
        public string ITCODE { get; set; }

        [Required]
        [StringLength(15)]
        public string ITDESC { get; set; }

        [Column(TypeName = "money")]
        public decimal? ITRATE { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PODETAIL> PODETAILs { get; set; }
    }
}
