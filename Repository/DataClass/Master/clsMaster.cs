using HospitalManagement.DbConnect;
using HospitalManagement.Repository.DataClass.Doctors;
using HospitalManagement.Repository.Interface.Master;
using HospitalManagement_Models;
using HospitalManagement_Models.Doctors;
using HospitalManagement_Models.Master;
using ISM_AppTrackerModels;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalManagement.Repository.DataClass.Master
{
    public class clsMaster : IMaster     
    {
        #region City
        public int InsertUpdateCity(CityModel objCityModel)
        {
            DbContext dbContext = new DbContext();
            try
            {
                int result = 0;
                string strQuery = "Select * from mascity Where varName = '" + objCityModel.strName + "' And intId !=" + objCityModel.intId + " and intActive = 1;";
                int Validation = dbContext.ExecuteScaler(strQuery, "DbContext");
                if (Validation <= 0)
                {
                    strQuery = "call mascity_CU_SP (?,?,?,?,?,now());";
                    MySqlParameter[] param = {
                new MySqlParameter("@intId", objCityModel.intId),
                new MySqlParameter("@varName", objCityModel.strName),
                new MySqlParameter("@intStateId", objCityModel.intStateId),
                new MySqlParameter("@intActive", objCityModel.intActive),
                new MySqlParameter("@intCreatedBy", objCityModel.intCreatedBy),
                new MySqlParameter("@dttCreationDate", objCityModel.dttCreationDate)

                };
                    result = dbContext.ExecuteCommandWithParameter(strQuery, param, true, "DbContext");
                }
                else
                {
                    return -1;
                }
                return result;
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }

        public List<CityModel> GetCity()
        {
            DbContext dbContext = new DbContext();
            List<CityModel> lstCity = new List<CityModel>();
            try
            {
                string strQuery = $"call mascity_R_SP (1,0);";
                DataTable dt = dbContext.GetDataTable(strQuery, "DbContext");
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        CityModel objCityModel = new CityModel();

                        objCityModel.intId = Convert.ToInt32(dr["intId"]);
                        objCityModel.strName = Convert.ToString(dr["varName"]);
                        if(Convert.ToInt32(dr["stateActive"])==1)
                            objCityModel.intStateId = Convert.ToInt32(dr["intStateId"]);
                        objCityModel.strStateName = Convert.ToString(dr["stateName"]);
                        if (Convert.ToInt32(dr["countryActive"]) == 1)
                            objCityModel.intCountryId = Convert.ToInt32(dr["intCountryId"]);
                        else
                            objCityModel.intStateId = 0;
                        objCityModel.strCountryName = Convert.ToString(dr["CountryName"]);
                        objCityModel.intActive = Convert.ToInt32(dr["intActive"]);
                        objCityModel.intCreatedBy = Convert.ToInt32(dr["intCreatedBy"]);
                        objCityModel.strCreationDate = Convert.ToDateTime(dr["dttCreationDate"]).ToString("yyyy-MM-dd");
                        lstCity.Add(objCityModel);
                    }
                    return lstCity;
                }

                else
                {
                    return lstCity;
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }
        public CityModel GetCityById(int id)
        {
            DbContext dbContext = new DbContext();
            try
            {
                CityModel objCityModel = new CityModel();
                string strQuery = $"call mascity_R_SP(2," + id + ");";
                DataTable dt = dbContext.GetDataTable(strQuery, "DbContext");
                if (dt.Rows.Count > 0)
                {
                    objCityModel.intId = Convert.ToInt32(dt.Rows[0]["intId"]);
                    objCityModel.strName = Convert.ToString(dt.Rows[0]["varName"]);
                    objCityModel.intStateId = Convert.ToInt32(dt.Rows[0]["intStateId"]);
                    objCityModel.intActive = Convert.ToInt32(dt.Rows[0]["intActive"]);
                    objCityModel.intCreatedBy = Convert.ToInt32(dt.Rows[0]["intCreatedBy"]);
                    objCityModel.dttCreationDate = Convert.ToDateTime(dt.Rows[0]["dttCreationDate"]);

                }
                return objCityModel;
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }
        public int DeleteCityByid(CityModel objCityModel)
        {
            DbContext dbContext = new DbContext();
            try
            {
                string strSQL = $"call mascity_D_SP (?);";
                MySqlParameter[] param = {
                    new MySqlParameter("@intId",objCityModel.intId),

                };

                return dbContext.ExecuteCommandWithParameter(strSQL, param, false, "DbContext");
            }

            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }
        #endregion

        #region Locations
        public int InsertUpdateLocations(LocationsModel objLocationsModel)
        {
            DbContext dbContext = new DbContext();
            try
            {
                string strPhoneEmailCheck = "call PatentEmailCheck_R_SP(5,'" + objLocationsModel.strEmail + "','');";
                int validation = dbContext.ExecuteScaler(strPhoneEmailCheck, "DbContext");
                if (validation <= 0)
                {
                    strPhoneEmailCheck = "call PatentEmailCheck_R_SP(6,'','" + objLocationsModel.strContactNumber + "');";
                    validation = dbContext.ExecuteScaler(strPhoneEmailCheck, "DbContext");
                    if (validation <= 0)
                    {
                        string strQuery = "Select * from maslocations Where varName = '" + objLocationsModel.strName + "' and intId !=" + objLocationsModel.intId + " and intActive = 1;";
                        validation = dbContext.ExecuteScaler(strQuery, "DbContext");
                        if (validation <= 0)
                        {
                            strQuery = "Select * from maslocations Where varEmail = '" + objLocationsModel.strEmail + "' and intId !=" + objLocationsModel.intId + " and intActive = 1;";
                            validation = dbContext.ExecuteScaler(strQuery, "DbContext");
                            if (validation <= 0)
                            {
                                strQuery = "Select * from maslocations Where varContactNumber = '" + objLocationsModel.strContactNumber + "' and intId !=" + objLocationsModel.intId + " and intActive = 1;";
                                validation = dbContext.ExecuteScaler(strQuery, "DbContext");
                                if (validation <= 0)
                                {
                                    strQuery = "call maslocations_CU_SP (?,?,?,?,?,?,?,?,?,?,?,?,?);";
                                    MySqlParameter[] param = {
                                        new MySqlParameter("@intId", objLocationsModel.intId),
                                        new MySqlParameter("@varName", objLocationsModel.strName),
                                        new MySqlParameter("@varAddress", objLocationsModel.strAddress),
                                        new MySqlParameter("@intCountry", objLocationsModel.intCountry),
                                        new MySqlParameter("@intCity", objLocationsModel.intCity),
                                        new MySqlParameter("@intState", objLocationsModel.intState),
                                        new MySqlParameter("@varEmail", objLocationsModel.strEmail),
                                        new MySqlParameter("@varContactNumber", objLocationsModel.strContactNumber),
                                        new MySqlParameter("@intActive", objLocationsModel.intActive),
                                        new MySqlParameter("@intCreatedBy", objLocationsModel.intCreatedBy),
                                        new MySqlParameter("@dttCreationDate", DateTime.Now),
                                        new MySqlParameter("@varLat", objLocationsModel.strLat),
                                        new MySqlParameter("@varLon", objLocationsModel.strLon),

                                    };
                                    int result = dbContext.ExecuteCommandWithParameter(strQuery, param, true, "DbContext");
                                    return result;
                                }
                                else
                                    return -2;//mobile already exists
                            }
                            else
                                return -1; //email already exists
                        }
                        else
                            return -3;  //location already exists                      
                    }
                    else
                        return -2;
                }
                else
                    return -1;

            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }

        public List<LocationsModel> GetLocations()
        {
            DbContext dbContext = new DbContext();
            List<LocationsModel> lstLocations = new List<LocationsModel>();
            try
            {
                string strQuery = $"call maslocations_R_SP (1,0);";
                DataTable dt = dbContext.GetDataTable(strQuery, "DbContext");
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        LocationsModel objLocationsModel = new LocationsModel();

                        objLocationsModel.intId = Convert.ToInt32(dr["intId"]);
                        objLocationsModel.strName = Convert.ToString(dr["varName"]);
                        objLocationsModel.strAddress = Convert.ToString(dr["varAddress"]);
                        objLocationsModel.intCountry = Convert.ToInt32(dr["intCountry"]);
                        objLocationsModel.strCountryName = Convert.ToString(dr["varCountryName"]);
                        objLocationsModel.intCity = Convert.ToInt32(dr["intCity"]);
                        objLocationsModel.strCityName = Convert.ToString(dr["VarCityName"]);
                        objLocationsModel.intState = Convert.ToInt32(dr["intState"]);
                        objLocationsModel.strStateName = Convert.ToString(dr["varStateName"]);
                        objLocationsModel.strEmail = Convert.ToString(dr["varEmail"]);
                        objLocationsModel.strContactNumber = Convert.ToString(dr["varContactNumber"]);
                        objLocationsModel.intActive = Convert.ToInt32(dr["intActive"]);
                        objLocationsModel.intCreatedBy = Convert.ToInt32(dr["intCreatedBy"]);
                        objLocationsModel.strCreationDate = Convert.ToDateTime(dr["dttCreationDate"]).ToString("yyyy-MM-dd");
                        objLocationsModel.strLat = Convert.ToString(dr["varLat"]);
                        objLocationsModel.strLon = Convert.ToString(dr["varLon"]);
                        lstLocations.Add(objLocationsModel);
                    }
                    return lstLocations;
                }

                else
                {
                    return lstLocations;
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }
        public LocationsModel GetLocationsById(int intId)
        {
            DbContext dbContext = new DbContext();
            try            
            {
                LocationsModel objLocationsModel = new LocationsModel();
                string strQuery = $"call maslocations_R_SP(2," + intId + ");";
                DataTable dt = dbContext.GetDataTable(strQuery, "DbContext");
                if (dt.Rows.Count > 0)
                {
                    objLocationsModel.intId = Convert.ToInt32(dt.Rows[0]["intId"]);
                    objLocationsModel.strName = Convert.ToString(dt.Rows[0]["varName"]);
                    objLocationsModel.strAddress = Convert.ToString(dt.Rows[0]["varAddress"]);
                    if (Convert.ToInt32(dt.Rows[0]["cityActive"]) == 1)
                        objLocationsModel.intCity = Convert.ToInt32(dt.Rows[0]["intCity"]);
                    if (Convert.ToInt32(dt.Rows[0]["stateActive"]) == 1)
                        objLocationsModel.intState = Convert.ToInt32(dt.Rows[0]["intState"]);
                    else
                    {
                        objLocationsModel.intCity = 0;
                    }
                    
                    if (Convert.ToInt32(dt.Rows[0]["countryActive"])==1)
                        objLocationsModel.intCountry = Convert.ToInt32(dt.Rows[0]["intCountry"]);
                    else
                    {
                        objLocationsModel.intCity = 0;
                        objLocationsModel.intState = 0;
                    }
                    objLocationsModel.strCountryName = Convert.ToString(dt.Rows[0]["varCountryName"]);
                    
                    objLocationsModel.strCityName = Convert.ToString(dt.Rows[0]["VarCityName"]);
                   
                    objLocationsModel.strStateName = Convert.ToString(dt.Rows[0]["varStateName"]);
                    objLocationsModel.strEmail = Convert.ToString(dt.Rows[0]["varEmail"]);
                    objLocationsModel.strContactNumber = Convert.ToString(dt.Rows[0]["varContactNumber"]);
                    objLocationsModel.intActive = Convert.ToInt32(dt.Rows[0]["intActive"]);
                    objLocationsModel.intCreatedBy = Convert.ToInt32(dt.Rows[0]["intCreatedBy"]);
                    objLocationsModel.strCreationDate = Convert.ToDateTime(dt.Rows[0]["dttCreationDate"]).ToString("yyyy-MM-dd");
                    objLocationsModel.strLat = Convert.ToString(dt.Rows[0]["varLat"]);
                    objLocationsModel.strLon = Convert.ToString(dt.Rows[0]["varLon"]);
                   
                }
                return objLocationsModel;
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }
        public int DeleteLocationByid(LocationsModel objLocationsModel)
        {
            DbContext dbContext = new DbContext();
            try
            {
                string strSQL = $"call maslocations_D_SP (?);";
                MySqlParameter[] param = {
                    new MySqlParameter("@intId",objLocationsModel.intId),

                };

                return dbContext.ExecuteCommandWithParameter(strSQL, param, false, "DbContext");
            }

            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }
        #endregion

        #region Slots
        public int InsertUpdateSlots(SlotsModel objSlotsModel)
        {
            DbContext dbContext = new DbContext();
            string strQuery = "";
            int intValidation = -1;
            try
            {
                int result = 0;
                if (Convert.ToDateTime(objSlotsModel.strStartTime) >= Convert.ToDateTime(objSlotsModel.strEndTime))
                {
                    return -1;
                }
                
                else
                {
                    strQuery = "select intId from masslots where (timediff('" + objSlotsModel.strStartTime + "',varStartTime) >=0 and timediff(varEndTime,'" + objSlotsModel.strEndTime + "') >=0) and intId != " + objSlotsModel.intId + " and intActive=1;";
                    intValidation = dbContext.ExecuteScaler(strQuery, "DbContext");
                    if (intValidation <= 0)
                    {
                        strQuery = "call masslots_CU_SP (?,?,?,?,?,now());";
                        MySqlParameter[] param = {
                        new MySqlParameter("@intId", objSlotsModel.intId),
                        new MySqlParameter("@varStartTime", objSlotsModel.strStartTime),
                        new MySqlParameter("@varEndTime", objSlotsModel.strEndTime),
                        new MySqlParameter("@intActive", objSlotsModel.intActive),
                        new MySqlParameter("@intCreatedBy", objSlotsModel.intCreatedBy),
                        new MySqlParameter("@dttCreationDate", objSlotsModel.strCreationDate)
                    };
                        result = dbContext.ExecuteCommandWithParameter(strQuery, param, true, "DbContext");
                    }
                }                
                return result;
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }   
        public List<SlotsModel> GetSlots()
        {
            DbContext dbContext = new DbContext();
            List<SlotsModel> lstSlots = new List<SlotsModel>();
            try
            {
                string strQuery = $"call masslots_R_SP (1,0,0);";
                DataTable dt = dbContext.GetDataTable(strQuery, "DbContext");
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        SlotsModel objSlotsModel = new SlotsModel();

                        objSlotsModel.intId = Convert.ToInt32(dr["intId"]);
                        objSlotsModel.strStartTime = Convert.ToString(dr["varStartTime"]);
                        objSlotsModel.strEndTime = Convert.ToString(dr["varEndTime"]);
                        objSlotsModel.strStartEndTime = objSlotsModel.strStartTime + "-" + objSlotsModel.strEndTime;
                        objSlotsModel.intActive = Convert.ToInt32(dr["intActive"]);
                        objSlotsModel.intCreatedBy = Convert.ToInt32(dr["intCreatedBy"]);
                        objSlotsModel.strCreationDate = Convert.ToDateTime(dr["dttCreationDate"]).ToString("yyyy-MM-dd");
                        lstSlots.Add(objSlotsModel);
                    }
                    return lstSlots;
                }

                else
                {
                    return lstSlots;
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }
        public List<SlotsModel> GetAllSlots()
        {
            DbContext dbContext = new DbContext();
            List<SlotsModel> lstSlots = new List<SlotsModel>();
            try
            {
                string strQuery = $"call masslots_R_SP (5,0,0);";
                DataTable dt = dbContext.GetDataTable(strQuery, "DbContext");
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        SlotsModel objSlotsModel = new SlotsModel();

                        objSlotsModel.intId = Convert.ToInt32(dr["intId"]);
                        objSlotsModel.strStartTime = Convert.ToString(dr["varStartTime"]);
                        objSlotsModel.strEndTime = Convert.ToString(dr["varEndTime"]);
                        objSlotsModel.strStartEndTime = objSlotsModel.strStartTime + "-" + objSlotsModel.strEndTime;
                        objSlotsModel.intActive = Convert.ToInt32(dr["intActive"]);
                        objSlotsModel.intCreatedBy = Convert.ToInt32(dr["intCreatedBy"]);
                        objSlotsModel.strCreationDate = Convert.ToDateTime(dr["dttCreationDate"]).ToString("yyyy-MM-dd");
                        lstSlots.Add(objSlotsModel);
                    }
                    return lstSlots;
                }

                else
                {
                    return lstSlots;
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }
        public List<SlotsModel> GetSlotsById(int intDoctorId)
        {
            DbContext dbContext = new DbContext();
            List<SlotsModel> lstSlots = new List<SlotsModel>();
            try
            {
                string strQuery = $"call masslots_R_SP(3," + intDoctorId + ",NOW());";
                DataTable dt = dbContext.GetDataTable(strQuery, "DbContext");
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        SlotsModel objSlotsModel = new SlotsModel();

                        objSlotsModel.intId = Convert.ToInt32(dr["intId"]);
                        //objSlotsModel.intSlotId = Convert.ToInt32(dr["intSlotId"]);
                        //objSlotsModel.intDoctorId = Convert.ToInt32(dr["intDoctorId"]);
                        //objSlotsModel.intStatusId = Convert.ToInt32(dr["intStatusId"]);
                        objSlotsModel.strStartTime = Convert.ToString(dr["varStartTime"]);
                        objSlotsModel.strEndTime = Convert.ToString(dr["varEndTime"]);
                        objSlotsModel.intActive = Convert.ToInt32(dr["intActive"]);
                        objSlotsModel.intCreatedBy = Convert.ToInt32(dr["intCreatedBy"]);
                        objSlotsModel.strCreationDate = Convert.ToDateTime(dr["dttCreationDate"]).ToString("yyyy-MM-dd");
                        lstSlots.Add(objSlotsModel);
                    }
                    return lstSlots;
                }

                else
                {
                    return lstSlots;
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }      
        public int DeleteSlotsByid(SlotsModel objSlotsModel)
        {
            DbContext dbContext = new DbContext();
            try
            {
                string strSQL = $"call masslots_D_SP (?);";
                MySqlParameter[] param = {
                    new MySqlParameter("@intId",objSlotsModel.intId),

                };
                return dbContext.ExecuteCommandWithParameter(strSQL, param, false, "DbContext");
            }

            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }
        #endregion

        #region Speciality
        public int InsertUpdateSpeciality(SpecialityModel objSpecialityModel)
        {
            DbContext dbContext = new DbContext();
            try
            {
                int result = 0;
                string strQuery = "Select * from masspeciality Where varName = '"+ objSpecialityModel.strName + "' And intId !="+ objSpecialityModel.intId + ";";                
                DataTable dt = dbContext.GetDataTable(strQuery, "DbContext");
                if (dt.Rows.Count == 0)
                {
                    strQuery = "call masspeciality_CU_SP (?,?,?,now(),?);";
                    MySqlParameter[] param = {
                        new MySqlParameter("@intId", objSpecialityModel.intId),
                        new MySqlParameter("@varName", objSpecialityModel.strName),
                        new MySqlParameter("@intCreatedBy", 1),
                        new MySqlParameter("@intActive", objSpecialityModel.intActive),
                        new MySqlParameter("@dttCreationDate", objSpecialityModel.strCreationDate),
                    };
                    result = dbContext.ExecuteCommandWithParameter(strQuery, param, true, "DbContext");
                }
                else
                {
                    return -1;
                }
                return result;
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                return 0;
            }
        }
      
        public List<SpecialityModel> GetSpeciality()
        {
            DbContext dbContext = new DbContext();
            List<SpecialityModel> lstSpeciality = new List<SpecialityModel>();
            try
            {
                string strQuery = $"call masspeciality_R_SP (1,0);";
                DataTable dt = dbContext.GetDataTable(strQuery, "DbContext");
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        SpecialityModel objSpecialityModel = new SpecialityModel();

                        objSpecialityModel.intId = Convert.ToInt32(dr["intId"]);
                        objSpecialityModel.strName = Convert.ToString(dr["varName"]);
                        objSpecialityModel.intCreatedBy = Convert.ToInt32(dr["intCreatedBy"]);
                        objSpecialityModel.strCreationDate = Convert.ToDateTime(dr["dttCreationDate"]).ToString("yyyy-MM-dd");
                        objSpecialityModel.intActive = Convert.ToInt32(dr["intActive"]);
                        lstSpeciality.Add(objSpecialityModel);
                    }
                    return lstSpeciality;
                }
                else
                {
                    return lstSpeciality;
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }
        public List<SpecialityModel> GetAllSpeciality()
        {
            DbContext dbContext = new DbContext();
            List<SpecialityModel> lstSpeciality = new List<SpecialityModel>();
            try
            {
                string strQuery = $"call masspeciality_R_SP (3,0);";
                DataTable dt = dbContext.GetDataTable(strQuery, "DbContext");
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        SpecialityModel objSpecialityModel = new SpecialityModel();

                        objSpecialityModel.intId = Convert.ToInt32(dr["intId"]);
                        objSpecialityModel.strName = Convert.ToString(dr["varName"]);
                        objSpecialityModel.intCreatedBy = Convert.ToInt32(dr["intCreatedBy"]);
                        objSpecialityModel.strCreationDate = Convert.ToDateTime(dr["dttCreationDate"]).ToString("yyyy-MM-dd");
                        objSpecialityModel.intActive = Convert.ToInt32(dr["intActive"]);
                        lstSpeciality.Add(objSpecialityModel);
                    }
                    return lstSpeciality;
                }
                else
                {
                    return lstSpeciality;
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }
        public SpecialityModel GetSpecialityById(int intId)
        {
            DbContext dbContext = new DbContext();
            try
            {
                SpecialityModel objSpecialityModel = new SpecialityModel();
                string strQuery = $"call masspeciality_R_SP(2," + intId + ");";
                DataTable dt = dbContext.GetDataTable(strQuery, "DbContext");
                if (dt.Rows.Count > 0)
                {
                    objSpecialityModel.intId = Convert.ToInt32(dt.Rows[0]["intId"]);
                    objSpecialityModel.strName = Convert.ToString(dt.Rows[0]["varName"]);
                    objSpecialityModel.intCreatedBy = Convert.ToInt32(dt.Rows[0]["intCreatedBy"]);
                    objSpecialityModel.intActive = Convert.ToInt32(dt.Rows[0]["intActive"]);
                    objSpecialityModel.dttCreationDate = Convert.ToDateTime(dt.Rows[0]["dttCreationDate"]);

                }
                return objSpecialityModel;
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }
        public int DeleteSpecialityById(SpecialityModel objSpecialityModel)
        {
            DbContext dbContext = new DbContext();
            try
            {
                string strSQL = $"call masspeciality_D_SP (?);";
                MySqlParameter[] param = {
                    new MySqlParameter("@intId",objSpecialityModel.intId),

                };

                return dbContext.ExecuteCommandWithParameter(strSQL, param, false, "DbContext");
            }

            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }
        #endregion

        #region country state district
        public List<CountryModel> GetCountry()
        {
            DbContext dbContext = new DbContext();
            List<CountryModel> lstCountryModel = new List<CountryModel>();
            try
            {
                string strQuery = $"call mascountry_R_SP (1,0);";
                DataTable dt = dbContext.GetDataTable(strQuery, "DbContext");
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        CountryModel objCountryModel = new CountryModel();
                        objCountryModel.intId = Convert.ToInt32(dt.Rows[i]["intId"]);
                        objCountryModel.strName = Convert.ToString(dt.Rows[i]["varName"]);
                        objCountryModel.strNameOthers = Convert.ToString(dt.Rows[i]["varNameOthers"]);
                        objCountryModel.intCreatedby = Convert.ToInt32(dt.Rows[i]["intCreatedby"]);
                        objCountryModel.dttCreationDate = Convert.ToString(dt.Rows[i]["dttCreationDate"]);
                        objCountryModel.intActive = Convert.ToInt32(dt.Rows[i]["intActive"]);


                        lstCountryModel.Add(objCountryModel);
                    }
                }
                return lstCountryModel;
            }


            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }

        public List<CountryModel> GetCountryForMaster()
        {
            DbContext dbContext = new DbContext();
            List<CountryModel> lstCountryModel = new List<CountryModel>();
            try
            {
                string strQuery = $"call mascountry_R_SP (3,0);";
                DataTable dt = dbContext.GetDataTable(strQuery, "DbContext");
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        CountryModel objCountryModel = new CountryModel();
                        objCountryModel.intId = Convert.ToInt32(dt.Rows[i]["intId"]);
                        objCountryModel.strName = Convert.ToString(dt.Rows[i]["varName"]);
                        objCountryModel.strNameOthers = Convert.ToString(dt.Rows[i]["varNameOthers"]);
                        objCountryModel.intCreatedby = Convert.ToInt32(dt.Rows[i]["intCreatedby"]);
                        objCountryModel.dttCreationDate = Convert.ToString(dt.Rows[i]["dttCreationDate"]);
                        objCountryModel.intActive = Convert.ToInt32(dt.Rows[i]["intActive"]);


                        lstCountryModel.Add(objCountryModel);
                    }
                }
                return lstCountryModel;
            }


            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }

        public int DeleteCountryById(CountryModel objCountryModel)
        {
            DbContext dbContext = new DbContext();
            try
            {
                string strSQL = $"call mascountry_CU_SP (2,?,'','',0,now(),0);";
                MySqlParameter[] param = {
                    new MySqlParameter("@intId",objCountryModel.intId),

                };

                return dbContext.ExecuteCommandWithParameter(strSQL, param, true, "DbContext");
            }

            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }
        public int InsertUpdateCountry(CountryModel objCountryModel)
        {
            DbContext dbContext = new DbContext();
            try
            {
                int result = 0;
                string strQuery = "Select * from mascountry Where varName = '" + objCountryModel.strName + "' And intId !=" + objCountryModel.intId + " and intActive = 1;";
                int Validation = dbContext.ExecuteScaler(strQuery, "DbContext");
                if (Validation <= 0)
                {
                    strQuery = "call mascountry_CU_SP (1,?,?,?,?,now(),?);";
                    MySqlParameter[] param = {
                        new MySqlParameter("@intId", objCountryModel.intId),
                        new MySqlParameter("@varName", objCountryModel.strName),
                        new MySqlParameter("@varNameOthers", " "),
                        new MySqlParameter("@intCreatedBy", objCountryModel.intCreatedby),
                        new MySqlParameter("@intActive", objCountryModel.intActive),                        
                    };
                    result = dbContext.ExecuteCommandWithParameter(strQuery, param, true, "DbContext");
                }
                else
                {
                    return -1;
                }
                return result;
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                return 0;
            }
        }
        public List<StateModel> GetStates()
        {
            DbContext dbContext = new DbContext();
            List<StateModel> lstStateModel = new List<StateModel>();
            try
            {
                string strQuery = $"call masstate_R_SP (2,0);";
                DataTable dt = dbContext.GetDataTable(strQuery, "DbContext");
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        StateModel objStateModel = new StateModel();
                        objStateModel.intId = Convert.ToInt32(dt.Rows[i]["intId"]);
                        objStateModel.strName = Convert.ToString(dt.Rows[i]["varName"]);                       
                        objStateModel.intCreatedby = Convert.ToInt32(dt.Rows[i]["intCreatedby"]);
                        objStateModel.strCountryName = Convert.ToString(dt.Rows[i]["countryName"]);
                        if(Convert.ToInt32(dt.Rows[i]["intCountryActive"]) ==1)
                            objStateModel.intCountryId = Convert.ToInt32(dt.Rows[i]["intCountryId"]);
                        objStateModel.dttCreationDate = Convert.ToString(dt.Rows[i]["dttCreationDate"]);
                        objStateModel.intActive = Convert.ToInt32(dt.Rows[i]["intActive"]);
                        
                        lstStateModel.Add(objStateModel);
                    }
                }
                return lstStateModel;
            }


            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }

        public int DeleteStateyById(StateModel objStateModel)
        {
            DbContext dbContext = new DbContext();
            try
            {
                string strSQL = $"update masstate set intActive=0 where intId = "+ objStateModel.intId + ";";     
                int result= dbContext.ExecuteScaler(strSQL, "DbContext");
                return result;
            }

            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }
        public int InsertUpdateState(StateModel objStateModel)
        {
            DbContext dbContext = new DbContext();
            try
            {
                int result = 0;
                string strQuery = "Select * from masstate Where varName = '" + objStateModel.strName + "' And intId !=" + objStateModel.intId + " and intActive = 1;";
                int Validation = dbContext.ExecuteScaler(strQuery, "DbContext");
                if (Validation <= 0)
                {
                    strQuery = "call masstate_CU_SP (1,?,?,?,?,now(),?);";
                    MySqlParameter[] param = {
                        new MySqlParameter("@intId", objStateModel.intId),
                        new MySqlParameter("@varName", objStateModel.strName),
                        new MySqlParameter("@intCountryId", objStateModel.intCountryId),
                        new MySqlParameter("@intCreatedBy", objStateModel.intCreatedby),
                        new MySqlParameter("@intActive", objStateModel.intActive),
                    };
                    result = dbContext.ExecuteCommandWithParameter(strQuery, param, true, "DbContext");
                }
                else
                {
                    return -1;
                }
                return result;
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                return 0;
            }
        }


        public List<StateModel> GetStateByCountryId(int intCountryID)
        {
            DbContext dbContext = new DbContext();
            List<StateModel> lstStateModel = new List<StateModel>();
            try
            {
                string strSQL = $"call masstate_R_SP(1," + intCountryID + ");";
                DataTable dt = dbContext.GetDataTable(strSQL, "DbContext");

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        StateModel objStateModel = new StateModel();
                        objStateModel.intId = Convert.ToInt32(dt.Rows[i]["intId"]);
                        objStateModel.strName = Convert.ToString(dt.Rows[i]["varName"]);
                        objStateModel.intCountryId = Convert.ToInt32(dt.Rows[i]["intCountryId"]);
                        objStateModel.intActive = Convert.ToInt32(dt.Rows[i]["intActive"]);
                        objStateModel.intCreatedby = Convert.ToInt32(dt.Rows[i]["intCreatedby"]);
                        objStateModel.dttCreationDate = Convert.ToString(dt.Rows[i]["dttCreationDate"]);

                        lstStateModel.Add(objStateModel);
                    }
                } 
                return lstStateModel;
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                return null;
            }
        }

        public List<CityModel> GetCityByStateId(int intStateId)
        {
            DbContext dbContext = new DbContext();
            List<CityModel> lstCityModel = new List<CityModel>();
            try
            {
                string strSQL = $"call mascity_RR_SP  (1," + intStateId + ");";
                DataTable dt = dbContext.GetDataTable(strSQL, "DbContext");

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        CityModel objCityModel = new CityModel();
                        objCityModel.intId = Convert.ToInt32(dt.Rows[i]["intId"]);
                        objCityModel.strName = Convert.ToString(dt.Rows[i]["varName"]);
                        objCityModel.intStateId = Convert.ToInt32(dt.Rows[i]["intStateId"]);
                        objCityModel.intActive = Convert.ToInt32(dt.Rows[i]["intActive"]);
                        objCityModel.intCreatedBy = Convert.ToInt32(dt.Rows[i]["intCreatedBy"]);
                        objCityModel.dttCreationDate = Convert.ToDateTime(dt.Rows[i]["dttCreationDate"]);

                        lstCityModel.Add(objCityModel);
                    }
                }  
                return lstCityModel;
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                return null;
            }
        }
        #endregion

        #region Doctor Skills
        public int InsertUpdateDoctorSkills(DoctorSkillsModel objDoctorSkills)
        {
            DbContext dbContext = new DbContext();
            try
            {
                string strQuery = "call doctorskills_CU_SP (?,?,?,?,?);";
                MySqlParameter[] param = {
                    new MySqlParameter("@intId", objDoctorSkills.intId),
                    new MySqlParameter("@intDoctorId", objDoctorSkills.intDoctorId),
                    new MySqlParameter("@varSkills", objDoctorSkills.strSkills),
                    new MySqlParameter("@intActive", objDoctorSkills.intActive),
                     new MySqlParameter("@intCreatedBy", objDoctorSkills.intCreatedById),
                };
                int result = dbContext.ExecuteCommandWithParameter(strQuery, param, true, "DbContext");
                return result;
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }
        public List<DoctorSkillsModel> GetDoctorSkillsByDoctorId(DoctorSkillsModel objDoctorSkill)
        {
            DbContext dbContext = new DbContext();
            List<DoctorSkillsModel> lstDoctorSkills = new List<DoctorSkillsModel>();
            try
            {
                string strQuery = $"call doctorskills_R_SP (1,"+ objDoctorSkill.intDoctorId + ");";
                DataTable dt = dbContext.GetDataTable(strQuery, "DbContext");
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        DoctorSkillsModel objDoctorSkills = new DoctorSkillsModel();
                        objDoctorSkills.intId = Convert.ToInt32(dr["intId"]);
                        objDoctorSkills.intDoctorId = Convert.ToInt32(dr["intDoctorId"]);
                        objDoctorSkills.strSkills = Convert.ToString(dr["varSkills"]);
                        objDoctorSkills.intActive = Convert.ToInt32(dr["intActive"]);
                        objDoctorSkills.strCreationDate = Convert.ToDateTime(dr["dttCreationDate"]).ToString("dd-MMM-yyyy");
                        objDoctorSkills.strCreatedBy = Convert.ToString(dr["varFirstName"]) + " " + Convert.ToString(dr["varMiddleName"]) + " " + Convert.ToString(dr["varLastName"]);
                        lstDoctorSkills.Add(objDoctorSkills);
                    }
                    return lstDoctorSkills;
                }

                else
                {
                    return lstDoctorSkills;
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }
        public int DeleteDoctorSkillByDoctorId(DoctorSkillsModel objDoctorSkills)
        {
            DbContext dbContext = new DbContext();
            try
            {
                string strSQL = $"call doctorskills_R_SP (3,"+objDoctorSkills.intId+");";                
                return dbContext.ExecuteCommand(strSQL, "DbContext");
            }

            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }
        public DoctorSkillsModel GetDoctorSkillsById(DoctorSkillsModel objDoctorSkill)
        {
            DbContext dbContext = new DbContext();
            DoctorSkillsModel objDoctorSkills = new DoctorSkillsModel();
            try
            {
                string strQuery = $"call doctorskills_R_SP (2," + objDoctorSkill.intId + ");";
                DataTable dt = dbContext.GetDataTable(strQuery, "DbContext");
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        
                        objDoctorSkills.intId = Convert.ToInt32(dr["intId"]);
                        objDoctorSkills.intDoctorId = Convert.ToInt32(dr["intDoctorId"]);
                        objDoctorSkills.strSkills = Convert.ToString(dr["varSkills"]);
                        objDoctorSkills.intActive = Convert.ToInt32(dr["intActive"]);
                        objDoctorSkills.strCreationDate = Convert.ToDateTime(dr["dttCreationDate"]).ToString("dd-MMM-yyyy");
                     //   objDoctorSkills.strCreatedBy = Convert.ToString(dr["varFirstName"]) + " " + Convert.ToString(dr["varMiddleName"]) + " " + Convert.ToString(dr["varLastName"]);
                        
                    }
                    return objDoctorSkills;
                }

                else
                {
                    return objDoctorSkills;
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }
        #endregion

        #region Doctor Speciality
        public int InsertUpdateDoctorSpeciality(DoctorSpecialityModel objDoctorSpeciality)
        {
            DbContext dbContext = new DbContext();
            try
            {
                int result = -1;
                string strPhoneEmailCheck = "select intId from doctorspecialty where intSpecialityId = " + objDoctorSpeciality.intSpecialityId + " and intDoctorId = " + objDoctorSpeciality.intDoctorId + " and intId != "+ objDoctorSpeciality.intId +";";
                int validation = dbContext.ExecuteScaler(strPhoneEmailCheck, "DbContext");
                if (validation <= 0)
                {
                    string strQuery = "call doctorspecialty_CU_SP (?,?,?,?,?);";
                    MySqlParameter[] param = {
                    new MySqlParameter("@intId", objDoctorSpeciality.intId),
                    new MySqlParameter("@intDoctorId", objDoctorSpeciality.intDoctorId),
                    new MySqlParameter("@varSkills", objDoctorSpeciality.intSpecialityId),
                    new MySqlParameter("@intActive", objDoctorSpeciality.intActive),
                    new MySqlParameter("@intCreatedBy", objDoctorSpeciality.intCreatedBy),
                };
                    result = dbContext.ExecuteCommandWithParameter(strQuery, param, true, "DbContext");
                }
                return result;
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }
        public List<DoctorSpecialityModel> GetDoctorSpecialityByDoctorId(DoctorSpecialityModel objDoctorSpecialitys)
        {
            DbContext dbContext = new DbContext();
            List<DoctorSpecialityModel> lstDoctorSpeciality = new List<DoctorSpecialityModel>();
            try
            {
                string strQuery = $"call doctorspecialty_R_SP (1,"+ objDoctorSpecialitys.intDoctorId + ");";
                DataTable dt = dbContext.GetDataTable(strQuery, "DbContext");
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        DoctorSpecialityModel objDoctorSpeciality = new DoctorSpecialityModel();
                        objDoctorSpeciality.intId = Convert.ToInt32(dr["intId"]);
                        objDoctorSpeciality.intDoctorId = Convert.ToInt32(dr["intDoctorId"]);
                        objDoctorSpeciality.intSpecialityId = Convert.ToInt32(dr["intSpecialityId"]);
                        objDoctorSpeciality.strSpeciality = Convert.ToString(dr["varSpeciality"]);
                        objDoctorSpeciality.intActive = Convert.ToInt32(dr["intActive"]);
                        objDoctorSpeciality.strCreationDate = Convert.ToDateTime(dr["dttCreationDate"]).ToString("dd-MMM-yyyy");
                        objDoctorSpeciality.strCreatedBy = Convert.ToString(dr["varFirstName"]) + " " + Convert.ToString(dr["varMiddleName"]) + " " + Convert.ToString(dr["varLastName"]);
                        lstDoctorSpeciality.Add(objDoctorSpeciality);
                    }
                    return lstDoctorSpeciality;
                }
                else
                {
                    return lstDoctorSpeciality;
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }

        public int GetDoctorSpecialityId(DoctorSpecialityModel objDoctorSpeciality)
        {
            DbContext dbContext = new DbContext();
            int intDocSpecialityId = 0;
            try
            {
                string strQuery = $"select intId from doctorspecialty where intDoctorId = "+ objDoctorSpeciality.intDoctorId  + " and intSpecialityId = " + objDoctorSpeciality.intSpecialityId +";";
               intDocSpecialityId = dbContext.ExecuteScaler(strQuery, "DbContext");
                
                return intDocSpecialityId;

            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }

        public int DeleteDoctorSpecialityByDoctorId(DoctorSpecialityModel objDoctorSpeciality)
        {
            DbContext dbContext = new DbContext();
            try
            {
                string appointmentCheck = $"call doctorspecialty_R_SP (5," + objDoctorSpeciality.intId + ");";
                int result = dbContext.ExecuteScaler(appointmentCheck, "DbContext");
                if (result <= 0)
                {
                    string strSQL = $"call doctorspecialty_R_SP (3," + objDoctorSpeciality.intId + ");";
                    return dbContext.ExecuteCommand(strSQL, "DbContext");
                }
                else
                    return -1;
            }

            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }

        public List<DoctorSpecialityModel> GetActiveDoctorSpeciality(DoctorSpecialityModel objDoctorSpecialitys)
        {
            DbContext dbContext = new DbContext();
            List<DoctorSpecialityModel> lstDoctorSpeciality = new List<DoctorSpecialityModel>();
            try
            {
                string strQuery = $"call doctorspecialty_R_SP (4," + objDoctorSpecialitys.intDoctorId + ");";
                DataTable dt = dbContext.GetDataTable(strQuery, "DbContext");
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        DoctorSpecialityModel objDoctorSpeciality = new DoctorSpecialityModel();
                        objDoctorSpeciality.intId = Convert.ToInt32(dr["intId"]);
                        objDoctorSpeciality.intDoctorId = Convert.ToInt32(dr["intDoctorId"]);
                        objDoctorSpeciality.intSpecialityId = Convert.ToInt32(dr["intSpecialityId"]);
                        objDoctorSpeciality.strSpeciality = Convert.ToString(dr["varSpeciality"]);
                        objDoctorSpeciality.intActive = Convert.ToInt32(dr["intActive"]);
                        objDoctorSpeciality.strCreationDate = Convert.ToDateTime(dr["dttCreationDate"]).ToString("dd-MMM-yyyy");
                        objDoctorSpeciality.strCreatedBy = Convert.ToString(dr["varFirstName"]) + " " + Convert.ToString(dr["varMiddleName"]) + " " + Convert.ToString(dr["varLastName"]);
                        lstDoctorSpeciality.Add(objDoctorSpeciality);
                    }
                    return lstDoctorSpeciality;
                }
                else
                {
                    return lstDoctorSpeciality;
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }

        public DoctorSpecialityModel GetDoctorSpecialityById(DoctorSpecialityModel objDoctorSpecialityModel)
        {
            DbContext dbContext = new DbContext();
            DoctorSpecialityModel objDoctorSpeciality = new DoctorSpecialityModel();
            try
            {
                string strQuery = $"call doctorspecialty_R_SP (2," + objDoctorSpecialityModel.intId + ");";
                DataTable dt = dbContext.GetDataTable(strQuery, "DbContext");
                if (dt.Rows.Count > 0)
                {

                    objDoctorSpeciality.intId = Convert.ToInt32(dt.Rows[0]["intId"]);
                    objDoctorSpeciality.intDoctorId = Convert.ToInt32(dt.Rows[0]["intDoctorId"]);
                    objDoctorSpeciality.intSpecialityId = Convert.ToInt32(dt.Rows[0]["intSpecialityId"]);
                    objDoctorSpeciality.strSpeciality = Convert.ToString(dt.Rows[0]["varSpeciality"]);
                    objDoctorSpeciality.intActive = Convert.ToInt32(dt.Rows[0]["intActive"]);
                    objDoctorSpeciality.strCreationDate = Convert.ToDateTime(dt.Rows[0]["dttCreationDate"]).ToString("dd-MMM-yyyy");
                }
                    return objDoctorSpeciality;
                
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }
        #endregion

        #region Doctor Education
        public int InsertUpdateDoctorEducation(DoctorEducationModel objDoctorEducation)
        {
            DbContext dbContext = new DbContext();
            try
            {
                string strQuery = "call doctoreducation_CU_SP (?,?,?,?,?,?,?,?);";
                MySqlParameter[] param = {
                    new MySqlParameter("@intId", objDoctorEducation.intId),
                    new MySqlParameter("@intDoctorId", objDoctorEducation.intDoctorId),
                    new MySqlParameter("@varStartYear", objDoctorEducation.strStartYear),
                    new MySqlParameter("@varEndYear", objDoctorEducation.strEndYear),
                    new MySqlParameter("@varDegree", objDoctorEducation.strDegree),
                    new MySqlParameter("@varInstitute", objDoctorEducation.strInstitute),
                    new MySqlParameter("@intActive", objDoctorEducation.intActive),
                    new MySqlParameter("@intCreatedBy", objDoctorEducation.intCreatedBy),
                };
                int result = dbContext.ExecuteCommandWithParameter(strQuery, param, true, "DbContext");
                return result;
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }
        public List<DoctorEducationModel> GetDoctorEducationByDoctorId(DoctorEducationModel objDoctorEducations)
        {
            DbContext dbContext = new DbContext();
            List<DoctorEducationModel> lstDoctorEducation = new List<DoctorEducationModel>();
            try
            {
                string strQuery = $"call doctoreducation_R_SP (1,"+ objDoctorEducations.intDoctorId + ");";
                DataTable dt = dbContext.GetDataTable(strQuery, "DbContext");
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        DoctorEducationModel objDoctorEducation = new DoctorEducationModel();
                        objDoctorEducation.intId = Convert.ToInt32(dr["intId"]);
                        objDoctorEducation.strStartYear = Convert.ToString(dr["varStartYear"]);
                        objDoctorEducation.strEndYear = Convert.ToString(dr["varEndYear"]);
                        objDoctorEducation.strDegree = Convert.ToString(dr["varDegree"]);
                        objDoctorEducation.strInstitute = Convert.ToString(dr["varInstitute"]);
                        objDoctorEducation.intActive = Convert.ToInt32(dr["intActive"]);
                        objDoctorEducation.strCreationDate = Convert.ToDateTime(dr["dttCreationDate"]).ToString("dd-MMM-yyyy");
                        objDoctorEducation.strCreatedBy = Convert.ToString(dr["varFirstName"]) + " " + Convert.ToString(dr["varMiddleName"]) + " " + Convert.ToString(dr["varLastName"]);
                        lstDoctorEducation.Add(objDoctorEducation);
                    }
                    return lstDoctorEducation;
                }
                else
                {
                    return lstDoctorEducation;
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }
        public int DeleteDoctorEducationByDoctorId(DoctorEducationModel objDoctorEducation)
        {
            DbContext dbContext = new DbContext();
            try
            {
                string strSQL = $"call doctoreducation_R_SP (3," + objDoctorEducation.intId + ");";
                return dbContext.ExecuteCommand(strSQL, "DbContext");
            }

            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }

        public DoctorEducationModel GetDoctorEducationById(DoctorEducationModel DoctorEducation)
        {
            DbContext dbContext = new DbContext();
            DoctorEducationModel objDoctorEducation = new DoctorEducationModel();
            try
            {
                string strQuery = $"call doctoreducation_R_SP (2," + DoctorEducation.intId + ");";
                DataTable dt = dbContext.GetDataTable(strQuery, "DbContext");
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        
                        objDoctorEducation.intId = Convert.ToInt32(dr["intId"]);
                        objDoctorEducation.strStartYear = Convert.ToString(dr["varStartYear"]);
                        objDoctorEducation.strEndYear = Convert.ToString(dr["varEndYear"]);
                        objDoctorEducation.strDegree = Convert.ToString(dr["varDegree"]);
                        objDoctorEducation.strInstitute = Convert.ToString(dr["varInstitute"]);
                        objDoctorEducation.intActive = Convert.ToInt32(dr["intActive"]);
                        objDoctorEducation.strCreationDate = Convert.ToDateTime(dr["dttCreationDate"]).ToString("dd-MMM-yyyy");
                   //     objDoctorEducation.strCreatedBy = Convert.ToString(dr["varFirstName"]) + " " + Convert.ToString(dr["varMiddleName"]) + " " + Convert.ToString(dr["varLastName"]);
                        
                    }
                    return objDoctorEducation;
                }
                else
                {
                    return objDoctorEducation;
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }
        #endregion

        #region Doctor Experience
        public int InsertUpdateDoctorExperience(DoctorExperienceModel objDoctorExperience)
        {
            DbContext dbContext = new DbContext();
            try
            {
                string strQuery = "call doctorexperience_SU_SP (?,?,?,?,?,?,?,?,?);";
                MySqlParameter[] param = {
                    new MySqlParameter("@intId", objDoctorExperience.intId),
                    new MySqlParameter("@intDoctorId", objDoctorExperience.intDoctorId),
                    new MySqlParameter("@varStartYear", objDoctorExperience.strStartYear),
                    new MySqlParameter("@varEndYear", objDoctorExperience.strEndYear),
                    new MySqlParameter("@varPosition", objDoctorExperience.strPosition),
                    new MySqlParameter("@varHospital", objDoctorExperience.strHospital),
                    new MySqlParameter("@varFeedback", objDoctorExperience.strFeedback),
                    new MySqlParameter("@intActive", objDoctorExperience.intActive),
                    new MySqlParameter("@intCreatedBy", objDoctorExperience.intCreatedBy),
                };
                int result = dbContext.ExecuteCommandWithParameter(strQuery, param, true, "DbContext");
                return result;
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }
        public List<DoctorExperienceModel> GetDoctorExperienceByDoctorId(DoctorExperienceModel objDoctorExperienc)
        {
            DbContext dbContext = new DbContext();
            List<DoctorExperienceModel> lstDoctorExperience = new List<DoctorExperienceModel>();
            try
            {
                string strQuery = $"call doctorexperience_R_SP (1,"+ objDoctorExperienc.intDoctorId + ");";
                DataTable dt = dbContext.GetDataTable(strQuery, "DbContext");
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        DoctorExperienceModel objDoctorExperience = new DoctorExperienceModel();
                        objDoctorExperience.intId = Convert.ToInt32(dr["intId"]);
                        objDoctorExperience.strStartYear = Convert.ToString(dr["varStartYear"]);
                        objDoctorExperience.strEndYear = Convert.ToString(dr["varEndYear"]);
                        objDoctorExperience.strPosition = Convert.ToString(dr["varPosition"]);
                        objDoctorExperience.strHospital = Convert.ToString(dr["varHospital"]);
                        objDoctorExperience.strFeedback = Convert.ToString(dr["varFeedback"]);
                        objDoctorExperience.intActive = Convert.ToInt32(dr["intActive"]);
                        objDoctorExperience.strCreationDate = Convert.ToDateTime(dr["dttCreationDate"]).ToString("dd-MMM-yyyy");
                        objDoctorExperience.strCreatedBy = Convert.ToString(dr["varFirstName"]) + " " + Convert.ToString(dr["varMiddleName"]) + " " + Convert.ToString(dr["varLastName"]);

                        lstDoctorExperience.Add(objDoctorExperience);
                    }
                    return lstDoctorExperience;
                }
                else
                {
                    return lstDoctorExperience;
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }
        public int DeleteDoctorExperienceByDoctorId(DoctorExperienceModel objDoctorExperience)
        {
            DbContext dbContext = new DbContext();
            try
            {
                string strSQL = $"call doctorexperience_R_SP (3," + objDoctorExperience.intId + ");";
                return dbContext.ExecuteCommand(strSQL, "DbContext");
            }

            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }

        public DoctorExperienceModel GetDoctorExperienceById(DoctorExperienceModel objDoctorExp)
        {
            DbContext dbContext = new DbContext();
            DoctorExperienceModel objDoctorExperience = new DoctorExperienceModel();
            try
            {
                string strQuery = $"call doctorexperience_R_SP (2," + objDoctorExp.intId + ");";
                DataTable dt = dbContext.GetDataTable(strQuery, "DbContext");
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        
                        objDoctorExperience.intId = Convert.ToInt32(dr["intId"]);
                        objDoctorExperience.strStartYear = Convert.ToString(dr["varStartYear"]);
                        objDoctorExperience.strEndYear = Convert.ToString(dr["varEndYear"]);
                        objDoctorExperience.strPosition = Convert.ToString(dr["varPosition"]);
                        objDoctorExperience.strHospital = Convert.ToString(dr["varHospital"]);
                        objDoctorExperience.strFeedback = Convert.ToString(dr["varFeedback"]);
                        objDoctorExperience.intActive = Convert.ToInt32(dr["intActive"]);
                        objDoctorExperience.strCreationDate = Convert.ToDateTime(dr["dttCreationDate"]).ToString("dd-MMM-yyyy");
                //        objDoctorExperience.strCreatedBy = Convert.ToString(dr["varFirstName"]) + " " + Convert.ToString(dr["varMiddleName"]) + " " + Convert.ToString(dr["varLastName"]);

                    }
                    return objDoctorExperience;
                }
                else
                {
                    return objDoctorExperience;
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }
        #endregion

        #region Doctor Slots
        public int InsertUpdateDoctorSlotsCount(DoctorSlots objDoctorSlots)
        {
            DbContext dbContext = new DbContext();
            try
            {
                string strQuery = "call doctorslotsCount_U_SP (1,?,?);";
                MySqlParameter[] param = {
                    new MySqlParameter("@intId", objDoctorSlots.intId),                    
                    new MySqlParameter("@intMaxSlots", objDoctorSlots.intMaxSlotCount)
                    
                };
                int result = dbContext.ExecuteCommandWithParameter(strQuery, param, true, "DbContext");
                return result;
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }
        public DoctorSlots GetDoctorSlotsCountsByDoctorIdSpecialtyId(DoctorSlots objDoctorSlots)
        {
            DbContext dbContext = new DbContext();
            DoctorSlots objRetDoctorSlots = new DoctorSlots();
            try
            {
                string strQuery = $"call doctorslotsCount_R_SP (1," + objDoctorSlots.intDoctorId + ");";
                DataTable dt = dbContext.GetDataTable(strQuery, "DbContext");
                if (dt.Rows.Count > 0)
                {
                    
                        
                        objRetDoctorSlots.intId = Convert.ToInt32(dt.Rows[0]["intId"]);
                        objRetDoctorSlots.intDoctorId = Convert.ToInt32(dt.Rows[0]["intDoctorId"]);
                        objRetDoctorSlots.intMaxSlotCount = Convert.ToInt32(dt.Rows[0]["intMaxSlots"]);
                        objRetDoctorSlots.intActive = Convert.ToInt32(dt.Rows[0]["intActive"]);
                        objRetDoctorSlots.strCreationDate = Convert.ToDateTime(dt.Rows[0]["dttCreationDate"]).ToString("dd-MMM-yyyy");
                        //objRetDoctorSlots.strCreatedBy = Convert.ToString(dt.Rows[0]["varFirstName"]) + " " + Convert.ToString(dt.Rows[0]["varMiddleName"]) + " " + Convert.ToString(dt.Rows[0]["varLastName"]);                        
                    
                }
                return objRetDoctorSlots;

            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }

        public List<DoctorSlots> GetDoctorSlotsCountsByDoctorId(DoctorSlots objDoctorSlots)
        {
            DbContext dbContext = new DbContext();
            List<DoctorSlots> lstDoctorSlots = new List<DoctorSlots>();
            try
            {
                string strQuery = $"call doctorslotsCount_R_SP (2," + objDoctorSlots.intDoctorId + ");";
                DataTable dt = dbContext.GetDataTable(strQuery, "DbContext");
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        DoctorSlots objRetDoctorSlots = new DoctorSlots();
                        objRetDoctorSlots.intId = Convert.ToInt32(dr["intId"]);
                        objRetDoctorSlots.intDoctorId = Convert.ToInt32(dr["intDoctorId"]);
                        objRetDoctorSlots.strSpeciality = Convert.ToString(dr["specialityName"]);
                        objRetDoctorSlots.intMaxSlotCount = Convert.ToInt32(dr["intMaxSlots"]);
                        objRetDoctorSlots.intActive = Convert.ToInt32(dr["intActive"]);
                        objRetDoctorSlots.strCreationDate = Convert.ToDateTime(dr["dttCreationDate"]).ToString("dd-MMM-yyyy");
                        objRetDoctorSlots.strCreatedBy = Convert.ToString(dr["varFirstName"]) + " " + Convert.ToString(dr["varMiddleName"]) + " " + Convert.ToString(dr["varLastName"]);
                        lstDoctorSlots.Add(objRetDoctorSlots);
                    }
                }
                return lstDoctorSlots;

            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }



        #endregion
        #region report
        public List<ReportPatientCount> GetPatientCountByGenderMonthWise()
        {
            DbContext dbContext = new DbContext();
            List<ReportPatientCount> lstReportPatientCount = new List<ReportPatientCount>();
            try
            {
                string strQuery = $"call Report_PatientCount_Month (1);";
                DataTable dt = dbContext.GetDataTable(strQuery, "DbContext");
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        ReportPatientCount objReportPatientCount = new ReportPatientCount();
                        objReportPatientCount.intmonthno = Convert.ToInt32(dr["monthno"]);
                        objReportPatientCount.strmonthname = Convert.ToString(dr["monthname"]);
                        objReportPatientCount.intmalecout = Convert.ToInt32(dr["malecount"]);
                        objReportPatientCount.intfemalecount = Convert.ToInt32(dr["femalecount"]);                        
                        lstReportPatientCount.Add(objReportPatientCount);
                    } 
                }
                    return lstReportPatientCount;
                
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }
        public ReportPatientCount GetPatientCountYearWise()
        {
            DbContext dbContext = new DbContext();
            ReportPatientCount objReportPatientCount = new ReportPatientCount();
            try
            {
                string strQuery = $"call Report_Patient_Count_CurrentYear (1);";
                objReportPatientCount.intAppointmentCount = dbContext.ExecuteScaler(strQuery, "DbContext");

                strQuery = $"call Report_Patient_Count_CurrentYear (3);";
                objReportPatientCount.intLastYearAppointmentCount = dbContext.ExecuteScaler(strQuery, "DbContext");

                strQuery = $"call Report_Patient_Count_CurrentYear (2);";
                objReportPatientCount.intPatientCount = dbContext.ExecuteScaler(strQuery, "DbContext");

                strQuery = $"call Report_Patient_Count_CurrentYear (4);";
                objReportPatientCount.intLastYearPatientCount = dbContext.ExecuteScaler(strQuery, "DbContext");

                return objReportPatientCount;
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }

        public List<PatientAppointmentModel> GetRecentPatientAppointments(int intstatus)
        {
            DbContext dbContext = new DbContext();

            List<PatientAppointmentModel> lsPatientAppointmentModel = new List<PatientAppointmentModel>();
            try
            {
                string strQuery = "";
                //2-confirmed, 3-completed, 4-5- cancelled
                if (intstatus == 2) 
                    strQuery = $"call Report_Upcoming_appointments(1);";
                 else if(intstatus==3)
                    strQuery = $"call Report_Upcoming_appointments(3);";
                 else
                    strQuery = $"call Report_Upcoming_appointments(4);";
                DataTable dt = dbContext.GetDataTable(strQuery, "DbContext");
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        PatientAppointmentModel objPatientAppointmentsModel = new PatientAppointmentModel();

                        objPatientAppointmentsModel.intId = Convert.ToInt32(dr["intId"]);
                        objPatientAppointmentsModel.intSlotId = Convert.ToInt32(dr["intSlotId"]);
                        objPatientAppointmentsModel.intDoctorId = Convert.ToInt32(dr["intDoctorId"]);
                        objPatientAppointmentsModel.intPatientId = Convert.ToInt32(dr["intPatientId"]);
                        objPatientAppointmentsModel.strSlotDate = Convert.ToDateTime(dr["dttDate"]).ToString("yyyy-MM-dd");                        
                        objPatientAppointmentsModel.strFirstName = Convert.ToString(dr["varFirstName"]);
                        objPatientAppointmentsModel.strMiddleName = Convert.ToString(dr["varMiddleName"]);
                        objPatientAppointmentsModel.strLastName = Convert.ToString(dr["varLastName"]);
                        
                        objPatientAppointmentsModel.strStartTime = Convert.ToString(dr["varStartTime"]);
                        objPatientAppointmentsModel.strEndTime = Convert.ToString(dr["varEndTime"]);
                        
                        objPatientAppointmentsModel.strDocName = Convert.ToString(dr["varDoctorName"]);
                        objPatientAppointmentsModel.strPatientImage = Convert.ToString(dr["varImagePath"]);
                        objPatientAppointmentsModel.strDocImagePath = Convert.ToString(dr["varDocImagePath"]);
                        objPatientAppointmentsModel.strSpeciality = Convert.ToString(dr["SpecialityName"]);
                        lsPatientAppointmentModel.Add(objPatientAppointmentsModel);
                    }
                    return lsPatientAppointmentModel;
                }

                else
                {
                    return lsPatientAppointmentModel;
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }

        }

        public List<PatientAppointmentModel> GetRecentPatientAppointmentsAll()
        {
            DbContext dbContext = new DbContext();

            List<PatientAppointmentModel> lsPatientAppointmentModel = new List<PatientAppointmentModel>();
            try
            {

                string strQuery = $"call Report_Upcoming_appointments(2);";
                DataTable dt = dbContext.GetDataTable(strQuery, "DbContext");
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        PatientAppointmentModel objPatientAppointmentsModel = new PatientAppointmentModel();

                        objPatientAppointmentsModel.intId = Convert.ToInt32(dr["intId"]);
                        objPatientAppointmentsModel.intSlotId = Convert.ToInt32(dr["intSlotId"]);
                        objPatientAppointmentsModel.intDoctorId = Convert.ToInt32(dr["intDoctorId"]);
                        objPatientAppointmentsModel.intPatientId = Convert.ToInt32(dr["intPatientId"]);
                        objPatientAppointmentsModel.strSlotDate = Convert.ToDateTime(dr["dttDate"]).ToString("yyyy-MM-dd");
                        objPatientAppointmentsModel.strFirstName = Convert.ToString(dr["varFirstName"]);
                        objPatientAppointmentsModel.strMiddleName = Convert.ToString(dr["varMiddleName"]);
                        objPatientAppointmentsModel.strLastName = Convert.ToString(dr["varLastName"]);

                        objPatientAppointmentsModel.strStartTime = Convert.ToString(dr["varStartTime"]);
                        objPatientAppointmentsModel.strEndTime = Convert.ToString(dr["varEndTime"]);

                        objPatientAppointmentsModel.strDocName = Convert.ToString(dr["varDoctorName"]);
                        objPatientAppointmentsModel.strPatientImage = Convert.ToString(dr["varImagePath"]);
                        objPatientAppointmentsModel.strDocImagePath = Convert.ToString(dr["varDocImagePath"]);
                        objPatientAppointmentsModel.strSpeciality= Convert.ToString(dr["SpecialityName"]);
                        lsPatientAppointmentModel.Add(objPatientAppointmentsModel);
                    }
                    return lsPatientAppointmentModel;
                }

                else
                {
                    return lsPatientAppointmentModel;
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }

        }

        public List<ReportPatientCount> GetPatientCountByDepartment()
        {
            DbContext dbContext = new DbContext();
            List<ReportPatientCount> lstReportPatientCount = new List<ReportPatientCount>();
            try
            {
                string strQuery = $"call Report_Patient_Department (1);";
                DataTable dt = dbContext.GetDataTable(strQuery, "DbContext");
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        ReportPatientCount objReportPatientCount = new ReportPatientCount();
                        objReportPatientCount.strdepartmentName = Convert.ToString(dr["varDepartmentname"]);
                        objReportPatientCount.intPatientCount = Convert.ToInt32(dr["intPatientCount"]);                        
                        lstReportPatientCount.Add(objReportPatientCount);
                    }
                }
                return lstReportPatientCount;

            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }

        public List<PatientAppointmentModel> GetRecentPatient()
        {
            DbContext dbContext = new DbContext();
            List<PatientAppointmentModel> lstobjPatient = new List<PatientAppointmentModel>();
            try
            {
                string strQuery = $"call Report_Recent_Patient (1,0);";
                DataTable dt = dbContext.GetDataTable(strQuery, "DbContext");
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        PatientAppointmentModel objPatientModel = new PatientAppointmentModel();
                        objPatientModel.intId = Convert.ToInt32(dr["intId"]);
                        objPatientModel.strFirstName = Convert.ToString(dr["varFirstName"]);
                        objPatientModel.strMiddleName = Convert.ToString(dr["varMiddleName"]);
                        objPatientModel.strLastName = Convert.ToString(dr["varLastName"]);
                       
                        objPatientModel.strMobileNo = Convert.ToString(dr["varMobileNo"]);
                        objPatientModel.strEmail = Convert.ToString(dr["varEmail"]);                        
                        
                        
                        objPatientModel.strPatientImage = Convert.ToString(dr["varImagePath"]);
                        objPatientModel.strSlotDate = Convert.ToString(dr["AppointmentCompletionTime"]);
                        objPatientModel.strTreatment = Convert.ToString(dr["varTreatment"]);
                        objPatientModel.strDocName = Convert.ToString(dr["DocName"]);
                        
                        objPatientModel.strDocImagePath = Convert.ToString(dr["varDocImagePath"]);
                        objPatientModel.strSpeciality = Convert.ToString(dr["SpecialityName"]);
                        lstobjPatient.Add(objPatientModel);
                    }
                    return lstobjPatient;
                }

                else
                {
                    return lstobjPatient;
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }

        public List<DoctorsModel> GetDoctorSpecialtyGraph()
        {
            DbContext dbContext = new DbContext();
            List<DoctorsModel> lstDoctorsModel = new List<DoctorsModel>();
            try
            {
                string strQuery = $"call Report_Doctor_R_SP (1,0);";
                DataTable dt = dbContext.GetDataTable(strQuery, "DbContext");
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        DoctorsModel objDoctorsModel = new DoctorsModel();
                        objDoctorsModel.strSpecialityName = Convert.ToString(dr["SpecialityName"]);
                        objDoctorsModel.intDocCount = Convert.ToInt32(dr["docCount"]);
                        lstDoctorsModel.Add(objDoctorsModel);
                    }
                    return lstDoctorsModel;
                }

                else
                {
                    return lstDoctorsModel;
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }

        public List<int> GetPatientAgeGraph()
        {
            DbContext dbContext = new DbContext();
            List<int> agecount = new List<int>();
            try
            {
                string strQuery = $"call patient_R_SP (9,0,'');";
                DataTable dt = dbContext.GetDataTable(strQuery, "DbContext");
                if (dt.Rows.Count > 0)
                {
                    agecount.Add(Convert.ToInt32(dt.Rows[0]["child"]));
                    agecount.Add(Convert.ToInt32(dt.Rows[0]["young"]));
                    agecount.Add(Convert.ToInt32(dt.Rows[0]["oldage"]));

                    
                }

                return agecount;
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }
        #endregion

        #region
        public ReportPatientCount GetPatientCountYearDoctorWise(int intDoctorId)
        {
            DbContext dbContext = new DbContext();
            ReportPatientCount objReportPatientCount = new ReportPatientCount();
            try
            {
                string strQuery = $"call Report_Patient_Count_CurrentYear_By_Doctor (1,"+ intDoctorId + ");";
                objReportPatientCount.intAppointmentCount = dbContext.ExecuteScaler(strQuery, "DbContext");

                strQuery = $"call Report_Patient_Count_CurrentYear_By_Doctor (3," + intDoctorId + ");";
                objReportPatientCount.intLastYearAppointmentCount = dbContext.ExecuteScaler(strQuery, "DbContext");

                strQuery = $"call Report_Patient_Count_CurrentYear_By_Doctor (2," + intDoctorId + ");";
                objReportPatientCount.intPatientCount = dbContext.ExecuteScaler(strQuery, "DbContext");

                strQuery = $"call Report_Patient_Count_CurrentYear_By_Doctor (4," + intDoctorId + ");";
                objReportPatientCount.intLastYearPatientCount = dbContext.ExecuteScaler(strQuery, "DbContext");

                return objReportPatientCount;
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }
        public List<ReportPatientCount> GetPatientCountByGenderMonthDoctorWise(int intDoctorId)
        {
            DbContext dbContext = new DbContext();
            List<ReportPatientCount> lstReportPatientCount = new List<ReportPatientCount>();
            try
            {
                string strQuery = $"call Report_PatientCount_Month_Male_Female_Doctor (1," + intDoctorId + ");";
                DataTable dt = dbContext.GetDataTable(strQuery, "DbContext");
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        ReportPatientCount objReportPatientCount = new ReportPatientCount();
                        objReportPatientCount.intmonthno = Convert.ToInt32(dr["monthno"]);
                        objReportPatientCount.strmonthname = Convert.ToString(dr["monthname"]);
                        objReportPatientCount.intmalecout = Convert.ToInt32(dr["malecount"]);
                        objReportPatientCount.intfemalecount = Convert.ToInt32(dr["femalecount"]);
                        lstReportPatientCount.Add(objReportPatientCount);
                    }
                }
                return lstReportPatientCount;

            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }

        public PatientModel GetPatientHealthCheckUp_ByPatientId(int intPatentId)
        {
            DbContext dbContext = new DbContext();
            PatientModel objPatientModel = new PatientModel();
            objPatientModel.lstWeight = new List<int>();
            objPatientModel.lstHeartRate = new List<int>();
            objPatientModel.lstTemperature = new List<int>();
            objPatientModel.lstBPL = new List<int>();
            objPatientModel.lstSleep = new List<int>();
            objPatientModel.lstHeight = new List<int>();
            objPatientModel.lstBPH = new List<int>();
            int intMeasurement = 0;
            
            try
            {
                string strQuery = $"call Report_Patient_HealthCheckUp_R_SP (1," + intPatentId + ");";
                DataTable dt = dbContext.GetDataTable(strQuery, "DbContext");
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        intMeasurement = Convert.ToInt32(dr["varMeasurement"]);
                        objPatientModel.lstWeight.Add(intMeasurement);
                    }
                   
                }
                strQuery = $"call Report_Patient_HealthCheckUp_R_SP (2," + intPatentId + ");";
                DataTable dtHR = dbContext.GetDataTable(strQuery, "DbContext");
                if (dtHR.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtHR.Rows)
                    {
                        intMeasurement = Convert.ToInt32(dr["varMeasurement"]);
                        objPatientModel.lstHeartRate.Add(intMeasurement);
                    }
                }
                strQuery = $"call Report_Patient_HealthCheckUp_R_SP (3," + intPatentId + ");";
                DataTable dtT = dbContext.GetDataTable(strQuery, "DbContext");
                if (dtT.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtT.Rows)
                    {
                        intMeasurement = Convert.ToInt32(dr["varMeasurement"]);
                        objPatientModel.lstTemperature.Add(intMeasurement);
                    }
                }
                strQuery = $"call Report_Patient_HealthCheckUp_R_SP (4," + intPatentId + ");";
                DataTable dtBPL = dbContext.GetDataTable(strQuery, "DbContext");
                if (dtBPL.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtBPL.Rows)
                    {
                        intMeasurement = Convert.ToInt32(dr["varMeasurement"]);
                        objPatientModel.lstBPL.Add(intMeasurement);
                    }
                }
                strQuery = $"call Report_Patient_HealthCheckUp_R_SP (5," + intPatentId + ");";
                DataTable dtS = dbContext.GetDataTable(strQuery, "DbContext");
                if (dtS.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtS.Rows)
                    {
                        intMeasurement = Convert.ToInt32(dr["varMeasurement"]);
                        objPatientModel.lstSleep.Add(intMeasurement);
                    }
                }
                strQuery = $"call Report_Patient_HealthCheckUp_R_SP (6," + intPatentId + ");";
                DataTable dtH = dbContext.GetDataTable(strQuery, "DbContext");
                if (dtH.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtH.Rows)
                    {
                        intMeasurement = Convert.ToInt32(dr["varMeasurement"]);
                        objPatientModel.lstHeight.Add(intMeasurement);
                    }
                }
                strQuery = $"call Report_Patient_HealthCheckUp_R_SP (7," + intPatentId + ");";
                DataTable dtBPH = dbContext.GetDataTable(strQuery, "DbContext");
                if (dtBPH.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtBPH.Rows)
                    {
                        intMeasurement = Convert.ToInt32(dr["varMeasurement"]);
                        objPatientModel.lstBPH.Add(intMeasurement);
                    }
                }
                return objPatientModel;

            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }
        public List<PatientAppointmentModel> GetRecentPatientAppointments_ByDoctor(int intDoctorId)
        {
            DbContext dbContext = new DbContext();

            List<PatientAppointmentModel> lsPatientAppointmentModel = new List<PatientAppointmentModel>();
            try
            {

                string strQuery = $"call Report_Upcoming_appointments_ByDoctor(1,"+ intDoctorId + ");";
                DataTable dt = dbContext.GetDataTable(strQuery, "DbContext");
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        PatientAppointmentModel objPatientAppointmentsModel = new PatientAppointmentModel();

                        objPatientAppointmentsModel.intId = Convert.ToInt32(dr["intId"]);
                        objPatientAppointmentsModel.intSlotId = Convert.ToInt32(dr["intSlotId"]);
                        objPatientAppointmentsModel.intDoctorId = Convert.ToInt32(dr["intDoctorId"]);
                        objPatientAppointmentsModel.intPatientId = Convert.ToInt32(dr["intPatientId"]);
                        objPatientAppointmentsModel.strSlotDate = Convert.ToDateTime(dr["dttDate"]).ToString("yyyy-MM-dd");
                        objPatientAppointmentsModel.strFirstName = Convert.ToString(dr["varFirstName"]);
                        objPatientAppointmentsModel.strMiddleName = Convert.ToString(dr["varMiddleName"]);
                        objPatientAppointmentsModel.strLastName = Convert.ToString(dr["varLastName"]);

                        objPatientAppointmentsModel.strStartTime = Convert.ToString(dr["varStartTime"]);
                        objPatientAppointmentsModel.strEndTime = Convert.ToString(dr["varEndTime"]);

                        objPatientAppointmentsModel.strDocName = Convert.ToString(dr["varDoctorName"]);
                        objPatientAppointmentsModel.strPatientImage = Convert.ToString(dr["varImagePath"]);
                        objPatientAppointmentsModel.strDocImagePath = Convert.ToString(dr["varDocImagePath"]);
                        objPatientAppointmentsModel.strSpeciality = Convert.ToString(dr["SpecialityName"]);
                        lsPatientAppointmentModel.Add(objPatientAppointmentsModel);
                    }
                    return lsPatientAppointmentModel;
                }

                else
                {
                    return lsPatientAppointmentModel;
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }

        }

        public List<PatientAppointmentModel> GetRecentPatient_ByDoctor(int intDoctorId)
        {
            DbContext dbContext = new DbContext();
            List<PatientAppointmentModel> lstobjPatient = new List<PatientAppointmentModel>();
            try
            {
                string strQuery = $"call Report_Recent_Patient (2,"+ intDoctorId + ");";
                DataTable dt = dbContext.GetDataTable(strQuery, "DbContext");
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        PatientAppointmentModel objPatientModel = new PatientAppointmentModel();
                        objPatientModel.intId = Convert.ToInt32(dr["intId"]);
                        objPatientModel.strFirstName = Convert.ToString(dr["varFirstName"]);
                        objPatientModel.strMiddleName = Convert.ToString(dr["varMiddleName"]);
                        objPatientModel.strLastName = Convert.ToString(dr["varLastName"]);

                        objPatientModel.strMobileNo = Convert.ToString(dr["varMobileNo"]);
                        objPatientModel.strEmail = Convert.ToString(dr["varEmail"]);


                        objPatientModel.strPatientImage = Convert.ToString(dr["varImagePath"]);
                        objPatientModel.strSlotDate = Convert.ToString(dr["AppointmentCompletionTime"]);
                        objPatientModel.strTreatment = Convert.ToString(dr["varTreatment"]);
                        objPatientModel.strDocName = Convert.ToString(dr["DocName"]);

                        objPatientModel.strDocImagePath = Convert.ToString(dr["varDocImagePath"]);
                        objPatientModel.strSpeciality = Convert.ToString(dr["SpecialityName"]);
                        lstobjPatient.Add(objPatientModel);
                    }
                    return lstobjPatient;
                }

                else
                {
                    return lstobjPatient;
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }
        #endregion
        #region BodyMeasurement
        public List<BodyMeasurementModel> GetBodyMeasurement()
        {
            DbContext dbContext = new DbContext();
            List<BodyMeasurementModel> lstCityBodyMeasurementModel = new List<BodyMeasurementModel>();
            try
            {
                string strQuery = $"call masbodymeasurement_R_SP (1,0);";
                DataTable dt = dbContext.GetDataTable(strQuery, "DbContext");
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        BodyMeasurementModel objBodyMeasurementModel = new BodyMeasurementModel();

                        objBodyMeasurementModel.intId = Convert.ToInt32(dr["intId"]);
                        objBodyMeasurementModel.strName = Convert.ToString(dr["varName"]) + "(" + Convert.ToString(dr["strUnit"]) + ")";
                        objBodyMeasurementModel.strUnit = Convert.ToString(dr["strUnit"]);

                        lstCityBodyMeasurementModel.Add(objBodyMeasurementModel);
                    }
                    return lstCityBodyMeasurementModel;
                }

                else
                {
                    return lstCityBodyMeasurementModel;
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }
        #endregion

        #region Relation

        public int InsertUpdateRelation(RelationModel objRelationModel)
        {
            DbContext dbContext = new DbContext();
            try
            {
                int result = -1;
                string strPhoneEmailCheck = "select intId from masrelation where varName = '" + objRelationModel.strName + "'  and intActive=1 and intId != " + objRelationModel.intId + ";";
                int validation = dbContext.ExecuteScaler(strPhoneEmailCheck, "DbContext");
                if (validation <= 0)
                {
                    string strQuery = "call masrelation_CU_SP (1,?,?,?,now(),?);";
                    MySqlParameter[] param = {
                    new MySqlParameter("@intId", objRelationModel.intId),
                    new MySqlParameter("@varName", objRelationModel.strName),                    
                    new MySqlParameter("@intCreatedBy", objRelationModel.intCreatedBy),
                    new MySqlParameter("@intActive", objRelationModel.intActive),
                };
                    result = dbContext.ExecuteCommandWithParameter(strQuery, param, true, "DbContext");
                }
                return result;
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }
        public List<RelationModel> GetRelation()
        {
            DbContext dbContext = new DbContext();
            List<RelationModel> lstRelationModel = new List<RelationModel>();
            try
            {
                string strQuery = $"call masrelation_R_SP (1,0);";
                DataTable dt = dbContext.GetDataTable(strQuery, "DbContext");
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        RelationModel objRelationModel = new RelationModel();

                        objRelationModel.intId = Convert.ToInt32(dr["intId"]);
                        objRelationModel.strName = Convert.ToString(dr["varName"]) ;
                        objRelationModel.intActive = Convert.ToInt32(dt.Rows[0]["intActive"]);
                        objRelationModel.strCreationDate = Convert.ToDateTime(dt.Rows[0]["dttCreationDate"]).ToString("dd-MMM-yyyy");
                        objRelationModel.strCreatedBy = Convert.ToString(dt.Rows[0]["varFirstName"]) + " " + Convert.ToString(dt.Rows[0]["varMiddleName"]) + " " + Convert.ToString(dt.Rows[0]["varLastName"]);


                        lstRelationModel.Add(objRelationModel);
                    }
                    return lstRelationModel;
                }

                else
                {
                    return lstRelationModel;
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }

        public List<RelationModel> GetRelationForAll()
        {
            DbContext dbContext = new DbContext();
            List<RelationModel> lstRelationModel = new List<RelationModel>();
            try
            {
                string strQuery = $"call masrelation_R_SP (3,0);";
                DataTable dt = dbContext.GetDataTable(strQuery, "DbContext");
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        RelationModel objRelationModel = new RelationModel();

                        objRelationModel.intId = Convert.ToInt32(dr["intId"]);
                        objRelationModel.strName = Convert.ToString(dr["varName"]);
                        objRelationModel.intActive = Convert.ToInt32(dr["intActive"]);
                        objRelationModel.strCreationDate = Convert.ToDateTime(dr["dttCreationDate"]).ToString("dd-MMM-yyyy");
                        objRelationModel.strCreatedBy = Convert.ToString(dr["varFirstName"]) + " " + Convert.ToString(dr["varMiddleName"]) + " " + Convert.ToString(dr["varLastName"]);


                        lstRelationModel.Add(objRelationModel);
                    }
                    return lstRelationModel;
                }

                else
                {
                    return lstRelationModel;
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }

        public RelationModel GetRelationById(int intRelationId)
        {
            DbContext dbContext = new DbContext();
            RelationModel objRelationModel = new RelationModel();
            try
            {
                string strQuery = $"call masrelation_R_SP (2," + intRelationId + ");";
                DataTable dt = dbContext.GetDataTable(strQuery, "DbContext");
                if (dt.Rows.Count > 0)
                {


                    objRelationModel.intId = Convert.ToInt32(dt.Rows[0]["intId"]);
                    objRelationModel.strName = Convert.ToString(dt.Rows[0]["varName"]);                    
                    objRelationModel.intActive = Convert.ToInt32(dt.Rows[0]["intActive"]);
                    objRelationModel.strCreationDate = Convert.ToDateTime(dt.Rows[0]["dttCreationDate"]).ToString("dd-MMM-yyyy");
              //      objRelationModel.strCreatedBy = Convert.ToString(dt.Rows[0]["varFirstName"]) + " " + Convert.ToString(dt.Rows[0]["varMiddleName"]) + " " + Convert.ToString(dt.Rows[0]["varLastName"]);                        

                }
                return objRelationModel;

            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }

        public int DeleteRelationById(RelationModel objRelationModel)
        {
            DbContext dbContext = new DbContext();
            try
            {
                string strSQL = $"update masrelation set intActive=0 where intId = " + objRelationModel.intId + ";";
                int result = dbContext.ExecuteScaler(strSQL, "DbContext");
                return result;
            }

            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }
        #endregion

        public List<EmployeePlannerModel> GetEmployeePlannerByEmployeeId(int intYear, int intMonth, int intEmpId)
        {
            try
            {
                int days = DateTime.DaysInMonth(intYear, intMonth);
                List<EmployeePlannerModel> lstemployeePlanner = new List<EmployeePlannerModel>();
                for (int intDaysCounter = 1; intDaysCounter <= days; intDaysCounter++)
                {
                    var DateOfMonth = new DateTime(intYear, intMonth, intDaysCounter);
                    string dayOnDateOfMonth = DateOfMonth.DayOfWeek.ToString();
                    EmployeePlannerModel employeePlanner = new EmployeePlannerModel();
                    employeePlanner.strDay = dayOnDateOfMonth;
                    employeePlanner.ddtDate = DateOfMonth;
                    employeePlanner.strDate = DateOfMonth.ToString("yyyy-MM-dd");
                    employeePlanner.intRowindex = 0;
                    employeePlanner.intColumnindex = 0;
                    employeePlanner.state = false;
                    employeePlanner.lstevent = GetEmpEventByDate(intEmpId, DateOfMonth);

                    lstemployeePlanner.Add(employeePlanner);
                }
                return lstemployeePlanner;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<Empevents> GetEmpEventByDate(int intEmpd, DateTime ddtEventDate)
        {
            DbContext dbContext = new DbContext();
            List<Empevents> lstEvents = new List<Empevents>();
            try
            {

                string strQuery = $"Call DoctorPlanner_R_SP(1," + intEmpd + ",'" + ddtEventDate.ToString("yyyy-MM-dd") + "');";
                DataTable dt = dbContext.GetDataTable(strQuery, "DbContext");
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Empevents ObjEmpEvent = new Empevents();
                        ObjEmpEvent.intId = Convert.ToInt32(dt.Rows[i]["intId"]);
                        DoctorsModel objDoctorsModel = new DoctorsModel();
                        clsDoctor objclsDoctor = new clsDoctor();
                        DoctorsAvailabilityModel objDoctorsAvailabilityModel = new DoctorsAvailabilityModel();
                        objDoctorsAvailabilityModel.intDoctorId = intEmpd;
                        objDoctorsAvailabilityModel.strDate = ddtEventDate.ToString("yyyy-MM-dd");
                        objDoctorsModel = objclsDoctor.GetDoctorsAvailabilitysByDateAndDocId(objDoctorsAvailabilityModel);
                        for (int slotcount = 0; slotcount < objDoctorsModel.lstSlots.Count; slotcount++)
                        {
                            ObjEmpEvent.intPatientCount = ObjEmpEvent.intPatientCount + objDoctorsModel.lstSlots[slotcount].intPatientCount;
                            ObjEmpEvent.intMaxPatientCount = ObjEmpEvent.intMaxPatientCount + objDoctorsModel.lstSlots[slotcount].intMaxPatientCount;
                        }
                        //ObjEmpEvent.strStartTime = Convert.ToDateTime(dt.Rows[i]["varStartTime"]).ToString("HH:mm");
                        //ObjEmpEvent.strEndTime = Convert.ToDateTime(dt.Rows[i]["varEndTime"]).ToString("HH:mm");
                        //ObjEmpEvent.strEvent = Convert.ToString(dt.Rows[i]["varTitle"]);
                        //ObjEmpEvent.strComment = Convert.ToString(dt.Rows[i]["varComment"]);
                        //ObjEmpEvent.strEventStatus = Convert.ToString(dt.Rows[i]["varName"]);
                        //ObjEmpEvent.intEventStatus = Convert.ToInt32(dt.Rows[i]["intStatus"]);
                        lstEvents.Add(ObjEmpEvent);
                    }
                    return lstEvents;
                }
                else
                    return lstEvents;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #region Notification
        public int InsertUpdateNotification(NotificationModel objNotificationModel)
        {
            DbContext dbContext = new DbContext();
            try
            {
                int result = 0;
                string strQuery = "call Bookingalerts_C_SP (?,?,?,?,?,?,now(),?);";
                MySqlParameter[] param = {
                    new MySqlParameter("@intId", objNotificationModel.intId),
                    new MySqlParameter("@intType", objNotificationModel.intType),
                    new MySqlParameter("@intUserId", objNotificationModel.intUserId),
                    new MySqlParameter("@varTitle", objNotificationModel.strTitle),
                    new MySqlParameter("@varBody", objNotificationModel.strBody),
                    new MySqlParameter("@intActive", objNotificationModel.intActive),
                    new MySqlParameter("@intBookingStatus", objNotificationModel.intBookingStatus),
                };
                result = dbContext.ExecuteCommandWithParameter(strQuery, param, true, "DbContext");

                return result;
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }

        public List<NotificationModel> GetNotification(int intUserId)
        {
            DbContext dbContext = new DbContext();
            List<NotificationModel> lstNotificationModel = new List<NotificationModel>();
            try
            {
                string strQuery = $"call BookingAlerts_R_SP (0," + intUserId + ",0);";
                DataTable dt = dbContext.GetDataTable(strQuery, "DbContext");
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        NotificationModel objNotification = new NotificationModel();
                        objNotification.intId = Convert.ToInt32(dr["intId"]);
                        objNotification.intUserId = Convert.ToInt32(dr["intUserId"]);
                        objNotification.intType = Convert.ToInt32(dr["intType"]);
                        objNotification.strTitle = Convert.ToString(dr["varTitle"]);
                        objNotification.strBody = Convert.ToString(dr["varBody"]);
                        objNotification.intActive = Convert.ToInt32(dr["intActive"]);
                        objNotification.intBookingStatus = Convert.ToInt32(dr["intBookingStatus"]);
                        objNotification.strCreationDate = Convert.ToDateTime(dr["dttCreationDate"]).ToString("dd-MMM-yyyy");
                        //   objNotification.strCreatedBy = Convert.ToString(dr["varFirstName"]) + " " + Convert.ToString(dr["varMiddleName"]) + " " + Convert.ToString(dr["varLastName"]);


                        lstNotificationModel.Add(objNotification);
                    }
                    return lstNotificationModel;
                }

                else
                {
                    return lstNotificationModel;
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }
        #endregion
        public List<PatientAppointmentModel> GetUnattendedPatientAppointments(int intstatus, string ddtstartTime, string ddtEndtime, int intDoctorId)
        {
            DbContext dbContext = new DbContext();

            List<PatientAppointmentModel> lsPatientAppointmentModel = new List<PatientAppointmentModel>();
            try
            {
                string strQuery = "";
                if (ddtstartTime =="")
                ddtstartTime = DateTime.Now.ToString("yyyy-MM-dd");
                if (ddtEndtime == "")
                    ddtEndtime = DateTime.Now.ToString("yyyy-MM-dd");
                strQuery = $"call Report_UnAttended_appointments(1, '"+ ddtstartTime + "','" + ddtEndtime + "'," + intDoctorId + ");";
                DataTable dt = dbContext.GetDataTable(strQuery, "DbContext");
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        PatientAppointmentModel objPatientAppointmentsModel = new PatientAppointmentModel();

                        objPatientAppointmentsModel.intId = Convert.ToInt32(dr["intId"]);
                        objPatientAppointmentsModel.intSlotId = Convert.ToInt32(dr["intSlotId"]);
                        objPatientAppointmentsModel.intDoctorId = Convert.ToInt32(dr["intDoctorId"]);
                        objPatientAppointmentsModel.intPatientId = Convert.ToInt32(dr["intPatientId"]);
                        objPatientAppointmentsModel.strSlotDate = Convert.ToDateTime(dr["dttDate"]).ToString("yyyy-MM-dd");
                        objPatientAppointmentsModel.strFirstName = Convert.ToString(dr["varFirstName"]);
                        objPatientAppointmentsModel.strMiddleName = Convert.ToString(dr["varMiddleName"]);
                        objPatientAppointmentsModel.strLastName = Convert.ToString(dr["varLastName"]);

                        objPatientAppointmentsModel.strStartTime = Convert.ToString(dr["varStartTime"]);
                        objPatientAppointmentsModel.strEndTime = Convert.ToString(dr["varEndTime"]);

                        objPatientAppointmentsModel.strDocName = Convert.ToString(dr["varDoctorName"]);
                        objPatientAppointmentsModel.strPatientImage = Convert.ToString(dr["varImagePath"]);
                        objPatientAppointmentsModel.strDocImagePath = Convert.ToString(dr["varDocImagePath"]);
                        objPatientAppointmentsModel.strSpeciality = Convert.ToString(dr["SpecialityName"]);
                        objPatientAppointmentsModel.strMobileNo = Convert.ToString(dr["varMobileNo"]);
                        objPatientAppointmentsModel.strEmail = Convert.ToString(dr["varEmail"]);
                        lsPatientAppointmentModel.Add(objPatientAppointmentsModel);
                    }
                    return lsPatientAppointmentModel;
                }

                else
                {
                    return lsPatientAppointmentModel;
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }

        }
    }
}
