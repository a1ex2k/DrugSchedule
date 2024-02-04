using System.IO;
using System.Linq.Expressions;
using DrugSchedule.Storage.Data.Entities;
using DrugSchedule.StorageContract.Data;
using Microsoft.EntityFrameworkCore;

namespace DrugSchedule.Storage.Extensions;

public static class EntityMapExpressions
{


    public static Expression<Func<Entities.Manufacturer, Contract.Manufacturer>> ToManufacturer => manufacturer => new Contract.Manufacturer
    {
        Id = manufacturer.Id,
        Name = manufacturer.Name,
        AdditionalInfo = manufacturer.AdditionalInfo,
    };


    public static Expression<Func<Entities.MedicamentReleaseForm, Contract.MedicamentReleaseForm>> ToMedicamentReleaseForm => releaseForm => new Contract.MedicamentReleaseForm
    {
        Id = releaseForm.Id,
        Name = releaseForm.Name
    };


    public static Expression<Func<Entities.Medicament, Contract.MedicamentExtended>> ToMedicamentExtended(bool withImages) => (medicament) => new Contract.MedicamentExtended
    {
        Id = medicament.Id,
        Name = medicament.Name,
        Composition = medicament.Composition,
        Description = medicament.Description,
        ReleaseForm = ToMedicamentReleaseForm.Compile().Invoke(medicament.ReleaseForm!),
        Manufacturer = medicament.ManufacturerId != null ? ToManufacturer.Compile().Invoke(medicament.Manufacturer!) : null,
        Images = withImages
            ? medicament.Images
                .AsQueryable()
                .Select(f => f.FileInfo!)
                .Select(ToFileInfo)
                .ToList()
            : null,
    };


    public static Expression<Func<Entities.Medicament, Contract.MedicamentSimple>> ToMedicamentSimple =>
    (medicament) => new MedicamentSimple
    {
        Id = medicament.Id,
        Name = medicament.Name,
        ReleaseForm = medicament.ReleaseForm!.Name,
        Manufacturer = medicament.ManufacturerId != null ? medicament.Manufacturer!.Name : null,
        MainImage = medicament.Images.Any() ? medicament.Images
            .AsQueryable()
            .Select(f => f.FileInfo!)
            .Select(ToFileInfo)
            .FirstOrDefault() : null
    };


    public static Expression<Func<Entities.FileInfo, Contract.FileInfo>> ToFileInfo => fileInfo =>
        new Contract.FileInfo
        {
            Guid = fileInfo.Guid,
            OriginalName = fileInfo.OriginalName,
            FileExtension = fileInfo.Extension,
            Category = fileInfo.FileCategory,
            MediaType = fileInfo.MediaType,
            Size = fileInfo.Size,
            CreatedAt = fileInfo.CreatedAt,
        };


    public static Expression<Func<Entities.UserProfileContact, Contract.UserContact>> ToUserContact(bool withAvatar) => (userProfileContact) => new Contract.UserContact
    {
        UserProfileId = userProfileContact.UserProfileId,
        Profile = ToUserProfile(withAvatar).Compile().Invoke(userProfileContact.ContactProfile!),
        CustomName = userProfileContact.Name
    };


    public static Expression<Func<Entities.UserProfile, Contract.UserProfile>> ToUserProfile(bool withAvatar) => (userProfile) => new Contract.UserProfile
    {
        UserProfileId = userProfile.Id,
        UserIdentityGuid = Guid.Parse(userProfile.IdentityGuid),
        RealName = userProfile.RealName,
        DateOfBirth = userProfile.DateOfBirth,
        Sex = userProfile.Sex,
        Avatar = withAvatar && userProfile.AvatarGuid != null ? ToFileInfo.Compile().Invoke(userProfile.AvatarInfo!) : null,
    };


    public static Expression<Func<Entities.UserMedicament, Contract.UserMedicamentSimple>> ToUserMedicamentSimple => userMedicament => new Contract.UserMedicamentSimple
    {
        Id = userMedicament.Id,
        Name = userMedicament.Name,
        ReleaseForm = userMedicament.ReleaseForm,
        ManufacturerName = userMedicament.ManufacturerName,
        MainImage = userMedicament.Images
            .AsQueryable()
            .Select(i => i.FileInfo!)
            .Select(ToFileInfo)
            .FirstOrDefault(),
        BasicMedicament = userMedicament.BasedOnMedicamentId != null
            ? ToMedicamentSimple.Compile().Invoke(userMedicament.BasedOnMedicament!)
            : null
    };


    public static Expression<Func<Entities.UserMedicament, Contract.UserMedicamentExtended>> ToUserMedicamentExtended(bool withBasic, bool withImages) => (userMedicament) => new Contract.UserMedicamentExtended
    {
        Id = userMedicament.Id,
        Name = userMedicament.Name,
        ReleaseForm = userMedicament.Name,
        ManufacturerName = userMedicament.Name,
        Description = userMedicament.Description,
        Composition = userMedicament.Composition,
        Images = withImages ? userMedicament.Images
            .AsQueryable()
            .Select(i => i.FileInfo!)
            .Select(ToFileInfo)
            .ToList()
            : null,
        BasicMedicament = userMedicament.BasedOnMedicamentId != null
        ? ToMedicamentExtended(withImages).Compile().Invoke(userMedicament.BasedOnMedicament!)
        : null
    };

}