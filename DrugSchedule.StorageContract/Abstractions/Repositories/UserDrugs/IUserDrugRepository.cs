using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.StorageContract.Abstractions;

public interface IUserDrugRepository
{
    public Task<UserMedicamentExtended?> GetMedicamentExtendedAsync(long userProfileId, long id, bool withImages, bool withBasicMedicament, CancellationToken cancellationToken = default);

    public Task<List<UserMedicamentExtended>> GetMedicamentsExtendedAsync(long userProfileId, UserMedicamentFilter userMedicamentFilter, bool withImages, bool withBasicMedicament, CancellationToken cancellationToken = default);

    public Task<UserMedicamentSimple?> GetMedicamentSimpleAsync(long userProfileId, long id, CancellationToken cancellationToken = default);

    public Task<List<UserMedicamentSimple>> GetMedicamentsSimpleAsync(long userProfileId, UserMedicamentFilter userMedicamentFilter,  CancellationToken cancellationToken = default);

    public Task<UserMedicament?> GetMedicamentAsync(long userProfileId, long id, CancellationToken cancellationToken = default);

    public Task<bool> DoesMedicamentExistAsync(long userProfileId, long id, CancellationToken cancellationToken = default);

    public Task<UserMedicament?> CreateMedicamentAsync(UserMedicament model, CancellationToken cancellationToken = default);

    public Task<UserMedicament?> UpdateMedicamentAsync(UserMedicament medicament, UserMedicamentUpdateFlags updateFlags, CancellationToken cancellationToken = default);

    public Task<RemoveOperationResult> RemoveMedicamentAsync(long userProfileId, long id, CancellationToken cancellationToken = default);

}