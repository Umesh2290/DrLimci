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
    
    public partial class Tbl_Cl_ForgetPassword
    {
        public int ForgetPasswordID { get; set; }
        public string UniqueCode { get; set; }
        public Nullable<int> ClientUserID { get; set; }
        public Nullable<int> ExpiresInMinutes { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    }
}
