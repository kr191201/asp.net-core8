using System.Data;
using System.Data.Common;
using BoardStudy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace BoardStudy.Controllers
{
    public class JoinController : Controller
    {
        private readonly IConfiguration _configuration; // 추가
        private readonly SqlConnection _connection;

        public JoinController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connection = new SqlConnection(_configuration.GetConnectionString("MSSQL"));
        }

        /*
         * 회원가입화면
         */
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        /*
         * 회원가입처리
         */
        [HttpPost]
        public async Task<int> Index(TblUserList tblUserList)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string connectionString = _configuration.GetConnectionString("MSSQL");

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        await connection.OpenAsync();

                        using (SqlCommand command = new SqlCommand("USP_INSERTUSER", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@USERID", tblUserList.USERID);
                            command.Parameters.AddWithValue("@USERNAME", tblUserList.USERNAME);
                            command.Parameters.AddWithValue("@USEREMAIL", tblUserList.USEREMAIL);
                            command.Parameters.AddWithValue("@USERPASSWORD", tblUserList.USERPASSWORD);

                            try
                            {
                                int rowsAffected = await command.ExecuteNonQueryAsync();
                                return rowsAffected;
                            }
                            catch (SqlException ex)
                            {
                                return -1;
                            }

                        }
                    }
                }
                else
                {
                    return -2;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        


    }
}
