using LaboratoryBusiness.DAL.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.BLL.User
{
    public class OpinionRequestRepository : LaboratoryBusiness.Repositories.User.IOpinionRequestRepository
    {
        private readonly LabSystemClient_RajLabEntities _context;
        private Tbl_Cl_OpinionRequest OpinionRequest_entity = new Tbl_Cl_OpinionRequest();
        private LaboratoryBusiness.POCO.User.Cl_OpinionRequest OpinionRequest_poco = new POCO.User.Cl_OpinionRequest();

        public OpinionRequestRepository()
        {
            _context = new LabSystemClient_RajLabEntities();
        }
        public OpinionRequestRepository(LabSystemClient_RajLabEntities context)
        {
            _context = context;
        }

        public IEnumerable<LaboratoryBusiness.POCO.User.Cl_OpinionRequest> GetAll()
        {

            var records = (from p in _context.Tbl_Cl_OpinionRequest.AsEnumerable()
                           select new LaboratoryBusiness.POCO.User.Cl_OpinionRequest
                           {

                          ConsultationOpinion=p.ConsultationOpinion,
                          NewActionBy=p.NewActionBy,
                          NewActionComments=p.NewActionComments,
                          NewActionDate=p.NewActionDate,
                          NewActionStatusID=p.NewActionStatusID,
                          OpinionNeededDescription=p.OpinionNeededDescription,
                          OpinionRequestID=p.OpinionRequestID,
                          PatientDetails=p.PatientDetails,
                          PendingActionBy=p.PendingActionBy,
                          PendingActionComments=p.PendingActionComments,
                          PendingActionDate=p.PendingActionDate,
                          RequestCreatedBy=p.RequestCreatedBy,
                          RequestCreatedDate=p.RequestCreatedDate,
                          SampleAnalysisDetails=p.SampleAnalysisDetails,
                          StatusID=p.StatusID,
                          SystemOpinionRequestID=p.SystemOpinionRequestID,
                          CommentForRequester = p.CommentForRequester,
                          OpinionBy = p.OpinionBy,
                          IsPublish = p.IsPublish


                           });
            return records;

        }

        public LaboratoryBusiness.POCO.User.Cl_OpinionRequest GetByID(int OpinionRequestID)
        {
            var record = (from p in _context.Tbl_Cl_OpinionRequest.AsEnumerable()
                          where p.OpinionRequestID == OpinionRequestID
                          select new LaboratoryBusiness.POCO.User.Cl_OpinionRequest
                          {


                              ConsultationOpinion = p.ConsultationOpinion,
                              NewActionBy = p.NewActionBy,
                              NewActionComments = p.NewActionComments,
                              NewActionDate = p.NewActionDate,
                              NewActionStatusID = p.NewActionStatusID,
                              OpinionNeededDescription = p.OpinionNeededDescription,
                              OpinionRequestID = p.OpinionRequestID,
                              PatientDetails = p.PatientDetails,
                              PendingActionBy = p.PendingActionBy,
                              PendingActionComments = p.PendingActionComments,
                              PendingActionDate = p.PendingActionDate,
                              RequestCreatedBy = p.RequestCreatedBy,
                              RequestCreatedDate = p.RequestCreatedDate,
                              SampleAnalysisDetails = p.SampleAnalysisDetails,
                              StatusID = p.StatusID,
                              SystemOpinionRequestID = p.SystemOpinionRequestID,
                              CommentForRequester = p.CommentForRequester,
                              OpinionBy = p.OpinionBy,
                              IsPublish = p.IsPublish



                          }).FirstOrDefault();
            return record;

        }

        public void Insert(LaboratoryBusiness.POCO.User.Cl_OpinionRequest p)
        {
            Tbl_Cl_OpinionRequest inp = new Tbl_Cl_OpinionRequest()
            {
                ConsultationOpinion = p.ConsultationOpinion,
                NewActionBy = p.NewActionBy,
                NewActionComments = p.NewActionComments,
                NewActionDate = p.NewActionDate,
                NewActionStatusID = p.NewActionStatusID,
                OpinionNeededDescription = p.OpinionNeededDescription,
                OpinionRequestID = p.OpinionRequestID,
                PatientDetails = p.PatientDetails,
                PendingActionBy = p.PendingActionBy,
                PendingActionComments = p.PendingActionComments,
                PendingActionDate = p.PendingActionDate,
                RequestCreatedBy = p.RequestCreatedBy,
                RequestCreatedDate = p.RequestCreatedDate,
                SampleAnalysisDetails = p.SampleAnalysisDetails,
                StatusID = p.StatusID,
                SystemOpinionRequestID = p.SystemOpinionRequestID,
                CommentForRequester = p.CommentForRequester,
                OpinionBy = p.OpinionBy,
                IsPublish = p.IsPublish

            };
            _context.Tbl_Cl_OpinionRequest.Add(inp);

            OpinionRequest_entity = inp;
            OpinionRequest_poco = p;

        }

        public void Update(LaboratoryBusiness.POCO.User.Cl_OpinionRequest p)
        {
            var record = _context.Tbl_Cl_OpinionRequest.Where(x => x.OpinionRequestID == p.OpinionRequestID).SingleOrDefault();
            if (record != null)
            {
   record.ConsultationOpinion = p.ConsultationOpinion;
                              record.NewActionBy = p.NewActionBy;
                              record.NewActionComments = p.NewActionComments;
                              record.NewActionDate = p.NewActionDate;
                              record.NewActionStatusID = p.NewActionStatusID;
                              record.OpinionNeededDescription = p.OpinionNeededDescription;
                              record.OpinionRequestID = p.OpinionRequestID;
                              record.PatientDetails = p.PatientDetails;
                              record.PendingActionBy = p.PendingActionBy;
                              record.PendingActionComments = p.PendingActionComments;
                              record.PendingActionDate = p.PendingActionDate;
                              record.RequestCreatedBy = p.RequestCreatedBy;
                              record.RequestCreatedDate = p.RequestCreatedDate;
                              record.SampleAnalysisDetails = p.SampleAnalysisDetails;
                              record.StatusID = p.StatusID;
                              record.SystemOpinionRequestID = p.SystemOpinionRequestID;
                              record.OpinionBy = p.OpinionBy;
                              record.CommentForRequester = p.CommentForRequester;
                              record.IsPublish = p.IsPublish;
            }
            else
            {
                throw new Exception("Record not found");
            }
        }

        public void Delete(int OpinionRequestID)
        {
            var record = _context.Tbl_Cl_OpinionRequest.Where(x => x.OpinionRequestID == OpinionRequestID).SingleOrDefault();
            _context.Tbl_Cl_OpinionRequest.Remove(record);
        }

        public void Save()
        {
            _context.SaveChanges();
            OpinionRequest_poco.OpinionRequestID = OpinionRequest_entity.OpinionRequestID;
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
