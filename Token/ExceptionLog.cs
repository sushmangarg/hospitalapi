using HospitalManagement.DbConnect;
using MySql.Data.MySqlClient;
using System;
using System.Diagnostics;
using System.Web;
namespace HospitalManagement
{
    public class ExceptionLog
    {
        private static string ErrorlineNo, Errormsg, extype, exurl, hostIp, ErrorLocation, HostAdd;
        protected static readonly string TableNameErrorLog = "errorlog";

        public static int SaveExceptionToTextByAPI(string strExecption, string strMethodName, string strBlockName, string strPlateform, string strToken, string strVersion, int intErrorType)
        {            
            try
            {
                var strQuery = string.Empty;
                DbContext dbContext = new DbContext();                
                MySqlParameter[] param = null;                
                strQuery = "INSERT INTO " + TableNameErrorLog + " (varMethodName,varBlockName,varPlatform,varLog,varToken,varVersion, dttDate) VALUES (?,?,?,?,?,?,?,?)";
                param = new MySqlParameter[] {
                    new MySqlParameter("varMethodName", strMethodName),
                    new MySqlParameter("varBlockName", strBlockName),
                    new MySqlParameter("varPlatform", strPlateform),
                    new MySqlParameter("varLog", strExecption),
                    new MySqlParameter("varToken", strToken),
                    new MySqlParameter("varVersion", strVersion),
                    new MySqlParameter("date", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                    new MySqlParameter("intErrorType", intErrorType),
                };

                 return dbContext.ExecuteCommandWithParameter(strQuery, param, false, "DbContext");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void SaveExceptionToText(Exception ex,string url,string methodName, string blockName, string plateform)
        {
            var line = Environment.NewLine + Environment.NewLine;
            var stackTrace = new StackTrace(ex, true);
            var frame = stackTrace.GetFrame(0);
            var lineNo = frame.GetFileLineNumber();

            ErrorlineNo = ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7) + ". Line-No:" + lineNo + ". ";
            Errormsg = ex.GetType().Name.ToString();
            extype = ex.GetType().ToString();
            Microsoft.AspNetCore.Http.HttpContext context = null;
            exurl = url;//Convert.ToString(context.Request.Path);
            ErrorLocation = ex.Message.ToString();

            try
            {
                var strQuery = string.Empty;
                DbContext dbContext = new DbContext();
                // Dim filepath As String = HttpContext.Current.Server.MapPath("~/ExceptionLogs/")

                MySqlParameter[] param = null;

                string error = line + "Error Line No :" + " " + ErrorlineNo + line + ", Error Message:" + " " + Errormsg + line + ", Exception Type:" + " " + extype + line + ", Error Location :" + " " + ErrorLocation + line;

                strQuery = "INSERT INTO " + TableNameErrorLog + " (varMethodName,varBlockName,varPlatform,varLog, dttDate) VALUES (?,?,?,?,?)";
                param = new MySqlParameter[] {                    
                    new MySqlParameter("varMethodName", methodName),
                    new MySqlParameter("varBlockName", blockName),
                    new MySqlParameter("varPlatform", plateform),
                    new MySqlParameter("Log", error),
                    new MySqlParameter("date", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                };

                dbContext.ExecuteCommandWithParameter(strQuery, param, false, "DbContext");
            }
            catch (Exception e)
            {
                e.ToString();
            }
        }

        public static void SaveExceptionToText(string ex)
        {
            var line = Environment.NewLine + Environment.NewLine;
            try
            {
                var strQuery = string.Empty;
                DbContext dbContext = new DbContext();
                // Dim filepath As String = HttpContext.Current.Server.MapPath("~/ExceptionLogs/")

                MySqlParameter[] param = null;

                string error = line + ex;

                strQuery = "INSERT INTO " + TableNameErrorLog + " (varLog, dttDate) VALUES (?,?)";
                param = new MySqlParameter[] {
            new MySqlParameter("Log", error),
            new MySqlParameter("date", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
        };

                dbContext.ExecuteCommandWithParameter(strQuery, param, false, "DbContext");
            }
            catch (Exception e)
            {
                e.ToString();
            }
        }
    }
}