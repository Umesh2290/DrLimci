using LaboratoryBusiness.DAL.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.BLL.User
{

    public class ClientUserDetailRepository : LaboratoryBusiness.Repositories.User.IClientUserDetailRepository
    {
        private readonly LabSystemClient_RajLabEntities _context;
        private Tbl_Cl_ClientUserDetail _ClientUserDetail_entity = new Tbl_Cl_ClientUserDetail();
        private LaboratoryBusiness.POCO.User.Cl_ClientUserDetail _ClientUserDetail_poco = new POCO.User.Cl_ClientUserDetail();

        public ClientUserDetailRepository()
        {
            _context = new LabSystemClient_RajLabEntities();
        }
        public ClientUserDetailRepository(LabSystemClient_RajLabEntities context)
        {
            _context = context;
        }

        public IEnumerable<LaboratoryBusiness.POCO.User.Cl_ClientUserDetail> GetAll()
        {

            var records = (from p in _context.Tbl_Cl_ClientUserDetail.AsEnumerable()
                           select new LaboratoryBusiness.POCO.User.Cl_ClientUserDetail
                           {
                               
                              CreatedBy=p.CreatedBy,
                              CreatedDate=p.CreatedDate,
                              IsFulltime=p.IsFulltime,
                              JoiningDate=p.JoiningDate,
                              License=p.License,
                              PdfLink=p.PdfLink,
                              Qualifications=p.Qualifications,
                              TerminationDate=p.TerminationDate,
                              TerminationReason=p.TerminationReason,
                              TypeOfCollection=p.TypeOfCollection,
                              UpdatedBy=p.UpdatedBy,
                              UpdatedDate=p.UpdatedDate,
                              UserDetailID=p.UserDetailID

                           });
            return records;

        }

        public LaboratoryBusiness.POCO.User.Cl_ClientUserDetail GetByID(int UserDetailID)
        {
            var record = (from p in _context.Tbl_Cl_ClientUserDetail.AsEnumerable()
                          where p.UserDetailID == UserDetailID
                          select new LaboratoryBusiness.POCO.User.Cl_ClientUserDetail
                          {
                              CreatedBy = p.CreatedBy,
                              CreatedDate = p.CreatedDate,
                              IsFulltime = p.IsFulltime,
                              JoiningDate = p.JoiningDate,
                              License = p.License,
                              PdfLink = p.PdfLink,
                              Qualifications = p.Qualifications,
                              TerminationDate = p.TerminationDate,
                              TerminationReason = p.TerminationReason,
                              TypeOfCollection = p.TypeOfCollection,
                              UpdatedBy = p.UpdatedBy,
                              UpdatedDate = p.UpdatedDate,
                              UserDetailID = p.UserDetailID

                          }).FirstOrDefault();
            return record;

        }

        public void Insert(LaboratoryBusiness.POCO.User.Cl_ClientUserDetail p)
        {
            Tbl_Cl_ClientUserDetail curd = new Tbl_Cl_ClientUserDetail()
            {

                CreatedBy = p.CreatedBy,
                CreatedDate = p.CreatedDate,
                IsFulltime = p.IsFulltime,
                JoiningDate = p.JoiningDate,
                License = p.License,
                PdfLink = p.PdfLink,
                Qualifications = p.Qualifications,
                TerminationDate = p.TerminationDate,
                TerminationReason = p.TerminationReason,
                TypeOfCollection = p.TypeOfCollection,
                UpdatedBy = p.UpdatedBy,
                UpdatedDate = p.UpdatedDate,
                UserDetailID = p.UserDetailID
            };
            _context.Tbl_Cl_ClientUserDetail.Add(curd);

            _ClientUserDetail_entity = curd;
            _ClientUserDetail_poco = p;

        }

        public void Update(LaboratoryBusiness.POCO.User.Cl_ClientUserDetail p)
        {
            var record = _context.Tbl_Cl_ClientUserDetail.Where(x => x.UserDetailID == p.UserDetailID).SingleOrDefault();
            if (record != null)
            {
                record.CreatedBy = p.CreatedBy;
                record.CreatedDate = p.CreatedDate;
                record.IsFulltime = p.IsFulltime;
                record.JoiningDate = p.JoiningDate;
                record.License = p.License;
                record.PdfLink = p.PdfLink;
                record.Qualifications = p.Qualifications;
                record.TerminationDate = p.TerminationDate;
                record.TerminationReason = p.TerminationReason;
                record.TypeOfCollection = p.TypeOfCollection;
                record.UpdatedBy = p.UpdatedBy;
                record.UpdatedDate = p.UpdatedDate;
                record.UserDetailID = p.UserDetailID;
            }
            else
            {
                throw new Exception("Record not found");
            }
        }

        public void Delete(int UserDetailID)
        {
            var record = _context.Tbl_Cl_ClientUserDetail.Where(x => x.UserDetailID == UserDetailID).SingleOrDefault();
            _context.Tbl_Cl_ClientUserDetail.Remove(record);
        }

        public void Save()
        {
            _context.SaveChanges();
            _ClientUserDetail_poco.UserDetailID = _ClientUserDetail_entity.UserDetailID;
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
