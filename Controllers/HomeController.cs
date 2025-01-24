using System.Data;
using System.Diagnostics;
using System.IO.Pipelines;
using BoardStudy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Reflection;

namespace BoardStudy.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        /*
        * 홈화면
        */
        //[Authorize]
        [HttpGet]
        public IActionResult Index2()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(); // 뷰를 반환
        }
    }
}
