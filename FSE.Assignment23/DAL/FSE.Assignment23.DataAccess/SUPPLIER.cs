namespace FSE.Assignment23.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SUPPLIER")]
    public partial class SUPPLIER
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SUPPLIER()
        {
            POMASTERs = new HashSet<POMASTER>();
        }

        [Key]
        [StringLength(4)]
        public string SUPLNO { get; set; }

        [Required]
        [StringLength(15)]
        public string SUPLNAME { get; set; }

        [StringLength(40)]
        public string SUPLADDR { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<POMASTER> POMASTERs { get; set; }
    }
}
