using LaboratoryBusiness.DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.BLL.Admin
{
   public class ClientParameterRepository : LaboratoryBusiness.Repositories.Admin.IClientParameterRepository
    {
        private readonly LabSystemDBEntities _context;

        public ClientParameterRepository()
        {
            _context = new LabSystemDBEntities();
        }
        public ClientParameterRepository(LabSystemDBEntities context)
        {
            _context = context;
        }

        public IEnumerable<LaboratoryBusiness.POCO.Admin.ClientParameter> GetAll()
        {

            var records = (from p in _context.Tbl_ClientParameter.AsEnumerable()
                           select new LaboratoryBusiness.POCO.Admin.ClientParameter
                           {
                              ClientDetailID=p.ClientDetailID,
                              ClientParameterID=p.ClientParameterID,
                              CreatedBy=p.CreatedBy,
                              CreatedDate=p.CreatedDate,
                              ParameterID=p.ParameterID,
                              ParameterValue=p.ParameterValue,
                              UpdatedBy=p.UpdatedBy,
                              UpdatedDate=p.UpdatedDate
                              
                              
                           

                           });
            return records;

        }

        public LaboratoryBusiness.POCO.Admin.ClientParameter GetByID(int ClientParameterID)
        {
            var record = (from p in _context.Tbl_ClientParameter.AsEnumerable()
                          where p.ClientParameterID == ClientParameterID
                          select new LaboratoryBusiness.POCO.Admin.ClientParameter
                          {
                              ClientDetailID = p.ClientDetailID,
                              ClientParameterID = p.ClientParameterID,
                              CreatedBy = p.CreatedBy,
                              CreatedDate = p.CreatedDate,
                              ParameterID = p.ParameterID,
                              ParameterValue = p.ParameterValue,
                              UpdatedBy = p.UpdatedBy,
                              UpdatedDate = p.UpdatedDate
                              
                          }).FirstOrDefault();
            return record;

        }

        public void Insert(LaboratoryBusiness.POCO.Admin.ClientParameter ClientParameter)
        {
            _context.Tbl_ClientParameter.Add(new Tbl_ClientParameter()
            {
                ClientDetailID = ClientParameter.ClientDetailID,
                //ClientParameterID = ClientParameter.ClientParameterID,
                CreatedBy = ClientParameter.CreatedBy,
                CreatedDate = ClientParameter.CreatedDate,
                ParameterID = ClientParameter.ParameterID,
                ParameterValue = ClientParameter.ParameterValue,
                UpdatedBy = ClientParameter.UpdatedBy,
                UpdatedDate = ClientParameter.UpdatedDate
            });
        }

        public void Update(LaboratoryBusiness.POCO.Admin.ClientParameter ClientParameter)
        {
            var record = _context.Tbl_ClientParameter.Where(x => x.ClientParameterID == ClientParameter.ClientParameterID).SingleOrDefault();
            if (record != null)
            {
                record.ClientDetailID = ClientParameter.ClientDetailID;
                //ClientParameterID = ClientParameter.ClientParameterID,
                record.CreatedBy = ClientParameter.CreatedBy;
                record.CreatedDate = ClientParameter.CreatedDate;
                record.ParameterID = ClientParameter.ParameterID;
                record.ParameterValue = ClientParameter.ParameterValue;
                record.UpdatedBy = ClientParameter.UpdatedBy;
                record.UpdatedDate = ClientParameter.UpdatedDate;
            }
            else
            {
                throw new Exception("Record not found");
            }
        }

        public void Delete(int ClientParameterID)
        {
            var record = _context.Tbl_ClientParameter.Where(x => x.ClientParameterID == ClientParameterID).SingleOrDefault();
            _context.Tbl_ClientParameter.Remove(record);
        }

        public void Save()
        {
            _context.SaveChanges();
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
