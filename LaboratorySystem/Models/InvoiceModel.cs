using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LaboratorySystem.Models
{
    public class InvoiceModel
    {
        public string InvoiceID { get; set; }
        public string HospitalName { get; set; }
        public String HospitalAddress { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime DueDate { get; set; }
        public string Amount { get; set; }
        public string Vat { get; set; }
        public string VatCost { get; set; }
        public string Total { get; set; }
        public string SubTotal { get; set; }
        public string LabName { get; set; }
        public string LabAddress { get; set; }
        public string LabCompanyNumber { get; set; }
        public string LabUniqueCode { get; set; }
        public string LabHeadOfficeAddress { get; set; }
        public string labEmail { get; set; }

        public List<InvoiceTestData> invoiceTestDatas { get; set; }
    }
}