using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.POCO.User
{
   public class Cl_LabReportConfiguration
    {
        public int ConfigID { get; set; }
        public Nullable<int> LabID { get; set; }
        public string LabImage { get; set; }
        public string LabName { get; set; }
        public string LabAddress { get; set; }
        public string LabCompanyNumber { get; set; }
        public string LabUniqueCode { get; set; }
        public string LabHeadOfficeAddress { get; set; }
        public string labEmail { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public string RegistrationNumber { get; set; }
        public string VatRate { get; set; }
    }
}
