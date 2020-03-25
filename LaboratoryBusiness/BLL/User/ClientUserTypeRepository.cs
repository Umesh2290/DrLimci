using LaboratoryBusiness.DAL.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.BLL.User
{
    public class ClientUserTypeRepository : LaboratoryBusiness.Repositories.User.IClientUserTypeRepository
    {
        private readonly LabSystemClient_RajLabEntities _context;

        public ClientUserTypeRepository()
        {
            _context = new LabSystemClient_RajLabEntities();
        }
        public ClientUserTypeRepository(LabSystemClient_RajLabEntities context)
        {
            _context = context;
        }

        public IEnumerable<LaboratoryBusiness.POCO.User.Cl_ClientUserType> GetAll()
        {
            var records = (from p in _context.Tbl_Cl_ClientUserType.AsEnumerable()
                           select new LaboratoryBusiness.POCO.User.Cl_ClientUserType
                           {
                               Description = p.Description,
                               ClientUserTypeID = p.ClientUserTypeID,
                               TypeName = p.TypeName


                           });
            return records;

        }

        public LaboratoryBusiness.POCO.User.Cl_ClientUserType GetByID(int ClientUserTypeID)
        {
            var record = (from p in _context.Tbl_Cl_ClientUserType.AsEnumerable()
                          where p.ClientUserTypeID == ClientUserTypeID
                          select new LaboratoryBusiness.POCO.User.Cl_ClientUserType
                          {
                              Description = p.Description,
                              ClientUserTypeID = p.ClientUserTypeID,
                              TypeName = p.TypeName
                          }).FirstOrDefault();

            return record;

        }

        public void Insert(LaboratoryBusiness.POCO.User.Cl_ClientUserType p)
        {
            _context.Tbl_Cl_ClientUserType.Add(new Tbl_Cl_ClientUserType()
            {
                Description = p.Description,
                //  SystemUserTypeID = p.SystemUserTypeID,
                TypeName = p.TypeName


            });
        }

        public void Update(LaboratoryBusiness.POCO.User.Cl_ClientUserType p)
        {
            var record = _context.Tbl_Cl_ClientUserType.Where(x => x.ClientUserTypeID == p.ClientUserTypeID).SingleOrDefault();
            if (record != null)
            {
                record.Description = p.Description;
                record.ClientUserTypeID = p.ClientUserTypeID;
                record.TypeName = p.TypeName;


            }
            else
            {
                throw new Exception("Record not found");
            }
        }

        public void Delete(int ClientUserTypeID)
        {
            var record = _context.Tbl_Cl_ClientUserType.Where(x => x.ClientUserTypeID == ClientUserTypeID).SingleOrDefault();
            _context.Tbl_Cl_ClientUserType.Remove(record);
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
