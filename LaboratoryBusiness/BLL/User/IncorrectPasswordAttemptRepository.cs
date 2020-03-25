using LaboratoryBusiness.DAL.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.BLL.User
{
    public class IncorrectPasswordAttemptRepository : LaboratoryBusiness.Repositories.User.IIncorrectPasswordAttemptRepository
    {
        private readonly LabSystemClient_RajLabEntities _context;
        private Tbl_Cl_IncorrectPasswordAttempt _incorrectpasswordattempt_entity = new Tbl_Cl_IncorrectPasswordAttempt();
        private LaboratoryBusiness.POCO.User.Cl_IncorrectPasswordAttempt _incorrectpasswordattempt_poco = new POCO.User.Cl_IncorrectPasswordAttempt();

        public IncorrectPasswordAttemptRepository()
        {
            _context = new LabSystemClient_RajLabEntities();
        }
        public IncorrectPasswordAttemptRepository(LabSystemClient_RajLabEntities context)
        {
            _context = context;
        }

        public IEnumerable<LaboratoryBusiness.POCO.User.Cl_IncorrectPasswordAttempt> GetAll()
        {

            var records = (from p in _context.Tbl_Cl_IncorrectPasswordAttempt.AsEnumerable()
                           select new LaboratoryBusiness.POCO.User.Cl_IncorrectPasswordAttempt
                           {
                               IncorrectPasswordID = p.IncorrectPasswordID,
                               CreatedDate = p.CreatedDate,
                               Password = p.Password,
                               ClientUserID = p.ClientUserID,
                               IsInclude = p.IsInclude


                           });
            return records;

        }

        public LaboratoryBusiness.POCO.User.Cl_IncorrectPasswordAttempt GetByID(int IncorrectPasswordAttemptID)
        {
            var record = (from p in _context.Tbl_Cl_IncorrectPasswordAttempt.AsEnumerable()
                          where p.IncorrectPasswordID == IncorrectPasswordAttemptID
                          select new LaboratoryBusiness.POCO.User.Cl_IncorrectPasswordAttempt
                          {

                              IncorrectPasswordID = p.IncorrectPasswordID,
                              CreatedDate = p.CreatedDate,
                              Password = p.Password,
                              ClientUserID = p.ClientUserID,
                              IsInclude = p.IsInclude

                          }).FirstOrDefault();
            return record;

        }

        public void Insert(LaboratoryBusiness.POCO.User.Cl_IncorrectPasswordAttempt IncorrectPasswordAttempt)
        {
            Tbl_Cl_IncorrectPasswordAttempt inpasswordattemp = new Tbl_Cl_IncorrectPasswordAttempt()
            {

                // IncorrectPasswordID = IncorrectPasswordAttempt.IncorrectPasswordID,
                CreatedDate = IncorrectPasswordAttempt.CreatedDate,
                Password = IncorrectPasswordAttempt.Password,
                ClientUserID = IncorrectPasswordAttempt.ClientUserID,
                IsInclude = IncorrectPasswordAttempt.IsInclude
            };
            _context.Tbl_Cl_IncorrectPasswordAttempt.Add(inpasswordattemp);

            _incorrectpasswordattempt_entity = inpasswordattemp;
            _incorrectpasswordattempt_poco = IncorrectPasswordAttempt;

        }

        public void Update(LaboratoryBusiness.POCO.User.Cl_IncorrectPasswordAttempt IncorrectPasswordAttempt)
        {
            var record = _context.Tbl_Cl_IncorrectPasswordAttempt.Where(x => x.IncorrectPasswordID == IncorrectPasswordAttempt.IncorrectPasswordID).SingleOrDefault();
            if (record != null)
            {
                record.CreatedDate = IncorrectPasswordAttempt.CreatedDate;
                record.Password = IncorrectPasswordAttempt.Password;
                record.ClientUserID = IncorrectPasswordAttempt.ClientUserID;
                record.IsInclude = IncorrectPasswordAttempt.IsInclude;
            }
            else
            {
                throw new Exception("Record not found");
            }
        }

        public void Delete(int IncorrectPasswordAttemptID)
        {
            var record = _context.Tbl_Cl_IncorrectPasswordAttempt.Where(x => x.IncorrectPasswordID == IncorrectPasswordAttemptID).SingleOrDefault();
            _context.Tbl_Cl_IncorrectPasswordAttempt.Remove(record);
        }

        public void Save()
        {
            _context.SaveChanges();
            _incorrectpasswordattempt_poco.IncorrectPasswordID = _incorrectpasswordattempt_entity.IncorrectPasswordID;
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
