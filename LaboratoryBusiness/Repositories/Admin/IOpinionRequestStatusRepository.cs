using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.Repositories.Admin
{
    public interface IOpinionRequestStatusRepository
    {
        IEnumerable<LaboratoryBusiness.POCO.Admin.OpinionRequestStatus> GetAll();
        LaboratoryBusiness.POCO.Admin.OpinionRequestStatus GetByID(int OpinionRequestStatusID);
        void Insert(LaboratoryBusiness.POCO.Admin.OpinionRequestStatus opinionrequeststatus);
        void Update(LaboratoryBusiness.POCO.Admin.OpinionRequestStatus opinionrequeststatus);
        void Delete(int OpinionRequestStatusID);
        void Save();
    }
}
