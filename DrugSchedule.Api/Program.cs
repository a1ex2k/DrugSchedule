using DrugSchedule.SqlServer.Data;
using DrugSchedule.SqlServer.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContextPool<DrugScheduleContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DrugSchedule"));

#if DEBUG
    options.EnableSensitiveDataLogging();
    options.EnableDetailedErrors();
#endif
});

builder.Services.AddScoped<IDrugScheduleService, DrugScheduleRepository>();
builder.Services.AddScoped<IDrugScheduleService, DrugScheduleRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IFileStorageService, LocalFileSystemStorageService>();

builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.MapControllers();

app.Run();