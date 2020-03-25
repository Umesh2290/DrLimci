using LaboratoryBusiness.DAL.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LaboratoryBusiness.BLL.User
{
    public class HospitalDetailRepository : LaboratoryBusiness.Repositories.User.IHospitalDetail
    {
        private readonly LabSystemClient_RajLabEntities _context;
        private Tbl_Cl_HospitalDetail hospital_entity = new Tbl_Cl_HospitalDetail();
        private LaboratoryBusiness.POCO.User.Cl_HospitalDetail hospital_poco = new POCO.User.Cl_HospitalDetail();

        public HospitalDetailRepository()
        {
            _context = new LabSystemClient_RajLabEntities();
        }
        public HospitalDetailRepository(LabSystemClient_RajLabEntities context)
        {
            _context = context;
        }

        public IEnumerable<LaboratoryBusiness.POCO.User.Cl_HospitalDetail> GetAll()
        {

            var records = (from p in _context.Tbl_Cl_HospitalDetail.AsEnumerable()
                           select new LaboratoryBusiness.POCO.User.Cl_HospitalDetail
                           {
                               HospitalDetailID = p.HospitalDetailID,
                               HospitalName = p.HospitalName,
                               PhoneNo = p.PhoneNo,
                               Address = p.Address,
                               FaxNo=p.FaxNo,                                                           
                               City = p.City,
                               CreatedBy = p.CreatedBy,
                               CreatedDate = p.CreatedDate,
                               UpdatedBy = p.UpdatedBy,
                               UpdatedDate = p.UpdatedDate
                           });
            return records;

        }
        public LaboratoryBusiness.POCO.User.Cl_HospitalDetail GetByID(int HospitalDetailID)
        {
            var record = (from p in _context.Tbl_Cl_HospitalDetail.AsEnumerable()
                          where p.HospitalDetailID == HospitalDetailID
                          select new LaboratoryBusiness.POCO.User.Cl_HospitalDetail
                          {
                              HospitalDetailID = p.HospitalDetailID,
                              HospitalName = p.HospitalName,
                              PhoneNo = p.PhoneNo,
                              Address = p.Address,
                              FaxNo = p.FaxNo,
                              City = p.City,
                              CreatedBy = p.CreatedBy,
                              CreatedDate = p.CreatedDate,
                              UpdatedBy = p.UpdatedBy,
                              UpdatedDate = p.UpdatedDate
                          }).FirstOrDefault();
            return record;

        }

        public void Insert(LaboratoryBusiness.POCO.User.Cl_HospitalDetail p)
        {
            Tbl_Cl_HospitalDetail inp = new Tbl_Cl_HospitalDetail()
            {
                HospitalDetailID = p.HospitalDetailID,
                HospitalName = p.HospitalName,
                PhoneNo = p.PhoneNo,
                Address = p.Address,
                FaxNo = p.FaxNo,
                City = p.City,
                CreatedBy = p.CreatedBy,
                CreatedDate = p.CreatedDate,
                UpdatedBy = p.UpdatedBy,
                UpdatedDate = p.UpdatedDate

            };
            _context.Tbl_Cl_HospitalDetail.Add(inp);

            hospital_entity = inp;
            hospital_poco = p;

        }

        public void Update(LaboratoryBusiness.POCO.User.Cl_HospitalDetail p)
        {
            var record = _context.Tbl_Cl_HospitalDetail.Where(x => x.HospitalDetailID == p.HospitalDetailID).SingleOrDefault();
            if (record != null)
            {
                record.HospitalDetailID = p.HospitalDetailID;
                record.HospitalName = p.HospitalName;
                record.PhoneNo = p.PhoneNo;
                record.Address = p.Address;
                record.FaxNo = p.FaxNo;
                record.City = p.City;
                record.CreatedBy = p.CreatedBy;
                record.CreatedDate = p.CreatedDate;
                record.UpdatedBy = p.UpdatedBy;
                record.UpdatedDate = p.UpdatedDate;

            }
            else
            {
                throw new Exception("Record not found");
            }
        }

        public void Delete(int HospitalDetailID)
        {
            var record = _context.Tbl_Cl_HospitalDetail.Where(x => x.HospitalDetailID == HospitalDetailID).SingleOrDefault();
            _context.Tbl_Cl_HospitalDetail.Remove(record);
        }

        public void Save()
        {
            _context.SaveChanges();
            hospital_poco.HospitalDetailID = hospital_entity.HospitalDetailID;
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
