using LaboratoryBusiness.DAL.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.BLL.User
{
    public class ExtraWorkRequestStatusRepository : LaboratoryBusiness.Repositories.User.IExtraWorkRequestStatusRepository
    {
        private readonly LabSystemClient_RajLabEntities _context;
        private Tbl_Cl_ExtraWorkRequestStatus ExtraWorkRequestStatus_entity = new Tbl_Cl_ExtraWorkRequestStatus();
        private LaboratoryBusiness.POCO.User.Cl_ExtraWorkRequestStatus ExtraWorkRequestStatus_poco = new POCO.User.Cl_ExtraWorkRequestStatus();

        public ExtraWorkRequestStatusRepository()
        {
            _context = new LabSystemClient_RajLabEntities();
        }
        public ExtraWorkRequestStatusRepository(LabSystemClient_RajLabEntities context)
        {
            _context = context;
        }

        public IEnumerable<LaboratoryBusiness.POCO.User.Cl_ExtraWorkRequestStatus> GetAll()
        {
            var records = (from p in _context.Tbl_Cl_ExtraWorkRequestStatus.AsEnumerable()
                           select new LaboratoryBusiness.POCO.User.Cl_ExtraWorkRequestStatus
                           {
                              Description=p.Description,
                              StatusName=p.StatusName,
                              WorkRequestStatusID=p.WorkRequestStatusID

                           });
            return records;

        }

        public LaboratoryBusiness.POCO.User.Cl_ExtraWorkRequestStatus GetByID(int WorkRequestStatusID)
        {
            var record = (from p in _context.Tbl_Cl_ExtraWorkRequestStatus.AsEnumerable()
                          where p.WorkRequestStatusID == WorkRequestStatusID
                          select new LaboratoryBusiness.POCO.User.Cl_ExtraWorkRequestStatus
                          {
                              Description = p.Description,
                              StatusName = p.StatusName,
                              WorkRequestStatusID = p.WorkRequestStatusID
                          }).FirstOrDefault();

            return record;

        }

        public void Insert(LaboratoryBusiness.POCO.User.Cl_ExtraWorkRequestStatus p)
        {
            Tbl_Cl_ExtraWorkRequestStatus cl_user = new Tbl_Cl_ExtraWorkRequestStatus()
            {
               
                Description = p.Description,
                StatusName = p.StatusName,
                WorkRequestStatusID = p.WorkRequestStatusID

            };
            _context.Tbl_Cl_ExtraWorkRequestStatus.Add(cl_user);

            ExtraWorkRequestStatus_entity = cl_user;
            ExtraWorkRequestStatus_poco = p;
        }

        public void Update(LaboratoryBusiness.POCO.User.Cl_ExtraWorkRequestStatus p)
        {
            var record = _context.Tbl_Cl_ExtraWorkRequestStatus.Where(x => x.WorkRequestStatusID == p.WorkRequestStatusID).SingleOrDefault();
            if (record != null)
            {
                  record.Description=p.Description;
                              record.StatusName=p.StatusName;
                              record.WorkRequestStatusID = p.WorkRequestStatusID;

            }
            else
            {
                throw new Exception("Record not found");
            }
        }

        public void Delete(int WorkRequestStatusID)
        {
            var record = _context.Tbl_Cl_ExtraWorkRequestStatus.Where(x => x.WorkRequestStatusID == WorkRequestStatusID).SingleOrDefault();
            _context.Tbl_Cl_ExtraWorkRequestStatus.Remove(record);
        }

      

        public void Save()
        {
            _context.SaveChanges();
            ExtraWorkRequestStatus_poco.WorkRequestStatusID = ExtraWorkRequestStatus_entity.WorkRequestStatusID;
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
