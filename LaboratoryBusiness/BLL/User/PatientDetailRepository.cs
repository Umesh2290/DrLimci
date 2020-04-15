using LaboratoryBusiness.DAL.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.BLL.User
{
    public class PatientDetailRepository : LaboratoryBusiness.Repositories.User.IPatientDetailRepository
    {
        private readonly LabSystemClient_RajLabEntities _context;
        private Tbl_Cl_PatientDetail patient_entity = new Tbl_Cl_PatientDetail();
        private LaboratoryBusiness.POCO.User.Cl_PatientDetail patient_poco = new POCO.User.Cl_PatientDetail();

        public PatientDetailRepository()
        {
            _context = new LabSystemClient_RajLabEntities();
        }
        public PatientDetailRepository(LabSystemClient_RajLabEntities context)
        {
            _context = context;
        }

        public IEnumerable<LaboratoryBusiness.POCO.User.Cl_PatientDetail> GetAll()
        {

            var records = (from p in _context.Tbl_Cl_PatientDetail.AsEnumerable()
                           select new LaboratoryBusiness.POCO.User.Cl_PatientDetail
                           {

                            Age=p.Age,
                            AlternateAddress=p.AlternateAddress,
                            AlternatePhoneNo=p.AlternatePhoneNo,
                            City=p.City,
                            CreatedBy=p.CreatedBy,
                            CreatedDate=p.CreatedDate,
                            MiddleName=p.MiddleName,
                            PatientDetailID=p.PatientDetailID,
                            Payment=p.Payment,
                            PaymentMode=p.PaymentMode,
                            PaymentReceiptPdfLink=p.PaymentReceiptPdfLink,
                            PdfLink=p.PdfLink,
                            ReferingDoctor=p.ReferingDoctor,
                            ReferingHospital=p.ReferingHospital,
                            Sex=p.Sex,
                            Streetname=p.Streetname,
                            UpdatedBy=p.UpdatedBy,
                            UpdatedDate=p.UpdatedDate,
                            HospitalID=p.HospitalID




                           });
            return records;

        }

        public LaboratoryBusiness.POCO.User.Cl_PatientDetail GetByID(int PatientDetailID)
        {
            var record = (from p in _context.Tbl_Cl_PatientDetail.AsEnumerable()
                          where p.PatientDetailID == PatientDetailID
                          select new LaboratoryBusiness.POCO.User.Cl_PatientDetail
                          {


                         Age=p.Age,
                            AlternateAddress=p.AlternateAddress,
                            AlternatePhoneNo=p.AlternatePhoneNo,
                            City=p.City,
                            CreatedBy=p.CreatedBy,
                            CreatedDate=p.CreatedDate,
                            MiddleName=p.MiddleName,
                            PatientDetailID=p.PatientDetailID,
                            Payment=p.Payment,
                            PaymentMode=p.PaymentMode,
                            PaymentReceiptPdfLink=p.PaymentReceiptPdfLink,
                            PdfLink=p.PdfLink,
                            ReferingDoctor=p.ReferingDoctor,
                            ReferingHospital=p.ReferingHospital,
                            Sex=p.Sex,
                            Streetname=p.Streetname,
                            UpdatedBy=p.UpdatedBy,
                            UpdatedDate=p.UpdatedDate,
                            HospitalID = p.HospitalID


                          }).FirstOrDefault();
            return record;

        }

        public void Insert(LaboratoryBusiness.POCO.User.Cl_PatientDetail p)
        {
            Tbl_Cl_PatientDetail inp = new Tbl_Cl_PatientDetail()
            {
                Age=p.Age,
                            AlternateAddress=p.AlternateAddress,
                            AlternatePhoneNo=p.AlternatePhoneNo,
                            City=p.City,
                            CreatedBy=p.CreatedBy,
                            CreatedDate=p.CreatedDate,
                            MiddleName=p.MiddleName,
                            PatientDetailID=p.PatientDetailID,
                            Payment=p.Payment,
                            PaymentMode=p.PaymentMode,
                            PaymentReceiptPdfLink=p.PaymentReceiptPdfLink,
                            PdfLink=p.PdfLink,
                            ReferingDoctor=p.ReferingDoctor,
                            ReferingHospital=p.ReferingHospital,
                            Sex=p.Sex,
                            Streetname=p.Streetname,
                            UpdatedBy=p.UpdatedBy,
                            UpdatedDate=p.UpdatedDate,
                            HospitalID = p.HospitalID

            };
            _context.Tbl_Cl_PatientDetail.Add(inp);

            patient_entity = inp;
            patient_poco = p;

        }

        public void Update(LaboratoryBusiness.POCO.User.Cl_PatientDetail p)
        {
            var record = _context.Tbl_Cl_PatientDetail.Where(x => x.PatientDetailID == p.PatientDetailID).SingleOrDefault();
            if (record != null)
            {      record.Age=p.Age;
                            record.AlternateAddress=p.AlternateAddress;
                            record.AlternatePhoneNo=p.AlternatePhoneNo;
                            record.City=p.City;
                            record.CreatedBy=p.CreatedBy;
                            record.CreatedDate = p.CreatedDate;
                            record.MiddleName=p.MiddleName;
                            record.PatientDetailID=p.PatientDetailID;
                            record.Payment=p.Payment;
                            record.PaymentMode=p.PaymentMode;
                            record.PaymentReceiptPdfLink=p.PaymentReceiptPdfLink;
                            record.PdfLink=p.PdfLink;
                            record.ReferingDoctor=p.ReferingDoctor;
                            record.ReferingHospital=p.ReferingHospital;
                            record.Sex=p.Sex;
                            record.Streetname=p.Streetname;
                            record.UpdatedBy=p.UpdatedBy;
                            record.UpdatedDate=p.UpdatedDate;
                            record.HospitalID = p.HospitalID;

            }
            else
            {
                throw new Exception("Record not found");
            }
        }

        public void Delete(int PatientDetailID)
        {
            var record = _context.Tbl_Cl_PatientDetail.Where(x => x.PatientDetailID == PatientDetailID).SingleOrDefault();
            _context.Tbl_Cl_PatientDetail.Remove(record);
        }

        public void Save()
        {
            _context.SaveChanges();
            patient_poco.PatientDetailID = patient_entity.PatientDetailID;
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
