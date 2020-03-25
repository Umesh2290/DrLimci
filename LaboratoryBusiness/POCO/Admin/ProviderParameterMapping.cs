using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.POCO.Admin
{
    public class ProviderParameterMapping
    {
        public int ProviderParameterMappingID { get; set; }
        public Nullable<int> ProviderID { get; set; }
        public Nullable<int> ParameterID { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
    }
}
