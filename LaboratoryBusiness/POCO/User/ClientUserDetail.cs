using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.POCO.User
{
    public class Cl_ClientUserDetail
    {
        public int UserDetailID { get; set; }
        public Nullable<System.DateTime> JoiningDate { get; set; }
        public Nullable<System.DateTime> TerminationDate { get; set; }
        public string TerminationReason { get; set; }
        public string Qualifications { get; set; }
        public string TypeOfCollection { get; set; }
        public string License { get; set; }
        public Nullable<bool> IsFulltime { get; set; }
        public string PdfLink { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
    }
}
