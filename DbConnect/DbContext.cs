using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using Microsoft.Extensions.Configuration;
using System.IO;
using HospitalManagement_Models.Master;

namespace HospitalManagement.DbConnect
{
    public class DbContext
    {
        string _conStr = null;
        public DbContext()
        {
            var configuration = GetConfiguration();
            _conStr = configuration.GetSection("ConnectionStrings").GetSection("DbContext").Value;                       
        }
        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
        public int ExecuteScaler(string strQuery, string strCon)
        {
            int oReturn = 0;
            try
            {
                if (!string.IsNullOrEmpty(strCon))
                {
                    var configuration = GetConfiguration();
                    _conStr = configuration.GetSection("ConnectionStrings").GetSection("DbContext").Value;
                }                    

                using (MySqlConnection con = new MySqlConnection(_conStr))
                {
                    con.Open();
                    using (MySqlCommand cmd = new MySqlCommand(strQuery, con))
                    {
                        oReturn = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
            return oReturn;
        }
        public string ExecuteScalerString(string strQuery, string strCon)
        {
            string oReturn = "";
            try
            {
                if (!string.IsNullOrEmpty(strCon))
                {
                    var configuration = GetConfiguration();
                    _conStr = configuration.GetSection("ConnectionStrings").GetSection("DbContext").Value;
                }

                using (MySqlConnection con = new MySqlConnection(_conStr))
                {
                    con.Open();
                    using (MySqlCommand cmd = new MySqlCommand(strQuery, con))
                    {
                        oReturn = Convert.ToString(cmd.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                return "";
            }

            return oReturn;
        }
        public int ExecuteCommand(string strQuery, string strCon)
        {
            int oReturn = 0;
            try
            {
                if (!string.IsNullOrEmpty(strCon))
                {
                    var configuration = GetConfiguration();
                    _conStr = configuration.GetSection("ConnectionStrings").GetSection("DbContext").Value;
                }

                using (MySqlConnection con = new MySqlConnection(_conStr))
                {
                    con.Open();
                    using (MySqlCommand cmd = new MySqlCommand(strQuery, con))
                    {
                        oReturn = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                return 0;
            }

            return oReturn;
        }        
        public int ExecuteCommandWithParameter(string strQuery, MySqlParameter[] param, bool IsReturnLastInsertedId, string strCon)
        {
            int oReturn = 0;            
            try
            {
                if (!string.IsNullOrEmpty(strCon))
                {
                    var configuration = GetConfiguration();
                    _conStr = configuration.GetSection("ConnectionStrings").GetSection("DbContext").Value;
                }
                using (MySqlConnection con = new MySqlConnection(_conStr))
                {
                    con.Open();
                    using (MySqlCommand cmd = new MySqlCommand(strQuery, con))
                    {
                        if(param != null)
                        {
                            foreach (var item in param)
                            {
                                cmd.Parameters.AddWithValue(item.ParameterName, item.Value);
                            }
                        }
                        if (IsReturnLastInsertedId)
                        {
                            oReturn = Convert.ToInt32(cmd.ExecuteScalar());                            
                        }
                        else
                        {
                            oReturn = cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oReturn;
        }
        public DataTable GetDataTable(string strQuery, string strCon)
        {
            DataTable oReturn = new DataTable();
            try
            {
                if (!string.IsNullOrEmpty(strCon))
                {
                    var configuration = GetConfiguration();
                    _conStr = configuration.GetSection("ConnectionStrings").GetSection("DbContext").Value;
                }

                using (MySqlConnection con = new MySqlConnection(_conStr))
                {
                    con.Open();
                    using (MySqlDataAdapter da = new MySqlDataAdapter(strQuery, con))
                    {
                        da.Fill(oReturn);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return oReturn;
        }
        public DataSet GetDataSet(string strQuery, string strCon)
        {
            DataSet oReturn = new DataSet();
            try
            {
                if (!string.IsNullOrEmpty(strCon))
                {
                    var configuration = GetConfiguration();
                    _conStr = configuration.GetSection("ConnectionStrings").GetSection("DbContext").Value;
                }

                using (MySqlConnection con = new MySqlConnection(_conStr))
                {
                    con.Open();
                    using (MySqlDataAdapter da = new MySqlDataAdapter(strQuery, con))
                    {
                        da.Fill(oReturn);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return oReturn;
        }
    }
}