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
    public class danhsachlophocController : Controller
    {
        private readonly IConfiguration _Configuration;

        public danhsachlophocController(IConfiguration Configuration)
        {
            _Configuration = Configuration;
        }
        [HttpGet("laydanhsachlophoc")]
        public IActionResult laydanhsachlophoc(string tukhoa,int mavitri,int makynang,string mataikhoan)
        {
            List<khoahoc> listdata = new List<khoahoc>();
            string query = "tim_kiem_khoa_hoc";
            string sqlcon = _Configuration.GetConnectionString("PosgresqlConection");
            using(NpgsqlConnection MyConn = new NpgsqlConnection(sqlcon))
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
                        ParameterName = "p_mavitri",
                        Value = mavitri
                    });
                    MyCommand.Parameters.Add(new NpgsqlParameter()
                    {
                        ParameterName = "p_makynang",
                        Value = makynang
                    });
                    MyCommand.Parameters.Add(new NpgsqlParameter()
                    {
                        ParameterName = "p_mataikhoan",
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
                    listdata = MyConn.Query<khoahoc>(resultSetReferenceCommand, null, commandType: CommandType.Text, transaction: null).ToList();                  
                    mytrans.Commit();
                    MyConn.Close();
                }    
            }
            //var jsonRE = JsonConvert.SerializeObject(listdata);
            return Ok(listdata);      
        }
        [HttpGet("laychitietkhoahoc")]
        public IActionResult laychitietkhoahoc(int makhoahoc,string mataikhoan)
        {
            var listdata = new khoahoc();
            string query = "lay_chi_tiet_khoahoc";
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
                        ParameterName = "p_makhoahoc",
                        Value = makhoahoc
                    });
                    MyCommand.Parameters.Add(new NpgsqlParameter()
                    {
                        ParameterName = "p_mataikhoan",
                        Value = mataikhoan
                    });
                    string resultSetReferenceCommand = string.Empty;
                    using (var MyReader = MyCommand.ExecuteReader())
                    {
                        if (MyReader.Read())
                        {
                            resultSetReferenceCommand = $"FETCH ALL IN \"{MyReader[0]}\";";

                        }
                    }
                    listdata = MyConn.QueryFirstOrDefault<khoahoc>(resultSetReferenceCommand, null, commandType: CommandType.Text, transaction: null);
                    mytrans.Commit();
                    MyConn.Close();
                }
            }
            //var jsonRE = JsonConvert.SerializeObject(listdata);
            return Ok(listdata);
        }
        [HttpGet("dangkykhoahoc")]
        public IActionResult dangkykhoahoc(int makhoahoc,string mataikhoan)
        {
            string query = "dangky_khoahoc";
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
                        ParameterName = "p_makhoahoc",
                        Value = makhoahoc
                    });
                    MyCommand.Parameters.Add(new NpgsqlParameter()
                    {
                        ParameterName = "p_mataikhoan",
                        Value = mataikhoan
                    });
                    string resultSetReferenceCommand = string.Empty;
                    using (var MyReader = MyCommand.ExecuteReader())
                    {
                    }
                    mytrans.Commit();
                    MyConn.Close();
                }
            }
            //var jsonRE = JsonConvert.SerializeObject(listdata);
            return Ok("Thành Công");
        }
        [HttpGet("laydanhsachcomment")]
        public IActionResult laydanhsachcomment(int makhoahoc)
        {
            List<danhsach_comment> listdata = new List<danhsach_comment>();
            string query = "lay_comment_khoahoc";
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
                        ParameterName = "p_makhoahoc",
                        Value = makhoahoc
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
                    listdata = MyConn.Query<danhsach_comment>(resultSetReferenceCommand, null, commandType: CommandType.Text, transaction: null).ToList();
                    mytrans.Commit();
                    MyConn.Close();
                }
            }
            //var jsonRE = JsonConvert.SerializeObject(listdata);
            return Ok(listdata);
        }
        [HttpGet("themcomment")]
        public IActionResult themcomment(int makhoahoc, string mataikhoan, string comment)
        {
            string query = "them_comment_khoahoc";
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
                        ParameterName = "p_makhoahoc",
                        Value = makhoahoc
                    });
                    MyCommand.Parameters.Add(new NpgsqlParameter()
                    {
                        ParameterName = "p_mataikhoan",
                        Value = mataikhoan
                    });
                    MyCommand.Parameters.Add(new NpgsqlParameter()
                    {
                        ParameterName = "p_comment",
                        Value = comment
                    });
                    string resultSetReferenceCommand = string.Empty;
                    using (var MyReader = MyCommand.ExecuteReader())
                    {
                    }
                    mytrans.Commit();
                    MyConn.Close();
                }
            }
            //var jsonRE = JsonConvert.SerializeObject(listdata);
            return Ok("Thành Công");
        }
    }

}

