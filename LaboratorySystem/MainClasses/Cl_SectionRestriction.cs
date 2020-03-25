using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LaboratorySystem
{
    public class Cl_SectionRestriction : LaboratoryBusiness.POCO.User.Cl_SectionRestriction
    {
        public LaboratoryBusiness.POCO.User.Cl_Menu Menu { get; set; }
    }
}