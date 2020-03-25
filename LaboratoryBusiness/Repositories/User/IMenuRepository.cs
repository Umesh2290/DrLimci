using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.Repositories.User
{
    public interface IMenuRepository
    {
        IEnumerable<LaboratoryBusiness.POCO.User.Cl_Menu> GetAll();
        LaboratoryBusiness.POCO.User.Cl_Menu GetByID(int MenuID);
        IEnumerable<LaboratoryBusiness.POCO.User.Cl_Menu> GetByLink(string link);
        void Insert(LaboratoryBusiness.POCO.User.Cl_Menu menu);
        void Update(LaboratoryBusiness.POCO.User.Cl_Menu menu);
        void Delete(int MenuID);
        void Save();

    }
}
