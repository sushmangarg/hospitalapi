using HospitalManagement.Model;
using HospitalManagement.Repository.DataClass.Master;
using HospitalManagement.Repository.DataClass.Patient;
using HospitalManagement.Repository.Interface;
using HospitalManagement.Services;
using HospitalManagement_Models;
using HospitalManagement_Models.Master;
using HospitalManagement_Models.Users;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static HospitalManagement.Model.ResponseModels;

namespace HospitalManagement.Controllers
{
  
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatient _repoPatient;
        private readonly IHostingEnvironment _hostingEnvironment;
        public PatientController(IPatient _repoPatient, IHostingEnvironment _hostingEnvironment)
        {
            this._repoPatient = _repoPatient;
            this._hostingEnvironment = _hostingEnvironment;
        }

        #region Patient


        [Route("api/[Controller]/PatientLogin")]
        [HttpPost]
        public IActionResult PatientLogin(PatientModel patientModel)
        {
            clsPatient objPatient = new clsPatient();
            ResponseModels objResponse = new ResponseModels();
            string strApiName = this.ControllerContext.RouteData.Values["controller"].ToString() + "/" + this.ControllerContext.RouteData.Values["action"].ToString();
            try
            {
                PatientModel objPat = _repoPatient.PatientLogin(patientModel);
                if (objPat == null)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.ValidationFails;
                    objResponse.strMessage = "UserName/Password Not correct.";
                    objResponse.objData = "";
                    return Ok(objResponse);
                }
                else
                {
                    SystemUsersModel objSystemUsersModel = new SystemUsersModel();
                    objSystemUsersModel.intId = objPat.intId;
                    objSystemUsersModel.strEmail = objPat.strEmail;
                    objSystemUsersModel.strUserName = objPat.strUserName;
                    objSystemUsersModel.intUserType = objPat.intUserType;
                    string token = TokenManager.GenerateToken_User(objSystemUsersModel);
                    //if(patientModel.strFirebaseDeviceId != "")
                    //{
                    //    _repoPatient.UpdatePatientDeviceInfo(objPat.strFirebaseDeviceId, objPat.intDeviceTypeId, objPat.strDeviceIMEI);
                    //}
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Login Succefully";
                    objResponse.objData = objPat;
                    objResponse.strAccess_Token = token;
                    return Ok(objResponse);
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, strApiName, "", "", "");
                objResponse.intStatusCode = (int)E_RESPONSESTATUS.ExceptionOccurs;
                objResponse.strMessage = ex.Message;
                objResponse.objData = "";
                return Ok(objResponse);
            }

        }


        [Route("api/[controller]/InsertUpdatePatient")]
        [HttpPost]
        public IActionResult InsertUpdatePatient()
        {
            ResponseModels objResponse = new ResponseModels();
            string strApiName = this.ControllerContext.RouteData.Values["controller"].ToString() + "/" + this.ControllerContext.RouteData.Values["action"].ToString();
            #region Token Validation
            Microsoft.Extensions.Primitives.StringValues token;
            if (Request.Headers.Count > 0)
            {
                Request.Headers.TryGetValue("access_token", out token);
            }
            if (string.IsNullOrEmpty(token))
            {
                objResponse.intStatusCode = (int)E_RESPONSESTATUS.Unauthorized;
                objResponse.strMessage = E_RESPONSESTATUS.Unauthorized.ToString();
                return Ok(objResponse);
            }
            string strKey = clsSecretKey.SecretKey;
            UserModel oUserToken = TokenManager.GetTokenClaims_User(Convert.ToString(token).Replace("Bearer ", ""));
            if (oUserToken.intStatus == -1)
            {
                objResponse.intStatusCode = (int)E_RESPONSESTATUS.Unauthorized;
                objResponse.strMessage = E_RESPONSESTATUS.Unauthorized.ToString();
                return Ok(objResponse);
            }
            #endregion
            try
            {
                int fliesize = 1;
                var supportedTypes = new[] { ".jpg", ".pdf", ".png", ".jpeg" };


                var httpRequest = HttpContext.Request;
                var formFiles = Request.Form.Files;
                string fileName = "";
                string filetext = "";
                string filePath = "/PatientImage/";
                string fileNameExt = "";
                int result = -1;
                int IsUpdateOrInsert = 0;
                PatientModel patientModell = new PatientModel();
                patientModell.intId = Convert.ToInt32(httpRequest.Form["intId"]);
                IsUpdateOrInsert = patientModell.intId;
                patientModell.intParentId = Convert.ToInt32(httpRequest.Form["intParentId"]);
                patientModell.strFirstName = Convert.ToString(httpRequest.Form["strFirstName"]);
                patientModell.strMiddleName = Convert.ToString(httpRequest.Form["strMiddleName"]);
                patientModell.strLastName = Convert.ToString(httpRequest.Form["strLastName"]);
                patientModell.intGender = Convert.ToInt32(httpRequest.Form["intGender"]);
                patientModell.strUserName = Convert.ToString(httpRequest.Form["strUserName"]);
                patientModell.strPassword = Convert.ToString(httpRequest.Form["strPassword"]);
                patientModell.strMobileNo = Convert.ToString(httpRequest.Form["strMobileNo"]);
                patientModell.strEmail = Convert.ToString(httpRequest.Form["strEmail"]);
                patientModell.intCountryId = Convert.ToInt32(httpRequest.Form["intCountryId"]);
                patientModell.intStateId = Convert.ToInt32(httpRequest.Form["intStateId"]);
                patientModell.intCityId = Convert.ToInt32(httpRequest.Form["intCityId"]);
                //  patientModell.intCreatedBy = 1;
                patientModell.dttDOB = Convert.ToString(httpRequest.Form["dttDOB"]);
              //  patientModell.strPostalCode = Convert.ToString(httpRequest.Form["strPostalCode"]);
                patientModell.strAddress = Convert.ToString(httpRequest.Form["strAddress"]);
                patientModell.intRelationId = Convert.ToInt32(httpRequest.Form["intRelation"]);
                int IsUpdated = Convert.ToInt32(httpRequest.Form["IsUpdated"]);
                patientModell.intCreatedBy = oUserToken.intId;
                result = _repoPatient.InsertUpdatePatient(patientModell);
                if (result > 0)
                {
                    foreach (var formFile in formFiles)
                    {
                        if (formFile.Length > 0 && formFile.Length < 2150000)
                        {

                            fileName = Guid.NewGuid().ToString();
                            filetext = Path.GetExtension(formFile.FileName);
                            fileNameExt = formFile.FileName;
                            string contentType = formFile.ContentType;
                            if (!supportedTypes.Contains(filetext.ToLower()))
                            {
                                objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                                objResponse.strMessage = "File format is not Supported, Please upload a valid fle.";
                                objResponse.objData = "";
                                return Ok(objResponse);
                            }
                            if (!Directory.Exists(_hostingEnvironment.ContentRootPath + filePath + result + "/"))
                            {
                                Directory.CreateDirectory(_hostingEnvironment.ContentRootPath + filePath + result + "/");
                            }
                            using (FileStream stream = new FileStream(_hostingEnvironment.ContentRootPath + filePath + result + "/" + fileName + filetext, FileMode.Create))
                            {
                                formFile.CopyTo(stream);
                            }
                            patientModell.strImagePath = filePath + result + "/" + fileName + filetext;
                            patientModell.intId = result;
                            result = _repoPatient.InsertUpdatePatientImagePath(patientModell);
                        }
                        else
                        {
                            objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                            objResponse.strMessage = "File size should be less than 2MB.";
                            objResponse.objData = "";
                            return Ok(objResponse);
                        }
                    }
                }
                if (result > 0)
                {
                    if (IsUpdateOrInsert > 0)
                    {
                        objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                        objResponse.strMessage = "Patient Updated Successfully";
                        objResponse.objData = result;
                        return Ok(objResponse);
                    }
                    else
                    {
                        objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                        objResponse.strMessage = "Patient Inserted Successfully";
                        objResponse.objData = result;
                        return Ok(objResponse);
                    }
                }
                else if (result == -1)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.ValidationFails;
                    objResponse.strMessage = "Duplicate email";
                    objResponse.objData = "";
                    return Ok(objResponse);

                }
                else if (result == -2)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.ValidationFails;
                    objResponse.strMessage = "Duplicate Phone no";
                    objResponse.objData = "";
                    return Ok(objResponse);

                }
                else
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.ValidationFails;
                    objResponse.strMessage = "Data Not Inserted";
                    objResponse.objData = "";
                    return Ok(objResponse);
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, strApiName, "", "", "");
                objResponse.intStatusCode = (int)E_RESPONSESTATUS.ExceptionOccurs;
                objResponse.strMessage = ex.Message;
                objResponse.objData = "";
                return Ok(objResponse);
            }

        }

        [Route("api/[controller]/UpdatePatientPassword")]
        [HttpPost]
        public IActionResult UpdatePatientPassword(PatientModel patientModel)
        {
            ResponseModels objResponse = new ResponseModels();
            string strApiName = this.ControllerContext.RouteData.Values["controller"].ToString() + "/" + this.ControllerContext.RouteData.Values["action"].ToString();
            #region Token Validation
            Microsoft.Extensions.Primitives.StringValues token;
            if (Request.Headers.Count > 0)
            {
                Request.Headers.TryGetValue("access_token", out token);
            }
            if (string.IsNullOrEmpty(token))
            {
                objResponse.intStatusCode = (int)E_RESPONSESTATUS.Unauthorized;
                objResponse.strMessage = E_RESPONSESTATUS.Unauthorized.ToString();
                return Ok(objResponse);
            }
            string strKey = clsSecretKey.SecretKey;
            UserModel oUserToken = TokenManager.GetTokenClaims_User(Convert.ToString(token).Replace("Bearer ", ""));
            if (oUserToken.intStatus == -1)
            {
                objResponse.intStatusCode = (int)E_RESPONSESTATUS.Unauthorized;
                objResponse.strMessage = E_RESPONSESTATUS.Unauthorized.ToString();
                return Ok(objResponse);
            }
            #endregion
            try
            {

                int result = _repoPatient.UpdatePatientPassword(patientModel);

                if (result > 0)
                {
                        objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                        objResponse.strMessage = "Password Updated Successfully";
                        objResponse.objData = "";
                        return Ok(objResponse);                   
                }                
                else
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Current password does not match, Plase enter correct password";
                    objResponse.objData = "";
                    return Ok(objResponse);
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, strApiName, "", "", "");
                objResponse.intStatusCode = (int)E_RESPONSESTATUS.ExceptionOccurs;
                objResponse.strMessage = ex.Message;
                objResponse.objData = "";
                return Ok(objResponse);
            }

        }
        [Route("api/[controller]/GetPatient")]
        [HttpPost]
        public IActionResult GetPatient(PatientModel patientModel)
        {

            ResponseModels objResponse = new ResponseModels();
            string strApiName = this.ControllerContext.RouteData.Values["controller"].ToString() + "/" + this.ControllerContext.RouteData.Values["action"].ToString();
            #region Token Validation
            Microsoft.Extensions.Primitives.StringValues token;
            if (Request.Headers.Count > 0)
            {
                Request.Headers.TryGetValue("access_token", out token);
            }
            if (string.IsNullOrEmpty(token))
            {
                objResponse.intStatusCode = (int)E_RESPONSESTATUS.Unauthorized;
                objResponse.strMessage = E_RESPONSESTATUS.Unauthorized.ToString();
                return Ok(objResponse);
            }
            string strKey = clsSecretKey.SecretKey;
            UserModel oUserToken = TokenManager.GetTokenClaims_User(Convert.ToString(token).Replace("Bearer ", ""));
            if (oUserToken.intStatus == -1)
            {
                objResponse.intStatusCode = (int)E_RESPONSESTATUS.Unauthorized;
                objResponse.strMessage = E_RESPONSESTATUS.Unauthorized.ToString();
                return Ok(objResponse);
            }
            #endregion
            try
            {
                List<PatientModel> lstPatientModel = _repoPatient.GetPatient(patientModel.strSearch);
                if (lstPatientModel != null)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Record Fetch Succesfully";
                    objResponse.objData = lstPatientModel;
                    return Ok(objResponse);
                }
                else
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.ValidationFails;
                    objResponse.strMessage = "Record not Fetch Succesfully";
                    objResponse.objData = null;
                    return Ok(objResponse);
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, strApiName, "", "", "");
                objResponse.intStatusCode = (int)E_RESPONSESTATUS.ExceptionOccurs;
                objResponse.strMessage = ex.Message;
                objResponse.objData = "";
                return Ok(objResponse);
            }
        }

        [Route("api/[controller]/GetPatientFamily")]
        [HttpPost]
        public IActionResult GetPatientFamily(PatientModel patientModel)
        {

            ResponseModels objResponse = new ResponseModels();
            string strApiName = this.ControllerContext.RouteData.Values["controller"].ToString() + "/" + this.ControllerContext.RouteData.Values["action"].ToString();
            #region Token Validation
            Microsoft.Extensions.Primitives.StringValues token;
            if (Request.Headers.Count > 0)
            {
                Request.Headers.TryGetValue("access_token", out token);
            }
            if (string.IsNullOrEmpty(token))
            {
                objResponse.intStatusCode = (int)E_RESPONSESTATUS.Unauthorized;
                objResponse.strMessage = E_RESPONSESTATUS.Unauthorized.ToString();
                return Ok(objResponse);
            }
            string strKey = clsSecretKey.SecretKey;
            UserModel oUserToken = TokenManager.GetTokenClaims_User(Convert.ToString(token).Replace("Bearer ", ""));
            if (oUserToken.intStatus == -1)
            {
                objResponse.intStatusCode = (int)E_RESPONSESTATUS.Unauthorized;
                objResponse.strMessage = E_RESPONSESTATUS.Unauthorized.ToString();
                return Ok(objResponse);
            }
            #endregion
            try
            {
                List<PatientModel> lstPatientModel = _repoPatient.GetPatientFamily(patientModel.intParentId);
                if (lstPatientModel != null)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Record Fetch Succesfully";
                    objResponse.objData = lstPatientModel;
                    return Ok(objResponse);
                }
                else
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.ValidationFails;
                    objResponse.strMessage = "Record not Fetch Succesfully";
                    objResponse.objData = null;
                    return Ok(objResponse);
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, strApiName, "", "", "");
                objResponse.intStatusCode = (int)E_RESPONSESTATUS.ExceptionOccurs;
                objResponse.strMessage = ex.Message;
                objResponse.objData = "";
                return Ok(objResponse);
            }
        }

        [Route("api/[controller]/GetPatientAndFamily")]
        [HttpPost]
        public IActionResult GetPatientAndFamily(PatientModel patientModel)
        {

            ResponseModels objResponse = new ResponseModels();
            string strApiName = this.ControllerContext.RouteData.Values["controller"].ToString() + "/" + this.ControllerContext.RouteData.Values["action"].ToString();
            #region Token Validation
            Microsoft.Extensions.Primitives.StringValues token;
            if (Request.Headers.Count > 0)
            {
                Request.Headers.TryGetValue("access_token", out token);
            }
            if (string.IsNullOrEmpty(token))
            {
                objResponse.intStatusCode = (int)E_RESPONSESTATUS.Unauthorized;
                objResponse.strMessage = E_RESPONSESTATUS.Unauthorized.ToString();
                return Ok(objResponse);
            }
            string strKey = clsSecretKey.SecretKey;
            UserModel oUserToken = TokenManager.GetTokenClaims_User(Convert.ToString(token).Replace("Bearer ", ""));
            if (oUserToken.intStatus == -1)
            {
                objResponse.intStatusCode = (int)E_RESPONSESTATUS.Unauthorized;
                objResponse.strMessage = E_RESPONSESTATUS.Unauthorized.ToString();
                return Ok(objResponse);
            }
            #endregion
            try
            {
                List<PatientModel> lstPatientModel = _repoPatient.GetPatientAndFamily(patientModel.intParentId);
                if (lstPatientModel != null)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Record Fetch Succesfully";
                    objResponse.objData = lstPatientModel;
                    return Ok(objResponse);
                }
                else
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.ValidationFails;
                    objResponse.strMessage = "Record not Fetch Succesfully";
                    objResponse.objData = null;
                    return Ok(objResponse);
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, strApiName, "", "", "");
                objResponse.intStatusCode = (int)E_RESPONSESTATUS.ExceptionOccurs;
                objResponse.strMessage = ex.Message;
                objResponse.objData = "";
                return Ok(objResponse);
            }
        }

        [Route("api/[controller]/GetPatientsByMobileNumber")]
        [HttpPost]
        public IActionResult GetPatientsByMobileNumber([FromBody] PatientModel patientModel)
        {

            ResponseModels objResponse = new ResponseModels();
            string strApiName = this.ControllerContext.RouteData.Values["controller"].ToString() + "/" + this.ControllerContext.RouteData.Values["action"].ToString();
            #region Token Validation
            Microsoft.Extensions.Primitives.StringValues token;
            if (Request.Headers.Count > 0)
            {
                Request.Headers.TryGetValue("access_token", out token);
            }
            if (string.IsNullOrEmpty(token))
            {
                objResponse.intStatusCode = (int)E_RESPONSESTATUS.Unauthorized;
                objResponse.strMessage = E_RESPONSESTATUS.Unauthorized.ToString();
                return Ok(objResponse);
            }
            string strKey = clsSecretKey.SecretKey;
            UserModel oUserToken = TokenManager.GetTokenClaims_User(Convert.ToString(token).Replace("Bearer ", ""));
            if (oUserToken.intStatus == -1)
            {
                objResponse.intStatusCode = (int)E_RESPONSESTATUS.Unauthorized;
                objResponse.strMessage = E_RESPONSESTATUS.Unauthorized.ToString();
                return Ok(objResponse);
            }
            #endregion
            try
            {
                List<PatientModel> lstPatientModel = _repoPatient.GetPatientsByMobileNumber(patientModel.strMobileNo);
                if (lstPatientModel != null)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Record Fetch Succesfully";
                    objResponse.objData = lstPatientModel;
                    return Ok(objResponse);
                }
                else
                {   
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.ValidationFails;
                    objResponse.strMessage = "Record not Fetch Succesfully";
                    objResponse.objData = null;
                    return Ok(objResponse);
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, strApiName, "", "", "");
                objResponse.intStatusCode = (int)E_RESPONSESTATUS.ExceptionOccurs;
                objResponse.strMessage = ex.Message;
                objResponse.objData = "";
                return Ok(objResponse);
            }
        }

        [Route("api/[controller]/GetPatientById")]
        [HttpPost]
        public IActionResult GetPatientById([FromBody] PatientModel patientModel)
        {
            ResponseModels objResponse = new ResponseModels();
            string strApiName = this.ControllerContext.RouteData.Values["controller"].ToString() + "/" + this.ControllerContext.RouteData.Values["action"].ToString();
            #region Token Validation
            Microsoft.Extensions.Primitives.StringValues token;
            if (Request.Headers.Count > 0)
            {
                Request.Headers.TryGetValue("access_token", out token);
            }
            if (string.IsNullOrEmpty(token))
            {
                objResponse.intStatusCode = (int)E_RESPONSESTATUS.Unauthorized;
                objResponse.strMessage = E_RESPONSESTATUS.Unauthorized.ToString();
                return Ok(objResponse);
            }
            string strKey = clsSecretKey.SecretKey;
            UserModel oUserToken = TokenManager.GetTokenClaims_User(Convert.ToString(token).Replace("Bearer ", ""));
            if (oUserToken.intStatus == -1)
            {
                objResponse.intStatusCode = (int)E_RESPONSESTATUS.Unauthorized;
                objResponse.strMessage = E_RESPONSESTATUS.Unauthorized.ToString();
                return Ok(objResponse);
            }
            #endregion
            try
            {
                if (patientModel.intId == 0)
                {
                    patientModel.intId = oUserToken.intId;
                }
                PatientModel objPatient = _repoPatient.GetPatientById(patientModel.intId);
                if (objPatient != null)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Record Fetch Succesfully";
                    objResponse.strAccess_Token = "";
                    objResponse.objData = objPatient;
                    return Ok(objResponse);
                }
                else
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.ValidationFails;
                    objResponse.strMessage = "Record not Fetch Succesfully";
                    objResponse.strAccess_Token = "";
                    objResponse.objData = null;
                    return Ok(objResponse);
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, strApiName, "", "", "");
                objResponse.intStatusCode = (int)E_RESPONSESTATUS.ExceptionOccurs;
                objResponse.strMessage = ex.Message;
                objResponse.objData = "";
                return Ok(objResponse);
            }
        }
        [Route("api/[Controller]/DeletePatientByid")]
        [HttpPost]
        public IActionResult DeletePatientByid([FromBody] PatientModel patientModel)
        {
            ResponseModels objResponse = new ResponseModels();
            string strApiName = this.ControllerContext.RouteData.Values["controller"].ToString() + "/" + this.ControllerContext.RouteData.Values["action"].ToString();
            #region Token Validation
            Microsoft.Extensions.Primitives.StringValues token;
            if (Request.Headers.Count > 0)
            {
                Request.Headers.TryGetValue("access_token", out token);
            }
            if (string.IsNullOrEmpty(token))
            {
                objResponse.intStatusCode = (int)E_RESPONSESTATUS.Unauthorized;
                objResponse.strMessage = E_RESPONSESTATUS.Unauthorized.ToString();
                return Ok(objResponse);
            }
            string strKey = clsSecretKey.SecretKey;
            UserModel oUserToken = TokenManager.GetTokenClaims_User(Convert.ToString(token).Replace("Bearer ", ""));
            if (oUserToken.intStatus == -1)
            {
                objResponse.intStatusCode = (int)E_RESPONSESTATUS.Unauthorized;
                objResponse.strMessage = E_RESPONSESTATUS.Unauthorized.ToString();
                return Ok(objResponse);
            }
            #endregion
            try
            {
                int result = _repoPatient.DeletePatientByid(patientModel);

                if (patientModel.intId > 0)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Record Delete Succesfully";
                    objResponse.objData = "";
                    return Ok(objResponse);
                }
                else
                {

                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.ValidationFails;
                    objResponse.strMessage = "Record not Delete Succesfully";
                    objResponse.objData = null;
                    return Ok(objResponse); ;
                }
            }

            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, strApiName, "", "", "");
                objResponse.strMessage = ex.Message;
                objResponse.objData = "";
                return Ok(objResponse);
            }
        }

        #endregion

        [Route("api/[controller]/InsertUpdatePatientAppointements")]
        [HttpPost]
        public IActionResult InsertUpdatePatientAppointements([FromBody] PatientAppointmentModel objpatientAppointmentModel)
        {
            ResponseModels objResponse = new ResponseModels();
            string strApiName = this.ControllerContext.RouteData.Values["controller"].ToString() + "/" + this.ControllerContext.RouteData.Values["action"].ToString();
            #region Token Validation
            Microsoft.Extensions.Primitives.StringValues token;
            if (Request.Headers.Count > 0)
            {
                Request.Headers.TryGetValue("access_token", out token);
            }
            if (string.IsNullOrEmpty(token))
            {
                objResponse.intStatusCode = (int)E_RESPONSESTATUS.Unauthorized;
                objResponse.strMessage = E_RESPONSESTATUS.Unauthorized.ToString();
                return Ok(objResponse);
            }
            string strKey = clsSecretKey.SecretKey;
            UserModel oUserToken = TokenManager.GetTokenClaims_User(Convert.ToString(token).Replace("Bearer ", ""));
            if (oUserToken.intStatus == -1)
            {
                objResponse.intStatusCode = (int)E_RESPONSESTATUS.Unauthorized;
                objResponse.strMessage = E_RESPONSESTATUS.Unauthorized.ToString();
                return Ok(objResponse);
            }
            #endregion
            try
            {

                int result = _repoPatient.InsertUpdatePatientAppointements(objpatientAppointmentModel);

                if (result > 0)
                {
                    clsFirebaseServices objFirebaseclass = new clsFirebaseServices();
                    string strDeviceId = "ewuopyPJT6SNUZhBYQKFbC:APA91bH5bEzsfIOskuTfOW1LyVjG0V1m8YAujYUzTD6tqyJ5GXzbqz45cZonlO9kq2UpjCiGKuYZjnt-3XLRQ49JHawml5n1NT5gSEs3jAZ4D5ZGphzZ9lEvxzPMl-ixF5ZdE5GBqUDZ";

                    objFirebaseclass.SendNotificationToPatient(strDeviceId, "Consultation", "Appointment Booked", 0, "{\"intType\":0}");
                    string strDocdeviceId = "dPho_9gsQ5Ke9jIBbuc9Xo:APA91bFpKqkpWc3-daLZGxT9jdMFfAgamY3XTscRYmM9CTgBfCEwWLAY8zsPu3JMGvytIS0kw5hykPjOn08ox0oWSWv19ijN54OnhvCW0LpMQXzU3OL62aC84Vg63JgSMUucVnI9hcpN";
                    objFirebaseclass.SendNotificationToDoctor(strDocdeviceId, "Consultation", "Appointment Booked", 0, "{\"intType\":0}");

                    clsMaster objclsMaster = new clsMaster();
                    NotificationModel objNotificationModel = new NotificationModel();
                    objNotificationModel.intUserId = objpatientAppointmentModel.intPatientId;
                    objNotificationModel.intType = 1;
                    objNotificationModel.strTitle = "Consultation";
                    objNotificationModel.strBody = "Appointment Booked";
                    objNotificationModel.intActive = 1;
                    objNotificationModel.intBookingStatus = 2;
                    objclsMaster.InsertUpdateNotification(objNotificationModel);

                    NotificationModel objNotificationModelDoc = new NotificationModel();
                    objNotificationModelDoc.intUserId = objpatientAppointmentModel.intDoctorId;
                    objNotificationModelDoc.intType = 2;
                    objNotificationModelDoc.strTitle = "Consultation";
                    objNotificationModelDoc.strBody = "Appointment Booked";
                    objNotificationModelDoc.intActive = 1;
                    objNotificationModelDoc.intBookingStatus = 2;
                    objclsMaster.InsertUpdateNotification(objNotificationModelDoc);
                    if (objpatientAppointmentModel.intId > 0)
                    {
                        objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                        objResponse.strMessage = "Patient Appointment Updated Successfully";
                        objResponse.objData = result;
                        return Ok(objResponse);
                    }
                    else
                    {
                        
                        objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                        objResponse.strMessage = "Patient Appointment Inserted Successfully";
                        objResponse.objData = result;
                        return Ok(objResponse);
                    }
                }
                else
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Patient appointment already exists";
                    objResponse.objData = "";
                    return Ok(objResponse);
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, strApiName, "", "", "");
                objResponse.intStatusCode = (int)E_RESPONSESTATUS.ExceptionOccurs;
                objResponse.strMessage = ex.Message;
                objResponse.objData = "";
                return Ok(objResponse);
            }

        }


        [Route("api/[controller]/InsertReschedulePatientAppointements")]
        [HttpPost]
        public IActionResult InsertReschedulePatientAppointements([FromBody] PatientAppointmentModel objpatientAppointmentModel)
        {
            ResponseModels objResponse = new ResponseModels();
            string strApiName = this.ControllerContext.RouteData.Values["controller"].ToString() + "/" + this.ControllerContext.RouteData.Values["action"].ToString();
            #region Token Validation
            Microsoft.Extensions.Primitives.StringValues token;
            if (Request.Headers.Count > 0)
            {
                Request.Headers.TryGetValue("access_token", out token);
            }
            if (string.IsNullOrEmpty(token))
            {
                objResponse.intStatusCode = (int)E_RESPONSESTATUS.Unauthorized;
                objResponse.strMessage = E_RESPONSESTATUS.Unauthorized.ToString();
                return Ok(objResponse);
            }
            string strKey = clsSecretKey.SecretKey;
            UserModel oUserToken = TokenManager.GetTokenClaims_User(Convert.ToString(token).Replace("Bearer ", ""));
            if (oUserToken.intStatus == -1)
            {
                objResponse.intStatusCode = (int)E_RESPONSESTATUS.Unauthorized;
                objResponse.strMessage = E_RESPONSESTATUS.Unauthorized.ToString();
                return Ok(objResponse);
            }
            #endregion
            try
            {

                int result = _repoPatient.InsertReschedulePatientAppointements(objpatientAppointmentModel);

                if (result > 0)
                {
                    if (objpatientAppointmentModel.intId > 0)
                    {
                        objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                        objResponse.strMessage = "Patient Appointment Updated Successfully";
                        objResponse.objData = result;
                        return Ok(objResponse);
                    }
                    else
                    {
                        objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                        objResponse.strMessage = "Patient Appointment Inserted Successfully";
                        objResponse.objData = result;
                        return Ok(objResponse);
                    }
                }
                else
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.ValidationFails;
                    objResponse.strMessage = "Data Not Inserted";
                    objResponse.objData = "";
                    return Ok(objResponse);
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, strApiName, "", "", "");
                objResponse.intStatusCode = (int)E_RESPONSESTATUS.ExceptionOccurs;
                objResponse.strMessage = ex.Message;
                objResponse.objData = "";
                return Ok(objResponse);
            }

        }

        [Route("api/[controller]/InsertReschedulePatientAppointementsAdmin")]
        [HttpPost]
        public IActionResult InsertReschedulePatientAppointementsAdmin([FromBody] PatientAppointmentModel objpatientAppointmentModel)
        {
            ResponseModels objResponse = new ResponseModels();
            string strApiName = this.ControllerContext.RouteData.Values["controller"].ToString() + "/" + this.ControllerContext.RouteData.Values["action"].ToString();
            #region Token Validation
            Microsoft.Extensions.Primitives.StringValues token;
            if (Request.Headers.Count > 0)
            {
                Request.Headers.TryGetValue("access_token", out token);
            }
            if (string.IsNullOrEmpty(token))
            {
                objResponse.intStatusCode = (int)E_RESPONSESTATUS.Unauthorized;
                objResponse.strMessage = E_RESPONSESTATUS.Unauthorized.ToString();
                return Ok(objResponse);
            }
            string strKey = clsSecretKey.SecretKey;
            UserModel oUserToken = TokenManager.GetTokenClaims_User(Convert.ToString(token).Replace("Bearer ", ""));
            if (oUserToken.intStatus == -1)
            {
                objResponse.intStatusCode = (int)E_RESPONSESTATUS.Unauthorized;
                objResponse.strMessage = E_RESPONSESTATUS.Unauthorized.ToString();
                return Ok(objResponse);
            }
            #endregion
            try
            {

                int result = _repoPatient.InsertReschedulePatientAppointementsAdmin(objpatientAppointmentModel);

                if (result > 0)
                {
                    clsFirebaseServices objFirebaseclass = new clsFirebaseServices();
                    string strDeviceId = "ewuopyPJT6SNUZhBYQKFbC:APA91bH5bEzsfIOskuTfOW1LyVjG0V1m8YAujYUzTD6tqyJ5GXzbqz45cZonlO9kq2UpjCiGKuYZjnt-3XLRQ49JHawml5n1NT5gSEs3jAZ4D5ZGphzZ9lEvxzPMl-ixF5ZdE5GBqUDZ";

                    objFirebaseclass.SendNotificationToPatient(strDeviceId, "Consultation", "Appointment Rescheduled", 0, "{\"intType\":0}");

                    string strDocdeviceId = "dPho_9gsQ5Ke9jIBbuc9Xo:APA91bFpKqkpWc3-daLZGxT9jdMFfAgamY3XTscRYmM9CTgBfCEwWLAY8zsPu3JMGvytIS0kw5hykPjOn08ox0oWSWv19ijN54OnhvCW0LpMQXzU3OL62aC84Vg63JgSMUucVnI9hcpN";
                    objFirebaseclass.SendNotificationToDoctor(strDocdeviceId, "Consultation", "Appointment Rescheduled", 0, "{\"intType\":0}");
                    clsMaster objclsMaster = new clsMaster();
                    NotificationModel objNotificationModel = new NotificationModel();
                    objNotificationModel.intUserId = objpatientAppointmentModel.intPatientId;
                    objNotificationModel.intType = 1;
                    objNotificationModel.strTitle = "Consultation";
                    objNotificationModel.strBody = "Appointment Rescheduled";
                    objNotificationModel.intActive = 1;
                    objNotificationModel.intBookingStatus = 6;
                    objclsMaster.InsertUpdateNotification(objNotificationModel);

                    NotificationModel objNotificationModelDoc = new NotificationModel();
                    objNotificationModelDoc.intUserId = objpatientAppointmentModel.intDoctorId;
                    objNotificationModelDoc.intType = 2;
                    objNotificationModelDoc.strTitle = "Consultation";
                    objNotificationModelDoc.strBody = "Appointment Rescheduled";
                    objNotificationModelDoc.intActive = 1;
                    objNotificationModelDoc.intBookingStatus = 6;
                    objclsMaster.InsertUpdateNotification(objNotificationModelDoc);
                    if (objpatientAppointmentModel.intId > 0)
                    {
                        objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                        objResponse.strMessage = "Patient Appointment Updated Successfully";
                        objResponse.objData = result;
                        return Ok(objResponse);
                    }
                    else
                    {
                        objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                        objResponse.strMessage = "Patient Appointment Inserted Successfully";
                        objResponse.objData = result;
                        return Ok(objResponse);
                    }
                }
                else
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Data Not Inserted";
                    objResponse.objData = result;
                    return Ok(objResponse);
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, strApiName, "", "", "");
                objResponse.intStatusCode = (int)E_RESPONSESTATUS.ExceptionOccurs;
                objResponse.strMessage = ex.Message;
                objResponse.objData = "";
                return Ok(objResponse);
            }

        }

        [Route("api/[controller]/CancelPatientAppointements")]
        [HttpPost]
        public IActionResult CancelPatientAppointements([FromBody] PatientAppointmentModel objpatientAppointmentModel)
        {
            ResponseModels objResponse = new ResponseModels();
            string strApiName = this.ControllerContext.RouteData.Values["controller"].ToString() + "/" + this.ControllerContext.RouteData.Values["action"].ToString();
            #region Token Validation
            Microsoft.Extensions.Primitives.StringValues token;
            if (Request.Headers.Count > 0)
            {
                Request.Headers.TryGetValue("access_token", out token);
            }
            if (string.IsNullOrEmpty(token))
            {
                objResponse.intStatusCode = (int)E_RESPONSESTATUS.Unauthorized;
                objResponse.strMessage = E_RESPONSESTATUS.Unauthorized.ToString();
                return Ok(objResponse);
            }
            string strKey = clsSecretKey.SecretKey;
            UserModel oUserToken = TokenManager.GetTokenClaims_User(Convert.ToString(token).Replace("Bearer ", ""));
            if (oUserToken.intStatus == -1)
            {
                objResponse.intStatusCode = (int)E_RESPONSESTATUS.Unauthorized;
                objResponse.strMessage = E_RESPONSESTATUS.Unauthorized.ToString();
                return Ok(objResponse);
            }
            #endregion
            try
            {

                int result = _repoPatient.CancelPatientAppointements(objpatientAppointmentModel);

                if (result > 0)
                {
                    clsFirebaseServices objFirebaseclass = new clsFirebaseServices();
                    string strDeviceId = "ewuopyPJT6SNUZhBYQKFbC:APA91bH5bEzsfIOskuTfOW1LyVjG0V1m8YAujYUzTD6tqyJ5GXzbqz45cZonlO9kq2UpjCiGKuYZjnt-3XLRQ49JHawml5n1NT5gSEs3jAZ4D5ZGphzZ9lEvxzPMl-ixF5ZdE5GBqUDZ";
                    objFirebaseclass.SendNotificationToPatient(strDeviceId, "Consulatation", "Appointment Cancelled", 0, "{\"intType\":0}");

                    string strDocdeviceId = "dPho_9gsQ5Ke9jIBbuc9Xo:APA91bFpKqkpWc3-daLZGxT9jdMFfAgamY3XTscRYmM9CTgBfCEwWLAY8zsPu3JMGvytIS0kw5hykPjOn08ox0oWSWv19ijN54OnhvCW0LpMQXzU3OL62aC84Vg63JgSMUucVnI9hcpN";
                    objFirebaseclass.SendNotificationToDoctor(strDocdeviceId, "Consultation", "Appointment Cancelled", 0, "{\"intType\":0}");
                    clsMaster objclsMaster = new clsMaster();
                    NotificationModel objNotificationModel = new NotificationModel();
                    objNotificationModel.intUserId = objpatientAppointmentModel.intPatientId;
                    objNotificationModel.intType = 1;
                    objNotificationModel.strTitle = "Consultation";
                    objNotificationModel.strBody = "Appointment Cancelled";
                    objNotificationModel.intActive = 1;
                    objNotificationModel.intBookingStatus = 4;
                    objclsMaster.InsertUpdateNotification(objNotificationModel);

                    NotificationModel objNotificationModelDoc = new NotificationModel();
                    objNotificationModelDoc.intUserId = objpatientAppointmentModel.intDoctorId;
                    objNotificationModelDoc.intType = 2;
                    objNotificationModelDoc.strTitle = "Consultation";
                    objNotificationModelDoc.strBody = "Appointment Cancelled";
                    objNotificationModelDoc.intActive = 1;
                    objNotificationModelDoc.intBookingStatus = 4;
                    objclsMaster.InsertUpdateNotification(objNotificationModelDoc);
                    if (objpatientAppointmentModel.intId > 0)
                    {
                        objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                        objResponse.strMessage = "Patient Appointment cancelled Successfully";
                        objResponse.objData = result;
                        return Ok(objResponse);
                    }
                    else 
                    {
                        objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                        objResponse.strMessage = "Patient Appointment cancelled Successfully";
                        objResponse.objData = result;
                        return Ok(objResponse);
                    }

                }
                else
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.ValidationFails;
                    objResponse.strMessage = "Data Not cancelled";
                    objResponse.objData = "";
                    return Ok(objResponse);
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, strApiName, "", "", "");
                objResponse.intStatusCode = (int)E_RESPONSESTATUS.ExceptionOccurs;
                objResponse.strMessage = ex.Message;
                objResponse.objData = "";
                return Ok(objResponse);
            }

        }

        [Route("api/[controller]/CompletePatientAppointements")]
        [HttpPost]
        public IActionResult CompletePatientAppointements([FromBody] PatientAppointmentModel objpatientAppointmentModel)
        {
            ResponseModels objResponse = new ResponseModels();
            string strApiName = this.ControllerContext.RouteData.Values["controller"].ToString() + "/" + this.ControllerContext.RouteData.Values["action"].ToString();
            #region Token Validation
            Microsoft.Extensions.Primitives.StringValues token;
            if (Request.Headers.Count > 0)
            {
                Request.Headers.TryGetValue("access_token", out token);
            }
            if (string.IsNullOrEmpty(token))
            {
                objResponse.intStatusCode = (int)E_RESPONSESTATUS.Unauthorized;
                objResponse.strMessage = E_RESPONSESTATUS.Unauthorized.ToString();
                return Ok(objResponse);
            }
            string strKey = clsSecretKey.SecretKey;
            UserModel oUserToken = TokenManager.GetTokenClaims_User(Convert.ToString(token).Replace("Bearer ", ""));
            if (oUserToken.intStatus == -1)
            {
                objResponse.intStatusCode = (int)E_RESPONSESTATUS.Unauthorized;
                objResponse.strMessage = E_RESPONSESTATUS.Unauthorized.ToString();
                return Ok(objResponse);
            }
            #endregion
            try
            {
                clsFirebaseServices objFirebaseclass = new clsFirebaseServices();
                string strDeviceId = "ewuopyPJT6SNUZhBYQKFbC:APA91bH5bEzsfIOskuTfOW1LyVjG0V1m8YAujYUzTD6tqyJ5GXzbqz45cZonlO9kq2UpjCiGKuYZjnt-3XLRQ49JHawml5n1NT5gSEs3jAZ4D5ZGphzZ9lEvxzPMl-ixF5ZdE5GBqUDZ";
                objFirebaseclass.SendNotificationToPatient(strDeviceId, "Consulatation", "Appointment Completed", 0, "{\"intType\":0}");
                int result = _repoPatient.CompletePatientAppointements(objpatientAppointmentModel);
                string strDocdeviceId = "dPho_9gsQ5Ke9jIBbuc9Xo:APA91bFpKqkpWc3-daLZGxT9jdMFfAgamY3XTscRYmM9CTgBfCEwWLAY8zsPu3JMGvytIS0kw5hykPjOn08ox0oWSWv19ijN54OnhvCW0LpMQXzU3OL62aC84Vg63JgSMUucVnI9hcpN";
                objFirebaseclass.SendNotificationToDoctor(strDocdeviceId, "Consultation", "Appointment Completed", 0, "{\"intType\":0}");

                clsMaster objclsMaster = new clsMaster();
                NotificationModel objNotificationModel = new NotificationModel();
                objNotificationModel.intUserId = objpatientAppointmentModel.intPatientId;
                objNotificationModel.intType = 1;
                objNotificationModel.strTitle = "Consultation";
                objNotificationModel.strBody = "Appointment Completed";
                objNotificationModel.intActive = 1;
                objNotificationModel.intBookingStatus = 3;
                objclsMaster.InsertUpdateNotification(objNotificationModel);

                NotificationModel objNotificationModelDoc = new NotificationModel();
                objNotificationModelDoc.intUserId = objpatientAppointmentModel.intDoctorId;
                objNotificationModelDoc.intType = 2;
                objNotificationModelDoc.strTitle = "Consultation";
                objNotificationModelDoc.strBody = "Appointment Completed";
                objNotificationModelDoc.intActive = 1;
                objNotificationModelDoc.intBookingStatus = 3;
                objclsMaster.InsertUpdateNotification(objNotificationModelDoc);
                if (result > 0)
                {
                    
                    if (objpatientAppointmentModel.intId > 0)
                    {
                        objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                        objResponse.strMessage = "Patient Appointment Completed Successfully";
                        objResponse.objData = result;
                        return Ok(objResponse);
                    }
                    else
                    {
                        objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                        objResponse.strMessage = "Patient Appointment Inserted Successfully";
                        objResponse.objData = result;
                        return Ok(objResponse);
                    }
                }
                else
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.ValidationFails;
                    objResponse.strMessage = "Data Not Inserted";
                    objResponse.objData = "";
                    return Ok(objResponse);
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, strApiName, "", "", "");
                objResponse.intStatusCode = (int)E_RESPONSESTATUS.ExceptionOccurs;
                objResponse.strMessage = ex.Message;
                objResponse.objData = "";
                return Ok(objResponse);
            }

        }

        [Route("api/[controller]/GetAllAppointements")]
        [HttpPost]
        public IActionResult GetAllAppointements()
        {
            ResponseModels objResponse = new ResponseModels();
            string strApiName = this.ControllerContext.RouteData.Values["controller"].ToString() + "/" + this.ControllerContext.RouteData.Values["action"].ToString();
            #region Token Validation
            Microsoft.Extensions.Primitives.StringValues token;
            if (Request.Headers.Count > 0)
            {
                Request.Headers.TryGetValue("access_token", out token);
            }
            if (string.IsNullOrEmpty(token))
            {
                objResponse.intStatusCode = (int)E_RESPONSESTATUS.Unauthorized;
                objResponse.strMessage = E_RESPONSESTATUS.Unauthorized.ToString();
                return Ok(objResponse);
            }
            string strKey = clsSecretKey.SecretKey;
            UserModel oUserToken = TokenManager.GetTokenClaims_User(Convert.ToString(token).Replace("Bearer ", ""));
            if (oUserToken.intStatus == -1)
            {
                objResponse.intStatusCode = (int)E_RESPONSESTATUS.Unauthorized;
                objResponse.strMessage = E_RESPONSESTATUS.Unauthorized.ToString();
                return Ok(objResponse);
            }
            #endregion

            try
            {


                List<AppointementsModel> objAppointementsModel = _repoPatient.GetAllAppointements();
                if (objAppointementsModel != null)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Record Fetch Succesfully";
                    objResponse.strAccess_Token = "";
                    objResponse.objData = objAppointementsModel;
                    return Ok(objResponse);
                }
                else
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.ValidationFails;
                    objResponse.strMessage = "Record not Fetch Succesfully";
                    objResponse.strAccess_Token = "";
                    objResponse.objData = null;
                    return Ok(objResponse);
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, strApiName, "", "", "");
                objResponse.intStatusCode = (int)E_RESPONSESTATUS.ExceptionOccurs;
                objResponse.strMessage = ex.Message;
                objResponse.objData = "";
                return Ok(objResponse);
            }
        }


        [Route("api/[controller]/GetPatientAppointmentsByintDoctorId")]
        [HttpPost]
        public IActionResult GetPatientAppointmentsByintDoctorId([FromBody] PatientAppointmentModel patientAppointmentsModel)
        {
            ResponseModels objResponse = new ResponseModels();
            string strApiName = this.ControllerContext.RouteData.Values["controller"].ToString() + "/" + this.ControllerContext.RouteData.Values["action"].ToString();
            #region Token Validation
            Microsoft.Extensions.Primitives.StringValues token;
            if (Request.Headers.Count > 0)
            {
                Request.Headers.TryGetValue("access_token", out token);
            }
            if (string.IsNullOrEmpty(token))
            {
                objResponse.intStatusCode = (int)E_RESPONSESTATUS.Unauthorized;
                objResponse.strMessage = E_RESPONSESTATUS.Unauthorized.ToString();
                return Ok(objResponse);
            }
            string strKey = clsSecretKey.SecretKey;
            UserModel oUserToken = TokenManager.GetTokenClaims_User(Convert.ToString(token).Replace("Bearer ", ""));
            if (oUserToken.intStatus == -1)
            {
                objResponse.intStatusCode = (int)E_RESPONSESTATUS.Unauthorized;
                objResponse.strMessage = E_RESPONSESTATUS.Unauthorized.ToString();
                return Ok(objResponse);
            }
            #endregion

            try
            {

             
                List<PatientAppointmentModel> objPatientAppointments = _repoPatient.GetPatientAppointmentsByintDoctorId(patientAppointmentsModel.intDoctorId, patientAppointmentsModel.intStatusId);
                if (objPatientAppointments != null)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Record Fetch Succesfully";
                    objResponse.strAccess_Token = "";
                    objResponse.objData = objPatientAppointments;
                    return Ok(objResponse);
                }
                else
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.ValidationFails;
                    objResponse.strMessage = "Record not Fetch Succesfully";
                    objResponse.strAccess_Token = "";
                    objResponse.objData = null;
                    return Ok(objResponse);
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, strApiName, "", "", "");
                objResponse.intStatusCode = (int)E_RESPONSESTATUS.ExceptionOccurs;
                objResponse.strMessage = ex.Message;
                objResponse.objData = "";
                return Ok(objResponse);
            }
        }
        [Route("api/[controller]/GetPatientAppointmentsByintPatientId")]
        [HttpPost]
        public IActionResult GetPatientAppointmentsByintPatientId([FromBody] PatientAppointmentModel patientAppointmentsModel)
        {
            ResponseModels objResponse = new ResponseModels();
            string strApiName = this.ControllerContext.RouteData.Values["controller"].ToString() + "/" + this.ControllerContext.RouteData.Values["action"].ToString();
            #region Token Validation
            Microsoft.Extensions.Primitives.StringValues token;
            if (Request.Headers.Count > 0)
            {
                Request.Headers.TryGetValue("access_token", out token);
            }
            if (string.IsNullOrEmpty(token))
            {
                objResponse.intStatusCode = (int)E_RESPONSESTATUS.Unauthorized;
                objResponse.strMessage = E_RESPONSESTATUS.Unauthorized.ToString();
                return Ok(objResponse);
            }
            string strKey = clsSecretKey.SecretKey;
            UserModel oUserToken = TokenManager.GetTokenClaims_User(Convert.ToString(token).Replace("Bearer ", ""));
            if (oUserToken.intStatus == -1)
            {
                objResponse.intStatusCode = (int)E_RESPONSESTATUS.Unauthorized;
                objResponse.strMessage = E_RESPONSESTATUS.Unauthorized.ToString();
                return Ok(objResponse);
            }
            #endregion

            try
            {
                //Here We are finiding the Dcotor Id from token Which is requirement from APP.
                if (patientAppointmentsModel.intPatientId == 0)
                {
                    patientAppointmentsModel.intPatientId = oUserToken.intId;
                }
                List<PatientAppointmentModel> objPatientAppointments = _repoPatient.GetPatientAppointmentsByintPatientId(patientAppointmentsModel.intPatientId,patientAppointmentsModel.intStatusId);
                if (objPatientAppointments != null)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Record Fetch Succesfully";
                    objResponse.strAccess_Token = "";
                    objResponse.objData = objPatientAppointments;
                    return Ok(objResponse);
                }
                else
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.ValidationFails;
                    objResponse.strMessage = "Record not Fetch Succesfully";
                    objResponse.strAccess_Token = "";
                    objResponse.objData = null;
                    return Ok(objResponse);
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, strApiName, "", "", "");
                objResponse.intStatusCode = (int)E_RESPONSESTATUS.ExceptionOccurs;
                objResponse.strMessage = ex.Message;
                objResponse.objData = "";
                return Ok(objResponse);
            }
        }

        [Route("api/[controller]/GetPatientByDocIdSlotId")]
        [HttpPost]
        public IActionResult GetPatientByDocIdSlotId([FromBody] PatientModel patientModel)
        {

            ResponseModels objResponse = new ResponseModels();
            string strApiName = this.ControllerContext.RouteData.Values["controller"].ToString() + "/" + this.ControllerContext.RouteData.Values["action"].ToString();
            #region Token Validation
            Microsoft.Extensions.Primitives.StringValues token;
            if (Request.Headers.Count > 0)
            {
                Request.Headers.TryGetValue("access_token", out token);
            }
            if (string.IsNullOrEmpty(token))
            {
                objResponse.intStatusCode = (int)E_RESPONSESTATUS.Unauthorized;
                objResponse.strMessage = E_RESPONSESTATUS.Unauthorized.ToString();
                return Ok(objResponse);
            }
            string strKey = clsSecretKey.SecretKey;
            UserModel oUserToken = TokenManager.GetTokenClaims_User(Convert.ToString(token).Replace("Bearer ", ""));
            if (oUserToken.intStatus == -1)
            {
                objResponse.intStatusCode = (int)E_RESPONSESTATUS.Unauthorized;
                objResponse.strMessage = E_RESPONSESTATUS.Unauthorized.ToString();
                return Ok(objResponse);
            }
            #endregion
            try
            {
                List<PatientModel> lstPatientModel = _repoPatient.GetPatientByDocIdSlotId(patientModel.intAppointmentSlot);
                if (lstPatientModel != null)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Record Fetch Succesfully";
                    objResponse.objData = lstPatientModel;
                    return Ok(objResponse);
                }
                else
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.ValidationFails;
                    objResponse.strMessage = "Record not Fetch Succesfully";
                    objResponse.objData = null;
                    return Ok(objResponse);
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, strApiName, "", "", "");
                objResponse.intStatusCode = (int)E_RESPONSESTATUS.ExceptionOccurs;
                objResponse.strMessage = ex.Message;
                objResponse.objData = "";
                return Ok(objResponse);
            }
        }

        [Route("api/[controller]/GetAllPatientByDocIdSlotId")]
        [HttpPost]
        public IActionResult GetAllPatientByDocIdSlotId([FromBody] PatientModel patientModel)
        {

            ResponseModels objResponse = new ResponseModels();
            string strApiName = this.ControllerContext.RouteData.Values["controller"].ToString() + "/" + this.ControllerContext.RouteData.Values["action"].ToString();
            #region Token Validation
            Microsoft.Extensions.Primitives.StringValues token;
            if (Request.Headers.Count > 0)
            {
                Request.Headers.TryGetValue("access_token", out token);
            }
            if (string.IsNullOrEmpty(token))
            {
                objResponse.intStatusCode = (int)E_RESPONSESTATUS.Unauthorized;
                objResponse.strMessage = E_RESPONSESTATUS.Unauthorized.ToString();
                return Ok(objResponse);
            }
            string strKey = clsSecretKey.SecretKey;
            UserModel oUserToken = TokenManager.GetTokenClaims_User(Convert.ToString(token).Replace("Bearer ", ""));
            if (oUserToken.intStatus == -1)
            {
                objResponse.intStatusCode = (int)E_RESPONSESTATUS.Unauthorized;
                objResponse.strMessage = E_RESPONSESTATUS.Unauthorized.ToString();
                return Ok(objResponse);
            }
            #endregion
            try
            {
                List<PatientModel> lstPatientModel = _repoPatient.GetAllPatientByDocIdSlotId(patientModel.intAppointmentSlot);
                if (lstPatientModel != null)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Record Fetch Succesfully";
                    objResponse.objData = lstPatientModel;
                    return Ok(objResponse);
                }
                else
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.ValidationFails;
                    objResponse.strMessage = "Record not Fetch Succesfully";
                    objResponse.objData = null;
                    return Ok(objResponse);
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, strApiName, "", "", "");
                objResponse.intStatusCode = (int)E_RESPONSESTATUS.ExceptionOccurs;
                objResponse.strMessage = ex.Message;
                objResponse.objData = "";
                return Ok(objResponse);
            }
        }
        [Route("api/[controller]/InsertUpdatePatientHealthCheckUp")]
        [HttpPost]
        public IActionResult InsertUpdatePatientHealthCheckUp([FromBody] PatientModel patientModel)
        {

            ResponseModels objResponse = new ResponseModels();
            string strApiName = this.ControllerContext.RouteData.Values["controller"].ToString() + "/" + this.ControllerContext.RouteData.Values["action"].ToString();
            #region Token Validation
            Microsoft.Extensions.Primitives.StringValues token;
            if (Request.Headers.Count > 0)
            {
                Request.Headers.TryGetValue("access_token", out token);
            }
            if (string.IsNullOrEmpty(token))
            {
                objResponse.intStatusCode = (int)E_RESPONSESTATUS.Unauthorized;
                objResponse.strMessage = E_RESPONSESTATUS.Unauthorized.ToString();
                return Ok(objResponse);
            }
            string strKey = clsSecretKey.SecretKey;
            UserModel oUserToken = TokenManager.GetTokenClaims_User(Convert.ToString(token).Replace("Bearer ", ""));
            if (oUserToken.intStatus == -1)
            {
                objResponse.intStatusCode = (int)E_RESPONSESTATUS.Unauthorized;
                objResponse.strMessage = E_RESPONSESTATUS.Unauthorized.ToString();
                return Ok(objResponse);
            }
            #endregion
            try
            {
                patientModel.intCreatedBy = oUserToken.intId;
               int result = _repoPatient.InsertUpdatePatientHealthCheckUp(patientModel);
                if (result>0)
                {
                    if (patientModel.intPatientHealthId == 0)
                    {
                        objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                        objResponse.strMessage = "Record saved Succesfully";
                        objResponse.objData = result;
                        return Ok(objResponse);
                    }
                    else
                    {
                        objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                        objResponse.strMessage = "Record updated Succesfully";
                        objResponse.objData = result;
                        return Ok(objResponse);
                    }
                }
                else
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.ValidationFails;
                    objResponse.strMessage = "Record do not saved";
                    objResponse.objData = null;
                    return Ok(objResponse);
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, strApiName, "", "", "");
                objResponse.intStatusCode = (int)E_RESPONSESTATUS.ExceptionOccurs;
                objResponse.strMessage = ex.Message;
                objResponse.objData = "";
                return Ok(objResponse);
            }
        }

        [Route("api/[controller]/GetPatientHealthCheckUpByPatientId")]
        [HttpPost]
        public IActionResult GetPatientHealthCheckUpByPatientId([FromBody] PatientModel patientModel)
        {

            ResponseModels objResponse = new ResponseModels();
            string strApiName = this.ControllerContext.RouteData.Values["controller"].ToString() + "/" + this.ControllerContext.RouteData.Values["action"].ToString();
            #region Token Validation
            Microsoft.Extensions.Primitives.StringValues token;
            if (Request.Headers.Count > 0)
            {
                Request.Headers.TryGetValue("access_token", out token);
            }
            if (string.IsNullOrEmpty(token))
            {
                objResponse.intStatusCode = (int)E_RESPONSESTATUS.Unauthorized;
                objResponse.strMessage = E_RESPONSESTATUS.Unauthorized.ToString();
                return Ok(objResponse);
            }
            string strKey = clsSecretKey.SecretKey;
            UserModel oUserToken = TokenManager.GetTokenClaims_User(Convert.ToString(token).Replace("Bearer ", ""));
            if (oUserToken.intStatus == -1)
            {
                objResponse.intStatusCode = (int)E_RESPONSESTATUS.Unauthorized;
                objResponse.strMessage = E_RESPONSESTATUS.Unauthorized.ToString();
                return Ok(objResponse);
            }
            #endregion
            try
            {
                List<PatientModel> lstPatientModel = _repoPatient.GetPatientHealthCheckUpByPatientId(patientModel.intId);
                if (lstPatientModel != null)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Record Fetch Succesfully";
                    objResponse.objData = lstPatientModel;
                    return Ok(objResponse);
                }
                else
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.ValidationFails;
                    objResponse.strMessage = "Record not Fetch Succesfully";
                    objResponse.objData = null;
                    return Ok(objResponse);
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, strApiName, "", "", "");
                objResponse.intStatusCode = (int)E_RESPONSESTATUS.ExceptionOccurs;
                objResponse.strMessage = ex.Message;
                objResponse.objData = "";
                return Ok(objResponse);
            }
        }

        [Route("api/[controller]/DeletePatientHealthCheckByPatientId")]
        [HttpPost]
        public IActionResult DeletePatientHealthCheckByPatientId([FromBody] PatientModel patientModel)
        {

            ResponseModels objResponse = new ResponseModels();
            string strApiName = this.ControllerContext.RouteData.Values["controller"].ToString() + "/" + this.ControllerContext.RouteData.Values["action"].ToString();
            #region Token Validation
            Microsoft.Extensions.Primitives.StringValues token;
            if (Request.Headers.Count > 0)
            {
                Request.Headers.TryGetValue("access_token", out token);
            }
            if (string.IsNullOrEmpty(token))
            {
                objResponse.intStatusCode = (int)E_RESPONSESTATUS.Unauthorized;
                objResponse.strMessage = E_RESPONSESTATUS.Unauthorized.ToString();
                return Ok(objResponse);
            }
            string strKey = clsSecretKey.SecretKey;
            UserModel oUserToken = TokenManager.GetTokenClaims_User(Convert.ToString(token).Replace("Bearer ", ""));
            if (oUserToken.intStatus == -1)
            {
                objResponse.intStatusCode = (int)E_RESPONSESTATUS.Unauthorized;
                objResponse.strMessage = E_RESPONSESTATUS.Unauthorized.ToString();
                return Ok(objResponse);
            }
            #endregion
            try
            {
                int result = _repoPatient.DeletePatientHealthCheckByPatientId(patientModel.intId);
                if (result>0)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Record Deleted Succesfully";
                    objResponse.objData = result;
                    return Ok(objResponse);
                }
                else
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.ValidationFails;
                    objResponse.strMessage = "Record can be deleted";
                    objResponse.objData = null;
                    return Ok(objResponse);
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, strApiName, "", "", "");
                objResponse.intStatusCode = (int)E_RESPONSESTATUS.ExceptionOccurs;
                objResponse.strMessage = ex.Message;
                objResponse.objData = "";
                return Ok(objResponse);
            }
        }
        [Route("api/[controller]/GetPatientHealthCheckUpByPatientHealthId")]
        [HttpPost]
        public IActionResult GetPatientHealthCheckUpByPatientHealthId([FromBody] PatientModel patientModel)
        {

            ResponseModels objResponse = new ResponseModels();
            string strApiName = this.ControllerContext.RouteData.Values["controller"].ToString() + "/" + this.ControllerContext.RouteData.Values["action"].ToString();
            #region Token Validation
            Microsoft.Extensions.Primitives.StringValues token;
            if (Request.Headers.Count > 0)
            {
                Request.Headers.TryGetValue("access_token", out token);
            }
            if (string.IsNullOrEmpty(token))
            {
                objResponse.intStatusCode = (int)E_RESPONSESTATUS.Unauthorized;
                objResponse.strMessage = E_RESPONSESTATUS.Unauthorized.ToString();
                return Ok(objResponse);
            }
            string strKey = clsSecretKey.SecretKey;
            UserModel oUserToken = TokenManager.GetTokenClaims_User(Convert.ToString(token).Replace("Bearer ", ""));
            if (oUserToken.intStatus == -1)
            {
                objResponse.intStatusCode = (int)E_RESPONSESTATUS.Unauthorized;
                objResponse.strMessage = E_RESPONSESTATUS.Unauthorized.ToString();
                return Ok(objResponse);
            }
            #endregion
            try
            {
                PatientModel objPatientModel = _repoPatient.GetPatientHealthCheckUpByPatientHealthId(patientModel.intPatientHealthId);
                if (objPatientModel != null)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Record Fetch Succesfully";
                    objResponse.objData = objPatientModel;
                    return Ok(objResponse);
                }
                else
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.ValidationFails;
                    objResponse.strMessage = "Record not Fetch Succesfully";
                    objResponse.objData = null;
                    return Ok(objResponse);
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, strApiName, "", "", "");
                objResponse.intStatusCode = (int)E_RESPONSESTATUS.ExceptionOccurs;
                objResponse.strMessage = ex.Message;
                objResponse.objData = "";
                return Ok(objResponse);
            }
        }

        [Route("api/[controller]/GetPatientAppointmentsHistoryByintPatientAppointmentId")]
        [HttpPost]
        public IActionResult GetPatientAppointmentsHistoryByintPatientAppointmentId([FromBody] PatientAppointmentModel patientAppointmentsModel)
        {
            ResponseModels objResponse = new ResponseModels();
            string strApiName = this.ControllerContext.RouteData.Values["controller"].ToString() + "/" + this.ControllerContext.RouteData.Values["action"].ToString();
            #region Token Validation
            Microsoft.Extensions.Primitives.StringValues token;
            if (Request.Headers.Count > 0)
            {
                Request.Headers.TryGetValue("access_token", out token);
            }
            if (string.IsNullOrEmpty(token))
            {
                objResponse.intStatusCode = (int)E_RESPONSESTATUS.Unauthorized;
                objResponse.strMessage = E_RESPONSESTATUS.Unauthorized.ToString();
                return Ok(objResponse);
            }
            string strKey = clsSecretKey.SecretKey;
            UserModel oUserToken = TokenManager.GetTokenClaims_User(Convert.ToString(token).Replace("Bearer ", ""));
            if (oUserToken.intStatus == -1)
            {
                objResponse.intStatusCode = (int)E_RESPONSESTATUS.Unauthorized;
                objResponse.strMessage = E_RESPONSESTATUS.Unauthorized.ToString();
                return Ok(objResponse);
            }
            #endregion

            try
            {

                List<PatientAppointmentModel> objPatientAppointments = _repoPatient.GetPatientAppointmentsHistoryByintPatientAppointmentId(patientAppointmentsModel.intId);
                if (objPatientAppointments != null)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Record Fetch Succesfully";
                    objResponse.strAccess_Token = "";
                    objResponse.objData = objPatientAppointments;
                    return Ok(objResponse);
                }
                else
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.ValidationFails;
                    objResponse.strMessage = "Record not Fetch Succesfully";
                    objResponse.strAccess_Token = "";
                    objResponse.objData = null;
                    return Ok(objResponse);
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, strApiName, "", "", "");
                objResponse.intStatusCode = (int)E_RESPONSESTATUS.ExceptionOccurs;
                objResponse.strMessage = ex.Message;
                objResponse.objData = "";
                return Ok(objResponse);
            }
        }

    }
}
