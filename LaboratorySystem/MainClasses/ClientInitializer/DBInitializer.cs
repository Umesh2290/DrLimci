using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using Repositories = LaboratoryBusiness.Repositories;
using BLL = LaboratoryBusiness.BLL;
using BusinessPOCO = LaboratoryBusiness.POCO;
using System.Diagnostics;

namespace LaboratorySystem
{
    public class DBInitializer
    {
        private Client _client;

        private string _connectionstring = string.Empty;
        public DBInitializer(Client cl)
        {
            if (cl != null)
            {
                try
                {
                    _client = cl;
                    if (cl.IsDBCreated.HasValue == true)
                    {
                        if (cl.IsDBCreated.Value == false)
                        {
                            try
                            {
                                CreateDB();

                                Repositories.Admin.IClientRepository cl_for_update = new BLL.Admin.ClientRepository();
                                var cll = cl_for_update.GetByID(cl.ClientDetailID);
                                cll.IsDBCreated = true;
                                cll.IsDbError = false;
                                cll.LastActionOnDB = DateTime.Now;

                                cl_for_update.Update(cll);

                                cl_for_update.Save();
                            }
                            catch (Exception ex)
                            {
                                Repositories.Admin.IClientRepository cl_for_update = new BLL.Admin.ClientRepository();
                                var cll = cl_for_update.GetByID(cl.ClientDetailID);
                                cll.IsDBCreated = false;
                                cll.IsDbError = true;
                                cll.DBErrorDetail = ex.Message;
                                cll.LastActionOnDB = DateTime.Now;

                                cl_for_update.Update(cll);

                                cl_for_update.Save();

                                throw ex;
                            }

                        }
                    }

                    else
                    {
                        try
                        {
                            CreateDB();

                            Repositories.Admin.IClientRepository cl_for_update = new BLL.Admin.ClientRepository();
                            var cll = cl_for_update.GetByID(cl.ClientDetailID);
                            cll.IsDBCreated = true;
                            cll.IsDbError = false;
                            cll.LastActionOnDB = DateTime.Now;

                            cl_for_update.Update(cll);

                            cl_for_update.Save();
                        }
                        catch (Exception ex)
                        {
                            Repositories.Admin.IClientRepository cl_for_update = new BLL.Admin.ClientRepository();
                            var cll = cl_for_update.GetByID(cl.ClientDetailID);
                            cll.IsDBCreated = false;
                            cll.IsDbError = true;
                            cll.DBErrorDetail = ex.Message;
                            cll.LastActionOnDB = DateTime.Now;

                            cl_for_update.Update(cll);

                            cl_for_update.Save();

                            throw ex;
                        }
                    }
                    if (MySession.SystemSession != null)
                    {
                        MySession.SystemSession.ReSync_MySystemUser_Session();
                    }

                    this._connectionstring = _client.EntityConnectionString;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                throw new Exception("Client Object is null");
            }

        }

        private void CreateDB()
        {
            try
            {
                string sqlConnectionString = string.Empty;

                if (_client.ConnectionString.Contains("password") && _client.ConnectionString.Contains("uid"))
                {
                    sqlConnectionString = _client.ConnectionString.Replace(("database=LabSystemClient_" + _client.CompanyName.Replace(" ", "")), "database=master");
                }

                else
                {
                    sqlConnectionString = _client.ConnectionString.Replace(("initial catalog=LabSystemClient_" + _client.CompanyName.Replace(" ", "")), "initial catalog=master");
                }

                string script = File.ReadAllText(HelpingClass.GetDefaultDirectory()+"\\bin\\" + WebConfigurationManager.AppSettings["ClientDBScriptPath"]);

                script = script.Replace("@clientdb", "LabSystemClient_"+_client.CompanyName.Replace(" ", ""));

                SqlConnection conn = new SqlConnection(sqlConnectionString);

                Server server = new Server(new ServerConnection(conn));

                server.ConnectionContext.ExecuteNonQuery(script);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "--- Something went wrong while creating DB");
            }
        }

