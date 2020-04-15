using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.Repositories.User
{
    public interface ILabReportConfiguration
    {
        IEnumerable<LaboratoryBusiness.POCO.User.Cl_LabReportConfiguration> GetAll();
        void Insert(LaboratoryBusiness.POCO.User.Cl_LabReportConfiguration clientusertype);
        void Update(LaboratoryBusiness.POCO.User.Cl_LabReportConfiguration clientusertype);
        void UpdateImage(LaboratoryBusiness.POCO.User.Cl_LabReportConfiguration clientusertype);

        void Save();
    }
}
