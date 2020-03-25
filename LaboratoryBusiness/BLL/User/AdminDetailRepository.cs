using LaboratoryBusiness.DAL.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.BLL.User
{
    public class AdminDetailRepository : LaboratoryBusiness.Repositories.User.IAdminDetailRepository
    {
        private readonly LabSystemClient_RajLabEntities _context;
        private Tbl_Cl_AdminDetail _admindetail_entity = new Tbl_Cl_AdminDetail();
        private LaboratoryBusiness.POCO.User.Cl_AdminDetail _admindetail_poco = new POCO.User.Cl_AdminDetail();

        public AdminDetailRepository()
        {
            _context = new LabSystemClient_RajLabEntities();
        }
        public AdminDetailRepository(LabSystemClient_RajLabEntities context)
        {
            _context = context;
        }

        public IEnumerable<LaboratoryBusiness.POCO.User.Cl_AdminDetail> GetAll()
        {

            var records = (from p in _context.Tbl_Cl_AdminDetail.AsEnumerable()
                           select new LaboratoryBusiness.POCO.User.Cl_AdminDetail
                           {
                              AdminDetailID = p.AdminDetailID,
                              ClientUserID = p.ClientUserID,
                              CreatedBy = p.CreatedBy,
                              CreatedDate = p.CreatedDate,
                              SystemClientID = p.SystemClientID,
                              UpdatedBy = p.UpdatedBy,
                              UpdatedDate = p.UpdatedDate

                           });
            return records;

        }

        public LaboratoryBusiness.POCO.User.Cl_AdminDetail GetByID(int AdminDetailID)
        {
            var record = (from p in _context.Tbl_Cl_AdminDetail.AsEnumerable()
                          where p.AdminDetailID == AdminDetailID
                          select new LaboratoryBusiness.POCO.User.Cl_AdminDetail
                          {
                              AdminDetailID = p.AdminDetailID,
                              ClientUserID = p.ClientUserID,
                              CreatedBy = p.CreatedBy,
                              CreatedDate = p.CreatedDate,
                              SystemClientID = p.SystemClientID,
                              UpdatedBy = p.UpdatedBy,
                              UpdatedDate = p.UpdatedDate

                          }).FirstOrDefault();
            return record;

        }

        public void Insert(LaboratoryBusiness.POCO.User.Cl_AdminDetail admindetailpoco)
        {
            Tbl_Cl_AdminDetail admindetail = new Tbl_Cl_AdminDetail()
            {
                AdminDetailID = admindetailpoco.AdminDetailID,
                ClientUserID = admindetailpoco.ClientUserID,
                CreatedBy = admindetailpoco.CreatedBy,
                CreatedDate = admindetailpoco.CreatedDate,
                SystemClientID = admindetailpoco.SystemClientID,
                UpdatedBy = admindetailpoco.UpdatedBy,
                UpdatedDate = admindetailpoco.UpdatedDate
            };
            _context.Tbl_Cl_AdminDetail.Add(admindetail);

            _admindetail_entity = admindetail;
            _admindetail_poco = admindetailpoco;

        }

        public void Update(LaboratoryBusiness.POCO.User.Cl_AdminDetail admindetailpoco)
        {
            var record = _context.Tbl_Cl_AdminDetail.Where(x => x.AdminDetailID == admindetailpoco.AdminDetailID).SingleOrDefault();
            if (record != null)
            {
                record.AdminDetailID = admindetailpoco.AdminDetailID;
                record.ClientUserID = admindetailpoco.ClientUserID;
                record.CreatedBy = admindetailpoco.CreatedBy;
                record.CreatedDate = admindetailpoco.CreatedDate;
                record.SystemClientID = admindetailpoco.SystemClientID;
                record.UpdatedBy = admindetailpoco.UpdatedBy;
                record.UpdatedDate = admindetailpoco.UpdatedDate;
            }
            else
            {
                throw new Exception("Record not found");
            }
        }

        public void Delete(int AdminDetailID)
        {
            var record = _context.Tbl_Cl_AdminDetail.Where(x => x.AdminDetailID == AdminDetailID).SingleOrDefault();
            _context.Tbl_Cl_AdminDetail.Remove(record);
        }

        public void Save()
        {
            _context.SaveChanges();
            _admindetail_poco.AdminDetailID = _admindetail_entity.AdminDetailID;
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
