namespace MMT_OMS.Models
{
    using MMT_OMS.Enums;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    [Table("CustomerOrder")]
    public partial class CustomerOrder : IValidatableObject
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CustomerOrder()
        {
            CustomerOrderPayments = new HashSet<CustomerOrderPayment>();
            CustomerOrderShades = new HashSet<CustomerOrderShade>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string ClientName { get; set; }

        [Required]
        [StringLength(100)]
        public string ClientFacebookName { get; set; }

        [StringLength(20)]
        public string BundleCode { get; set; }

        public int Amount { get; set; }

        [Required]
        [StringLength(100)]
        public string BrandName { get; set; }

        [Required]
        [StringLength(10)]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(200)]
        public string DeliveryAddress { get; set; }

        [Required]
        public string UniqueCode { get; set; }

        [Required]
        public DateTime OrderDate { get; set; } = DateTime.Today;

        [Required]
        public int OrderStatusId { get; set; } = 1;

        public virtual Bundle Bundle { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CustomerOrderPayment> CustomerOrderPayments { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CustomerOrderShade> CustomerOrderShades { get; set; }

        public virtual ICollection<Settlement> CustomerOrderSettlements { get; set; }

        public virtual CustomerOrderStatus OrderStatus { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(string.IsNullOrEmpty(PhoneNumber) || string.IsNullOrWhiteSpace(PhoneNumber) || PhoneNumber.Length != 10 || !PhoneNumberIsNumeric(PhoneNumber))
            {
                yield return new ValidationResult("Phone number is in incorrect format.", new string[] { nameof(PhoneNumber) });
            }

            if(ClientName.Length > 100)
            {
                yield return new ValidationResult("Client name number of characters is more than allowed.", new string[] { nameof(ClientName) });
            }

            if (ClientFacebookName.Length > 100)
            {
                yield return new ValidationResult("Client Facebook Name number of characters is more than allowed.", new string[] { nameof(ClientFacebookName) });
            }

            if (BrandName.Length > 100)
            {
                yield return new ValidationResult("Brand name number of characters is more than allowed.", new string[] { nameof(BrandName) });
            }

            if (DeliveryAddress.Length > 100)
            {
                yield return new ValidationResult("Delivery Address number of characters is more than allowed.", new string[] { nameof(DeliveryAddress) });
            }
        }

        private bool PhoneNumberIsNumeric(string number)
        {
            return number.All(c => c >= 48 && c <= 57);
        }
    }
}
