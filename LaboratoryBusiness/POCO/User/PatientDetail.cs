using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.POCO.User
{
    public class Cl_PatientDetail
    {
        public int PatientDetailID { get; set; }
        public string MiddleName { get; set; }
        public string Age { get; set; }
        public string Sex { get; set; }
        public string Streetname { get; set; }
        public string City { get; set; }
        public string AlternateAddress { get; set; }
        public string AlternatePhoneNo { get; set; }
        public string ReferingDoctor { get; set; }
        public string ReferingHospital { get; set; }
        public string PaymentMode { get; set; }
        public Nullable<decimal> Payment { get; set; }
        public string PdfLink { get; set; }
        public string PaymentReceiptPdfLink { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<int> HospitalID { get; set; }
    }
}
