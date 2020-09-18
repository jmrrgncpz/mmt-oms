using MMT_OMS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MMT_OMS.Services
{
    public class CustomerOrderService
    {
        public string SubmitOrder(HttpFileCollection files, MMTModel context, CustomerOrder customerOrder)
        {
            customerOrder.Id = Guid.NewGuid();
            var customerOrderShades = new List<CustomerOrderShade>();
            foreach(var cos in customerOrder.CustomerOrderShades)
            {
                customerOrderShades.Add(cos);
            }

            customerOrder.CustomerOrderShades.Clear();
            context.CustomerOrders.Add(customerOrder);

            var savedCustomerOrder = context.CustomerOrders.Find(customerOrder.Id);
            var layoutImagesFolder = "LayoutImages";
            var root = HttpContext.Current.Server.MapPath($"~/{layoutImagesFolder}/");
            Directory.CreateDirectory(root);
            foreach (var shade in customerOrderShades)
            {
                var postedFile = files[shade.FileName];
                var filePath = $"{layoutImagesFolder}/{postedFile.FileName}";
                var serverFilePath = HttpContext.Current.Server.MapPath($"~/{filePath}");
                postedFile.SaveAs(serverFilePath);

                shade.LayoutImageFilePath = filePath;
                shade.Id = Guid.NewGuid();
                shade.CustomerOrderId = customerOrder.Id;
                savedCustomerOrder.CustomerOrderShades.Add(shade);
            }
            try
            {
                context.SaveChanges();
                return savedCustomerOrder.UniqueCode;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void SubmitSettlement(MMTModel context, HttpRequest request, CustomerOrder customerOrder)
        {
            var orderPaymentImageFolder = "OrderPayments";
            var root = HttpContext.Current.Server.MapPath($"~/{orderPaymentImageFolder}/");
            Directory.CreateDirectory(root);
            for(int i = 0; i < request.Files.Count; i++)
            {
                var file = request.Files[i];
                var filePath = $"{orderPaymentImageFolder}/{file.FileName}";
                var serverFilePath = HttpContext.Current.Server.MapPath($"~/{filePath}");
                file.SaveAs(serverFilePath);

                var newSettlement = new CustomerOrderPayment
                {
                    PaymentImageFilePath = filePath
                };

                customerOrder.CustomerOrderPayments.Add(newSettlement);
            }

            context.SaveChanges();
        }
    }
}