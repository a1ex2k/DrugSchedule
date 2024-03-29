﻿using DrugSchedule.Services.Models;
using DrugSchedule.StorageContract.Data;
using OneOf.Types;

namespace DrugSchedule.Services.Services.Abstractions;

public interface IUserDrugLibrary
{
    public Task<OneOf<UserMedicamentExtendedModel, NotFound>> GetMedicamentExtendedAsync(long id, CancellationToken cancellationToken = default);

    public Task<UserMedicamentExtendedCollection> GetMedicamentsExtendedAsync(UserMedicamentFilter filter, CancellationToken cancellationToken = default);

    public Task<OneOf<UserMedicamentSimpleModel, NotFound>> GetMedicamentSimpleAsync(long id, CancellationToken cancellationToken = default);

    public Task<UserMedicamentSimpleCollection> GetMedicamentsSimpleAsync(UserMedicamentFilter filter, CancellationToken cancellationToken = default);

    public Task<OneOf<UserMedicamentId, InvalidInput>> CreateMedicamentAsync(NewUserMedicament model, CancellationToken cancellationToken = default);

    public Task<OneOf<UserMedicamentId, NotFound, InvalidInput>> UpdateMedicamentAsync(UserMedicamentUpdate updateModel, CancellationToken cancellationToken = default);

    public Task<OneOf<True, NotFound, InvalidInput>> RemoveMedicamentAsync(long id, CancellationToken cancellationToken = default);

    public Task<OneOf<DownloadableFile, NotFound, InvalidInput>> AddImageAsync(long medicamentId, InputFile inputFile, CancellationToken cancellationToken = default);

    public Task<OneOf<True, NotFound>> RemoveImageAsync(long medicamentId, Guid fileGuid, CancellationToken cancellationToken = default);

    public Task<OneOf<UserMedicamentExtendedModel, NotFound>> GetSharedUserMedicamentAsync(long userMedicamentId, CancellationToken cancellationToken = default);
}