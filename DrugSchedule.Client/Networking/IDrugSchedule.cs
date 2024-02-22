using System.Dynamic;
using DrugSchedule.Api.Shared.Dtos;
using DrugSchedule.StorageContract.Data;
using System.Runtime.InteropServices;

namespace DrugSchedule.Client.Networking;

public interface IAuthControllerClient
{
    Task<ApiCallResult<TokenDto>> LoginAsync(LoginDto body, CancellationToken cancellationToken = default);

    Task<ApiCallResult<string>> RegisterAsync(RegisterDto body, CancellationToken cancellationToken = default);

    Task<ApiCallResult<string>> RefreshTokenAsync(TokenDto body, CancellationToken cancellationToken = default);

    Task<ApiCallResult<AvailableUsernameDto>> UsernameAvailableAsync(UsernameDto body, CancellationToken cancellationToken = default);
}

public interface IContactsControllerClient
{
    Task<ApiCallResult<UserContactsSimpleCollectionDto>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<ApiCallResult<UserContactsSimpleCollectionDto>> GetCommonAsync(CancellationToken cancellationToken = default);

    Task<ApiCallResult<UserContactDto>> GetSingleExtendedAsync(UserIdDto body, CancellationToken cancellationToken = default);

    Task<ApiCallResult<UserContactsCollectionDto>> GetManyExtendedAsync(UserContactFilterDto body, CancellationToken cancellationToken = default);

    Task<ApiCallResult<string>> AddOrUpdateAsync(NewUserContactDto body, CancellationToken cancellationToken = default);

    Task<ApiCallResult<string>> RemoveAsync(UserIdDto body, CancellationToken cancellationToken = default);
}

public interface IDrugLibraryControllerClient
{
    Task<ApiCallResult<MedicamentSimpleDto>> GetMedicamentAsync(MedicamentIdDto body, CancellationToken cancellationToken = default);

    Task<ApiCallResult<MedicamentExtendedDto>> GetMedicamentExtendedAsync(MedicamentIdDto body, CancellationToken cancellationToken = default);

    Task<ApiCallResult<MedicamentSimpleCollectionDto>> GetMedicamentsAsync(MedicamentFilterDto body, CancellationToken cancellationToken = default);

    Task<ApiCallResult<MedicamentExtendedCollectionDto>> GetMedicamentsExtendedAsync(MedicamentFilterDto body, CancellationToken cancellationToken = default);

    Task<ApiCallResult<ManufacturerDto>> GetManufacturerAsync(ManufacturerIdDto body, CancellationToken cancellationToken = default);

    Task<ApiCallResult<ManufacturerCollectionDto>> GetManufacturersAsync(ManufacturerFilterDto body, CancellationToken cancellationToken = default);

    Task<ApiCallResult<ReleaseFormCollectionDto>> GetReleaseFormsAsync(MedicamentReleaseFormFilterDto body, CancellationToken cancellationToken = default);
}

public interface IUsersControllerClient
{
    Task<ApiCallResult<UserFullDto>> User.GetMeAsync(CancellationToken cancellationToken = default);

    Task<ApiCallResult<UserPublicCollectionDto>> User.SearchAsync(UserSearchDto body, CancellationToken cancellationToken = default);

    Task<ApiCallResult<UserIdDto>> User.UpdateProfileAsync(UserUpdateDto body, CancellationToken cancellationToken = default);

    Task<ApiCallResult<string>> User.ChangePasswordAsync(NewPasswordDto body, CancellationToken cancellationToken = default);

    Task<DownloadableFileDto> SetAvatarAsync( file);

    Task<ApiCallResult<string>> User.RemoveAvatarAsync(FileIdDto body, CancellationToken cancellationToken = default);
}



public interface IDrugSchedule
{



    Task<ApiCallResult<T>> Schedule.SearchForScheduleAsync(ScheduleSearchDto body, CancellationToken cancellationToken = default);

    Task<ApiCallResult<T>> Schedule.GetScheduleSimpleAsync(ScheduleIdDto body, CancellationToken cancellationToken = default);

    Task<ApiCallResult<T>> Schedule.GetSchedulesSimpleAsync(TakingScheduleFilterDto body, CancellationToken cancellationToken = default);

