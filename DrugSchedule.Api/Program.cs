using System.Text;
using DrugSchedule.Api.FileAccessProvider;
using DrugSchedule.Api.Middlwares;
using DrugSchedule.BusinessLogic.Options;
using DrugSchedule.BusinessLogic.Services;
using DrugSchedule.BusinessLogic.Services.Abstractions;
using DrugSchedule.Storage;
using DrugSchedule.Storage.Data;
using DrugSchedule.Storage.Services;
using DrugSchedule.StorageContract.Abstractions;
using Mapster;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using TokenService = DrugSchedule.Api.Jwt.TokenService;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.local.json", true);

#region Services

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMapster();
builder.Services.AddScoped<IFileAccessService, FileAccessService>();
builder.Services.AddScoped<CurrentUserMiddleware>();

builder.Services.AddScoped<IFileInfoRepository, FileInfoRepository>();
builder.Services.AddScoped<IFileStorage, FileStorageService>();
builder.Services.AddScoped<IIdentityRepository, IdentityRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<IUserProfileRepository, UserProfileRepository>();
builder.Services.AddScoped<IReadonlyDrugRepository, DrugRepository>();

builder.Services.AddScoped<ICurrentUserIdentifier, CurrentUserIdentifier>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IIdentityService, UserService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserContactsService, UserService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IFileUrlProvider, FileUrlProvider>();
builder.Services.AddScoped<IDownloadableFileConverter, DownloadableFileConverter>();
builder.Services.AddScoped<IDrugLibraryService, DrugLibraryService>();


#endregion

#region Options

builder.Services.AddOptions<JwtOptions>()
    .BindConfiguration(JwtOptions.Title)
    .ValidateDataAnnotations()
    .ValidateOnStart();
builder.Services.AddOptions<FileStorageOptions>()
    .BindConfiguration(FileStorageOptions.Title)
    .ValidateDataAnnotations()
    .ValidateOnStart();
builder.Services.AddOptions<PrivateFileAccessOptions>()
    .BindConfiguration(PrivateFileAccessOptions.Title)
    .ValidateDataAnnotations()
    .ValidateOnStart();

#endregion


builder.Services.AddDbContext<DrugScheduleContext>(o =>
{
    o.UseSqlServer(builder.Configuration.GetConnectionString("DrugSchedule"));
    o.EnableSensitiveDataLogging();
});

#region Auth

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
    {
        options.Password.RequireDigit = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
    })
    .AddEntityFrameworkStores<DrugScheduleContext>();
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        var jwtOptions = builder.Configuration
            .GetSection(JwtOptions.Title)
            .Get<JwtOptions>();
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.Zero,
            ValidAudience = jwtOptions!.ValidAudience,
            ValidIssuer = jwtOptions.ValidIssuer,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret))
        };
    });

#endregion

#region Swagger

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n Enter 'Bearer' [space] and then your token in the text input below.\r\nExample: \"Bearer 1safsfsdfdfd\"",
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});


#endregion


var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<CurrentUserMiddleware>();
app.MapControllers();
app.Run();