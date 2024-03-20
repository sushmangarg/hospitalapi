using HospitalManagement_Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalManagement.Services
{
    public class clsFirebaseServices
    {
        public void SendNotificationToPatient(string deviceID, string title, string bodyMessage, int Patientid, string data)
        {
            string senderId = "481290015041";
            try
            {
                var client = new RestClient("https://fcm.googleapis.com/fcm/send");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Authorization", "key=AAAAcA8e0UE:APA91bGMReQE3lCFPQWxxMuyFVOWPaSRwrjNzDf9mdtfBN7eSV3t7oM8VY5Ci3fS5k1E7h2TIcgqgqddjQNRH_GVCTs_BI1cEk6N1qKlk1Ei4Wp8Nv_SwykVdeeIdDOSF8SkMIeGkCNN");
                request.AddHeader("Content-Type", "application/json");
                var body = @"{
                    " + "\n" +
                    @"    ""to"": """ + deviceID + @""",
                    " + "\n" +
                    @"    ""notification"": {
                    " + "\n" +
                    @"        ""body"": """ + bodyMessage + @""",
                    " + "\n" +
                    @"        ""title"": """ + title + @"""
                    " + "\n" +
                    @" },
                    " + "\n" +
                    @" ""data"" : 
                    " + "\n" +
                    @"     " + data + @"
                    " + "\n" +
                //@"     ""intStudentSubjectId"" : ""Value for key_2""
                //" + "\n" +
                @"}";


                request.AddParameter("application/json", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                AppNotificationModel nModel = new AppNotificationModel();
                nModel.strTitle = "Hospital Management";
                nModel.strMessage = bodyMessage;
                nModel.intMessageForId = Patientid;
                nModel.strMessageFor = "HM";
                nModel.strFirebaseDeviceId = deviceID;
                nModel.strSenderId = senderId;
                nModel.strResult = Convert.ToString(response.Content);

                int res = 0;
                //C_Management obj = new C_Management();
                //obj.InsertNotification(nModel, out res);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public void SendNotificationToDoctor(string deviceID, string title, string bodyMessage, int Patientid, string data)
        {
            string senderId = "602792271719";
            try
            {
                var client = new RestClient("https://fcm.googleapis.com/fcm/send");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Authorization", "key=AAAAjFk4H2c:APA91bEU6q5iCcc_nw5GbZZQbM5k8MNfvqTZsIm86dC6tqAb7gGs1sOIIVw1iqzzm4bhzz_AFGqGsxutZYBA_WPbWa0b28J_1UeLmnjxThnttadB8d_KVxR7vhm8ioJJmvMh45sIMURL");
                request.AddHeader("Content-Type", "application/json");
                var body = @"{
                    " + "\n" +
                    @"    ""to"": """ + deviceID + @""",
                    " + "\n" +
                    @"    ""notification"": {
                    " + "\n" +
                    @"        ""body"": """ + bodyMessage + @""",
                    " + "\n" +
                    @"        ""title"": """ + title + @"""
                    " + "\n" +
                    @" },
                    " + "\n" +
                    @" ""data"" : 
                    " + "\n" +
                    @"     " + data + @"
                    " + "\n" +
                //@"     ""intStudentSubjectId"" : ""Value for key_2""
                //" + "\n" +
                @"}";


                request.AddParameter("application/json", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                AppNotificationModel nModel = new AppNotificationModel();
                nModel.strTitle = "Hospital Management";
                nModel.strMessage = bodyMessage;
                nModel.intMessageForId = Patientid;
                nModel.strMessageFor = "HM";
                nModel.strFirebaseDeviceId = deviceID;
                nModel.strSenderId = senderId;
                nModel.strResult = Convert.ToString(response.Content);

                int res = 0;
                //C_Management obj = new C_Management();
                //obj.InsertNotification(nModel, out res);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
