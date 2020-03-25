using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.POCO.User
{
    public  class Cl_ClientUserAttachmentDetail
    {
        public int AttachmentID { get; set; }
        public string AttachmentName { get; set; }
        public string Link { get; set; }
        public string Extension { get; set; }
        public Nullable<int> UserDetailID { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
    }
}
