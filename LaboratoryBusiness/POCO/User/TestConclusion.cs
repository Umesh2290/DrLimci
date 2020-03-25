using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.POCO.User
{
    public class Cl_TestConclusion
    {
        public int TestConclusionID { get; set; }
        public Nullable<int> TestReportTypeID { get; set; }
        public Nullable<int> TestID { get; set; }
        public string SpecimenDetails { get; set; }
        public string ClinicalDetails { get; set; }
        public string Microscopy { get; set; }
        public string Macroscopy { get; set; }
        public string Conclusion { get; set; }
        public string SnomedCoding { get; set; }
        public string SampleDescription { get; set; }
        public string Report { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
    }
}
