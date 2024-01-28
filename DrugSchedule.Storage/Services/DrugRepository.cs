﻿using DrugSchedule.Storage.Data;
using DrugSchedule.Storage.Data.Entities;
using DrugSchedule.Storage.Extensions;
using DrugSchedule.StorageContract.Abstractions;
using DrugSchedule.StorageContract.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DrugSchedule.Storage.Services;

public class DrugRepository : IReadonlyDrugRepository, IDrugRepository
{
    private readonly DrugScheduleContext _dbContext;
    private readonly ILogger<DrugRepository> _logger;

    public DrugRepository(DrugScheduleContext dbContext, ILogger<DrugRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }


    public async Task<List<MedicamentExtended>> GetMedicamentsExtendedAsync(MedicamentFilter filter, bool withImages, CancellationToken cancellationToken = default)
    {
        var medicaments = await _dbContext.Medicaments
            .AsNoTracking()
            .WithFilter(m => m.Id, filter.IdFilter)
            .WithFilter(m => m.Name, filter.NameFilter)
            .WithFilter(m => m.ManufacturerId, filter.ManufacturerFilter?.IdFilter?.ConvertAll(v => (int?)v))
            .WithFilter(m => m.Manufacturer!.Name, filter.ManufacturerFilter?.NameFilter)
            .WithFilter(m => m.ReleaseFormId, filter.MedicamentReleaseFormFilter?.IdFilter)
            .WithFilter(m => m.ReleaseForm!.Name, filter.MedicamentReleaseFormFilter?.NameFilter)
            .OrderBy(m => m.Name)
            .WithPaging(filter)
            .Select(m => m.ToContractModel(withImages))
            .ToListAsync(cancellationToken);

        return medicaments;
    }


