using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalManagement
{
    public class TokenClaims
    {
        public string sUserType { get; set; }
        public DateTime sExpiryTime { get; set; }
        public string sUserName { get; set; }
        public string sEmail { get; set; }
        public int iUniversityId { get; set; }
        public int iLocationId { get; set; }
        //<summary>
        //This property indicates the status output if token is valid or invalid.
        //</summary>
        //<returns></returns>
        public int iStatus { get; set; }
        public int UserId { get; set; }
        public int iRoleId { get; set; }
        public string LocationName { get; set; }
        public int NewsUserId { get; set; }
        public int FinanceUserId { get; set; }
        public string EnrollmentNo { get; set; }
        public int ApplicationNo { get; set; }
    }
}