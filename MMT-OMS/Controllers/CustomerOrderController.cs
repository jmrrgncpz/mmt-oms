using MMT_OMS.Enums;
using MMT_OMS.Models;
using MMT_OMS.Models.DTO;
using MMT_OMS.Services;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace MMT_OMS.Controllers
{
    [RoutePrefix("api/customer-order")]
    [Authorize]
    public class CustomerOrderController : BaseController
    {
        private CustomerOrderService service = new CustomerOrderService();

        [Route("")]
        [HttpGet]
        public IHttpActionResult GetCustomerOrders()
        {
            var customerOrders = from co in context.CustomerOrders
                                 select new CustomerOrderGridtem
                                 {
                                     Amount = co.Amount,
                                     BrandName = co.BrandName,
                                     BundleCode = co.BundleCode,
                                     ClientFacebookName = co.ClientFacebookName,
                                     ClientName = co.ClientName,
                                     DeliveryAddress = co.DeliveryAddress,
                                     Id = co.Id,
                                     PhoneNumber = co.PhoneNumber,
                                     UniqueCode = co.UniqueCode,
                                     OrderStatusId = co.OrderStatusId,
                                     OrderStatus = (OrderStatusName)co.OrderStatusId,
                                     OrderDate = co.OrderDate
                                 };
            return Ok(customerOrders);
        }

        [Route("{id}")]
        [HttpGet]
        public IHttpActionResult GetCustomerOrderById([FromUri]Guid id)
        {
            var co = context.CustomerOrders.SingleOrDefault(x => x.Id == id);
            if (co == null)
            {
                return BadRequest("Customer Order doesn't exists");
            }

            var result = new CustomerOrderGridtem
            {
                Amount = co.Amount,
                BrandName = co.BrandName,
                BundleCode = co.BundleCode,
                DeliveryAddress = co.DeliveryAddress,
                ClientFacebookName = co.ClientFacebookName,
                ClientName = co.ClientName,
                Id = co.Id,
                PhoneNumber = co.PhoneNumber,
                UniqueCode = co.UniqueCode,
                OrderStatusId = co.OrderStatusId
            };

            return Ok(result);
        }

        [Route("{customerOrderId}/payments")]
        [HttpGet]
        public IHttpActionResult GetCustomerOrderPayments([FromUri]Guid customerOrderId)
        {
            if (!CustomerOrderExists(customerOrderId))
            {
                return BadRequest("Customer order doesn't exists");
            }

            var payments = context.CustomerOrderPayments
                                  .Where(cop => cop.CustomerOrderId == customerOrderId)
                                  .Select(cop => new Payment
                                  {
                                      Id = cop.Id,
                                      CustomerOrderId = cop.CustomerOrderId,
                                      PaymentImageFilePath = cop.PaymentImageFilePath
                                  });

            return Ok(payments.ToList());
        }

        [Route("{customerOrderId}/shades")]
        [HttpGet]
        public IHttpActionResult GetCustomerOrderShades([FromUri]Guid customerOrderId)
        {
            if (!CustomerOrderExists(customerOrderId))
            {
                return BadRequest("Customer order doesn't exists");
            }

            var shades = context.CustomerOrderShades
                                .Where(cop => cop.CustomerOrderId == customerOrderId)
                                .Select(cop => new Shade
                                {
                                    CustomerOrderId = cop.CustomerOrderId,
                                    Id = cop.Id,
                                    LayoutImageFilePath = cop.LayoutImageFilePath,
                                    Quantity = cop.Quantity,
                                    ShadeId = cop.ShadeId,
                                    ShadeName = cop.Shade.Name,
                                    CustomShadeName = cop.ShadeName,
                                    TintType = cop.Shade.TintType.Name
                                });
            return Ok(shades.ToList());
        }

        [HttpPost]
        [Route("set-status")]
        public IHttpActionResult SetOrderStatus(SetOrderStatusParameters parameters)
        {
            if (!CustomerOrderExists(parameters.CustomerOrderId))
            {
                return BadRequest("Customer Order doesn't exists.");
            }

            var customerOrder = context.CustomerOrders.SingleOrDefault(co => co.Id == parameters.CustomerOrderId);
            customerOrder.OrderStatusId = parameters.OrderStatusId;

            context.SaveChanges();
            return Ok();
        }

        [HttpPost]
        [Route("submit")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> SubmitCustomerOrder()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var request = HttpContext.Current.Request;
            var postedFiles = request.Files;

            if (!Request.Content.IsMimeMultipartContent())
            {
                Request.CreateResponse(HttpStatusCode.UnsupportedMediaType);
            }

            if (postedFiles.Count == 0)
            {
                return BadRequest("No import file was attached");
            }

            var model = request.Form["customerOrder"];
            if (model == null)
            {
                return BadRequest("Model is missing");
            }

            var customerOrder = JsonConvert.DeserializeObject<CustomerOrder>(model);
            var uniqueCode = service.SubmitOrder(postedFiles, context, customerOrder);

            return Ok(uniqueCode);
        }

        [HttpPost]
        [Route("submit-payment")]
        [AllowAnonymous]
        public IHttpActionResult SubmitCustomerOrderPayment()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var httpRequest = HttpContext.Current.Request;
            var postedFiles = httpRequest.Files;

            if (!Request.Content.IsMimeMultipartContent())
            {
                Request.CreateResponse(HttpStatusCode.UnsupportedMediaType);
            }

            if (postedFiles.Count == 0)
            {
                return BadRequest("No import file was attached");
            }


            var customerOrderUniqueCode = httpRequest.Form["uniqueCode"];
            if (string.IsNullOrEmpty(customerOrderUniqueCode) || string.IsNullOrWhiteSpace(customerOrderUniqueCode))
            {
                return BadRequest("Order code can't be empty");
            }

            var customerOrder = context.CustomerOrders.SingleOrDefault(co => co.UniqueCode == customerOrderUniqueCode);
            if (customerOrder == null)
            {
                return BadRequest("Order with provided code doesn't exist.");
            }

            var customerOrderService = new CustomerOrderService();
            customerOrderService.SubmitSettlement(context, httpRequest, customerOrder);
            return Ok();
        }

        private bool CustomerOrderExists(Guid customerOrderId)
        {
            return context.CustomerOrders.Any(co => co.Id == customerOrderId);
        }

        [HttpGet]
        [Route("get-order-form-resources")]
        [AllowAnonymous]
        public IHttpActionResult GetOrderFormResources()
        {
            var shades = from shade in context.Shades
                          select new
                          {
                              Id = shade.Id,
                              Name = shade.Name,
                              TintTypeId = shade.TintTypeId
                          };
            var gelTints = shades.Where(s => s.TintTypeId == 1).ToList();
            var matteTints = shades.Where(s => s.TintTypeId == 2).ToList();

            var bundles = (from bundle in context.Bundles
                          select new
                          {
                              BundleTintTypes = (from bundleTintType in bundle.BundleTintTypes
                                                 select new {
                                                     Quantity = bundleTintType.Quantity,
                                                     TintTypeId = bundleTintType.TintTypeId
                                                 }),
                              Code = bundle.Code,
                              Description = bundle.Description,
                              Amount = bundle.Amount
                          }).ToList();

            var tintTypes = (from tintType in context.TintTypes
                            select new Models.DTO.TintType
                            {
                                Id = tintType.Id,
                                Name = tintType.Name,
                                Price = tintType.Price
                            }).ToList();

            return Ok(new {
                GelTints = gelTints,
                MatteTints = matteTints,
                Bundles = bundles,
                TintTypes = tintTypes
            });
        }

        [HttpPost]
        [Route("create-settlement")]
        public IHttpActionResult CreateNewSettlement(Settlement settlement)
        {
            var customerOrder = context.CustomerOrders.SingleOrDefault(co => co.Id == settlement.CustomerOrderId);
            if(customerOrder == null)
            {
                return BadRequest("Customer order doesn't exists.");
            }

            if(settlement.Amount == 0)
            {
                return BadRequest($"{settlement.Amount} is an invalid settlement amount.");
            }
            var savedSettlement = context.Settlements.Add(new Settlement
            {
                Amount = settlement.Amount,
                SettlementDate = settlement.SettlementDate,
                CustomerOrderId = settlement.CustomerOrderId
            });

            context.SaveChanges();

            var settlements = context.Settlements.Where(s => s.CustomerOrderId == customerOrder.Id);
            if (settlements.Any())
            {
                var settlementSum = settlements.Sum(x => x.Amount);
                if (settlementSum >= customerOrder.Amount)
                {
                    customerOrder.OrderStatusId = (int)OrderStatusName.LabelPrinting;
                    context.SaveChanges();
                }
            }

            return Ok(savedSettlement);
        }

        [HttpGet]
        [Route("{customerOrderId}/settlements")]
        public IHttpActionResult GetSettlements(Guid customerOrderId)
        {
            if (!CustomerOrderExists(customerOrderId))
            {
                return BadRequest("Customer order doesn't exists.");
            }

            var settlements = context.Settlements
                                     .Where(s => s.CustomerOrderId == customerOrderId)
                                     .Select(s => new SettlementItem
                                     {
                                         Amount = s.Amount,
                                         Id = s.Id,
                                         SettlementDate = s.SettlementDate
                                     })
                                     .ToList()
                                     .OrderByDescending(x => x.SettlementDate)
                                     .ThenByDescending(x => x.Id);
            return Ok(settlements);
        }

        private class Payment
        {
            public Guid Id { get; set; }

            public Guid CustomerOrderId { get; set; }

            public string PaymentImageFilePath { get; set; }
        }

        private class SettlementItem
        {
            public int Id { get; set; }
            public int Amount { get; set; }
            public DateTime SettlementDate { get; set; }
        }

        private class Shade
        {
            public Guid Id { get; set; }

            public Guid CustomerOrderId { get; set; }

            public Guid ShadeId { get; set; }

            public int Quantity { get; set; }

            public string ShadeName { get; set; }

            public string CustomShadeName { get; set; }

            public string LayoutImageFilePath { get; set; }

            public string TintType { get; set; }
        }

        public class SetOrderStatusParameters
        {
            public Guid CustomerOrderId { get; set; }
            public int OrderStatusId { get; set; }
        }
    }
}
