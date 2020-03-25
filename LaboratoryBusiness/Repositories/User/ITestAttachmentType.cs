using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.Repositories.User
{
    public interface ITestAttachmentTypeRepository
    {
        IEnumerable<LaboratoryBusiness.POCO.User.Cl_TestAttachmentType> GetAll();
        LaboratoryBusiness.POCO.User.Cl_TestAttachmentType GetByID(int ClientUserTypeID);
        void Insert(LaboratoryBusiness.POCO.User.Cl_TestAttachmentType clientusertype);
        void Update(LaboratoryBusiness.POCO.User.Cl_TestAttachmentType clientusertype);
        void Delete(int ClientUserTypeID);
        void Save();
    }
}
