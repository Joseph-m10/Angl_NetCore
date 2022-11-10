using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace PMODashboard.Model
{
    public class CommonClass
    {
    }
    public class UploadFeed
    {
        public DataTable Feed { get; set; }
        public string ReportingPeriod { get; set; }
        public string AssociateID { get; set; }
    }
}
