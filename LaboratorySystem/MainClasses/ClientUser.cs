using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Repositories = LaboratoryBusiness.Repositories;
using BLL = LaboratoryBusiness.BLL;
using BusinessPOCO = LaboratoryBusiness.POCO;

namespace LaboratorySystem
{

    public enum Cl_DetailTypeEnum
    {
        ClientAdmin = 1,
        Receptionist = 2,
        Lab_Technician = 3,
        Clinical_Laboratory_Scientist = 4,
        Clinical_Laboratory_Consultant = 5,
        Head_Lab_Technician = 6,
        Inventory_Manager = 7,
        Patient = 8,
    }
    public class ClientUser : LaboratoryBusiness.POCO.User.Cl_ClientUser
    {
        private string Url { get; set; }
        private DBInitializer innerdb { get; set; }
        public List<Cl_Role> AssignedRoles
        {
            get;
            set;
        }

        public ClientUser()
        {

        }

        public ClientUser(string url,DBInitializer db)
        {
            this.Url = url;
            this.innerdb = db;
        }
        public Cl_Role CurrentRole { get; set; }
        public List<BusinessPOCO.User.Cl_Menu> AssignedMenus { get; set; }
        public List<BusinessPOCO.User.Cl_SectionRestriction> SectionRestrictions { get; set; }

        public MenuPriorityEnum MenuPriority { get; set; }

        public Cl_DetailTypeEnum DetailTypeProperty { get; set; }

        public BusinessPOCO.User.Cl_AdminDetail AdminDetail { get; set; }

        public BusinessPOCO.User.Cl_ClientUserDetail ClientUserDetail { get; set; }

        public BusinessPOCO.User.Cl_PatientDetail PatientDetail { get; set; }

        public List<BusinessPOCO.User.Cl_ClientUserAttachmentDetail> AttachmentList { get; set; }

