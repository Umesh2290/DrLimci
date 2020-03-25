using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.POCO.Admin
{
    public class ForgetPassword
    {
        public int ForgetPasswordID { get; set; }
        public string UniqueCode { get; set; }
        public Nullable<int> SystemUserID { get; set; }
        public Nullable<int> ExpiresInMinutes { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    }
}
