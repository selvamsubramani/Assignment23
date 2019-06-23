namespace FSE.Assignment23.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PODETAIL")]
    public partial class PODETAIL
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(4)]
        public string PONO { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(4)]
        public string ITCODE { get; set; }

        public int? QTY { get; set; }

        public virtual ITEM ITEM { get; set; }

        public virtual POMASTER POMASTER { get; set; }
    }
}
