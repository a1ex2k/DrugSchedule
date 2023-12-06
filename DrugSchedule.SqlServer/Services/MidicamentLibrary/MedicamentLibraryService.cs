using DrugSchedule.Api.Data;

namespace DrugSchedule.Api.Services.MedicamentLibrary;

public class MedicamentLibraryService : IMedicamentLibraryService
{
    private readonly DrugScheduleContext _dbContext;
    private readonly ILogger<MedicamentLibraryService> _logger;

    public MedicamentLibraryService(DrugScheduleContext dbContext, ILogger<MedicamentLibraryService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
}