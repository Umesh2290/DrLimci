using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.Repositories.Admin
{
    public interface ISalesAndContactQueryTypeRepository
    {
        IEnumerable<LaboratoryBusiness.POCO.Admin.SalesAndContactQueryType> GetAll();
        LaboratoryBusiness.POCO.Admin.SalesAndContactQueryType GetByID(int SalesAndContactQueryTypeID);
        void Insert(LaboratoryBusiness.POCO.Admin.SalesAndContactQueryType role);
        void Update(LaboratoryBusiness.POCO.Admin.SalesAndContactQueryType role);
        void Delete(int SalesAndContactQueryType);
        void Save();
    }
}
