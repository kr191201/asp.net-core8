using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BoardStudy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using System.Reflection;

namespace BoardStudy.Controllers
{
    public class LoginController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly SqlConnection _connection;

		public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connection = new SqlConnection(_configuration.GetConnectionString("MSSQL"));
        }

        /*
        * 로그인 화면
        */
        [HttpGet]
		public IActionResult Index()
        {
            return View();
        }

        /*
         * 로그인처리
         */
        [HttpPost]
		public IActionResult Proc([FromBody] LoginModel model)
        {
            try
            {
                _connection.Open();

                var parameters = new DynamicParameters();
                parameters.Add("@USEREMAIL", model.USEREMAIL, DbType.String, ParameterDirection.Input);
                parameters.Add("@USERPASSWORD", model.USERPASSWORD, DbType.String, ParameterDirection.Input);
                parameters.Add("@Result", dbType: DbType.Int32, direction: ParameterDirection.Output);

                _connection.Execute("USP_LOGIN", parameters, commandType: CommandType.StoredProcedure); // 프로시저 실행

				int result = parameters.Get<int>("@Result"); // OUTPUT 매개변수 값 읽기

				if (result == 1)
                {
                    var token = GenerateToken(model);

                    // JWT를 쿠키에 저장
                    var cookieOptions = new CookieOptions
                    {
                        HttpOnly = true,
                        Expires = DateTimeOffset.UtcNow.AddHours(1),
                        IsEssential = true // 필수 쿠키로 설정
                    };
                    Response.Cookies.Append("jwt", token, cookieOptions);

                    return Ok(new { Token = token });
                }
                else
                {
					return Unauthorized(new { Error = "Invalid email or password." }); // 인증 실패 시
				}
            }
            catch (Exception ex)
            {
                // 예외 처리 (로그 기록, 사용자에게 에러 메시지 등)
                return StatusCode(500, "An error occurred while processing your request.");
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                {
                    _connection.Close();
                }
            }
        }

        /*
         * JWT 토큰 생성SecretKey
         */
        public string GenerateToken(LoginModel model)
		{
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"])); // 설정된 암호화 키
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512); // 암호화 방식

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, model.USEREMAIL),
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(30)),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpGet]
        public IActionResult Logout()
        {
            return Ok(new { Message = "Logout successful. Please delete the JWT token from your client." });
        }

        //[Authorize]
        [HttpGet]
        public IActionResult GetUserInfo()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;

                var userName = User.Identity.Name;

                _connection.Open();
                var parameters = new DynamicParameters();

                parameters.Add("@SEL_USER_EMAIL", userEmail);

                var result = _connection.Query<TblUserList>("USP_USER_INFO", parameters, commandType: CommandType.StoredProcedure).ToList();

                var redirectUrl = "/home/index";

                return Ok(new { IsAuthenticated = true, UserName = userName, UserEmail = userEmail, redirectUrl = redirectUrl }); // 인증 상태와 사용자 정보를 함께 반환
            }
            else
            {
                return Ok(new { IsAuthenticated = false }); // 인증되지 않은 경우에도 JSON 반환
            }
        }
    }
}
