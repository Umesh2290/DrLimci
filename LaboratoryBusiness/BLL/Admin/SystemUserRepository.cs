using LaboratoryBusiness.DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.BLL.Admin
{
    public class SystemUserRepository : LaboratoryBusiness.Repositories.Admin.ISystemUserRepository
    {
        private readonly LabSystemDBEntities _context;
        private Tbl_SystemUser _systemuser_entity = new Tbl_SystemUser();
        private LaboratoryBusiness.POCO.Admin.SystemUser _systemuser_poco = new POCO.Admin.SystemUser();

        public SystemUserRepository()
        {
            _context = new LabSystemDBEntities();
        }
        public SystemUserRepository(LabSystemDBEntities context)
        {
            _context = context;
        }

        public IEnumerable<LaboratoryBusiness.POCO.Admin.SystemUser> GetAll()
        {
            var records = (from p in _context.Tbl_SystemUser.AsEnumerable()
                           select new LaboratoryBusiness.POCO.Admin.SystemUser
                           {
                               SystemUserID = p.SystemUserID,
                               Username = p.Username,
                               Password = p.Password,
                               Address = p.Address,
                               CreatedBy = p.CreatedBy,
                               CreatedDate = p.CreatedDate,
                               DetailID = p.DetailID,
                               DetailType = p.DetailType,
                               Email = p.Email,
                               FirstName = p.FirstName,
                               IsActive = p.IsActive,
                               LastName = p.LastName,
                               MiddleName = p.MiddleName,
                               MobileNo = p.MobileNo,
                               ProfilePic = p.ProfilePic,
                               UpdatedBy = p.UpdatedBy,
                               UpdatedDate = p.UpdatedDate,
                               IsBlock = p.IsBlock

                           });
            return records;

        }

        public LaboratoryBusiness.POCO.Admin.SystemUser GetByID(int SystemUserID)
        {
            var record = (from p in _context.Tbl_SystemUser.AsEnumerable()
                          where p.SystemUserID == SystemUserID
                          select new LaboratoryBusiness.POCO.Admin.SystemUser
                          {
                              SystemUserID = p.SystemUserID,
                              Username = p.Username,
                              Password = p.Password,
                              Address = p.Address,
                              CreatedBy = p.CreatedBy,
                              CreatedDate = p.CreatedDate,
                              DetailID = p.DetailID,
                              DetailType = p.DetailType,
                              Email = p.Email,
                              FirstName = p.FirstName,
                              IsActive = p.IsActive,
                              LastName = p.LastName,
                              MiddleName = p.MiddleName,
                              MobileNo = p.MobileNo,
                              ProfilePic = p.ProfilePic,
                              UpdatedBy = p.UpdatedBy,
                              UpdatedDate = p.UpdatedDate,
                              IsBlock = p.IsBlock
                          }).FirstOrDefault();

            return record;

        }

        public void Insert(LaboratoryBusiness.POCO.Admin.SystemUser systemuser)
        {
            Tbl_SystemUser sys_user = new Tbl_SystemUser()
            {
                //SystemUserID = p.SystemUserID,
                Username = systemuser.Username,
                Password = systemuser.Password,
                Address = systemuser.Address,
                CreatedBy = systemuser.CreatedBy,
                CreatedDate = systemuser.CreatedDate,
                DetailID = systemuser.DetailID,
                DetailType = systemuser.DetailType,
                Email = systemuser.Email,
                FirstName = systemuser.FirstName,
                IsActive = systemuser.IsActive,
                LastName = systemuser.LastName,
                MiddleName = systemuser.MiddleName,
                MobileNo = systemuser.MobileNo,
                ProfilePic = systemuser.ProfilePic,
                UpdatedBy = systemuser.UpdatedBy,
                UpdatedDate = systemuser.UpdatedDate,
                IsBlock = systemuser.IsBlock

            };
            _context.Tbl_SystemUser.Add(sys_user);

            _systemuser_entity = sys_user;
            _systemuser_poco = systemuser;
        }

        public void Update(LaboratoryBusiness.POCO.Admin.SystemUser systemuser)
        {
            var record = _context.Tbl_SystemUser.Where(x => x.SystemUserID == systemuser.SystemUserID).SingleOrDefault();
            if (record != null)
            {
                record.Username = systemuser.Username;
                record.Password = systemuser.Password;
                record.Address = systemuser.Address;
                record.CreatedBy = systemuser.CreatedBy;
                record.CreatedDate = systemuser.CreatedDate;
                record.DetailID = systemuser.DetailID;
                record.DetailType = systemuser.DetailType;
                record.Email = systemuser.Email;
                record.FirstName = systemuser.FirstName;
                record.IsActive = systemuser.IsActive;
                record.LastName = systemuser.LastName;
                record.MiddleName = systemuser.MiddleName;
                record.MobileNo = systemuser.MobileNo;
                record.ProfilePic = systemuser.ProfilePic;
                record.UpdatedBy = systemuser.UpdatedBy;
                record.UpdatedDate = systemuser.UpdatedDate;
                record.IsBlock = systemuser.IsBlock;

            }
            else
            {
                throw new Exception("Record not found");
            }
        }

        public void Delete(int SystemUserID)
        {
            var record = _context.Tbl_SystemUser.Where(x => x.SystemUserID == SystemUserID).SingleOrDefault();
            _context.Tbl_SystemUser.Remove(record);
        }

        public bool Unblock(int SystemUserID, bool isAutoUnblock, bool isclientadmin = false)
        {
            Repositories.Admin.IIncorrectPasswordAttemptRepository incorrectpassword = new BLL.Admin.IncorrectPasswordAttemptRepository();

            int blockafternumofattemts = 3;//Change to 10 after testing
            int afterhourstounblock = -1;//Change to -12 after testing

            var attempts = incorrectpassword.GetAll().Where(x => x.SystemUserID == SystemUserID //&&
    //(x.CreatedDate.Value.CompareTo(DateTime.Now.AddHours(afterhourstounblock)) == 0 || x.CreatedDate.Value.CompareTo(DateTime.Now.AddHours(afterhourstounblock)) == 1)
    && x.IsInclude == true).OrderByDescending(x=>x.CreatedDate).ToList();

            if (isAutoUnblock)
            {
                if (attempts.Count > 0 || isclientadmin)
                {
                    DateTime lastblockattemp = DateTime.Now;
                    if (!isclientadmin)
                    {
                        lastblockattemp = attempts[0].CreatedDate.Value;
                    }
                    if (((DateTime.Now - lastblockattemp).TotalMinutes > ((afterhourstounblock*-1) * 60)) || isclientadmin)
                    {
                        Repositories.Admin.ISystemUserRepository systemuser = new BLL.Admin.SystemUserRepository();

                        var systemuserobj = systemuser.GetByID(SystemUserID);
                        systemuserobj.IsBlock = false;

                        systemuser.Update(systemuserobj);
                        systemuser.Save();

                        foreach (var attempt in attempts)
                        {
                            attempt.IsInclude = false;
                            incorrectpassword.Update(attempt);

                        }

                        incorrectpassword.Save();

                        return true;

                    }

                    return false;
                }

                return true;
            }

            else
            {
                if (attempts.Count > 0 || isclientadmin)
                {
                        Repositories.Admin.ISystemUserRepository systemuser = new BLL.Admin.SystemUserRepository();

                        var systemuserobj = systemuser.GetByID(SystemUserID);
                        systemuserobj.IsBlock = false;

                        systemuser.Update(systemuserobj);
                        systemuser.Save();

                        foreach (var attempt in attempts)
                        {
                            attempt.IsInclude = false;
                            incorrectpassword.Update(attempt);

                        }

                        incorrectpassword.Save();

                        return true;

                }

                return true;
            }
        }

        public void Save()
        {
            _context.SaveChanges();
            _systemuser_poco.SystemUserID = _systemuser_entity.SystemUserID;
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
