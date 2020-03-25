using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.POCO.User
{
    public class Cl_Inventory
    {
        public int InventoryID { get; set; }
        public string ItemName { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string QuantityOrdered { get; set; }
        public string QuantityLeft { get; set; }
        public string OrderedHistory { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }
        public Nullable<int> InventoryStatusID { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
    }
}
