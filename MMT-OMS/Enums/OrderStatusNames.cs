using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MMT_OMS.Enums
{
    public enum OrderStatusName
    {
        Payment = 1,
        LabelPrinting = 2,
        LabelApplication = 3,
        Packaging = 4,
        Shipping = 5,
        Done = 6
    }
}