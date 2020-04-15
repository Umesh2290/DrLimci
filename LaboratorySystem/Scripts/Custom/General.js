/*
All request url will be used by variable.
*/
var _Admin_Account_Login = "/Admin/Account/Login"
var _Change_Admin_Account_Role = "/Admin/Account/ChangeRole"
var _Admin_Account_SectionRestriction = "/Admin/Account/SectionRestriction"
var _Admin_Account_ForgetPassword = "/Admin/Account/ForgetPassword"
var _Admin_Account_UpdateNotification = "/Admin/Account/UpdateNotification"
var _Admin_Account_MoreNotification = "/Admin/Account/MoreNotification"
var _ChangePassword = "ChangePassword/ChangePasswordSave"
var _ChangePasswordInternal = "/Admin/Account/ChangePasswordInternal"

var _Admin_Plan_Create = "/Admin/Plan/Create"
var _Admin_Plan_Update = "/Admin/Plan/Update"
var _Admin_Plan_Get = "/Admin/Plan/Get"
var _Admin_Plan_GetList = "/Admin/Plan/GetList"
var _Admin_Plan_GetDetail = "/Admin/Plan/GetDetail"

var _Admin_Consultant_Create = "/Admin/Consultant/Create"
var _Admin_Consultant_Update = "/Admin/Consultant/Update"
var _Admin_Consultant_Get = "/Admin/Consultant/Get"
var _Admin_Consultant_GetList = "/Admin/Consultant/GetList"
var _Admin_Consultant_GetDetail = "/Admin/Consultant/GetDetail"
var _Admin_Consultant_UnblockConsultant = "/Admin/Consultant/UnblockConsultant"

var _Admin_Employee_Create = "/Admin/Employee/Create"
var _Admin_Employee_Update = "/Admin/Employee/Update"
var _Admin_Employee_Get = "/Admin/Employee/Get"
var _Admin_Employee_GetList = "/Admin/Employee/GetList"
var _Admin_Employee_GetDetail = "/Admin/Employee/GetDetail"
var _Admin_Employee_UnblockEmployee = "/Admin/Employee/UnblockEmployee"

var _Admin_Client_Create = "/Admin/Client/Create"
var _Admin_Client_Update = "/Admin/Client/Update"
var _Admin_Client_Get = "/Admin/Client/Get"
var _Admin_Client_GetList = "/Admin/Client/GetList"
var _Admin_Client_GetDetail = "/Admin/Client/GetDetail"
var _Admin_Client_UnblockClient = "/Admin/Client/UnblockClient"
var _Admin_Client_GetProviderParameters = "/Admin/Client/GetProviderParameters"
var _Admin_Client_UploadFile = "/Admin/Client/UploadFile"
var _Admin_Client_DeleteFile = "/Admin/Client/DeleteFile"

var _Admin_Payment_GetClientLicensePayments = "/Admin/Payment/GetClientLicensePayments"
var _Admin_Payment_GetClientDetail = "/Admin/Payment/GetClientDetail"
var _Admin_Payment_DownloadMultipleInvoice = "Admin/Payment/DownloadMultipleInvoice"//Deliberately Leading slash is not added

var _Admin_Subdomain_GetList = "/Admin/Subdomain/GetList"
var _Admin_Subdomain_GetDetail = "/Admin/Subdomain/GetDetail"

var _Admin_SalesAndContactRequest_GetList = "/Admin/SalesAndContactRequest/GetList"
var _Admin_SalesAndContactRequest_GetDetail = "/Admin/SalesAndContactRequest/GetDetail"
var _Admin_SalesAndContactRequest_UpdateStatus = "/Admin/SalesAndContactRequest/UpdateStatus"

var _Admin_OpinionRequest_GetList = "/Admin/OpinionRequest/GetList";
var _Admin_OpinionRequest_GetDetail = "/Admin/OpinionRequest/GetDetail";
var _Admin_OpinionRequest_UpdateStatus = "/Admin/OpinionRequest/UpdateStatus";

var _Admin_ConsultantRequest_GetList = "/Admin/ConsultantRequest/GetList";
var _Admin_ConsultantRequest_GetDetail = "/Admin/ConsultantRequest/GetDetail";
var _Admin_ConsultantRequest_UpdateStatus = "/Admin/ConsultantRequest/UpdateStatus";

var _Admin_ConsultantPayment_GetList = "/Admin/ConsultantPayment/GetList"

var _Admin_Payment_GetConsultantDoctorPayments = "/Admin/Payment/GetConsultantDoctorPayments"
var _Admin_Payment_DownloadMultipleInvoiceConsultant = "Admin/Payment/DownloadMultipleInvoiceConsultant"
var _Admin_Payment_GetConsultantOpinionDetail = "/Admin/Payment/GetConsultantOpinionDetail"

