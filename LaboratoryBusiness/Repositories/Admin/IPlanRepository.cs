using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.Repositories.Admin
{
    public interface IPlanRepository
    {
        IEnumerable<LaboratoryBusiness.POCO.Admin.Plan> GetAll();
        LaboratoryBusiness.POCO.Admin.Plan GetByID(int PlanID);
        void Insert(LaboratoryBusiness.POCO.Admin.Plan notification);
        void Update(LaboratoryBusiness.POCO.Admin.Plan notification);
        void Delete(int PlanID);
        void Save();
    }
}
