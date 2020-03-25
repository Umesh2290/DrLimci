using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.POCO.User
{
    public class Cl_Menu
    {
        public int MenuID { get; set; }
        public string MenuName { get; set; }
        public string Description { get; set; }
        public Nullable<int> ParentID { get; set; }
        public string Icon { get; set; }
        public string ToolTip { get; set; }
        public string Link { get; set; }
        public Nullable<bool> IsViewable { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
    }
}
