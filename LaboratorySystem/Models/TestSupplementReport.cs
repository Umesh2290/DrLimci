using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LaboratorySystem
{
    public class TestSupplementReport
    {
        public int TestSupplementReportID { get; set; }
        public Nullable<int> TestReportTypeID { get; set; }
        public string TestReportType { get; set; }
        public Nullable<int> TestID { get; set; }
        public string SpecimenDetails { get; set; }
        public string ClinicalDetails { get; set; }
        public string Microscopy { get; set; }
        public string Macroscopy { get; set; }
        public string SupplementReportConclusion { get; set; }
        public string SnomedCoding { get; set; }
        public string SampleDescription { get; set; }
        public string Report { get; set; }
        public string SupplementBy { get; set; }
        public string SupplementDate { get; set; }
        public List<LaboratoryBusiness.POCO.User.Cl_TestAttachment> AttachmentList { get; set; }
        public bool IsPublished { get; set; }
    }
}