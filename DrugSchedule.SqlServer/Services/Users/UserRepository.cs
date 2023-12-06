using DrugSchedule.Api.Data;

namespace DrugSchedule.Api.Services.Users;

public class UserRepository : IUserService
{
    private readonly DrugScheduleContext _dbContext;
    private readonly ILogger<UserRepository> _logger;

    public UserRepository(DrugScheduleContext dbContext, ILogger<UserRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
}