using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using TaskManagerCourse.Api.Models;
using TaskManagerCourse.Api.Models.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// ����������� ��
builder.Services.AddDbContext<ApplicationContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DbConnection"));


}

);



// ������������� � NEWGET ��������� ���������� JWTBearer 
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        // ��������, ����� �� �������������� �������� ��� ��������� ������
        ValidateIssuer = true,
        // ������, �������������� ��������
        ValidIssuer = AuthOptions.ISSUER,

        // ����� �� �������������� ����������� ������
        ValidateAudience = true,
        // ��������� ����������� ������
        ValidAudience = AuthOptions.AUDIENCE,
        // ����� �� �������������� ����� �������������
        ValidateLifetime = true,

        // ��������� ����� ������������
        IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
        // ��������� ����� ������������
        ValidateIssuerSigningKey = true,
    };
}
);
builder.Services.AddControllersWithViews(); 


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseRouting();
app.UseAuthentication();    
app.UseAuthorization();



app.MapControllers();

app.Run();

// �����
//public void ConfigureServices(IServiceCollection services)
//{
//    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//            .AddJwtBearer(options =>
//            {
//                options.RequireHttpsMetadata = false;
//                options.TokenValidationParameters = new TokenValidationParameters
//                {
//                    // ��������, ����� �� �������������� �������� ��� ��������� ������
//                    ValidateIssuer = true,
//                    // ������, �������������� ��������
//                    ValidIssuer = AuthOptions.ISSUER,

//                    // ����� �� �������������� ����������� ������
//                    ValidateAudience = true,
//                    // ��������� ����������� ������
//                    ValidAudience = AuthOptions.AUDIENCE,
//                    // ����� �� �������������� ����� �������������
//                    ValidateLifetime = true,

//                    // ��������� ����� ������������
//                    IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
//                    // ��������� ����� ������������
//                    ValidateIssuerSigningKey = true,
//                };
//            });
//    services.AddControllersWithViews();
//}