//////////////////User Side////////////////
var _User_Account_Login = "/User/Account/Login"
var _User_Account_ForgetPassword = "/User/Account/ForgetPassword"
var _Change_User_Account_Role = "/User/Account/ChangeRole"
var _User_Account_SectionRestriction = "/User/Account/SectionRestriction"
var _User_Account_UpdateNotification = "/User/Account/UpdateNotification"
var _User_Account_MoreNotification = "/User/Account/MoreNotification"
var _User_Account_ChangePasswordInternal = "/User/Account/ChangePasswordInternal"
var _User_ChangePassword = "/User/Account/ChangePasswordSave"

var _User_InventoryManager_Create = "/User/InventoryManager/Create"
var _User_InventoryManager_Update = "/User/InventoryManager/Update"
var _User_InventoryManager_Get = "/User/InventoryManager/Get"
var _User_InventoryManager_GetList = "/User/InventoryManager/GetList"
var _User_InventoryManager_GetDetail = "/User/InventoryManager/GetDetail"

var _User_InventoryRequest_RaiseCreate = "/User/InventoryRequest/RaiseCreate"
var _User_InventoryRequest_GetList = "/User/InventoryRequest/GetList"
var _User_InventoryRequest_GetDetail = "/User/InventoryRequest/GetDetail"
var _User_InventoryRequest_UpdateStatus = "/User/InventoryRequest/UpdateStatus"

var _User_OpinionRequest_CreateNew = "/User/OpinionRequest/CreateNew"
var _User_OpinionRequest_GetList = "/User/OpinionRequest/GetList"
var _User_OpinionRequest_GetDetail = "/User/OpinionRequest/GetDetail"

var _User_Patients_Create = "/User/Patients/Create"
var _User_Patients_Update = "/User/Patients/Update"
var _User_Patients_Get = "/User/Patients/Get"
var _User_Patients_GetList = "/User/Patients/GetList"
var _User_Patients_GetDetail = "/User/Patients/GetDetail"
var _User_Patients_UnblockPatient = "/User/Patients/UnblockPatient"
var _User_Patients_ForgetPassword = "/User/Patients/ForgetPassword"

var _User_Employee_Create = "/User/Employee/Create"
var _User_Employee_Update = "/User/Employee/Update"
var _User_Employee_Get = "/User/Employee/Get"
var _User_Employee_GetList = "/User/Employee/GetList"
var _User_Employee_GetDetail = "/User/Employee/GetDetail"
var _User_Employee_UnblockEmployee = "/User/Employee/UnblockEmployee"
var _User_Employee_ForgetPassword = "/User/Employee/ForgetPassword"
var _User_Employee_UploadFile = "/User/Employee/UploadFile"
var _User_Employee_DeleteFile = "/User/Employee/DeleteFile"

var _Main_ContactUs_NewRequest = "/Main/ContactUsNewRequest"

var _User_MedicalTest_GetPatientDetail = "/User/MedicalTest/GetPatientDetail"
var _User_MedicalTest_AddTestInfo = "/User/MedicalTest/AddTestInfo"
var _User_MedicalTest_AddSampleCollectionInfo = "/User/MedicalTest/AddSampleCollectionInfo"
var _User_MedicalTest_AddInvestigationInfo = "/User/MedicalTest/AddInvestigationInfo"
var _User_MedicalTest_AddConclusionInfo = "/User/MedicalTest/AddConclusionInfo"
var _User_MedicalTest_AddSupplementaryInfo = "/User/MedicalTest/AddSupplementaryInfo"
var _User_MedicalTest_GetTestDetail = "/User/MedicalTest/GetTestDetail"
var _User_MedicalTest_UploadFile = "/User/MedicalTest/UploadFile"
var _User_MedicalTest_DeleteFile = "/User/MedicalTest/DeleteFile"
var _User_MedicalTest_GetList = "/User/MedicalTest/GetList"

var Payment_GetPatientRegistration_Graph = "/User/Payments/GetGraphData"
var Payment_GetBookTest_Graph = "/User/Payments/GetGraphDataForTest"

var _User_Reports_GetList = "/User/Reports/GetList"

var _User_MyReports_GetList = "/User/MyReports/GetList"

var _User_ExtraWorkRequest_GetTestDetail = "/User/ExtraWorkRequest/GetTestDetail"
var _User_ExtraWorkRequest_UploadFile = "/User/ExtraWorkRequest/UploadFile"
var _User_ExtraWorkRequest_AddExtraWorkRequest = "/User/ExtraWorkRequest/AddExtraWorkRequest"
var _User_ExtraWorkRequest_GetList = "/User/ExtraWorkRequest/GetList"
var _User_ExtraWorkRequest_GetDetail = "/User/ExtraWorkRequest/GetDetail"
var _User_ExtraWorkRequest_UpdateStatus = "/User/ExtraWorkRequest/UpdateStatus"

var _User_ExportData_GetAllPatientRecords = "/User/ExportData/AllPatientRecords"
var _User_ExportData_DownloadAllPatientRecords = "/User/ExportData/DownloadAllPatientRecords"
var _User_ExportData_GetDetail = "/User/ExportData/GetDetail"

