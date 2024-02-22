using DrugSchedule.Api.Shared.Dtos;
using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.Client.Networking;

public interface IDrugScheduleApi
{
    [Post("/api/Auth/Login")]
    Task Login([Body] LoginDto body);

    [Post("/api/Auth/Register")]
    Task Register([Body] RegisterDto body);

    [Post("/api/Auth/RefreshToken")]
    Task RefreshToken([Body] TokenDto body);

    [Post("/api/Auth/UsernameAvailable")]
    Task UsernameAvailable([Body] UsernameDto body);

    [Post("/api/GetAll")]
    Task GetAll();

    [Post("/api/GetCommon")]
    Task GetCommon();

    [Post("/api/GetSingleExtended")]
    Task GetSingleExtended([Body] UserIdDto body);

    [Post("/api/GetManyExtended")]
    Task GetManyExtended([Body] UserContactFilterDto body);

    [Post("/api/AddOrUpdate")]
    Task AddOrUpdate([Body] NewUserContactDto body);

    [Post("/api/Remove")]
    Task Remove([Body] UserIdDto body);

    [Post("/api/DrugLibrary/GetMedicament")]
    Task GetMedicament([Body] MedicamentIdDto body);

    [Post("/api/DrugLibrary/GetMedicamentExtended")]
    Task GetMedicamentExtended([Body] MedicamentIdDto body);

    [Post("/api/DrugLibrary/GetMedicaments")]
    Task GetMedicaments([Body] MedicamentFilterDto body);

    [Post("/api/DrugLibrary/GetMedicamentsExtended")]
    Task GetMedicamentsExtended([Body] MedicamentFilterDto body);

    [Post("/api/DrugLibrary/GetManufacturer")]
    Task GetManufacturer([Body] ManufacturerIdDto body);

    [Post("/api/DrugLibrary/GetManufacturers")]
    Task GetManufacturers([Body] ManufacturerFilterDto body);

    [Post("/api/DrugLibrary/GetReleaseForms")]
    Task GetReleaseForms([Body] MedicamentReleaseFormFilterDto body);

    [Post("/api/Schedule/SearchForSchedule")]
    Task SearchForSchedule([Body] ScheduleSearchDto body);

    [Post("/api/Schedule/GetScheduleSimple")]
    Task GetScheduleSimple([Body] ScheduleIdDto body);

    [Post("/api/Schedule/GetSchedulesSimple")]
    Task GetSchedulesSimple([Body] TakingScheduleFilterDto body);

    [Post("/api/Schedule/GetScheduleExtended")]
    Task GetScheduleExtended([Body] ScheduleIdDto body);

    [Post("/api/Schedule/GetSchedulesExtended")]
    Task GetSchedulesExtended([Body] TakingScheduleFilterDto body);

    [Post("/api/Schedule/GetTakingConfirmations")]
    Task GetTakingConfirmations([Body] TakingConfirmationFilterDto body);

    [Post("/api/Schedule/CreateSchedule")]
    Task CreateSchedule([Body] NewScheduleDto body);

    [Post("/api/Schedule/UpdateSchedule")]
    Task UpdateSchedule([Body] ScheduleUpdateDto body);

    [Post("/api/Schedule/RemoveSchedule")]
    Task RemoveSchedule([Body] ScheduleIdDto body);

    [Post("/api/Schedule/CreateRepeat")]
    Task CreateRepeat([Body] NewScheduleRepeatDto body);

    [Post("/api/Schedule/UpdateRepeat")]
    Task UpdateRepeat([Body] ScheduleRepeatUpdateDto body);

    [Post("/api/Schedule/RemoveRepeat")]
    Task RemoveRepeat([Body] RepeatIdDto body);

    [Post("/api/Schedule/AddOrUpdateShare")]
    Task AddOrUpdateShare([Body] ScheduleShareUpdateDto body);

    [Post("/api/Schedule/RemoveShare")]
    Task RemoveShare([Body] ScheduleShareRemoveDto body);

    [Post("/api/Schedule/CreateConfirmation")]
    Task CreateConfirmation([Body] NewTaking—onfirmationDto body);

    [Post("/api/Schedule/UpdateConfirmation")]
    Task UpdateConfirmation([Body] Taking—onfirmationUpdateDto body);

    [Post("/api/Schedule/RemoveConfirmation")]
    Task RemoveConfirmation([Body] ConfirmationIdDto body);

    [Post("/api/Schedule/AddConfirmationImage")]
    Task AddConfirmationImage([Body] NewConfirmationImageDto body);

    [Post("/api/Schedule/RemoveConfirmationImage")]
    Task RemoveConfirmationImage([Body] ConfirmationImageRemoveDto body);

    [Post("/api/User/GetMe")]
    Task GetMe();

    [Post("/api/User/Search")]
    Task Search([Body] UserSearchDto body);

    [Post("/api/User/UpdateProfile")]
    Task UpdateProfile([Body] UserUpdateDto body);

    [Post("/api/User/ChangePassword")]
    Task ChangePassword([Body] NewPasswordDto body);

    [Post("/api/User/SetAvatar")]
    Task SetAvatar( StreamPart file);

    [Post("/api/User/RemoveAvatar")]
    Task RemoveAvatar([Body] FileIdDto body);

    [Post("/api/UserDrugs/GetSingle")]
    Task GetSingle([Body] UserMedicamentIdDto body);

    [Post("/api/UserDrugs/GetSingleExtended")]
    Task GetSingleExtended2([Body] UserMedicamentIdDto body);

    [Post("/api/UserDrugs/GetMany")]
    Task GetMany([Body] UserMedicamentFilterDto body);

    [Post("/api/UserDrugs/GetManyExtended")]
    Task GetManyExtended2([Body] UserMedicamentFilterDto body);

    [Post("/api/UserDrugs/GetSharedExtended")]
    Task GetSharedExtended([Body] UserMedicamentIdDto body);

    [Post("/api/UserDrugs/Add")]
    Task Add([Body] NewUserMedicamentDto body);

    [Multipart]
    [Post("/api/UserDrugs/AddImage")]
    Task AddImage([Query, AliasAs("UserMedicamentId.UserMedicamentId")] long? userMedicamentId_UserMedicamentId, [AliasAs("FormFile")] StreamPart formFile);

    [Post("/api/UserDrugs/RemoveImage")]
    Task RemoveImage([Body] UserMedicamentImageRemoveDto body);

    [Post("/api/UserDrugs/Update")]
    Task Update([Body] UserMedicamentUpdateDto body);

    [Post("/api/UserDrugs/Remove")]
    Task Remove2([Body] UserMedicamentIdDto body);


}