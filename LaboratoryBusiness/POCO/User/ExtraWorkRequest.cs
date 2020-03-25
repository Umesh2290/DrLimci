using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.POCO.User
{
    public class Cl_ExtraWorkRequest
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
