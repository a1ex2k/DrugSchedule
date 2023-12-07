using DrugSchedule.SqlServer.Data;
using DrugSchedule.StorageContract.Abstractions;
using Microsoft.Extensions.Logging;

namespace DrugSchedule.SqlServer.Services;

public class UserRepository : IUserRepository
{
    private readonly DrugScheduleContext _dbContext;
    private readonly ILogger<UserRepository> _logger;

    public UserRepository(DrugScheduleContext dbContext, ILogger<UserRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
}