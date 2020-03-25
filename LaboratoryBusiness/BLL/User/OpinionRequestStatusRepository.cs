using LaboratoryBusiness.DAL.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.BLL.User
{
    public class OpinionRequestStatus : LaboratoryBusiness.Repositories.User.IOpinionRequestStatus
    {
        private readonly LabSystemClient_RajLabEntities _context;
        private Tbl_Cl_OpinionRequestStatus OpinionRequest_entity = new Tbl_Cl_OpinionRequestStatus();
        private LaboratoryBusiness.POCO.User.Cl_OpinionRequestStatus OpinionRequest_poco = new POCO.User.Cl_OpinionRequestStatus();

        public OpinionRequestStatus()
        {
            _context = new LabSystemClient_RajLabEntities();
        }
        public OpinionRequestStatus(LabSystemClient_RajLabEntities context)
        {
            _context = context;
        }

        public IEnumerable<LaboratoryBusiness.POCO.User.Cl_OpinionRequestStatus> GetAll()
        {

            var records = (from p in _context.Tbl_Cl_OpinionRequestStatus.AsEnumerable()
                           select new LaboratoryBusiness.POCO.User.Cl_OpinionRequestStatus
                           {

                            Description=p.Description,
                            Name=p.Name,
                            OpinionRequestStatusID=p.OpinionRequestStatusID



                           });
            return records;

        }

        public LaboratoryBusiness.POCO.User.Cl_OpinionRequestStatus GetByID(int OpinionRequestStatusID)
        {
            var record = (from p in _context.Tbl_Cl_OpinionRequestStatus.AsEnumerable()
                          where p.OpinionRequestStatusID == OpinionRequestStatusID
                          select new LaboratoryBusiness.POCO.User.Cl_OpinionRequestStatus
                          {
                              Description = p.Description,
                              Name = p.Name,
                              OpinionRequestStatusID = p.OpinionRequestStatusID


                          }).FirstOrDefault();
            return record;

        }

        public void Insert(LaboratoryBusiness.POCO.User.Cl_OpinionRequestStatus p)
        {
            Tbl_Cl_OpinionRequestStatus inp = new Tbl_Cl_OpinionRequestStatus()
            {
                Description = p.Description,
                Name = p.Name,
                OpinionRequestStatusID = p.OpinionRequestStatusID

            };
            _context.Tbl_Cl_OpinionRequestStatus.Add(inp);

            OpinionRequest_entity = inp;
            OpinionRequest_poco = p;

        }

        public void Update(LaboratoryBusiness.POCO.User.Cl_OpinionRequestStatus p)
        {
            var record = _context.Tbl_Cl_OpinionRequestStatus.Where(x => x.OpinionRequestStatusID == p.OpinionRequestStatusID).SingleOrDefault();
            if (record != null)
            {
                
                record.Description = p.Description;
                record.Name = p.Name;
                record.OpinionRequestStatusID = p.OpinionRequestStatusID;
            }
            else
            {
                throw new Exception("Record not found");
            }
        }

        public void Delete(int OpinionRequestStatusID)
        {
            var record = _context.Tbl_Cl_OpinionRequestStatus.Where(x => x.OpinionRequestStatusID == OpinionRequestStatusID).SingleOrDefault();
            _context.Tbl_Cl_OpinionRequestStatus.Remove(record);
        }

        public void Save()
        {
            _context.SaveChanges();
            OpinionRequest_poco.OpinionRequestStatusID = OpinionRequest_entity.OpinionRequestStatusID;
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
