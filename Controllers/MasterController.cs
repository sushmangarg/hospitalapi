using HospitalManagement.Model;
using HospitalManagement.Repository.Interface.Master;
using HospitalManagement_Models;
using HospitalManagement_Models.Doctors;
using HospitalManagement_Models.Master;
using HospitalManagement_Models.Users;
using ISM_AppTrackerModels;
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
    public class MasterController : ControllerBase
    {
        private readonly IMaster _repoMaster;
        public MasterController(IMaster _repoMaster)
        {
            this._repoMaster = _repoMaster;
        }

        #region City
        [Route("api/[controller]/InsertUpdateCity")]
        [HttpPost]
        public IActionResult InsertUpdateCity([FromBody] CityModel cityModel)
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
                cityModel.intCreatedBy = oUserToken.intId;
                int result = _repoMaster.InsertUpdateCity(cityModel);

                if (result > 0)
                {
                    if (cityModel.intId > 0)
                    {
                        objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                        objResponse.strMessage = "City Updated Successfully";
                        objResponse.objData = "";
                        return Ok(objResponse);
                    }
                    else
                    {
                        objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                        objResponse.strMessage = "City Inserted Successfully";
                        objResponse.objData = "";
                        return Ok(objResponse);
                    }
                }
                else if (result == -1)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "City already exists";
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

        [Route("api/[controller]/GetCity")]
        [HttpGet]
        public IActionResult GetCity()
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
                List<CityModel> lstCityModel = _repoMaster.GetCity();
                if (lstCityModel != null)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Record Fetch Succesfully";
                    objResponse.objData = lstCityModel;
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
        [Route("api/[controller]/GetCityById")]
        [HttpPost]
        public IActionResult GetCityById([FromBody] CityModel cityModel)
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

                CityModel objCity = _repoMaster.GetCityById(cityModel.intId);
                if (objCity != null)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Record Fetch Succesfully";
                    objResponse.strAccess_Token = "";
                    objResponse.objData = objCity;
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
        [Route("api/[Controller]/DeleteCityByid")]
        [HttpPost]
        public IActionResult DeleteCityByid([FromBody] CityModel cityModel)
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
                int result = _repoMaster.DeleteCityByid(cityModel);

                if (cityModel.intId > 0)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Record deleted succesfully";
                    objResponse.objData = "";
                    return Ok(objResponse);
                }
                else
                {

                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.ValidationFails;
                    objResponse.strMessage = "Record can not Delete";
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

        #region Locations
        [Route("api/[controller]/InsertUpdateLocations")]
        [HttpPost]
        public IActionResult InsertUpdateLocations([FromBody] LocationsModel locationsModel)
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

                int result = _repoMaster.InsertUpdateLocations(locationsModel);

                if (result > 0)
                {
                    if (locationsModel.intId > 0)
                    {
                        objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                        objResponse.strMessage = "Locations Updated Successfully";
                        objResponse.objData = "";
                        return Ok(objResponse);
                    }
                    else
                    {
                        objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                        objResponse.strMessage = "Locations Inserted Successfully";
                        objResponse.objData = "";
                        return Ok(objResponse);
                    }
                }
                else if (result == -1)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Duplicate email";
                    objResponse.objData = "";
                    return Ok(objResponse);

                }
                else if (result == -2)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Duplicate phone no";
                    objResponse.objData = "";
                    return Ok(objResponse);

                }
                else if (result == -3)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Duplicate location";
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

        [Route("api/[controller]/GetLocations")]
        [HttpGet]
        public IActionResult GetLocations()
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
                List<LocationsModel> lstLocationsModel = _repoMaster.GetLocations();
                if (lstLocationsModel != null)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Record Fetch Succesfully";
                    objResponse.objData = lstLocationsModel;
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
        [Route("api/[controller]/GetLocationsById")]
        [HttpPost]
        public IActionResult GetLocationsById([FromBody] LocationsModel locationsModel)
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

                LocationsModel objLocations = _repoMaster.GetLocationsById(locationsModel.intId);
                if (objLocations != null)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Record Fetch Succesfully";
                    objResponse.strAccess_Token = "";
                    objResponse.objData = objLocations;
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
        [Route("api/[Controller]/DeleteLocationByid")]
        [HttpPost]
        public IActionResult DeleteLocationByid([FromBody] LocationsModel locationsModel)
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
                int result = _repoMaster.DeleteLocationByid(locationsModel);

                if (locationsModel.intId > 0)
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

        #region Slots
        [Route("api/[controller]/InsertUpdateSlots")]
        [HttpPost]
        public IActionResult InsertUpdateSlots([FromBody] SlotsModel slotsModel)
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
                slotsModel.intCreatedBy = oUserToken.intCreatedBy;

                int result = _repoMaster.InsertUpdateSlots(slotsModel);
                if (result > 0)
                {
                    if (slotsModel.intId > 0)
                    {
                        objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                        objResponse.strMessage = "Slots Updated Successfully";
                        objResponse.objData = "";
                        return Ok(objResponse);
                    }
                    else
                    {
                        objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                        objResponse.strMessage = "Slots Inserted Successfully";
                        objResponse.objData = "";
                        return Ok(objResponse);
                    }
                }
                else if (result == -1)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Start Time Should not be greater or equal to End Time.";
                    objResponse.objData = "";
                    return Ok(objResponse);
                }
                else if (result == 0)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Slot already exists";
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
        [Route("api/[controller]/GetSlots")]
        [HttpGet]
        public IActionResult GetSlots()
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
                List<SlotsModel> lstSlotsModel = _repoMaster.GetSlots();
                if (lstSlotsModel != null)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Record Fetch Succesfully";
                    objResponse.objData = lstSlotsModel;
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
        [Route("api/[controller]/GetAllSlots")]
        [HttpGet]
        public IActionResult GetAllSlots()
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
                List<SlotsModel> lstSlotsModel = _repoMaster.GetAllSlots();
                if (lstSlotsModel != null)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Record Fetch Succesfully";
                    objResponse.objData = lstSlotsModel;
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
        [Route("api/[controller]/GetSlotsById")]
        [HttpPost]
        public IActionResult GetSlotsById([FromBody] SlotsModel slotsModel)
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

                List<SlotsModel> objSlots = _repoMaster.GetSlotsById(slotsModel.intDoctorId);
                if (objSlots != null)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Record Fetch Succesfully";
                    objResponse.strAccess_Token = "";
                    objResponse.objData = objSlots;
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
        [Route("api/[Controller]/DeleteSlotsByid")]
        [HttpPost]
        public IActionResult DeleteSlotsByid([FromBody] SlotsModel slotsModel)
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
                int result = _repoMaster.DeleteSlotsByid(slotsModel);

                if (slotsModel.intId > 0)
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

        #region Speciality
        [Route("api/[controller]/InsertUpdateSpeciality")]
        [HttpPost]
        public IActionResult InsertUpdateSpeciality([FromBody] SpecialityModel specialityModel)
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
                specialityModel.intCreatedBy = oUserToken.intId;
                int result = _repoMaster.InsertUpdateSpeciality(specialityModel);

                if (result > 0)
                {
                    if (specialityModel.intId > 0)
                    {
                        objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                        objResponse.strMessage = "Speciality Updated Successfully";
                        objResponse.objData = "";
                        return Ok(objResponse);
                    }
                    else
                    {
                        objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                        objResponse.strMessage = "Speciality Inserted Successfully";
                        objResponse.objData = "";
                        return Ok(objResponse);
                    }
                }
                else if (result == -1)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.ValidationFails;
                    objResponse.strMessage = "Speciality Already Exists.";
                    objResponse.objData = "";
                    return Ok(objResponse);
                }
                else
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.ValidationFails;
                    objResponse.strMessage = "Data too long";
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

        [Route("api/[controller]/GetSpeciality")]
        [HttpGet]
        public IActionResult GetSpeciality()
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
                List<SpecialityModel> lstSpecialityModel = _repoMaster.GetSpeciality();
                if (lstSpecialityModel != null)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Record Fetch Succesfully";
                    objResponse.objData = lstSpecialityModel;
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
        [Route("api/[controller]/GetAllSpeciality")]
        [HttpGet]
        public IActionResult GetAllSpeciality()
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
                List<SpecialityModel> lstSpecialityModel = _repoMaster.GetAllSpeciality();
                if (lstSpecialityModel != null)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Record Fetch Succesfully";
                    objResponse.objData = lstSpecialityModel;
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

        [Route("api/[controller]/GetSpecialityById")]
        [HttpPost]
        public IActionResult GetSpecialityById([FromBody] SpecialityModel specialityModel)
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

                SpecialityModel objSpeciality = _repoMaster.GetSpecialityById(specialityModel.intId);
                if (objSpeciality != null)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Record Fetch Succesfully";
                    objResponse.strAccess_Token = "";
                    objResponse.objData = objSpeciality;
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
        [Route("api/[Controller]/DeleteSpecialityById")]
        [HttpPost]
        public IActionResult DeleteSpecialityById([FromBody] SpecialityModel specialityModel)
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
                int result = _repoMaster.DeleteSpecialityById(specialityModel);

                if (specialityModel.intId > 0)
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

        #region country state district
        [Route("api/[controller]/InsertUpdateCountry")]
        [HttpPost]
        public IActionResult InsertUpdateCountry([FromBody] CountryModel objCountryModel)
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
                objCountryModel.intCreatedby = oUserToken.intId;
                int result = _repoMaster.InsertUpdateCountry(objCountryModel);

                if (result > 0)
                {
                    if (objCountryModel.intId > 0)
                    {
                        objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                        objResponse.strMessage = "Country Updated Successfully";
                        objResponse.objData = "";
                        return Ok(objResponse);
                    }
                    else
                    {
                        objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                        objResponse.strMessage = "Country Inserted Successfully";
                        objResponse.objData = "";
                        return Ok(objResponse);
                    }
                }
                else
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Country already exists";
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

        [Route("api/[controller]/GetCountry")]
        [HttpPost]
        public IActionResult GetCountry()
        {
            ResponseModels oReturn = new ResponseModels();
            string strApiName = this.ControllerContext.RouteData.Values["controller"].ToString() + "/" + this.ControllerContext.RouteData.Values["action"].ToString();
            #region Token Validation
            Microsoft.Extensions.Primitives.StringValues token;
            if (Request.Headers.Count > 0)
            {
                Request.Headers.TryGetValue("access_token", out token);
            }
            if (string.IsNullOrEmpty(token))
            {
                oReturn.intStatusCode = (int)E_RESPONSESTATUS.Unauthorized;
                oReturn.strMessage = E_RESPONSESTATUS.Unauthorized.ToString();
                return Ok(oReturn);
            }
            string strKey = clsSecretKey.SecretKey;
            UserModel oUserToken = TokenManager.GetTokenClaims_User(Convert.ToString(token).Replace("Bearer ", ""));
            if (oUserToken.intStatus == -1)
            {
                oReturn.intStatusCode = (int)E_RESPONSESTATUS.Unauthorized;
                oReturn.strMessage = E_RESPONSESTATUS.Unauthorized.ToString();
                return Ok(oReturn);
            }
            #endregion
            try
            {

                List<CountryModel> lstCountryModel = _repoMaster.GetCountry();
                if (lstCountryModel != null)
                {
                    oReturn.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    oReturn.strMessage = "Record Fetch Succesfully";
                    oReturn.objData = lstCountryModel;
                    return Ok(oReturn);
                }
                else
                {
                    oReturn.intStatusCode = (int)E_RESPONSESTATUS.ValidationFails;
                    oReturn.strMessage = "Record not Fetch Succesfully";
                    oReturn.objData = null;
                    return Ok(oReturn);
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, strApiName, "", "", "");
                oReturn.intStatusCode = (int)E_RESPONSESTATUS.ExceptionOccurs;
                oReturn.strMessage = ex.Message;
                oReturn.objData = "";
                return Ok(oReturn);
            }
        }

        [Route("api/[controller]/GetCountryForMaster")]
        [HttpPost]
        public IActionResult GetCountryForMaster()
        {
            ResponseModels oReturn = new ResponseModels();
            string strApiName = this.ControllerContext.RouteData.Values["controller"].ToString() + "/" + this.ControllerContext.RouteData.Values["action"].ToString();
            #region Token Validation
            Microsoft.Extensions.Primitives.StringValues token;
            if (Request.Headers.Count > 0)
            {
                Request.Headers.TryGetValue("access_token", out token);
            }
            if (string.IsNullOrEmpty(token))
            {
                oReturn.intStatusCode = (int)E_RESPONSESTATUS.Unauthorized;
                oReturn.strMessage = E_RESPONSESTATUS.Unauthorized.ToString();
                return Ok(oReturn);
            }
            string strKey = clsSecretKey.SecretKey;
            UserModel oUserToken = TokenManager.GetTokenClaims_User(Convert.ToString(token).Replace("Bearer ", ""));
            if (oUserToken.intStatus == -1)
            {
                oReturn.intStatusCode = (int)E_RESPONSESTATUS.Unauthorized;
                oReturn.strMessage = E_RESPONSESTATUS.Unauthorized.ToString();
                return Ok(oReturn);
            }
            #endregion
            try
            {

                List<CountryModel> lstCountryModel = _repoMaster.GetCountryForMaster();
                if (lstCountryModel != null)
                {
                    oReturn.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    oReturn.strMessage = "Record Fetch Succesfully";
                    oReturn.objData = lstCountryModel;
                    return Ok(oReturn);
                }
                else
                {
                    oReturn.intStatusCode = (int)E_RESPONSESTATUS.ValidationFails;
                    oReturn.strMessage = "Record not Fetch Succesfully";
                    oReturn.objData = null;
                    return Ok(oReturn);
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, strApiName, "", "", "");
                oReturn.intStatusCode = (int)E_RESPONSESTATUS.ExceptionOccurs;
                oReturn.strMessage = ex.Message;
                oReturn.objData = "";
                return Ok(oReturn);
            }
        }

        [Route("api/[Controller]/DeleteCountryById")]
        [HttpPost]
        public IActionResult DeleteCountryById([FromBody] CountryModel objCountryModel)
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
                int result = _repoMaster.DeleteCountryById(objCountryModel);

                if (result > 0)
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
        [Route("api/[Controller]/GetStateByCountryId")]
        [HttpPost]
        public IActionResult GetStateByCountryId([FromBody] StateModel objStateModel)
        {
            ResponseModels oReturn = new ResponseModels();
            string strApiName = this.ControllerContext.RouteData.Values["controller"].ToString() + "/" + this.ControllerContext.RouteData.Values["action"].ToString();
            #region Token Validation
            Microsoft.Extensions.Primitives.StringValues token;
            if (Request.Headers.Count > 0)
            {
                Request.Headers.TryGetValue("access_token", out token);
            }
            if (string.IsNullOrEmpty(token))
            {
                oReturn.intStatusCode = (int)E_RESPONSESTATUS.Unauthorized;
                oReturn.strMessage = E_RESPONSESTATUS.Unauthorized.ToString();
                return Ok(oReturn);
            }
            string strKey = clsSecretKey.SecretKey;
            UserModel oUserToken = TokenManager.GetTokenClaims_User(Convert.ToString(token).Replace("Bearer ", ""));
            if (oUserToken.intStatus == -1)
            {
                oReturn.intStatusCode = (int)E_RESPONSESTATUS.Unauthorized;
                oReturn.strMessage = E_RESPONSESTATUS.Unauthorized.ToString();
                return Ok(oReturn);
            }
            #endregion
            try
            {
                List<StateModel> lstStateModel = _repoMaster.GetStateByCountryId(objStateModel.intCountryId);

                if (lstStateModel != null)
                {
                    oReturn.intStatusCode = 200;
                    oReturn.strMessage = "Data Fetched";
                    oReturn.objData = lstStateModel;
                    return Ok(oReturn);
                }
                else
                {
                    oReturn.intStatusCode = 201;
                    oReturn.strMessage = "Data Not Available";
                    oReturn.objData = null;
                    return Ok(oReturn);
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, strApiName, "", "", "");
                oReturn.intStatusCode = (int)E_RESPONSESTATUS.ExceptionOccurs;
                oReturn.strMessage = ex.Message.ToString();
                oReturn.objData = "Internal Error !";
                return Ok(oReturn);
            }
        }

        [Route("api/[Controller]/GetCityByStateId")]
        [HttpPost]
        public IActionResult GetCityByStateId([FromBody] CityModel objCityModel)
        {
            ResponseModels oReturn = new ResponseModels();
            string strApiName = this.ControllerContext.RouteData.Values["controller"].ToString() + "/" + this.ControllerContext.RouteData.Values["action"].ToString();
            #region Token Validation
            Microsoft.Extensions.Primitives.StringValues token;
            if (Request.Headers.Count > 0)
            {
                Request.Headers.TryGetValue("access_token", out token);
            }
            if (string.IsNullOrEmpty(token))
            {
                oReturn.intStatusCode = (int)E_RESPONSESTATUS.Unauthorized;
                oReturn.strMessage = E_RESPONSESTATUS.Unauthorized.ToString();
                return Ok(oReturn);
            }
            string strKey = clsSecretKey.SecretKey;
            UserModel oUserToken = TokenManager.GetTokenClaims_User(Convert.ToString(token).Replace("Bearer ", ""));
            if (oUserToken.intStatus == -1)
            {
                oReturn.intStatusCode = (int)E_RESPONSESTATUS.Unauthorized;
                oReturn.strMessage = E_RESPONSESTATUS.Unauthorized.ToString();
                return Ok(oReturn);
            }
            #endregion
            try
            {
                List<CityModel> lstCityModel = _repoMaster.GetCityByStateId(objCityModel.intStateId);

                if (lstCityModel != null)
                {
                    oReturn.intStatusCode = 200;
                    oReturn.strMessage = "Data Fetched";
                    oReturn.objData = lstCityModel;
                    return Ok(oReturn);
                }
                else
                {
                    oReturn.intStatusCode = 201;
                    oReturn.strMessage = "Data Not Available";
                    oReturn.objData = null;
                    return Ok(oReturn);
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, strApiName, "", "", "");
                oReturn.intStatusCode = (int)E_RESPONSESTATUS.ExceptionOccurs;
                oReturn.strMessage = ex.Message.ToString();
                oReturn.objData = "Internal Error !";
                return Ok(oReturn);
            }
        }

        [Route("api/[controller]/InsertUpdateState")]
        [HttpPost]
        public IActionResult InsertUpdateState([FromBody] StateModel objStateModel)
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
                objStateModel.intCreatedby = oUserToken.intId;
                int result = _repoMaster.InsertUpdateState(objStateModel);

                if (result > 0)
                {
                    if (objStateModel.intId > 0)
                    {
                        objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                        objResponse.strMessage = "State Updated Successfully";
                        objResponse.objData = "";
                        return Ok(objResponse);
                    }
                    else
                    {
                        objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                        objResponse.strMessage = "State Inserted Successfully";
                        objResponse.objData = "";
                        return Ok(objResponse);
                    }
                }
                else
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "State already exists";
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

        [Route("api/[controller]/GetStates")]
        [HttpPost]
        public IActionResult GetStates()
        {
            ResponseModels oReturn = new ResponseModels();
            string strApiName = this.ControllerContext.RouteData.Values["controller"].ToString() + "/" + this.ControllerContext.RouteData.Values["action"].ToString();
            #region Token Validation
            Microsoft.Extensions.Primitives.StringValues token;
            if (Request.Headers.Count > 0)
            {
                Request.Headers.TryGetValue("access_token", out token);
            }
            if (string.IsNullOrEmpty(token))
            {
                oReturn.intStatusCode = (int)E_RESPONSESTATUS.Unauthorized;
                oReturn.strMessage = E_RESPONSESTATUS.Unauthorized.ToString();
                return Ok(oReturn);
            }
            string strKey = clsSecretKey.SecretKey;
            UserModel oUserToken = TokenManager.GetTokenClaims_User(Convert.ToString(token).Replace("Bearer ", ""));
            if (oUserToken.intStatus == -1)
            {
                oReturn.intStatusCode = (int)E_RESPONSESTATUS.Unauthorized;
                oReturn.strMessage = E_RESPONSESTATUS.Unauthorized.ToString();
                return Ok(oReturn);
            }
            #endregion
            try
            {

                List<StateModel> lstStateModel = _repoMaster.GetStates();
                if (lstStateModel != null)
                {
                    oReturn.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    oReturn.strMessage = "Record Fetch Succesfully";
                    oReturn.objData = lstStateModel;
                    return Ok(oReturn);
                }
                else
                {
                    oReturn.intStatusCode = (int)E_RESPONSESTATUS.ValidationFails;
                    oReturn.strMessage = "Record not Fetch Succesfully";
                    oReturn.objData = null;
                    return Ok(oReturn);
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, strApiName, "", "", "");
                oReturn.intStatusCode = (int)E_RESPONSESTATUS.ExceptionOccurs;
                oReturn.strMessage = ex.Message;
                oReturn.objData = "";
                return Ok(oReturn);
            }
        }
        [Route("api/[Controller]/DeleteStateyById")]
        [HttpPost]
        public IActionResult DeleteStateyById([FromBody] StateModel objStateModel)
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
                int result = _repoMaster.DeleteStateyById(objStateModel);

                if (result > 0)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Record Delete Succesfully";
                    objResponse.objData = "";
                    return Ok(objResponse);
                }
                else
                {

                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
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

        #region Current Date
        [Route("api/[controller]/GetCurrentDate")]
        [HttpGet]
        public IActionResult GetCurrentDate()
        {

            ResponseModels objResponse = new ResponseModels();
            string strApiName = this.ControllerContext.RouteData.Values["controller"].ToString() + "/" + this.ControllerContext.RouteData.Values["action"].ToString();
            try
            {
                objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                objResponse.strMessage = "";
                objResponse.objData = DateTime.Now.ToString("yyyy-MM-dd");
                return Ok(objResponse);
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

        [Route("api/[controller]/GetCurrentTime")]
        [HttpGet]
        public IActionResult GetCurrentTime()
        {

            ResponseModels objResponse = new ResponseModels();
            string strApiName = this.ControllerContext.RouteData.Values["controller"].ToString() + "/" + this.ControllerContext.RouteData.Values["action"].ToString();
            try
            {
                objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                objResponse.strMessage = "";
                objResponse.objData = DateTime.Now.ToString("hh:mm:ss");
                return Ok(objResponse);
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

        #region Get Dates
        [Route("api/[controller]/GetWeekDates")]
        [HttpPost]
        public IActionResult GetWeekDates([FromBody] DateModal objDateModel)
        {
            ResponseModels objResponse = new ResponseModels();
            string strApiName = this.ControllerContext.RouteData.Values["controller"].ToString() + "/" + this.ControllerContext.RouteData.Values["action"].ToString();
            try
            {
                List<DateModal> lstDates = new List<DateModal>();
                if (objDateModel.intType == 1)
                {
                    DateTime currDate = Convert.ToDateTime(objDateModel.strDate);
                    currDate = currDate.AddDays(7);
                    for (int index = 0; index < 7; index++)
                    {
                        DateModal objDate = new DateModal();
                        objDate.strDate = currDate.ToString("yyyy-MM-dd");
                        objDate.strDateFormatted = currDate.ToString("dd-MMM-yyyy");
                        currDate = currDate.AddDays(1);
                        lstDates.Add(objDate);
                    }
                }
                else if (objDateModel.intType == 2)
                {
                    DateTime currDate = Convert.ToDateTime(objDateModel.strDate);
                    currDate = currDate.AddDays(-7);
                    for (int index = 0; index < 7; index++)
                    {
                        DateModal objDate = new DateModal();
                        objDate.strDate = currDate.ToString("yyyy-MM-dd");
                        objDate.strDateFormatted = currDate.ToString("dd-MMM-yyyy");
                        lstDates.Add(objDate);
                        currDate = currDate.AddDays(1);
                    }
                }
                else
                {
                    DateTime currDate = Convert.ToDateTime(objDateModel.strDate);
                    for (int index = 0; index < 7; index++)
                    {
                        DateModal objDate = new DateModal();
                        objDate.strDate = currDate.ToString("yyyy-MM-dd");
                        objDate.strDateFormatted = currDate.ToString("dd-MMM-yyyy");
                        lstDates.Add(objDate);
                        currDate = currDate.AddDays(1);
                    }
                }
                objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                objResponse.strMessage = "";
                objResponse.objData = lstDates;
                return Ok(objResponse);
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

        #region Doctor Skills
        [Route("api/[controller]/InsertUpdateDoctorSkills")]
        [HttpPost]
        public IActionResult InsertUpdateDoctorSkills([FromBody] DoctorSkillsModel doctorSkills)
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
                doctorSkills.intCreatedById = oUserToken.intId;
                int result = _repoMaster.InsertUpdateDoctorSkills(doctorSkills);

                if (result > 0)
                {
                    if (doctorSkills.intId > 0)
                    {
                        objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                        objResponse.strMessage = "Skill Updated Successfully";
                        objResponse.objData = "";
                        return Ok(objResponse);
                    }
                    else
                    {
                        objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                        objResponse.strMessage = "Skill Inserted Successfully";
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

        [Route("api/[controller]/GetDoctorSkillsByDoctorId")]
        [HttpPost]
        public IActionResult GetDoctorSkillsByDoctorId(DoctorSkillsModel doctorSkills)
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
                if (doctorSkills.intDoctorId == 0)
                {
                    doctorSkills.intDoctorId = oUserToken.intId;
                }
                List<DoctorSkillsModel> lstDoctorSkills = _repoMaster.GetDoctorSkillsByDoctorId(doctorSkills);
                if (lstDoctorSkills != null)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Record Fetch Succesfully";
                    objResponse.objData = lstDoctorSkills;
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
        [Route("api/[controller]/DeleteDoctorSkillByDoctorId")]
        [HttpPost]
        public IActionResult DeleteDoctorSkillByDoctorId([FromBody] DoctorSkillsModel doctorSkills)
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

                int retVal = _repoMaster.DeleteDoctorSkillByDoctorId(doctorSkills);
                if (retVal > 0)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Record Deleted Succesfully";
                    objResponse.strAccess_Token = "";
                    objResponse.objData = "";
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

        [Route("api/[controller]/GetDoctorSkillsById")]
        [HttpPost]
        public IActionResult GetDoctorSkillsById(DoctorSkillsModel doctorSkills)
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
                DoctorSkillsModel lstDoctorSkills = _repoMaster.GetDoctorSkillsById(doctorSkills);
                if (lstDoctorSkills != null)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Record Fetch Succesfully";
                    objResponse.objData = lstDoctorSkills;
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

        #region Doctor Speciality
        [Route("api/[controller]/InsertUpdateDoctorSpeciality")]
        [HttpPost]
        public IActionResult InsertUpdateDoctorSpeciality([FromBody] DoctorSpecialityModel objDoctorSpeciality)
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
                objDoctorSpeciality.intCreatedBy = oUserToken.intId;
                int result = _repoMaster.InsertUpdateDoctorSpeciality(objDoctorSpeciality);

                if (result > 0)
                {
                    if (objDoctorSpeciality.intId > 0)
                    {
                        objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                        objResponse.strMessage = "Speciality Updated Successfully";
                        objResponse.objData = "";
                        return Ok(objResponse);
                    }
                    else
                    {
                        objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                        objResponse.strMessage = "Speciality Inserted Successfully";
                        objResponse.objData = "";
                        return Ok(objResponse);
                    }
                }
                else if (result == -1)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Speciality already exists";
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

        [Route("api/[controller]/GetDoctorSpecialityByDoctorId")]
        [HttpPost]
        public IActionResult GetDoctorSpecialityByDoctorId(DoctorSpecialityModel objDoctorSpeciality)
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
                if (objDoctorSpeciality.intDoctorId == 0)
                {
                    objDoctorSpeciality.intDoctorId = oUserToken.intId;
                }
                List<DoctorSpecialityModel> lstDoctorSpeciality = _repoMaster.GetDoctorSpecialityByDoctorId(objDoctorSpeciality);
                if (lstDoctorSpeciality != null)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Record Fetch Succesfully";
                    objResponse.objData = lstDoctorSpeciality;
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
        [Route("api/[controller]/GetDoctorSpecialityId")]
        [HttpPost]
        public IActionResult GetDoctorSpecialityId(DoctorSpecialityModel objDoctorSpeciality)
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
                int intDoctorSpecalityId = _repoMaster.GetDoctorSpecialityId(objDoctorSpeciality);
                if (intDoctorSpecalityId != 0)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Record Fetch Succesfully";
                    objResponse.objData = intDoctorSpecalityId;
                    return Ok(objResponse);
                }
                else
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.ValidationFails;
                    objResponse.strMessage = "Record not Fetch Succesfully";
                    objResponse.objData = 0;
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

        [Route("api/[controller]/DeleteDoctorSpecialityByDoctorId")]
        [HttpPost]
        public IActionResult DeleteDoctorSpecialityByDoctorId([FromBody] DoctorSpecialityModel objDoctorSpeciality)
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
                int retVal = _repoMaster.DeleteDoctorSpecialityByDoctorId(objDoctorSpeciality);
                if (retVal > 0)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Record Deleted Succesfully";
                    objResponse.strAccess_Token = "";
                    objResponse.objData = "";
                    return Ok(objResponse);
                }
                else
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Appointments are booked on this speciality,can not delete";
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

        [Route("api/[controller]/GetActiveDoctorSpeciality")]
        [HttpPost]
        public IActionResult GetActiveDoctorSpeciality(DoctorSpecialityModel objDoctorSpeciality)
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
                if (objDoctorSpeciality.intDoctorId == 0)
                {
                    objDoctorSpeciality.intDoctorId = oUserToken.intId;
                }
                List<DoctorSpecialityModel> lstDoctorSpeciality = _repoMaster.GetActiveDoctorSpeciality(objDoctorSpeciality);
                if (lstDoctorSpeciality != null)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Record Fetch Succesfully";
                    objResponse.objData = lstDoctorSpeciality;
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

        [Route("api/[controller]/GetDoctorSpecialityById")]
        [HttpPost]
        public IActionResult GetDoctorSpecialityById(DoctorSpecialityModel objDoctorSpeciality)
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
                DoctorSpecialityModel objDoctorSpecialityModel = _repoMaster.GetDoctorSpecialityById(objDoctorSpeciality);
                if (objDoctorSpecialityModel != null)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Record Fetch Succesfully";
                    objResponse.objData = objDoctorSpecialityModel;
                    return Ok(objResponse);
                }
                else
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.ValidationFails;
                    objResponse.strMessage = "Record not Fetch Succesfully";
                    objResponse.objData = 0;
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

        #region Doctor Education
        [Route("api/[controller]/InsertUpdateDoctorEducation")]
        [HttpPost]
        public IActionResult InsertUpdateDoctorEducation([FromBody] DoctorEducationModel objDoctorEducation)
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
                objDoctorEducation.intCreatedBy = oUserToken.intId;
                int result = _repoMaster.InsertUpdateDoctorEducation(objDoctorEducation);

                if (result > 0)
                {
                    if (objDoctorEducation.intId > 0)
                    {
                        objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                        objResponse.strMessage = "Education Updated Successfully";
                        objResponse.objData = "";
                        return Ok(objResponse);
                    }
                    else
                    {
                        objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                        objResponse.strMessage = "Education Inserted Successfully";
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

        [Route("api/[controller]/GetDoctorEducationByDoctorId")]
        [HttpPost]
        public IActionResult GetDoctorEducationByDoctorId(DoctorEducationModel objDoctorEducation)
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
            {   //Here We are finiding the Dcotor Id from token Which is requirement from APP.
                if (objDoctorEducation.intDoctorId == 0)
                {
                    objDoctorEducation.intDoctorId = oUserToken.intId;
                }
                List<DoctorEducationModel> lstDoctorEducation = _repoMaster.GetDoctorEducationByDoctorId(objDoctorEducation);
                if (lstDoctorEducation != null)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Record Fetch Succesfully";
                    objResponse.objData = lstDoctorEducation;
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
        [Route("api/[controller]/DeleteDoctorEducationByDoctorId")]
        [HttpPost]
        public IActionResult DeleteDoctorEducationByDoctorId([FromBody] DoctorEducationModel objDoctorEducation)
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
                int retVal = _repoMaster.DeleteDoctorEducationByDoctorId(objDoctorEducation);
                if (retVal > 0)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Record Deleted Succesfully";
                    objResponse.strAccess_Token = "";
                    objResponse.objData = "";
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
        [Route("api/[controller]/GetDoctorEducationById")]
        [HttpPost]
        public IActionResult GetDoctorEducationById(DoctorEducationModel objDoctorEducation)
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
                DoctorEducationModel lstDoctorEducation = _repoMaster.GetDoctorEducationById(objDoctorEducation);
                if (lstDoctorEducation != null)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Record Fetch Succesfully";
                    objResponse.objData = lstDoctorEducation;
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

        #region Doctor Experience
        [Route("api/[controller]/InsertUpdateDoctorExperience")]
        [HttpPost]
        public IActionResult InsertUpdateDoctorExperience([FromBody] DoctorExperienceModel objDoctorExperience)
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
                objDoctorExperience.intCreatedBy = oUserToken.intId;
                int result = _repoMaster.InsertUpdateDoctorExperience(objDoctorExperience);
                if (result > 0)
                {
                    if (objDoctorExperience.intId > 0)
                    {
                        objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                        objResponse.strMessage = "Experience Updated Successfully";
                        objResponse.objData = "";
                        return Ok(objResponse);
                    }
                    else
                    {
                        objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                        objResponse.strMessage = "Experience Inserted Successfully";
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

        [Route("api/[controller]/GetDoctorExperienceByDoctorId")]
        [HttpPost]
        public IActionResult GetDoctorExperienceByDoctorId([FromBody] DoctorExperienceModel objDoctorExperience)
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
            {   //Here We are finiding the Dcotor Id from token Which is requirement from APP.
                if (objDoctorExperience.intDoctorId == 0)
                {
                    objDoctorExperience.intDoctorId = oUserToken.intId;
                }
                List<DoctorExperienceModel> lstDoctorExperience = _repoMaster.GetDoctorExperienceByDoctorId(objDoctorExperience);
                if (lstDoctorExperience != null)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Record Fetch Succesfully";
                    objResponse.objData = lstDoctorExperience;
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
        [Route("api/[controller]/DeleteDoctorExperienceByDoctorId")]
        [HttpPost]
        public IActionResult DeleteDoctorExperienceByDoctorId([FromBody] DoctorExperienceModel objDoctorExperience)
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
                int retVal = _repoMaster.DeleteDoctorExperienceByDoctorId(objDoctorExperience);
                if (retVal > 0)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Record Deleted Succesfully";
                    objResponse.strAccess_Token = "";
                    objResponse.objData = "";
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

        [Route("api/[controller]/GetDoctorExperienceById")]
        [HttpPost]
        public IActionResult GetDoctorExperienceById([FromBody] DoctorExperienceModel objDoctorExperience)
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
                DoctorExperienceModel lstDoctorExperience = _repoMaster.GetDoctorExperienceById(objDoctorExperience);
                if (lstDoctorExperience != null)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Record Fetch Succesfully";
                    objResponse.objData = lstDoctorExperience;
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

        #region Doctor slots count
        [Route("api/[controller]/InsertUpdateDoctorSlotsCount")]
        [HttpPost]
        public IActionResult InsertUpdateDoctorSlotsCount([FromBody] DoctorSlots objDoctorSlots)
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
                objDoctorSlots.intCreatedBy = oUserToken.intId;
                int result = _repoMaster.InsertUpdateDoctorSlotsCount(objDoctorSlots);
                if (result > 0)
                {
                    if (objDoctorSlots.intId > 0)
                    {
                        objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                        objResponse.strMessage = "Max slot count Successfully";
                        objResponse.objData = "";
                        return Ok(objResponse);
                    }
                    else
                    {
                        objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                        objResponse.strMessage = "Max slot count Inserted Successfully";
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

        [Route("api/[controller]/GetDoctorSlotsCountsByDoctorIdSpecialtyId")]
        [HttpPost]
        public IActionResult GetDoctorSlotsCountsByDoctorIdSpecialtyId([FromBody] DoctorSlots objDoctorSlots)
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
                DoctorSlots objDoctorSlot = _repoMaster.GetDoctorSlotsCountsByDoctorIdSpecialtyId(objDoctorSlots);
                if (objDoctorSlot != null)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Record Fetch Succesfully";
                    objResponse.objData = objDoctorSlot;
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

        [Route("api/[controller]/GetDoctorSlotsCountsByDoctorId")]
        [HttpPost]
        public IActionResult GetDoctorSlotsCountsByDoctorId([FromBody] DoctorSlots objDoctorSlots)
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
                List<DoctorSlots> objDoctorSlot = _repoMaster.GetDoctorSlotsCountsByDoctorId(objDoctorSlots);
                if (objDoctorSlot != null)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Record Fetch Succesfully";
                    objResponse.objData = objDoctorSlot;
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

        #region report
        [Route("api/[controller]/GetPatientCountByGenderMonthWise")]
        [HttpPost]
        public IActionResult GetPatientCountByGenderMonthWise()
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
                List<ReportPatientCount> lstReportPatientCount = _repoMaster.GetPatientCountByGenderMonthWise();
                if (lstReportPatientCount.Count > 0)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Record fetched Succesfully";
                    objResponse.strAccess_Token = "";
                    objResponse.objData = lstReportPatientCount;
                    return Ok(objResponse);
                }
                else
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.ValidationFails;
                    objResponse.strMessage = "No record found";
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

        [Route("api/[controller]/GetPatientCountYearWise")]
        [HttpPost]
        public IActionResult GetPatientCountYearWise()
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
                ReportPatientCount objReportPatientCount = _repoMaster.GetPatientCountYearWise();

                objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                objResponse.strMessage = "record found";
                objResponse.strAccess_Token = "";
                objResponse.objData = objReportPatientCount;
                return Ok(objResponse);

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

        [Route("api/[controller]/GetRecentPatientAppointments")]
        [HttpPost]
        public IActionResult GetRecentPatientAppointments(PatientAppointmentModel objPatientAppointmentModel)
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

                List<PatientAppointmentModel> objPatientAppointments = _repoMaster.GetRecentPatientAppointments(objPatientAppointmentModel.intStatusId);
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
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
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

        [Route("api/[controller]/GetPatientCountByDepartment")]
        [HttpPost]
        public IActionResult GetPatientCountByDepartment()
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
                List<ReportPatientCount> lstReportPatientCount = _repoMaster.GetPatientCountByDepartment();
                if (lstReportPatientCount.Count >= 0)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Record fetched Succesfully";
                    objResponse.strAccess_Token = "";
                    objResponse.objData = lstReportPatientCount;
                    return Ok(objResponse);
                }
                else
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.ValidationFails;
                    objResponse.strMessage = "No record found";
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

        [Route("api/[controller]/GetRecentPatient")]
        [HttpPost]
        public IActionResult GetRecentPatient()
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
                
                List<PatientAppointmentModel> lstPatientAppointments = _repoMaster.GetRecentPatient();
                if (lstPatientAppointments != null)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Record Fetch Succesfully";
                    objResponse.strAccess_Token = "";
                    objResponse.objData = lstPatientAppointments;
                    return Ok(objResponse);
                }
                else
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Record not Fetch Succesfully";
                    objResponse.strAccess_Token = "";
                    objResponse.objData = lstPatientAppointments;
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

        [Route("api/[controller]/GetPatientAgeGraph")]
        [HttpPost]
        public IActionResult GetPatientAgeGraph()
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

                List<int> lstPatientCount = _repoMaster.GetPatientAgeGraph();
                if (lstPatientCount != null)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Record Fetch Succesfully";
                    objResponse.strAccess_Token = "";
                    objResponse.objData = lstPatientCount;
                    return Ok(objResponse);
                }
                else
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Record not Fetch Succesfully";
                    objResponse.strAccess_Token = "";
                    objResponse.objData = lstPatientCount;
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

        #region
        [Route("api/[controller]/GetPatientCountYearDoctorWise")]
        [HttpPost]
        public IActionResult GetPatientCountYearDoctorWise(DoctorsModel objDoctorModel)
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
                if (objDoctorModel.intId == 0)
                {
                    objDoctorModel.intId = oUserToken.intId;
                }
                ReportPatientCount objReportPatientCount = _repoMaster.GetPatientCountYearDoctorWise(objDoctorModel.intId);

                objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                objResponse.strMessage = "record found";
                objResponse.strAccess_Token = "";
                objResponse.objData = objReportPatientCount;
                return Ok(objResponse);

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
        [Route("api/[controller]/GetPatientCountByGenderMonthDoctorWise")]
        [HttpPost]
        public IActionResult GetPatientCountByGenderMonthDoctorWise(DoctorsModel objDoctorModel)
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
                if (objDoctorModel.intId == 0)
                {
                    objDoctorModel.intId = oUserToken.intId;
                }
                List<ReportPatientCount> lstReportPatientCount = _repoMaster.GetPatientCountByGenderMonthDoctorWise(objDoctorModel.intId);
                if (lstReportPatientCount.Count > 0)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Record fetched Succesfully";
                    objResponse.strAccess_Token = "";
                    objResponse.objData = lstReportPatientCount;
                    return Ok(objResponse);
                }
                else
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.ValidationFails;
                    objResponse.strMessage = "No record found";
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

        [Route("api/[controller]/GetPatientHealthCheckUp_ByPatientId")]
        [HttpPost]
        public IActionResult GetPatientHealthCheckUp_ByPatientId(PatientModel objPatient)
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
                if (objPatient.intId == 0)
                {
                    objPatient.intId = oUserToken.intId;
                }
                PatientModel objPatientModel = _repoMaster.GetPatientHealthCheckUp_ByPatientId(objPatient.intId);

                objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                objResponse.strMessage = "Record fetched Succesfully";
                objResponse.strAccess_Token = "";
                objResponse.objData = objPatientModel;
                return Ok(objResponse);

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

        [Route("api/[controller]/GetRecentPatientAppointments_ByDoctor")]
        [HttpPost]
        public IActionResult GetRecentPatientAppointments_ByDoctor(DoctorsModel objDoctorModel)
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
                if (objDoctorModel.intId == 0)
                {
                    objDoctorModel.intId = oUserToken.intId;
                }
                List<PatientAppointmentModel> objPatientAppointments = _repoMaster.GetRecentPatientAppointments_ByDoctor(objDoctorModel.intId);
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
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
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

        [Route("api/[controller]/GetRecentPatient_ByDoctor")]
        [HttpPost]
        public IActionResult GetRecentPatient_ByDoctor(DoctorsModel objDoctorModel)
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
                if (objDoctorModel.intId == 0)
                {
                    objDoctorModel.intId = oUserToken.intId;
                }
                List<PatientAppointmentModel> objPatientAppointments = _repoMaster.GetRecentPatient_ByDoctor(objDoctorModel.intId);
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
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
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
        #region
        [Route("api/[controller]/GetBodyMeasurement")]
        [HttpPost]
        public IActionResult GetBodyMeasurement()
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

                List<BodyMeasurementModel> lstBodyMeasurement = _repoMaster.GetBodyMeasurement();
                if (lstBodyMeasurement != null)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Record Fetch Succesfully";
                    objResponse.strAccess_Token = "";
                    objResponse.objData = lstBodyMeasurement;
                    return Ok(objResponse);
                }
                else
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
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

        [Route("api/[controller]/GetRecentPatientAppointmentsAll")]
        [HttpPost]
        public IActionResult GetRecentPatientAppointmentsAll()
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

                List<PatientAppointmentModel> objPatientAppointments = _repoMaster.GetRecentPatientAppointmentsAll();
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
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
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

        #region


        [Route("api/[controller]/GetEmployeePlannerByEmployeeId")]
        [HttpPost]
        public IActionResult GetEmployeePlannerByEmployeeId([FromBody] ReportModel reportModel)
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

                List<EmployeePlannerModel> lstemployeePlanner = _repoMaster.GetEmployeePlannerByEmployeeId(reportModel.intYearId, reportModel.intMonthId, reportModel.intEmployeeId).ToList();
                if (lstemployeePlanner != null)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Record Fetch Succesfully";
                    objResponse.strAccess_Token = "";
                    objResponse.objData = lstemployeePlanner;
                    return Ok(objResponse);
                }
                else
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
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

        #region relation

        [Route("api/[controller]/InsertUpdateRelation")]
        [HttpPost]
        public IActionResult InsertUpdateRelation([FromBody] RelationModel objRelationModel)
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
                objRelationModel.intCreatedBy = oUserToken.intId;
                int result = _repoMaster.InsertUpdateRelation(objRelationModel);

                if (result > 0)
                {
                    if (objRelationModel.intId > 0)
                    {
                        objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                        objResponse.strMessage = "Relation Updated Successfully";
                        objResponse.objData = "";
                        return Ok(objResponse);
                    }
                    else
                    {
                        objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                        objResponse.strMessage = "Relation Inserted Successfully";
                        objResponse.objData = "";
                        return Ok(objResponse);
                    }
                }
                else if (result == -1)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Relation already exists";
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
        [Route("api/[controller]/GetRelation")]
        [HttpPost]
        public IActionResult GetRelation()
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

                List<RelationModel> lstRelationModel = _repoMaster.GetRelation();
                if (lstRelationModel != null)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Record Fetch Succesfully";
                    objResponse.strAccess_Token = "";
                    objResponse.objData = lstRelationModel;
                    return Ok(objResponse);
                }
                else
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
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

        [Route("api/[controller]/GetRelationForAll")]
        [HttpPost]
        public IActionResult GetRelationForAll()
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

                List<RelationModel> lstRelationModel = _repoMaster.GetRelationForAll();
                if (lstRelationModel != null)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Record Fetch Succesfully";
                    objResponse.strAccess_Token = "";
                    objResponse.objData = lstRelationModel;
                    return Ok(objResponse);
                }
                else
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
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

        [Route("api/[controller]/GetRelationById")]
        [HttpPost]
        public IActionResult GetRelationById([FromBody] RelationModel objRelation)
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

                RelationModel objRelationModel = _repoMaster.GetRelationById(objRelation.intId);
                if (objRelationModel != null)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Record Fetch Succesfully";
                    objResponse.strAccess_Token = "";
                    objResponse.objData = objRelationModel;
                    return Ok(objResponse);
                }
                else
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
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

        [Route("api/[Controller]/DeleteRelationById")]
        [HttpPost]
        public IActionResult DeleteRelationById([FromBody] RelationModel objRelation)
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
                int result = _repoMaster.DeleteRelationById(objRelation);

                if (result > 0)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Record Delete Succesfully";
                    objResponse.objData = "";
                    return Ok(objResponse);
                }
                else
                {

                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
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

        #region Graph
        [Route("api/[controller]/GetDoctorSpecialtyGraph")]
        [HttpPost]
        public IActionResult GetDoctorSpecialtyGraph()
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

                List<DoctorsModel> lstRelationModel = _repoMaster.GetDoctorSpecialtyGraph();
                if (lstRelationModel != null)
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
                    objResponse.strMessage = "Record Fetch Succesfully";
                    objResponse.strAccess_Token = "";
                    objResponse.objData = lstRelationModel;
                    return Ok(objResponse);
                }
                else
                {
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
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

        [Route("api/[controller]/GetUnattendedPatientAppointments")]
        [HttpPost]
        public IActionResult GetUnattendedPatientAppointments(PatientAppointmentModel objPatientAppointmentModel)
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

                List<PatientAppointmentModel> objPatientAppointments = _repoMaster.GetUnattendedPatientAppointments(objPatientAppointmentModel.intStatusId,objPatientAppointmentModel.ddtstartTime,objPatientAppointmentModel.ddtEndtime,objPatientAppointmentModel.intDoctorId);
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
                    objResponse.intStatusCode = (int)E_RESPONSESTATUS.Success;
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
