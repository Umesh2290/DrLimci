using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LaboratorySystem
{
    public class ExtraWorkRequest:LaboratoryBusiness.POCO.User.Cl_ExtraWorkRequest
    {
        public List<LaboratoryBusiness.POCO.User.Cl_TestInvestigation> Investigations { get; set; }
        public List<LaboratoryBusiness.POCO.User.Cl_ExtraWorkAttachment> AttachmentList { get; set; }

        public string CompletedBy { get; set; }
        public string CompletedDateCustom { get; set; }
    }
}