    public async Task<MedicamentSimple?> GetMedicamentSimpleByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var medicament = await _dbContext.Medicaments
            .AsNoTracking()
            .Select(m => new MedicamentSimple
            {
                Id = m.Id,
                Name = m.Name,
                ReleaseForm = m.ReleaseForm!.Name,
                Manufacturer = m.ManufacturerId != null ? m.Manufacturer!.Name : null,
            })
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
        return medicament;
    }


    public async Task<List<Contract.Manufacturer>> GetManufacturersAsync(ManufacturerFilter filter, CancellationToken cancellationToken = default)
    {
        var manufacturers = await _dbContext.Manufacturers
            .AsNoTracking()
            .WithFilter(m => m.Id, filter.IdFilter)
            .WithFilter(m => m.Name, filter.NameFilter)
            .OrderBy(m => m.Name)
            .WithPaging(filter)
            .Select(m => m.ToContractModel())
            .ToListAsync(cancellationToken);

        return manufacturers;
    }


    public async Task<List<Contract.MedicamentReleaseForm>> GetMedicamentReleaseFormsAsync(MedicamentReleaseFormFilter filter, CancellationToken cancellationToken = default)
    {
        var releaseForms = await _dbContext.ReleaseForms
            .AsNoTracking()
            .WithFilter(m => m.Id, filter.IdFilter)
            .WithFilter(m => m.Name, filter.NameFilter)
            .OrderBy(m => m.Name)
            .WithPaging(filter)
            .Select(m => m.ToContractModel())
            .ToListAsync(cancellationToken);

        return releaseForms;
    }


    public async Task<MedicamentExtended?> GetMedicamentExtendedByIdAsync(int id, bool withImages, CancellationToken cancellationToken = default)
    {
        var medicament = await _dbContext.Medicaments
            .AsNoTracking()
            .Select(m => m.ToContractModel(withImages))
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);

        return medicament;
    }

    public async Task<List<MedicamentSimple>> GetMedicamentsSimpleAsync(MedicamentFilter filter, CancellationToken cancellationToken = default)
    {
        var medicaments = await _dbContext.Medicaments
            .AsNoTracking()
            .WithFilter(m => m.Id, filter.IdFilter)
            .WithFilter(m => m.Name, filter.NameFilter)
            .WithFilter(m => m.ManufacturerId, filter.ManufacturerFilter?.IdFilter?.ConvertAll(v => (int?)v))
            .WithFilter(m => m.Manufacturer!.Name, filter.ManufacturerFilter?.NameFilter)
            .WithFilter(m => m.ReleaseFormId, filter.MedicamentReleaseFormFilter?.IdFilter)
            .WithFilter(m => m.ReleaseForm!.Name, filter.MedicamentReleaseFormFilter?.NameFilter)
            .OrderBy(m => m.Name)
            .WithPaging(filter)
            .Select(m => new MedicamentSimple
            {
                Id = m.Id,
                Name = m.Name,
                ReleaseForm = m.ReleaseForm!.Name,
                Manufacturer = m.ManufacturerId != null ? m.Manufacturer!.Name : null,
            })
            .ToListAsync(cancellationToken);
        return medicaments;
    }

    public async Task<MedicamentExtended?> UpdateMedicamentAsync(MedicamentExtended medicamentSimple, MedicamentUpdateFlags updateFlags, CancellationToken cancellationToken = default)
    {
        var existingMedicament = await _dbContext.Medicaments
            .Include(m => m.Images)
            .FirstOrDefaultAsync(m => m.Id == medicamentSimple.Id, cancellationToken);
        if (existingMedicament is null)
        {
            return null;
        }

        if (updateFlags.Name)
        {
            existingMedicament.Name = medicamentSimple.Name;
        }

        if (updateFlags.Description)
        {
            existingMedicament.Description = medicamentSimple.Description;
        }

        if (updateFlags.Composition)
        {
            existingMedicament.Composition = medicamentSimple.Composition;
        }

        if (updateFlags.ReleaseForm)
        {
            existingMedicament.ReleaseForm = new Entities.MedicamentReleaseForm
            {
                Id = medicamentSimple.ReleaseForm.Id,
                Name = medicamentSimple.ReleaseForm.Name
            };
        }

        if (updateFlags.Manufacturer)
        {
            existingMedicament.Manufacturer = medicamentSimple.Manufacturer is null ? null : new Entities.Manufacturer
            {
                Id = medicamentSimple.Manufacturer.Id,
                Name = medicamentSimple.Manufacturer.Name,
                AdditionalInfo = medicamentSimple.Manufacturer.AdditionalInfo
            };
        }

        if (updateFlags.ImagesGuids)
        {
            existingMedicament.Images.Clear();
            var newGuids = medicamentSimple?.Images?.Select(i => i.Guid);
            if (newGuids != null)
            {
                var newMedicamentFilesEntities = newGuids.Select(guid => new MedicamentFile
                {
                    FileGuid = guid
                });
                existingMedicament.Images.AddRange(newMedicamentFilesEntities);
            }
        }

        var saved = await _dbContext.TrySaveChangesAsync(_logger, cancellationToken);
        return saved ? existingMedicament.ToContractModel(false) : null;
    }


    public async Task<Contract.Manufacturer?> GetManufacturerByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var manufacturer = await _dbContext.Manufacturers
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
        return manufacturer?.ToContractModel();
    }

    public async Task<Contract.Manufacturer?> UpdateManufacturerAsync(Contract.Manufacturer manufacturer, ManufacturerUpdateFlags updateFlags,
        CancellationToken cancellationToken = default)
    {
        var existingManufacturer = await _dbContext.Manufacturers
            .FirstOrDefaultAsync(f => f.Id == manufacturer.Id, cancellationToken);
        if (existingManufacturer is null)
        {
            return null;
        }

        if (updateFlags.Name)
        {
            existingManufacturer.Name = manufacturer.Name;
        }

        if (updateFlags.AdditionalInfo)
        {
            existingManufacturer.AdditionalInfo = manufacturer.AdditionalInfo;
        }

        var saved = await _dbContext.TrySaveChangesAsync(_logger, cancellationToken);
        return saved ? existingManufacturer.ToContractModel() : null;
    }


    public async Task<Contract.MedicamentReleaseForm?> GetMedicamentReleaseFormByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var releaseForm = await _dbContext.ReleaseForms
            .AsNoTracking()
            .FirstOrDefaultAsync(f => f.Id == id, cancellationToken);

        return releaseForm?.ToContractModel();
    }

    public async Task<Contract.MedicamentReleaseForm?> UpdateMedicamentReleaseFormAsync(Contract.MedicamentReleaseForm releaseForm, MedicamentReleaseFormUpdateFlags updateFlags,
        CancellationToken cancellationToken = default)
    {
        var existingReleaseForm = await _dbContext.ReleaseForms
            .FirstOrDefaultAsync(f => f.Id == releaseForm.Id, cancellationToken: cancellationToken);
        if (existingReleaseForm is null)
        {
            return null;
        }

        if (updateFlags.Name)
        {
            existingReleaseForm.Name = releaseForm.Name;
        }

        var saved = await _dbContext.TrySaveChangesAsync(_logger, cancellationToken);
        return saved ? existingReleaseForm.ToContractModel() : null;
    }


    public async Task<MedicamentExtended?> CreateMedicamentAsync(MedicamentExtended medicament, CancellationToken cancellationToken = default)
    {
        var medicamentEntity = new Entities.Medicament
        {
            Name = medicament.Name,
            Description = medicament.Description,
            Composition = medicament.Composition,
            ReleaseFormId = medicament.ReleaseForm.Id,
            ManufacturerId = medicament.Manufacturer?.Id,
        };

        if (medicament.ReleaseForm.Id == 0)
        {
            medicamentEntity.ReleaseForm = new Entities.MedicamentReleaseForm()
            {
                Name = medicament.ReleaseForm.Name
            };
        }

        if (medicament.Manufacturer!.Id == 0)
        {
            medicamentEntity.Manufacturer = new Entities.Manufacturer()
            {
                Id = medicament.Manufacturer.Id,
                Name = medicament.Manufacturer.Name,
                AdditionalInfo = medicament.Manufacturer.AdditionalInfo,
            };
        }

        var newMedicamentFilesEntities = medicament?.Images?.Select(i => new MedicamentFile
        {
            FileGuid = i.Guid
        });
        if (newMedicamentFilesEntities != null)
        {
            medicamentEntity.Images.AddRange(newMedicamentFilesEntities);
        }

        var saved = await _dbContext.TrySaveChangesAsync(_logger, cancellationToken);
        return saved ? medicamentEntity.ToContractModel(false) : null;
    }


    public async Task<Contract.Manufacturer?> CreateManufacturerAsync(Contract.Manufacturer manufacturer, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(manufacturer);

        var manufacturerEntity = new Entities.Manufacturer
        {
            Name = manufacturer.Name,
            AdditionalInfo = manufacturer.AdditionalInfo
        };

        await _dbContext.AddAsync(manufacturerEntity, cancellationToken);
        var saved = await _dbContext.TrySaveChangesAsync(_logger, cancellationToken);
        return saved ? manufacturerEntity.ToContractModel() : null;
    }

    public async Task<Contract.MedicamentReleaseForm?> CreateMedicamentReleaseForAsync(Contract.MedicamentReleaseForm releaseForm, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(releaseForm);

        var releaseFormEntity = new Entities.MedicamentReleaseForm
        {
            Name = releaseForm.Name
        };

        await _dbContext.ReleaseForms.AddAsync(releaseFormEntity, cancellationToken);
        var saved = await _dbContext.TrySaveChangesAsync(_logger, cancellationToken);
        return saved ? releaseFormEntity.ToContractModel() : null;
    }


    public async Task<RemoveOperationResult> RemoveMedicamentByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var medicament = await _dbContext.Medicaments
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);

        if (medicament is null)
        {
            return RemoveOperationResult.NotFound;
        }

        _dbContext.Medicaments.Remove(medicament);
        var saved = await _dbContext.TrySaveChangesAsync(_logger, cancellationToken);
        return saved ? RemoveOperationResult.SuccessfullyRemoved : RemoveOperationResult.Used;
    }

    public async Task<bool> AddMedicamentImageGuidAsync(int medicamentId, Guid imageGuid, CancellationToken cancellationToken = default)
    {
        var medicamentFile = new Entities.MedicamentFile
        {
            MedicamentId = medicamentId,
            FileGuid = imageGuid
        };

        await _dbContext.MedicamentFiles.AddAsync(medicamentFile, cancellationToken);
        var saved = await _dbContext.TrySaveChangesAsync(_logger, cancellationToken);
        return saved;
    }

    public async Task<bool> RemoveMedicamentImageGuidAsync(int medicamentId, Guid imageGuid, CancellationToken cancellationToken = default)
    {
        var removedCount = await _dbContext.MedicamentFiles
           .Where(m => m.MedicamentId == medicamentId && m.FileGuid == imageGuid)
           .ExecuteDeleteAsync(cancellationToken);

        return removedCount >= 1;
    }


    public async Task<RemoveOperationResult> RemoveManufacturerByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var manufacturer = await _dbContext.Manufacturers
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);

        if (manufacturer is null)
        {
            return RemoveOperationResult.NotFound;
        }

        _dbContext.Manufacturers.Remove(manufacturer);
        var removed = await _dbContext.TrySaveChangesAsync(_logger, cancellationToken);
        return removed ? RemoveOperationResult.SuccessfullyRemoved : RemoveOperationResult.Used;
    }


    public async Task<RemoveOperationResult> RemoveMedicamentReleaseFormByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var releaseForm = await _dbContext.ReleaseForms
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);

        if (releaseForm is null)
        {
            return RemoveOperationResult.NotFound;
        }

        _dbContext.Remove(releaseForm);
        var removed = await _dbContext.TrySaveChangesAsync(_logger, cancellationToken);
        return removed ? RemoveOperationResult.SuccessfullyRemoved : RemoveOperationResult.Used;
    }
}