var _User_ExportData_GetAllTestRecords = "/User/ExportData/GetAllTestRecords"
var _User_ExportData_DownloadAllTestRecords = "/User/ExportData/DownloadAllTestRecords"

var _User_ExportData_DownloadAllPaymentRecords = "/User/ExportData/DownloadAllPaymentRecords"

var _User_ExportData_GetAllEmployeeRecords = "/User/ExportData/GetAllEmployeeRecords"
var _User_ExportData_DownloadAllEmployeeRecords = "/User/ExportData/DownloadAllEmployeeRecords"
var _User_ExportData_GetEmployeeDetail = "/User/ExportData/GetEmployeeDetail"

var _User_Hospital_Create = "/User/Hospital/Create"
var _User_Hospital_Get = "/User/Hospital/Get"
var _User_Hospital_GetDetail = "/User/Hospital/GetAllHospital"

var _User_ReportConfiguration_Get = "/User/ReportConfiguration/GetAll"



var Admin_Home_Domain_Graph = "/Admin/Home/GetDomainData"
var Admin_Home_Client_Graph = "/Admin/Home/GetClientData"
var Admin_Home_ActiveInActiveClient_Graph = "/Admin/Home/ActiveInActiveClient"
var Admin_Home_ContactRequest_Graph = "/Admin/Home/ContactRequest"
var Admin_Home_NewPendingClosed_Graph = "/Admin/Home/NewPendingClosed"
//var Payment_GetBookTest_Graph = "/User/Payments/GetGraphDataForTest"

function MakeRequest(_url, _method, _data,_datatype, _beforeSend, _success, _failed) {
    _method = typeof _method !== 'undefined' ? _method : 'GET';
    _data = typeof _data !== 'undefined' ? _data : null;
    _datatype = typeof _datatype !== 'undefined' ? _datatype : 'json';
    _beforeSend = typeof _beforeSend !== 'undefined' ? _beforeSend : _beforeSend;
    _success = typeof _success !== 'undefined' ? _success : _success;
    _failed = typeof _failed !== 'undefined' ? _failed : _failed;

    var _isbeforesendprovided = false;
    var _issuccessprovided = false;
    var _isfailedprovided = false;
    if (typeof _beforeSend !== 'undefined') {
        _isbeforesendprovided = true;
    }

    if (typeof _success !== 'undefined') {
        _issuccessprovided = true;
    }

    if (typeof _failed !== 'undefined') {
        _isfailedprovided = true;
    }

    $.ajax(_url,
{
    dataType: _datatype, // type of response data
    data: _data,
    type:_method,
    beforeSend: function (jqXHR, settings) {
        if (_isbeforesendprovided) {
            _beforeSend(jqXHR, settings);
        }
    },
    success: function (data, status, xhr) {   // success callback function
        if (_issuccessprovided) {
            _success(data, status, xhr);
        }
    },
    error: function (jqXhr, textStatus, errorMessage) { // error callback 
        if (_isfailedprovided) {
            _failed(jqXhr, textStatus, errorMessage);
        }
    }
});

}

function objectifyForm(formArray) {//serialize data function

    var returnArray = {};
    for (var i = 0; i < formArray.length; i++) {
        returnArray[formArray[i]['name']] = formArray[i]['value'];
    }
    return returnArray;
}

function objectifyFormwithExtraData(formArray,extradata) {//serialize data function

    var returnArray = {};
    for (var i = 0; i < formArray.length; i++) {
        returnArray[formArray[i]['name']] = formArray[i]['value'];
    }
    returnArray[extradata.key] = extradata.value;
    return returnArray;
}

function WebResponse(data) {

    if (data.ResponseType == "toastr-error") {
        toastr.error(data.description, data.title);
    }

    else if (data.ResponseType == "toastr-sucsess") {
        toastr.success(data.description, data.title);
    }

    else if (data.ResponseType == "toastr-info") {
        toastr.info(data.description, data.title);
    }

    else if (data.ResponseType == "toastr-warning") {
        toastr.warning(data.description, data.title);
    }

    else if (data.ResponseType == "swal-sucsess") {
        swal.fire({ title: data.title, icon: "success", html: data.description });
    }

    else if (data.ResponseType == "swal-info") {
        swal.fire({ title: data.title, icon: "info", html: data.description });
    }

    else if (data.ResponseType == "swal-warning") {
        swal.fire({ title: data.title, icon: "warning", html: data.description });
    }

    else if (data.ResponseType == "swal-error") {
        swal.fire({ title: data.title, icon: "error", html: data.description });
    }

    else if (data.ResponseType == "swal-custom") {
        
        swal.fire({ title: data.title, html: data.description });
    }

    else if (data.ResponseType == "Redirect") {
        window.location.replace(data.path);
    }

    var ac = {
        ResponseData: data.data,
        ResponseType: data.ResponseType

    }
    return ac;
}