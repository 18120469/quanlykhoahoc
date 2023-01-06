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
    public class danhsachtrainerController : Controller
    {
        private readonly IConfiguration _Configuration;

        public danhsachtrainerController(IConfiguration Configuration)
        {
            _Configuration = Configuration;
        }
        [HttpGet("laydanhsachtrainer")]
        public IActionResult laydanhsachtrainer(string tukhoa, string trainerid)
        {
            List<trainer> listdata = new List<trainer>();
            string query = "tim_kiem_trainer";
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
                        ParameterName = "p_trainerid",
                        Value = trainerid
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
                    listdata = MyConn.Query<trainer>(resultSetReferenceCommand, null, commandType: CommandType.Text, transaction: null).ToList();
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
