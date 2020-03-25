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
    public class Client : LaboratoryBusiness.POCO.Admin.Client
    {
        public LaboratoryBusiness.POCO.Admin.Plan Plan { get; set; }

        public LaboratoryBusiness.POCO.Admin.StorageandDBProvider StorageAndDBProvider { get; set; }

        public List<LaboratoryBusiness.POCO.Admin.ClientParameter> ClientParameters { get; set; }

        public string ConnectionString { get; set; }

        public string EntityConnectionString { get; set; }

        private void Initialize(int _clientid)
        {
            Repositories.Admin.IClientRepository client = new BLL.Admin.ClientRepository();
            Repositories.Admin.IPlanRepository plan = new BLL.Admin.PlanRepository();
            Repositories.Admin.IStorageandDBProviderRepository storageanddbprovider = new BLL.Admin.StorageandDBProviderRepository();
            Repositories.Admin.IClientParameterRepository clientparameter = new BLL.Admin.ClientParameterRepository();
            Repositories.Admin.IConsultantRepository consultant = new BLL.Admin.ConsultantRepository();
            Repositories.Admin.IProviderParamterRepository providerparameter = new BLL.Admin.ProviderParamterRepository();

            LaboratoryBusiness.POCO.Admin.Client clientpoco = client.GetByID(_clientid);
            LaboratoryBusiness.POCO.Admin.Plan planpoco = plan.GetByID(clientpoco.PlanID.Value);
            LaboratoryBusiness.POCO.Admin.StorageandDBProvider standdbprovider = storageanddbprovider.GetByID(clientpoco.STandDBprovdiderID.Value);
            List<LaboratoryBusiness.POCO.Admin.ClientParameter> clparameters = clientparameter.GetAll().Where(x => x.ClientDetailID == clientpoco.ClientDetailID).ToList();

            if (standdbprovider.ProviderName.StartsWith("Server DB"))
            {
                string _connectionstring = WebConfigurationManager.ConnectionStrings["ServerDB_Client"].ConnectionString;
                string _entityconnectionstring = WebConfigurationManager.ConnectionStrings["ServerDB_Client_Entity"].ConnectionString;
                _connectionstring = _connectionstring.Replace("@clientdb", ("LabSystemClient_" + clientpoco.CompanyName.Replace(" ", "")));
                _entityconnectionstring = _entityconnectionstring.Replace("@clientdb", ("LabSystemClient_" + clientpoco.CompanyName.Replace(" ", "")));

                this.ConnectionString = _connectionstring;

                this.EntityConnectionString = _entityconnectionstring;
            }

            else
            {
                string _host = (from a in clparameters.AsEnumerable()
                                join b in providerparameter.GetAll() on a.ParameterID equals b.ParameterID
                                select new { a, b }).Where(x => x.b.ParameterName.Equals("DB Server/Host IP")).Select(x => x.a.ParameterValue).FirstOrDefault();

                string _username = (from a in clparameters.AsEnumerable()
                                    join b in providerparameter.GetAll() on a.ParameterID equals b.ParameterID
                                    select new { a, b }).Where(x => x.b.ParameterName.Equals("DB Username")).Select(x => x.a.ParameterValue).FirstOrDefault();

                string _password = (from a in clparameters.AsEnumerable()
                                    join b in providerparameter.GetAll() on a.ParameterID equals b.ParameterID
                                    select new { a, b }).Where(x => x.b.ParameterName.Equals("DB Password")).Select(x => x.a.ParameterValue).FirstOrDefault();

                string _connectionstring = "server=" + _host + ";database=" + ("LabSystemClient_" + clientpoco.CompanyName.Replace(" ", "")) + ";uid=" + _username + ";password=" + _password + ";";

                this.ConnectionString = _connectionstring;

                this.EntityConnectionString = "metadata=res://*/DAL.Client.ClientEntity.csdl|res://*/DAL.Client.ClientEntity.ssdl|res://*/DAL.Client.ClientEntity.msl;provider=System.Data.SqlClient;provider connection string='data source=" + _host + ";initial catalog=" + ("LabSystemClient_" + clientpoco.CompanyName.Replace(" ", "")) + ";uid=" + _username + ";pwd=" + _password + ";timeout=100000;MultipleActiveResultSets=True;App=EntityFramework'";
            }

            this.ClientParameters = clparameters;
            this.Plan = planpoco;
            this.StorageAndDBProvider = standdbprovider;

            this.ClientDetailID = clientpoco.ClientDetailID;
            this.CompanyName = clientpoco.CompanyName;
            this.CreatedBy = clientpoco.CreatedBy;
            this.CreatedDate = clientpoco.CreatedDate;
            this.DBErrorDetail = clientpoco.DBErrorDetail;
            this.InvoiceID = clientpoco.InvoiceID;
            this.InvoicePDFLink = clientpoco.InvoicePDFLink;
            this.IsDBCreated = clientpoco.IsDBCreated;
            this.IsDbError = clientpoco.IsDbError;
            this.IsStorageConfigured = clientpoco.IsStorageConfigured;
            this.IsStorageError = clientpoco.IsStorageError;
            this.LastActionOnDB = clientpoco.LastActionOnDB;
            this.LastActionOnStorage = clientpoco.LastActionOnStorage;
            this.PlanDuration = clientpoco.PlanDuration;
            this.PlanID = clientpoco.PlanID;
            this.PriceUnit = clientpoco.PriceUnit;
            this.STandDBprovdiderID = clientpoco.STandDBprovdiderID;
            this.StorageErrorDetail = clientpoco.StorageErrorDetail;
            this.Subdomain = clientpoco.Subdomain;
            this.TotalLicenseCost = clientpoco.TotalLicenseCost;
            this.UpdatedBy = clientpoco.UpdatedBy;
            this.UpdatedDate = clientpoco.UpdatedDate;
            this.LogoImgLink = clientpoco.LogoImgLink;
            this.BackgroundImgLink = clientpoco.BackgroundImgLink;
        }

        public Client(int clientid)
        {
            Initialize(clientid);
        }

        public Client()
        {

        }

        public Client(string subdomain)
        {
            Repositories.Admin.IClientRepository client = new BLL.Admin.ClientRepository();
            int clid = client.GetAll().Where(x => x.Subdomain.Trim().Equals(subdomain)).FirstOrDefault().ClientDetailID;
            Initialize(clid);
        }
    }
}