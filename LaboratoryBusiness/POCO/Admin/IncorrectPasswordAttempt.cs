using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.POCO.Admin
{
   public class IncorrectPasswordAttempt
    {
        public int IncorrectPasswordID { get; set; }
        public Nullable<int> SystemUserID { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string Password { get; set; }
        public Nullable<bool> IsInclude { get; set; }
    }
}
