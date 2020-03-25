using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.POCO.User
{
    public class Cl_ForgetPassword
    {
        public int ForgetPasswordID { get; set; }
        public string UniqueCode { get; set; }
        public Nullable<int> ClientUserID { get; set; }
        public Nullable<int> ExpiresInMinutes { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    }
}
