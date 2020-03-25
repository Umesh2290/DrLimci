using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.Repositories.Admin
{
    public interface IMenuRepository
    {
        IEnumerable<LaboratoryBusiness.POCO.Admin.Menu> GetAll();
        LaboratoryBusiness.POCO.Admin.Menu GetByID(int MenuID);
        IEnumerable<LaboratoryBusiness.POCO.Admin.Menu> GetByLink(string link);
        void Insert(LaboratoryBusiness.POCO.Admin.Menu menu);
        void Update(LaboratoryBusiness.POCO.Admin.Menu menu);
        void Delete(int MenuID);
        void Save();

    }
}
