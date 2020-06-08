using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.Repositories.User
{
   public  interface IInvoiceRepository
    {
        IEnumerable<LaboratoryBusiness.POCO.User.Invoice> GetAll();
        LaboratoryBusiness.POCO.User.Invoice GetByID(int ClientUserTypeID);
        void Insert(LaboratoryBusiness.POCO.User.Invoice clientusertype);
        void Update(LaboratoryBusiness.POCO.User.Invoice clientusertype);
        void Delete(int ClientUserTypeID);
        void Save();
    }
}
