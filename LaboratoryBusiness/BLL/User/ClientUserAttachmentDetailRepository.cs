using LaboratoryBusiness.DAL.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.BLL.User
{
    public class ClientUserAttachmentDetailRepository : LaboratoryBusiness.Repositories.User.IClientUserAttachmentDetailRepository
    {
        private readonly LabSystemClient_RajLabEntities _context;
        private Tbl_Cl_ClientUserAttachmentDetail _ClientUserAttachmentDetail_entity = new Tbl_Cl_ClientUserAttachmentDetail();
        private LaboratoryBusiness.POCO.User.Cl_ClientUserAttachmentDetail _ClientUserAttachmentDetail_poco = new POCO.User.Cl_ClientUserAttachmentDetail();

        public ClientUserAttachmentDetailRepository()
        {
            _context = new LabSystemClient_RajLabEntities();
        }
        public ClientUserAttachmentDetailRepository(LabSystemClient_RajLabEntities context)
        {
            _context = context;
        }

        public IEnumerable<LaboratoryBusiness.POCO.User.Cl_ClientUserAttachmentDetail> GetAll()
        {

            var records = (from p in _context.Tbl_Cl_ClientUserAttachmentDetail.AsEnumerable()
                           select new LaboratoryBusiness.POCO.User.Cl_ClientUserAttachmentDetail
                           {
                               AttachmentID = p.AttachmentID,

                               AttachmentName = p.AttachmentName,
                               CreatedBy = p.CreatedBy,
                               CreatedDate = p.CreatedDate,
                               Extension = p.Extension,
                               Link = p.Link,
                               UpdatedBy = p.UpdatedBy,
                               UpdatedDate = p.UpdatedDate,
                               UserDetailID = p.UserDetailID

                           });
            return records;

        }

        public LaboratoryBusiness.POCO.User.Cl_ClientUserAttachmentDetail GetByID(int AttachmentID)
        {
            var record = (from p in _context.Tbl_Cl_ClientUserAttachmentDetail.AsEnumerable()
                          where p.AttachmentID == AttachmentID
                          select new LaboratoryBusiness.POCO.User.Cl_ClientUserAttachmentDetail
                          {
                              AttachmentID = p.AttachmentID,

                              AttachmentName = p.AttachmentName,
                              CreatedBy = p.CreatedBy,
                              CreatedDate = p.CreatedDate,
                              Extension = p.Extension,
                              Link = p.Link,
                              UpdatedBy = p.UpdatedBy,
                              UpdatedDate = p.UpdatedDate,
                              UserDetailID = p.UserDetailID

                          }).FirstOrDefault();
            return record;

        }

        public void Insert(LaboratoryBusiness.POCO.User.Cl_ClientUserAttachmentDetail cp)
        {
            Tbl_Cl_ClientUserAttachmentDetail curd = new Tbl_Cl_ClientUserAttachmentDetail()
            {
                AttachmentID = cp.AttachmentID,

                AttachmentName = cp.AttachmentName,
                CreatedBy = cp.CreatedBy,
                CreatedDate = cp.CreatedDate,
                Extension = cp.Extension,
                Link = cp.Link,
                UpdatedBy = cp.UpdatedBy,
                UpdatedDate = cp.UpdatedDate,
                UserDetailID = cp.UserDetailID
            };
            _context.Tbl_Cl_ClientUserAttachmentDetail.Add(curd);

            _ClientUserAttachmentDetail_entity = curd;
            _ClientUserAttachmentDetail_poco = cp;

        }

        public void Update(LaboratoryBusiness.POCO.User.Cl_ClientUserAttachmentDetail cuad)
        {
            var record = _context.Tbl_Cl_ClientUserAttachmentDetail.Where(x => x.AttachmentID == cuad.AttachmentID).SingleOrDefault();
            if (record != null)
            {
                record.AttachmentID = cuad.AttachmentID;
                record.AttachmentName = cuad.AttachmentName;
                record.CreatedBy = cuad.CreatedBy;
                record.CreatedDate = cuad.CreatedDate;
                record.Extension = cuad.Extension;
                record.Link = cuad.Link;
                record.UpdatedBy = cuad.UpdatedBy;
                record.UpdatedDate = cuad.UpdatedDate;
                record.UserDetailID = cuad.UserDetailID;
            }
            else
            {
                throw new Exception("Record not found");
            }
        }

        public void Delete(int AttachmentID)
        {
            var record = _context.Tbl_Cl_ClientUserAttachmentDetail.Where(x => x.AttachmentID == AttachmentID).SingleOrDefault();
            _context.Tbl_Cl_ClientUserAttachmentDetail.Remove(record);
        }

        public void Save()
        {
            _context.SaveChanges();
            _ClientUserAttachmentDetail_poco.AttachmentID = _ClientUserAttachmentDetail_entity.AttachmentID;
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
