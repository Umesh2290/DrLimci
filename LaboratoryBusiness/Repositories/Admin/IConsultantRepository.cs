using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.Repositories.Admin
{
    public interface IConsultantRepository
    {
        IEnumerable<LaboratoryBusiness.POCO.Admin.Consultant> GetAll();
        LaboratoryBusiness.POCO.Admin.Consultant GetByID(int ConsultantID);
        void Insert(LaboratoryBusiness.POCO.Admin.Consultant menuassignment);
        void Update(LaboratoryBusiness.POCO.Admin.Consultant menuassignment);
        void Delete(int ConsultantID);
        void Save();
    }
}
