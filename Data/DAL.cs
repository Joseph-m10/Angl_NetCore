using CXODashboard.Data;
using PMODashboard.Controllers;
using PMODashboard.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PMODashboard.Data
{
    public class DAL
    {
        public static DataSet GetTemplateData(int tempId)
        {
            try
            {
                using (var typecommand = new SqlCommand())
                {
                    DataSet typeResult = new DataSet();
                    typecommand.CommandText = "[getTemplate]";
                    typecommand.CommandType = CommandType.StoredProcedure;
                    typecommand.Parameters.AddWithValue("@TemplateID", tempId);
                    typeResult = DataHandler.ExecuteDataSet(typecommand, PMOController.NewConnectionString);
                    return typeResult;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataSet GetChartdata(int type)
        {
            try
            {
                using (var typecommand = new SqlCommand())
                {
                    DataSet typeResult = new DataSet();
                    typecommand.CommandText = "[SP_GetChartdata]";
                    typecommand.CommandType = CommandType.StoredProcedure;
                    typecommand.Parameters.AddWithValue("@type", type);
                    typeResult = DataHandler.ExecuteDataSet(typecommand, PMOController.NewConnectionString);
                    return typeResult;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataSet GetTabledata(int type)
        {
            try
            {
                using (var typecommand = new SqlCommand())
                {
                    DataSet typeResult = new DataSet();
                    typecommand.CommandText = "[SP_GetTabledata]";
                    typecommand.CommandType = CommandType.StoredProcedure;
                    typecommand.Parameters.AddWithValue("@type", type);
                    typeResult = DataHandler.ExecuteDataSet(typecommand, PMOController.NewConnectionString);
                    return typeResult;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string UploadDataFeedTemplate(UploadFeed uploadfeed, string flag)
        {
            using (var typecommand = new SqlCommand())
            {
                //switch (flag)
                //{
                //    case "1":
                //        typecommand.CommandText = "[dbo].[UploadDataFeed]";
                //        break;
                //    case "2":
                //        typecommand.CommandText = "[dbo].[MetricCalculationWRTRawData]";
                //        break;
                //    default:
                //        typecommand.CommandText = flag;
                //        break;
                //}
                typecommand.CommandText = "[dbo].[SP_Input]";
                //typecommand.CommandText = "[dbo].[UploadDataFeed]";
                typecommand.CommandType = CommandType.StoredProcedure;
                typecommand.Parameters.AddWithValue("@Feeddata", uploadfeed.Feed);
                typecommand.Parameters.AddWithValue("@ReportingPeriod", uploadfeed.ReportingPeriod);
                typecommand.Parameters.AddWithValue("@Associateid", uploadfeed.AssociateID);
                return DataHandler.ExecuteCommand(typecommand, PMOController.NewConnectionString);
            }

        }

    }
}
