using System.Text;
using DrugSchedule.BusinessLogic.Options;
using DrugSchedule.BusinessLogic.Services;
using DrugSchedule.SqlServer.Data;
using DrugSchedule.SqlServer.Services;
using DrugSchedule.StorageContract.Abstractions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
//builder.Services.AddEndpointsApiExplorer();


//builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
//builder.Services.AddScoped<IUserProfileRepository, UserProfileRepository>();
//builder.Services.AddScoped<IIdentityRepository, IdentityRepository>();

//builder.Services.AddScoped<ITokenService, TokenService>();
//builder.Services.AddScoped<IUserService, UserService>();



//builder.Services.Configure<JwtOptions>(
//    builder.Configuration.GetSection(JwtOptions.Jwt));


//builder.Services.AddDbContext<IdentityContext>(o =>
//{
//    o.UseSqlServer(builder.Configuration.GetConnectionString("DrugSchedule"), x =>
//        x.MigrationsAssembly(nameof(DrugSchedule.SqlServer)));
//    o.EnableSensitiveDataLogging();
//});

//builder.Services.AddDbContext<DrugScheduleContext>(o =>
//{
//    o.UseSqlServer(builder.Configuration.GetConnectionString("DrugSchedule"), x =>
//        x.MigrationsAssembly(nameof(DrugSchedule.SqlServer)));
//    o.EnableSensitiveDataLogging();
//});

//builder.Services.AddIdentity<IdentityUser, IdentityRole>(o =>
//    {
//        o.User.RequireUniqueEmail = true;
//    })
//    .AddEntityFrameworkStores<IdentityContext>();


//builder.Services.AddAuthentication(options =>
//    {
//        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
//    })

//    .AddJwtBearer(options =>
//    {
//        options.SaveToken = true;
//        options.RequireHttpsMetadata = false;

//        var jwtOptions = builder.Configuration
//            .GetSection(JwtOptions.Jwt)
//            .Get<JwtOptions>();

//        options.TokenValidationParameters = new TokenValidationParameters()
//        {
//            ValidateIssuer = true,
//            ValidateAudience = true,
//            ValidateLifetime = true,
//            ValidateIssuerSigningKey = true,
//            ClockSkew = TimeSpan.Zero,
//            ValidAudience = jwtOptions!.ValidAudience,
//            ValidIssuer = jwtOptions.ValidIssuer,
//            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret))
//        };
//    });


builder.Services.AddSwaggerGen(options =>
{
    //options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    //{
    //    Name = "Authorization",
    //    Type = SecuritySchemeType.ApiKey,
    //    Scheme = "Bearer",
    //    BearerFormat = "JWT",
    //    In = ParameterLocation.Header,
    //    Description = "JWT Authorization header using the Bearer scheme. \r\n Enter 'Bearer' [space] and then your token in the text input below.\r\nExample: \"Bearer 1safsfsdfdfd\"",
    //});
    //options.AddSecurityRequirement(new OpenApiSecurityRequirement
    //{
    //    {
    //        new OpenApiSecurityScheme
    //        {
    //            Reference = new OpenApiReference
    //            {
    //                Type = ReferenceType.SecurityScheme,
    //                Id = "Bearer"
    //            }
    //        },
    //        new string[] {}
    //    }
    //});
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseAuthentication();
//app.UseAuthorization();
app.MapControllers();

app.Run();