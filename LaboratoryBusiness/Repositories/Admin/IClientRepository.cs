using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.Repositories.Admin
{
    public interface IClientRepository
    {
        IEnumerable<LaboratoryBusiness.POCO.Admin.Client> GetAll();
        LaboratoryBusiness.POCO.Admin.Client GetByID(int ClientID);
        void Insert(LaboratoryBusiness.POCO.Admin.Client menuassignment);
        void Update(LaboratoryBusiness.POCO.Admin.Client menuassignment);
        void Delete(int ClientID);
        void Save();
    }
}
