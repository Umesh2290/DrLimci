using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Repositories = LaboratoryBusiness.Repositories;
using BLL = LaboratoryBusiness.BLL;
using BusinessPOCO = LaboratoryBusiness.POCO;
using System.Web.Configuration;

namespace LaboratorySystem
{
    public enum MenuPriorityEnum
    {
        Role=1,
        Employee = 2,
    }

    public enum DetailTypeEnum
    {
        Client = 1,
        Employee = 2,
        Consultant = 3,
    }
    public class SystemUser : LaboratoryBusiness.POCO.Admin.SystemUser
    {
        public List<Role> AssignedRoles
        {
            get;
            set;
        }

        public Role CurrentRole { get; set; }
        public List<BusinessPOCO.Admin.Menu> AssignedMenus { get; set; }

        public List<BusinessPOCO.Admin.SectionRestriction> SectionRestrictions { get; set; }

        public MenuPriorityEnum MenuPriority { get; set; }

        public DetailTypeEnum DetailTypeProperty { get; set; }

        public Client Client { get; set; }

        public BusinessPOCO.Admin.Consultant Consultant { get; set; }



        public static SystemUser Initialize(LaboratoryBusiness.POCO.Admin.SystemUser user)
        {
            try
            {
                #region Repositories Initialization
                Repositories.Admin.IRoleRepository userrole = new BLL.Admin.RoleRepository();
                Repositories.Admin.IRoleMappingRepository userrolemapping = new BLL.Admin.RoleMappingRepository();
                Repositories.Admin.IMenuRepository rolemenu = new BLL.Admin.MenuRepository();
                Repositories.Admin.IMenuAssignmentRepository rolemenumapping = new BLL.Admin.MenuAssignmentRepository();
                Repositories.Admin.ISectionRestrictionRepository sectionrestriction = new BLL.Admin.SectionRestrictionRepository();
                Repositories.Admin.IConsultantRepository consultant = new BLL.Admin.ConsultantRepository();
                #endregion

                #region Assigned Roles
                var _assignedroles = (from r in userrole.GetAll()
                                      join rm in userrolemapping.GetAll() on r.RoleID equals rm.RoleID
                                      where rm.UserID == user.SystemUserID
                                      select new { r,rm }).ToList();

                List<Role> roles = new List<Role>();
                List<LaboratoryBusiness.POCO.Admin.Menu> menus = new List<BusinessPOCO.Admin.Menu>();
                List<SectionRestriction> sectionrestrictions = new List<SectionRestriction>();

                foreach (var role in _assignedroles)
                {
                    var _assignedmenus = (from m in rolemenu.GetAll()
                                          join mm in rolemenumapping.GetAll() on m.MenuID equals mm.MenuID
                                          where mm.RoleID == role.r.RoleID
                                          select m).ToList();

                    menus = new List<BusinessPOCO.Admin.Menu>();
                    foreach (var menu in _assignedmenus)
                    {
                        menus.Add(new BusinessPOCO.Admin.Menu()
                        {
                            MenuID = menu.MenuID,
                            MenuName = menu.MenuName,
                            ParentID = menu.ParentID,
                            ToolTip = menu.ToolTip,
                            Link = menu.Link,
                            IsViewable = menu.IsViewable,
                            Icon = menu.Icon,
                            Description = menu.Description

                        });
                    }

                    var _sectionrestrictions = (from sr in sectionrestriction.GetAll()
                                                where sr.RoleID == role.r.RoleID
                                                select sr).ToList();

                    sectionrestrictions = new List<SectionRestriction>();

                    foreach (var section in _sectionrestrictions)
                    {
                        sectionrestrictions.Add(new SectionRestriction()
                        {
                            RoleID = section.RoleID,
                            SectionID = section.SectionID,
                            SectionSelector = section.SectionSelector,
                            MenuID = section.MenuID,
                            EmployeeID = section.EmployeeID,
                            Menu = rolemenu.GetByID(section.MenuID.Value)
                        });
                    }

                    roles.Add(new Role()
                    {
                        AssignedMenus = menus,
                        SectionRestrictions = sectionrestrictions,
                        Description = role.r.Description,
                        IsActive = role.r.IsActive,
                        RoleID = role.r.RoleID,
                        RoleName = role.r.RoleName,
                        IsDefault = role.rm.IsDefault
                    });
                }

                #endregion

                #region Assigned Individual Menus and SectionRestrictions
                var _userassignedmenus = (from m in rolemenu.GetAll()
                                      join mm in rolemenumapping.GetAll() on m.MenuID equals mm.MenuID
                                      where mm.EmployeeID==user.SystemUserID
                                      select m).ToList();

                var _usersectionrestrictions = (from sr in sectionrestriction.GetAll()
                                            where sr.EmployeeID == user.SystemUserID
                                            select sr).ToList();
                #endregion

                #region Current Role Assignment
                var _usercurrentrole = (from r in userrole.GetAll()
                                      join rm in userrolemapping.GetAll() on r.RoleID equals rm.RoleID
                                      where rm.UserID == user.SystemUserID && rm.IsDefault.HasValue?rm.IsDefault == true:false
                                      select new { r, rm }).ToList().FirstOrDefault();

                var _assignedmenuscurrentrole = (from m in rolemenu.GetAll()
                                      join mm in rolemenumapping.GetAll() on m.MenuID equals mm.MenuID
                                                 where mm.RoleID == _usercurrentrole.r.RoleID
                                      select m).ToList();

                menus = new List<BusinessPOCO.Admin.Menu>();
                foreach (var menu in _assignedmenuscurrentrole)
                {
                    menus.Add(new BusinessPOCO.Admin.Menu()
                    {
                        MenuID = menu.MenuID,
                        MenuName = menu.MenuName,
                        ParentID = menu.ParentID,
                        ToolTip = menu.ToolTip,
                        Link = menu.Link,
                        IsViewable = menu.IsViewable,
                        Icon = menu.Icon,
                        Description = menu.Description

                    });
                }

                var _sectionrestrictionscurrentrole = (from sr in sectionrestriction.GetAll()
                                                       where sr.RoleID == _usercurrentrole.r.RoleID
                                            select sr).ToList();

                sectionrestrictions = new List<SectionRestriction>();

                foreach (var section in _sectionrestrictionscurrentrole)
                {
                    sectionrestrictions.Add(new SectionRestriction()
                    {
                        RoleID = section.RoleID,
                        SectionID = section.SectionID,
                        SectionSelector = section.SectionSelector,
                        MenuID = section.MenuID,
                        EmployeeID = section.EmployeeID,
                        Menu = rolemenu.GetByID(section.MenuID.Value)
                    });
                }

                Role _usercurrentroleconfirm = new Role()
                    {
                        AssignedMenus = menus,
                        SectionRestrictions = sectionrestrictions,
                        Description = _usercurrentrole.r.Description,
                        IsActive = _usercurrentrole.r.IsActive,
                        RoleID = _usercurrentrole.r.RoleID,
                        RoleName = _usercurrentrole.r.RoleName,
                        IsDefault = _usercurrentrole.rm.IsDefault
                    };
                #endregion

                #region Client and Consultant
                Client _cl = null;
                LaboratoryBusiness.POCO.Admin.Consultant _consultant = null;

                if (((DetailTypeEnum)user.DetailType.Value) == DetailTypeEnum.Client)
                {
                     _cl = new Client(user.DetailID.Value);

                }

                else if (((DetailTypeEnum)user.DetailType.Value) == DetailTypeEnum.Consultant)
                {
                    _consultant = consultant.GetByID(user.DetailID.Value);
                }

                #endregion

                SystemUser inneruser = new SystemUser()
                {
                    Password = user.Password,
                    Username = user.Username,
                    SystemUserID = user.SystemUserID,
                    AssignedRoles = roles,
                    AssignedMenus = _userassignedmenus,
                    SectionRestrictions = _usersectionrestrictions,
                    CurrentRole = _usercurrentroleconfirm,
                    MenuPriority = _userassignedmenus.Count > 0 ? MenuPriorityEnum.Employee:MenuPriorityEnum.Role,
                    FirstName = user.FirstName,
                    MiddleName = user.MiddleName,
                    LastName = user.LastName,
                    Email = user.Email,
                    MobileNo =user.MobileNo,
                    Address = user.Address,
                    IsActive = user.IsActive,
                    ProfilePic = user.ProfilePic,
                    DetailType = user.DetailType,
                    DetailTypeProperty = (DetailTypeEnum)user.DetailType.Value,
                    DetailID = user.DetailID,
                    CreatedBy = user.CreatedBy,
                    CreatedDate = user.CreatedDate,
                    UpdatedBy = user.UpdatedBy,
                    UpdatedDate = user.UpdatedDate,
                    Client = _cl,
                    Consultant = _consultant

                };

                return inneruser;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ReSync_MySystemUser_Session()
        {
            try
            {
                Repositories.Admin.ISystemUserRepository userrep = new BLL.Admin.SystemUserRepository();
                BusinessPOCO.Admin.SystemUser user = userrep.GetByID(this.SystemUserID);
                SystemUser su = SystemUser.Initialize(user);
                MySession.SystemSession = su;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "--- Something went wrong while update the SystemSession");
            }
        }

        public static LaboratoryBusiness.POCO.Admin.SystemUser GetSystemUserBySubdomain(string subdomain)
        {

            Repositories.Admin.ISystemUserRepository systemuser = new BLL.Admin.SystemUserRepository();
            Repositories.Admin.IClientRepository client = new BLL.Admin.ClientRepository();
            Repositories.Admin.ISystemUserTypeRepository systemsusertype = new BLL.Admin.SystemUserTypeRepository();

            int clientdetailtypeid = systemsusertype.GetAll().Where(x => x.TypeName.Equals("Client")).FirstOrDefault().SystemUserTypeID;

            var returnobj = (from cl in client.GetAll()
                             join su in systemuser.GetAll() on cl.ClientDetailID equals su.DetailID
                             where su.DetailType.Value == clientdetailtypeid && cl.Subdomain.Equals(subdomain)
                             select su).FirstOrDefault();

            return returnobj;
        }
    }
}