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
    
    public partial class Tbl_Cl_ExtraWorkRequest
    {
        public int ExtraWorkID { get; set; }
        public int TestID { get; set; }
        public string H_ELevels { get; set; }
        public string SpecialStains { get; set; }
        public string ImmunoHistoChemistry { get; set; }
        public string Others { get; set; }
        public Nullable<int> StatusID { get; set; }
        public Nullable<System.DateTime> RequestCreatedDate { get; set; }
        public Nullable<int> RequestCreatedBy { get; set; }
        public Nullable<System.DateTime> NewActionDate { get; set; }
        public Nullable<int> NewActionBy { get; set; }
        public Nullable<int> NewActionStatusID { get; set; }
        public string NewActionComments { get; set; }
        public Nullable<int> PendingActionBy { get; set; }
        public Nullable<System.DateTime> PendingActionDate { get; set; }
        public string PendingActionComments { get; set; }
    }
}
