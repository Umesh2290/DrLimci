using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.Repositories.Admin
{
    public interface ISalesAndContactQueryStatusRepository
    {
        IEnumerable<LaboratoryBusiness.POCO.Admin.SalesAndContactQueryStatus> GetAll();
        LaboratoryBusiness.POCO.Admin.SalesAndContactQueryStatus GetByID(int SalesAndContactQueryStatusID);
        void Insert(LaboratoryBusiness.POCO.Admin.SalesAndContactQueryStatus role);
        void Update(LaboratoryBusiness.POCO.Admin.SalesAndContactQueryStatus role);
        void Delete(int SalesAndContactQueryStatus);
        void Save();
    }
}
