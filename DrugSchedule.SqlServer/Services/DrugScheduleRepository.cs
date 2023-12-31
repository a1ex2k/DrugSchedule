﻿using DrugSchedule.SqlServer.Data;
using DrugSchedule.StorageContract.Abstractions;
using Microsoft.Extensions.Logging;

namespace DrugSchedule.SqlServer.Services;

public class DrugScheduleRepository : IDrugScheduleRepository
{
    private readonly DrugScheduleContext _dbContext;
    private readonly ILogger<DrugScheduleRepository> _logger;

    public DrugScheduleRepository(DrugScheduleContext dbContext, ILogger<DrugScheduleRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
}