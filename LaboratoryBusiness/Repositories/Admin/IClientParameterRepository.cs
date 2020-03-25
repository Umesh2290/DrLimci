using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.Repositories.Admin
{
    public interface IClientParameterRepository
    {
        IEnumerable<LaboratoryBusiness.POCO.Admin.ClientParameter> GetAll();
        LaboratoryBusiness.POCO.Admin.ClientParameter GetByID(int ClientParameterID);
        void Insert(LaboratoryBusiness.POCO.Admin.ClientParameter menuassignment);
        void Update(LaboratoryBusiness.POCO.Admin.ClientParameter menuassignment);
        void Delete(int ClientParameterID);
        void Save();
    }
}
