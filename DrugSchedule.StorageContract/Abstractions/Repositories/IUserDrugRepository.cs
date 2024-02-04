using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.StorageContract.Abstractions;

public interface IUserDrugRepository
{
    public Task<UserMedicamentExtended?> GetMedicamentsExtendedAsync(long userProfileId, long id, bool withImages, bool withBasicMedicament, CancellationToken cancellationToken = default);

    public Task<List<UserMedicamentExtended>> GetMedicamentsExtendedAsync(long userProfileId, UserMedicamentFilter userMedicamentFilter, bool withImages, bool withBasicMedicament, CancellationToken cancellationToken = default);

    public Task<UserMedicamentSimple?> GetMedicamentSimpleAsync(long userProfileId, long id, CancellationToken cancellationToken = default);

    public Task<List<UserMedicamentSimple>> GetMedicamentsSimpleAsync(long userProfileId, UserMedicamentFilter userMedicamentFilter,  CancellationToken cancellationToken = default);

    public Task<UserMedicamentExtended?> CreateMedicamentAsync(UserMedicamentExtended userMedicamentExtended, CancellationToken cancellationToken = default);

    public Task<UserMedicamentExtended?> UpdateMedicamentAsync(UserMedicamentExtended userMedicamentExtended, UserMedicamentUpdateFlags updateFlags, CancellationToken cancellationToken = default);

    public Task<RemoveOperationResult> RemoveContactAsync(long medicamentId, CancellationToken cancellationToken = default);

}