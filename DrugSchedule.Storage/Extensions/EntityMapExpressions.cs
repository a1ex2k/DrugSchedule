using System.Linq.Expressions;
using DrugSchedule.Storage.Data;
using DrugSchedule.Storage.Data.Entities;
using DrugSchedule.StorageContract.Data;
using Microsoft.AspNetCore.Identity;

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
            ? medicament.Files
                .AsQueryable()
                .Select(medicamentFile => medicamentFile.FileInfo!)
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
        ManufacturerName = medicament.ManufacturerId == null ? null : medicament.Manufacturer!.Name,
        MainImage = medicament.Files.Any() ? medicament.Files
            .AsQueryable()
            .Select(medicamentFile => medicamentFile.FileInfo!)
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
            HasThumbnail = fileInfo.HasThumbnail
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
        MainImage = userMedicament.Files
            .AsQueryable()
            .Select(userMedicamentFile => userMedicamentFile.FileInfo!)
            .Select(ToFileInfo)
            .FirstOrDefault(),
    };


    public static Expression<Func<Entities.UserMedicament, Contract.UserMedicamentExtended>> ToUserMedicamentExtended(bool withImages) => (userMedicament) => new Contract.UserMedicamentExtended
    {
        Id = userMedicament.Id,
        Name = userMedicament.Name,
        ReleaseForm = userMedicament.Name,
        ManufacturerName = userMedicament.Name,
        Description = userMedicament.Description,
        Composition = userMedicament.Composition,
        Images = withImages ? userMedicament.Files
            .AsQueryable()
            .Select(userMedicamentFile => userMedicamentFile.FileInfo!)
            .Select(ToFileInfo)
            .ToList()
            : null,
        BasicMedicamentId = userMedicament.BasedOnMedicamentId
    };


    public static Expression<Func<Entities.UserMedicament, Contract.UserMedicament>> ToUserMedicament
        => userMedicament => new Contract.UserMedicament
        {
            BasicMedicamentId = userMedicament.BasedOnMedicamentId,
            Name = userMedicament.Name,
            Description = userMedicament.Description,
            Composition = userMedicament.Composition,
            ReleaseForm = userMedicament.ReleaseForm,
            ManufacturerName = userMedicament.ManufacturerName,
            UserProfileId = userMedicament.UserProfileId,
            ImageGuids = userMedicament.Files.Select(userMedicamentFile => userMedicamentFile.FileGuid).ToList(),
        };


    public static Expression<Func<IdentityUser, Contract.UserIdentity>> ToIdentity
        => identityUser => new Contract.UserIdentity()
        {
            Guid = Guid.Parse(identityUser.Id),
            Username = identityUser.UserName,
            Email = identityUser.Email,
            IsEmailConfirmed = identityUser.EmailConfirmed
        };


    public static Expression<Func<MedicamentTakingSchedule, Contract.TakingSchedulePlain>> ToSchedulePlain
        => s => new Contract.TakingSchedulePlain
        {
            Id = s.Id,
            UserProfileId = s.UserProfileId,
            GlobalMedicamentId = s.GlobalMedicamentId,
            UserMedicamentId = s.UserMedicamentId,
            Information = s.Information,
            CreatedAt = s.CreatedAt,
            Enabled = s.Enabled,
        };


    public static Expression<Func<ScheduleRepeat, Contract.ScheduleRepeatPlain>> ToScheduleRepeatPlain
        => s => new Contract.ScheduleRepeatPlain
        {
            Id = s.Id,
            BeginDate = s.BeginDate,
            Time = s.Time,
            TimeOfDay = s.TimeOfDay,
            RepeatDayOfWeek = s.RepeatDayOfWeek,
            EndDate = s.EndDate,
            MedicamentTakingScheduleId = s.MedicamentTakingScheduleId,
            TakingRule = s.TakingRule,
        };


    public static Expression<Func<Entities.ScheduleShare, Contract.ScheduleSharePlain>> ToScheduleSharePlain
        => s => new Contract.ScheduleSharePlain
        {
            Id = s.Id,
            MedicamentTakingScheduleId = s.MedicamentTakingScheduleId,
            ShareUserProfileId = s.ShareWithContact!.ContactProfileId,
            Comment = s.Comment,
        };


    public static Expression<Func<Entities.TakingСonfirmation, Contract.TakingСonfirmationPlain>> ToScheduleConfirmationPlain
        => s => new Contract.TakingСonfirmationPlain
        {
            Id = s.Id,
            CreatedAt = s.CreatedAt,
            ImagesGuids = s.Files.Select(c => c.FileGuid)
                .ToList(),
            Text = s.Text,
            ScheduleRepeatId = s.ScheduleRepeatId,
            ForDate = s.ForDate,
            ForTime = s.ForTime,
            ForTimeOfDay = s.ForTimeOfDay,
        };


    public static Expression<Func<Entities.TakingСonfirmation, Contract.TakingСonfirmation>> ToScheduleConfirmation
        => s => new Contract.TakingСonfirmation
        {
            Id = s.Id,
            RepeatId = s.ScheduleRepeatId,
            CreatedAt = s.CreatedAt,
            Images = s.Files
                .Select(c => c.FileInfo!).AsQueryable()
                .Select(ToFileInfo).ToList(),
            Text = s.Text,
            ForDate = s.ForDate,
            ForTime = s.ForTime,
            ForTimeOfDay = s.ForTimeOfDay,
        };


    public static Expression<Func<Entities.MedicamentTakingSchedule, Contract.TakingScheduleSimple>> ToScheduleSimpleOwned
        => s => new Contract.TakingScheduleSimple
        {
            Id = s.Id,
            MedicamentName = s.UserMedicamentId == null
                ? s.GlobalMedicament!.Name 
                : s.UserMedicament!.Name,
            MedicamentReleaseFormName = s.UserMedicamentId == null
                ? s.GlobalMedicament!.ReleaseForm!.Name
                : s.UserMedicament!.ReleaseForm,
            MedicamentImage = s.GlobalMedicament!.Files.Select(f => f.FileInfo!)
                .AsQueryable()
                    .Union(s.UserMedicament!.Files.Select(f => f.FileInfo!))
                    .Select(ToFileInfo)
                    .FirstOrDefault(),
            CreatedAt = s.CreatedAt,
            Enabled = s.Enabled
        };


    public static Expression<Func<Entities.MedicamentTakingSchedule, Contract.TakingScheduleSimple>> ToScheduleSimple(DrugScheduleContext context, long userId)
        => s => new Contract.TakingScheduleSimple
        {
            Id = s.Id,
            MedicamentName = s.UserMedicamentId == null
                ? s.GlobalMedicament!.Name
                : s.UserMedicament!.Name,
            MedicamentReleaseFormName = s.UserMedicamentId == null
                ? s.GlobalMedicament!.ReleaseForm!.Name
                : s.UserMedicament!.ReleaseForm,
            MedicamentImage = s.GlobalMedicament!.Files.Select(f => f.FileInfo!)
                .AsQueryable()
                .Union(s.UserMedicament!.Files.Select(f => f.FileInfo!))
                .Select(ToFileInfo)
                .FirstOrDefault(),
            ContactOwner = s.UserProfileId == userId ? null : context.UserProfileContacts
                .Where(c => c.UserProfileId == userId && c.ContactProfileId == s.UserProfileId)
                .Select(ToContactSimple(context))
                .FirstOrDefault(),
            CreatedAt = s.CreatedAt,
            Enabled = s.Enabled
        };



    public static Expression<Func<Entities.MedicamentTakingSchedule, Contract.TakingScheduleExtended>> ToScheduleExtended(DrugScheduleContext context, long userId)
        => s => new Contract.TakingScheduleExtended
        {
            Id = s.Id,
            GlobalMedicament = s.GlobalMedicamentId == null ? null : ToMedicamentSimple.Compile().Invoke(s.GlobalMedicament!),
            UserMedicament = s.UserMedicamentId == null ? null : ToUserMedicamentSimple.Compile().Invoke(s.UserMedicament!),
            Information = s.Information,
            CreatedAt = s.CreatedAt,
            Enabled = s.Enabled,
            ScheduleRepeats = s.RepeatSchedules
                .AsQueryable()
                .Select(ToScheduleRepeatPlain)
                .ToList(),
            ContactOwner = s.UserProfileId == userId ? null : context.UserProfileContacts
                .Where(c => c.UserProfileId == userId && c.ContactProfileId == s.UserProfileId)
                .Select(ToContactSimple(context))
                .FirstOrDefault(),
            ScheduleShares = s.UserProfileId != userId ? null : s.ScheduleShares
                .AsQueryable()
                .Select(ToScheduleShare(context))
                .ToList()
        };


    public static Expression<Func<Entities.ScheduleShare, Contract.ScheduleShareExtended>> ToScheduleShare(DrugScheduleContext context)
        => s => new Contract.ScheduleShareExtended
        {
            Id = s.Id,
            UserContact = ToContactSimple(context).Compile().Invoke(s.ShareWithContact!),
            Comment = s.Comment,
        };


    public static Expression<Func<Entities.UserProfileContact, Contract.UserContactSimple>> ToContactSimple(DrugScheduleContext context)
        => c => new Contract.UserContactSimple
        {
            ContactProfileId = c.ContactProfileId,
            CustomName = c.CustomName,
            Avatar = c.ContactProfile!.AvatarGuid == null
                ? null
                : EntityMapExpressions.ToFileInfo.Compile().Invoke(c.ContactProfile!.AvatarInfo!),
            IsCommon = context.UserProfileContacts
                .Any(c2 => c2.UserProfileId == c.ContactProfileId
                           && c2.ContactProfileId == c.UserProfileId)
        };
}