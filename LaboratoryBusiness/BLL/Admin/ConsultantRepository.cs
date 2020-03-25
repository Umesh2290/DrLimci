using LaboratoryBusiness.DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.BLL.Admin
{
   public class ConsultantRepository : LaboratoryBusiness.Repositories.Admin.IConsultantRepository
    {
        private readonly LabSystemDBEntities _context;
        private Tbl_Consultant _consultant_entity = new Tbl_Consultant();
        private LaboratoryBusiness.POCO.Admin.Consultant _consultant_poco = new POCO.Admin.Consultant();

        public ConsultantRepository()
        {
            _context = new LabSystemDBEntities();
        }
        public ConsultantRepository(LabSystemDBEntities context)
        {
            _context = context;
        }

        public IEnumerable<LaboratoryBusiness.POCO.Admin.Consultant> GetAll()
        {

            var records = (from p in _context.Tbl_Consultant.AsEnumerable()
                           select new LaboratoryBusiness.POCO.Admin.Consultant
                           {
                              ConsultantID=p.ConsultantID,
                              CreatedBy=p.CreatedBy,
                              CreatedDate=p.CreatedDate,
                              SpecialisationAreas=p.SpecialisationAreas,
                              UpdatedBy=p.UpdatedBy,
                              UpdatedDate=p.UpdatedDate


                           });
            return records;

        }

        public LaboratoryBusiness.POCO.Admin.Consultant GetByID(int ConsultantID)
        {
            var record = (from p in _context.Tbl_Consultant.AsEnumerable()
                          where p.ConsultantID == ConsultantID
                          select new LaboratoryBusiness.POCO.Admin.Consultant
                          {
                              ConsultantID = p.ConsultantID,
                              CreatedBy = p.CreatedBy,
                              CreatedDate = p.CreatedDate,
                              SpecialisationAreas = p.SpecialisationAreas,
                              UpdatedBy = p.UpdatedBy,
                              UpdatedDate = p.UpdatedDate,
                            
                          }).FirstOrDefault();
            return record;

        }

        public void Insert(LaboratoryBusiness.POCO.Admin.Consultant Consultant)
        {
            Tbl_Consultant con = new Tbl_Consultant()
            {
                // ConsultantID = p.ConsultantID,
                CreatedBy = Consultant.CreatedBy,
                CreatedDate = Consultant.CreatedDate,
                SpecialisationAreas = Consultant.SpecialisationAreas,
                UpdatedBy = Consultant.UpdatedBy,
                UpdatedDate = Consultant.UpdatedDate,
                //  ConsultantID=Consultant.ConsultantID
            };
            _context.Tbl_Consultant.Add(con);

            _consultant_entity = con;
            _consultant_poco = Consultant;
        }

        public void Update(LaboratoryBusiness.POCO.Admin.Consultant Consultant)
        {
            var record = _context.Tbl_Consultant.Where(x => x.ConsultantID == Consultant.ConsultantID).SingleOrDefault();
            if (record != null)
            {
                record.CreatedBy = Consultant.CreatedBy;
                record.CreatedDate = Consultant.CreatedDate;
                record.SpecialisationAreas = Consultant.SpecialisationAreas;
                record.UpdatedBy = Consultant.UpdatedBy;
                record.UpdatedDate = Consultant.UpdatedDate;
            }
            else
            {
                throw new Exception("Record not found");
            }
        }

        public void Delete(int ConsultantID)
        {
            var record = _context.Tbl_Consultant.Where(x => x.ConsultantID == ConsultantID).SingleOrDefault();
            _context.Tbl_Consultant.Remove(record);
        }

        public void Save()
        {
            _context.SaveChanges();
            _consultant_poco.ConsultantID = _consultant_entity.ConsultantID;
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
