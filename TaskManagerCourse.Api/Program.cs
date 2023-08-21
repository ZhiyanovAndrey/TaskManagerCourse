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

// регистрация БД
builder.Services.AddDbContext<ApplicationContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DbConnection"));


}

);



// дополнительно к NEWGET установим расширение JWTBearer 
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        // укзывает, будет ли валидироваться издатель при валидации токена
        ValidateIssuer = true,
        // строка, представляющая издателя
        ValidIssuer = AuthOptions.ISSUER,

        // будет ли валидироваться потребитель токена
        ValidateAudience = true,
        // установка потребителя токена
        ValidAudience = AuthOptions.AUDIENCE,
        // будет ли валидироваться время существования
        ValidateLifetime = true,

        // установка ключа безопасности
        IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
        // валидация ключа безопасности
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

// ТОКЕН
//public void ConfigureServices(IServiceCollection services)
//{
//    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//            .AddJwtBearer(options =>
//            {
//                options.RequireHttpsMetadata = false;
//                options.TokenValidationParameters = new TokenValidationParameters
//                {
//                    // укзывает, будет ли валидироваться издатель при валидации токена
//                    ValidateIssuer = true,
//                    // строка, представляющая издателя
//                    ValidIssuer = AuthOptions.ISSUER,

//                    // будет ли валидироваться потребитель токена
//                    ValidateAudience = true,
//                    // установка потребителя токена
//                    ValidAudience = AuthOptions.AUDIENCE,
//                    // будет ли валидироваться время существования
//                    ValidateLifetime = true,

//                    // установка ключа безопасности
//                    IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
//                    // валидация ключа безопасности
//                    ValidateIssuerSigningKey = true,
//                };
//            });
//    services.AddControllersWithViews();
//}