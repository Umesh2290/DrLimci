using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.POCO.User
{
    public class Cl_MenuAssignment
    {
        public int AssignmentID { get; set; }
        public Nullable<int> MenuID { get; set; }
        public Nullable<int> RoleID { get; set; }
        public Nullable<int> EmployeeID { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
    }
}