        public static ClientUser Initialize(LaboratoryBusiness.POCO.User.Cl_ClientUser client,string url,DBInitializer db)
        {
            try
            {
                #region Repositories Initialization
                Repositories.User.IRoleRepository userrole = db.RoleRepository();
                Repositories.User.IRoleMappingRepository userrolemapping = db.RoleMappingRepository();
                Repositories.User.IMenuRepository rolemenu = db.MenuRepository();
                Repositories.User.IMenuAssignmentRepository rolemenumapping = db.MenuAssignmentRepository();
                Repositories.User.ISectionRestrictionRepository sectionrestriction = db.SectionRestrictionRepository();
                Repositories.User.IAdminDetailRepository admindetail = db.AdminDetailRepository();
                Repositories.User.IClientUserDetailRepository clientuserdetail = db.ClientUserDetailRepository();
                Repositories.User.IPatientDetailRepository patientdetail = db.PatientDetailRepository();
                Repositories.User.IClientUserAttachmentDetailRepository clientuserattachmentdetail = db.ClientUserAttachmentDetailRepository();
                #endregion

                #region Assigned Roles
                var _assignedroles = (from r in userrole.GetAll()
                                      join rm in userrolemapping.GetAll() on r.RoleID equals rm.RoleID
                                      where rm.UserID == client.ClientUserID
                                      select new { r, rm }).ToList();

                List<Cl_Role> roles = new List<Cl_Role>();
                List<LaboratoryBusiness.POCO.User.Cl_Menu> menus = new List<BusinessPOCO.User.Cl_Menu>();
                List<Cl_SectionRestriction> sectionrestrictions = new List<Cl_SectionRestriction>();

                foreach (var role in _assignedroles)
                {
                    var _assignedmenus = (from m in rolemenu.GetAll()
                                          join mm in rolemenumapping.GetAll() on m.MenuID equals mm.MenuID
                                          where mm.RoleID == role.r.RoleID
                                          select m).ToList();

                    menus = new List<LaboratoryBusiness.POCO.User.Cl_Menu>();
                    foreach (var menu in _assignedmenus)
                    {
                        menus.Add(new BusinessPOCO.User.Cl_Menu()
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

                    sectionrestrictions = new List<Cl_SectionRestriction>();

                    foreach (var section in _sectionrestrictions)
                    {
                        sectionrestrictions.Add(new Cl_SectionRestriction()
                        {
                            RoleID = section.RoleID,
                            SectionID = section.SectionID,
                            SectionSelector = section.SectionSelector,
                            MenuID = section.MenuID,
                            EmployeeID = section.EmployeeID,
                            Menu = rolemenu.GetByID(section.MenuID.Value)
                        });
                    }

                    roles.Add(new Cl_Role()
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
                                          where mm.EmployeeID == client.ClientUserID
                                          select m).ToList();

                var _usersectionrestrictions = (from sr in sectionrestriction.GetAll()
                                                where sr.EmployeeID == client.ClientUserID
                                                select sr).ToList();
                #endregion

                #region Current Role Assignment
                var _usercurrentrole = (from r in userrole.GetAll()
                                        join rm in userrolemapping.GetAll() on r.RoleID equals rm.RoleID
                                        where rm.UserID == client.ClientUserID && rm.IsDefault.HasValue ? rm.IsDefault == true : false
                                        select new { r, rm }).ToList().FirstOrDefault();

                var _assignedmenuscurrentrole = (from m in rolemenu.GetAll()
                                                 join mm in rolemenumapping.GetAll() on m.MenuID equals mm.MenuID
                                                 where mm.RoleID == (_usercurrentrole==null?0: _usercurrentrole.r.RoleID)
                                                 select m).ToList();

                menus = new List<BusinessPOCO.User.Cl_Menu>();
                if (_assignedmenuscurrentrole != null)
                {
                    foreach (var menu in _assignedmenuscurrentrole)
                    {
                        menus.Add(new BusinessPOCO.User.Cl_Menu()
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
                }

                var _sectionrestrictionscurrentrole = (from sr in sectionrestriction.GetAll()
                                                       where sr.RoleID == (_usercurrentrole==null?0: _usercurrentrole.r.RoleID)
                                                       select sr).ToList();

                sectionrestrictions = new List<Cl_SectionRestriction>();

                if (_sectionrestrictionscurrentrole != null)
                {
                    foreach (var section in _sectionrestrictionscurrentrole)
                    {
                        sectionrestrictions.Add(new Cl_SectionRestriction()
                        {
                            RoleID = section.RoleID,
                            SectionID = section.SectionID,
                            SectionSelector = section.SectionSelector,
                            MenuID = section.MenuID,
                            EmployeeID = section.EmployeeID,
                            Menu = rolemenu.GetByID(section.MenuID.Value)
                        });
                    }
                }

                Cl_Role _usercurrentroleconfirm = _usercurrentrole==null ? null:(new Cl_Role()
                {
                    AssignedMenus = menus,
                    SectionRestrictions = sectionrestrictions,
                    Description = _usercurrentrole.r.Description,
                    IsActive = _usercurrentrole.r.IsActive,
                    RoleID = _usercurrentrole.r.RoleID,
                    RoleName = _usercurrentrole.r.RoleName,
                    IsDefault = _usercurrentrole.rm.IsDefault
                });
                #endregion

                #region Detail Type Objects
                LaboratoryBusiness.POCO.User.Cl_AdminDetail _admindetail = null;
                LaboratoryBusiness.POCO.User.Cl_ClientUserDetail _clientuserdetail = null;
                LaboratoryBusiness.POCO.User.Cl_PatientDetail _patientdetail = null;
                List<LaboratoryBusiness.POCO.User.Cl_ClientUserAttachmentDetail> _attachmentlist = new List<BusinessPOCO.User.Cl_ClientUserAttachmentDetail>();

                if (((Cl_DetailTypeEnum)client.DetailType.Value) == Cl_DetailTypeEnum.ClientAdmin)
                {
                    _admindetail = admindetail.GetByID(client.DetailID.Value);

                }
                else if (((Cl_DetailTypeEnum)client.DetailType.Value) != Cl_DetailTypeEnum.Patient)
                {
                    _clientuserdetail = clientuserdetail.GetByID(client.DetailID.Value);
                    _attachmentlist = clientuserattachmentdetail.GetAll().Where(x => x.UserDetailID.Value == client.DetailID.Value).ToList();
                }
                else
                {
                    _patientdetail = patientdetail.GetByID(client.DetailID.Value);
                }


                #endregion

                ClientUser inneruser = new ClientUser()
                {
                    Password = client.Password,
                    Username = client.Username,
                    ClientUserID = client.ClientUserID,
                    AssignedRoles = roles,
                    AssignedMenus = _userassignedmenus,
                    SectionRestrictions = _usersectionrestrictions,
                    CurrentRole = _usercurrentroleconfirm,
                    MenuPriority = _userassignedmenus.Count > 0 ? MenuPriorityEnum.Employee : MenuPriorityEnum.Role,
                    FirstName = client.FirstName,
                    LastName = client.LastName,
                    Email = client.Email,
                    MobileNo = client.MobileNo,
                    Address = client.Address,
                    IsActive = client.IsActive,
                    ProfilePic = client.ProfilePic,
                    DetailType = client.DetailType,
                    DetailTypeProperty = (Cl_DetailTypeEnum)client.DetailType.Value,
                    DetailID = client.DetailID,
                    CreatedBy = client.CreatedBy,
                    CreatedDate = client.CreatedDate,
                    UpdatedBy = client.UpdatedBy,
                    UpdatedDate = client.UpdatedDate,
                    AdminDetail = _admindetail,
                    ClientUserDetail = _clientuserdetail,
                    PatientDetail = _patientdetail,
                    AttachmentList = _attachmentlist,
                    Url = url,
                    innerdb = db,
                    HospitalDetailID=client.HospitalDetailID

                };

                return inneruser;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void ReSync_MyClientUser_Session()
        {
            try
            {
                Repositories.User.IClientUserRepository userrep = this.innerdb.ClientUserRepository();
                BusinessPOCO.User.Cl_ClientUser user = userrep.GetByID(this.ClientUserID);
                ClientUser cu = ClientUser.Initialize(user,this.Url,this.innerdb);
                MySession.SetClientSession(this.Url, cu);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "--- Something went wrong while update the SystemSession");
            }
        }

    }
}