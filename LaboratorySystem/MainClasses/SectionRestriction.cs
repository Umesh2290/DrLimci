using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LaboratorySystem
{
    public class SectionRestriction : LaboratoryBusiness.POCO.Admin.SectionRestriction
    {
        public LaboratoryBusiness.POCO.Admin.Menu Menu { get; set; }
    }
}