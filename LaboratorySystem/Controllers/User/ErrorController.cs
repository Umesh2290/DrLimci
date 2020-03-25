using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Configuration;

namespace LaboratorySystem.Controllers.User
{
    [RoutePrefix("User/Error")]
    public class ErrorController : Controller
    {
        // GET: Error
        [Route("NotFound")]

        public ActionResult NotFound(int ErrorCode = 0)
        {
            ViewBag.Error = NewtonJSONError.Serializer.Error(ErrorCode, HelpingClass.GetErrorConf());
            return View("~/Views/User/Error/NotFound.cshtml");
        }

        [Route("SubDomainNotFound")]

        public ActionResult SubDomainNotFound(int ErrorCode = 0)
        {
            ViewBag.Error = NewtonJSONError.Serializer.Error(ErrorCode, HelpingClass.GetErrorConf());
            return View("~/Views/User/Error/SubDomainNotFound.cshtml");
        }

        [Route("InactiveAccount")]

        public ActionResult InactiveAccount(int ErrorCode = 0)
        {
            ViewBag.Error = NewtonJSONError.Serializer.Error(ErrorCode, HelpingClass.GetErrorConf());
            return View("~/Views/User/Error/InactiveAccount.cshtml");
        }

        [Route("Exception")]

        public ActionResult Exception(int ErrorCode = 0)
        {
            ViewBag.Error = NewtonJSONError.Serializer.Error(ErrorCode, HelpingClass.GetErrorConf());
            return View("~/Views/User/Error/Exception.cshtml");
        }
    }
}