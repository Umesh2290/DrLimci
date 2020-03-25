using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.Repositories.Admin
{
    public interface IStorageandDBProviderRepository
    {
        IEnumerable<LaboratoryBusiness.POCO.Admin.StorageandDBProvider> GetAll();
        LaboratoryBusiness.POCO.Admin.StorageandDBProvider GetByID(int StorageandDBProvideID);
        void Insert(LaboratoryBusiness.POCO.Admin.StorageandDBProvider storageanddbprovider);
        void Update(LaboratoryBusiness.POCO.Admin.StorageandDBProvider storageanddbprovider);
        void Delete(int StorageandDBProvideID);
        void Save();
    }
}
