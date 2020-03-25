using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.Repositories.User
{
    public interface IMenuAssignmentRepository
    {
        IEnumerable<LaboratoryBusiness.POCO.User.Cl_MenuAssignment> GetAll();
        LaboratoryBusiness.POCO.User.Cl_MenuAssignment GetByID(int MenuAssignmentID);
        void Insert(LaboratoryBusiness.POCO.User.Cl_MenuAssignment menuassignment);
        void Update(LaboratoryBusiness.POCO.User.Cl_MenuAssignment menuassignment);
        void Delete(int MenuAssignmentID);
        void Save();
    }
}
