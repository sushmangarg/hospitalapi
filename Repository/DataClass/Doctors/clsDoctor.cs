using HospitalManagement.DbConnect;
using HospitalManagement.Repository.DataClass.Patient;
using HospitalManagement.Repository.Interface;
using HospitalManagement_Models;
using HospitalManagement_Models.Doctors;
using HospitalManagement_Models.Master;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalManagement.Repository.DataClass.Doctors
{
    public class clsDoctor : IDoctors
    {
        #region DoctorsLogin
        public DoctorsModel DoctorsLogin(DoctorsModel doctors)
        {
            DbContext dbContext = new DbContext();
            DoctorsModel usertoken = new DoctorsModel();

            try
            {
                string strQuery = "select * from doctors where varEmail = '" + doctors.strEmail + "' and varPassword = '" + doctors.strPassword + "';";
                DataTable dt = dbContext.GetDataTable(strQuery, "DbContext");
                if (dt.Rows.Count > 0)
                {
                    {
                        usertoken.intId = Convert.ToInt32(dt.Rows[0]["intId"]);
                        usertoken.strEmail = Convert.ToString(dt.Rows[0]["varEmail"]);
                        usertoken.strUserName = Convert.ToString(dt.Rows[0]["varUserName"]);
                        usertoken.intActive = Convert.ToInt32(dt.Rows[0]["intActive"]);
                        usertoken.intUserType = 2;
                    }

                    return usertoken;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                return null;
            }
        }
        #endregion

        #region Doctors
        public int InsertUpdateDoctors(DoctorsModel objDoctorsModel)
        {
            DbContext dbContext = new DbContext();
            int result = -1;
            try
            {
                string strPhoneEmailCheck = "call PatentEmailCheck_R_SP(3,'" + objDoctorsModel.strEmail + "','');";
                int validation = dbContext.ExecuteScaler(strPhoneEmailCheck, "DbContext");
                if (validation <= 0)
                {
                    strPhoneEmailCheck = "call PatentEmailCheck_R_SP(4,'','" + objDoctorsModel.strMobileNo + "');";
                    validation = dbContext.ExecuteScaler(strPhoneEmailCheck, "DbContext");
                    if (validation <= 0)
                    {
                         strPhoneEmailCheck = "select intId from doctors where (varEmail = '" + objDoctorsModel.strEmail + "') and intId != " + objDoctorsModel.intId + ";";
                         validation = dbContext.ExecuteScaler(strPhoneEmailCheck, "DbContext");
                        if (validation <= 0)
                        {
                            strPhoneEmailCheck = "select intId from doctors where (varMobileNo = '" + objDoctorsModel.strMobileNo + "') and intId != " + objDoctorsModel.intId + ";";
                            validation = dbContext.ExecuteScaler(strPhoneEmailCheck, "DbContext");
                            if (validation <= 0)
                            {
                                string strQuery = "call doctors_CU_SP (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?);";
                                MySqlParameter[] param = {
                                    new MySqlParameter("@intId", objDoctorsModel.intId),
                                    new MySqlParameter("@varFirstName", objDoctorsModel.strFirstName),
                                    new MySqlParameter("@varMiddleName", objDoctorsModel.strMiddleName),
                                    new MySqlParameter("@varLastName", objDoctorsModel.strLastName),
                                    new MySqlParameter("@intGender", objDoctorsModel.intGender),
                                    new MySqlParameter("@varUserName", objDoctorsModel.strUserName),
                                    new MySqlParameter("@varPassword", objDoctorsModel.strPassword),
                                    new MySqlParameter("@varMobileNo", objDoctorsModel.strMobileNo),
                                    new MySqlParameter("@varEmail", objDoctorsModel.strEmail),
                                    new MySqlParameter("@intCountryId", objDoctorsModel.intCountryId),
                                    new MySqlParameter("@intStateId", objDoctorsModel.intStateId),
                                    new MySqlParameter("@intCityId", objDoctorsModel.intCityId),
                                    new MySqlParameter("@intCreatedBy", objDoctorsModel.intCreatedBy),
                                    new MySqlParameter("@dttCreationDate", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")),
                                    new MySqlParameter("@intActive", objDoctorsModel.intActive),
                                    new MySqlParameter("@dttDOB", objDoctorsModel.dttDOB),
                                    new MySqlParameter("@varDesignation", objDoctorsModel.strDesignation),
                                    new MySqlParameter("@varPostalCode", objDoctorsModel.strPostalCode),
                                    new MySqlParameter("@varAddress", objDoctorsModel.strAddress),
                                    new MySqlParameter("@varBiography", objDoctorsModel.strBiography),
                                    new MySqlParameter("@varEducation", objDoctorsModel.strEducation),
                                    new MySqlParameter("@varImagePath", objDoctorsModel.strImagePath),
                                };
                                result = dbContext.ExecuteCommandWithParameter(strQuery, param, true, "DbContext");
                                return result;
                                //DoctorSpecialtyModel objDoctorSpecialtyModel = new DoctorSpecialtyModel();
                                //objDoctorSpecialtyModel.intDoctorId = result;
                                //objDoctorSpecialtyModel.intSpecialityId = objDoctorsModel.intSpecialityId;
                                //objDoctorSpecialtyModel.intCreatedBy = objDoctorsModel.intCreatedBy;
                                //objDoctorSpecialtyModel.intActive = objDoctorsModel.intActive;
                                //InsertUpdateDoctorSpecialty(objDoctorSpecialtyModel);
                            }
                            else
                            {
                                return -2;
                            }//phone no
                        }
                        else
                        {
                            return result;// email}

                        }
                    }
                    else
                        return -2;
                }
                else
                    return result;
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }

        public int InsertUpdateDoctorsImagePath(DoctorsModel objDoctorsModel)
        {
            DbContext dbContext = new DbContext();
            int result = -1;
            try
            {

                string strQuery = "call doctors_ImagePath_CU_SP (?,?);";
                MySqlParameter[] param = {
                new MySqlParameter("@intId", objDoctorsModel.intId),
                new MySqlParameter("@varImagePath", objDoctorsModel.strImagePath),
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
        public List<DoctorsModel> GetDoctors(DoctorsModel objDoctor)
        {
            DbContext dbContext = new DbContext();
            List<DoctorsModel> lstCity = new List<DoctorsModel>();
            try
            {
                string strQuery = $"call doctors_R_SP (1,"+ objDoctor.intSpecialityId +",'2023-09-09','"+  objDoctor.strSearch + "');";
                DataTable dt = dbContext.GetDataTable(strQuery, "DbContext");
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        DoctorsModel objDoctors = new DoctorsModel();

                        objDoctors.intId = Convert.ToInt32(dr["intId"]);
                        objDoctors.strFirstName = Convert.ToString(dr["varFirstName"] ?? "");
                        objDoctors.strMiddleName = Convert.ToString(dr["varMiddleName"] ?? "");
                        objDoctors.strLastName = Convert.ToString(dr["varLastName"] ?? "");
                        objDoctors.intGender = Convert.ToInt32(dr["intGender"] ?? 0);
                        objDoctors.strUserName = Convert.ToString(dr["varUserName"] ?? "");
                        objDoctors.strPassword = Convert.ToString(dr["varPassword"] ?? "");
                        objDoctors.strMobileNo = Convert.ToString(dr["varMobileNo"] ?? "");
                        objDoctors.strEmail = Convert.ToString(dr["varEmail"] ?? "");
                        objDoctors.intCountryId = Convert.ToInt32(dr["intCountryId"] ?? 0);
                        objDoctors.strCountryName = Convert.ToString(dr["varCountryName"] ?? "");
                        objDoctors.intStateId = Convert.ToInt32(dr["intStateId"] ?? 0);
                        objDoctors.strStateName = Convert.ToString(dr["varStateName"] ?? "");
                        objDoctors.intCityId = Convert.ToInt32(dr["intCityId"] ?? 0);
                        objDoctors.strCityName = Convert.ToString(dr["varCityName"] ?? "");
                        objDoctors.intCreatedBy = Convert.ToInt32(dr["intCreatedBy"] ?? 0);
                        objDoctors.intActive = Convert.ToInt32(dr["intActive"] ?? 0);
                        objDoctors.strCreationDate = Convert.ToDateTime(dr["dttCreationDate"]).ToString("dd-MM-yyyy");
                        objDoctors.dttDOB = Convert.ToString(dr["dttDOB"] ?? "");                        
                        objDoctors.strPostalCode = Convert.ToString(dr["varPostalCode"] ?? "");
                        objDoctors.strAddress = Convert.ToString(dr["varAddress"] ?? "");
                        objDoctors.strBiography = Convert.ToString(dr["varBiography"] ?? "");
                        objDoctors.strImagePath = Convert.ToString(dr["varImagePath"] ?? "");
                        objDoctors.strSpecialityName = Convert.ToString(dr["speciality"] ?? "");
                        objDoctors.strCreatedBy = Convert.ToString(dr["varUserFirstName"]) + " " + Convert.ToString(dr["varUserMiddleName"]) + " " + Convert.ToString(dr["varUserLastName"]);
                        lstCity.Add(objDoctors);
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

        public List<DoctorsModel> GetDoctorsWithSpeciality(DoctorsModel objDoctor)
        {
            DbContext dbContext = new DbContext();
            List<DoctorsModel> lstDoctor= new List<DoctorsModel>();
            try
            {
                string strQuery = $"call doctors_R_SP (5," + objDoctor.intSpecialityId + ",'2023-09-09','" + objDoctor.strSearch + "');";
                DataTable dt = dbContext.GetDataTable(strQuery, "DbContext");
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        DoctorsModel objDoctors = new DoctorsModel();

                        objDoctors.intId = Convert.ToInt32(dr["intDocSpecialityId"]);
                        objDoctors.intDoctorId = Convert.ToInt32(dr["intId"]);
                        objDoctors.strFirstName = Convert.ToString(dr["varFirstName"] ?? "");
                        objDoctors.strMiddleName = Convert.ToString(dr["varMiddleName"] ?? "");
                        objDoctors.strLastName = Convert.ToString(dr["varLastName"] ?? "");
                        objDoctors.intGender = Convert.ToInt32(dr["intGender"] ?? 0);
                        objDoctors.strUserName = Convert.ToString(dr["varUserName"] ?? "");
                        objDoctors.strPassword = Convert.ToString(dr["varPassword"] ?? "");
                        objDoctors.strMobileNo = Convert.ToString(dr["varMobileNo"] ?? "");
                        objDoctors.strEmail = Convert.ToString(dr["varEmail"] ?? "");
                        objDoctors.intCountryId = Convert.ToInt32(dr["intCountryId"] ?? 0);
                        objDoctors.strCountryName = Convert.ToString(dr["varCountryName"] ?? "");
                        objDoctors.intStateId = Convert.ToInt32(dr["intStateId"] ?? 0);
                        objDoctors.strStateName = Convert.ToString(dr["varStateName"] ?? "");
                        objDoctors.intCityId = Convert.ToInt32(dr["intCityId"] ?? 0);
                        objDoctors.strCityName = Convert.ToString(dr["varCityName"] ?? "");
                        objDoctors.intCreatedBy = Convert.ToInt32(dr["intCreatedBy"] ?? 0);
                        objDoctors.intActive = Convert.ToInt32(dr["intActive"] ?? 0);
                        objDoctors.strCreationDate = Convert.ToDateTime(dr["dttCreationDate"]).ToString("dd-MM-yyyy");
                        objDoctors.dttDOB = Convert.ToString(dr["dttDOB"] ?? "");
                        objDoctors.strPostalCode = Convert.ToString(dr["varPostalCode"] ?? "");
                        objDoctors.strAddress = Convert.ToString(dr["varAddress"] ?? "");
                        objDoctors.strBiography = Convert.ToString(dr["varBiography"] ?? "");
                        objDoctors.strImagePath = Convert.ToString(dr["varImagePath"] ?? "");
                        objDoctors.intSpecialityId = Convert.ToInt32(dr["intSpecialityId"] ?? 0);
                        objDoctors.strSpecialityName = Convert.ToString(dr["specialityName"] ?? "");
                        objDoctors.strCreatedBy = Convert.ToString(dr["varUserFirstName"]) + " " + Convert.ToString(dr["varUserMiddleName"]) + " " + Convert.ToString(dr["varUserLastName"]);
                        lstDoctor.Add(objDoctors);
                    }
                    return lstDoctor;
                }

                else
                {
                    return lstDoctor;
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }

        public DoctorsModel GetDoctorsById(int intId)
        {
            DbContext dbContext = new DbContext();
            try
            {
                DoctorsModel objDoctorsModel = new DoctorsModel();
                string strQuery = $"call doctors_R_SP(2," + intId + ",'2023-09-09','');";
                DataTable dt = dbContext.GetDataTable(strQuery, "DbContext");
                if (dt.Rows.Count > 0)
                {
                    objDoctorsModel.intId = Convert.ToInt32(dt.Rows[0]["intId"]);
                    objDoctorsModel.strFirstName = Convert.ToString(dt.Rows[0]["varFirstName"]);
                    objDoctorsModel.strMiddleName = Convert.ToString(dt.Rows[0]["varMiddleName"]);
                    objDoctorsModel.strLastName = Convert.ToString(dt.Rows[0]["varLastName"]);
                    objDoctorsModel.intGender = Convert.ToInt32(dt.Rows[0]["intGender"]);
                    objDoctorsModel.strUserName = Convert.ToString(dt.Rows[0]["varUserName"]);
                    objDoctorsModel.strPassword = Convert.ToString(dt.Rows[0]["varPassword"]);
                    objDoctorsModel.strMobileNo = Convert.ToString(dt.Rows[0]["varMobileNo"]);
                    objDoctorsModel.strEmail = Convert.ToString(dt.Rows[0]["varEmail"]);
                    if (Convert.ToInt32(dt.Rows[0]["cityActive"]) == 1)
                        objDoctorsModel.intCityId = Convert.ToInt32(dt.Rows[0]["intCityId"]);
                    if (Convert.ToInt32(dt.Rows[0]["stateActive"]) == 1)
                        objDoctorsModel.intStateId = Convert.ToInt32(dt.Rows[0]["intStateId"]);
                    else
                    {
                        objDoctorsModel.intCityId = 0;
                    }

                    if (Convert.ToInt32(dt.Rows[0]["countryActive"]) == 1)
                        objDoctorsModel.intCountryId = Convert.ToInt32(dt.Rows[0]["intCountryId"]);
                    else
                    {
                        objDoctorsModel.intCityId = 0;
                        objDoctorsModel.intStateId = 0;
                    }
                //    objDoctorsModel.intCountryId = Convert.ToInt32(dt.Rows[0]["intCountryId"]);
                    objDoctorsModel.strCountryName = Convert.ToString(dt.Rows[0]["varCountryName"]);
                  //  objDoctorsModel.intStateId = Convert.ToInt32(dt.Rows[0]["intStateId"]);
                    objDoctorsModel.strStateName = Convert.ToString(dt.Rows[0]["varStateName"]);
                 //   objDoctorsModel.intCityId = Convert.ToInt32(dt.Rows[0]["intCityId"]);
                    objDoctorsModel.strCityName = Convert.ToString(dt.Rows[0]["varCityName"]);
                    objDoctorsModel.intCreatedBy = Convert.ToInt32(dt.Rows[0]["intCreatedBy"]);
                    objDoctorsModel.dttCreationDate = Convert.ToDateTime(dt.Rows[0]["dttCreationDate"]);
                    objDoctorsModel.intActive = Convert.ToInt32(dt.Rows[0]["intActive"]);
                    objDoctorsModel.dttDOB = Convert.ToDateTime(dt.Rows[0]["dttDOB"]).ToString("yyyy-MM-dd");
                    objDoctorsModel.strDesignation = Convert.ToString(dt.Rows[0]["varDesignation"]);
                    objDoctorsModel.strPostalCode = Convert.ToString(dt.Rows[0]["varPostalCode"]);
                    objDoctorsModel.strAddress = Convert.ToString(dt.Rows[0]["varAddress"]);
                    objDoctorsModel.strBiography = Convert.ToString(dt.Rows[0]["varBiography"]);
                    objDoctorsModel.strImagePath = Convert.ToString(dt.Rows[0]["varImagePath"] ?? "");
                    
                }
                return objDoctorsModel;
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }
        public int DeleteDoctorsByid(DoctorsModel objDoctorsModel)
        {
            DbContext dbContext = new DbContext();
            try
            {
                string strSQL = $"call doctors_D_SP (?);";
                MySqlParameter[] param = {
                    new MySqlParameter("@intId",objDoctorsModel.intId),

                };

                return dbContext.ExecuteCommandWithParameter(strSQL, param, false, "DbContext");
            }

            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }

        public int UpdateDocotrPassword(DoctorsModel objDoctorsModel)
        {
            DbContext dbContext = new DbContext();
            int result = -1;
            try
            {

                string strQuery = "call Doctor_Password_U_SP (?,?,?);";
                MySqlParameter[] param = {
                new MySqlParameter("@intId", objDoctorsModel.intId),
                new MySqlParameter("@varPassword", objDoctorsModel.strPassword),
                new MySqlParameter("@varOldPassword", objDoctorsModel.strOldPassword),
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
        #endregion

        #region DoctorsAvailability
        public int InsertUpdateDoctorsAvailability(DoctorsAvailabilityModel objDoctorsAvailabilityModel)
        {
            DbContext dbContext = new DbContext();
            int result = -1;
            int intDoctorAvailabilityId = 0;
            try
            {
                // insert record in doctorsavailability
                string strQuery = "select intId from doctorsavailability where dttDate = '" + objDoctorsAvailabilityModel.strDate + "' and intDoctorId = " + objDoctorsAvailabilityModel.intDoctorId + ";";
                intDoctorAvailabilityId = dbContext.ExecuteScaler(strQuery, "DbContext");
                if (intDoctorAvailabilityId <= 0)
                {
                    strQuery = "call doctorsavailability_CU_SP (?,?,?,?,?,?);";
                    MySqlParameter[] param = {
                        new MySqlParameter("@intId", objDoctorsAvailabilityModel.intId),
                        new MySqlParameter("@dttDate", objDoctorsAvailabilityModel.strDate),
                        new MySqlParameter("@intStatus", objDoctorsAvailabilityModel.intStatus),
                        new MySqlParameter("@intCreatedBy", objDoctorsAvailabilityModel.intCreatedBy),
                        new MySqlParameter("@dttCreationDate", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")),
                        new MySqlParameter("@intDoctorId", objDoctorsAvailabilityModel.intDoctorId),
                    };
                    intDoctorAvailabilityId = dbContext.ExecuteCommandWithParameter(strQuery, param, true, "DbContext");
                    // insert record in doctorsavailabilityhistory
                    strQuery = "call doctorsavailabilityhistory_CU_SP (?,?,?,?,?,?);";
                    MySqlParameter[] paramdoctorsAvailibility = {
                        new MySqlParameter("@intId", objDoctorsAvailabilityModel.intId),
                        new MySqlParameter("@IntDoctorsAvailId", result),
                        new MySqlParameter("@dttDate", objDoctorsAvailabilityModel.strDate),
                        new MySqlParameter("@intStatus", objDoctorsAvailabilityModel.intStatus),
                        new MySqlParameter("@intCreatedBy", objDoctorsAvailabilityModel.intCreatedBy),
                        new MySqlParameter("@dttCreationDate", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")),
                    };
                    result = dbContext.ExecuteCommandWithParameter(strQuery, paramdoctorsAvailibility, true, "DbContext");
                }
                // insert record in doctorsavailabilityslots
                string[] values = objDoctorsAvailabilityModel.strDoctorSlots.Split(',');
                for (int i = 0; i < values.Length; i++)
                {
                    try
                    {
                        strQuery = "call doctorsavailabilityslots_CU_SP (1,?,?,?,?,?,?,?,?);";
                        MySqlParameter[] param1 = {
                         new MySqlParameter("@intId", objDoctorsAvailabilityModel.intIdSlot),
                         new MySqlParameter("@intSlotId", int.Parse(values[i].Trim())),
                         new MySqlParameter("@intStatusId", 1),
                         new MySqlParameter("@intCreatedBy", objDoctorsAvailabilityModel.intCreatedBy),
                         new MySqlParameter("@dttCreationDate", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")),
                         new MySqlParameter("@intDoctorId", objDoctorsAvailabilityModel.intDoctorId),
                         new MySqlParameter("@intActive", objDoctorsAvailabilityModel.intActiveSlot),
                         new MySqlParameter("@intDocAvailabilityId", intDoctorAvailabilityId),
                        };
                        result = dbContext.ExecuteCommandWithParameter(strQuery, param1, true, "DbContext");


                        //Insert record in doctorsavailabilityslotshistory
                        strQuery = "call doctorsavailabilityslotshistory_CU_SP (1,?,?,?,?,?,?,?);";
                        MySqlParameter[] paramdoctorsAvailibilitySlots = {
                          new MySqlParameter("@intId", objDoctorsAvailabilityModel.intId),
                          new MySqlParameter("@intMainSlotId", result),
                          new MySqlParameter("@intActive", objDoctorsAvailabilityModel.intActiveSlot),
                          new MySqlParameter("@intSlotId", objDoctorsAvailabilityModel.intSlotId),
                          new MySqlParameter("@intStatusId",1),
                          new MySqlParameter("@intCreatedBy", objDoctorsAvailabilityModel.intCreatedBy),
                          new MySqlParameter("@dttCreationDate", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")),
                        };
                        result = dbContext.ExecuteCommandWithParameter(strQuery, paramdoctorsAvailibilitySlots, true, "DbContext");

                    }
                    catch (Exception ex2)
                    {
                        ExceptionLog.SaveExceptionToText(ex2, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                        throw ex2;
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

        public int InsertMultipleDoctorsAvailabilityByDate(DoctorsAvailabilityModel objDoctorsAvailabilityModel)
        {
            DbContext dbContext = new DbContext();
            int intDoctorAvailabilityId = 0;
            int result = -1;
            try
            {
                if (objDoctorsAvailabilityModel.strDoctorId.Length > 0)
                {
                    string[] arrDoctorIds = objDoctorsAvailabilityModel.strDoctorId.Split(',');
                    for (int j = 0; j < arrDoctorIds.Length; j++)
                    {
                        // insert record in doctorsavailability
                        string strQuery = "select intId from doctorsavailability where dttDate = '" + objDoctorsAvailabilityModel.strDate + "' and intDoctorId = " + Convert.ToInt32(arrDoctorIds[j]) + ";";
                        intDoctorAvailabilityId = dbContext.ExecuteScaler(strQuery, "DbContext");
                        if (intDoctorAvailabilityId <= 0)
                        {
                            strQuery = "call doctorsavailability_CU_SP (?,?,?,?,?,?);";
                            MySqlParameter[] param = {
                            new MySqlParameter("@intId", objDoctorsAvailabilityModel.intId),
                            new MySqlParameter("@dttDate", objDoctorsAvailabilityModel.strDate),
                            new MySqlParameter("@intStatus", objDoctorsAvailabilityModel.intStatus),
                            new MySqlParameter("@intCreatedBy", objDoctorsAvailabilityModel.intCreatedBy),
                            new MySqlParameter("@dttCreationDate", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")),
                            new MySqlParameter("@intDoctorId", arrDoctorIds[j]),
                        };
                            intDoctorAvailabilityId = dbContext.ExecuteCommandWithParameter(strQuery, param, true, "DbContext");
                        }
                        // insert record in doctorsavailabilityhistory
                        strQuery = "call doctorsavailabilityhistory_CU_SP (?,?,?,?,?,?);";
                        MySqlParameter[] paramdoctorsAvailibility = {
                            new MySqlParameter("@intId", objDoctorsAvailabilityModel.intId),
                            new MySqlParameter("@IntDoctorsAvailId", result),
                            new MySqlParameter("@dttDate", objDoctorsAvailabilityModel.strDate),
                            new MySqlParameter("@intStatus", objDoctorsAvailabilityModel.intStatus),
                            new MySqlParameter("@intCreatedBy", objDoctorsAvailabilityModel.intCreatedBy),
                            new MySqlParameter("@dttCreationDate", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")),
                        };
                        result = dbContext.ExecuteCommandWithParameter(strQuery, paramdoctorsAvailibility, true, "DbContext");

                        // insert record in doctorsavailabilityslots
                        List<SlotsModel> lstSlots = new List<SlotsModel>();
                        strQuery = $"call masslots_R_SP (1,0,0);";
                        DataTable dt = dbContext.GetDataTable(strQuery, "DbContext");
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                SlotsModel objSlotsModel = new SlotsModel();
                                objSlotsModel.intId = Convert.ToInt32(dr["intId"]);
                                objSlotsModel.strStartTime = Convert.ToString(dr["varStartTime"]);
                                objSlotsModel.strEndTime = Convert.ToString(dr["varEndTime"]);
                                objSlotsModel.intActive = Convert.ToInt32(dr["intActive"]);
                                objSlotsModel.intCreatedBy = Convert.ToInt32(dr["intCreatedBy"]);
                                objSlotsModel.strCreationDate = Convert.ToDateTime(dr["dttCreationDate"]).ToString("yyyy-MM-dd");
                                lstSlots.Add(objSlotsModel);
                            }
                        }
                        //     string[] values = objDoctorsAvailabilityModel.strDoctorSlots.Split(',');
                        for (int i = 0; i < lstSlots.Count; i++)
                        {
                            try
                            {
                                strQuery = "select intId from doctorsavailabilityslots where intSlotId = " + lstSlots[i].intId + " and intActive=1 and intDoctorId = " + arrDoctorIds[j] + " and intDocAvailabilityId = " + intDoctorAvailabilityId + ";";
                                int isExists = dbContext.ExecuteScaler(strQuery, "DbContext");
                                if (isExists <= 0)
                                {
                                    strQuery = "call doctorsavailabilityslots_CU_SP (1,?,?,?,?,?,?,?,?);";
                                    MySqlParameter[] param1 = {
                                         new MySqlParameter("@intId", objDoctorsAvailabilityModel.intIdSlot),
                                         new MySqlParameter("@intSlotId", lstSlots[i].intId),
                                         new MySqlParameter("@intStatusId", 1),
                                         new MySqlParameter("@intCreatedBy", objDoctorsAvailabilityModel.intCreatedBy),
                                         new MySqlParameter("@dttCreationDate", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")),
                                         new MySqlParameter("@intDoctorId", arrDoctorIds[j]),
                                         new MySqlParameter("@intActive", objDoctorsAvailabilityModel.intActiveSlot),
                                         new MySqlParameter("@intDocAvailabilityId", intDoctorAvailabilityId),
                                        };
                                    result = dbContext.ExecuteCommandWithParameter(strQuery, param1, true, "DbContext");

                                    //Insert record in doctorsavailabilityslotshistory
                                    strQuery = "call doctorsavailabilityslotshistory_CU_SP (1,?,?,?,?,?,?,?);";
                                    MySqlParameter[] paramdoctorsAvailibilitySlots = {
                                          new MySqlParameter("@intId", objDoctorsAvailabilityModel.intId),
                                          new MySqlParameter("@intMainSlotId", result),
                                          new MySqlParameter("@intActive", objDoctorsAvailabilityModel.intActiveSlot),
                                          new MySqlParameter("@intSlotId", objDoctorsAvailabilityModel.intSlotId),
                                          new MySqlParameter("@intStatusId",1),
                                          new MySqlParameter("@intCreatedBy", objDoctorsAvailabilityModel.intCreatedBy),
                                          new MySqlParameter("@dttCreationDate", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")),
                                        };
                                    result = dbContext.ExecuteCommandWithParameter(strQuery, paramdoctorsAvailibilitySlots, true, "DbContext");
                                }
                            }
                            catch (Exception ex2)
                            {
                                ExceptionLog.SaveExceptionToText(ex2, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                                throw ex2;
                            }
                        }
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

        public int InsertUpdateDoctorsAvailability_Multiple(DoctorsAvailabilityModel objDoctorsAvailabilityModel)
        {
            DbContext dbContext = new DbContext();
            int result = -1;
            try
            {
                // insert record in doctorsavailabilityslots
                string[] values = objDoctorsAvailabilityModel.strDoctorSlots.Split(',');
                for (int i = 0; i < values.Length; i++)
                {
                    try
                    {
                        string strQuery = "call doctorsavailabilityslots_CU_SP (1,?,?,?,?,?,?,?,?);";
                        MySqlParameter[] param1 = {
                         new MySqlParameter("@intId", objDoctorsAvailabilityModel.intIdSlot),
                         new MySqlParameter("@intSlotId", int.Parse(values[i].Trim())),
                         new MySqlParameter("@intStatusId", 1),
                         new MySqlParameter("@intCreatedBy", objDoctorsAvailabilityModel.intCreatedBy),
                         new MySqlParameter("@dttCreationDate", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")),
                         new MySqlParameter("@intDoctorId", objDoctorsAvailabilityModel.intDoctorId),
                         new MySqlParameter("@intActive", objDoctorsAvailabilityModel.intActiveSlot),
                         new MySqlParameter("@intDocAvailabilityId", 1),
                        };
                        result = dbContext.ExecuteCommandWithParameter(strQuery, param1, true, "DbContext");


                        //Insert record in doctorsavailabilityslotshistory
                        strQuery = "call doctorsavailabilityslotshistory_CU_SP (1,?,?,?,?,?,?,?);";
                        MySqlParameter[] paramdoctorsAvailibilitySlots = {
                          new MySqlParameter("@intId", objDoctorsAvailabilityModel.intId),
                          new MySqlParameter("@intMainSlotId", result),
                          new MySqlParameter("@intActive", objDoctorsAvailabilityModel.intActiveSlot),
                          new MySqlParameter("@intSlotId", objDoctorsAvailabilityModel.intSlotId),
                          new MySqlParameter("@intStatusId",1),
                          new MySqlParameter("@intCreatedBy", objDoctorsAvailabilityModel.intCreatedBy),
                          new MySqlParameter("@dttCreationDate", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")),
                        };
                        result = dbContext.ExecuteCommandWithParameter(strQuery, paramdoctorsAvailibilitySlots, true, "DbContext");

                    }
                    catch (Exception ex2)
                    {
                        ExceptionLog.SaveExceptionToText(ex2, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                        throw ex2;
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
        public List<DoctorsModel> GetDoctorsAvailability(DoctorsAvailabilityModel objDoctorsAvailabilityModel)
        {
            DbContext dbContext = new DbContext();
            List<DoctorsModel> lstDoctors = new List<DoctorsModel>();
            try
            {
                string strQuery = $"call doctorsavailability_R_SP (2,'" + objDoctorsAvailabilityModel.strDate + "',0,0);";
                DataTable dt = dbContext.GetDataTable(strQuery, "DbContext");
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {

                        DoctorsModel objDoctors = new DoctorsModel();

                        objDoctors.intId = Convert.ToInt32(dr["intId"]);
                        objDoctors.strFirstName = Convert.ToString(dr["varFirstName"]);
                        objDoctors.strMiddleName = Convert.ToString(dr["varMiddleName"]);
                        objDoctors.strLastName = Convert.ToString(dr["varLastName"]);
                        objDoctors.intGender = Convert.ToInt32(dr["intGender"]);
                        objDoctors.strUserName = Convert.ToString(dr["varUserName"]);
                        objDoctors.strPassword = Convert.ToString(dr["varPassword"]);
                        objDoctors.strMobileNo = Convert.ToString(dr["varMobileNo"]);
                        objDoctors.strEmail = Convert.ToString(dr["varEmail"]);
                        objDoctors.intCountryId = Convert.ToInt32(dr["intCountryId"]);
                        objDoctors.strCountryName = Convert.ToString(dr["varCountryName"]);
                        objDoctors.intStateId = Convert.ToInt32(dr["intStateId"]);
                        objDoctors.strStateName = Convert.ToString(dr["varStateName"]);
                        objDoctors.intCityId = Convert.ToInt32(dr["intCityId"]);
                        objDoctors.strCityName = Convert.ToString(dr["varCityName"]);
                        objDoctors.intCreatedBy = Convert.ToInt32(dr["intCreatedBy"]);
                        objDoctors.intActive = Convert.ToInt32(dr["intActive"]);
                        objDoctors.strCreationDate = Convert.ToDateTime(dr["dttCreationDate"]).ToString("dd-MM-yyyy");
                        objDoctors.dttDOB = Convert.ToDateTime(dr["dttDOB"]).ToString("yyyy-MM-dd");
                        objDoctors.intSpecialityId = Convert.ToInt32(dr["intSpecialityId"]);
                        objDoctors.strSpecialityName = Convert.ToString(dr["varSpecialityName"]);
                        objDoctors.strEducation = Convert.ToString(dt.Rows[0]["varEducation"]);
                        objDoctors.strDesignation = Convert.ToString(dr["varDesignation"]);
                        objDoctors.strPostalCode = Convert.ToString(dr["varPostalCode"]);
                        objDoctors.strAddress = Convert.ToString(dr["varAddress"]);
                        objDoctors.strBiography = Convert.ToString(dr["varBiography"]);
                        objDoctors.strImagePath = Convert.ToString(dr["varImagePath"]);
                        lstDoctors.Add(objDoctors);

                    }
                    return lstDoctors;
                }

                else
                {
                    return lstDoctors;
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }



        public List<DoctorsModel> GetDoctorsAvailabilityByDateandSpecaility(DoctorsAvailabilityModel objDoctorsAvailabilityModel)
        {
            DbContext dbContext = new DbContext();
            Master.clsMaster objMaster = new Master.clsMaster();
            List<DoctorsModel> lstDoctors = new List<DoctorsModel>();
            DoctorSpecialityModel objDoctorSpecialitys = new DoctorSpecialityModel();
            DoctorEducationModel doctorEducationModel = new DoctorEducationModel();
            DoctorExperienceModel doctorExperienceModel = new DoctorExperienceModel();
            try
            {
                string strQuery = $"call doctorsavailability_R_SP (3,'" + objDoctorsAvailabilityModel.strDate + "'," + objDoctorsAvailabilityModel.intSpecialityId + ",0,0);";
                DataTable dt = dbContext.GetDataTable(strQuery, "DbContext");
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {

                        DoctorsModel objDoctors = new DoctorsModel();

                        objDoctors.intId = Convert.ToInt32(dr["intId"]);
                        objDoctors.strFirstName = Convert.ToString(dr["varFirstName"]);
                        objDoctors.strMiddleName = Convert.ToString(dr["varMiddleName"]);
                        objDoctors.strLastName = Convert.ToString(dr["varLastName"]);
                        objDoctors.intGender = Convert.ToInt32(dr["intGender"]);
                        objDoctors.strUserName = Convert.ToString(dr["varUserName"]);
                        objDoctors.strPassword = Convert.ToString(dr["varPassword"]);
                        objDoctors.strMobileNo = Convert.ToString(dr["varMobileNo"]);
                        objDoctors.strEmail = Convert.ToString(dr["varEmail"]);
                        objDoctors.intCountryId = Convert.ToInt32(dr["intCountryId"]);
                        objDoctors.strCountryName = Convert.ToString(dr["varCountryName"]);
                        objDoctors.intStateId = Convert.ToInt32(dr["intStateId"]);
                        objDoctors.strStateName = Convert.ToString(dr["varStateName"]);
                        objDoctors.intCityId = Convert.ToInt32(dr["intCityId"]);
                        objDoctors.strCityName = Convert.ToString(dr["varCityName"]);
                        objDoctors.intCreatedBy = Convert.ToInt32(dr["intCreatedBy"]);
                        objDoctors.intActive = Convert.ToInt32(dr["intActive"]);
                        objDoctors.strCreationDate = Convert.ToDateTime(dr["dttCreationDate"]).ToString("dd-MM-yyyy");
                        objDoctors.dttDOB = Convert.ToDateTime(dr["dttDOB"]).ToString("yyyy-MM-dd");
                        objDoctorSpecialitys.intDoctorId = objDoctors.intId;
                        objDoctors.lstSpeciality = objMaster.GetDoctorSpecialityByDoctorId(objDoctorSpecialitys);
                        doctorEducationModel.intDoctorId= objDoctors.intId;
                        objDoctors.lstEducation = objMaster.GetDoctorEducationByDoctorId(doctorEducationModel);
                        doctorExperienceModel.intDoctorId = objDoctors.intId;
                        objDoctors.lstExperience = objMaster.GetDoctorExperienceByDoctorId(doctorExperienceModel);
                        //objDoctors.intSpecialityId = Convert.ToInt32(dr["intSpecialityId"]);
                        //objDoctors.strSpecialityName = Convert.ToString(dr["varSpecialityName"]);
                        //objDoctors.strEducation = Convert.ToString(dt.Rows[0]["varEducation"]);
                        objDoctors.strDesignation = Convert.ToString(dr["varDesignation"]);
                        objDoctors.strPostalCode = Convert.ToString(dr["varPostalCode"]);
                        objDoctors.strAddress = Convert.ToString(dr["varAddress"]);
                        objDoctors.strBiography = Convert.ToString(dr["varBiography"]);
                        objDoctors.strImagePath = Convert.ToString(dr["varImagePath"]);
                        lstDoctors.Add(objDoctors);

                    }
                    return lstDoctors;
                }

                else
                {
                    return lstDoctors;
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }


        public List<DoctorsAppointementsModel> GetDoctorsAppointementsBySlotId(int intDoctorId, int intStatus)
        {
            DbContext dbContext = new DbContext();
            try
            {
                List<DoctorsAppointementsModel> lstDoctorsAvailabilityModel = new List<DoctorsAppointementsModel>();

                string strQuery = $"call doctorsavailabilityslots_R_SP(2," + intDoctorId + ");";
                DataTable dt = dbContext.GetDataTable(strQuery, "DbContext");
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        DoctorsAppointementsModel objDoctorsAppointementsModel = new DoctorsAppointementsModel();
                        objDoctorsAppointementsModel.intSlotId = Convert.ToInt32(dr["intSlotId"]);
                        objDoctorsAppointementsModel.strSlotTimings = Convert.ToString(dr["varStartTime"]) + "-" + Convert.ToString(dr["varEndTime"]);
                        strQuery = $"call patientappointements_R_SP(4," + intDoctorId + ",0," + intStatus + "," + Convert.ToInt32(dr["intSlotId"]) + " " + ");";
                        dt = dbContext.GetDataTable(strQuery, "DbContext");
                        if (dt.Rows.Count > 0)
                        {
                            List<PatientAppointmentModel> lstPatientAppointmentModel = new List<PatientAppointmentModel>();
                            foreach (DataRow dr_patient in dt.Rows)
                            {
                                PatientAppointmentModel objPatientAppointmentsModel = new PatientAppointmentModel();
                                objPatientAppointmentsModel.intId = Convert.ToInt32(dr_patient["intAppointementId"]);
                                objPatientAppointmentsModel.intAppointmentSlot = Convert.ToInt32(dr_patient["intAppointmentSlot"]);
                                objPatientAppointmentsModel.intDoctorId = Convert.ToInt32(dr["intDoctorId"]);
                                objPatientAppointmentsModel.intPatientId = Convert.ToInt32(dr_patient["intPatientId"]);
                                objPatientAppointmentsModel.dttCreationDate = Convert.ToDateTime(dr_patient["dttCreationDate"]);
                                objPatientAppointmentsModel.intAppointementStatus = Convert.ToInt32(dr_patient["intAppointementStatus"]);
                                objPatientAppointmentsModel.strFirstName = Convert.ToString(dr_patient["varDoctorName"]);
                                objPatientAppointmentsModel.strMiddleName = Convert.ToString(dr_patient["varMiddleName"]);
                                objPatientAppointmentsModel.strLastName = Convert.ToString(dr_patient["varLastName"]);
                                objPatientAppointmentsModel.strSpeciality = Convert.ToString(dr_patient["varSpeciality"]);
                                objPatientAppointmentsModel.strStatusName = Convert.ToString(dr_patient["varStatusName"]);
                                objPatientAppointmentsModel.strStartTime = Convert.ToString(dr_patient["varStartTime"]);
                                objPatientAppointmentsModel.strEndTime = Convert.ToString(dr_patient["varEndTime"]);
                                objPatientAppointmentsModel.strEmail = Convert.ToString(dr_patient["varEmail"]);
                                objPatientAppointmentsModel.strMobileNo = Convert.ToString(dr_patient["varMobileNo"]);
                                objPatientAppointmentsModel.intSlotId = Convert.ToInt32(dr_patient["intSlotId"]);
                                objPatientAppointmentsModel.intStatusId = Convert.ToInt32(dr_patient["intStatusId"]);

                                lstPatientAppointmentModel.Add(objPatientAppointmentsModel);
                            }
                            objDoctorsAppointementsModel.lstPatientAppointmentModel = lstPatientAppointmentModel;

                        }
                        lstDoctorsAvailabilityModel.Add(objDoctorsAppointementsModel);
                    }
                    return lstDoctorsAvailabilityModel;
                }
                else
                {
                    return lstDoctorsAvailabilityModel;
                }

            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }
        public int DeleteDoctorsAvailabilityByid(DoctorsAvailabilityModel objDoctorsAvailabilityModel)
        {
            DbContext dbContext = new DbContext();
            try
            {
                string strSQL = $"call doctorsavailability_D_SP (?);";
                MySqlParameter[] param = {
                    new MySqlParameter("@intId",objDoctorsAvailabilityModel.intId),

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

        #region DoctorsAvailabilityHistory
        public int InsertUpdateDoctorsAvailabilityHistory(DoctorsAvailabilityHistoryModel objDoctorsAvailabilityHistoryModel)
        {
            DbContext dbContext = new DbContext();
            try
            {
                string strQuery = "call doctorsavailabilityhistory_CU_SP (?,?,?,?,?,?);";
                MySqlParameter[] param = {
                new MySqlParameter("@intId", objDoctorsAvailabilityHistoryModel.intId),
                new MySqlParameter("@IntDoctorsAvailId", objDoctorsAvailabilityHistoryModel.IntDoctorsAvailId),
                new MySqlParameter("@dttDate", objDoctorsAvailabilityHistoryModel.dttDate),
                new MySqlParameter("@intStatus", objDoctorsAvailabilityHistoryModel.intStatus),
                 new MySqlParameter("@intCreatedBy", objDoctorsAvailabilityHistoryModel.intCreatedBy),
                new MySqlParameter("@dttCreationDate", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")),


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



        public List<DoctorsAvailabilityHistoryModel> GetDoctorsAvailabilityHistory()
        {
            DbContext dbContext = new DbContext();
            List<DoctorsAvailabilityHistoryModel> lstDoctorsAvailabilityHistory = new List<DoctorsAvailabilityHistoryModel>();
            try
            {
                string strQuery = $"call doctorsavailabilityhistory_R_SP (1,0);";
                DataTable dt = dbContext.GetDataTable(strQuery, "DbContext");
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        DoctorsAvailabilityHistoryModel objDoctorsAvailabilityHistoryModel = new DoctorsAvailabilityHistoryModel();

                        objDoctorsAvailabilityHistoryModel.intId = Convert.ToInt32(dr["intId"]);
                        objDoctorsAvailabilityHistoryModel.IntDoctorsAvailId = Convert.ToInt32(dr["IntDoctorsAvailId"]);
                        objDoctorsAvailabilityHistoryModel.dttDate = Convert.ToString(dr["dttDate"]);
                        objDoctorsAvailabilityHistoryModel.intStatus = Convert.ToInt32(dr["intStatus"]);
                        objDoctorsAvailabilityHistoryModel.intCreatedBy = Convert.ToInt32(dr["intCreatedBy"]);
                        objDoctorsAvailabilityHistoryModel.dttCreationDate = Convert.ToDateTime(dr["dttCreationDate"]);

                        lstDoctorsAvailabilityHistory.Add(objDoctorsAvailabilityHistoryModel);
                    }
                    return lstDoctorsAvailabilityHistory;
                }

                else
                {
                    return lstDoctorsAvailabilityHistory;
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }

        public DoctorsAvailabilityHistoryModel GetDoctorsAvailabilityHistoryById(int intId)
        {
            DbContext dbContext = new DbContext();
            try
            {
                DoctorsAvailabilityHistoryModel objDoctorsAvailabilityHistoryModel = new DoctorsAvailabilityHistoryModel();
                string strQuery = $"call doctorsavailabilityhistory_R_SP(2," + intId + ");";
                DataTable dt = dbContext.GetDataTable(strQuery, "DbContext");
                if (dt.Rows.Count > 0)
                {
                    objDoctorsAvailabilityHistoryModel.intId = Convert.ToInt32(dt.Rows[0]["intId"]);
                    objDoctorsAvailabilityHistoryModel.IntDoctorsAvailId = Convert.ToInt32(dt.Rows[0]["IntDoctorsAvailId"]);
                    objDoctorsAvailabilityHistoryModel.dttDate = Convert.ToString(dt.Rows[0]["dttDate"]);
                    objDoctorsAvailabilityHistoryModel.intStatus = Convert.ToInt32(dt.Rows[0]["intStatus"]);
                    objDoctorsAvailabilityHistoryModel.intCreatedBy = Convert.ToInt32(dt.Rows[0]["intCreatedBy"]);
                    objDoctorsAvailabilityHistoryModel.dttCreationDate = Convert.ToDateTime(dt.Rows[0]["dttCreationDate"]);


                }
                return objDoctorsAvailabilityHistoryModel;
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }
        public int DeleteDoctorsAvailabilityHistoryByid(DoctorsAvailabilityHistoryModel objDoctorsAvailabilityHistoryModel)
        {
            DbContext dbContext = new DbContext();
            try
            {
                string strSQL = $"call doctorsavailabilityhistory_D_SP (?);";
                MySqlParameter[] param = {
                    new MySqlParameter("@intId",objDoctorsAvailabilityHistoryModel.intId),

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

        #region DoctorsAvailabilitySlots
        public int InsertUpdateDoctorsAvailabilitySlots(DoctorsAvailabilitySlotsModel objDoctorsAvailabilitySlotsModel)
        {
            DbContext dbContext = new DbContext();
            try
            {
                string strQuery = "call doctorsavailabilityslots_CU_SP (?,?,?,?,?,?,?,?);";
                MySqlParameter[] param = {
                new MySqlParameter("@intId", objDoctorsAvailabilitySlotsModel.intId),
                new MySqlParameter("@intSlotId", objDoctorsAvailabilitySlotsModel.intSlotId),
                new MySqlParameter("@intStatusId", objDoctorsAvailabilitySlotsModel.intStatusId),
                new MySqlParameter("@intCreatedBy", objDoctorsAvailabilitySlotsModel.intCreatedBy),
                new MySqlParameter("@dttCreationDate", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")),
                new MySqlParameter("@intDoctorId", objDoctorsAvailabilitySlotsModel.intDoctorId),
                new MySqlParameter("@intActive", objDoctorsAvailabilitySlotsModel.intActive),
                new MySqlParameter("@intDocAvailabilityId", 1),
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

        public List<DoctorsAvailabilitySlotsModel> GetDoctorsAvailabilitySlots()
        {
            DbContext dbContext = new DbContext();
            List<DoctorsAvailabilitySlotsModel> lstDoctorsAvailabilitySlots = new List<DoctorsAvailabilitySlotsModel>();
            try
            {
                string strQuery = $"call doctorsavailabilityslots_R_SP (1,0,'');";
                DataTable dt = dbContext.GetDataTable(strQuery, "DbContext");
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        DoctorsAvailabilitySlotsModel objDoctorsAvailabilitySlotsModel = new DoctorsAvailabilitySlotsModel();

                        objDoctorsAvailabilitySlotsModel.intId = Convert.ToInt32(dr["intId"]);
                        objDoctorsAvailabilitySlotsModel.intSlotId = Convert.ToInt32(dr["intSlotId"]);
                        objDoctorsAvailabilitySlotsModel.intStatusId = Convert.ToInt32(dr["intStatusId"]);
                        objDoctorsAvailabilitySlotsModel.intCreatedBy = Convert.ToInt32(dr["intCreatedBy"]);
                        objDoctorsAvailabilitySlotsModel.dttCreationDate = Convert.ToDateTime(dr["dttCreationDate"]);
                        objDoctorsAvailabilitySlotsModel.intDoctorId = Convert.ToInt32(dr["intDoctorId"]);
                        objDoctorsAvailabilitySlotsModel.intActive = Convert.ToInt32(dr["intActive"]);

                        lstDoctorsAvailabilitySlots.Add(objDoctorsAvailabilitySlotsModel);
                    }
                    return lstDoctorsAvailabilitySlots;
                }

                else
                {
                    return lstDoctorsAvailabilitySlots;
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }

        public DoctorsAvailabilitySlotsModel GetDoctorsAvailabilitySlotsById(int intDoctorId)
        {
            DbContext dbContext = new DbContext();
            try
            {
                DoctorsAvailabilitySlotsModel objDoctorsAvailabilitySlotsModel = new DoctorsAvailabilitySlotsModel();
                string strQuery = $"call doctorsavailabilityslots_R_SP(2," + intDoctorId + ",'');";
                DataTable dt = dbContext.GetDataTable(strQuery, "DbContext");
                if (dt.Rows.Count > 0)
                {
                    objDoctorsAvailabilitySlotsModel.intId = Convert.ToInt32(dt.Rows[0]["intId"]);
                    objDoctorsAvailabilitySlotsModel.intSlotId = Convert.ToInt32(dt.Rows[0]["intSlotId"]);
                    objDoctorsAvailabilitySlotsModel.intStatusId = Convert.ToInt32(dt.Rows[0]["intStatusId"]);
                    objDoctorsAvailabilitySlotsModel.intCreatedBy = Convert.ToInt32(dt.Rows[0]["intCreatedBy"]);
                    objDoctorsAvailabilitySlotsModel.dttCreationDate = Convert.ToDateTime(dt.Rows[0]["dttCreationDate"]);
                    objDoctorsAvailabilitySlotsModel.intDoctorId = Convert.ToInt32(dt.Rows[0]["intDoctorId"]);
                    objDoctorsAvailabilitySlotsModel.intActive = Convert.ToInt32(dt.Rows[0]["intActive"]);
                    objDoctorsAvailabilitySlotsModel.strStartTime = Convert.ToString(dt.Rows[0]["varStartTime"]);
                    objDoctorsAvailabilitySlotsModel.strEndTime = Convert.ToString(dt.Rows[0]["varEndTime"]);


                }
                return objDoctorsAvailabilitySlotsModel;
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }


        public int DeleteDoctorsAvailabilitySlotsByid(DoctorsAvailabilitySlotsModel objDoctorsAvailabilitySlotsModel)
        {
            DbContext dbContext = new DbContext();
            //DoctorsAvailabilityModel objDoctorsAvailabilityModel = new DoctorsAvailabilityModel();
            //DoctorsModel objDoocModel = new DoctorsModel();
            try
            {
                //string strquery = $"select * from doctorsavailabilityslots where intId = " + objDoctorsAvailabilitySlotsModel.intId + ";";
                //DataTable dt = dbContext.GetDataTable(strquery, "DbContext");
                //if (dt.Rows.Count > 0)
                //{
                //    objDoctorsAvailabilityModel.intDoctorId = Convert.ToInt32(dt.Rows[0]["intDoctorId"]);
                //    objDoctorsAvailabilityModel.strDate = Convert.ToString(dt.Rows[0]["varSlotDate"]);

                //}
                string strSQL = $"call doctorsavailabilityslots_D_SP (?);";

                MySqlParameter[] param = {
                    new MySqlParameter("@intId",objDoctorsAvailabilitySlotsModel.intId),

                };
                return dbContext.ExecuteCommandWithParameter(strSQL, param, false, "DbContext");
                //AllDoctorsAppointementsModel objAllDoctorsAppointementsModel = new AllDoctorsAppointementsModel();
                //objAllDoctorsAppointementsModel = GetDoctorsAvailabilitysByDateAndDocId(objDoctorsAvailabilityModel);
                //if (objAllDoctorsAppointementsModel.lstDoctors.Count > 0)
                //    objDoocModel = objAllDoctorsAppointementsModel.lstDoctors[0];
                //return objDoocModel;
            }

            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }

        #endregion

        #region DoctorSpecialty
        public int InsertUpdateDoctorSpecialty(DoctorSpecialtyModel objDoctorSpecialtyModel)
        {
            DbContext dbContext = new DbContext();
            try
            {
                string strUpadteDoctorSpe = "update doctorspecialty set intActive = 0 where intDoctorId = " + objDoctorSpecialtyModel.intDoctorId + ";";
                int validation = dbContext.ExecuteCommand(strUpadteDoctorSpe, "DbContext");
                string strQuery = "call doctorspecialty_CU_SP (?,?,?,?,?,?);";
                MySqlParameter[] param = {
                new MySqlParameter("@intId", objDoctorSpecialtyModel.intId),
                new MySqlParameter("@intDoctorId", objDoctorSpecialtyModel.intDoctorId),
                new MySqlParameter("@intSpecialityId", objDoctorSpecialtyModel.intSpecialityId),
                new MySqlParameter("@intActive", objDoctorSpecialtyModel.intActive),
                new MySqlParameter("@intCreatedBy", objDoctorSpecialtyModel.intCreatedBy),
                new MySqlParameter("@dttCreationDate", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")),

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

        public List<DoctorSpecialtyModel> GetDoctorSpecialty()
        {
            DbContext dbContext = new DbContext();
            List<DoctorSpecialtyModel> lstDoctorSpecialty = new List<DoctorSpecialtyModel>();
            try
            {
                string strQuery = $"call doctorspecialty_R_SP (1,0);";
                DataTable dt = dbContext.GetDataTable(strQuery, "DbContext");
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        DoctorSpecialtyModel objDoctorSpecialtyModel = new DoctorSpecialtyModel();

                        objDoctorSpecialtyModel.intId = Convert.ToInt32(dr["intId"]);
                        objDoctorSpecialtyModel.intDoctorId = Convert.ToInt32(dr["intDoctorId"]);
                        objDoctorSpecialtyModel.intSpecialityId = Convert.ToInt32(dr["intSpecialityId"]);
                        objDoctorSpecialtyModel.intActive = Convert.ToInt32(dr["intActive"]);
                        objDoctorSpecialtyModel.intCreatedBy = Convert.ToInt32(dr["intCreatedBy"]);
                        objDoctorSpecialtyModel.dttCreationDate = Convert.ToDateTime(dr["dttCreationDate"]);

                        lstDoctorSpecialty.Add(objDoctorSpecialtyModel);
                    }
                    return lstDoctorSpecialty;
                }

                else
                {
                    return lstDoctorSpecialty;
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }

        public List<DoctorsModel> GetDoctorSpecialtyById(int intSpecialityId)
        {
            DbContext dbContext = new DbContext();
            List<DoctorsModel> lstDoctorsModel = new List<DoctorsModel>();
            try
            {
                string strQuery = $"call doctorspecialty_R_SP(2," + intSpecialityId + ");";
                DataTable dt = dbContext.GetDataTable(strQuery, "DbContext");
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        DoctorsModel objDoctors = new DoctorsModel();

                        objDoctors.intId = Convert.ToInt32(dr["intId"]);
                        objDoctors.strFirstName = Convert.ToString(dr["varFirstName"] ?? "");
                        objDoctors.strMiddleName = Convert.ToString(dr["varMiddleName"] ?? "");
                        objDoctors.strLastName = Convert.ToString(dr["varLastName"] ?? "");
                        objDoctors.intGender = Convert.ToInt32(dr["intGender"] ?? 0);
                        objDoctors.strUserName = Convert.ToString(dr["varUserName"] ?? "");
                        objDoctors.strPassword = Convert.ToString(dr["varPassword"] ?? "");
                        objDoctors.strMobileNo = Convert.ToString(dr["varMobileNo"] ?? "");
                        objDoctors.strEmail = Convert.ToString(dr["varEmail"] ?? "");
                        objDoctors.intCountryId = Convert.ToInt32(dr["intCountryId"] ?? 0);
                        objDoctors.strCountryName = Convert.ToString(dr["varCountryName"] ?? "");
                        objDoctors.intStateId = Convert.ToInt32(dr["intStateId"] ?? 0);
                        objDoctors.strStateName = Convert.ToString(dr["varStateName"] ?? "");
                        objDoctors.intCityId = Convert.ToInt32(dr["intCityId"] ?? 0);
                        objDoctors.strCityName = Convert.ToString(dr["varCityName"] ?? "");
                        objDoctors.intCreatedBy = Convert.ToInt32(dr["intCreatedBy"] ?? 0);
                        objDoctors.intActive = Convert.ToInt32(dr["intActive"] ?? 0);
                        objDoctors.strCreationDate = Convert.ToDateTime(dr["dttCreationDate"]).ToString("dd-MM-yyyy");
                        objDoctors.dttDOB = Convert.ToString(dr["dttDOB"] ?? "");
                        objDoctors.intSpecialityId = Convert.ToInt32(dr["intSpecialityId"] ?? 0);
                        objDoctors.strSpecialityName = Convert.ToString(dr["varSpecialityName"] ?? "");
                        objDoctors.strEducation = Convert.ToString(dt.Rows[0]["varEducation"] ?? "");
                        objDoctors.strDesignation = Convert.ToString(dr["varDesignation"] ?? "");
                        objDoctors.strPostalCode = Convert.ToString(dr["varPostalCode"] ?? "");
                        objDoctors.strAddress = Convert.ToString(dr["varAddress"] ?? "");
                        objDoctors.strBiography = Convert.ToString(dr["varBiography"] ?? "");

                        lstDoctorsModel.Add(objDoctors);
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

        public int DeleteDoctorSpecialtyByid(DoctorSpecialtyModel objDoctorSpecialtyModel)
        {
            DbContext dbContext = new DbContext();
            try
            {
                string strSQL = $"call doctorspecialty_D_SP (?);";
                MySqlParameter[] param = {
                    new MySqlParameter("@intId",objDoctorSpecialtyModel.intId),

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

        #region DoctorsAvailabilitySlotsHistory
        public List<DoctorsAvailabilitySlotsHistoryModel> GetDoctorsAvailabilitySlotsHistory()
        {
            DbContext dbContext = new DbContext();
            List<DoctorsAvailabilitySlotsHistoryModel> lstDoctorsAvailabilitySlotsHistory = new List<DoctorsAvailabilitySlotsHistoryModel>();
            try
            {
                string strQuery = $"call doctorsavailabilityslotshistory_R_SP (1,0);";
                DataTable dt = dbContext.GetDataTable(strQuery, "DbContext");
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        DoctorsAvailabilitySlotsHistoryModel objDoctorsAvailabilitySlotsHistoryModel = new DoctorsAvailabilitySlotsHistoryModel();

                        objDoctorsAvailabilitySlotsHistoryModel.intId = Convert.ToInt32(dr["intId"]);
                        objDoctorsAvailabilitySlotsHistoryModel.intMainSlotId = Convert.ToInt32(dr["intMainSlotId"]);
                        objDoctorsAvailabilitySlotsHistoryModel.intActive = Convert.ToInt32(dr["intActive"]);
                        objDoctorsAvailabilitySlotsHistoryModel.intSlotId = Convert.ToInt32(dr["intSlotId"]);
                        objDoctorsAvailabilitySlotsHistoryModel.intStatusId = Convert.ToInt32(dr["intStatusId"]);
                        objDoctorsAvailabilitySlotsHistoryModel.intCreatedBy = Convert.ToInt32(dr["intCreatedBy"]);
                        objDoctorsAvailabilitySlotsHistoryModel.dttCreationDate = Convert.ToDateTime(dr["dttCreationDate"]);

                        lstDoctorsAvailabilitySlotsHistory.Add(objDoctorsAvailabilitySlotsHistoryModel);
                    }
                    return lstDoctorsAvailabilitySlotsHistory;
                }

                else
                {
                    return lstDoctorsAvailabilitySlotsHistory;
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }
        #endregion

        #region AppointementsReport

        public List<DoctorsAppointementsModel> GetDoctorsAppointementsByDate(int intDoctorId, int intTypeId)
        {
            DbContext dbContext = new DbContext();
            try
            {
                List<DoctorsAppointementsModel> lstDoctorsAvailabilityModel = new List<DoctorsAppointementsModel>();

                string strQuery = $"call doctorsavailabilityslots_R_SP(2," + intDoctorId + ");";
                DataTable dt = dbContext.GetDataTable(strQuery, "DbContext");
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        DoctorsAppointementsModel objDoctorsAppointementsModel = new DoctorsAppointementsModel();
                        objDoctorsAppointementsModel.intSlotId = Convert.ToInt32(dr["intSlotId"]);
                        objDoctorsAppointementsModel.strSlotTimings = Convert.ToString(dr["varStartTime"]) + "-" + Convert.ToString(dr["varEndTime"]);
                        strQuery = $"call patientappointements_R_SP(" + intTypeId + "," + intDoctorId + ",0,0," + objDoctorsAppointementsModel.intSlotId + " " + ");";
                        dt = dbContext.GetDataTable(strQuery, "DbContext");
                        if (dt.Rows.Count > 0)
                        {
                            List<PatientAppointmentModel> lstPatientAppointmentModel = new List<PatientAppointmentModel>();
                            foreach (DataRow dr_patient in dt.Rows)
                            {
                                PatientAppointmentModel objPatientAppointmentsModel = new PatientAppointmentModel();
                                objPatientAppointmentsModel.intId = Convert.ToInt32(dr_patient["intId"]);
                                objPatientAppointmentsModel.intAppointmentSlot = Convert.ToInt32(dr_patient["intAppointmentSlot"]);
                                objPatientAppointmentsModel.intDoctorId = Convert.ToInt32(dr["intDoctorId"]);
                                objPatientAppointmentsModel.intPatientId = Convert.ToInt32(dr_patient["intPatientId"]);
                                objPatientAppointmentsModel.dttCreationDate = Convert.ToDateTime(dr_patient["dttCreationDate"]);
                                objPatientAppointmentsModel.intAppointementStatus = Convert.ToInt32(dr_patient["intAppointementStatus"]);
                                objPatientAppointmentsModel.strStatusName = Convert.ToString(dr_patient["varStatusName"]);
                                objPatientAppointmentsModel.strStartTime = Convert.ToString(dr_patient["varStartTime"]);
                                objPatientAppointmentsModel.strEndTime = Convert.ToString(dr_patient["varEndTime"]);
                                objPatientAppointmentsModel.intSlotId = objDoctorsAppointementsModel.intSlotId;
                                lstPatientAppointmentModel.Add(objPatientAppointmentsModel);
                            }
                            objDoctorsAppointementsModel.lstPatientAppointmentModel = lstPatientAppointmentModel;

                        }
                        lstDoctorsAvailabilityModel.Add(objDoctorsAppointementsModel);
                    }
                    return lstDoctorsAvailabilityModel;
                }
                else
                {
                    return lstDoctorsAvailabilityModel;
                }

            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }

        public List<DoctorsAvailabilitySlotsModel> GetDoctorsAvailabilitySlotsByDoctorId(int intDoctorId)
        {
            DbContext dbContext = new DbContext();
            List<DoctorsAvailabilitySlotsModel> lstDoctorsAvailabilitySlots = new List<DoctorsAvailabilitySlotsModel>();
            try
            {
                string strQuery = $"call doctorsavailabilityslots_R_SP(2," + intDoctorId + ",'');";
                DataTable dt = dbContext.GetDataTable(strQuery, "DbContext");
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        DoctorsAvailabilitySlotsModel objDoctorsAvailabilitySlotsModel = new DoctorsAvailabilitySlotsModel();

                        objDoctorsAvailabilitySlotsModel.intId = Convert.ToInt32(dr["intId"]);
                        objDoctorsAvailabilitySlotsModel.intSlotId = Convert.ToInt32(dr["intSlotId"]);
                        objDoctorsAvailabilitySlotsModel.intStatusId = Convert.ToInt32(dr["intStatusId"]);
                        objDoctorsAvailabilitySlotsModel.intCreatedBy = Convert.ToInt32(dr["intCreatedBy"]);
                        objDoctorsAvailabilitySlotsModel.dttCreationDate = Convert.ToDateTime(dr["dttCreationDate"]);
                        objDoctorsAvailabilitySlotsModel.intDoctorId = Convert.ToInt32(dr["intDoctorId"]);
                        objDoctorsAvailabilitySlotsModel.intActive = Convert.ToInt32(dr["intActive"]);
                        objDoctorsAvailabilitySlotsModel.strStartTime = Convert.ToString(dr["varStartTime"]);
                        objDoctorsAvailabilitySlotsModel.strEndTime = Convert.ToString(dr["varEndTime"]);

                        lstDoctorsAvailabilitySlots.Add(objDoctorsAvailabilitySlotsModel);
                    }
                    return lstDoctorsAvailabilitySlots;
                }

                else
                {
                    return lstDoctorsAvailabilitySlots;
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }

        public List<DoctorsAvailabilitySlotsModel> GetDoctorsAvailabilitySlotsByIdAndDate(int intDoctorId, string strDate)
        {
            DbContext dbContext = new DbContext();
            List<DoctorsAvailabilitySlotsModel> lstDoctorsAvailabilitySlots = new List<DoctorsAvailabilitySlotsModel>();
            try
            {
                string strQuery = $"call doctorsavailabilityslots_R_SP(3," + intDoctorId + ",'" + strDate + "');";
                DataTable dt = dbContext.GetDataTable(strQuery, "DbContext");
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        DoctorsAvailabilitySlotsModel objDoctorsAvailabilitySlotsModel = new DoctorsAvailabilitySlotsModel();

                        objDoctorsAvailabilitySlotsModel.intId = Convert.ToInt32(dr["intId"]);
                        objDoctorsAvailabilitySlotsModel.intSlotId = Convert.ToInt32(dr["intSlotId"]);
                        objDoctorsAvailabilitySlotsModel.intStatusId = Convert.ToInt32(dr["intStatusId"]);
                        objDoctorsAvailabilitySlotsModel.intCreatedBy = Convert.ToInt32(dr["intCreatedBy"]);
                        objDoctorsAvailabilitySlotsModel.dttCreationDate = Convert.ToDateTime(dr["dttCreationDate"]);
                        objDoctorsAvailabilitySlotsModel.intDoctorId = Convert.ToInt32(dr["intDoctorId"]);
                        objDoctorsAvailabilitySlotsModel.intActive = Convert.ToInt32(dr["intActive"]);
                        objDoctorsAvailabilitySlotsModel.strStartTime = Convert.ToString(dr["varStartTime"]);
                        objDoctorsAvailabilitySlotsModel.strEndTime = Convert.ToString(dr["varEndTime"]);

                        lstDoctorsAvailabilitySlots.Add(objDoctorsAvailabilitySlotsModel);
                    }
                    return lstDoctorsAvailabilitySlots;
                }

                else
                {
                    return lstDoctorsAvailabilitySlots;
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }

        public AllDoctorsAppointementsModel GetAllDoctorsAvailabilitysByDate_(DoctorsAvailabilityModel objDoctorsAvailabilityModel)
        {
            DbContext dbContext = new DbContext();
            List<SlotsModel> lstSlots = new List<SlotsModel>();
            List<DoctorsModel> lstDoctorsModel = new List<DoctorsModel>();
            AllDoctorsAppointementsModel lstAllDoctorsAppointementsModel = new AllDoctorsAppointementsModel();
            DateTime currentTime = DateTime.Now;
            DateTime SlotStartTime;
            DateTime SlotEndTime;
            string strSlotStartTime;
            string strSlotEndTime;
            string strSlotDate;
            DateTime ddtSlotDate;
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
                        
                        strSlotStartTime = Convert.ToDateTime(objSlotsModel.strStartTime).ToString("yyyy-MM-dd HH:mm");
                        strSlotEndTime = Convert.ToDateTime(objSlotsModel.strEndTime).ToString("yyyy-MM-dd HH:mm");
                        SlotStartTime = DateTime.Parse(strSlotStartTime);
                        SlotEndTime = DateTime.Parse(strSlotEndTime);
                        //SlotStartTime = Convert.ToDateTime(strSlotStartTime);
                        //SlotEndTime = Convert.ToDateTime(strSlotEndTime);
                        strSlotDate = Convert.ToDateTime(objDoctorsAvailabilityModel.strDate).ToString("yyyy-MM-dd HH:mm");
                        // ddtSlotDate = DateTime.Parse(strSlotDate);
                        ddtSlotDate = Convert.ToDateTime(strSlotDate);
                        if (currentTime.Date < ddtSlotDate.Date)
                        {
                            objSlotsModel.intPresentPastFutureStatus = 3;
                        }
                        else if (currentTime.Date == ddtSlotDate.Date)
                        {
                            if (TimeSpan.Compare(currentTime.TimeOfDay, SlotStartTime.TimeOfDay) == -1)
                            {
                                objSlotsModel.intPresentPastFutureStatus = 3;
                            }
                            else if (TimeSpan.Compare(currentTime.TimeOfDay, SlotEndTime.TimeOfDay) == 1)
                            {
                                objSlotsModel.intPresentPastFutureStatus = 1;
                            }
                            else
                                objSlotsModel.intPresentPastFutureStatus = 2;
                        }
                        else
                            objSlotsModel.intPresentPastFutureStatus = 1;
                        objSlotsModel.intActive = Convert.ToInt32(dr["intActive"]);
                        objSlotsModel.intCreatedBy = Convert.ToInt32(dr["intCreatedBy"]);
                        objSlotsModel.strCreationDate = Convert.ToDateTime(dr["dttCreationDate"]).ToString("yyyy-MM-dd");
                        lstSlots.Add(objSlotsModel);
                    }
                    strQuery = $"call doctors_R_SP (3," + objDoctorsAvailabilityModel.intSpecialityId + ",'" + objDoctorsAvailabilityModel.strDate + "','" + objDoctorsAvailabilityModel.strDoctorName + "');";
                    dt = dbContext.GetDataTable(strQuery, "DbContext");

                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            DoctorsModel objDoctors = new DoctorsModel();

                            objDoctors.intDoctorId = Convert.ToInt32(dr["intId"]);
                           objDoctors.intId = Convert.ToInt32(dr["intDocSpecialityId"]);
                            objDoctors.strFirstName = Convert.ToString(dr["varFirstName"] ?? "");
                            objDoctors.strMiddleName = Convert.ToString(dr["varMiddleName"] ?? "");
                            objDoctors.strLastName = Convert.ToString(dr["varLastName"] ?? "");
                            objDoctors.intGender = Convert.ToInt32(dr["intGender"] ?? 0);
                            objDoctors.strMobileNo = Convert.ToString(dr["varMobileNo"]);
                            objDoctors.strEmail = Convert.ToString(dr["varEmail"]);
                            objDoctors.intCountryId = Convert.ToInt32(dr["intCountryId"]);
                            objDoctors.strCountryName = Convert.ToString(dr["varCountryName"]);
                            objDoctors.intStateId = Convert.ToInt32(dr["intStateId"]);
                            objDoctors.strStateName = Convert.ToString(dr["varStateName"]);
                            objDoctors.intCityId = Convert.ToInt32(dr["intCityId"]);
                            objDoctors.strCityName = Convert.ToString(dr["varCityName"]);
                            objDoctors.intCreatedBy = Convert.ToInt32(dr["intCreatedBy"]);
                            objDoctors.intActive = Convert.ToInt32(dr["intActive"]);
                            objDoctors.strCreationDate = Convert.ToDateTime(dr["dttCreationDate"]).ToString("dd-MM-yyyy");
                            objDoctors.dttDOB = Convert.ToDateTime(dr["dttDOB"]).ToString("yyyy-MM-dd");
                            //objDoctors.intSpecialityId = Convert.ToInt32(dr["intSpecialityId"]);
                            //objDoctors.strSpecialityName = Convert.ToString(dr["varSpecialityName"]);
                            //objDoctors.strEducation = Convert.ToString(dt.Rows[0]["varEducation"]);
                            objDoctors.strDesignation = Convert.ToString(dr["varDesignation"]);
                            objDoctors.strPostalCode = Convert.ToString(dr["varPostalCode"]);
                            objDoctors.strAddress = Convert.ToString(dr["varAddress"]);
                            objDoctors.strBiography = Convert.ToString(dr["varBiography"]);
                            objDoctors.strImagePath = Convert.ToString(dr["varImagePath"] ?? "");
                            List<SlotsModel> lstdoctorsSlots = new List<SlotsModel>();
                            for (int intslotcount = 0; intslotcount < lstSlots.Count; intslotcount++)
                            {
                                //    strQuery = $"call doctorsavailability_R_SP (5,'" + objDoctorsAvailabilityModel.strDate + "',0," + objDoctors.intId + "," + lstSlots[intslotcount].intId + ");";
                                strQuery = $"call doctorsavailability_R_SP (6,'" + objDoctorsAvailabilityModel.strDate + "',0," + objDoctors.intDoctorId + "," + lstSlots[intslotcount].intId + ");";
                                DataTable dtdocslots = dbContext.GetDataTable(strQuery, "DbContext");
                                if (dtdocslots.Rows.Count > 0)
                                {
                                    if (Convert.ToInt32(dtdocslots.Rows[0]["intDoctorId"]) == objDoctors.intId)
                                    {
                                        SlotsModel objSlotsModel = new SlotsModel();
                                        objSlotsModel.intId = Convert.ToInt32(dtdocslots.Rows[0]["intId"]);
                                        objSlotsModel.intStatusId = 1;
                                        objSlotsModel.strStartTime = lstSlots[intslotcount].strStartTime;
                                        objSlotsModel.strEndTime = lstSlots[intslotcount].strEndTime;
                                        objSlotsModel.intPresentPastFutureStatus = lstSlots[intslotcount].intPresentPastFutureStatus;
                                        objSlotsModel.intMaxPatientCount = Convert.ToInt32(dtdocslots.Rows[0]["intMaxSlots"]);
                                        objSlotsModel.intPatientCount = GetPatientCountByDocIdAndSlotId(Convert.ToInt32(dtdocslots.Rows[0]["intId"]));
                                        objSlotsModel.decBookPercentage = Convert.ToDecimal((objSlotsModel.intPatientCount * 100) / objSlotsModel.intMaxPatientCount);
                                        
                                        objSlotsModel.strSpeciality = Convert.ToString(dtdocslots.Rows[0]["specialtyName"]);
                                        lstdoctorsSlots.Add(objSlotsModel);
                                    }
                                    else
                                    {
                                        SlotsModel objSlotsModel = new SlotsModel();
                                        objSlotsModel.intId = Convert.ToInt32(dtdocslots.Rows[0]["intId"]);
                                        objSlotsModel.intStatusId = 3;
                                        objSlotsModel.strStartTime = lstSlots[intslotcount].strStartTime;
                                        objSlotsModel.strEndTime = lstSlots[intslotcount].strEndTime;
                                        objSlotsModel.intPresentPastFutureStatus = lstSlots[intslotcount].intPresentPastFutureStatus;
                                        objSlotsModel.intMaxPatientCount = Convert.ToInt32(dtdocslots.Rows[0]["intMaxSlots"]);
                                        objSlotsModel.intPatientCount = GetPatientCountByDocIdAndSlotId(Convert.ToInt32(dtdocslots.Rows[0]["intId"]));
                                        objSlotsModel.decBookPercentage = Convert.ToDecimal((objSlotsModel.intPatientCount * 100) / objSlotsModel.intMaxPatientCount);
                                        objSlotsModel.strSpeciality = Convert.ToString(dtdocslots.Rows[0]["specialtyName"]);
                                        lstdoctorsSlots.Add(objSlotsModel);
                                    }
                                }
                                else
                                {
                                    SlotsModel objSlotsModel = new SlotsModel();
                                    objSlotsModel.intId = lstSlots[intslotcount].intId;
                                    objSlotsModel.intStatusId = 2;
                                    objSlotsModel.strStartTime = lstSlots[intslotcount].strStartTime;
                                    objSlotsModel.strEndTime = lstSlots[intslotcount].strEndTime;
                                    objSlotsModel.intPresentPastFutureStatus = lstSlots[intslotcount].intPresentPastFutureStatus;
                                    objSlotsModel.intMaxPatientCount = 0;
                                    objSlotsModel.intPatientCount = 0;
                                    objSlotsModel.decBookPercentage = 0 ;
                                    lstdoctorsSlots.Add(objSlotsModel);
                                }
                            }

                            objDoctors.lstSlots = lstdoctorsSlots;
                            lstDoctorsModel.Add(objDoctors);
                        }
                    }
                    lstAllDoctorsAppointementsModel.lstSlots = lstSlots;
                    lstAllDoctorsAppointementsModel.lstDoctors = lstDoctorsModel;

                    return lstAllDoctorsAppointementsModel;
                }

                else
                {
                    return lstAllDoctorsAppointementsModel;
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }

        public AllDoctorsAppointementsModel GetAllDoctorsAvailabilitysByDate(DoctorsAvailabilityModel objDoctorsAvailabilityModel)
        {
            DbContext dbContext = new DbContext();
            List<SlotsModel> lstSlots = new List<SlotsModel>();
            List<DoctorsModel> lstDoctorsModel = new List<DoctorsModel>();
            AllDoctorsAppointementsModel lstAllDoctorsAppointementsModel = new AllDoctorsAppointementsModel();
            DateTime currentTime = DateTime.Now;
            DateTime SlotStartTime;
            DateTime SlotEndTime;
            string strSlotStartTime;
            string strSlotEndTime;
            string strSlotDate;
            DateTime ddtSlotDate;
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

                        strSlotStartTime = Convert.ToDateTime(objSlotsModel.strStartTime).ToString("yyyy-MM-dd HH:mm");
                        strSlotEndTime = Convert.ToDateTime(objSlotsModel.strEndTime).ToString("yyyy-MM-dd HH:mm");
                        SlotStartTime = DateTime.Parse(strSlotStartTime);
                        SlotEndTime = DateTime.Parse(strSlotEndTime);
                        //SlotStartTime = Convert.ToDateTime(strSlotStartTime);
                        //SlotEndTime = Convert.ToDateTime(strSlotEndTime);
                        strSlotDate = Convert.ToDateTime(objDoctorsAvailabilityModel.strDate).ToString("yyyy-MM-dd HH:mm");
                        // ddtSlotDate = DateTime.Parse(strSlotDate);
                        ddtSlotDate = Convert.ToDateTime(strSlotDate);
                        if (currentTime.Date < ddtSlotDate.Date)
                        {
                            objSlotsModel.intPresentPastFutureStatus = 3;
                        }
                        else if (currentTime.Date == ddtSlotDate.Date)
                        {
                            if (TimeSpan.Compare(currentTime.TimeOfDay, SlotStartTime.TimeOfDay) == -1)
                            {
                                objSlotsModel.intPresentPastFutureStatus = 3;
                            }
                            else if (TimeSpan.Compare(currentTime.TimeOfDay, SlotEndTime.TimeOfDay) == 1)
                            {
                                objSlotsModel.intPresentPastFutureStatus = 1;
                            }
                            else
                                objSlotsModel.intPresentPastFutureStatus = 2;
                        }
                        else
                            objSlotsModel.intPresentPastFutureStatus = 1;
                        objSlotsModel.intActive = Convert.ToInt32(dr["intActive"]);
                        objSlotsModel.intCreatedBy = Convert.ToInt32(dr["intCreatedBy"]);
                        objSlotsModel.strCreationDate = Convert.ToDateTime(dr["dttCreationDate"]).ToString("yyyy-MM-dd");
                        lstSlots.Add(objSlotsModel);
                    }
                    strQuery = $"call doctors_R_SP (3," + objDoctorsAvailabilityModel.intSpecialityId + ",'" + objDoctorsAvailabilityModel.strDate + "','" + objDoctorsAvailabilityModel.strDoctorName + "');";
                    dt = dbContext.GetDataTable(strQuery, "DbContext");

                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            DoctorsModel objDoctors = new DoctorsModel();

                            objDoctors.intDoctorId = Convert.ToInt32(dr["intId"]);
                            objDoctors.intId = Convert.ToInt32(dr["intDocSpecialityId"]);
                            objDoctors.strFirstName = Convert.ToString(dr["varFirstName"] ?? "");
                            objDoctors.strMiddleName = Convert.ToString(dr["varMiddleName"] ?? "");
                            objDoctors.strLastName = Convert.ToString(dr["varLastName"] ?? "");
                            objDoctors.intGender = Convert.ToInt32(dr["intGender"] ?? 0);
                            objDoctors.strMobileNo = Convert.ToString(dr["varMobileNo"]);
                            objDoctors.strEmail = Convert.ToString(dr["varEmail"]);
                            objDoctors.intCountryId = Convert.ToInt32(dr["intCountryId"]);
                            objDoctors.strCountryName = Convert.ToString(dr["varCountryName"]);
                            objDoctors.intStateId = Convert.ToInt32(dr["intStateId"]);
                            objDoctors.strStateName = Convert.ToString(dr["varStateName"]);
                            objDoctors.intCityId = Convert.ToInt32(dr["intCityId"]);
                            objDoctors.strCityName = Convert.ToString(dr["varCityName"]);
                            objDoctors.intCreatedBy = Convert.ToInt32(dr["intCreatedBy"]);
                            objDoctors.intActive = Convert.ToInt32(dr["intActive"]);
                            objDoctors.strCreationDate = Convert.ToDateTime(dr["dttCreationDate"]).ToString("dd-MM-yyyy");
                            objDoctors.dttDOB = Convert.ToDateTime(dr["dttDOB"]).ToString("yyyy-MM-dd");
                            //objDoctors.intSpecialityId = Convert.ToInt32(dr["intSpecialityId"]);
                            //objDoctors.strSpecialityName = Convert.ToString(dr["varSpecialityName"]);
                            //objDoctors.strEducation = Convert.ToString(dt.Rows[0]["varEducation"]);
                            objDoctors.strDesignation = Convert.ToString(dr["varDesignation"]);
                            objDoctors.strPostalCode = Convert.ToString(dr["varPostalCode"]);
                            objDoctors.strAddress = Convert.ToString(dr["varAddress"]);
                            objDoctors.strBiography = Convert.ToString(dr["varBiography"]);
                            objDoctors.strImagePath = Convert.ToString(dr["varImagePath"] ?? "");
                            List<SlotsModel> lstdoctorsSlots = new List<SlotsModel>();
                            for (int intslotcount = 0; intslotcount < lstSlots.Count; intslotcount++)
                            {
                                //    strQuery = $"call doctorsavailability_R_SP (5,'" + objDoctorsAvailabilityModel.strDate + "',0," + objDoctors.intId + "," + lstSlots[intslotcount].intId + ");";
                                strQuery = $"call doctorsavailability_R_SP (6,'" + objDoctorsAvailabilityModel.strDate + "',0," + objDoctors.intDoctorId + "," + lstSlots[intslotcount].intId + ");";
                                DataTable dtdocslots = dbContext.GetDataTable(strQuery, "DbContext");
                                if (dtdocslots.Rows.Count > 0)
                                {
                                    if (Convert.ToInt32(dtdocslots.Rows[0]["intDoctorId"]) == objDoctors.intId)
                                    {
                                        SlotsModel objSlotsModel = new SlotsModel();
                                        objSlotsModel.intId = Convert.ToInt32(dtdocslots.Rows[0]["intId"]);
                                        objSlotsModel.intStatusId = 1;
                                        objSlotsModel.strStartTime = lstSlots[intslotcount].strStartTime;
                                        objSlotsModel.strEndTime = lstSlots[intslotcount].strEndTime;
                                        objSlotsModel.intPresentPastFutureStatus = lstSlots[intslotcount].intPresentPastFutureStatus;
                                        objSlotsModel.intMaxPatientCount = Convert.ToInt32(dtdocslots.Rows[0]["intMaxSlots"]);
                                        objSlotsModel.intPatientCount = GetPatientCountByDocIdAndSlotId(Convert.ToInt32(dtdocslots.Rows[0]["intId"]));
                                        objSlotsModel.decBookPercentage = Convert.ToDecimal((objSlotsModel.intPatientCount * 100) / objSlotsModel.intMaxPatientCount);

                                        objSlotsModel.strSpeciality = Convert.ToString(dtdocslots.Rows[0]["specialtyName"]);
                                        lstdoctorsSlots.Add(objSlotsModel);
                                    }
                                    else
                                    {
                                        SlotsModel objSlotsModel = new SlotsModel();
                                        objSlotsModel.intId = Convert.ToInt32(dtdocslots.Rows[0]["intId"]);
                                        objSlotsModel.intStatusId = 3;
                                        objSlotsModel.strStartTime = lstSlots[intslotcount].strStartTime;
                                        objSlotsModel.strEndTime = lstSlots[intslotcount].strEndTime;
                                        objSlotsModel.intPresentPastFutureStatus = lstSlots[intslotcount].intPresentPastFutureStatus;
                                        objSlotsModel.intMaxPatientCount = Convert.ToInt32(dtdocslots.Rows[0]["intMaxSlots"]);
                                        objSlotsModel.intPatientCount = GetPatientCountByDocIdAndSlotId(Convert.ToInt32(dtdocslots.Rows[0]["intId"]));
                                        objSlotsModel.decBookPercentage = Convert.ToDecimal((objSlotsModel.intPatientCount * 100) / objSlotsModel.intMaxPatientCount);
                                        objSlotsModel.strSpeciality = Convert.ToString(dtdocslots.Rows[0]["specialtyName"]);
                                        lstdoctorsSlots.Add(objSlotsModel);
                                    }
                                }
                                else
                                {
                                    SlotsModel objSlotsModel = new SlotsModel();
                                    objSlotsModel.intId = lstSlots[intslotcount].intId;
                                    objSlotsModel.intStatusId = 2;
                                    objSlotsModel.strStartTime = lstSlots[intslotcount].strStartTime;
                                    objSlotsModel.strEndTime = lstSlots[intslotcount].strEndTime;
                                    objSlotsModel.intPresentPastFutureStatus = lstSlots[intslotcount].intPresentPastFutureStatus;
                                    objSlotsModel.intMaxPatientCount = 0;
                                    objSlotsModel.intPatientCount = 0;
                                    objSlotsModel.decBookPercentage = 0;
                                    lstdoctorsSlots.Add(objSlotsModel);
                                }
                            }

                            objDoctors.lstSlots = lstdoctorsSlots;
                            lstDoctorsModel.Add(objDoctors);
                        }
                    }
                    lstAllDoctorsAppointementsModel.lstSlots = lstSlots;
                    lstAllDoctorsAppointementsModel.lstDoctors = lstDoctorsModel;

                    return lstAllDoctorsAppointementsModel;
                }

                else
                {
                    return lstAllDoctorsAppointementsModel;
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }
        public DoctorsModel GetDoctorsAvailabilitysByDateAndDocId(DoctorsAvailabilityModel objDoctorsAvailabilityModel)
        {
            DbContext dbContext = new DbContext();
            List<SlotsModel> lstSlots = new List<SlotsModel>();
            DoctorsModel objDoctors = new DoctorsModel();
            DateTime currentTime = DateTime.Now;
            DateTime SlotStartTime;
            DateTime SlotEndTime;
            string strSlotStartTime;
            string strSlotEndTime;
            string strSlotDate;
            DateTime ddtSlotDate;
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
                        strSlotStartTime = Convert.ToDateTime(objSlotsModel.strStartTime).ToString("yyyy-MM-dd HH:mm");
                        strSlotEndTime = Convert.ToDateTime(objSlotsModel.strEndTime).ToString("yyyy-MM-dd HH:mm");
                        SlotStartTime = DateTime.Parse(strSlotStartTime);
                        SlotEndTime = DateTime.Parse(strSlotEndTime);
                        //SlotStartTime = Convert.ToDateTime(strSlotStartTime);
                        //SlotEndTime = Convert.ToDateTime(strSlotEndTime);
                        strSlotDate = Convert.ToDateTime(objDoctorsAvailabilityModel.strDate).ToString("yyyy-MM-dd HH:mm");
                        // ddtSlotDate = DateTime.Parse(strSlotDate);
                        ddtSlotDate = Convert.ToDateTime(strSlotDate);
                        if (currentTime.Date < ddtSlotDate.Date)
                        {
                            objSlotsModel.intPresentPastFutureStatus = 3;
                        }
                        else if (currentTime.Date == ddtSlotDate.Date)
                        {
                            if (TimeSpan.Compare(currentTime.TimeOfDay, SlotStartTime.TimeOfDay) == -1)
                            {
                                objSlotsModel.intPresentPastFutureStatus = 3;
                            }
                            else if (TimeSpan.Compare(currentTime.TimeOfDay, SlotEndTime.TimeOfDay) == 1)
                            {
                                objSlotsModel.intPresentPastFutureStatus = 1;
                            }
                            else
                                objSlotsModel.intPresentPastFutureStatus = 2;
                        }
                        else
                            objSlotsModel.intPresentPastFutureStatus = 1;
                        objSlotsModel.intActive = Convert.ToInt32(dr["intActive"]);
                        objSlotsModel.intCreatedBy = Convert.ToInt32(dr["intCreatedBy"]);
                        objSlotsModel.strCreationDate = Convert.ToDateTime(dr["dttCreationDate"]).ToString("yyyy-MM-dd");
                        lstSlots.Add(objSlotsModel);
                    }
                    strQuery = $"call doctors_R_SP (4," + objDoctorsAvailabilityModel.intDoctorId + ",'" + objDoctorsAvailabilityModel.strDate + "','');";
                    dt = dbContext.GetDataTable(strQuery, "DbContext");

                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            objDoctors.intId = Convert.ToInt32(dr["intDocSpecialityId"]);
                            objDoctors.strFirstName = Convert.ToString(dr["varFirstName"] ?? "");
                            objDoctors.strMiddleName = Convert.ToString(dr["varMiddleName"] ?? "");
                            objDoctors.strLastName = Convert.ToString(dr["varLastName"] ?? "");
                            objDoctors.intGender = Convert.ToInt32(dr["intGender"] ?? 0);
                            List<SlotsModel> lstdoctorsSlots = new List<SlotsModel>();
                            strQuery = $"call doctorsavailability_R_SP (7,'" + objDoctorsAvailabilityModel.strDate + "',0," + objDoctors.intId + ",0);";
                            int intMaxCount = dbContext.ExecuteScaler(strQuery, "DbContext");
                            for (int intslotcount = 0; intslotcount < lstSlots.Count; intslotcount++)
                            {
                                strQuery = $"call doctorsavailability_R_SP (5,'" + objDoctorsAvailabilityModel.strDate + "',0," + objDoctors.intId + "," + lstSlots[intslotcount].intId + ");";
                                DataTable dtdocslots = dbContext.GetDataTable(strQuery, "DbContext");
                                if (dtdocslots.Rows.Count > 0)
                                {

                                    if (Convert.ToInt32(dtdocslots.Rows[0]["intDoctorId"]) == objDoctors.intId)
                                    {

                                        SlotsModel objSlotsModel = new SlotsModel();
                                        objSlotsModel.intId = Convert.ToInt32(dtdocslots.Rows[0]["intId"]);
                                        objSlotsModel.strStartTime = lstSlots[intslotcount].strStartTime;
                                        objSlotsModel.strEndTime = lstSlots[intslotcount].strEndTime;
                                        objSlotsModel.strStartEndTime = lstSlots[intslotcount].strStartTime + "-" + lstSlots[intslotcount].strEndTime;
                                        objSlotsModel.intPresentPastFutureStatus = lstSlots[intslotcount].intPresentPastFutureStatus;
                                        objSlotsModel.intStatusId = 1;
                                        objSlotsModel.intMaxPatientCount = Convert.ToInt32(dtdocslots.Rows[0]["intMaxSlots"]);
                                        objSlotsModel.intPatientCount = GetPatientCountByDocIdAndSlotId(Convert.ToInt32(dtdocslots.Rows[0]["intId"]));
                                        objSlotsModel.intPatientComplete = GetPatientCompleteCountByDocIdAndSlotId(Convert.ToInt32(dtdocslots.Rows[0]["intId"]));
                                        strQuery = $"call patient_R_SP (6," + objDoctorsAvailabilityModel.intAppointmentSlot + ",0);";
                                        DataTable dtPatient = dbContext.GetDataTable(strQuery, "DbContext");
                                        if (dtPatient.Rows.Count > 0)
                                        {
                                            objSlotsModel.strPatientIds = Convert.ToString(dtPatient.Rows[0]["varPatientIds"]);
                                            objSlotsModel.strPatientAppIds = Convert.ToString(dtPatient.Rows[0]["varPatientAppIds"]);
                                        }
                                        clsPatient objclsPatient = new clsPatient();                                      

                                        objSlotsModel.lstPatient = objclsPatient.GetPatientByDocIdSlotId(objSlotsModel.intId);
                                        objSlotsModel.decBookPercentage = Convert.ToDecimal((objSlotsModel.intPatientCount * 100) / objSlotsModel.intMaxPatientCount);
                                        objSlotsModel.strSpeciality = Convert.ToString(dtdocslots.Rows[0]["specialtyName"]);
                                        lstdoctorsSlots.Add(objSlotsModel);
                                    }
                                    else
                                    {
                                        SlotsModel objSlotsModel = new SlotsModel();
                                        objSlotsModel.intId = Convert.ToInt32(dtdocslots.Rows[0]["intId"]);
                                        objSlotsModel.strStartTime = lstSlots[intslotcount].strStartTime;
                                        objSlotsModel.strEndTime = lstSlots[intslotcount].strEndTime;
                                        objSlotsModel.strStartEndTime = lstSlots[intslotcount].strStartTime + "-" + lstSlots[intslotcount].strEndTime;
                                        objSlotsModel.intPresentPastFutureStatus = lstSlots[intslotcount].intPresentPastFutureStatus;
                                        objSlotsModel.intStatusId = 3;
                                        objSlotsModel.intMaxPatientCount = Convert.ToInt32(dtdocslots.Rows[0]["intMaxSlots"]);
                                        objSlotsModel.intPatientCount = GetPatientCountByDocIdAndSlotId(Convert.ToInt32(dtdocslots.Rows[0]["intId"]));
                                        objSlotsModel.intPatientComplete = GetPatientCompleteCountByDocIdAndSlotId(Convert.ToInt32(dtdocslots.Rows[0]["intId"]));
                                        strQuery = $"call patient_R_SP (6," + objDoctorsAvailabilityModel.intAppointmentSlot + ",0);";
                                        DataTable dtPatient = dbContext.GetDataTable(strQuery, "DbContext");
                                        if (dtPatient.Rows.Count > 0)
                                        {
                                            objSlotsModel.strPatientIds = Convert.ToString(dtPatient.Rows[0]["varPatientIds"]);
                                            objSlotsModel.strPatientAppIds = Convert.ToString(dtPatient.Rows[0]["varPatientAppIds"]);
                                        }
                                        clsPatient objclsPatient = new clsPatient();
                                        objSlotsModel.lstPatient = objclsPatient.GetPatientByDocIdSlotId(objSlotsModel.intId);
                                        objSlotsModel.decBookPercentage = Convert.ToDecimal((objSlotsModel.intPatientCount * 100) / objSlotsModel.intMaxPatientCount);
                                        objSlotsModel.strSpeciality = Convert.ToString(dtdocslots.Rows[0]["specialtyName"]);
                                        lstdoctorsSlots.Add(objSlotsModel);
                                    }
                                }
                                else
                                {
                                    SlotsModel objSlotsModel = new SlotsModel();
                                    objSlotsModel.intId = lstSlots[intslotcount].intId;
                                    objSlotsModel.strStartTime = lstSlots[intslotcount].strStartTime;
                                    objSlotsModel.strEndTime = lstSlots[intslotcount].strEndTime;
                                    objSlotsModel.strStartEndTime = lstSlots[intslotcount].strStartTime + "-" + lstSlots[intslotcount].strEndTime;
                                    objSlotsModel.intPresentPastFutureStatus = lstSlots[intslotcount].intPresentPastFutureStatus;
                                    objSlotsModel.intStatusId = 2;
                                    objSlotsModel.intMaxPatientCount = 0; 
                                    objSlotsModel.intPatientCount = 0;
                                    objSlotsModel.intPatientComplete = 0;
                                    objSlotsModel.strPatientIds = "";
                                    objSlotsModel.decBookPercentage = 0;
                                    clsPatient objclsPatient = new clsPatient();
                                    objSlotsModel.lstPatient = objclsPatient.GetPatientByDocIdSlotId(objSlotsModel.intId);
                                    lstdoctorsSlots.Add(objSlotsModel);
                                }
                            }

                            objDoctors.lstSlots = lstdoctorsSlots;
                        }
                    }
                    return objDoctors;
                }
                else
                {
                    return objDoctors;
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }

        //public List<SlotsModel> GetDoctorsSlotsAvailabilitysByDateAndDocId(DoctorsAvailabilityModel objDoctorsAvailabilityModel)
        //{
        //    DbContext dbContext = new DbContext();
        //    List<SlotsModel> lstSlots = new List<SlotsModel>();
        //    DoctorsModel objDoctors = new DoctorsModel();
        //    try
        //    {
        //        string strQuery = $"call masslots_R_SP (1,0,0);";
        //        DataTable dt = dbContext.GetDataTable(strQuery, "DbContext");
        //        if (dt.Rows.Count > 0)
        //        {
        //            foreach (DataRow dr in dt.Rows)
        //            {
        //                SlotsModel objSlotsModel = new SlotsModel();
        //                objSlotsModel.intId = Convert.ToInt32(dr["intId"]);
        //                objSlotsModel.strStartTime = Convert.ToString(dr["varStartTime"]);
        //                objSlotsModel.strEndTime = Convert.ToString(dr["varEndTime"]);
        //                objSlotsModel.intActive = Convert.ToInt32(dr["intActive"]);
        //                objSlotsModel.intCreatedBy = Convert.ToInt32(dr["intCreatedBy"]);
        //                objSlotsModel.strCreationDate = Convert.ToDateTime(dr["dttCreationDate"]).ToString("yyyy-MM-dd");

        //                #region MyRegion
        //                if (dt.Rows.Count > 0)
        //                {                            
        //                    List<SlotsModel> lstdoctorsSlots = new List<SlotsModel>();
        //                    for (int intslotcount = 0; intslotcount < lstSlots.Count; intslotcount++)
        //                    {
        //                        strQuery = $"call doctorsavailability_R_SP (5,'" + objDoctorsAvailabilityModel.strDate + "',0," + objDoctors.intId + "," + lstSlots[intslotcount].intId + ");";
        //                        DataTable dtdocslots = dbContext.GetDataTable(strQuery, "DbContext");
        //                        if (dtdocslots.Rows.Count > 0)
        //                        {
        //                            objSlotsModel.intId = Convert.ToInt32(dtdocslots.Rows[0]["intId"]);
        //                            objSlotsModel.intStatusId = 1;
        //                            objSlotsModel.intMaxPatientCount = Convert.ToInt32(dtdocslots.Rows[0]["intMaxSlots"]);
        //                            objSlotsModel.intPatientCount = GetPatientCountByDocIdAndSlotId(Convert.ToInt32(dtdocslots.Rows[0]["intId"]));
        //                            strQuery = $"call patient_R_SP (6," + objDoctorsAvailabilityModel.intAppointmentSlot + ",0);";
        //                            DataTable dtPatient = dbContext.GetDataTable(strQuery, "DbContext");
        //                            if (dtPatient.Rows.Count > 0)
        //                            {
        //                                objSlotsModel.strPatientIds = Convert.ToString(dtPatient.Rows[0]["varPatientIds"]);
        //                                objSlotsModel.strPatientAppIds = Convert.ToString(dtPatient.Rows[0]["varPatientAppIds"]);
        //                            }
        //                            objSlotsModel.decBookPercentage = Convert.ToDecimal((objSlotsModel.intPatientCount * 100) / objSlotsModel.intMaxPatientCount);
        //                            lstdoctorsSlots.Add(objSlotsModel);
        //                        }
        //                        objDoctors.lstSlots = lstdoctorsSlots;
        //                    }
        //                }
        //                #endregion
        //                lstSlots.Add(objSlotsModel);
        //            }
        //            return lstSlots;
        //        }
        //        else
        //        {
        //            return lstSlots;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
        //        throw ex;
        //    }
        //}
        public int GetPatientCountByDocIdAndSlotId(int intAvailableslotId)
        {
            int result = 0;
            DbContext dbContext = new DbContext();
            try
            {
                string strQuery = $"call PatientCountByDocIdAndSlotId_R_SP(1," + intAvailableslotId + "); ";
                result = dbContext.ExecuteScaler(strQuery, "DbContext");
                return result;
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }
        public int GetPatientCompleteCountByDocIdAndSlotId(int intAvailableslotId)
        {
            int result = 0;
            DbContext dbContext = new DbContext();
            try
            {
                string strQuery = $"call PatientCountByDocIdAndSlotId_R_SP(2," + intAvailableslotId + "); ";
                result = dbContext.ExecuteScaler(strQuery, "DbContext");
                return result;
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }
        public AllDoctorsAppointementsModel GetAllDoctorsAvailabilitysByDateAndPatientCounts(DoctorsAvailabilityModel objDoctorsAvailabilityModel)
        {
            DbContext dbContext = new DbContext();
            List<SlotsModel> lstSlots = new List<SlotsModel>();
            List<DoctorsModel> lstDoctorsModel = new List<DoctorsModel>();
            AllDoctorsAppointementsModel lstAllDoctorsAppointementsModel = new AllDoctorsAppointementsModel();
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
                        objSlotsModel.intActive = Convert.ToInt32(dr["intActive"]);
                        objSlotsModel.intCreatedBy = Convert.ToInt32(dr["intCreatedBy"]);
                        objSlotsModel.strCreationDate = Convert.ToDateTime(dr["dttCreationDate"]).ToString("yyyy-MM-dd");
                        lstSlots.Add(objSlotsModel);
                    }
                    strQuery = $"call doctors_R_SP (1,0,'2023-09-09','');";
                    dt = dbContext.GetDataTable(strQuery, "DbContext");

                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            DoctorsModel objDoctors = new DoctorsModel();
                            objDoctors.lstPatientAppointements = new List<PatientAppointmentModel>();
                            objDoctors.intId = Convert.ToInt32(dr["intId"]);
                            objDoctors.strFirstName = Convert.ToString(dr["varFirstName"] ?? "");
                            objDoctors.strMiddleName = Convert.ToString(dr["varMiddleName"] ?? "");
                            objDoctors.strLastName = Convert.ToString(dr["varLastName"] ?? "");
                            objDoctors.intGender = Convert.ToInt32(dr["intGender"] ?? 0);

                        }
                    }
                    lstAllDoctorsAppointementsModel.lstSlots = lstSlots;
                    lstAllDoctorsAppointementsModel.lstDoctors = lstDoctorsModel;


                    // procedure (Query Select * from doctorsavailability where  dttDate="2023-10-04")
                    // uske bad loop chalana h inke records ka (list of DoctorsAppintement Table k Records ka)
                    // Or har ek index k bad dusra procedure calll krna h 
                    // slots kki id k liye (Query Select * from doctorsavailabilityslots where intDoctorId=1 and varSlotDate="";)
                    // doctor id  milege tumhe DoctorsAppintement se or date milege tumhe iuser se input lene h jo tum yha pass karogae



                    /*  lstAllDoctorsAppointementsModel.Add(lstSlots)*/
                    return lstAllDoctorsAppointementsModel;
                }
                else
                {
                    return lstAllDoctorsAppointementsModel;
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }



        #endregion
    }
}