using HospitalManagement.DbConnect;
using HospitalManagement.Repository.Interface;
using HospitalManagement_Models.Users;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalManagement.Repository.DataClass.Master
{
    public class clsSystemUser
    {
        public class clsSystemUsers : ISystemUser
        {
            #region User
            public UserModel UserLogin(UserModel Users)
            {
                DbContext dbContext = new DbContext();
                UserModel usertoken = new UserModel();
                try
                {
                    string strQuery = $"call UserLogin_R_SP('" + Users.strLoginName + "', '" + Users.strLoginPassword + "')";

                    DataTable dt = dbContext.GetDataTable(strQuery, "DbContext");
                    if (dt.Rows.Count > 0)
                    {
                        usertoken.intId = Convert.ToInt32(dt.Rows[0]["intId"]);
                        usertoken.strLoginName = Convert.ToString(dt.Rows[0]["varLoginName"]);
                        // usertoken.strName = Convert.ToString(dt.Rows[0]["varName"]);
                        usertoken.intActive = Convert.ToInt32(dt.Rows[0]["intActive"]);
                        usertoken.intUserType = Convert.ToInt32(dt.Rows[0]["intUserType"]);
                        return usertoken;
                    }
                    else
                        return null;
                }
                catch (Exception ex)
                {
                    ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                    return null;
                }
            }
            #endregion

            #region System User
            public SystemUsersModel SystemUserLogin(SystemUsersModel objsystemUsersModel)
            {
                DbContext dbContext = new DbContext();
                SystemUsersModel usertoken = new SystemUsersModel();
                try
                {
                   // string strQuery = "call system_UserLogin('" + objsystemUsersModel.strEmail + "','" + objsystemUsersModel.strPassword + "','" + objsystemUsersModel.strFirebaseDeviceId + "'," + objsystemUsersModel.intDeviceTypeId + ",'" + objsystemUsersModel.strDeviceIMEI + "');";
                    string strQuery = "select * from systemusers where varEmail = '" + objsystemUsersModel.strEmail + "' and varPassword = '" + objsystemUsersModel.strPassword + "';";
                    DataTable dt = dbContext.GetDataTable(strQuery, "DbContext");
                    if (dt.Rows.Count > 0)
                    {
                        usertoken.intId = Convert.ToInt32(dt.Rows[0]["intId"]);
                        usertoken.strEmail = Convert.ToString(dt.Rows[0]["varEmail"]);                        
                        usertoken.strUserName = Convert.ToString(dt.Rows[0]["varUserName"]);                        
                        usertoken.intUserType = 1;
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
            public int InsertUpdateSystemUsers(SystemUsersModel objsystemUsersModel)
            {
                DbContext dbContext = new DbContext();
                try
                {
                    string strQuery1 = @"Select varMobileNo from systemusers where varMobileNo = '" + objsystemUsersModel.strMobileNo + "' and intId!=" + objsystemUsersModel.intId + ";";
                    string retval = dbContext.ExecuteScalerString(strQuery1, "DbContext");
                    if (!string.IsNullOrEmpty(retval))
                    {
                        return -1;
                    }

                    string strQuery2 = @"Select varEmailAddres from systemusers where varEmailAddres = '" + objsystemUsersModel.strEmail + "' and intId!=" + objsystemUsersModel.intId + ";";
                    string retval1 = dbContext.ExecuteScalerString(strQuery2, "DbContext");
                    if (!string.IsNullOrEmpty(retval1))
                    {
                        return -1;
                    }

                    string strQuery = "call Systemusers_CU_SP(?,?,?,?,?,?,?,?,?,?,?,?);";
                    MySqlParameter[] param = {
                    new MySqlParameter("@intId", objsystemUsersModel.intId),
                    new MySqlParameter("@varFirstName", objsystemUsersModel.strFirstName),
                    new MySqlParameter("@varMiddleName", objsystemUsersModel.strMiddleName),
                    new MySqlParameter("@varLastName", objsystemUsersModel.strLastName),
                    new MySqlParameter("@intGender", objsystemUsersModel.intGender),
                    new MySqlParameter("@varUserName", objsystemUsersModel.strUserName),
                    new MySqlParameter("@varPassword", objsystemUsersModel.strPassword),
                    new MySqlParameter("@varMobileNo", objsystemUsersModel.strMobileNo),
                    new MySqlParameter("@varEmail",objsystemUsersModel.strEmail),
                    new MySqlParameter("@intCountryId",objsystemUsersModel.intCountryId),
                    new MySqlParameter("@intStateId",objsystemUsersModel.intStateId),
                    new MySqlParameter("@intCityId",objsystemUsersModel.intCityId),

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

            public List<SystemUsersModel> GetSystemUsers()
            {
                DbContext dbContext = new DbContext();
                List<SystemUsersModel> lstSystemUser = new List<SystemUsersModel>();
                try
                {
                    string strQuery = $"call Systemusers_R_SP(1,0);";
                    DataTable dt = dbContext.GetDataTable(strQuery, "DbContext");
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            SystemUsersModel objSystemUsers = new SystemUsersModel();
                            objSystemUsers.intId = Convert.ToInt32(dr["intId"]);
                            objSystemUsers.strFirstName = Convert.ToString(dr["varFirstName"]);
                            objSystemUsers.strMiddleName = Convert.ToString(dr["varMiddleName"]);
                            objSystemUsers.strLastName = Convert.ToString(dr["varLastName"]);
                            objSystemUsers.intGender = Convert.ToInt32(dr["intGender"]);
                            objSystemUsers.strUserName = Convert.ToString(dr["varUserName"]);
                            objSystemUsers.strPassword = Convert.ToString(dr["varPassword"]);
                            objSystemUsers.strMobileNo = Convert.ToString(dr["varMobileNo"]);
                            objSystemUsers.strEmail = Convert.ToString(dr["varEmail"]);
                            objSystemUsers.intCountryId = Convert.ToInt32(dr["intCountryId"]);
                            objSystemUsers.intStateId = Convert.ToInt32(dr["intStateId"]);
                            objSystemUsers.intCityId = Convert.ToInt32(dr["intCityId"]);

                            lstSystemUser.Add(objSystemUsers);
                        }
                        return lstSystemUser;
                    }
                    else
                    {
                        return lstSystemUser;
                    }
                }
                catch (Exception ex)
                {
                    ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                    throw ex;
                }
            }

            public SystemUsersModel GetSystemUsersById(int intId)
            {
                DbContext dbContext = new DbContext();
                try
                {
                    SystemUsersModel objsystemUsersModel = new SystemUsersModel();
                    string strQuery = $"call Systemusers_R_SP(2," + intId + ");";
                    DataTable dt = dbContext.GetDataTable(strQuery, "DbContext");
                    if (dt.Rows.Count > 0)
                    {
                        objsystemUsersModel.intId = Convert.ToInt32(dt.Rows[0]["intId"]);
                        objsystemUsersModel.strFirstName = Convert.ToString(dt.Rows[0]["varFirstName"]);
                        objsystemUsersModel.strMiddleName = Convert.ToString(dt.Rows[0]["varMiddleName"]);
                        objsystemUsersModel.strLastName = Convert.ToString(dt.Rows[0]["varLastName"]);
                        objsystemUsersModel.intGender = Convert.ToInt32(dt.Rows[0]["intGender"]);
                        objsystemUsersModel.strUserName = Convert.ToString(dt.Rows[0]["varUserName"]);
                        objsystemUsersModel.strPassword = Convert.ToString(dt.Rows[0]["varPassword"]);
                        objsystemUsersModel.strMobileNo = Convert.ToString(dt.Rows[0]["varMobileNo"]);
                        objsystemUsersModel.strEmail = Convert.ToString(dt.Rows[0]["varEmail"]);
                        objsystemUsersModel.intCountryId = Convert.ToInt32(dt.Rows[0]["intCountryId"]);
                        objsystemUsersModel.intStateId = Convert.ToInt32(dt.Rows[0]["intStateId"]);
                        objsystemUsersModel.intCityId = Convert.ToInt32(dt.Rows[0]["intCityId"]);

                    }
                    return objsystemUsersModel;
                }
                catch (Exception ex)
                {
                    ExceptionLog.SaveExceptionToText(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "", "");
                    throw ex;
                }
            }
        }
        #endregion
    }
}
