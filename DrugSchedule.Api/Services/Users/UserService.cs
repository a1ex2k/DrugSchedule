using DrugSchedule.Api.Data;

namespace DrugSchedule.Api.Services.Users;

public class UserService : IUserService
{
    private readonly DrugScheduleContext _dbContext;
    private readonly ILogger<UserService> _logger;

    public UserService(DrugScheduleContext dbContext, ILogger<UserService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
}