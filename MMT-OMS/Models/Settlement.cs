namespace MMT_OMS.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Settlement")]
    public partial class Settlement
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Required]
        public int Amount { get; set; }

        [Required]
        public DateTime SettlementDate { get; set; }

        [ForeignKey("CustomerOrder")]
        public Guid CustomerOrderId { get; set; }

        public virtual CustomerOrder CustomerOrder { get; set; }
    }
}