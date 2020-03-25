using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LaboratorySystem
{
    public class Cl_Role : LaboratoryBusiness.POCO.User.Cl_Role
    {
        public Nullable<bool> IsDefault { get; set; }
        public List<LaboratoryBusiness.POCO.User.Cl_Menu> AssignedMenus { get; set; }
        public List<Cl_SectionRestriction> SectionRestrictions { get; set; }
    }
}