        public Repositories.User.IMenuRepository MenuRepository()
        {
            return new BLL.User.MenuRepository(new LaboratoryBusiness.DAL.Client.LabSystemClient_RajLabEntities(this._connectionstring));
        }

        public Repositories.User.IMenuAssignmentRepository MenuAssignmentRepository()
        {
            return new BLL.User.MenuAssignmentRepository(new LaboratoryBusiness.DAL.Client.LabSystemClient_RajLabEntities(this._connectionstring));
        }

        public Repositories.User.INotificationRepository NotificationRepository()
        {
            return new BLL.User.NotificationRepository(new LaboratoryBusiness.DAL.Client.LabSystemClient_RajLabEntities(this._connectionstring));
        }
        public Repositories.User.IRoleRepository RoleRepository()
        {
            return new BLL.User.RoleRepository(new LaboratoryBusiness.DAL.Client.LabSystemClient_RajLabEntities(this._connectionstring));
        }

        public Repositories.User.IRoleMappingRepository RoleMappingRepository()
        {
            return new BLL.User.RoleMappingRepository(new LaboratoryBusiness.DAL.Client.LabSystemClient_RajLabEntities(this._connectionstring));
        }
        public Repositories.User.ISectionRestrictionRepository SectionRestrictionRepository()
        {
            return new BLL.User.SectionRestrictionRepository(new LaboratoryBusiness.DAL.Client.LabSystemClient_RajLabEntities(this._connectionstring));
        }
        public Repositories.User.IIncorrectPasswordAttemptRepository IncorrectPasswordAttemptRepository()
        {
            return new BLL.User.IncorrectPasswordAttemptRepository(new LaboratoryBusiness.DAL.Client.LabSystemClient_RajLabEntities(this._connectionstring));
        }
        public Repositories.User.IForgetPasswordRepository ForgetPasswordRepository()
        {
            return new BLL.User.ForgetPasswordRepository(new LaboratoryBusiness.DAL.Client.LabSystemClient_RajLabEntities(this._connectionstring));
        }
        public Repositories.User.IClientUserRepository ClientUserRepository()
        {
            return new BLL.User.ClientUserRepository(new LaboratoryBusiness.DAL.Client.LabSystemClient_RajLabEntities(this._connectionstring));
        }
        public Repositories.User.IClientUserTypeRepository ClientUserTypeRepository()
        {
            return new BLL.User.ClientUserTypeRepository(new LaboratoryBusiness.DAL.Client.LabSystemClient_RajLabEntities(this._connectionstring));
        }
        public Repositories.User.IAdminDetailRepository AdminDetailRepository()
        {
            return new BLL.User.AdminDetailRepository(new LaboratoryBusiness.DAL.Client.LabSystemClient_RajLabEntities(this._connectionstring));
        }

        ////

        public Repositories.User.IClientUserAttachmentDetailRepository ClientUserAttachmentDetailRepository()
        {
            return new BLL.User.ClientUserAttachmentDetailRepository(new LaboratoryBusiness.DAL.Client.LabSystemClient_RajLabEntities(this._connectionstring));
        }

        public Repositories.User.IClientUserDetailRepository ClientUserDetailRepository()
        {
            return new BLL.User.ClientUserDetailRepository(new LaboratoryBusiness.DAL.Client.LabSystemClient_RajLabEntities(this._connectionstring));
        }

        public Repositories.User.IExtraWorkRequestReposotory ExtraWorkRequestRepository()
        {
            return new BLL.User.ExtraWorkRequestRepository(new LaboratoryBusiness.DAL.Client.LabSystemClient_RajLabEntities(this._connectionstring));
        }

        public Repositories.User.IExtraWorkAttachmentRepository ExtraWorkAttachmentRepository()
        {
            return new BLL.User.ExtraWorkAttachmentRepository(new LaboratoryBusiness.DAL.Client.LabSystemClient_RajLabEntities(this._connectionstring));
        }

        public Repositories.User.IExtraWorkRequestStatusRepository ExtraWorkRequestStatusRepository()
        {
            return new BLL.User.ExtraWorkRequestStatusRepository(new LaboratoryBusiness.DAL.Client.LabSystemClient_RajLabEntities(this._connectionstring));
        }

