using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LaboratorySystem.Models
{
    public class ReportModel
    {
        public string Name { get; set; } 
        public string DOB { get; set; } 
        public string Address { get; set; } 
        public string Referring_lab_no { get; set; } 
        public string Gender { get; set; }
        public string Referring_hospital_name { get; set; }
        public string Specimen_type { get; set; }
        public string Clinical_details { get; set; }
        public string SampleDescription { get; set; }
        public string Report { get; set; }
        public int TestReportTypeID { get; set; }
        public string Macroscopy { get; set; } 
        public string Microscopy { get; set; } 
        public string Conclusion { get; set; } 

        public string reported_by { get; set; }

        public string Report_number { get; set; }

        public string NHS_report_number { get; set; }
        
       
        
        public string LabName { get; set; }
        public string LabAddress { get; set; }
        public string LabCompanyNumber { get; set; }
        public string LabUniqueCode { get; set; }
        public string LabHeadOfficeAddress { get; set; }
        public string labEmail { get; set; }

    }
}