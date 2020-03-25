using LaboratoryBusiness.DAL.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.BLL.User
{

    public class ExtraWorkRequestRepository : LaboratoryBusiness.Repositories.User.IExtraWorkRequestReposotory
    {
        private readonly LabSystemClient_RajLabEntities _context;
        private Tbl_Cl_ExtraWorkRequest ExtraWorkRequest_entity = new Tbl_Cl_ExtraWorkRequest();
        private LaboratoryBusiness.POCO.User.Cl_ExtraWorkRequest ExtraWorkRequestt_poco = new POCO.User.Cl_ExtraWorkRequest();

        public ExtraWorkRequestRepository()
        {
            _context = new LabSystemClient_RajLabEntities();
        }
        public ExtraWorkRequestRepository(LabSystemClient_RajLabEntities context)
        {
            _context = context;
        }

        public IEnumerable<LaboratoryBusiness.POCO.User.Cl_ExtraWorkRequest> GetAll()
        {

            var records = (from p in _context.Tbl_Cl_ExtraWorkRequest.AsEnumerable()
                           select new LaboratoryBusiness.POCO.User.Cl_ExtraWorkRequest
                           {
                               ExtraWorkID = p.ExtraWorkID,
                               H_ELevels = p.H_ELevels,
                               ImmunoHistoChemistry = p.ImmunoHistoChemistry,
                               NewActionBy = p.NewActionBy,
                               NewActionComments = p.NewActionComments,
                               NewActionDate = p.NewActionDate,
                               NewActionStatusID = p.NewActionStatusID,
                               Others = p.Others,
                               PendingActionBy = p.PendingActionBy,
                               PendingActionComments = p.PendingActionComments,
                               PendingActionDate = p.PendingActionDate,
                               RequestCreatedBy = p.RequestCreatedBy,
                               RequestCreatedDate = p.RequestCreatedDate,
                               SpecialStains = p.SpecialStains,
                               StatusID = p.StatusID,
                               TestID = p.TestID

                           });
            return records;

        }

        public LaboratoryBusiness.POCO.User.Cl_ExtraWorkRequest GetByID(int ExtraWorkID)
        {
            var record = (from p in _context.Tbl_Cl_ExtraWorkRequest.AsEnumerable()
                          where p.ExtraWorkID == ExtraWorkID
                          select new LaboratoryBusiness.POCO.User.Cl_ExtraWorkRequest
                          {
                              ExtraWorkID = p.ExtraWorkID,
                              H_ELevels = p.H_ELevels,
                              ImmunoHistoChemistry = p.ImmunoHistoChemistry,
                              NewActionBy = p.NewActionBy,
                              NewActionComments = p.NewActionComments,
                              NewActionDate = p.NewActionDate,
                              NewActionStatusID = p.NewActionStatusID,
                              Others = p.Others,
                              PendingActionBy = p.PendingActionBy,
                              PendingActionComments = p.PendingActionComments,
                              PendingActionDate = p.PendingActionDate,
                              RequestCreatedBy = p.RequestCreatedBy,
                              RequestCreatedDate = p.RequestCreatedDate,
                              SpecialStains = p.SpecialStains,
                              StatusID = p.StatusID,
                              TestID = p.TestID


                          }).FirstOrDefault();
            return record;

        }

        public void Insert(LaboratoryBusiness.POCO.User.Cl_ExtraWorkRequest p)
        {
            Tbl_Cl_ExtraWorkRequest curd = new Tbl_Cl_ExtraWorkRequest()
            {

                ExtraWorkID = p.ExtraWorkID,
                H_ELevels = p.H_ELevels,
                ImmunoHistoChemistry = p.ImmunoHistoChemistry,
                NewActionBy = p.NewActionBy,
                NewActionComments = p.NewActionComments,
                NewActionDate = p.NewActionDate,
                NewActionStatusID = p.NewActionStatusID,
                Others = p.Others,
                PendingActionBy = p.PendingActionBy,
                PendingActionComments = p.PendingActionComments,
                PendingActionDate = p.PendingActionDate,
                RequestCreatedBy = p.RequestCreatedBy,
                RequestCreatedDate = p.RequestCreatedDate,
                SpecialStains = p.SpecialStains,
                StatusID = p.StatusID,
                TestID = p.TestID
            };
            _context.Tbl_Cl_ExtraWorkRequest.Add(curd);

            ExtraWorkRequest_entity = curd;
            ExtraWorkRequestt_poco = p;

        }

        public void Update(LaboratoryBusiness.POCO.User.Cl_ExtraWorkRequest p)
        {
            var record = _context.Tbl_Cl_ExtraWorkRequest.Where(x => x.ExtraWorkID == p.ExtraWorkID).SingleOrDefault();
            if (record != null)
            {
                 record.ExtraWorkID = p.ExtraWorkID;
                               record.H_ELevels = p.H_ELevels;
                               record.ImmunoHistoChemistry = p.ImmunoHistoChemistry;
                               record.NewActionBy = p.NewActionBy;
                               record.NewActionComments = p.NewActionComments;
                               record.NewActionDate = p.NewActionDate;
                               record.NewActionStatusID = p.NewActionStatusID;
                               record.Others = p.Others;
                               record.PendingActionBy = p.PendingActionBy;
                               record.PendingActionComments = p.PendingActionComments;
                               record.PendingActionDate = p.PendingActionDate;
                               record.RequestCreatedBy = p.RequestCreatedBy;
                               record.RequestCreatedDate = p.RequestCreatedDate;
                               record.SpecialStains = p.SpecialStains;
                               record.StatusID = p.StatusID;
                               record.TestID = p.TestID;
            }
            else
            {
                throw new Exception("Record not found");
            }
        }

        public void Delete(int ExtraWorkID)
        {
            var record = _context.Tbl_Cl_ExtraWorkRequest.Where(x => x.ExtraWorkID == ExtraWorkID).SingleOrDefault();
            _context.Tbl_Cl_ExtraWorkRequest.Remove(record);
        }

        public void Save()
        {
            _context.SaveChanges();
            ExtraWorkRequestt_poco.ExtraWorkID = ExtraWorkRequest_entity.ExtraWorkID;
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

