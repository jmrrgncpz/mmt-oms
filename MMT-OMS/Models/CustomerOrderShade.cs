namespace MMT_OMS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CustomerOrderShade")]
    public partial class CustomerOrderShade
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid CustomerOrderId { get; set; }

        public Guid ShadeId { get; set; }

        public int Quantity { get; set; }

        [StringLength(30)]
        public string ShadeName { get; set; }

        public string LayoutImageFilePath { get; set; }

        public bool IsPrinted { get; set; }

        [NotMapped]
        public string FileName { get; set; }

        public virtual CustomerOrder CustomerOrder { get; set; }

        public virtual Shade Shade { get; set; }
    }
}
