namespace MMT_OMS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("CustomerOrderPayment")]
    public partial class CustomerOrderPayment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid CustomerOrderId { get; set; }

        public string PaymentImageFilePath { get; set; }

        public virtual CustomerOrder CustomerOrder { get; set; }
    }
}
