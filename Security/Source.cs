using HospitalManagement.Model;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using static HospitalManagement.Model.ResponseModels;

namespace HospitalManagement.Security
{
    public class Source
    {
        public static int CheckAndValidateSource(int source)
        {
            try
            {
                string value = "";
                string xmlFile = File.ReadAllText(@"Repository/XML/710ff5c9-e814-40e9-a733-14dc3d8cb01d.xml");
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.LoadXml(xmlFile);          
                if (source == 1)
                {
                    XmlNodeList nodeList = xmldoc.GetElementsByTagName("web");
                    value = nodeList[0].InnerText;
                }
                else if (source == 2)
                {
                    XmlNodeList nodeList = xmldoc.GetElementsByTagName("android");
                    value = nodeList[0].InnerText;
                }
                else if (source == 3)
                {
                    XmlNodeList nodeList = xmldoc.GetElementsByTagName("ios");
                    value = nodeList[0].InnerText;
                }
                return Convert.ToInt32(value);
            }
            catch (Exception ex) { return 0; }
        }
        public static ResponseModels CheckSourceAndReturnResponse(int sourceValue)
        {
            ResponseModels oReturn = new ResponseModels();
            try
            {
                if (sourceValue == 0)
                {
                    oReturn.intStatusCode = (int)E_RESPONSESTATUS.ValidationFails;
                    oReturn.strMessage = "Source Not Validated";                    
                }
                else if (sourceValue == 2)
                {
                    oReturn.intStatusCode = (int)E_RESPONSESTATUS.MaintenanceMode;
                    oReturn.strMessage = "Maintenance Mode";                    
                }
                else if (sourceValue == 3)
                {
                    oReturn.intStatusCode = (int)E_RESPONSESTATUS.ForceLogout;
                    oReturn.strMessage = "Please Login Again";                    
                }
                return oReturn;
            }
            catch (Exception ex) {
                oReturn.intStatusCode = (int)E_RESPONSESTATUS.ExceptionOccurs;
                oReturn.strMessage = "Some error occured. Please contact to Administrator";
                return oReturn;
            }
        }
    }
}
