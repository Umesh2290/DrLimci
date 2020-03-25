using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.POCO.Admin
{
    public class Notification
    {
        public int NotificationID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public string Type { get; set; }
        public string ClickLink { get; set; }
        public Nullable<System.DateTime> CreatedDatetime { get; set; }
        public Nullable<bool> Isviewed { get; set; }
        public Nullable<System.DateTime> ViewedDatetime { get; set; }
        public Nullable<int> EmployeeID { get; set; }
        public Nullable<int> CreatedBy { get; set; }
    }
}
