using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace PMODashboard.Controllers
{
    using PMODashboard.Controllers;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Newtonsoft.Json;
    using PMODashboard.Business;
    using PMODashboard.Model;
    using System.IO;
    using System.Net.Http.Headers;
    using Microsoft.Extensions.Logging;
    using Serilog.Core;

    [Produces("application/json")]
    [Route("api/PMO")]
    public class PMOController : Controller
    {
        public static string NewConnectionString { get; private set; }
        private IWebHostEnvironment _hostingEnvironment;
        private Logger _logger;
        private IConfiguration Configuration;

        public PMOController(IConfiguration _configuration, IWebHostEnvironment hostingEnvironment)
        {
            Configuration = _configuration;
            _hostingEnvironment = hostingEnvironment;
            _logger = Startup.loggerFactory;
            NewConnectionString = this.Configuration.GetConnectionString("customDB");
        }


        [HttpGet]
        [Route("GetTemplateData")]
        public string GetTemplateData(int tempId)
        {

            try
            {
                var ds = Business.BAL.GetTemplateData(tempId);
                return JsonConvert.SerializeObject(ds);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpGet]
        [Route("GetTabledata")]
        public DataSet GetTabledata(int type)
        {
            try
            {
                DataSet ds = Business.BAL.GetTabledata(type);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [HttpGet]
        [Route("GetChartdata")]
        public DataSet GetChartdata(int type)
        {
            try
            {
                DataSet ds = Business.BAL.GetChartdata(type);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private void DeleteExcelFile(string filename)
        {
            if (System.IO.File.Exists(filename))
            {
                System.IO.File.Delete(filename);
            }
        }

        [HttpPost]
        [Route("UploadDataFeedTemplate")]
        public string UploadDataFeedTemplate(string Flag)
        {
            string parameter = "";
            string fileName = "";
            UploadFeed paramList;
            var file = Request.Form.Files[0];
            paramList = new UploadFeed();
            paramList.ReportingPeriod = Convert.ToString(Request.Form["date"]);
            var fullPath = "";
            try
            {
                string folderName = "Import";
                string webRootPath = _hostingEnvironment.WebRootPath;
                string newPath = Path.Combine(webRootPath, folderName);
                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }

                if (file.Length > 0)
                {
                    fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).Name.Trim('"');
                    parameter = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    if (fileName.IndexOfAny(Path.GetInvalidFileNameChars()) > -1)
                    {

                        throw new FileNotFoundException("FileName not valid");

                    }

                    fullPath = Path.Combine(newPath, fileName);

                    if (!fullPath.StartsWith(newPath))

                    {

                        throw new FileNotFoundException("Path not valid");

                    }

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {

                        file.CopyTo(stream);
                    }
                    FileInfo fileInfo = new FileInfo(fullPath);
                    paramList.Feed = ExcelUtilities.ExcelToDataTable(fileInfo);
                    DeleteExcelFile(fullPath);
                    var res = Business.BAL.UploadDataFeedTemplate(paramList, Flag);
                    //return res;
                }

                return "Success";
            }
            catch (System.Exception ex)
            {
                _logger.Error("UploadDataFeedTemplate method in Upload Controller : ", ex.Message);
                return "Failure";
            }
        }

    }
}
