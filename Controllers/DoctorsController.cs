using HospitalManagement.Model;
using HospitalManagement.Repository.DataClass.Doctors;
using HospitalManagement.Repository.Interface;
using HospitalManagement.Security;
using HospitalManagement_Models.Doctors;
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
    public class DoctorsController : ControllerBase
    {
        private readonly IDoctors _repoDoctors;
        private readonly IHostingEnvironment _hostingEnvironment;

        public DoctorsController(IDoctors _repoDoctors, IHostingEnvironment _hostingEnvironment)
        {
            this._repoDoctors = _repoDoctors;
            this._hostingEnvironment = _hostingEnvironment;
        }
        #region DoctorsLogin

        [Route("api/[Controller]/DoctorsLogin")]
        [HttpPost]
        public IActionResult DoctorsLogin(DoctorsModel doctorsModel)

        {
            clsDoctor objDoctor = new clsDoctor();
            ResponseModels objResponse = new ResponseModels();
            UserModel usertoken = new UserModel();
            string strKey = clsSecretKey.SecretKey;
            string strApiName = this.ControllerContext.RouteData.Values["controller"].ToString() + "/" + this.ControllerContext.RouteData.Values["action"].ToString();
            try
            {
                DoctorsModel objDoctors = objDoctor.DoctorsLogin(doctorsModel);
                if (objDoctors == null)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.ValidationFails;
                    objResponse.strMessage = "UserName/Password Not correct.";
                    objResponse.objData = "";
                    return Ok(objResponse);
                }
                else if(objDoctors.intActive==0)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.ValidationFails;
                    objResponse.strMessage = "Your account is inactive";
                    objResponse.objData = "";
                    return Ok(objResponse);                    
                }
                else
                {
                    SystemUsersModel objSystemUsersModel = new SystemUsersModel();
                    objSystemUsersModel.intId = objDoctors.intId;
                    objSystemUsersModel.strEmail = objDoctors.strEmail;
                    objSystemUsersModel.strUserName = objDoctors.strUserName;
                    objSystemUsersModel.intUserType = objDoctors.intUserType;
                    string token = TokenManager.GenerateToken_User(objSystemUsersModel);
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Login Succefully";
                    objResponse.objData = objDoctors;
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

        #endregion

        #region Doctors
        [Route("api/[controller]/InsertUpdateDoctors")]
        [HttpPost]
        public IActionResult InsertUpdateDoctors()
        {
            ResponseModels objResponse = new ResponseModels();
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
            string strApiName = this.ControllerContext.RouteData.Values["controller"].ToString() + "/" + this.ControllerContext.RouteData.Values["action"].ToString();
            try
            {
                int fliesize = 1;
                var supportedTypes = new[] { ".jpg", ".pdf", ".png", ".jpeg" };


                var httpRequest = HttpContext.Request;
                var formFiles = Request.Form.Files;
                string fileName = "";
                string filetext = "";
                string filePath = "/DocImage/";
                string fileNameExt = "";
                int result = -1;
                int IsUpdateOrInsert = 0;
                DoctorsModel doctorsModel = new DoctorsModel();
                doctorsModel.intId = Convert.ToInt32(httpRequest.Form["intId"]);
                IsUpdateOrInsert = doctorsModel.intId;
                doctorsModel.strFirstName = Convert.ToString(httpRequest.Form["strFirstName"]);
                doctorsModel.strMiddleName = Convert.ToString(httpRequest.Form["strMiddleName"]);
                doctorsModel.strLastName = Convert.ToString(httpRequest.Form["strLastName"]);
                doctorsModel.intGender = Convert.ToInt32(httpRequest.Form["intGender"]);
                doctorsModel.strUserName = Convert.ToString(httpRequest.Form["strUserName"]);
                doctorsModel.strPassword = Convert.ToString(httpRequest.Form["strPassword"]);
                doctorsModel.strMobileNo = Convert.ToString(httpRequest.Form["strMobileNo"]);
                doctorsModel.strEmail = Convert.ToString(httpRequest.Form["strEmail"]);
                doctorsModel.intCountryId = Convert.ToInt32(httpRequest.Form["intCountryId"]);
                doctorsModel.intStateId = Convert.ToInt32(httpRequest.Form["intStateId"]);
                doctorsModel.intCityId = Convert.ToInt32(httpRequest.Form["intCityId"]);
                doctorsModel.intCreatedBy = 1;
                doctorsModel.intActive = Convert.ToInt32(httpRequest.Form["intActive"]);
            //    doctorsModel.ddtDOBDate = Convert.ToDateTime(httpRequest.Form["dttDOB"]);
                doctorsModel.dttDOB = Convert.ToString(httpRequest.Form["dttDOB"]);
                doctorsModel.intSpecialityId = Convert.ToInt32(httpRequest.Form["intSpecialityId"]);
                doctorsModel.strDesignation = Convert.ToString(httpRequest.Form["strDesignation"]);
                doctorsModel.strPostalCode = Convert.ToString(httpRequest.Form["strPostalCode"]);
                doctorsModel.strAddress = Convert.ToString(httpRequest.Form["strAddress"]);
                doctorsModel.strBiography = Convert.ToString(httpRequest.Form["strBiography"]);
                doctorsModel.strEducation = Convert.ToString(httpRequest.Form["strEducation"]);
                int IsUpdated = Convert.ToInt32(httpRequest.Form["IsUpdated"]);
                doctorsModel.intCreatedBy = oUserToken.intId;
                result = _repoDoctors.InsertUpdateDoctors(doctorsModel);
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
                                objResponse.strMessage = "File format is not Supported, Please upload a valid file.";
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
                            doctorsModel.strImagePath = filePath + result + "/" + fileName + filetext;
                            doctorsModel.intId = result;
                            int imageresult = _repoDoctors.InsertUpdateDoctorsImagePath(doctorsModel);
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
                        objResponse.strMessage = "Doctors Updated Successfully";
                        objResponse.objData = result;
                        return Ok(objResponse);
                    }
                    else
                    {
                        objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                        objResponse.strMessage = "Doctors Added Successfully";
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
                    objResponse.strMessage = "Duplicate phone no";
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
        [Route("api/[controller]/GetDoctors")]
        [HttpPost]
        public IActionResult GetDoctors([FromBody] DoctorsModel objDoctors)
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
                List<DoctorsModel> lstDoctorsModel = _repoDoctors.GetDoctors(objDoctors);
                if (lstDoctorsModel != null)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Record Fetch Succesfully";
                    objResponse.objData = lstDoctorsModel;
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
        [Route("api/[controller]/GetDoctorsById")]
        [HttpPost]
        public IActionResult GetDoctorsById([FromBody] DoctorsModel doctorsModel)
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
                if (doctorsModel.intId == 0)
                {
                    doctorsModel.intId = oUserToken.intId;
                }
                DoctorsModel objDoctors = _repoDoctors.GetDoctorsById(doctorsModel.intId);
                if (objDoctors != null)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Record Fetch Succesfully";
                    objResponse.strAccess_Token = "";
                    objResponse.objData = objDoctors;
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
        [Route("api/[Controller]/DeleteDoctorsByid")]
        [HttpPost]
        public IActionResult DeleteDoctorsByid([FromBody] DoctorsModel doctorsModel)
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
                int result = _repoDoctors.DeleteDoctorsByid(doctorsModel);

                if (doctorsModel.intId > 0)
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
        [Route("api/[controller]/UpdateDoctorPassword")]
        [HttpPost]
        public IActionResult UpdateDocotrPassword(DoctorsModel doctorsModel)
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

                int result = _repoDoctors.UpdateDocotrPassword(doctorsModel);

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

        [Route("api/[controller]/GetDoctorsWithSpeciality")]
        [HttpPost]
        public IActionResult GetDoctorsWithSpeciality([FromBody] DoctorsModel objDoctors)
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
                List<DoctorsModel> lstDoctorsModel = _repoDoctors.GetDoctorsWithSpeciality(objDoctors);
                if (lstDoctorsModel != null)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Record Fetch Succesfully";
                    objResponse.objData = lstDoctorsModel;
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

        #endregion

        #region DoctorsAvailability
        [Route("api/[controller]/InsertUpdateDoctorsAvailability")]
        [HttpPost]
        public IActionResult InsertUpdateDoctorsAvailability([FromBody] DoctorsAvailabilityModel doctorsavailabilityModel)
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
                doctorsavailabilityModel.intCreatedBy = oUserToken.intId;
                int result = _repoDoctors.InsertUpdateDoctorsAvailability(doctorsavailabilityModel);

                if (result > 0)
                {
                    if (doctorsavailabilityModel.intId > 0)
                    {
                        objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                        objResponse.strMessage = "DoctorsAvailablity Updated Successfully";
                        objResponse.objData = "";
                        return Ok(objResponse);
                    }
                    else
                    {
                        objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                        objResponse.strMessage = "DoctorsAvailablity Inserted Successfully";
                        objResponse.objData = "";
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

        [Route("api/[controller]/InsertMultipleDoctorsAvailabilityByDate")]
        [HttpPost]
        public IActionResult InsertMultipleDoctorsAvailabilityByDate([FromBody] DoctorsAvailabilityModel doctorsavailabilityModel)
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
                doctorsavailabilityModel.intCreatedBy = oUserToken.intId;
                int result = _repoDoctors.InsertMultipleDoctorsAvailabilityByDate(doctorsavailabilityModel);

                if (result > 0)
                {
                    if (doctorsavailabilityModel.intId > 0)
                    {
                        objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                        objResponse.strMessage = "Slots Availability Successfully";
                        objResponse.objData = "";
                        return Ok(objResponse);
                    }
                    else
                    {
                        objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                        objResponse.strMessage = "Slots Availability Successfully";
                        objResponse.objData = "";
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

        [Route("api/[controller]/InsertUpdateDoctorsAvailability_Multiple")]
        [HttpPost]
        public IActionResult InsertUpdateDoctorsAvailability_Multiple([FromBody] DoctorsAvailabilityModel doctorsavailabilityModel)
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
                doctorsavailabilityModel.intCreatedBy = oUserToken.intId;
                int result = _repoDoctors.InsertUpdateDoctorsAvailability_Multiple(doctorsavailabilityModel);

                if (result > 0)
                {
                    if (doctorsavailabilityModel.intId > 0)
                    {
                        objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                        objResponse.strMessage = "DoctorsAvailablity Updated Successfully";
                        objResponse.objData = "";
                        return Ok(objResponse);
                    }
                    else
                    {
                        objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                        objResponse.strMessage = "DoctorsAvailablity Inserted Successfully";
                        objResponse.objData = "";
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


        [Route("api/[controller]/GetDoctorsAvailability")]
        [HttpPost]
        public IActionResult GetDoctorsAvailability([FromBody] DoctorsAvailabilityModel doctorsavailabilityModel)
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
                List<DoctorsModel> lstDoctorsAvailabilityModel = _repoDoctors.GetDoctorsAvailability(doctorsavailabilityModel);
                if (lstDoctorsAvailabilityModel != null)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Record Fetch Succesfully";
                    objResponse.objData = lstDoctorsAvailabilityModel;
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
        
        [Route("api/[controller]/GetDoctorsAvailabilityByDateandSpecaility")]
        [HttpPost]
        public IActionResult GetDoctorsAvailabilityByDateandSpecaility([FromBody] DoctorsAvailabilityModel doctorsavailabilityModel)
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
                List<DoctorsModel> lstDoctorsAvailabilityModel = _repoDoctors.GetDoctorsAvailabilityByDateandSpecaility(doctorsavailabilityModel);
                if (lstDoctorsAvailabilityModel != null)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Record Fetch Succesfully";
                    objResponse.objData = lstDoctorsAvailabilityModel;
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


        [Route("api/[controller]/GetDoctorsAppointementsBySlotId")]
        [HttpPost]
        public IActionResult GetDoctorsAvailabilityById([FromBody] DoctorsAvailabilityModel doctorsavailabilityModel)
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

                List<DoctorsAppointementsModel> objDoctorsAvailability = _repoDoctors.GetDoctorsAppointementsBySlotId(doctorsavailabilityModel.intDoctorId,doctorsavailabilityModel.intStatus);
                if (objDoctorsAvailability != null)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Record Fetch Succesfully";
                    objResponse.strAccess_Token = "";
                    objResponse.objData = objDoctorsAvailability;
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
        [Route("api/[Controller]/DeleteDoctorsAvailabilityByid")]
        [HttpPost]
        public IActionResult DeleteDoctorsAvailabilityByid([FromBody] DoctorsAvailabilityModel doctorsavailabilityModel)
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
                int result = _repoDoctors.DeleteDoctorsAvailabilityByid(doctorsavailabilityModel);

                if (doctorsavailabilityModel.intId > 0)
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

        #region DoctorsAvailabilityHistory

        [Route("api/[controller]/InsertUpdateDoctorsAvailabilityHistory")]
        [HttpPost]
        public IActionResult InsertUpdateDoctorsAvailabilityHistory([FromBody] DoctorsAvailabilityHistoryModel doctorsavailabilityhistoryModel)
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

                int result = _repoDoctors.InsertUpdateDoctorsAvailabilityHistory(doctorsavailabilityhistoryModel);

                if (result > 0)
                {
                    if (doctorsavailabilityhistoryModel.intId > 0)
                    {
                        objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                        objResponse.strMessage = "DoctorsAvailabilityHistory Updated Successfully";
                        objResponse.objData = "";
                        return Ok(objResponse);
                    }
                    else
                    {
                        objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                        objResponse.strMessage = "DoctorsAvailabilityHistory Inserted Successfully";
                        objResponse.objData = "";
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
        [Route("api/[controller]/GetDoctorsAvailabilityHistory")]
        [HttpGet]
        public IActionResult GetDoctorsAvailabilityHistory()
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
                List<DoctorsAvailabilityHistoryModel> lstDoctorsAvailabilityHistoryModel = _repoDoctors.GetDoctorsAvailabilityHistory();
                if (lstDoctorsAvailabilityHistoryModel != null)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Record Fetch Succesfully";
                    objResponse.objData = lstDoctorsAvailabilityHistoryModel;
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
        [Route("api/[controller]/GetDoctorsAvailabilityHistoryById")]
        [HttpPost]
        public IActionResult GetDoctorsAvailabilityHistoryById([FromBody] DoctorsAvailabilityHistoryModel doctorsavailabilityhistoryModel)
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

                DoctorsAvailabilityHistoryModel objDoctorsAvailabilityHistory = _repoDoctors.GetDoctorsAvailabilityHistoryById(doctorsavailabilityhistoryModel.intId);
                if (objDoctorsAvailabilityHistory != null)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Record Fetch Succesfully";
                    objResponse.strAccess_Token = "";
                    objResponse.objData = objDoctorsAvailabilityHistory;
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
        [Route("api/[Controller]/DeleteDoctorsAvailabilityHistoryByid")]
        [HttpPost]
        public IActionResult DeleteDoctorsAvailabilityHistoryByid([FromBody] DoctorsAvailabilityHistoryModel doctorsavailabilityhistoryModel)
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
                int result = _repoDoctors.DeleteDoctorsAvailabilityHistoryByid(doctorsavailabilityhistoryModel);

                if (doctorsavailabilityhistoryModel.intId > 0)
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

        #region DoctorsAvailabilitySlots
        [Route("api/[controller]/InsertUpdateDoctorsAvailabilitySlots")]
        [HttpPost]
        public IActionResult InsertUpdateDoctorsAvailabilitySlots([FromBody] DoctorsAvailabilitySlotsModel doctorsavailabilityslotsModel)
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

                int result = _repoDoctors.InsertUpdateDoctorsAvailabilitySlots(doctorsavailabilityslotsModel);

                if (result > 0)
                {
                    if (doctorsavailabilityslotsModel.intId > 0)
                    {
                        objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                        objResponse.strMessage = "DoctorsAvailabilitySlots Updated Successfully";
                        objResponse.objData = "";
                        return Ok(objResponse);
                    }
                    else
                    {
                        objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                        objResponse.strMessage = "DoctorsAvailabilitySlots Inserted Successfully";
                        objResponse.objData = "";
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
        [Route("api/[controller]/GetDoctorsAvailabilitySlots")]
        [HttpGet]
        public IActionResult GetDoctorsAvailabilitySlots()
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
                List<DoctorsAvailabilitySlotsModel> lstDoctorsAvailabilitySlotsModel = _repoDoctors.GetDoctorsAvailabilitySlots();
                if (lstDoctorsAvailabilitySlotsModel != null)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Record Fetch Succesfully";
                    objResponse.objData = lstDoctorsAvailabilitySlotsModel;
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

        [Route("api/[controller]/GetAllDoctorsAvailabilitysByDate")]
        [HttpPost]
        public IActionResult GetAllDoctorsAvailabilitysByDate(DoctorsAvailabilityModel objDoctorsAvailabilityModel)
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
                AllDoctorsAppointementsModel objAllDoctorsAppointementsModel = _repoDoctors.GetAllDoctorsAvailabilitysByDate(objDoctorsAvailabilityModel);
                if (objAllDoctorsAppointementsModel != null)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Record Fetch Succesfully";
                    objResponse.objData = objAllDoctorsAppointementsModel;
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


        [Route("api/[controller]/GetAllDoctorsAvailabilitysByDateAndPatientCounts")]
        [HttpPost]
        public IActionResult GetAllDoctorsAvailabilitysByDateAndPatientCounts(DoctorsAvailabilityModel objDoctorsAvailabilityModel)
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
                AllDoctorsAppointementsModel objAllDoctorsAppointementsModel = _repoDoctors.GetAllDoctorsAvailabilitysByDateAndPatientCounts(objDoctorsAvailabilityModel);
                if (objAllDoctorsAppointementsModel != null)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Record Fetch Succesfully";
                    objResponse.objData = objAllDoctorsAppointementsModel;
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


        [Route("api/[controller]/GetDoctorsAvailabilitySlotsById")]
        [HttpPost]
        public IActionResult GetDoctorsAvailabilitySlotsById([FromBody] DoctorsAvailabilitySlotsModel doctorsavailabilityslotsModel)
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

                List<DoctorsAvailabilitySlotsModel> objDoctorsAvailabilitySlots = _repoDoctors.GetDoctorsAvailabilitySlotsByDoctorId(doctorsavailabilityslotsModel.intDoctorId);
                if (objDoctorsAvailabilitySlots != null)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Record Fetch Succesfully";
                    objResponse.strAccess_Token = "";
                    objResponse.objData = objDoctorsAvailabilitySlots;
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

        [Route("api/[controller]/GetDoctorsAvailabilitySlotsByIdAndDate")]
        [HttpPost]
        public IActionResult GetDoctorsAvailabilitySlotsByIdAndDate([FromBody] DoctorsAvailabilitySlotsModel doctorsavailabilityslotsModel)
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

                List<DoctorsAvailabilitySlotsModel> objDoctorsAvailabilitySlots = _repoDoctors.GetDoctorsAvailabilitySlotsByIdAndDate(doctorsavailabilityslotsModel.intDoctorId,doctorsavailabilityslotsModel.strDate);
                if (objDoctorsAvailabilitySlots != null)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Record Fetch Succesfully";
                    objResponse.strAccess_Token = "";
                    objResponse.objData = objDoctorsAvailabilitySlots;
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

        [Route("api/[Controller]/DeleteDoctorsAvailabilitySlotsByid")]
        [HttpPost]
        public IActionResult DeleteDoctorsAvailabilitySlotsByid([FromBody] DoctorsAvailabilitySlotsModel doctorsavailabilityslotsModel)
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
                int result = _repoDoctors.DeleteDoctorsAvailabilitySlotsByid(doctorsavailabilityslotsModel);

                if (result > 0)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Record Delete Succesfully";
                    objResponse.objData = null;
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

        [Route("api/[Controller]/GetDoctorsAvailabilitysByDateAndDocId")]
        [HttpPost]
        public IActionResult GetDoctorsAvailabilitysByDateAndDocId([FromBody] DoctorsAvailabilityModel objDoctorsAvailabilityModel)
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
                DoctorsModel objDoctorsModel = _repoDoctors.GetDoctorsAvailabilitysByDateAndDocId(objDoctorsAvailabilityModel);

                if (objDoctorsModel.intId > 0)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Record fetched Succesfully";
                    objResponse.objData = objDoctorsModel;
                    return Ok(objResponse);
                }
                else
                {

                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.ValidationFails;
                    objResponse.strMessage = "Record not fetched Succesfully";
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

        #region DoctorSpecialty

        [Route("api/[controller]/InsertUpdateDoctorSpecialty")]
        [HttpPost]
        public IActionResult InsertUpdateDoctorSpecialty([FromBody] DoctorSpecialtyModel doctorspecialtyModel)
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

                int result = _repoDoctors.InsertUpdateDoctorSpecialty(doctorspecialtyModel);

                if (result > 0)
                {
                    if (doctorspecialtyModel.intId > 0)
                    {
                        objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                        objResponse.strMessage = "DoctorSpecialty Updated Successfully";
                        objResponse.objData = "";
                        return Ok(objResponse);
                    }
                    else
                    {
                        objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                        objResponse.strMessage = "DoctorSpecialty Inserted Successfully";
                        objResponse.objData = "";
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
        [Route("api/[controller]/GetDoctorSpecialty")]
        [HttpGet]
        public IActionResult GetDoctorSpecialty()
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
                List<DoctorSpecialtyModel> lstDoctorSpecialtyModel = _repoDoctors.GetDoctorSpecialty();
                if (lstDoctorSpecialtyModel != null)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Record Fetch Succesfully";
                    objResponse.objData = lstDoctorSpecialtyModel;
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
        [Route("api/[controller]/GetDoctorSpecialtyById")]
        [HttpPost]
        public IActionResult GetDoctorSpecialtyById([FromBody] DoctorSpecialtyModel doctorspecialtyModel)
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

                List<DoctorsModel> objDoctorSpecialty = _repoDoctors.GetDoctorSpecialtyById(doctorspecialtyModel.intSpecialityId);
                if (objDoctorSpecialty != null)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Record Fetch Succesfully";
                    objResponse.strAccess_Token = "";
                    objResponse.objData = objDoctorSpecialty;
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


        [Route("api/[Controller]/DeleteDoctorSpecialtyByid")]
        [HttpPost]
        public IActionResult DeleteDoctorSpecialtyByid([FromBody] DoctorSpecialtyModel doctorspecialtyModel)
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
                int result = _repoDoctors.DeleteDoctorSpecialtyByid(doctorspecialtyModel);

                if (doctorspecialtyModel.intId > 0)
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

        #region DoctorsAvailabilitySlotsHistory

        [Route("api/[controller]/GetDoctorsAvailabilitySlotsHistory")]
        [HttpGet]
        public IActionResult GetDoctorsAvailabilitySlotsHistory()
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
                List<DoctorsAvailabilitySlotsHistoryModel> lstDoctorsAvailabilitySlotsHistoryModel = _repoDoctors.GetDoctorsAvailabilitySlotsHistory();
                if (lstDoctorsAvailabilitySlotsHistoryModel != null)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Record Fetch Succesfully";
                    objResponse.objData = lstDoctorsAvailabilitySlotsHistoryModel;
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
        #endregion

        #region DoctorsReport

        [Route("api/[controller]/GetDoctorsAppointementsByDate")]
        [HttpPost]
        public IActionResult GetDoctorsAppointementsByDate([FromBody] DoctorsAvailabilityModel doctorsavailabilityModel)
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

                List<DoctorsAppointementsModel> objDoctorsAvailability = _repoDoctors.GetDoctorsAppointementsByDate(doctorsavailabilityModel.intDoctorId, doctorsavailabilityModel.intTypeId);
                if (objDoctorsAvailability != null)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Record Fetch Succesfully";
                    objResponse.strAccess_Token = "";
                    objResponse.objData = objDoctorsAvailability;
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
        #endregion
    }
}