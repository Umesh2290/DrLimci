using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.POCO.Admin
{
    public class ClientParameter
    {
        public int ClientParameterID { get; set; }
        public Nullable<int> ParameterID { get; set; }
        public string ParameterValue { get; set; }
        public Nullable<int> ClientDetailID { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }

    }
}
