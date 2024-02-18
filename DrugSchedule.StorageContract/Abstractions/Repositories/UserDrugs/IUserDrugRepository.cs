using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.StorageContract.Abstractions;

public interface IUserDrugRepository
{
    public Task<UserMedicamentExtended?> GetMedicamentExtendedAsync(long userId, long id, bool withImages, bool withBasicMedicament, CancellationToken cancellationToken = default);

    public Task<List<UserMedicamentExtended>> GetMedicamentsExtendedAsync(long userId, UserMedicamentFilter userMedicamentFilter, bool withImages, bool withBasicMedicament, CancellationToken cancellationToken = default);

    public Task<UserMedicamentSimple?> GetMedicamentSimpleAsync(long userId, long id, CancellationToken cancellationToken = default);

    public Task<List<UserMedicamentSimple>> GetMedicamentsSimpleAsync(long userId, UserMedicamentFilter userMedicamentFilter,  CancellationToken cancellationToken = default);

    public Task<UserMedicamentPlain?> GetMedicamentAsync(long userId, long id, CancellationToken cancellationToken = default);

    public Task<bool> DoesMedicamentExistAsync(long userId, long id, CancellationToken cancellationToken = default);

    public Task<UserMedicamentPlain?> CreateMedicamentAsync(UserMedicamentPlain medicament, CancellationToken cancellationToken = default);

    public Task<UserMedicamentPlain?> UpdateMedicamentAsync(UserMedicamentPlain medicament, UserMedicamentUpdateFlags updateFlags, CancellationToken cancellationToken = default);

    public Task<RemoveOperationResult> RemoveMedicamentAsync(long userId, long id, CancellationToken cancellationToken = default);

    public Task<Guid?> AddMedicamentImageAsync(long id, Guid fileGuid, CancellationToken cancellationToken = default);

    public Task<RemoveOperationResult> RemoveMedicamentImageAsync(long id, Guid fileGuid, CancellationToken cancellationToken = default);
}