using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.POCO.User
{
    public class Cl_OpinionRequest
    {
        public int OpinionRequestID { get; set; }
        public string PatientDetails { get; set; }
        public string SampleAnalysisDetails { get; set; }
        public string OpinionNeededDescription { get; set; }
        public string ConsultationOpinion { get; set; }
        public Nullable<int> SystemOpinionRequestID { get; set; }
        public Nullable<int> StatusID { get; set; }
        public Nullable<System.DateTime> RequestCreatedDate { get; set; }
        public Nullable<int> RequestCreatedBy { get; set; }
        public Nullable<System.DateTime> NewActionDate { get; set; }
        public Nullable<int> NewActionBy { get; set; }
        public Nullable<int> NewActionStatusID { get; set; }
        public string NewActionComments { get; set; }
        public Nullable<int> PendingActionBy { get; set; }
        public Nullable<System.DateTime> PendingActionDate { get; set; }
        public string PendingActionComments { get; set; }
        public string OpinionBy { get; set; }
        public string CommentForRequester { get; set; }
        public Nullable<bool> IsPublish { get; set; }
    }
}
