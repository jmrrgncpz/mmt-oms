using MMT_OMS.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MMT_OMS.Models.DTO
{
    public class CustomerOrderGridtem
    {
        public Guid Id { get; set; }
        public string ClientName { get; set; }
        public string ClientFacebookName { get; set; }
        public string BundleCode { get; set; }
        public int Amount { get; set; }
        public string BrandName { get; set; }
        public string PhoneNumber { get; set; }
        public string DeliveryAddress { get; set; }
        public string UniqueCode { get; set; }
        public int OrderStatusId { get; set; }
        public OrderStatusName OrderStatus { get; set; }
        public DateTime OrderDate { get; set; }
    }
}