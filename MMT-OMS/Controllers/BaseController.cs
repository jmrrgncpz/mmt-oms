using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MMT_OMS.Controllers
{
    public class BaseController : ApiController
    {
        protected BaseController()
        {
            context = new MMTModel();
        }

        protected MMTModel context { get; set; }
    }
}
