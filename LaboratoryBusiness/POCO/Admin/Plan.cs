using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.POCO.Admin
{
   public class Plan
    {
        public int PlanID { get; set; }
        public string PlanName { get; set; }
        public string PlanDescription { get; set; }
        public string PlanDetail { get; set; }
        public Nullable<decimal> PlanCost { get; set; }
        public Nullable<int> PlanStatus { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
    }
}
