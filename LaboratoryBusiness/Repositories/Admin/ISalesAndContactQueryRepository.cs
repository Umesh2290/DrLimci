using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.Repositories.Admin
{
    public interface ISalesAndContactQueryRepository
    {
        IEnumerable<LaboratoryBusiness.POCO.Admin.SalesAndContactQuery> GetAll();
        LaboratoryBusiness.POCO.Admin.SalesAndContactQuery GetByID(int SalesAndContactQueryID);
        void Insert(LaboratoryBusiness.POCO.Admin.SalesAndContactQuery role);
        void Update(LaboratoryBusiness.POCO.Admin.SalesAndContactQuery role);
        void Delete(int SalesAndContactQueryID);
        void Save();
    }
}
