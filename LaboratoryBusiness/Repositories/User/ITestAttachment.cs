using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.Repositories.User
{
    public interface ITestAttachmentRepository
    {
        IEnumerable<LaboratoryBusiness.POCO.User.Cl_TestAttachment> GetAll();
        LaboratoryBusiness.POCO.User.Cl_TestAttachment GetByID(int ClientUserTypeID);
        void Insert(LaboratoryBusiness.POCO.User.Cl_TestAttachment clientusertype);
        void Update(LaboratoryBusiness.POCO.User.Cl_TestAttachment clientusertype);
        void Delete(int ClientUserTypeID);
        void Save();
    }
}
