using MiddleWare.Data;
using MiddleWare.Models.DataServerModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text.Json;
using Newtonsoft.Json;
using Nancy.Json;
using MiddleWare.Models.OptionsModels;
using MiddleWare.OPCUALayer;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Extensions.Options;
using Microsoft.Data.SqlClient;
using ClosedXML.Excel;

namespace MiddleWare.Controllers
{
    public class TableController : BaseApiController
    {
        private readonly IConfiguration _configuration;
        private readonly OPCUAServers[] _uaServers;
        private readonly IUaClientSingleton _uaClient;
        public TableController(IOptions<OPCUAServersOptions> servers, IUaClientSingleton UAClient, IConfiguration configuration)
        {
            this._uaServers = servers.Value.Servers;
            for (int i = 0; i < _uaServers.Length; i++) _uaServers[i].Id = i;
            this._uaClient = UAClient;
            this._configuration = configuration;
        }
        [HttpGet]
        [Route("productdata/top10000")]
        public async Task <IActionResult> GetMaterialTank()
        {
            string queryProduct = @"
                SELECT TOP (1000) * FROM [dbo].[Products]";
            string queryCustomer = @"
                SELECT TOP (1000) * FROM [dbo].[Customers]";
            DataTable tableProduct = new DataTable();
            DataTable tableCustomer = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("MiddlewareDbConnection");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(queryProduct, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    tableProduct.Load(myReader); ;

                    myReader.Close();
                    myCon.Close();
                }
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(queryCustomer, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    tableCustomer.Load(myReader); ;

                    myReader.Close();
                    myCon.Close();
                }
            }
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(tableProduct, "Product");
                wb.Worksheets.Add(tableCustomer, "Customer");
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return Ok(File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Report.xlsx"));
                }
            }
            //return new JsonResult(table);
        }
    }
}
