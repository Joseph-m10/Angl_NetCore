using PMODashboard.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace PMODashboard.Business
{
    public class BAL
    {
        public static DataSet GetTemplateData(int tempId)
        {
            return Data.DAL.GetTemplateData(tempId);
        }
        public static DataSet GetChartdata(int type)
        {
            return Data.DAL.GetChartdata(type);
        }
        public static DataSet GetTabledata(int type)
        {
            return Data.DAL.GetTabledata(type);
        }
        public static string UploadDataFeedTemplate(UploadFeed uploadfeed, string flag)
        {
            return Data.DAL.UploadDataFeedTemplate(uploadfeed, flag);
        }
    }
}
