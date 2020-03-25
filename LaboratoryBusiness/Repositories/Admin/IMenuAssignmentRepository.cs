using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.Repositories.Admin
{
    public interface IMenuAssignmentRepository
    {
        IEnumerable<LaboratoryBusiness.POCO.Admin.MenuAssignment> GetAll();
        LaboratoryBusiness.POCO.Admin.MenuAssignment GetByID(int MenuAssignmentID);
        void Insert(LaboratoryBusiness.POCO.Admin.MenuAssignment menuassignment);
        void Update(LaboratoryBusiness.POCO.Admin.MenuAssignment menuassignment);
        void Delete(int MenuAssignmentID);
        void Save();
    }
}
