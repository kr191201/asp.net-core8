using BoardStudy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Dapper;
using Microsoft.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace BoardStudy.Controllers
{
	//[Authorize]
	public class BoardController : Controller
    {

        private readonly IConfiguration _configuration;
        private readonly SqlConnection _connection;
		private readonly ILogger<BoardController> _logger;

		public BoardController(IConfiguration configuration, ILogger<BoardController> logger)
        {
            _configuration = configuration;
            _connection = new SqlConnection(_configuration.GetConnectionString("MSSQL"));
			_logger = logger;
		}
		
		//[Authorize]
		[HttpGet]
		public IActionResult Index(LoginModel loginModel)
		{

            var username = User.Identity.Name; // JWT에서 사용자 이름 가져오기
                                               //return View();
            return Ok(new { Message = "test2" });
        }

        //[Authorize]
		[HttpGet]
        public IActionResult Index2()
        {
            return View();
		}

		/*
         * 게시판 작성 화면
		 */
		//[HttpPost]
		//public async Task<IActionResult> Write()
		//{
		//	TblUserList? user = HttpContext.Session.Get<TblUserList>("LoginUser");
		//	return View();
		//}

    }
}