        public Repositories.User.IInventory InventoryRepository()
        {
            return new BLL.User.InventoryRepository(new LaboratoryBusiness.DAL.Client.LabSystemClient_RajLabEntities(this._connectionstring));
        }

        public Repositories.User.IInventoryRequestRepository InventoryRequestRepository()
        {
            return new BLL.User.InventoryRequestRepository(new LaboratoryBusiness.DAL.Client.LabSystemClient_RajLabEntities(this._connectionstring));
        }

        public Repositories.User.IInventoryRequestStatusRepository InventoryRequestStatusRepository()
        {
            return new BLL.User.InventoryRequestStatusRepository(new LaboratoryBusiness.DAL.Client.LabSystemClient_RajLabEntities(this._connectionstring));
        }

        public Repositories.User.IInventoryStatusTypeRepository InventoryStatusTypeRepository()
        {
            return new BLL.User.InventoryStatusTypeRepository(new LaboratoryBusiness.DAL.Client.LabSystemClient_RajLabEntities(this._connectionstring));
        }

        public Repositories.User.IOpinionRequestRepository OpinionRequestRepository()
        {
            return new BLL.User.OpinionRequestRepository(new LaboratoryBusiness.DAL.Client.LabSystemClient_RajLabEntities(this._connectionstring));
        }

        public Repositories.User.IOpinionRequestStatus OpinionRequestStatus()
        {
            return new BLL.User.OpinionRequestStatus(new LaboratoryBusiness.DAL.Client.LabSystemClient_RajLabEntities(this._connectionstring));
        }

        public Repositories.User.IPatientDetailRepository PatientDetailRepository()
        {
            return new BLL.User.PatientDetailRepository(new LaboratoryBusiness.DAL.Client.LabSystemClient_RajLabEntities(this._connectionstring));
        }

        public Repositories.User.ITestRepository TestRepository()
        {
            return new BLL.User.TestRepository(new LaboratoryBusiness.DAL.Client.LabSystemClient_RajLabEntities(this._connectionstring));
        }

        public Repositories.User.ITestAttachmentRepository TestAttachmentRepository()
        {
            return new BLL.User.TestAttachmentRepository(new LaboratoryBusiness.DAL.Client.LabSystemClient_RajLabEntities(this._connectionstring));
        }

        public Repositories.User.ITestAttachmentTypeRepository TestAttachmentTypeRepository()
        {
            return new BLL.User.TestAttachmentTypeRepository(new LaboratoryBusiness.DAL.Client.LabSystemClient_RajLabEntities(this._connectionstring));
        }

        public Repositories.User.ITestConclusionRepository TestConclusionRepository()
        {
            return new BLL.User.TestConclusionRepository(new LaboratoryBusiness.DAL.Client.LabSystemClient_RajLabEntities(this._connectionstring));
        }

        public Repositories.User.ITestInvestigationRepository TestInvestigationRepository()
        {
            return new BLL.User.TestInvestigationRepository(new LaboratoryBusiness.DAL.Client.LabSystemClient_RajLabEntities(this._connectionstring));
        }

        public Repositories.User.ITestReportTypeRepository TestReportTypeRepository()
        {
            return new BLL.User.TestReportTypeRepository(new LaboratoryBusiness.DAL.Client.LabSystemClient_RajLabEntities(this._connectionstring));
        }

        public Repositories.User.ITestStatusRepositories TestStatusRepositories()
        {
            return new BLL.User.TestStatusRepositories(new LaboratoryBusiness.DAL.Client.LabSystemClient_RajLabEntities(this._connectionstring));
        }

        public Repositories.User.ITestSupplementReportRepository TestSupplementReportRepository()
        {
            return new BLL.User.TestSupplementReportRepository(new LaboratoryBusiness.DAL.Client.LabSystemClient_RajLabEntities(this._connectionstring));
        }
        public Repositories.User.IHospitalDetail HospitalDetailRepository()
        {
            return new BLL.User.HospitalDetailRepository(new LaboratoryBusiness.DAL.Client.LabSystemClient_RajLabEntities(this._connectionstring));
        }
    }
}