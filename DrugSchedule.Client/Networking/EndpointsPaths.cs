﻿namespace DrugSchedule.Client.Networking;

internal static class EndpointsPaths
{
    public const string Auth_Login = "/api/Auth/Login";
    public const string Auth_Register = "/api/Auth/Register";
    public const string Auth_RefreshToken = "/api/Auth/RefreshToken";
    public const string Auth_UsernameAvailable = "/api/Auth/UsernameAvailable";
    public const string Auth_RevokeTokens = "/api/Auth/RevokeTokens";
    public const string Contacts_GetAll = "/api/GetAll";
    public const string Contacts_GetCommon = "/api/GetCommon";
    public const string Contacts_GetSingleExtended = "/api/GetSingleExtended";
    public const string Contacts_GetManyExtended = "/api/GetManyExtended";
    public const string Contacts_AddOrUpdate = "/api/AddOrUpdate";
    public const string Contacts_Remove = "/api/Remove";
    public const string DrugLibrary_GetMedicament = "/api/DrugLibrary/GetMedicament";
    public const string DrugLibrary_GetMedicamentExtended = "/api/DrugLibrary/GetMedicamentExtended";
    public const string DrugLibrary_GetMedicaments = "/api/DrugLibrary/GetMedicaments";
    public const string DrugLibrary_GetMedicamentsExtended = "/api/DrugLibrary/GetMedicamentsExtended";
    public const string DrugLibrary_GetManufacturer = "/api/DrugLibrary/GetManufacturer";
    public const string DrugLibrary_GetManufacturers = "/api/DrugLibrary/GetManufacturers";
    public const string DrugLibrary_GetReleaseForms = "/api/DrugLibrary/GetReleaseForms";
    public const string Schedule_SearchForSchedule = "/api/Schedule/SearchForSchedule";
    public const string Schedule_GetScheduleSimple = "/api/Schedule/GetScheduleSimple";
    public const string Schedule_GetSchedulesSimple = "/api/Schedule/GetSchedulesSimple";
    public const string Schedule_GetScheduleExtended = "/api/Schedule/GetScheduleExtended";
    public const string Schedule_GetSchedulesExtended = "/api/Schedule/GetSchedulesExtended";
    public const string Schedule_GetTakingConfirmations = "/api/Schedule/GetTakingConfirmations";
    public const string Schedule_CreateSchedule = "/api/Schedule/CreateSchedule";
    public const string Schedule_UpdateSchedule = "/api/Schedule/UpdateSchedule";
    public const string Schedule_RemoveSchedule = "/api/Schedule/RemoveSchedule";
    public const string Schedule_CreateRepeat = "/api/Schedule/CreateRepeat";
    public const string Schedule_UpdateRepeat = "/api/Schedule/UpdateRepeat";
    public const string Schedule_RemoveRepeat = "/api/Schedule/RemoveRepeat";
    public const string Schedule_AddOrUpdateShare = "/api/Schedule/AddOrUpdateShare";
    public const string Schedule_RemoveShare = "/api/Schedule/RemoveShare";
    public const string Schedule_CreateConfirmation = "/api/Schedule/CreateConfirmation";
    public const string Schedule_UpdateConfirmation = "/api/Schedule/UpdateConfirmation";
    public const string Schedule_RemoveConfirmation = "/api/Schedule/RemoveConfirmation";
    public const string Schedule_AddConfirmationImage = "/api/Schedule/AddConfirmationImage";
    public const string Schedule_RemoveConfirmationImage = "/api/Schedule/RemoveConfirmationImage";
    public const string User_GetMe = "/api/User/GetMe";
    public const string User_Search = "/api/User/Search";
    public const string User_UpdateProfile = "/api/User/UpdateProfile";
    public const string User_ChangePassword = "/api/User/ChangePassword";
    public const string User_SetAvatar = "/api/User/SetAvatar";
    public const string User_RemoveAvatar = "/api/User/RemoveAvatar";
    public const string UserDrugs_GetSingle = "/api/UserDrugs/GetSingle";
    public const string UserDrugs_GetSingleExtended = "/api/UserDrugs/GetSingleExtended";
    public const string UserDrugs_GetMany = "/api/UserDrugs/GetMany";
    public const string UserDrugs_GetManyExtended = "/api/UserDrugs/GetManyExtended";
    public const string UserDrugs_GetSharedExtended = "/api/UserDrugs/GetSharedExtended";
    public const string UserDrugs_Add = "/api/UserDrugs/Add";
    public const string UserDrugs_AddImage = "/api/UserDrugs/AddImage";
    public const string UserDrugs_RemoveImage = "/api/UserDrugs/RemoveImage";
    public const string UserDrugs_Update = "/api/UserDrugs/Update";
    public const string UserDrugs_Remove = "/api/UserDrugs/Remove";
}