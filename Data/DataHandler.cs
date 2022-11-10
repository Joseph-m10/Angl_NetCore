using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Serilog.Core;
namespace CXODashboard.Data
{

    public class DataHandler
    {
        private static Logger _logger;
        /// <summary>
        /// Executes the data set.
        /// </summary>
        /// <param name="Command"></param>
        /// <returns></returns>
        public static DataSet ExecuteDataSet(SqlCommand Command, string connectionString)
        {
            return ExecuteDataSetWithConnection(Command, connectionString);
        }
        public static byte[] ExecuteReder(SqlCommand Command, string connectionString,string columnName)
        {
            byte[] documentData = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    if (Command != null)
                    {
                        conn.Open();
                        Command.Connection = conn;
                        SqlDataReader reader = Command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                documentData = (byte[])reader[columnName];
                            }
                        }
                        
                        conn.Close();
                    }
                }
                
            }
            catch (SqlException sqlex)
            {
                _logger.Error("method is ExecuteDataSetWithConnection - error:" + sqlex.Message);
                // ClsLogging.DWBLogger("GetActions: " + sqlex.Message + " Stack: " + sqlex.StackTrace);
            }

            return documentData;

        }
        /// <summary>
        /// ExcuteDataSetWithConnection
        /// </summary>
        /// <param name="Command"></param>
        /// <param name="connection"></param>
        /// using (SqlConnection conn = new SqlConnection(@"Data Source = CTSC00890273401; Initial Catalog = Gannet; User Id = c2scrumlogin; password = c2scrum@123;"))

        /// <returns></returns>
        private static DataSet ExecuteDataSetWithConnection(SqlCommand Command, string connection)
        {
            DataSet ds = new DataSet();
            //try
            //{
                using (SqlConnection conn = new SqlConnection(connection))
                {
                    if (Command != null)
                    {
                        conn.Open();
                        Command.Connection = conn;
                        SqlDataAdapter da = new SqlDataAdapter(Command);
                        da.Fill(ds);
                        conn.Close();
                    }
                }
            //}
            //catch (SqlException sqlex)
            //{
             //   _logger.Error("method is ExecuteDataSetWithConnection - error:" + sqlex.Message);
                // ClsLogging.DWBLogger("GetActions: " + sqlex.Message + " Stack: " + sqlex.StackTrace);
            //}
            return ds;
        }

        /// <summary>
        /// Execute command and return string
        /// </summary>
        /// <param name="Command"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static string ExecuteCommand(SqlCommand Command, string connectionString)
        {
            string result = "";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    if (Command != null)
                    {
                        conn.Open();
                        Command.Connection = conn;
                        Command.CommandTimeout = 0;
                        Command.ExecuteNonQuery();
                        conn.Close();
                        result = "success";
                    }

                }
            }
            catch (SqlException sqlex)
            {
                _logger.Error("method is ExecuteCommand - error:" + sqlex.Message);
                result = "failure";
            }
            return result;
        }

        public static object ExecuteScalar(SqlCommand Command, string connectionString)
        {
            object result = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    if (Command != null)
                    {
                        conn.Open();
                        Command.Connection = conn;
                        Command.CommandTimeout = 0;
                        result = Command.ExecuteScalar();
                        conn.Close();
                    }
                }
            }
            catch (SqlException sqlex)
            {
                _logger.Error("method is ExecuteScalar - error:" + sqlex.Message);
                
            }
            return result;
        }
    }
}
