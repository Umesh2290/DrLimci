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
    
    public partial class Tbl_Cl_Test
    {
        public int TestID { get; set; }
        public string TestName { get; set; }
        public Nullable<bool> IsSampleRequired { get; set; }
        public string ComplaintHistory { get; set; }
        public string Description { get; set; }
        public Nullable<int> PatientUserID { get; set; }
        public Nullable<int> TestStatusID { get; set; }
        public Nullable<bool> IsSampleCollected { get; set; }
        public string SampleLabel { get; set; }
        public string SampleCode { get; set; }
        public string SampleType { get; set; }
        public Nullable<bool> IsPublish { get; set; }
        public string PdfLink { get; set; }
        public Nullable<System.DateTime> TestCreatedDate { get; set; }
        public Nullable<int> TestCreatedBy { get; set; }
        public Nullable<System.DateTime> SampleCollectionDate { get; set; }
        public Nullable<int> SampleCollectionBy { get; set; }
        public Nullable<System.DateTime> AnalysisDate { get; set; }
        public Nullable<int> AnalysisBy { get; set; }
        public Nullable<System.DateTime> ConclusionDate { get; set; }
        public Nullable<int> ConclusionBy { get; set; }
        public string Cost { get; set; }
        public Nullable<bool> IsInvoiceGenerated { get; set; }
        public Nullable<System.DateTime> AurthorizeDate { get; set; }
    }
}
