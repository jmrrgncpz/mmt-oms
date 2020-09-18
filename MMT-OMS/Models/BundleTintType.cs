namespace MMT_OMS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BundleTintType")]
    public partial class BundleTintType
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [StringLength(20)]
        public string BundleCode { get; set; }

        public int TintTypeId { get; set; }

        public int Quantity { get; set; }

        public virtual Bundle Bundle { get; set; }

        public virtual TintType TintType { get; set; }
    }
}
