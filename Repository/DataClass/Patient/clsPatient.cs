using HospitalManagement.DbConnect;
using HospitalManagement.Repository.Interface;
using HospitalManagement_Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalManagement.Repository.DataClass.Patient
{

    public class clsPatient : IPatient
    {

        #region Patient Login
        public PatientModel PatientLogin(PatientModel patients)
        {
            DbContext dbContext = new DbContext();
            PatientModel usertoken = new PatientModel();
            try
            {
                string strQuery = "select * from patient where varEmail = '" + patients.strEmail + "' and varPassword = '" + patients.strPassword + "';";
                DataTable dt = dbContext.GetDataTable(strQuery, "DbContext");
                if (dt.Rows.Count > 0)
                {
                    usertoken.intId = Convert.ToInt32(dt.Rows[0]["intId"]);
                    usertoken.strEmail = Convert.ToString(dt.Rows[0]["varEmail"]);
                    usertoken.strUserName = Convert.ToString(dt.Rows[0]["varUserName"]);
                    /*usertoken.intActive = Convert.ToInt32(dt.Rows[0]["intActive"]);*/
                    usertoken.intUserType = 3;
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
        public int InsertUpdatePatient(PatientModel objPatientModel)
        {
            DbContext dbContext = new DbContext();
            try
            {

                int result = -1;
                int validation = 0;
                string strPhoneEmailCheck = "";
                strPhoneEmailCheck = "call PatentEmailCheck_R_SP(1,'" + objPatientModel.strEmail + "','');";
                validation = dbContext.ExecuteScaler(strPhoneEmailCheck, "DbContext");
                if (validation <= 0)
                {
                    strPhoneEmailCheck = "call PatentEmailCheck_R_SP(2,'','" + objPatientModel.strMobileNo + "');";
                    validation = dbContext.ExecuteScaler(strPhoneEmailCheck, "DbContext");
                    if (validation <= 0)
                    {
                        if (objPatientModel.intParentId == 0)
                        {
                            strPhoneEmailCheck = "select intId from patient where (varEmail = '" + objPatientModel.strEmail + "' ) and intId != " + objPatientModel.intId + ";";
                            validation = dbContext.ExecuteScaler(strPhoneEmailCheck, "DbContext");
                            if (validation <= 0)
                            {
                                strPhoneEmailCheck = "select intId from patient where (varMobileNo = '" + objPatientModel.strMobileNo + "') and intId != " + objPatientModel.intId + ";";
                                validation = dbContext.ExecuteScaler(strPhoneEmailCheck, "DbContext");
                                if (validation <= 0)
                                {
                                    string strQuery = "call patient_CU_SP (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?);";
                                    MySqlParameter[] param = {
                        new MySqlParameter("@intId", objPatientModel.intId),
                        new MySqlParameter("@intParentId", objPatientModel.intParentId),
                        new MySqlParameter("@varFirstName", objPatientModel.strFirstName),
                        new MySqlParameter("@varMiddleName", objPatientModel.strMiddleName),
                        new MySqlParameter("@varLastName", objPatientModel.strLastName),
                        new MySqlParameter("@intGender", objPatientModel.intGender),
                        new MySqlParameter("@varUserName", objPatientModel.strUserName),
                        new MySqlParameter("@varPassword", objPatientModel.strPassword),
                        new MySqlParameter("@varMobileNo", objPatientModel.strMobileNo),
                        new MySqlParameter("@varEmail", objPatientModel.strEmail),
                        new MySqlParameter("@intCountryId", objPatientModel.intCountryId),
                        new MySqlParameter("@intStateId", objPatientModel.intStateId),
                        new MySqlParameter("@intCityId", objPatientModel.intCityId),
                        new MySqlParameter("@intCreatedBy", objPatientModel.intCreatedBy),
                        new MySqlParameter("@varAddress", objPatientModel.strAddress),
                        new MySqlParameter("@intRelation", objPatientModel.intRelationId),
                        new MySqlParameter("@dttDOB", objPatientModel.dttDOB),
                    };
                                    result = dbContext.ExecuteCommandWithParameter(strQuery, param, true, "DbContext");
                                    return result;
                                }
                                else
                                    return -2;
                            }
                            else
                                return -1;
                        }
                        else
                        {
                            strPhoneEmailCheck = "select intId from patient where (varEmail = '" + objPatientModel.strEmail + "') and ((intId != " + objPatientModel.intParentId + " and intParentId !=" + objPatientModel.intParentId + ") ) and intId != " + objPatientModel.intId + ";";
                            validation = dbContext.ExecuteScaler(strPhoneEmailCheck, "DbContext");
                            if (validation <= 0)
                            {
                                strPhoneEmailCheck = "select intId from patient where (varMobileNo = '" + objPatientModel.strMobileNo + "') and ((intId != " + objPatientModel.intParentId + " and intParentId !=" + objPatientModel.intParentId + ") ) and intId != " + objPatientModel.intId + "; ";
                                validation = dbContext.ExecuteScaler(strPhoneEmailCheck, "DbContext");
                                if (validation <= 0)
                                {
                                    string strQuery = "call patient_CU_SP (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?);";
                                    MySqlParameter[] param = {
                        new MySqlParameter("@intId", objPatientModel.intId),
                        new MySqlParameter("@intParentId", objPatientModel.intParentId),
                        new MySqlParameter("@varFirstName", objPatientModel.strFirstName),
                        new MySqlParameter("@varMiddleName", objPatientModel.strMiddleName),
                        new MySqlParameter("@varLastName", objPatientModel.strLastName),
                        new MySqlParameter("@intGender", objPatientModel.intGender),
                        new MySqlParameter("@varUserName", objPatientModel.strUserName),
                        new MySqlParameter("@varPassword", objPatientModel.strPassword),
                        new MySqlParameter("@varMobileNo", objPatientModel.strMobileNo),
                        new MySqlParameter("@varEmail", objPatientModel.strEmail),
                        new MySqlParameter("@intCountryId", objPatientModel.intCountryId),
                        new MySqlParameter("@intStateId", objPatientModel.intStateId),
                        new MySqlParameter("@intCityId", objPatientModel.intCityId),
                        new MySqlParameter("@intCreatedBy", objPatientModel.intCreatedBy),
                        new MySqlParameter("@varAddress", objPatientModel.strAddress),
                        new MySqlParameter("@intRelation", objPatientModel.intRelationId),
                        new MySqlParameter("@dttDOB", objPatientModel.dttDOB),

                    };
                                    result = dbContext.ExecuteCommandWithParameter(strQuery, param, true, "DbContext");
                                    return result;
                                }
                                else
                                    return -2;
                            }
                            else
                                return -1;

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
        public int InsertUpdatePatientImagePath(PatientModel objPatientModel)
        {
            DbContext dbContext = new DbContext();
            int result = -1;
            try
            {

                string strQuery = "call patient_ImagePath_CU_SP (?,?);";
                MySqlParameter[] param = {
                new MySqlParameter("@intId", objPatientModel.intId),
                new MySqlParameter("@varImagePath", objPatientModel.strImagePath),
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
        public int UpdatePatientPassword(PatientModel patientModel)
        {
            DbContext dbContext = new DbContext();
            int result = -1;
            try
            {

                string strQuery = "call patient_Password_U_SP (?,?,?);";
                MySqlParameter[] param = {
                new MySqlParameter("@intId", patientModel.intId),
                new MySqlParameter("@varPassword", patientModel.strPassword),
                new MySqlParameter("@varOldPassword", patientModel.strOldPassword),
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
        public List<PatientModel> GetPatient(string strSearch)
        {
            DbContext dbContext = new DbContext();
            List<PatientModel> lstobjPatient = new List<PatientModel>();
            try
            {
                string strQuery = $"call patient_R_SP (1,0,'"+ strSearch + "');";
                DataTable dt = dbContext.GetDataTable(strQuery, "DbContext");
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        PatientModel objPatientModel = new PatientModel();
                        objPatientModel.intId = Convert.ToInt32(dr["intId"]);
                        objPatientModel.intParentId = Convert.ToInt32(dr["intParentId"]);
                        objPatientModel.strFirstName = Convert.ToString(dr["varFirstName"]);
                        objPatientModel.strMiddleName = Convert.ToString(dr["varMiddleName"]);
                        objPatientModel.strLastName = Convert.ToString(dr["varLastName"]);
                        objPatientModel.intGender = Convert.ToInt32(dr["intGender"]);
                        objPatientModel.strUserName = Convert.ToString(dr["varUserName"]);
                        objPatientModel.strPassword = Convert.ToString(dr["varPassword"]);
                        objPatientModel.strMobileNo = Convert.ToString(dr["varMobileNo"]);
                        objPatientModel.strEmail = Convert.ToString(dr["varEmail"]);
                        objPatientModel.intCountryId = Convert.ToInt32(dr["intCountryId"]);
                        objPatientModel.intStateId = Convert.ToInt32(dr["intStateId"]);
                        objPatientModel.intCityId = Convert.ToInt32(dr["intCityId"]);
                        objPatientModel.strCountryName = Convert.ToString(dr["varCountryName"]);
                        objPatientModel.strStateName = Convert.ToString(dr["varStateName"]);
                        objPatientModel.strCityName = Convert.ToString(dr["varCityName"]);
                        objPatientModel.intCreatedBy = Convert.ToInt32(dr["intCreatedBy"]);
                        objPatientModel.strImagePath = Convert.ToString(dr["varImagePath"]);
                        objPatientModel.strAddress = Convert.ToString(dr["varAddress"]);
                        objPatientModel.strRelation = Convert.ToString(dr["Relation"]);
                        objPatientModel.dttDOB= Convert.ToDateTime(dr["dttDOB"]).ToString("yyyy-MM-dd");
                        objPatientModel.strCreationDate = Convert.ToDateTime(dr["dttCreationDate"]).ToString("yyyy-MM-dd");
                        objPatientModel.strCreatedBy = Convert.ToString(dr["varUserFirstName"]) + " " + Convert.ToString(dr["varUserMiddleName"]) + " " + Convert.ToString(dr["varUserLastName"]);
                        
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
        public List<PatientModel> GetPatientFamily(int intParentId)
        {
            DbContext dbContext = new DbContext();
            List<PatientModel> lstobjPatient = new List<PatientModel>();
            try
            {
                string strQuery = $"call patient_R_SP (8," + intParentId + ",'');";
                DataTable dt = dbContext.GetDataTable(strQuery, "DbContext");
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        PatientModel objPatientModel = new PatientModel();
                        objPatientModel.intId = Convert.ToInt32(dr["intId"]);
                        objPatientModel.strFirstName = Convert.ToString(dr["varFirstName"]);
                        objPatientModel.strMiddleName = Convert.ToString(dr["varMiddleName"]);
                        objPatientModel.strLastName = Convert.ToString(dr["varLastName"]);
                        objPatientModel.intGender = Convert.ToInt32(dr["intGender"]);
                        objPatientModel.strUserName = Convert.ToString(dr["varUserName"]);
                        objPatientModel.strPassword = Convert.ToString(dr["varPassword"]);
                        objPatientModel.strMobileNo = Convert.ToString(dr["varMobileNo"]);
                        objPatientModel.strEmail = Convert.ToString(dr["varEmail"]);
                        objPatientModel.intCountryId = Convert.ToInt32(dr["intCountryId"]);
                        objPatientModel.intStateId = Convert.ToInt32(dr["intStateId"]);
                        objPatientModel.intCityId = Convert.ToInt32(dr["intCityId"]);
                        objPatientModel.strCountryName = Convert.ToString(dr["varCountryName"]);
                        objPatientModel.strStateName = Convert.ToString(dr["varStateName"]);
                        objPatientModel.strCityName = Convert.ToString(dr["varCityName"]);
                        objPatientModel.intCreatedBy = Convert.ToInt32(dr["intCreatedBy"]);
                        objPatientModel.strImagePath = Convert.ToString(dr["varImagePath"]);
                        objPatientModel.strAddress = Convert.ToString(dr["varAddress"]);
                        objPatientModel.strRelation = Convert.ToString(dr["SpecialityName"]); 
                        objPatientModel.strCreationDate = Convert.ToDateTime(dr["dttCreationDate"]).ToString("yyyy-MM-dd");
                        objPatientModel.strCreatedBy = Convert.ToString(dr["varUserFirstName"]) + " " + Convert.ToString(dr["varUserMiddleName"]) + " " + Convert.ToString(dr["varUserLastName"]);
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
        public List<PatientModel> GetPatientAndFamily(int intParentId)
        {
            DbContext dbContext = new DbContext();
            List<PatientModel> lstobjPatient = new List<PatientModel>();
            try
            {
                string strQuery = $"call patient_R_SP (5," + intParentId + ",'');";
                DataTable dt = dbContext.GetDataTable(strQuery, "DbContext");
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        PatientModel objPatientModel = new PatientModel();
                        objPatientModel.intId = Convert.ToInt32(dr["intId"]);
                        objPatientModel.intParentId = Convert.ToInt32(dr["intParentId"]);
                        objPatientModel.strFirstName = Convert.ToString(dr["varFirstName"]);
                        objPatientModel.strMiddleName = Convert.ToString(dr["varMiddleName"]);
                        objPatientModel.strLastName = Convert.ToString(dr["varLastName"]);
                        objPatientModel.intGender = Convert.ToInt32(dr["intGender"]);
                        objPatientModel.strUserName = Convert.ToString(dr["varUserName"]);
                        objPatientModel.strPassword = Convert.ToString(dr["varPassword"]);
                        objPatientModel.strMobileNo = Convert.ToString(dr["varMobileNo"]);
                        objPatientModel.strEmail = Convert.ToString(dr["varEmail"]);
                        //     objPatientModel.intCountryId = Convert.ToInt32(dr["intCountryId"]);
                        ///     objPatientModel.intStateId = Convert.ToInt32(dr["intStateId"]);
                        //     objPatientModel.intCityId = Convert.ToInt32(dr["intCityId"]);
                        if (Convert.ToInt32(dr["cityActive"]) == 1)
                            objPatientModel.intCityId = Convert.ToInt32(dr["intCityId"]);
                        if (Convert.ToInt32(dr["stateActive"]) == 1)
                            objPatientModel.intStateId = Convert.ToInt32(dr["intStateId"]);
                        else
                        {
                            objPatientModel.intCityId = 0;
                        }

                        if (Convert.ToInt32(dr["countryActive"]) == 1)
                            objPatientModel.intCountryId = Convert.ToInt32(dr["intCountryId"]);
                        else
                        {
                            objPatientModel.intCityId = 0;
                            objPatientModel.intStateId = 0;
                        }
                        objPatientModel.strCountryName = Convert.ToString(dr["varCountryName"]);
                        objPatientModel.strStateName = Convert.ToString(dr["varStateName"]);
                        objPatientModel.strCityName = Convert.ToString(dr["varCityName"]);
                        objPatientModel.intCreatedBy = Convert.ToInt32(dr["intCreatedBy"]);
                        objPatientModel.strImagePath = Convert.ToString(dr["varImagePath"]);
                        objPatientModel.strAddress = Convert.ToString(dr["varAddress"]);
                        objPatientModel.intRelationId = Convert.ToInt32(dr["intRelation"]);
                        objPatientModel.strRelation = Convert.ToString(dr["Relation"]);
                        objPatientModel.dttDOB = Convert.ToDateTime(dr["dttDOB"]).ToString("yyyy-MM-dd");
                        objPatientModel.strCreationDate = Convert.ToDateTime(dr["dttCreationDate"]).ToString("yyyy-MM-dd");
                        objPatientModel.strCreatedBy = Convert.ToString(dr["varUserFirstName"]) + " " + Convert.ToString(dr["varUserMiddleName"]) + " " + Convert.ToString(dr["varUserLastName"]);
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
        public List<PatientModel> GetPatientsByMobileNumber(string strMobilenumber)
        {
            DbContext dbContext = new DbContext();
            List<PatientModel> lstobjPatient = new List<PatientModel>();
            try
            {
                string strQuery = $"call patient_R_SP (3,0,'" + strMobilenumber + "');";
                DataTable dt = dbContext.GetDataTable(strQuery, "DbContext");
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        PatientModel objPatientModel = new PatientModel();

                        objPatientModel.intId = Convert.ToInt32(dr["intId"]);
                        objPatientModel.strFirstName = Convert.ToString(dr["varFirstName"]);
                        objPatientModel.strMiddleName = Convert.ToString(dr["varMiddleName"]);
                        objPatientModel.strLastName = Convert.ToString(dr["varLastName"]);
                        objPatientModel.intGender = Convert.ToInt32(dr["intGender"]);
                        objPatientModel.strUserName = Convert.ToString(dr["varUserName"]);
                        objPatientModel.strPassword = Convert.ToString(dr["varPassword"]);
                        objPatientModel.strMobileNo = Convert.ToString(dr["varMobileNo"]);
                        objPatientModel.strEmail = Convert.ToString(dr["varEmail"]);
                        objPatientModel.intCountryId = Convert.ToInt32(dr["intCountryId"]);
                        objPatientModel.intStateId = Convert.ToInt32(dr["intStateId"]);
                        objPatientModel.intCityId = Convert.ToInt32(dr["intCityId"]);
                        objPatientModel.strCountryName = Convert.ToString(dr["varCountryName"]);
                        objPatientModel.strStateName = Convert.ToString(dr["varStateName"]);
                        objPatientModel.strCityName = Convert.ToString(dr["varCityName"]);
                        objPatientModel.intCreatedBy = Convert.ToInt32(dr["intCreatedBy"]);
                        objPatientModel.strImagePath = Convert.ToString(dr["varImagePath"]);
                        objPatientModel.strCreationDate = Convert.ToDateTime(dr["dttCreationDate"]).ToString("dd-MM-yyyy");
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
        public PatientModel GetPatientById(int intId)
        {
            DbContext dbContext = new DbContext();
            try
            {
                PatientModel objPatientModel = new PatientModel();
                string strQuery = $"call patient_R_SP(2," + intId + ",'');";
                DataTable dt = dbContext.GetDataTable(strQuery, "DbContext");
                if (dt.Rows.Count > 0)
                {
                    objPatientModel.intId = Convert.ToInt32(dt.Rows[0]["intId"]);
                    objPatientModel.strFirstName = Convert.ToString(dt.Rows[0]["varFirstName"]);
                    objPatientModel.strMiddleName = Convert.ToString(dt.Rows[0]["varMiddleName"]);
                    objPatientModel.strLastName = Convert.ToString(dt.Rows[0]["varLastName"]);
                    objPatientModel.intGender = Convert.ToInt32(dt.Rows[0]["intGender"]);
                    objPatientModel.strUserName = Convert.ToString(dt.Rows[0]["varUserName"]);
                    objPatientModel.strPassword = Convert.ToString(dt.Rows[0]["varPassword"]);
                    objPatientModel.strMobileNo = Convert.ToString(dt.Rows[0]["varMobileNo"]);
                    objPatientModel.strEmail = Convert.ToString(dt.Rows[0]["varEmail"]);
                    if (Convert.ToInt32(dt.Rows[0]["cityActive"]) == 1)
                        objPatientModel.intCityId = Convert.ToInt32(dt.Rows[0]["intCityId"]);
                    if (Convert.ToInt32(dt.Rows[0]["stateActive"]) == 1)
                        objPatientModel.intStateId = Convert.ToInt32(dt.Rows[0]["intStateId"]);
                    else
                    {
                        objPatientModel.intCityId = 0;
                    }

                    if (Convert.ToInt32(dt.Rows[0]["countryActive"]) == 1)
                        objPatientModel.intCountryId = Convert.ToInt32(dt.Rows[0]["intCountryId"]);
                    else
                    {
                        objPatientModel.intCityId = 0;
                        objPatientModel.intStateId = 0;
                    }
                    //   objPatientModel.intCountryId = Convert.ToInt32(dt.Rows[0]["intCountryId"]);
                    objPatientModel.strCountryName = Convert.ToString(dt.Rows[0]["varCountryName"]);
                //    objPatientModel.intStateId = Convert.ToInt32(dt.Rows[0]["intStateId"]);
                    objPatientModel.strStateName = Convert.ToString(dt.Rows[0]["varStateName"]);
                 //   objPatientModel.intCityId = Convert.ToInt32(dt.Rows[0]["intCityId"]);
                    objPatientModel.strCityName = Convert.ToString(dt.Rows[0]["varCityName"]);
                    objPatientModel.intCreatedBy = Convert.ToInt32(dt.Rows[0]["intCreatedBy"]);
                    objPatientModel.strImagePath = Convert.ToString(dt.Rows[0]["varImagePath"]);
                    objPatientModel.strAddress = Convert.ToString(dt.Rows[0]["varAddress"]);
                    objPatientModel.intRelationId = Convert.ToInt32(dt.Rows[0]["intRelation"]);
                    objPatientModel.strRelation = Convert.ToString(dt.Rows[0]["Relation"]);
                    objPatientModel.dttDOB = Convert.ToDateTime(dt.Rows[0]["dttDOB"]).ToString("yyyy-MM-dd");
                }
                return objPatientModel;
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }
        public int DeletePatientByid(PatientModel objPatientModel)
        {
            DbContext dbContext = new DbContext();
            try
            {
                string strSQL = $"call patient_D_SP (?);";
                MySqlParameter[] param = {
                    new MySqlParameter("@intId",objPatientModel.intId),

                };

                return dbContext.ExecuteCommandWithParameter(strSQL, param, false, "DbContext");
            }

            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }
        public int InsertUpdatePatientAppointements(PatientAppointmentModel objPatientAppointmentModel)
        {
            DbContext dbContext = new DbContext();
            int result = -1;
            try
            {
                string strPhoneEmailCheck = "select intId from patientappointements where intPatientId = " + objPatientAppointmentModel.intPatientId + " and intAppointmentSlot = " + objPatientAppointmentModel.intAppointmentSlot + " and (intAppointementStatus=1 or intAppointementStatus=2);";
                int validation = dbContext.ExecuteScaler(strPhoneEmailCheck, "DbContext");
                if (validation <= 0)
                {
                    string strQuery = "call patientappointment_CU_SP (?,?,?,?,?,?);";
                    MySqlParameter[] param = {
                        new MySqlParameter("@intId", objPatientAppointmentModel.intId),
                        new MySqlParameter("@intAppointmentSlot", objPatientAppointmentModel.intAppointmentSlot),
                        new MySqlParameter("@intDoctorId", objPatientAppointmentModel.intDoctorId),
                        new MySqlParameter("@intPatientId", objPatientAppointmentModel.intPatientId),
                        new MySqlParameter("@dttCreationDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                        new MySqlParameter("@intAppointementStatus", objPatientAppointmentModel.intAppointementStatus),
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
        public int InsertReschedulePatientAppointements(PatientAppointmentModel objPatientAppointmentModel)
        {
            int result = -1;
            string[] patientIds = objPatientAppointmentModel.strPatientIds.Split(',');
            string[] patientAppIds = objPatientAppointmentModel.strPatientAppIds.Split(',');
            for (int i = 0; i < patientIds.Length; i++)
            {
                PatientAppointmentModel objReschePatientAppointmentModel = new PatientAppointmentModel();
                objReschePatientAppointmentModel.intId = 0;
                objReschePatientAppointmentModel.intDoctorId = objPatientAppointmentModel.intDoctorId;
                objReschePatientAppointmentModel.intPatientId = Convert.ToInt32(patientIds[i]);//objPatientAppointmentModel.lstSelectedPatientApp[i].intPatientId;
                objReschePatientAppointmentModel.intAppointmentSlot = objPatientAppointmentModel.intAppointmentSlot;
                objReschePatientAppointmentModel.intAppointementStatus = objPatientAppointmentModel.intAppointementStatus;
                result = InsertUpdatePatientAppointements(objReschePatientAppointmentModel);
                if (result > 0)
                {
                    objReschePatientAppointmentModel.intId = Convert.ToInt32(patientAppIds[i]);//objPatientAppointmentModel.lstSelectedPatientApp[i].intId;
                    objReschePatientAppointmentModel.intStatusId = objPatientAppointmentModel.intStatusId;
                    CancelReschedulePatientAppointements(objReschePatientAppointmentModel);
                }
            }
            //string[] arrPatientIds = objpatientAppointmentModel.strPatientIds.Split(',');
            //for(int i = 0; i < arrPatientIds.Length; i++)
            //{
            //    objpatientAppointmentModel.intPatientId = Convert.ToInt32(arrPatientIds[i]);
            //    result = InsertUpdatePatientAppointements(objpatientAppointmentModel);
            //}
            //CancelPatientAppointements(objpatientAppointmentModel);
            return result;
        }

        public PatientAppointmentModel GetPatientAppointmentsByintAppointmentId(int intAppointementId)
        {
            DbContext dbContext = new DbContext();
            PatientAppointmentModel objPatientAppointmentModel = new PatientAppointmentModel();
            try
            {

                string strQuery = $"select * from patientappointements where intId = "+ intAppointementId + ";";
                DataTable dt = dbContext.GetDataTable(strQuery, "DbContext");
                if (dt.Rows.Count > 0)
                {                        
                        objPatientAppointmentModel.intId = Convert.ToInt32(dt.Rows[0]["intId"]);
                        objPatientAppointmentModel.intAppointmentSlot = Convert.ToInt32(dt.Rows[0]["intAppointmentSlot"]);
                        objPatientAppointmentModel.intDoctorId = Convert.ToInt32(dt.Rows[0]["intDoctorId"]);
                        objPatientAppointmentModel.intPatientId = Convert.ToInt32(dt.Rows[0]["intPatientId"]);
                        objPatientAppointmentModel.dttCreationDate = Convert.ToDateTime(dt.Rows[0]["dttCreationDate"]);
                        objPatientAppointmentModel.intAppointementStatus = Convert.ToInt32(dt.Rows[0]["intAppointementStatus"]);                     
                }
                return objPatientAppointmentModel;
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }

        }
        public int InsertReschedulePatientAppointementsAdmin(PatientAppointmentModel objPatientAppointmentModel)
        {
            int result = -1;
            for (int i = 0; i < objPatientAppointmentModel.lstSelectedPatientApp.Count; i++)
            {
                PatientAppointmentModel objReschePatientAppointmentModel = new PatientAppointmentModel();
                objReschePatientAppointmentModel.intId = objPatientAppointmentModel.lstSelectedPatientApp[i].intId; 
                objReschePatientAppointmentModel.intDoctorId = objPatientAppointmentModel.intDoctorId;
                objReschePatientAppointmentModel.intPatientId = objPatientAppointmentModel.lstSelectedPatientApp[i].intPatientId;
                objReschePatientAppointmentModel.intAppointmentSlot = objPatientAppointmentModel.intAppointmentSlot;
                objReschePatientAppointmentModel.intAppointementStatus = objPatientAppointmentModel.intAppointementStatus;
                result = UpdatePatientAppointements(objReschePatientAppointmentModel);
                
                //if (result > 0)
                //{
                //    objReschePatientAppointmentModel.intId = objPatientAppointmentModel.lstSelectedPatientApp[i].intId;
                //    objReschePatientAppointmentModel.intStatusId = objPatientAppointmentModel.intStatusId;
                //    CancelReschedulePatientAppointements(objReschePatientAppointmentModel);
                //}
            }
            //string[] arrPatientIds = objpatientAppointmentModel.strPatientIds.Split(',');
            //for(int i = 0; i < arrPatientIds.Length; i++)
            //{
            //    objpatientAppointmentModel.intPatientId = Convert.ToInt32(arrPatientIds[i]);
            //    result = InsertUpdatePatientAppointements(objpatientAppointmentModel);
            //}
            //CancelPatientAppointements(objpatientAppointmentModel);
            return result;
        }

        public int UpdatePatientAppointements(PatientAppointmentModel objPatientAppointmentModel)
        {
            DbContext dbContext = new DbContext();
            int result = -1;
            try
            {
                string strPhoneEmailCheck = "select intId from patientappointements where intPatientId = " + objPatientAppointmentModel.intPatientId + " and intAppointmentSlot = " + objPatientAppointmentModel.intAppointmentSlot + " and (intAppointementStatus=1 or intAppointementStatus=2);";
                int validation = dbContext.ExecuteScaler(strPhoneEmailCheck, "DbContext");
                if (validation <= 0)
                {
                    string strQuery = "update patientappointements set intAppointmentSlot = " + objPatientAppointmentModel.intAppointmentSlot + " where intId = " + objPatientAppointmentModel.intId + ";";
                    result = dbContext.ExecuteCommand(strQuery, "DbContext");

                    PatientAppointmentModel objPatientAppointmentHistoryModel = new PatientAppointmentModel();
                    objPatientAppointmentHistoryModel = GetPatientAppointmentsByintAppointmentId(objPatientAppointmentModel.intId);
                    objPatientAppointmentHistoryModel.intId = 0;
                    strQuery = "call patientappointementshistory_C_SP (?,?,?,?,?,?,?);";
                    MySqlParameter[] param = {
                        new MySqlParameter("@intId", objPatientAppointmentHistoryModel.intId),
                        new MySqlParameter("@intpatientAppointment", objPatientAppointmentModel.intId),
                        new MySqlParameter("@intAppointmentSlot", objPatientAppointmentHistoryModel.intAppointmentSlot),
                        new MySqlParameter("@intDoctorId", objPatientAppointmentHistoryModel.intDoctorId),
                        new MySqlParameter("@intPatientId", objPatientAppointmentHistoryModel.intPatientId),
                        new MySqlParameter("@dttCreationDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                        new MySqlParameter("@intAppointementStatus", 6),
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
        public int CancelReschedulePatientAppointements(PatientAppointmentModel objPatientAppointmentModel)
        {
            DbContext dbContext = new DbContext();
            int result = -1;
            try
            {

                string strQuery = "update patientappointements set intAppointementStatus=" + objPatientAppointmentModel.intStatusId + " where intId = " + objPatientAppointmentModel.intId + ";";
                result = dbContext.ExecuteCommand(strQuery, "DbContext");

                PatientAppointmentModel objPatientAppointmentHistoryModel = new PatientAppointmentModel();
                objPatientAppointmentHistoryModel = GetPatientAppointmentsByintAppointmentId(objPatientAppointmentModel.intId);
                objPatientAppointmentHistoryModel.intId = 0;
                strQuery = "call patientappointementshistory_C_SP (?,?,?,?,?,?,?);";
                MySqlParameter[] param = {
                        new MySqlParameter("@intId", objPatientAppointmentHistoryModel.intId),
                        new MySqlParameter("@intpatientAppointment", objPatientAppointmentModel.intId),
                        new MySqlParameter("@intAppointmentSlot", objPatientAppointmentHistoryModel.intAppointmentSlot),
                        new MySqlParameter("@intDoctorId", objPatientAppointmentHistoryModel.intDoctorId),
                        new MySqlParameter("@intPatientId", objPatientAppointmentHistoryModel.intPatientId),
                        new MySqlParameter("@dttCreationDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                        new MySqlParameter("@intAppointementStatus", objPatientAppointmentModel.intStatusId),
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
        public int CancelPatientAppointements(PatientAppointmentModel objPatientAppointmentModel)
        {
            DbContext dbContext = new DbContext();
            int result = -1;
            try
            {
                string[] arrAppointmentds = objPatientAppointmentModel.strAppointmentIds.Split(',');
                for (int i = 0; i < arrAppointmentds.Length; i++)
                {
                   // int intPatientAppointmentd = Convert.ToInt32(arrAppointmentds[i]);
                    string strQuery = "update patientappointements set intAppointementStatus=4 where intId = " + Convert.ToInt32(arrAppointmentds[i]) + ";";
                    result = dbContext.ExecuteCommand(strQuery, "DbContext");
                    PatientAppointmentModel objPatientAppointmentHistoryModel = new PatientAppointmentModel();
                    objPatientAppointmentHistoryModel = GetPatientAppointmentsByintAppointmentId(Convert.ToInt32(arrAppointmentds[i]));
                    objPatientAppointmentHistoryModel.intId = 0;
                    strQuery = "call patientappointementshistory_C_SP (?,?,?,?,?,?,?);";
                    MySqlParameter[] param = {
                        new MySqlParameter("@intId", objPatientAppointmentHistoryModel.intId),
                        new MySqlParameter("@intpatientAppointment", Convert.ToInt32(arrAppointmentds[i])),
                        new MySqlParameter("@intAppointmentSlot", objPatientAppointmentHistoryModel.intAppointmentSlot),
                        new MySqlParameter("@intDoctorId", objPatientAppointmentHistoryModel.intDoctorId),
                        new MySqlParameter("@intPatientId", objPatientAppointmentHistoryModel.intPatientId),
                        new MySqlParameter("@dttCreationDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                        new MySqlParameter("@intAppointementStatus", 4),
                    };
                   int resultHistory = dbContext.ExecuteCommandWithParameter(strQuery, param, true, "DbContext");
                }
                return result;
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }
        public int CompletePatientAppointements(PatientAppointmentModel objPatientAppointmentModel)
        {
            DbContext dbContext = new DbContext();
            int result = -1;
            int intPatienttreatmentId = 0;
            try
            {
                string[] arrAppointmentds = objPatientAppointmentModel.strAppointmentIds.Split(',');
                for (int i = 0; i < arrAppointmentds.Length; i++)
                {
                    string strQuery = "update patientappointements set intAppointementStatus=3 where intId = " + Convert.ToInt32(arrAppointmentds[i]) + ";";
                    result = dbContext.ExecuteCommand(strQuery, "DbContext");

                    strQuery = "call patenttreatment_CU_SP (?,?,?,?);";
                    MySqlParameter[] param = {
                new MySqlParameter("@intId", intPatienttreatmentId),
                new MySqlParameter("@intPatientAppointmentd", Convert.ToInt32(arrAppointmentds[i])),
                new MySqlParameter("@varTreatment", objPatientAppointmentModel.strTreatment),
                new MySqlParameter("@dttCreationDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
            };
                    result = dbContext.ExecuteCommandWithParameter(strQuery, param, true, "DbContext");
                    
                    PatientAppointmentModel objPatientAppointmentHistoryModel = new PatientAppointmentModel();
                    objPatientAppointmentHistoryModel = GetPatientAppointmentsByintAppointmentId(Convert.ToInt32(arrAppointmentds[i]));
                    objPatientAppointmentHistoryModel.intId = 0;
                    strQuery = "call patientappointementshistory_C_SP (?,?,?,?,?,?,?);";
                    MySqlParameter[] paramPatientHistory = {
                        new MySqlParameter("@intId", objPatientAppointmentHistoryModel.intId),
                        new MySqlParameter("@intpatientAppointment", Convert.ToInt32(arrAppointmentds[i])),
                        new MySqlParameter("@intAppointmentSlot", objPatientAppointmentHistoryModel.intAppointmentSlot),
                        new MySqlParameter("@intDoctorId", objPatientAppointmentHistoryModel.intDoctorId),
                        new MySqlParameter("@intPatientId", objPatientAppointmentHistoryModel.intPatientId),
                        new MySqlParameter("@dttCreationDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                        new MySqlParameter("@intAppointementStatus", 3),
                    };
                    int resultHistory = dbContext.ExecuteCommandWithParameter(strQuery, paramPatientHistory, true, "DbContext");
                }
                return result;
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }
        public List<AppointementsModel> GetAllAppointements()
        {
            DbContext dbContext = new DbContext();
            List<AppointementsModel> lstAppointementsModel = new List<AppointementsModel>();
            try
            {

                string strQuery = $"call patientappointements_R_SP(8,0,0,0,0,' ');";
                DataTable dt = dbContext.GetDataTable(strQuery, "DbContext");
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        AppointementsModel objAppointementsModel = new AppointementsModel();
                        objAppointementsModel.intDoctorId = Convert.ToInt32(dr["intDoctorId"]);
                        objAppointementsModel.intPatientId = Convert.ToInt32(dr["intPatientId"]);
                        objAppointementsModel.intGender = Convert.ToInt32(dr["intGender"]);
                        objAppointementsModel.intCountryId = Convert.ToInt32(dr["intCountryId"]);
                        objAppointementsModel.intStateId = Convert.ToInt32(dr["intStateId"]);
                        objAppointementsModel.intCreatedBy = Convert.ToInt32(dr["intCreatedBy"]);
                        objAppointementsModel.intActive = Convert.ToInt32(dr["intActive"]);
                        objAppointementsModel.intSpecialityId = Convert.ToInt32(dr["intSpecialityId"]);
                        objAppointementsModel.strSpecialityName = Convert.ToString(dr["varSpecialityName"]);
                        objAppointementsModel.strAppointementDate = Convert.ToString(dr["varAppointementDate"]);
                        objAppointementsModel.strDoctorFirstName = Convert.ToString(dr["varDoctorFirstName"]);
                        objAppointementsModel.strDoctorMiddleName = Convert.ToString(dr["varDoctorMiddleName"]);
                        objAppointementsModel.strDoctorLastName = Convert.ToString(dr["varDoctorLastName"]);
                        objAppointementsModel.strUserName = Convert.ToString(dr["varUserName"]);
                        objAppointementsModel.strStartTime = Convert.ToString(dr["varStartTime"]);
                        objAppointementsModel.strEndTime = Convert.ToString(dr["varEndTime"]);
                        objAppointementsModel.strMobileNo = Convert.ToString(dr["varMobileNo"]);
                        objAppointementsModel.strEmail = Convert.ToString(dr["varEmail"]);
                        objAppointementsModel.strDesignation = Convert.ToString(dr["varDesignation"]);
                        objAppointementsModel.strPostalCode = Convert.ToString(dr["varPostalCode"]);
                        objAppointementsModel.strAddress = Convert.ToString(dr["varAddress"]);
                        objAppointementsModel.strBiography = Convert.ToString(dr["varBiography"]);
                        objAppointementsModel.strEducation = Convert.ToString(dr["varEducation"]);
                        objAppointementsModel.strPatientFirstName = Convert.ToString(dr["varPatientFirstName"]);
                        objAppointementsModel.strPatientMiddleName = Convert.ToString(dr["varPatientMiddleName"]);
                        objAppointementsModel.strPatientLastName = Convert.ToString(dr["varPatientLastName"]);
                        objAppointementsModel.strStatusName = Convert.ToString(dr["varStatusName"]);
                        objAppointementsModel.dttCreationDate = Convert.ToDateTime(dr["dttCreationDate"]);
                        lstAppointementsModel.Add(objAppointementsModel);
                    }
                    return lstAppointementsModel;
                }

                else
                {
                    return lstAppointementsModel;
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }

        }
        public List<PatientAppointmentModel> GetPatientAppointmentsByintDoctorId(int intDoctorId, int intStatus)
        {
            DbContext dbContext = new DbContext();
            List<PatientAppointmentModel> lsPatientAppointmentModel = new List<PatientAppointmentModel>();
            try
            {

                string strQuery = $"call (2," + intDoctorId + ",0," + intStatus + ",0);";
                DataTable dt = dbContext.GetDataTable(strQuery, "DbContext");
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        PatientAppointmentModel objPatientAppointmentsModel = new PatientAppointmentModel();
                        objPatientAppointmentsModel.intId = Convert.ToInt32(dr["intAppointementId"]);
                        objPatientAppointmentsModel.intAppointmentSlot = Convert.ToInt32(dr["intAppointmentSlot"]);
                        objPatientAppointmentsModel.intDoctorId = Convert.ToInt32(dr["intDoctorId"]);
                        objPatientAppointmentsModel.intPatientId = Convert.ToInt32(dr["intPatientId"]);
                        objPatientAppointmentsModel.dttCreationDate = Convert.ToDateTime(dr["dttCreationDate"]);
                        objPatientAppointmentsModel.intAppointementStatus = Convert.ToInt32(dr["intAppointementStatus"]);
                        objPatientAppointmentsModel.strFirstName = Convert.ToString(dr["varDoctorName"]);
                        objPatientAppointmentsModel.strMiddleName = Convert.ToString(dr["varMiddleName"]);
                        objPatientAppointmentsModel.strLastName = Convert.ToString(dr["varLastName"]);
                        objPatientAppointmentsModel.strSpeciality = Convert.ToString(dr["varSpeciality"]);
                        objPatientAppointmentsModel.strStatusName = Convert.ToString(dr["varStatusName"]);
                        objPatientAppointmentsModel.strStartTime = Convert.ToString(dr["varStartTime"]);
                        objPatientAppointmentsModel.strEndTime = Convert.ToString(dr["varEndTime"]);
                        objPatientAppointmentsModel.strEmail = Convert.ToString(dr["varEmail"]);
                        objPatientAppointmentsModel.strMobileNo = Convert.ToString(dr["varMobileNo"]);
                        objPatientAppointmentsModel.intSlotId = Convert.ToInt32(dr["intSlotId"]);
                        objPatientAppointmentsModel.intStatusId = Convert.ToInt32(dr["intStatusId"]);

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
        public List<PatientAppointmentModel> GetPatientAppointmentsByintPatientId(int intPatientId, int intStatus)
        {
            DbContext dbContext = new DbContext();

            List<PatientAppointmentModel> lsPatientAppointmentModel = new List<PatientAppointmentModel>();
            try
            {

                string strQuery = $"call patientappointements_R_SP(10,0," + intPatientId + "," + intStatus + ",0,' ');";
                DataTable dt = dbContext.GetDataTable(strQuery, "DbContext");
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        PatientAppointmentModel objPatientAppointmentsModel = new PatientAppointmentModel();

                        objPatientAppointmentsModel.intId = Convert.ToInt32(dr["intAppointementId"]);
                        objPatientAppointmentsModel.intAppointmentSlot = Convert.ToInt32(dr["intAppointmentSlot"]);
                        objPatientAppointmentsModel.intDoctorId = Convert.ToInt32(dr["intDoctorId"]);
                        objPatientAppointmentsModel.intPatientId = Convert.ToInt32(dr["intPatientId"]);
                        objPatientAppointmentsModel.strSlotDate = Convert.ToDateTime(dr["dttDate"]).ToString("yyyy-MM-dd");
                        objPatientAppointmentsModel.intAppointementStatus = Convert.ToInt32(dr["intAppointementStatus"]);
                        objPatientAppointmentsModel.strFirstName = Convert.ToString(dr["varFirstName"]);
                        objPatientAppointmentsModel.strMiddleName = Convert.ToString(dr["varMiddleName"]);
                        objPatientAppointmentsModel.strLastName = Convert.ToString(dr["varLastName"]);
                        objPatientAppointmentsModel.strSpeciality = Convert.ToString(dr["SpecialityName"]);
                        objPatientAppointmentsModel.strStatusName = Convert.ToString(dr["varStatusName"]);
                        objPatientAppointmentsModel.strStartTime = Convert.ToString(dr["varStartTime"]);
                        objPatientAppointmentsModel.strEndTime = Convert.ToString(dr["varEndTime"]);
                        objPatientAppointmentsModel.strEmail = Convert.ToString(dr["varEmail"]);
                        objPatientAppointmentsModel.strMobileNo = Convert.ToString(dr["varMobileNo"]);
                        objPatientAppointmentsModel.intSlotId = Convert.ToInt32(dr["intSlotId"]);
                        objPatientAppointmentsModel.intStatusId = Convert.ToInt32(dr["intStatusId"]);
                        objPatientAppointmentsModel.strDocName = Convert.ToString(dr["varDoctorName"]);
                        objPatientAppointmentsModel.strDocEmail = Convert.ToString(dr["varDocEmail"]);
                        objPatientAppointmentsModel.strDocMobile = Convert.ToString(dr["varDocMobile"]);
                        objPatientAppointmentsModel.strTreatment = Convert.ToString(dr["varTreatment"]);
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

        public List<PatientAppointmentModel> GetPatientAppointmentsHistoryByintPatientAppointmentId(int intPatientAppointmentId)
        {
            DbContext dbContext = new DbContext();

            List<PatientAppointmentModel> lsPatientAppointmentModel = new List<PatientAppointmentModel>();
            try
            {

                string strQuery = $"call patientappointements_R_SP(11,0," + intPatientAppointmentId + ",0,0,' ');";
                DataTable dt = dbContext.GetDataTable(strQuery, "DbContext");
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        PatientAppointmentModel objPatientAppointmentsModel = new PatientAppointmentModel();

                        objPatientAppointmentsModel.intId = Convert.ToInt32(dr["intAppointementId"]);
                        objPatientAppointmentsModel.intAppointmentSlot = Convert.ToInt32(dr["intAppointmentSlot"]);
                        objPatientAppointmentsModel.intDoctorId = Convert.ToInt32(dr["intDoctorId"]);
                        objPatientAppointmentsModel.intPatientId = Convert.ToInt32(dr["intPatientId"]);
                        objPatientAppointmentsModel.strSlotDate = Convert.ToDateTime(dr["dttDate"]).ToString("yyyy-MM-dd");
                        objPatientAppointmentsModel.intAppointementStatus = Convert.ToInt32(dr["intAppointementStatus"]);
                        objPatientAppointmentsModel.strFirstName = Convert.ToString(dr["varFirstName"]);
                        objPatientAppointmentsModel.strMiddleName = Convert.ToString(dr["varMiddleName"]);
                        objPatientAppointmentsModel.strLastName = Convert.ToString(dr["varLastName"]);
                        //objPatientAppointmentsModel.strSpeciality = Convert.ToString(dr["varSpeciality"]);
                        objPatientAppointmentsModel.strStatusName = Convert.ToString(dr["varStatusName"]);
                        objPatientAppointmentsModel.strStartTime = Convert.ToString(dr["varStartTime"]);
                        objPatientAppointmentsModel.strEndTime = Convert.ToString(dr["varEndTime"]);
                        objPatientAppointmentsModel.strEmail = Convert.ToString(dr["varEmail"]);
                        objPatientAppointmentsModel.strMobileNo = Convert.ToString(dr["varMobileNo"]);
                        objPatientAppointmentsModel.intSlotId = Convert.ToInt32(dr["intSlotId"]);
                        objPatientAppointmentsModel.intStatusId = Convert.ToInt32(dr["intStatusId"]);
                        objPatientAppointmentsModel.strDocName = Convert.ToString(dr["varDoctorName"]);
                        objPatientAppointmentsModel.strDocEmail = Convert.ToString(dr["varDocEmail"]);
                        objPatientAppointmentsModel.strDocMobile = Convert.ToString(dr["varDocMobile"]);
                        objPatientAppointmentsModel.strTreatment = Convert.ToString(dr["varTreatment"]);
                        objPatientAppointmentsModel.strCreationDate = Convert.ToDateTime(dr["AppHisdttCreationDate"]).ToString("dd-MM-yyyy :HH:mm:ss");
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
        public List<PatientModel> GetPatientByDocIdSlotId(int intSlotId)
        {
            DbContext dbContext = new DbContext();
            List<PatientModel> lstobjPatient = new List<PatientModel>();
            try
            {
                string strQuery = $"call patient_R_SP (4," + intSlotId + ",0);";
                DataTable dt = dbContext.GetDataTable(strQuery, "DbContext");
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        PatientModel objPatientModel = new PatientModel();

                        objPatientModel.intId = Convert.ToInt32(dr["intId"]);
                        objPatientModel.strFirstName = Convert.ToString(dr["varFirstName"]);
                        objPatientModel.strMiddleName = Convert.ToString(dr["varMiddleName"]);
                        objPatientModel.strLastName = Convert.ToString(dr["varLastName"]);
                        objPatientModel.intGender = Convert.ToInt32(dr["intGender"]);
                        objPatientModel.strUserName = Convert.ToString(dr["varUserName"]);
                        objPatientModel.strPassword = Convert.ToString(dr["varPassword"]);
                        objPatientModel.strMobileNo = Convert.ToString(dr["varMobileNo"]);
                        objPatientModel.strEmail = Convert.ToString(dr["varEmail"]);
                        objPatientModel.intCountryId = Convert.ToInt32(dr["intCountryId"]);
                        objPatientModel.intStateId = Convert.ToInt32(dr["intStateId"]);
                        objPatientModel.intCityId = Convert.ToInt32(dr["intCityId"]);
                        objPatientModel.strCountryName = Convert.ToString(dr["varCountryName"]);
                        objPatientModel.strStateName = Convert.ToString(dr["varStateName"]);
                        objPatientModel.strCityName = Convert.ToString(dr["varCityName"]);
                        objPatientModel.intCreatedBy = Convert.ToInt32(dr["intCreatedBy"]);
                        objPatientModel.strImagePath = Convert.ToString(dr["varImagePath"]);
                        objPatientModel.intPatientAppointment = Convert.ToInt32(dr["intPatientAppointment"]);
                        objPatientModel.strCreationDate = Convert.ToDateTime(dr["dttCreationDate"]).ToString("dd-MM-yyyy");
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

        public List<PatientModel> GetAllPatientByDocIdSlotId(int intSlotId)
        {
            DbContext dbContext = new DbContext();
            List<PatientModel> lstobjPatient = new List<PatientModel>();
            try
            {
                string strQuery = $"call patient_R_SP (7," + intSlotId + ",0);";
                DataTable dt = dbContext.GetDataTable(strQuery, "DbContext");
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        PatientModel objPatientModel = new PatientModel();

                        objPatientModel.intId = Convert.ToInt32(dr["intId"]);
                        objPatientModel.strFirstName = Convert.ToString(dr["varFirstName"]);
                        objPatientModel.strMiddleName = Convert.ToString(dr["varMiddleName"]);
                        objPatientModel.strLastName = Convert.ToString(dr["varLastName"]);
                        objPatientModel.intGender = Convert.ToInt32(dr["intGender"]);
                        objPatientModel.strUserName = Convert.ToString(dr["varUserName"]);
                        objPatientModel.strPassword = Convert.ToString(dr["varPassword"]);
                        objPatientModel.strMobileNo = Convert.ToString(dr["varMobileNo"]);
                        objPatientModel.strEmail = Convert.ToString(dr["varEmail"]);
                        objPatientModel.intCountryId = Convert.ToInt32(dr["intCountryId"]);
                        objPatientModel.intStateId = Convert.ToInt32(dr["intStateId"]);
                        objPatientModel.intCityId = Convert.ToInt32(dr["intCityId"]);
                        objPatientModel.strCountryName = Convert.ToString(dr["varCountryName"]);
                        objPatientModel.strStateName = Convert.ToString(dr["varStateName"]);
                        objPatientModel.strCityName = Convert.ToString(dr["varCityName"]);
                        objPatientModel.intCreatedBy = Convert.ToInt32(dr["intCreatedBy"]);
                        objPatientModel.strImagePath = Convert.ToString(dr["varImagePath"]);
                        objPatientModel.intPatientAppointment = Convert.ToInt32(dr["intPatientAppointment"]);
                        objPatientModel.intAppointementStatus = Convert.ToInt32(dr["intAppointementStatus"]);
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
        #region
        public int InsertUpdatePatientHealthCheckUp(PatientModel objPatientModel)
        {
            DbContext dbContext = new DbContext();
            try
            {
                string strQuery = "call userbodymeasurement_CU_SP (?,?,?,?,?,?);";
                MySqlParameter[] param = {
                    new MySqlParameter("@intId", objPatientModel.intPatientHealthId),
                    new MySqlParameter("@intPatientId", objPatientModel.intId),
                    new MySqlParameter("@intMasBodyMeasurement", objPatientModel.intBodyMeasurement),
                    new MySqlParameter("@varMeasurement", objPatientModel.strBodyMeasurement),
                    new MySqlParameter("@intActive", 1),
                    new MySqlParameter("@intCreatedBy", objPatientModel.intCreatedBy),                    
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

        public PatientModel GetPatientHealthCheckUpByPatientHealthId(int intPatientHealthId)
        {
            DbContext dbContext = new DbContext();
            PatientModel objPatientModel = new PatientModel();
            try
            {
                string strQuery = $"call userbodymeasurement_R_SP (2," + intPatientHealthId + ");";
                DataTable dt = dbContext.GetDataTable(strQuery, "DbContext");
                if (dt.Rows.Count > 0)
                {       
                        objPatientModel.intPatientHealthId = Convert.ToInt32(dt.Rows[0]["intId"]);
                        objPatientModel.intBodyMeasurement = Convert.ToInt32(dt.Rows[0]["intMasBodyMeasurement"]);
                        objPatientModel.strMasBodyMeasurement = Convert.ToString(dt.Rows[0]["masbodymeasuremenName"]);
                        objPatientModel.strBodyMeasurement = Convert.ToString(dt.Rows[0]["varMeasurement"]);
                        objPatientModel.intActive = Convert.ToInt32(dt.Rows[0]["intActive"]);
                        objPatientModel.strCreatedBy = Convert.ToString(dt.Rows[0]["varFirstName"]) + " " + Convert.ToString(dt.Rows[0]["varMiddleName"]) + " " + Convert.ToString(dt.Rows[0]["varLastName"]);                       
                   
                    return objPatientModel;
                }
                else
                {
                    return objPatientModel;
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }
        public List<PatientModel> GetPatientHealthCheckUpByPatientId(int intPatientId)
        {
            DbContext dbContext = new DbContext();
            List<PatientModel> lstPatientModel = new List<PatientModel>();
            try
            {
                string strQuery = $"call userbodymeasurement_R_SP (1," + intPatientId + ");";
                DataTable dt = dbContext.GetDataTable(strQuery, "DbContext");
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        PatientModel objPatientModel = new PatientModel();
                        objPatientModel.intId = Convert.ToInt32(dr["intId"]);
                        objPatientModel.intBodyMeasurement = Convert.ToInt32(dr["intMasBodyMeasurement"]);
                        objPatientModel.strMasBodyMeasurement = Convert.ToString(dr["masbodymeasuremenName"]);
                        objPatientModel.strBodyMeasurement = Convert.ToString(dr["varMeasurement"]);                        
                        objPatientModel.intActive = Convert.ToInt32(dr["intActive"]);
                        objPatientModel.strCreationDate = Convert.ToDateTime(dr["dttCreationDate"]).ToString("dd-MM-yyyy HH:mm:ss");
                        objPatientModel.strCreatedBy = Convert.ToString(dr["varFirstName"]) + " " + Convert.ToString(dr["varMiddleName"]) + " " + Convert.ToString(dr["varLastName"]);
                        objPatientModel.strUnit = Convert.ToString(dr["unitName"]);
                        lstPatientModel.Add(objPatientModel);
                    }
                    return lstPatientModel;
                }
                else
                {
                    return lstPatientModel;
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                throw ex;
            }
        }
        public int DeletePatientHealthCheckByPatientId(int intBodyMeasurementId)
        {
            DbContext dbContext = new DbContext();
            try
            {
                string strSQL = $"update userbodymeasurement set intActive =0 where intId = " + intBodyMeasurementId + ";";
                return dbContext.ExecuteCommand(strSQL, "DbContext");
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



