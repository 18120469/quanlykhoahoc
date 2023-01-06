using Microsoft.AspNetCore.Mvc;
using Dapper;
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
    public class danhsachtraineeController : Controller
    {
        

        private readonly IConfiguration _Configuration;

        public danhsachtraineeController(IConfiguration Configuration)
        {
            _Configuration = Configuration;
        }
        [HttpGet("laydanhsachtrainee")]
        public IActionResult laydanhsachtrainee(string tukhoa, string traineeid)
        {
            List<trainee> listdata = new List<trainee>();
            string query = "tim_kiem_trainee";
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
                        ParameterName = "p_tukhoa",
                        Value = tukhoa
                    });
                    MyCommand.Parameters.Add(new NpgsqlParameter()
                    {
                        ParameterName = "p_traineeid",
                        Value = traineeid
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
                    listdata = MyConn.Query<trainee>(resultSetReferenceCommand, null, commandType: CommandType.Text, transaction: null).ToList();
                    mytrans.Commit();
                    MyConn.Close();
                }
            }
            //var jsonRE = JsonConvert.SerializeObject(listdata);
            return Ok(listdata);
        }
        //[HttpGet("laychitietkhoahoc")]
        //public IActionResult laychitietkhoahoc(int makhoahoc)
        //{
        //    var listdata = new khoahoc();
        //    string query = "lay_chi_tiet_khoahoc";
        //    string sqlcon = _Configuration.GetConnectionString("PosgresqlConection");
        //    using (NpgsqlConnection MyConn = new NpgsqlConnection(sqlcon))
        //    {
        //        MyConn.Open();
        //        var mytrans = MyConn.BeginTransaction();
        //        using (NpgsqlCommand MyCommand = new NpgsqlCommand(query, MyConn))
        //        {
        //            MyCommand.CommandType = CommandType.StoredProcedure;
        //            MyCommand.Parameters.Add(new NpgsqlParameter()
        //            {
        //                ParameterName = "p_makhoahoc",
        //                Value = makhoahoc
        //            });
        //            string resultSetReferenceCommand = string.Empty;
        //            using (var MyReader = MyCommand.ExecuteReader())
        //            {
        //                if (MyReader.Read())
        //                {
        //                    resultSetReferenceCommand = $"FETCH ALL IN \"{MyReader[0]}\";";

        //                }
        //            }
        //            listdata = MyConn.QueryFirstOrDefault<khoahoc>(resultSetReferenceCommand, null, commandType: CommandType.Text, transaction: null);
        //            mytrans.Commit();
        //            MyConn.Close();
        //        }
        //    }
        //    //var jsonRE = JsonConvert.SerializeObject(listdata);
        //    return Ok(listdata);
        //}
    }
}
