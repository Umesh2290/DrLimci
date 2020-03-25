using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.POCO.Admin
{
  public  class SalesAndContactQuery
    {
        public int SalesAndContactQueryID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string ContactNo { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public Nullable<int> TypeID { get; set; }
        public Nullable<int> StatusID { get; set; }
        public Nullable<System.DateTime> RequestCreatedDate { get; set; }
        public Nullable<int> NewActionBy { get; set; }
        public Nullable<System.DateTime> NewActionDate { get; set; }
        public Nullable<int> NewActionStatusID { get; set; }
        public string NewActionComments { get; set; }
        public Nullable<int> PendingActionBy { get; set; }
        public Nullable<System.DateTime> PendingActionDate { get; set; }
        public string PendingActionComments { get; set; }
    }
}
