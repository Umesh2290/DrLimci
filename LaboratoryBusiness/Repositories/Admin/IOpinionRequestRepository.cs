using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.Repositories.Admin
{
    public interface IOpinionRequestRepository
    {
        IEnumerable<LaboratoryBusiness.POCO.Admin.OpinionRequest> GetAll();
        LaboratoryBusiness.POCO.Admin.OpinionRequest GetByID(int OpinionRequestID);
        void Insert(LaboratoryBusiness.POCO.Admin.OpinionRequest opinionrequest);
        void Update(LaboratoryBusiness.POCO.Admin.OpinionRequest opinionrequest);
        void Delete(int OpinionRequestID);
        void Save();
    }
}
