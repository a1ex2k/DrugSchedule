﻿namespace DrugSchedule.Storage.Extensions;

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
        };
    }

    public static Contract.UserProfile ToContractModel(this Entities.UserProfile userProfile)
    {
        return new Contract.UserProfile
        {
            UserProfileId = userProfile.Id,
            UserIdentityGuid = Guid.Parse(userProfile.IdentityGuid),
            RealName = userProfile.RealName,
            DateOfBirth = userProfile.DateOfBirth ?? DateOnly.MinValue,
            AvatarGuid = userProfile.AvatarGuid,
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

    public static Contract.Medicament ToContractModel(this Entities.Medicament medicament)
    {
        return new Contract.Medicament
        {
            Id = medicament.Id,
            Name = medicament.Name,
            Composition = medicament.Composition,
            Description = medicament.Description,
            ReleaseForm = medicament.ReleaseForm!.ToContractModel(),
            Manufacturer = medicament.Manufacturer!.ToContractModel(),
            ImagesGuids = medicament.Images.Select(f => f.FileGuid).ToList()
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

    public static Contract.UserContact ToContractModel(this Entities.UserProfileContact userProfileContact)
    {
        return new Contract.UserContact
        {
            UserProfileId = userProfileContact.UserProfileId,
            Profile = userProfileContact.ContactProfile!.ToContractModel(),
            CustomName = userProfileContact.Name
        };
    }
}