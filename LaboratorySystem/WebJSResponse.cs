using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LaboratorySystem
{
    public enum ToastrEnum
    {
        [Description("toastr-sucsess")]
        success = 1,
        [Description("toastr-error")]
        error = 2,
        [Description("toastr-warning")]
        warning = 3,
        [Description("toastr-info")]
        info = 4,
    }

    public enum SwalEnum
    {
        [Description("swal-sucsess")]
        success = 1,
        [Description("swal-error")]
        error = 2,
        [Description("swal-warning")]
        warning = 3,
        [Description("swal-info")]
        info = 4,
        [Description("swal-custom")]
        custom = 5
    }
    public static class WebJSResponse
    {
        public static JsonResult ResponseToastr(ToastrEnum en, string title, string description,object obj)
        {
            return new JsonResult() { JsonRequestBehavior = JsonRequestBehavior.AllowGet, Data = new { ResponseType = en.ToDescriptionString(), title = title, description = description, data = obj } };
        }

        public static JsonResult ResponseSWAL(SwalEnum en, string title, string descriptionorHtml, object obj)
        {
            return new JsonResult() { JsonRequestBehavior = JsonRequestBehavior.AllowGet, Data = new { ResponseType = en.ToDescriptionString(), title = title, description = descriptionorHtml, data = obj } };
        }

        public static JsonResult ResponseSimple(object obj)
        {
            return new JsonResult() { JsonRequestBehavior = JsonRequestBehavior.AllowGet, Data = new { ResponseType = "Simple", data = obj } };
        }

        public static JsonResult ResponseRedirect(string path,object obj)
        {
            return new JsonResult() { JsonRequestBehavior = JsonRequestBehavior.AllowGet, Data = new { ResponseType = "Redirect",path=path, data = obj } };
        }
    }
}