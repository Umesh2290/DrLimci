using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LaboratorySystem.Controllers.User
{
    [RoutePrefix("User/Payments")]
    public class PaymentsController : RedirectController
    {
        // GET: Payments
        [Route("BookedTestPayments")]
        [ClientAuthorizeMember]
        public ActionResult BookedTestPayments()
        {
            return View("~/Views/User/Payments/BookedTestPayments.cshtml");
        }
        [Route("PatientRegistrationPayments")]
        [ClientAuthorizeMember]
        // [SystemAuthorizeMember]
        [HttpGet]
        public ActionResult PatientRegistrationPayments()
        {
            return View("~/Views/User/Payments/PatientRegistrationPayments.cshtml");

        }
        [HttpGet]
        [Route("GetGraphData")]
        public ActionResult GetGraphData()
        {
            try{
            LaboratoryBusiness.Repositories.User.IClientUserRepository clientuser = this.currentdomaindb.ClientUserRepository();
            LaboratoryBusiness.Repositories.User.IPatientDetailRepository patientdetail = this.currentdomaindb.PatientDetailRepository();
            var patientdetailobj = patientdetail.GetAll().ToList();

             if (patientdetailobj != null)
                    {
                       

                        return WebJSResponse.ResponseSimple(new { patientuserjson = patientdetailobj });
                    }

                    else
                    {
                        return WebJSResponse.ResponseToastr(ToastrEnum.error, "No patient found !", "There is no patient associated to this patient id.", new { });
                    }


                

                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Incorrect patient-id !", "Kindly provide correct patient id.", new { });

            }
            
            catch (Exception ex)
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
            }

        }

        [HttpGet]
        [Route("GetGraphDataForTest")]
        public ActionResult GetGraphDataForTest()
        {
            try
            {
                LaboratoryBusiness.Repositories.User.IClientUserRepository clientuser = this.currentdomaindb.ClientUserRepository();
                LaboratoryBusiness.Repositories.User.ITestRepository testDetail = this.currentdomaindb.TestRepository();

                var testDetailobj = testDetail.GetAll().ToList().GroupBy(x=>x.TestCreatedDate);

                if (testDetailobj != null)
                {


                    return WebJSResponse.ResponseSimple(new { patienttestjson = testDetailobj });
                }

                else
                {
                    return WebJSResponse.ResponseToastr(ToastrEnum.error, "No patient found !", "There is no patient associated to this patient id.", new { });
                }




                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Incorrect patient-id !", "Kindly provide correct patient id.", new { });

            }

            catch (Exception ex)
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
            }

        }
    }
}