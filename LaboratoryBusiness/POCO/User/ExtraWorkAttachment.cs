using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.POCO.User
{
    public class Cl_ExtraWorkAttachment
    {
        public int ExtraWorkAttachmentID { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
        public string Link { get; set; }
        public Nullable<int> ExtraWorkID { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    }
}
