using LaboratoryBusiness.DAL.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.BLL.User
{
    public class InventoryRequestRepository : LaboratoryBusiness.Repositories.User.IInventoryRequestRepository
    {
        private readonly LabSystemClient_RajLabEntities _context;
        private Tbl_Cl_InventoryRequest InventoryRequest_entity = new Tbl_Cl_InventoryRequest();
        private LaboratoryBusiness.POCO.User.Cl_InventoryRequest InventoryRequest_poco = new POCO.User.Cl_InventoryRequest();

        public InventoryRequestRepository()
        {
            _context = new LabSystemClient_RajLabEntities();
        }
        public InventoryRequestRepository(LabSystemClient_RajLabEntities context)
        {
            _context = context;
        }

        public IEnumerable<LaboratoryBusiness.POCO.User.Cl_InventoryRequest> GetAll()
        {

            var records = (from p in _context.Tbl_Cl_InventoryRequest.AsEnumerable()
                           select new LaboratoryBusiness.POCO.User.Cl_InventoryRequest
                           {
                               Comments=p.Comments,
                               Description=p.Description,
                               InventoryRequestID=p.InventoryRequestID,
                               ItemName=p.ItemName,
                               OpenActionBy=p.OpenActionBy,
                               OpenActionComments=p.OpenActionComments,
                               OpenActionDate=p.OpenActionDate,
                               OpenActionStatusID=p.OpenActionStatusID,
                               ProgressActionBy=p.ProgressActionBy,
                               ProgressActionComments=p.ProgressActionComments,
                               ProgressActionDate=p.ProgressActionDate,
                               Quantity=p.Quantity,
                               RequestCreatedBy=p.RequestCreatedBy,
                               RequestCreatedDate=p.RequestCreatedDate,
                               StatusID=p.StatusID



                           });
            return records;

        }

        public LaboratoryBusiness.POCO.User.Cl_InventoryRequest GetByID(int InventoryRequestID)
        {
            var record = (from p in _context.Tbl_Cl_InventoryRequest.AsEnumerable()
                          where p.InventoryRequestID == InventoryRequestID
                          select new LaboratoryBusiness.POCO.User.Cl_InventoryRequest
                          {

                              Comments = p.Comments,
                              Description = p.Description,
                              InventoryRequestID = p.InventoryRequestID,
                              ItemName = p.ItemName,
                              OpenActionBy = p.OpenActionBy,
                              OpenActionComments = p.OpenActionComments,
                              OpenActionDate = p.OpenActionDate,
                              OpenActionStatusID = p.OpenActionStatusID,
                              ProgressActionBy = p.ProgressActionBy,
                              ProgressActionComments = p.ProgressActionComments,
                              ProgressActionDate = p.ProgressActionDate,
                              Quantity = p.Quantity,
                              RequestCreatedBy = p.RequestCreatedBy,
                              RequestCreatedDate = p.RequestCreatedDate,
                              StatusID = p.StatusID


                          }).FirstOrDefault();
            return record;

        }

        public void Insert(LaboratoryBusiness.POCO.User.Cl_InventoryRequest p)
        {
            Tbl_Cl_InventoryRequest inp = new Tbl_Cl_InventoryRequest()
            {

                Comments = p.Comments,
                Description = p.Description,
                InventoryRequestID = p.InventoryRequestID,
                ItemName = p.ItemName,
                OpenActionBy = p.OpenActionBy,
                OpenActionComments = p.OpenActionComments,
                OpenActionDate = p.OpenActionDate,
                OpenActionStatusID = p.OpenActionStatusID,
                ProgressActionBy = p.ProgressActionBy,
                ProgressActionComments = p.ProgressActionComments,
                ProgressActionDate = p.ProgressActionDate,
                Quantity = p.Quantity,
                RequestCreatedBy = p.RequestCreatedBy,
                RequestCreatedDate = p.RequestCreatedDate,
                StatusID = p.StatusID

            };
            _context.Tbl_Cl_InventoryRequest.Add(inp);

            InventoryRequest_entity = inp;
            InventoryRequest_poco = p;

        }

        public void Update(LaboratoryBusiness.POCO.User.Cl_InventoryRequest p)
        {
            var record = _context.Tbl_Cl_InventoryRequest.Where(x => x.InventoryRequestID == p.InventoryRequestID).SingleOrDefault();
            if (record != null)
            {
                   record.Comments = p.Comments;
                              record.Description = p.Description;
                              record.InventoryRequestID = p.InventoryRequestID;
                              record.ItemName = p.ItemName;
                              record.OpenActionBy = p.OpenActionBy;
                             record.OpenActionComments = p.OpenActionComments;
                              record.OpenActionDate = p.OpenActionDate;
                              record.OpenActionStatusID = p.OpenActionStatusID;
                              record.ProgressActionBy = p.ProgressActionBy;
                              record.ProgressActionComments = p.ProgressActionComments;
                              record.ProgressActionDate = p.ProgressActionDate;
                              record.Quantity = p.Quantity;
                              record.RequestCreatedBy = p.RequestCreatedBy;
                              record.RequestCreatedDate = p.RequestCreatedDate;
                              record.StatusID = p.StatusID;
            
            }
            else
            {
                throw new Exception("Record not found");
            }
        }

        public void Delete(int InventoryRequestID)
        {
            var record = _context.Tbl_Cl_InventoryRequest.Where(x => x.InventoryRequestID == InventoryRequestID).SingleOrDefault();
            _context.Tbl_Cl_InventoryRequest.Remove(record);
        }

        public void Save()
        {
            _context.SaveChanges();
            InventoryRequest_poco.InventoryRequestID = InventoryRequest_entity.InventoryRequestID;
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
