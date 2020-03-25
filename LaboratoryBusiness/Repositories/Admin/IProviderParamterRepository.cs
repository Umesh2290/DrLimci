using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.Repositories.Admin
{
    public interface IProviderParamterRepository
    {
        IEnumerable<LaboratoryBusiness.POCO.Admin.ProviderParamter> GetAll();
        LaboratoryBusiness.POCO.Admin.ProviderParamter GetByID(int ProviderParamterID);
        void Insert(LaboratoryBusiness.POCO.Admin.ProviderParamter providerparamter);
        void Update(LaboratoryBusiness.POCO.Admin.ProviderParamter providerparamter);
        void Delete(int ProviderParamter);
        void Save();
    }
}
