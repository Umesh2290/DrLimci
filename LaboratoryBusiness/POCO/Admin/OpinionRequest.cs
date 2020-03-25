using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.POCO.Admin
{
    public class OpinionRequest
    {
        public int OpinionRequestID { get; set; }
        public string PatientDetails { get; set; }
        public string SampleAnalysisDetails { get; set; }
        public string OpinionNeededDescription { get; set; }
        public string ConsultantOpinion { get; set; }
        public Nullable<int> ClientOpinionRequestID { get; set; }
        public Nullable<int> ClientID { get; set; }
        public Nullable<int> StatusID { get; set; }
        public Nullable<int> AssignedTo { get; set; }
        public string CommentForRequester { get; set; }
        public string CommentForConsultant { get; set; }
        public Nullable<decimal> Payment { get; set; }
        public string PaymentReceiptPdfLink { get; set; }
        public Nullable<System.DateTime> RequestCreatedDate { get; set; }
        public Nullable<int> RequestCreatedBy { get; set; }
        public Nullable<System.DateTime> NewActionDate { get; set; }
        public Nullable<int> NewActionBy { get; set; }
        public Nullable<int> NewActionStatusID { get; set; }
        public string NewActionComments { get; set; }
        public Nullable<int> PendingActionBy { get; set; }
        public Nullable<System.DateTime> PendingActionDate { get; set; }
        public string PendingActionComments { get; set; }
        public Nullable<bool> IsPublish { get; set; }
    }
}
