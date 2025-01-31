using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

IConfiguration configuration = builder.Configuration;

/* 
 * JWT 기반 인증 설정
 * 
 * ASP.NET Core 애플리케이션에서 JWT(JSON Web Token) 인증을 설정하는 역할을 합니다. 
 * 그리고 TokenValidationParameters 속성을 통해 
 * 전달되는 값들은 JWT 토큰의 유효성을 검증하는 데 사용되는 중요한 정보들을 담고 있습니다.
 * 
 */
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    // 추출된 토큰에 대해 아래에서 설정된 항목들에 따라 유효성 검증을 수행합니다.
    // 검증 성공 시 HttpContext.User에 사용자 정보가 설정되고, 이후 코드에서 User.Identity.IsAuthenticated를 통해 사용자 인증 여부를 확인할 수 있습니다.
    // 검증 실패 시 검증에 실패하면 인증 실패 처리를 수행합니다.
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true, // 토큰을 발급한 서버(issuer)가 올바른지 확인합니다. 즉, 설정된 ValidIssuer 값과 일치하는지 비교합니다.
        ValidateAudience = true, // 토큰이 발급된 대상(audience)이 올바른지 확인합니다. 즉, 설정된 ValidAudience 값과 일치하는지 비교합니다.
        ValidateLifetime = true, // 토큰의 유효 기간을 검사합니다. 발급된 시간과 만료 시간을 비교하여 현재 시간이 유효 기간 내에 있는지 확인합니다.
        ValidateIssuerSigningKey = true, // 토큰 서명의 유효성을 검사합니다. 즉, 설정된 IssuerSigningKey를 사용하여 토큰 서명을 검증합니다.

        // 아래 3가지
        // 각각 토큰 발급 서버, 토큰 사용 대상, 토큰 서명에 사용된 비밀키를 나타내는 값입니다. 이 값들은 JWT 토큰 생성 시에도 사용되어 토큰에 포함됩니다.
        ValidIssuer = configuration["Jwt:Issuer"], // https://localhost:7123
        ValidAudience = configuration["Jwt:Audience"], // https://localhost:7123
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]))
    };

    // 2025.01.31 추가
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            // 쿠키에서 토큰을 가져옵니다.
            var token = context.HttpContext.Request.Cookies["jwt"];
            if (!string.IsNullOrEmpty(token))
            {
                context.Token = token;
            }
            return Task.CompletedTask;
        }
    };
})

//.AddCookie(options =>
//{
//    options.Cookie.SameSite = SameSiteMode.Strict; // SameSite 속성 설정
//    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // HTTPS에서만 쿠키 전송

//})
;

builder.Services.AddControllersWithViews();
builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddRazorPages();



var app = builder.Build();

// 요청 처리 파이프라인 설정
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
} else
{
	app.UseDeveloperExceptionPage();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication(); // 첫번째 호출, 인증이 먼저 이루어진 후에
app.UseAuthorization(); // 두번째 호출, 권한 부여가 진행되어야 함
app.MapControllers();

app.MapRazorPages();
app.UseEndpoints(endpoints => // 4 추가
{
	endpoints.MapControllers();
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();
