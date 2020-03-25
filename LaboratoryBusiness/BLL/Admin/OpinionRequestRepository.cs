using LaboratoryBusiness.DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.BLL.Admin
{
    public class OpinionRequestRepository : LaboratoryBusiness.Repositories.Admin.IOpinionRequestRepository
    {
        private readonly LabSystemDBEntities _context;
        private Tbl_OpinionRequest _opinionrequest_entity = new Tbl_OpinionRequest();
        private LaboratoryBusiness.POCO.Admin.OpinionRequest _opinionrequest_poco = new POCO.Admin.OpinionRequest();

        public OpinionRequestRepository()
        {
            _context = new LabSystemDBEntities();
        }
        public OpinionRequestRepository(LabSystemDBEntities context)
        {
            _context = context;
        }

        public IEnumerable<LaboratoryBusiness.POCO.Admin.OpinionRequest> GetAll()
        {
            var records = (from p in _context.Tbl_OpinionRequest.AsEnumerable()
                           select new LaboratoryBusiness.POCO.Admin.OpinionRequest
                           {
                              OpinionRequestID = p.OpinionRequestID,
                              PatientDetails = p.PatientDetails,
                              Payment = p.Payment,
                              PaymentReceiptPdfLink = p.PaymentReceiptPdfLink,
                              SampleAnalysisDetails = p.SampleAnalysisDetails,
                              OpinionNeededDescription = p.OpinionNeededDescription,
                              ConsultantOpinion = p.ConsultantOpinion,
                              CommentForRequester = p.CommentForRequester,
                              CommentForConsultant = p.CommentForConsultant,
                              ClientOpinionRequestID = p.ClientOpinionRequestID,
                              ClientID = p.ClientID,
                              AssignedTo = p.AssignedTo,
                              NewActionBy=p.NewActionBy,
                              NewActionComments=p.NewActionComments,
                              NewActionDate=p.NewActionDate,
                              NewActionStatusID=p.NewActionStatusID,
                              PendingActionBy=p.PendingActionBy,
                              PendingActionComments=p.PendingActionComments,
                              PendingActionDate=p.PendingActionDate,
                              RequestCreatedDate=p.RequestCreatedDate,
                              RequestCreatedBy = p.RequestCreatedBy,
                              StatusID=p.StatusID,
                              IsPublish=p.IsPublish

                           });
            return records;

        }

        public LaboratoryBusiness.POCO.Admin.OpinionRequest GetByID(int OpinionRequestID)
        {
            var record = (from p in _context.Tbl_OpinionRequest.AsEnumerable()
                          where p.OpinionRequestID == OpinionRequestID
                          select new LaboratoryBusiness.POCO.Admin.OpinionRequest
                          {
                              OpinionRequestID = p.OpinionRequestID,
                              PatientDetails = p.PatientDetails,
                              Payment = p.Payment,
                              PaymentReceiptPdfLink = p.PaymentReceiptPdfLink,
                              SampleAnalysisDetails = p.SampleAnalysisDetails,
                              OpinionNeededDescription = p.OpinionNeededDescription,
                              ConsultantOpinion = p.ConsultantOpinion,
                              CommentForRequester = p.CommentForRequester,
                              CommentForConsultant = p.CommentForConsultant,
                              ClientOpinionRequestID = p.ClientOpinionRequestID,
                              ClientID = p.ClientID,
                              AssignedTo = p.AssignedTo,
                              NewActionBy = p.NewActionBy,
                              NewActionComments = p.NewActionComments,
                              NewActionDate = p.NewActionDate,
                              NewActionStatusID = p.NewActionStatusID,
                              PendingActionBy = p.PendingActionBy,
                              PendingActionComments = p.PendingActionComments,
                              PendingActionDate = p.PendingActionDate,
                              RequestCreatedDate = p.RequestCreatedDate,
                              RequestCreatedBy = p.RequestCreatedBy,
                              StatusID = p.StatusID,
                              IsPublish=p.IsPublish
                          }).FirstOrDefault();

            return record;

        }

        public void Insert(LaboratoryBusiness.POCO.Admin.OpinionRequest opinionrequest)
        {
            Tbl_OpinionRequest or = new Tbl_OpinionRequest()
            {
                PatientDetails = opinionrequest.PatientDetails,
                Payment = opinionrequest.Payment,
                PaymentReceiptPdfLink = opinionrequest.PaymentReceiptPdfLink,
                SampleAnalysisDetails = opinionrequest.SampleAnalysisDetails,
                OpinionNeededDescription = opinionrequest.OpinionNeededDescription,
                ConsultantOpinion = opinionrequest.ConsultantOpinion,
                CommentForRequester = opinionrequest.CommentForRequester,
                CommentForConsultant = opinionrequest.CommentForConsultant,
                ClientOpinionRequestID = opinionrequest.ClientOpinionRequestID,
                ClientID = opinionrequest.ClientID,
                AssignedTo = opinionrequest.AssignedTo,
                NewActionBy = opinionrequest.NewActionBy,
                NewActionComments = opinionrequest.NewActionComments,
                NewActionDate = opinionrequest.NewActionDate,
                NewActionStatusID = opinionrequest.NewActionStatusID,
                PendingActionBy = opinionrequest.PendingActionBy,
                PendingActionComments = opinionrequest.PendingActionComments,
                PendingActionDate = opinionrequest.PendingActionDate,
                RequestCreatedDate = opinionrequest.RequestCreatedDate,
                RequestCreatedBy = opinionrequest.RequestCreatedBy,
                StatusID = opinionrequest.StatusID,
                IsPublish = opinionrequest.IsPublish
            };

            _context.Tbl_OpinionRequest.Add(or);

            _opinionrequest_entity = or;
            _opinionrequest_poco = opinionrequest;
        }

        public void Update(LaboratoryBusiness.POCO.Admin.OpinionRequest opinionrequest)
        {
            var record = _context.Tbl_OpinionRequest.Where(x => x.OpinionRequestID == opinionrequest.OpinionRequestID).SingleOrDefault();
            if (record != null)
            {
                record.PatientDetails = opinionrequest.PatientDetails;
                record.Payment = opinionrequest.Payment;
                record.PaymentReceiptPdfLink = opinionrequest.PaymentReceiptPdfLink;
                record.SampleAnalysisDetails = opinionrequest.SampleAnalysisDetails;
                record.OpinionNeededDescription = opinionrequest.OpinionNeededDescription;
                record.ConsultantOpinion = opinionrequest.ConsultantOpinion;
                record.CommentForRequester = opinionrequest.CommentForRequester;
                record.CommentForConsultant = opinionrequest.CommentForConsultant;
                record.ClientOpinionRequestID = opinionrequest.ClientOpinionRequestID;
                record.ClientID = opinionrequest.ClientID;
                record.AssignedTo = opinionrequest.AssignedTo;
                record.NewActionBy = opinionrequest.NewActionBy;
                record.NewActionComments = opinionrequest.NewActionComments;
                record.NewActionDate = opinionrequest.NewActionDate;
                record.NewActionStatusID = opinionrequest.NewActionStatusID;
                record.PendingActionBy = opinionrequest.PendingActionBy;
                record.PendingActionComments = opinionrequest.PendingActionComments;
                record.PendingActionDate = opinionrequest.PendingActionDate;
                record.RequestCreatedDate = opinionrequest.RequestCreatedDate;
                record.RequestCreatedBy = opinionrequest.RequestCreatedBy;
                record.StatusID = opinionrequest.StatusID;
                record.IsPublish = opinionrequest.IsPublish;

            }
            else
            {
                throw new Exception("Record not found");
            }
        }

        public void Delete(int OpinionRequestID)
        {
            var record = _context.Tbl_OpinionRequest.Where(x => x.OpinionRequestID == OpinionRequestID).SingleOrDefault();
            _context.Tbl_OpinionRequest.Remove(record);
        }

        public void Save()
        {
            _context.SaveChanges();
            _opinionrequest_poco.OpinionRequestID = _opinionrequest_entity.OpinionRequestID;
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
