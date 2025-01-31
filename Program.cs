using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

IConfiguration configuration = builder.Configuration;

/* 
 * JWT ��� ���� ����
 * 
 * ASP.NET Core ���ø����̼ǿ��� JWT(JSON Web Token) ������ �����ϴ� ������ �մϴ�. 
 * �׸��� TokenValidationParameters �Ӽ��� ���� 
 * ���޵Ǵ� ������ JWT ��ū�� ��ȿ���� �����ϴ� �� ���Ǵ� �߿��� �������� ��� �ֽ��ϴ�.
 * 
 */
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    // ����� ��ū�� ���� �Ʒ����� ������ �׸�鿡 ���� ��ȿ�� ������ �����մϴ�.
    // ���� ���� �� HttpContext.User�� ����� ������ �����ǰ�, ���� �ڵ忡�� User.Identity.IsAuthenticated�� ���� ����� ���� ���θ� Ȯ���� �� �ֽ��ϴ�.
    // ���� ���� �� ������ �����ϸ� ���� ���� ó���� �����մϴ�.
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true, // ��ū�� �߱��� ����(issuer)�� �ùٸ��� Ȯ���մϴ�. ��, ������ ValidIssuer ���� ��ġ�ϴ��� ���մϴ�.
        ValidateAudience = true, // ��ū�� �߱޵� ���(audience)�� �ùٸ��� Ȯ���մϴ�. ��, ������ ValidAudience ���� ��ġ�ϴ��� ���մϴ�.
        ValidateLifetime = true, // ��ū�� ��ȿ �Ⱓ�� �˻��մϴ�. �߱޵� �ð��� ���� �ð��� ���Ͽ� ���� �ð��� ��ȿ �Ⱓ ���� �ִ��� Ȯ���մϴ�.
        ValidateIssuerSigningKey = true, // ��ū ������ ��ȿ���� �˻��մϴ�. ��, ������ IssuerSigningKey�� ����Ͽ� ��ū ������ �����մϴ�.

        // �Ʒ� 3����
        // ���� ��ū �߱� ����, ��ū ��� ���, ��ū ���� ���� ���Ű�� ��Ÿ���� ���Դϴ�. �� ������ JWT ��ū ���� �ÿ��� ���Ǿ� ��ū�� ���Ե˴ϴ�.
        ValidIssuer = configuration["Jwt:Issuer"], // https://localhost:7123
        ValidAudience = configuration["Jwt:Audience"], // https://localhost:7123
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]))
    };

    // 2025.01.31 �߰�
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            // ��Ű���� ��ū�� �����ɴϴ�.
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
//    options.Cookie.SameSite = SameSiteMode.Strict; // SameSite �Ӽ� ����
//    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // HTTPS������ ��Ű ����

//})
;

builder.Services.AddControllersWithViews();
builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddRazorPages();



var app = builder.Build();

// ��û ó�� ���������� ����
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
app.UseAuthentication(); // ù��° ȣ��, ������ ���� �̷���� �Ŀ�
app.UseAuthorization(); // �ι�° ȣ��, ���� �ο��� ����Ǿ�� ��
app.MapControllers();

app.MapRazorPages();
app.UseEndpoints(endpoints => // 4 �߰�
{
	endpoints.MapControllers();
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();
