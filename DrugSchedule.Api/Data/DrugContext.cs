using Microsoft.EntityFrameworkCore;

namespace DrugSchedule.Api.Data;

public class DrugContext : DbContext
{
    public DrugContext()
    {
    }

    public DrugContext(DbContextOptions<DrugContext> options) : base(options)
    {
    }

    public DbSet<Country> Countries { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<EventStatus> EventStatuses { get; set; }
    public DbSet<EventType> EventTypes { get; set; }
    public DbSet<EventReport> EventReport { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Institution> Institutions { get; set; }
    public DbSet<RoadMap> RoadMaps { get; set; }
    public DbSet<RoadMapStatus> RoadMapStatuses { get; set; }
    public DbSet<Schedule> Schedules { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Log> Logs { get; set; }


}