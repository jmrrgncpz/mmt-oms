using MMT_OMS.Models.DTO;
using System.Linq;
using System.Web.Http;

namespace MMT_OMS.Controllers
{
    [Authorize]
    [RoutePrefix("api/shade-layouts")]
    public class ShadeLayoutsController : BaseController
    {
        [Route("")]
        [HttpGet]
        public IHttpActionResult GetCustomerOrderShadeLayouts()
        {
            var shades = context.CustomerOrders
                                    .Where(co => co.CustomerOrderSettlements.Count > 0 && co.CustomerOrderSettlements.Sum(s => s.Amount) >= co.Amount / 2)
                                    .SelectMany(co => co.CustomerOrderShades)
                                    .Select(cop => new ShadeLayout
                                    {
                                        CustomerOrderId = cop.CustomerOrderId,
                                        CustomerOrderShadeId = cop.Id,
                                        LayoutImageFilePath = cop.LayoutImageFilePath,
                                        Quantity = cop.Quantity,
                                        ShadeId = cop.ShadeId,
                                        ShadeName = cop.Shade.Name,
                                        CustomShadeName = cop.ShadeName,
                                        TintType = cop.Shade.TintType.Name,
                                        TintTypeId = cop.Shade.TintTypeId,
                                        CustomerOrder = new CustomerOrderGridtem
                                        {
                                            BrandName = cop.CustomerOrder.BrandName,
                                            ClientFacebookName = cop.CustomerOrder.ClientFacebookName,
                                            UniqueCode = cop.CustomerOrder.UniqueCode
                                        }
                                    })
                                    .ToList();
            return Ok(shades);
        }
    }
}
