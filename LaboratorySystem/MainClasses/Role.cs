using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LaboratorySystem
{
    public class Role:LaboratoryBusiness.POCO.Admin.Role
    {
        public Nullable<bool> IsDefault { get; set; }
        public List<LaboratoryBusiness.POCO.Admin.Menu> AssignedMenus { get; set; }
        public List<SectionRestriction> SectionRestrictions { get; set; }

    }
}