using System;

namespace MMT_OMS.Models.DTO
{
    public class ShadeLayout
    {
        public Guid CustomerOrderShadeId { get; set; }

        public Guid CustomerOrderId { get; set; }

        public Guid ShadeId { get; set; }

        public int Quantity { get; set; }

        public string ShadeName { get; set; }

        public string CustomShadeName { get; set; }

        public string LayoutImageFilePath { get; set; }

        public string TintType { get; set; }
        
        public int TintTypeId { get; set; }

        public bool IsPrinted { get; set; }

        public CustomerOrderGridtem CustomerOrder { get; set; }
    }
}