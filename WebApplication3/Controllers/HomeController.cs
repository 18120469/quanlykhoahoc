using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Npgsql;
using System.Data;
using System.Data.Common;
using System.Transactions;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : Controller
    {
        private readonly IConfiguration _Configuration;

        public HomeController(IConfiguration Configuration)
        {
            _Configuration = Configuration;
        }
        [HttpGet(Name = "test")]
        public IActionResult Login(string mataikhoan)
        {
            var listdata = JsonConvert.DeserializeObject<taikhoan>(mataikhoan);        
            string query = "dang_nhap";
            string sqlcon = _Configuration.GetConnectionString("PosgresqlConection");
            using (NpgsqlConnection MyConn = new NpgsqlConnection(sqlcon))
            {
                MyConn.Open();
                var mytrans = MyConn.BeginTransaction();
                using (NpgsqlCommand MyCommand = new NpgsqlCommand(query, MyConn))
                {
                    MyCommand.CommandType = CommandType.StoredProcedure;
                    MyCommand.Parameters.Add(new NpgsqlParameter()
                    {
                        ParameterName = "p_taikhoan",
                        Value = mataikhoan
                    });
                    string resultSetReferenceCommand = string.Empty;
                    using (var MyReader = MyCommand.ExecuteReader())
                    {
                        if (MyReader.Read())
                        {
                            resultSetReferenceCommand = $"FETCH ALL IN \"{MyReader[0]}\";";
                            //resultSetReferenceCommand = $"FETCH ALL IN \" + MyReader[0]";

                        }
                    }
                    listdata = MyConn.Query<taikhoan>(resultSetReferenceCommand, null, commandType: CommandType.Text, transaction: null).Single();
                    mytrans.Commit();
                    MyConn.Close();
                }
            }
            //var jsonRE = JsonConvert.SerializeObject(listdata);
            return Ok(listdata);
        }

    }
}
