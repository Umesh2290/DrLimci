using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.POCO.User
{
    public class Cl_TestInvestigation
    {
        public int InvestigationID { get; set; }
        public string InvestigationName { get; set; }
        public string Value { get; set; }
        public string Range { get; set; }
        public string InvestigationResult { get; set; }
        public Nullable<int> ExtraWorkID { get; set; }
        public Nullable<int> TestID { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
    }
}
