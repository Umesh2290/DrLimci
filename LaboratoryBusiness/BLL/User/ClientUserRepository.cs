using LaboratoryBusiness.DAL.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryBusiness.BLL.User
{
    public class ClientUserRepository : LaboratoryBusiness.Repositories.User.IClientUserRepository
    {
        private readonly LabSystemClient_RajLabEntities _context;
        private Tbl_Cl_ClientUser _clientuser_entity = new Tbl_Cl_ClientUser();
        private LaboratoryBusiness.POCO.User.Cl_ClientUser _clientuser_poco = new POCO.User.Cl_ClientUser();

        public ClientUserRepository()
        {
            _context = new LabSystemClient_RajLabEntities();
        }
        public ClientUserRepository(LabSystemClient_RajLabEntities context)
        {
            _context = context;
        }

        public IEnumerable<LaboratoryBusiness.POCO.User.Cl_ClientUser> GetAll()
        {
            var records = (from p in _context.Tbl_Cl_ClientUser.AsEnumerable()
                           select new LaboratoryBusiness.POCO.User.Cl_ClientUser
                           {
                               ClientUserID = p.ClientUserID,
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
                               MobileNo = p.MobileNo,
                               ProfilePic = p.ProfilePic,
                               UpdatedBy = p.UpdatedBy,
                               UpdatedDate = p.UpdatedDate,
                               IsBlock = p.IsBlock,
                               HospitalDetailID=p.HospitalDetailID

                           });
            return records;

        }

        public LaboratoryBusiness.POCO.User.Cl_ClientUser GetByID(int ClientUserID)
        {
            var record = (from p in _context.Tbl_Cl_ClientUser.AsEnumerable()
                          where p.ClientUserID == ClientUserID
                          select new LaboratoryBusiness.POCO.User.Cl_ClientUser
                          {
                              ClientUserID = p.ClientUserID,
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
                              MobileNo = p.MobileNo,
                              ProfilePic = p.ProfilePic,
                              UpdatedBy = p.UpdatedBy,
                              UpdatedDate = p.UpdatedDate,
                              IsBlock = p.IsBlock,
                              HospitalDetailID=p.HospitalDetailID
                          }).FirstOrDefault();

            return record;

        }

        public void Insert(LaboratoryBusiness.POCO.User.Cl_ClientUser clientuser)
        {
            Tbl_Cl_ClientUser cl_user = new Tbl_Cl_ClientUser()
            {
                //SystemUserID = p.SystemUserID,
                Username = clientuser.Username,
                Password = clientuser.Password,
                Address = clientuser.Address,
                CreatedBy = clientuser.CreatedBy,
                CreatedDate = clientuser.CreatedDate,
                DetailID = clientuser.DetailID,
                DetailType = clientuser.DetailType,
                Email = clientuser.Email,
                FirstName = clientuser.FirstName,
                IsActive = clientuser.IsActive,
                LastName = clientuser.LastName,
                MobileNo = clientuser.MobileNo,
                ProfilePic = clientuser.ProfilePic,
                UpdatedBy = clientuser.UpdatedBy,
                UpdatedDate = clientuser.UpdatedDate,
                IsBlock = clientuser.IsBlock,
                HospitalDetailID=clientuser.HospitalDetailID

            };
            _context.Tbl_Cl_ClientUser.Add(cl_user);

            _clientuser_entity = cl_user;
            _clientuser_poco = clientuser;
        }

        public void Update(LaboratoryBusiness.POCO.User.Cl_ClientUser clientuser)
        {
            var record = _context.Tbl_Cl_ClientUser.Where(x => x.ClientUserID == clientuser.ClientUserID).SingleOrDefault();
            if (record != null)
            {
                record.Username = clientuser.Username;
                record.Password = clientuser.Password;
                record.Address = clientuser.Address;
                record.CreatedBy = clientuser.CreatedBy;
                record.CreatedDate = clientuser.CreatedDate;
                record.DetailID = clientuser.DetailID;
                record.DetailType = clientuser.DetailType;
                record.Email = clientuser.Email;
                record.FirstName = clientuser.FirstName;
                record.IsActive = clientuser.IsActive;
                record.LastName = clientuser.LastName;
                record.MobileNo = clientuser.MobileNo;
                record.ProfilePic = clientuser.ProfilePic;
                record.UpdatedBy = clientuser.UpdatedBy;
                record.UpdatedDate = clientuser.UpdatedDate;
                record.IsBlock = clientuser.IsBlock;
                record.HospitalDetailID = clientuser.HospitalDetailID;

            }
            else
            {
                throw new Exception("Record not found");
            }
        }

        public void Delete(int ClientUserID)
        {
            var record = _context.Tbl_Cl_ClientUser.Where(x => x.ClientUserID == ClientUserID).SingleOrDefault();
            _context.Tbl_Cl_ClientUser.Remove(record);
        }

        public bool Unblock(int ClientUserID, bool isAutoUnblock,Repositories.User.IIncorrectPasswordAttemptRepository increpo,Repositories.User.IClientUserRepository clurepo
            , Repositories.User.IClientUserTypeRepository cltyperepo, Repositories.User.IAdminDetailRepository addetailrepo)
        {
            Repositories.User.IIncorrectPasswordAttemptRepository incorrectpassword = increpo;

            int blockafternumofattemts = 3;//Change to 10 after testing
            int afterhourstounblock = -1;//Change to -12 after testing

            var attempts = incorrectpassword.GetAll().Where(x => x.ClientUserID == ClientUserID //&&
                //(x.CreatedDate.Value.CompareTo(DateTime.Now.AddHours(afterhourstounblock)) == 0 || x.CreatedDate.Value.CompareTo(DateTime.Now.AddHours(afterhourstounblock)) == 1)
    && x.IsInclude == true).OrderByDescending(x => x.CreatedDate).ToList();

            int clientdetailtypeid = cltyperepo.GetAll().Where(x => x.TypeName.Equals("Client Admin")).FirstOrDefault().ClientUserTypeID;

            if (isAutoUnblock)
            {
                if (attempts.Count > 0)
                {
                    DateTime lastblockattemp = attempts[0].CreatedDate.Value;
                    if ((DateTime.Now - lastblockattemp).TotalMinutes > ((afterhourstounblock * -1) * 60))
                    {
                        Repositories.User.IClientUserRepository clientuser = clurepo;

                        var clientuserobj = clientuser.GetByID(ClientUserID);

                        if (clientuserobj.DetailType.Value == clientdetailtypeid)
                        {
                            //Update on System Admin Side as well
                            int systemclientid = addetailrepo.GetAll().Where(x => x.AdminDetailID == clientuserobj.DetailID.Value).FirstOrDefault().SystemClientID.Value;
                            Repositories.Admin.ISystemUserRepository sysrepo = new BLL.Admin.SystemUserRepository();
                            var sysclient =  sysrepo.GetByID(systemclientid);
                            sysclient.IsBlock = false;
                            sysrepo.Update(sysclient);
                            sysrepo.Save();
                        }

                        clientuserobj.IsBlock = false;

                        clientuser.Update(clientuserobj);
                        clientuser.Save();

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
                if (attempts.Count > 0)
                {
                    Repositories.User.IClientUserRepository clientuser = clurepo;

                    var clientuserobj = clientuser.GetByID(ClientUserID);

                    if (clientuserobj.DetailType.Value == clientdetailtypeid)
                    {
                        //Update on System Admin Side as well
                        int systemclientid = addetailrepo.GetAll().Where(x => x.AdminDetailID == clientuserobj.DetailID.Value).FirstOrDefault().SystemClientID.Value;
                        Repositories.Admin.ISystemUserRepository sysrepo = new BLL.Admin.SystemUserRepository();
                        var sysclient = sysrepo.GetByID(systemclientid);
                        sysclient.IsBlock = false;
                        sysrepo.Update(sysclient);
                        sysrepo.Save();
                        //Commented because does not this code here it will already unblock the client from system admin side before calling this method.
                    }

                    clientuserobj.IsBlock = false;

                    clientuser.Update(clientuserobj);
                    clientuser.Save();

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
            _clientuser_poco.ClientUserID = _clientuser_entity.ClientUserID;
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
