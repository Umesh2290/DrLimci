using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LaboratorySystem.Controllers.Admin
{
    [RoutePrefix("Admin/Error")]
    public class ErrorController : RedirectController
    {
        // GET: Error
        // GET: Error
        [Route("NotFound")]
        public ActionResult Index(int ErrorCode = 0)
        {
            ViewBag.Error = NewtonJSONError.Serializer.Error(ErrorCode, HelpingClass.GetErrorConf());
            return View("~/Views/Admin/Error/NotFound.cshtml");
        }
    }
}