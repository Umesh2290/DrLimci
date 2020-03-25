using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.Repositories.Admin
{
    public interface IProviderParameterMappingRepository
    {
        IEnumerable<LaboratoryBusiness.POCO.Admin.ProviderParameterMapping> GetAll();
        LaboratoryBusiness.POCO.Admin.ProviderParameterMapping GetByID(int ProviderParameterMappingID);
        void Insert(LaboratoryBusiness.POCO.Admin.ProviderParameterMapping notification);
        void Update(LaboratoryBusiness.POCO.Admin.ProviderParameterMapping notification);
        void Delete(int ProviderParameterMappingID);
        void Save();
    }
}
