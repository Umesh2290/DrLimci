using LaboratoryBusiness.DAL.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.BLL.User
{

    public class ExtraWorkAttachmentRepository : LaboratoryBusiness.Repositories.User.IExtraWorkAttachmentRepository
    {
        private readonly LabSystemClient_RajLabEntities _context;
        private Tbl_Cl_ExtraWorkAttachment ExtraWorkAttachment_entity = new Tbl_Cl_ExtraWorkAttachment();
        private LaboratoryBusiness.POCO.User.Cl_ExtraWorkAttachment ExtraWorkAttachment_poco = new POCO.User.Cl_ExtraWorkAttachment();

        public ExtraWorkAttachmentRepository()
        {
            _context = new LabSystemClient_RajLabEntities();
        }
        public ExtraWorkAttachmentRepository(LabSystemClient_RajLabEntities context)
        {
            _context = context;
        }

        public IEnumerable<LaboratoryBusiness.POCO.User.Cl_ExtraWorkAttachment> GetAll()
        {

            var records = (from p in _context.Tbl_Cl_ExtraWorkAttachment.AsEnumerable()
                           select new LaboratoryBusiness.POCO.User.Cl_ExtraWorkAttachment
                           {
                            CreatedBy=p.CreatedBy,
                            CreatedDate=p.CreatedDate,
                            Extension=p.Extension,
                            ExtraWorkAttachmentID=p.ExtraWorkAttachmentID,
                            ExtraWorkID=p.ExtraWorkID,
                            Link=p.Link,
                            Name=p.Name
                        

                           });
            return records;

        }

        public LaboratoryBusiness.POCO.User.Cl_ExtraWorkAttachment GetByID(int ExtraWorkAttachmentID)
        {
            var record = (from p in _context.Tbl_Cl_ExtraWorkAttachment.AsEnumerable()
                          where p.ExtraWorkAttachmentID == ExtraWorkAttachmentID
                          select new LaboratoryBusiness.POCO.User.Cl_ExtraWorkAttachment
                          {
                              CreatedBy = p.CreatedBy,
                              CreatedDate = p.CreatedDate,
                              Extension = p.Extension,
                              ExtraWorkAttachmentID = p.ExtraWorkAttachmentID,
                              ExtraWorkID = p.ExtraWorkID,
                              Link = p.Link,
                              Name = p.Name
                        

                          }).FirstOrDefault();
            return record;

        }

        public void Insert(LaboratoryBusiness.POCO.User.Cl_ExtraWorkAttachment p)
        {
            Tbl_Cl_ExtraWorkAttachment curd = new Tbl_Cl_ExtraWorkAttachment()
            {

                CreatedBy = p.CreatedBy,
                CreatedDate = p.CreatedDate,
                Extension = p.Extension,
                ExtraWorkAttachmentID = p.ExtraWorkAttachmentID,
                ExtraWorkID = p.ExtraWorkID,
                Link = p.Link,
                Name = p.Name
            };
            _context.Tbl_Cl_ExtraWorkAttachment.Add(curd);

            ExtraWorkAttachment_entity = curd;
            ExtraWorkAttachment_poco = p;

        }

        public void Update(LaboratoryBusiness.POCO.User.Cl_ExtraWorkAttachment p)
        {
            var record = _context.Tbl_Cl_ExtraWorkAttachment.Where(x => x.ExtraWorkAttachmentID == p.ExtraWorkAttachmentID).SingleOrDefault();
            if (record != null)
            {
                record.CreatedBy = p.CreatedBy;
                record.CreatedDate = p.CreatedDate;
                record.Extension = p.Extension;
                record.ExtraWorkAttachmentID = p.ExtraWorkAttachmentID;
                record.ExtraWorkID = p.ExtraWorkID;
                record.Link = p.Link;
                record.Name = p.Name;
            }
            else
            {
                throw new Exception("Record not found");
            }
        }

        public void Delete(int ExtraWorkAttachmentID)
        {
            var record = _context.Tbl_Cl_ExtraWorkAttachment.Where(x => x.ExtraWorkAttachmentID == ExtraWorkAttachmentID).SingleOrDefault();
            _context.Tbl_Cl_ExtraWorkAttachment.Remove(record);
        }

        public void Save()
        {
            _context.SaveChanges();
            ExtraWorkAttachment_poco.ExtraWorkAttachmentID = ExtraWorkAttachment_entity.ExtraWorkAttachmentID;
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

