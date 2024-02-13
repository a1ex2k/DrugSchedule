using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.Storage.Extensions;

public static class EntityMapExtensions
{
    public static Contract.UserIdentity ToContractModel(this Microsoft.AspNetCore.Identity.IdentityUser identityUser)
    {
        return new Contract.UserIdentity
        {
            Guid = Guid.Parse(identityUser.Id),
            Username = identityUser.UserName,
            Email = identityUser.Email,
            IsEmailConfirmed = identityUser.EmailConfirmed
        };
    }

    public static Contract.FileInfo ToContractModel(this Entities.FileInfo fileInfo)
    {
        return new Contract.FileInfo
        {
            Guid = fileInfo.Guid,
            OriginalName = fileInfo.OriginalName,
            FileExtension = fileInfo.Extension,
            Category = fileInfo.FileCategory,
            MediaType = fileInfo.MediaType,
            Size = fileInfo.Size,
            CreatedAt = fileInfo.CreatedAt,
            HasThumbnail = fileInfo.HasThumbnail
        };
    }

    public static Contract.UserProfile ToContractModel(this Entities.UserProfile userProfile, bool withAvatar)
    {
        return new Contract.UserProfile
        {
            UserProfileId = userProfile.Id,
            UserIdentityGuid = Guid.Parse(userProfile.IdentityGuid),
            RealName = userProfile.RealName,
            DateOfBirth = userProfile.DateOfBirth ?? DateOnly.MinValue,
            Avatar = withAvatar ? userProfile.AvatarInfo?.ToContractModel() : null,
            Sex = userProfile.Sex
        };
    }

    public static Contract.Manufacturer ToContractModel(this Entities.Manufacturer manufacturer)
    {
        return new Contract.Manufacturer
        {
            Id = manufacturer.Id,
            Name = manufacturer.Name,
            AdditionalInfo = manufacturer.AdditionalInfo,
        };
    }

    public static Contract.MedicamentReleaseForm ToContractModel(this Entities.MedicamentReleaseForm releaseForm)
    {
        return new Contract.MedicamentReleaseForm
        {
            Id = releaseForm.Id,
            Name = releaseForm.Name
        };
    }

    public static Contract.MedicamentExtended ToContractModel(this Entities.Medicament medicament, bool withImages)
    {
        return new Contract.MedicamentExtended
        {
            Id = medicament.Id,
            Name = medicament.Name,
            Composition = medicament.Composition,
            Description = medicament.Description,
            ReleaseForm = medicament.ReleaseForm!.ToContractModel(),
            Manufacturer = medicament.Manufacturer!.ToContractModel(),
            Images = withImages ? medicament.Files.Select(f => f.FileInfo!.ToContractModel()).ToList() : null,
        };
    }

    public static Contract.RefreshTokenEntry ToContractModel(this Entities.RefreshTokenEntry refreshTokenEntry)
    {
        return new Contract.RefreshTokenEntry
        {
            UserGuid = Guid.Parse(refreshTokenEntry.IdentityUserGuid),
            RefreshToken = refreshTokenEntry.RefreshToken,
            RefreshTokenExpiryTime = refreshTokenEntry.RefreshTokenExpiryTime,
            ClientInfo = refreshTokenEntry.ClientInfo
        };
    }


    public static Contract.UserMedicament ToContractModel(this Entities.UserMedicament userMedicament)
    {
        return new Contract.UserMedicament
        {
            BasicMedicamentId = userMedicament.BasedOnMedicamentId,
            Name = userMedicament.Name,
            Description = userMedicament.Description,
            Composition = userMedicament.Composition,
            ReleaseForm = userMedicament.ReleaseForm,
            ManufacturerName = userMedicament.ManufacturerName,
            UserProfileId = userMedicament.UserProfileId,
        };
    }

    public static Contract.TakingSchedulePlain ToContractModel(this Entities.MedicamentTakingSchedule takingSchedule)
    {
        return new Contract.TakingSchedulePlain
        {
            Id = takingSchedule.Id,
            UserProfileId = takingSchedule.UserProfileId,
            GlobalMedicamentId = takingSchedule.GlobalMedicamentId,
            UserMedicamentId = takingSchedule.UserMedicamentId,
            Information = takingSchedule.Information,
            CreatedAt = takingSchedule.CreatedAt,
            Enabled = takingSchedule.Enabled,
        };
    }

    public static Contract.ScheduleRepeatPlain ToContractModel(this Entities.ScheduleRepeat scheduleRepeat)
    {
        return new Contract.ScheduleRepeatPlain
        {
            Id = scheduleRepeat.Id,
            BeginDate = scheduleRepeat.BeginDate,
            Time = scheduleRepeat.Time,
            TimeOfDay = scheduleRepeat.TimeOfDay,
            RepeatDayOfWeek = scheduleRepeat.RepeatDayOfWeek,
            EndDate = scheduleRepeat.EndDate,
            MedicamentTakingScheduleId = scheduleRepeat.MedicamentTakingScheduleId,
            TakingRule = scheduleRepeat.TakingRule,
        };
    }
}