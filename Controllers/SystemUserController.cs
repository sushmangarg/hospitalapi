using HospitalManagement.Model;
using HospitalManagement.Repository.Interface;
using HospitalManagement_Models.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static HospitalManagement.Model.ResponseModels;

namespace HospitalManagement.Controllers
{
    
    [ApiController]
    public class SystemUserController : ControllerBase
    {
        private readonly ISystemUser _repoSystemUser;

        public SystemUserController(ISystemUser _repoSystemUser)
        {
            this._repoSystemUser = _repoSystemUser;
        }

        #region SystemUsers

        [Route("api/[Controller]/SystemUserLogin")]
        [HttpPost]
        public IActionResult SystemUserLogin(SystemUsersModel systemusersModel)
        {
            ResponseModels objResponse = new ResponseModels();
            string strApiName = this.ControllerContext.RouteData.Values["controller"].ToString() + "/" + this.ControllerContext.RouteData.Values["action"].ToString();
            try
            {
                SystemUsersModel objSystemUsersModel = _repoSystemUser.SystemUserLogin(systemusersModel);
                if (objSystemUsersModel == null)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.ValidationFails;
                    objResponse.strMessage = "UserName/Password Not correct.";
                    objResponse.objData = "";
                    return Ok(objResponse);
                }
                else
                {

                    string token = TokenManager.GenerateToken_User(objSystemUsersModel);
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Login Succefully";
                    objResponse.objData = objSystemUsersModel;                    
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
        [Route("api/[controller]/InsertUpdateSystemUsers")]
        [HttpPost]
        public IActionResult InsertUpdateSystemUsers([FromBody] SystemUsersModel systemusersModel)
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
                
                int result = _repoSystemUser.InsertUpdateSystemUsers(systemusersModel);

                if (result > 0)
                {
                    if (systemusersModel.intId > 0)
                    {
                        objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                        objResponse.strMessage = "System Users Updated Successfully";
                        objResponse.objData = "";
                        return Ok(objResponse);
                    }
                    else
                    {
                        objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                        objResponse.strMessage = "System Users Inserted Successfully";
                        objResponse.objData = "";
                        return Ok(objResponse);
                    }
                }
                else if (result == -1)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.ValidationFails;
                    objResponse.strMessage = "Mobile Number is already Exist";
                    objResponse.objData = "";
                    return Ok(objResponse);
                }
                else if (result == -2)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.ValidationFails;
                    objResponse.strMessage = "Email id is already Exist";
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

        [Route("api/[controller]/GetSystemUsers")]
        [HttpGet]
        public IActionResult GetSystemUsers()
        {
            ResponseModels objResponse = new ResponseModels();
            string strApiName = this.ControllerContext.RouteData.Values["controller"].ToString() + "/" + this.ControllerContext.RouteData.Values["action"].ToString();
            try
            {
                List<SystemUsersModel> lstSystemUser = _repoSystemUser.GetSystemUsers();
                if (lstSystemUser != null)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Record Fetch Succesfully";
                    objResponse.objData = lstSystemUser;
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
        [Route("api/[controller]/GetSystemUsersById")]
        [HttpPost]
        public IActionResult GetSystemUsersById(SystemUsersModel systemusersModel)
        {
            ResponseModels objResponse = new ResponseModels();
            string strApiName = this.ControllerContext.RouteData.Values["controller"].ToString() + "/" + this.ControllerContext.RouteData.Values["action"].ToString();
            try
            {
                SystemUsersModel objsystemUsers = _repoSystemUser.GetSystemUsersById(systemusersModel.intId);
                if (objsystemUsers != null)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Record Fetch Succesfully";
                    objResponse.strAccess_Token = "";
                    objResponse.objData = objsystemUsers;
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
                objResponse.strAccess_Token = ex.Message;
                objResponse.objData = "";
                return Ok(objResponse);
            }

        }
        #endregion
    }
}
