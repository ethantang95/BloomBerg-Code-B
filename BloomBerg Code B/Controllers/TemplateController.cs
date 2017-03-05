using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BloomBerg_Code_B.Controllers
{
    [RoutePrefix("Home/Template")]
    public class TemplateController : Controller
    {
        [HttpGet]
        [Route("LoginDialog")]
        public ActionResult LoginDialog() {
            return new FilePathResult("~/Views/Template/LoginDialog.html", "text/html");
        }
    }
}