    Task<ApiCallResult<T>> Schedule.GetScheduleExtendedAsync(ScheduleIdDto body, CancellationToken cancellationToken = default);

    Task<ApiCallResult<T>> Schedule.GetSchedulesExtendedAsync(TakingScheduleFilterDto body, CancellationToken cancellationToken = default);

    Task<ApiCallResult<T>> Schedule.GetTakingConfirmationsAsync(TakingConfirmationFilterDto body, CancellationToken cancellationToken = default);

    Task<ApiCallResult<T>> Schedule.CreateScheduleAsync(NewScheduleDto body, CancellationToken cancellationToken = default);

    Task<ApiCallResult<T>> Schedule.UpdateScheduleAsync(ScheduleUpdateDto body, CancellationToken cancellationToken = default);

    Task<ApiCallResult<T>> Schedule.RemoveScheduleAsync(ScheduleIdDto body, CancellationToken cancellationToken = default);

    Task<ApiCallResult<T>> Schedule.CreateRepeatAsync(NewScheduleRepeatDto body, CancellationToken cancellationToken = default);

    Task<ApiCallResult<T>> Schedule.UpdateRepeatAsync(ScheduleRepeatUpdateDto body, CancellationToken cancellationToken = default);

    Task<ApiCallResult<T>> Schedule.RemoveRepeatAsync(RepeatIdDto body, CancellationToken cancellationToken = default);

    Task<ApiCallResult<T>> Schedule.AddOrUpdateShareAsync(ScheduleShareUpdateDto body, CancellationToken cancellationToken = default);

    Task<ApiCallResult<T>> Schedule.RemoveShareAsync(ScheduleShareRemoveDto body, CancellationToken cancellationToken = default);

    Task<ApiCallResult<T>> Schedule.CreateConfirmationAsync(NewTakingСonfirmationDto body, CancellationToken cancellationToken = default);

    Task<ApiCallResult<T>> Schedule.UpdateConfirmationAsync(TakingСonfirmationUpdateDto body, CancellationToken cancellationToken = default);

    Task<ApiCallResult<T>> Schedule.RemoveConfirmationAsync(ConfirmationIdDto body, CancellationToken cancellationToken = default);

    Task<ApiCallResult<T>> Schedule.AddConfirmationImageAsync(NewConfirmationImageDto body, CancellationToken cancellationToken = default);

    Task<ApiCallResult<T>> Schedule.RemoveConfirmationImageAsync(ConfirmationImageRemoveDto body, CancellationToken cancellationToken = default);



    Task<ApiCallResult<T>> UserDrugs.GetSingleAsync(UserMedicamentIdDto body, CancellationToken cancellationToken = default);

    Task<ApiCallResult<T>> UserDrugs.GetSingleExtendedAsync(UserMedicamentIdDto body, CancellationToken cancellationToken = default);

    Task<ApiCallResult<T>> UserDrugs.GetManyAsync(UserMedicamentFilterDto body, CancellationToken cancellationToken = default);

    Task<ApiCallResult<T>> UserDrugs.GetManyExtendedAsync(UserMedicamentFilterDto body, CancellationToken cancellationToken = default);

    Task<ApiCallResult<T>> UserDrugs.GetSharedExtendedAsync(UserMedicamentIdDto body, CancellationToken cancellationToken = default);

    Task<ApiCallResult<T>> UserDrugs.AddAsync(NewUserMedicamentDto body, CancellationToken cancellationToken = default);

    [Multipart]
    [Post("/api/UserDrugs/AddImage")]
    Task AddImage([Query, AliasAs("UserMedicamentId.UserMedicamentId")] long? userMedicamentId_UserMedicamentId, [AliasAs("FormFile")] StreamPart formFile);

    Task<ApiCallResult<T>> UserDrugs.RemoveImageAsync(UserMedicamentImageRemoveDto body, CancellationToken cancellationToken = default);

    Task<ApiCallResult<T>> UserDrugs.UpdateAsync(UserMedicamentUpdateDto body, CancellationToken cancellationToken = default);

    Task<ApiCallResult<T>> UserDrugs.RemoveAsync(UserMedicamentIdDto body, CancellationToken cancellationToken = default);
}