using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.POCO.Admin
{
    public class ProviderParamter
    {

        public int ParameterID { get; set; }
        public string ParameterName { get; set; }
        public string Type { get; set; }
        public Nullable<bool> IsRequired { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
    }
}
