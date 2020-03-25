using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Repositories = LaboratoryBusiness.Repositories;
using BLL = LaboratoryBusiness.BLL;
using BusinessPOCO = LaboratoryBusiness.POCO;
using System.IO;
using System.Diagnostics;
using System.Net;

namespace LaboratorySystem
{
    public class FileInitializer
    {
        private Client _client;

        private string ftphost = string.Empty;
        private string ftpusername = string.Empty;
        private string ftppassword = string.Empty;

        public FileInitializer(Client cl)
        {
            if (cl != null)
            {
                try
                {
                    this._client = cl;
                    Repositories.Admin.IProviderParamterRepository providerparameter = new BLL.Admin.ProviderParamterRepository();

                    if (!cl.StorageAndDBProvider.ProviderName.EndsWith("+ Server Storage"))
                    {
                        ftphost = (from a in cl.ClientParameters.AsEnumerable()
                                   join b in providerparameter.GetAll() on a.ParameterID equals b.ParameterID
                                   select new { a, b }).Where(x => x.b.ParameterName.Equals("FTP Server/Host IP")).Select(x => x.a.ParameterValue).FirstOrDefault();

                        ftpusername = (from a in cl.ClientParameters.AsEnumerable()
                                       join b in providerparameter.GetAll() on a.ParameterID equals b.ParameterID
                                       select new { a, b }).Where(x => x.b.ParameterName.Equals("FTP Username")).Select(x => x.a.ParameterValue).FirstOrDefault();

                        ftppassword = (from a in cl.ClientParameters.AsEnumerable()
                                       join b in providerparameter.GetAll() on a.ParameterID equals b.ParameterID
                                       select new { a, b }).Where(x => x.b.ParameterName.Equals("FTP Password")).Select(x => x.a.ParameterValue).FirstOrDefault();

                    }

                        if (_client.IsStorageConfigured.HasValue)
                        {
                            if (_client.IsStorageConfigured.Value == false)
                            {
                                FileManager testfm = new FileManager(HelpingClass.GetDefaultDirectory() + "\\TestUploadFile.txt");
                                string up;
                                bool isuploaded = this.UploadFile(testfm, "\\TestFolder", out up);

                                if (!isuploaded)
                                {
                                    Repositories.Admin.IClientRepository cl_for_update = new BLL.Admin.ClientRepository();
                                    var cll = cl_for_update.GetByID(cl.ClientDetailID);
                                    cll.IsStorageConfigured = false;
                                    cll.IsStorageError = true;
                                    cll.StorageErrorDetail = "";
                                    cll.LastActionOnStorage = DateTime.Now;

                                    cl_for_update.Update(cll);

                                    cl_for_update.Save();
                                }

                                else
                                {
                                    Repositories.Admin.IClientRepository cl_for_update = new BLL.Admin.ClientRepository();
                                    var cll = cl_for_update.GetByID(cl.ClientDetailID);
                                    cll.IsStorageConfigured = true;
                                    cll.IsStorageError = false;
                                    cll.LastActionOnStorage = DateTime.Now;

                                    cl_for_update.Update(cll);

                                    cl_for_update.Save();
                                }
                            }
                        }

                        else
                        {
                            FileManager testfm = new FileManager(HelpingClass.GetDefaultDirectory() + "\\TestUploadFile.txt");
                            string up;
                            bool isuploaded = this.UploadFile(testfm, "\\TestFolder", out up);

                            if (!isuploaded)
                            {
                                Repositories.Admin.IClientRepository cl_for_update = new BLL.Admin.ClientRepository();
                                var cll = cl_for_update.GetByID(cl.ClientDetailID);
                                cll.IsStorageConfigured = false;
                                cll.IsStorageError = true;
                                cll.StorageErrorDetail = "";
                                cll.LastActionOnStorage = DateTime.Now;

                                cl_for_update.Update(cll);

                                cl_for_update.Save();
                            }

                            else
                            {
                                Repositories.Admin.IClientRepository cl_for_update = new BLL.Admin.ClientRepository();
                                var cll = cl_for_update.GetByID(cl.ClientDetailID);
                                cll.IsStorageConfigured = true;
                                cll.IsStorageError = false;
                                cll.LastActionOnStorage = DateTime.Now;

                                cl_for_update.Update(cll);

                                cl_for_update.Save();
                            }
                        }
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

        public bool UploadFile(FileManager file, string DirectoryPath,out string uploadedfilepath)
        {
            if (!string.IsNullOrEmpty(DirectoryPath))
            {
                if (_client.StorageAndDBProvider.ProviderName.EndsWith("+ Server Storage"))
                {
                    if (!Directory.Exists(HelpingClass.GetDefaultDirectory() + "\\ClientsData\\" + _client.CompanyName.Replace(" ", "") + "" + DirectoryPath))
                    {
                        Directory.CreateDirectory(HelpingClass.GetDefaultDirectory() + "\\ClientsData\\" + _client.CompanyName.Replace(" ", "") + "" + DirectoryPath);
                    }

                    if (File.Exists((HelpingClass.GetDefaultDirectory() + "\\ClientsData\\" + _client.CompanyName.Replace(" ", "") + "" + DirectoryPath+"\\" + file.Name + file.Extension)))
                    {
                        File.Delete((HelpingClass.GetDefaultDirectory() + "\\ClientsData\\" + _client.CompanyName.Replace(" ", "") + "" + DirectoryPath + "\\" + file.Name + file.Extension));
                    }

                    FileManager.ByteArrayToFile((HelpingClass.GetDefaultDirectory() + "\\ClientsData\\" + _client.CompanyName.Replace(" ", "") + "" + DirectoryPath+"\\" + file.Name + file.Extension), file.FileBytes);

                    uploadedfilepath = (HelpingClass.GetDefaultDirectory() + "\\ClientsData\\" + _client.CompanyName.Replace(" ", "") + "" + DirectoryPath + "\\" + file.Name + file.Extension);
                    return true;
                }

                else
                {
                    MakeFTPDir(this.ftphost, @"MyData_" + (_client.CompanyName.Replace(" ", "") + DirectoryPath.Replace("\\","/")), this.ftpusername, this.ftppassword);

                    FtpWebRequest request =
                    (FtpWebRequest)WebRequest.Create("ftp://" + this.ftphost + (@"/MyData_" + (_client.CompanyName.Replace(" ", "") + DirectoryPath.Replace("\\", "/")))+"/" + file.Name + file.Extension);
                    request.Credentials = new NetworkCredential(this.ftpusername, this.ftppassword);
                    request.Method = WebRequestMethods.Ftp.UploadFile;

                    using (MemoryStream fileStream = new MemoryStream(file.FileBytes))
                    using (Stream ftpStream = request.GetRequestStream())
                    {
                        fileStream.CopyTo(ftpStream);
                    }

                    uploadedfilepath = ("http://" + this.ftphost + "/MyFiles"+(@"/MyData_" + (_client.CompanyName.Replace(" ", "") + DirectoryPath.Replace("\\", "/"))) + "/" + file.Name + file.Extension);
                    return true;
                }
            }
            else
            {
                throw new Exception("DirectoryPath cannot be empty or null !");
            }
        }

        private static void MakeFTPDir(string ftpAddress, string pathToCreate, string login, string password)
        {
            FtpWebRequest reqFTP = null;
            Stream ftpStream = null;

            string[] subDirs = pathToCreate.Split('/');

            string currentDir = string.Format("ftp://{0}", ftpAddress);

            foreach (string subDir in subDirs)
            {
                try
                {
                    currentDir = currentDir + "/" + subDir;
                    reqFTP = (FtpWebRequest)FtpWebRequest.Create(currentDir);
                    reqFTP.Method = WebRequestMethods.Ftp.MakeDirectory;
                    reqFTP.UseBinary = true;
                    reqFTP.Credentials = new NetworkCredential(login, password);
                    FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                    ftpStream = response.GetResponseStream();
                    ftpStream.Close();
                    response.Close();
                }
                catch (WebException ex)
                {
                    //directory already exist I know that is weak but there is no way to check if a folder exist on ftp...
                    if (ex.Response is FtpWebResponse)
                    {
                        if (((FtpWebResponse)ex.Response).StatusDescription.Contains("file already exists"))
                        {

                        }

                        else
                        {
                            throw ex;
                        }
                    }

                    else
                    {
                        throw ex;
                    }
                }
            }
        }
    }
}