using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalManagement.Model
{
    public class ResponseModels
    {
        public int intStatusCode { get; set; }
        public string strMessage { get; set; }
        public object objData { get; set; }
        public string strAccess_Token { get; set; }
    }
    public static class clsSecretKey
    {
        public const string SecretKey = "HOSPITALMANAGEMENT";
    }


    public enum E_RESPONSESTATUS
        {
            Success = 200,
            ValidationFails = 102,
            NoDataFound = 103,
            ExceptionOccurs = 104,
            FileNotFound = 105,
            DuplicateRecord = 106,
            InvalidReceiptNo = 107,
            InvalidVoucherNo = 108,
            InvalidUser = 109,
            ConnectionNotFound = 110,
            Unauthorized = 111,
            InActive = 112,
            Created = 201,
            Updated = 202,
            BadRequest = 400,
            InternalServerError = 500,
            MaintenanceMode = 501,
            MultiLocationLogin = 502,
            ForceLogout = 503
        }
        public enum E_PLATEFORM
        {
            Web = 1,
            Android = 2,
            IOS = 3
        }
        public enum E_SOURCEERRORCODE
        {
            s_Ok = 1,
            MaintenanceMode = 2,
            ForceLogout = 3
        }        
    }

