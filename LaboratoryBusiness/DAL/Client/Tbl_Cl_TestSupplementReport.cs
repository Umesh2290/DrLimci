//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LaboratoryBusiness.DAL.Client
{
    using System;
    using System.Collections.Generic;
    
    public partial class Tbl_Cl_TestSupplementReport
    {
        public int TestSupplementReportID { get; set; }
        public Nullable<int> TestReportTypeID { get; set; }
        public Nullable<int> TestID { get; set; }
        public string SpecimenDetails { get; set; }
        public string ClinicalDetails { get; set; }
        public string Microscopy { get; set; }
        public string Macroscopy { get; set; }
        public string SupplementReportConclusion { get; set; }
        public string SnomedCoding { get; set; }
        public string SampleDescription { get; set; }
        public string Report { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<bool> IsPublished { get; set; }
    }
}
