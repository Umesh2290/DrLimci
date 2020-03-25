using LaboratoryBusiness.DAL.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.BLL.User
{
    public class ForgetPasswordRepository : LaboratoryBusiness.Repositories.User.IForgetPasswordRepository
    {
        private readonly LabSystemClient_RajLabEntities _context;
        private Tbl_Cl_ForgetPassword _forgetpassword_entity = new Tbl_Cl_ForgetPassword();
        private LaboratoryBusiness.POCO.User.Cl_ForgetPassword _forgetpassword_poco = new POCO.User.Cl_ForgetPassword();

        public ForgetPasswordRepository()
        {
            _context = new LabSystemClient_RajLabEntities();
        }
        public ForgetPasswordRepository(LabSystemClient_RajLabEntities context)
        {
            _context = context;
        }

        public IEnumerable<LaboratoryBusiness.POCO.User.Cl_ForgetPassword> GetAll()
        {

            var records = (from p in _context.Tbl_Cl_ForgetPassword.AsEnumerable()
                           select new LaboratoryBusiness.POCO.User.Cl_ForgetPassword
                           {
                               ForgetPasswordID = p.ForgetPasswordID,
                               CreatedDate = p.CreatedDate,
                               ExpiresInMinutes = p.ExpiresInMinutes,
                               ClientUserID = p.ClientUserID,
                               UniqueCode = p.UniqueCode


                           });
            return records;

        }

        public LaboratoryBusiness.POCO.User.Cl_ForgetPassword GetByID(int ForgetPasswordID)
        {
            var record = (from p in _context.Tbl_Cl_ForgetPassword.AsEnumerable()
                          where p.ForgetPasswordID == ForgetPasswordID
                          select new LaboratoryBusiness.POCO.User.Cl_ForgetPassword
                          {

                              ForgetPasswordID = p.ForgetPasswordID,
                              CreatedDate = p.CreatedDate,
                              ExpiresInMinutes = p.ExpiresInMinutes,
                              ClientUserID = p.ClientUserID,
                              UniqueCode = p.UniqueCode
                          }).FirstOrDefault();
            return record;

        }

        public LaboratoryBusiness.POCO.User.Cl_ForgetPassword GetByGUID(string guid)
        {
            var record = (from p in _context.Tbl_Cl_ForgetPassword.AsEnumerable()
                          where p.UniqueCode == guid
                          select new LaboratoryBusiness.POCO.User.Cl_ForgetPassword
                          {

                              ForgetPasswordID = p.ForgetPasswordID,
                              CreatedDate = p.CreatedDate,
                              ExpiresInMinutes = p.ExpiresInMinutes,
                              ClientUserID = p.ClientUserID,
                              UniqueCode = p.UniqueCode
                          }).FirstOrDefault();
            return record;

        }


        public void Insert(LaboratoryBusiness.POCO.User.Cl_ForgetPassword ForgetPassword)
        {
            Tbl_Cl_ForgetPassword forgpassword = new Tbl_Cl_ForgetPassword()
            {
                CreatedDate = ForgetPassword.CreatedDate,
                ExpiresInMinutes = ForgetPassword.ExpiresInMinutes,
                ClientUserID = ForgetPassword.ClientUserID,
                UniqueCode = ForgetPassword.UniqueCode
            };
            _context.Tbl_Cl_ForgetPassword.Add(forgpassword);
            Save();

            _forgetpassword_entity = forgpassword;
            _forgetpassword_poco = ForgetPassword;
        }

        public void Update(LaboratoryBusiness.POCO.User.Cl_ForgetPassword ForgetPassword)
        {
            var record = _context.Tbl_Cl_ForgetPassword.Where(x => x.ForgetPasswordID == ForgetPassword.ForgetPasswordID).SingleOrDefault();
            if (record != null)
            {
                record.CreatedDate = ForgetPassword.CreatedDate;
                record.ExpiresInMinutes = ForgetPassword.ExpiresInMinutes;
                record.ClientUserID = ForgetPassword.ClientUserID;
                record.UniqueCode = ForgetPassword.UniqueCode;
            }
            else
            {
                throw new Exception("Record not found");
            }
        }

        public void Delete(int ForgetPasswordID)
        {
            var record = _context.Tbl_Cl_ForgetPassword.Where(x => x.ForgetPasswordID == ForgetPasswordID).SingleOrDefault();
            _context.Tbl_Cl_ForgetPassword.Remove(record);
        }

        public void Save()
        {
            _context.SaveChanges();

            _forgetpassword_poco.ForgetPasswordID = _forgetpassword_entity.ForgetPasswordID;
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
