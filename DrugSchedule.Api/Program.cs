using DrugSchedule.SqlServer.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();




builder.Services.AddDbContext<IdentityContext>(o =>
{
    o.UseSqlServer(builder.Configuration.GetConnectionString("DrugSchedule"), x =>
        x.MigrationsAssembly(nameof(DrugSchedule.SqlServer)));
    o.EnableSensitiveDataLogging();
});

builder.Services.AddDbContext<DrugScheduleContext>(o =>
{
    o.UseSqlServer(builder.Configuration.GetConnectionString("DrugSchedule"), x =>
        x.MigrationsAssembly(nameof(DrugSchedule.SqlServer)));
    o.EnableSensitiveDataLogging();
});


builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.MapControllers();

app.Run();