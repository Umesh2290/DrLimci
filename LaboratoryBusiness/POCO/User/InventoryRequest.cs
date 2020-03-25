using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.POCO.User
{
    public class Cl_InventoryRequest
    {
        public int InventoryRequestID { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
        public string Comments { get; set; }
        public string Quantity { get; set; }
        public Nullable<int> StatusID { get; set; }
        public Nullable<System.DateTime> RequestCreatedDate { get; set; }
        public Nullable<int> RequestCreatedBy { get; set; }
        public Nullable<int> OpenActionBy { get; set; }
        public Nullable<System.DateTime> OpenActionDate { get; set; }
        public Nullable<int> OpenActionStatusID { get; set; }
        public string OpenActionComments { get; set; }
        public Nullable<int> ProgressActionBy { get; set; }
        public Nullable<System.DateTime> ProgressActionDate { get; set; }
        public string ProgressActionComments { get; set; }
    }
}
