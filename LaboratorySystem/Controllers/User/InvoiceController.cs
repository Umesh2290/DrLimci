using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Repositories = LaboratoryBusiness.Repositories;
using BLL = LaboratoryBusiness.BLL;
using BusinessPOCO = LaboratoryBusiness.POCO;
using System.IO;
using LaboratorySystem.Models;
using System.Dynamic;
using System;

namespace LaboratorySystem.Controllers.User
{
    [RoutePrefix("User/Invoice")]
    public class InvoiceController : RedirectController
    {
        [Route]
        [ClientAuthorizeMember]
        // GET: Invoice
        public ActionResult Index()
        {
            ViewBag.currentdomaindb = this.currentdomaindb;

            return View("~/Views/User/Invoice/Index.cshtml");
        }
        [Route("Getlist")]
        [HttpPost]
        public JsonResult Getlist(DateTime fromdate,DateTime todate,string hospital)
        {

            try
            {
                Repositories.User.IClientUserRepository clientuser = this.currentdomaindb.ClientUserRepository();
                Repositories.User.IPatientDetailRepository patientdetailrepo = this.currentdomaindb.PatientDetailRepository();
                Repositories.User.ITestRepository testrep = this.currentdomaindb.TestRepository();
                Repositories.User.ITestStatusRepositories teststatusrep = this.currentdomaindb.TestStatusRepositories();
                object result = null;
                if ((fromdate != null && todate != null) && hospital != null)
                {
                    result = (from ts in testrep.GetAll()
                              join pt in patientdetailrepo.GetAll() on ts.PatientUserID equals pt.PatientDetailID
                              join ptus in clientuser.GetAll() on ts.PatientUserID equals ptus.ClientUserID
                              where ts.TestStatusID == 5 && ts.IsPublish == true && ts.IsInvoiceGenerated == false
                            && pt.HospitalID == Convert.ToInt32(hospital)
                            && (ts.AurthorizeDate >= fromdate.Date && ts.AurthorizeDate <= todate.Date)
                              select new
                              {
                                  ts.TestID,
                                  ts.TestName,
                                  PatientName = (ptus.FirstName + " " + (pt.MiddleName == null ? "" : pt.MiddleName) + " " + ptus.LastName),
                                  Status = "Completed",
                                  ts.Cost,
                                  ts.SampleCode
                              }).ToList();
                    //return WebJSResponse.ResponseSimple(new { testinvoicejson = result });
                  
                }
                else if ((fromdate == null && todate == null) && hospital != null)
                {
                     result = (from ts in testrep.GetAll()
                              join pt in patientdetailrepo.GetAll() on ts.PatientUserID equals pt.PatientDetailID
                              join ptus in clientuser.GetAll() on ts.PatientUserID equals ptus.ClientUserID
                              where ts.TestStatusID == 5 && ts.IsPublish == true && ts.IsInvoiceGenerated == false
                            && pt.HospitalID == Convert.ToInt32(hospital)

                              select new
                              {
                                  ts.TestID,
                                  ts.TestName,
                                  PatientName = (ptus.FirstName + " " + (pt.MiddleName == null ? "" : pt.MiddleName) + " " + ptus.LastName),
                                  Status = "Completed",
                                  ts.Cost,
                                  ts.SampleCode
                              }).ToList();

                   

                }

                
                return WebJSResponse.ResponseSimple(new { testinvoicejson = result });
            }

            catch (Exception ex)
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
            }
        }
        [Route("GenerateInvoice")]
        [HttpPost]
        public JsonResult GenerateInvoice(DateTime fromdate, DateTime todate, string hospital)
        {

            try
            {
                Repositories.User.IClientUserRepository clientuser = this.currentdomaindb.ClientUserRepository();
                Repositories.User.IPatientDetailRepository patientdetailrepo = this.currentdomaindb.PatientDetailRepository();
                Repositories.User.ITestRepository testrep = this.currentdomaindb.TestRepository();
                Repositories.User.ITestStatusRepositories teststatusrep = this.currentdomaindb.TestStatusRepositories();
                Repositories.User.IInvoiceRepository invoiceRepository = this.currentdomaindb.Invoice();
                Repositories.User.IHospitalDetail hospitalDetail = this.currentdomaindb.HospitalDetailRepository();
                int total = 0, Subtotal, vatcost, vatrate = 20;
                var invoiceModel = new InvoiceModel();
                List<InvoiceTestData> invoiceTestDatas = new List<InvoiceTestData>();
                //var invoiceTestDatas = new InvoiceTestData();
                //object result = null;
                if ((fromdate != null && todate != null) && hospital != null)
                {
                    BusinessPOCO.User.Cl_HospitalDetail hospitaldata = hospitalDetail.GetByID (Convert.ToInt32(hospital));
                    var result = (from ts in testrep.GetAll()
                              join pt in patientdetailrepo.GetAll() on ts.PatientUserID equals pt.PatientDetailID
                              join ptus in clientuser.GetAll() on ts.PatientUserID equals ptus.ClientUserID
                              where ts.TestStatusID == 5 && ts.IsPublish == true && ts.IsInvoiceGenerated ==false
                              && pt.HospitalID == Convert.ToInt32(hospital)
                            && (ts.AurthorizeDate >= fromdate.Date && ts.AurthorizeDate <= todate.Date)
                              select new
                              {
                                  ts.TestID,
                                  ts.TestName,
                                  PatientName = (ptus.FirstName + " " + (pt.MiddleName == null ? "" : pt.MiddleName) + " " + ptus.LastName),
                                  Status = "Completed",
                                  ts.Cost,
                                  ts.SampleCode
                              }).ToList();
                    if (result.Count > 0)
                    {
                       
                        foreach (var i in result)
                        {
                            InvoiceTestData invdata = new InvoiceTestData();
                            BusinessPOCO.User.Cl_Test test = testrep.GetByID(i.TestID);
                            total += Convert.ToInt32(i.Cost);
                            
                            invdata.SampleCode = i.SampleCode;
                            invdata.PaitientName = i.PatientName;
                            invdata.TestName = i.TestName;
                            invdata.Cost = i.Cost;
                            test.IsInvoiceGenerated = true;
                            testrep.Update(test);
                            testrep.Save();
                            // invdata.SampleCode
                            invoiceTestDatas.Add(invdata);

                        }

                        invoiceModel.invoiceTestDatas = invoiceTestDatas;

                        vatcost = (total * vatrate) / 100;
                        Subtotal = total + vatcost;


                        BusinessPOCO.User.Invoice invoiceobj = new BusinessPOCO.User.Invoice();
                        invoiceobj.HospitalId = Convert.ToInt32(hospital);
                        invoiceobj.InvoiceDate = DateTime.Now.Date;
                        invoiceobj.DueDate = DateTime.Now.Date.AddDays(21);
                        invoiceobj.Amount = Subtotal;
                        invoiceRepository.Insert(invoiceobj);

                        invoiceRepository.Save();
                        invoiceModel.InvoiceID = invoiceobj.InvoiceID.ToString();
                        invoiceModel.HospitalName = hospitaldata.HospitalName;
                            
                        invoiceModel.HospitalAddress = hospitaldata.Address;
                        invoiceModel.InvoiceDate = Convert.ToDateTime(invoiceobj.InvoiceDate);
                        invoiceModel.DueDate = Convert.ToDateTime(invoiceobj.DueDate);
                        invoiceModel.Amount = invoiceobj.Amount.ToString();
                        invoiceModel.Vat = vatrate.ToString();
                        invoiceModel.VatCost = vatcost.ToString();
                        invoiceModel.Total = total.ToString();
                        invoiceModel.SubTotal = Subtotal.ToString();

                        string invoicepdflink = string.Empty;
                        FileManager fmp = PdfCreator.CreateInvoicePDF(invoiceModel, this.subdomainurl, this.currentdomaindb, out invoicepdflink);
                        //BusinessPOCO.User.Cl_PatientDetail patientdetailobjupdate = patientdetail.GetByID(patientdetailobj.PatientDetailID);
                        //patientdetailobjupdate.PaymentReceiptPdfLink = HelpingClass.LocalUploadPathToRelativeWebPath(paymentpdflink);
                        //patientdetailobjupdate.PdfLink = HelpingClass.LocalUploadPathToRelativeWebPath(pdflink);
                        invoiceobj.PDFLink= HelpingClass.LocalUploadPathToRelativeWebPath(invoicepdflink);
                        invoiceRepository.Update(invoiceobj);
                        invoiceRepository.Save();

                                            }

                    result = (from ts in testrep.GetAll()
                              join pt in patientdetailrepo.GetAll() on ts.PatientUserID equals pt.PatientDetailID
                              join ptus in clientuser.GetAll() on ts.PatientUserID equals ptus.ClientUserID
                              where ts.TestStatusID == 5 && ts.IsPublish == true && ts.IsInvoiceGenerated == false
                              && pt.HospitalID == Convert.ToInt32(hospital)
                            && (ts.AurthorizeDate >= fromdate.Date && ts.AurthorizeDate <= todate.Date)
                              select new
                              {
                                  ts.TestID,
                                  ts.TestName,
                                  PatientName = (ptus.FirstName + " " + (pt.MiddleName == null ? "" : pt.MiddleName) + " " + ptus.LastName),
                                  Status = "Completed",
                                  ts.Cost,
                                  ts.SampleCode
                              }).ToList();
                    return WebJSResponse.ResponseSWAL(SwalEnum.success, "Created Successfully !", "Invoice has been created successfully<br>", new { testinvoicejson = result });

                    //return WebJSResponse.ResponseSimple(new { testinvoicejson = result });

                }
                else if ((fromdate == null && todate == null) && hospital != null)
                {
                   var result = (from ts in testrep.GetAll()
                              join pt in patientdetailrepo.GetAll() on ts.PatientUserID equals pt.PatientDetailID
                              join ptus in clientuser.GetAll() on ts.PatientUserID equals ptus.ClientUserID
                              where ts.TestStatusID == 5 && ts.IsPublish == true && ts.IsInvoiceGenerated == false
                            && pt.HospitalID == Convert.ToInt32(hospital)

                              select new
                              {
                                  ts.TestID,
                                  ts.TestName,
                                  PatientName = (ptus.FirstName + " " + (pt.MiddleName == null ? "" : pt.MiddleName) + " " + ptus.LastName),
                                  Status = "Completed",
                                  ts.Cost,
                                  ts.SampleCode
                              }).ToList();

                    return WebJSResponse.ResponseSWAL(SwalEnum.success, "Created Successfully !", "Invoice has been created successfully<br>", new { testinvoicejson = "" });

                }

              //  return WebJSResponse.ResponseSWAL(SwalEnum.success, "Created Successfully !", "Invoice has been created successfully<br>");
                return WebJSResponse.ResponseSimple(new { testinvoicejson = "" });
            }

            catch (Exception ex)
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
            }
        }

        [ClientAuthorizeMember]
        [Route("AllGeneratedInvoice")]
        // GET: ExportData
        [HttpGet]
        public ActionResult AllGeneratedInvoice()
        {
            return View("~/Views/User/Invoice/AllGeneratedInvoice.cshtml");
        }
        [Route("AllGeneratedInvoice")]
        [HttpPost]
        public JsonResult GetAllGeneratedInvoice()
        {
            try
            {
                Repositories.User.IInvoiceRepository invoiceRepository = this.currentdomaindb.Invoice();
                Repositories.User.IHospitalDetail hospitalDetail = this.currentdomaindb.HospitalDetailRepository();

                

                //int clientuserdetailtypeid = clientusertype.GetAll().Where(x => x.TypeName.Equals("Patient")).FirstOrDefault().ClientUserTypeID;

                object invoiceobj = null;

                invoiceobj = (from il in invoiceRepository.GetAll()
                                    join hd in hospitalDetail.GetAll() on il.HospitalId equals hd.HospitalDetailID
                                    //where cl.DetailType.Value == clientuserdetailtypeid && pd.PaymentReceiptPdfLink != null && pd.PdfLink != null
                                    select new
                                    {
                                        il.InvoiceID,
                                        hd.HospitalName,
                                        il.Amount,
                                        CreatedDate = il.InvoiceDate.Value.ToString("dd/MM/yyyy"),
                                        DueDate=il.DueDate.Value.ToString("dd/MM/yyyy"),
                                        il.PDFLink
                                        
                                    }).ToList();

                if (invoiceobj != null)
                {
                    return WebJSResponse.ResponseSimple(new { invoicejson = invoiceobj });
                }

                else
                {
                    return WebJSResponse.ResponseToastr(ToastrEnum.error, "No record found !", "There is no invoice generated.", new { });
                }

            }
            catch (Exception ex)
            {
                return WebJSResponse.ResponseToastr(ToastrEnum.error, "Its not your fault", "Something went wrong from our side!please try later<br>" + ex.Message, new { });
            }
        }
    }
}