using System.ComponentModel.DataAnnotations;

namespace MMT_OMS.Models
{


    public partial class CustomerOrderStatus
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}