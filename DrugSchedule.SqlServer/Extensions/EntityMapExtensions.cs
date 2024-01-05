namespace DrugSchedule.SqlServer.Extensions;

public static class EntityMapExtensions
{
    public static Contract.UserIdentity ToContractUserIdentity(this Microsoft.AspNetCore.Identity.IdentityUser identityUser)
    {
        return new Contract.UserIdentity
        {
            Guid = Guid.Parse(identityUser.Id),
            Username = identityUser.UserName,
            Email = identityUser.Email,
            IsEmailConfirmed = identityUser.EmailConfirmed
        };
    }

    public static Contract.FileInfo ToContractFileInfo(this Entities.FileInfo fileInfo)
    {
        return new Contract.FileInfo
        {
            Guid = fileInfo.Guid,
            FileName = fileInfo.FileName,
            ContentType = fileInfo.ContentType,
            Size = fileInfo.Size,
            CreatedAt = fileInfo.CreatedAt,
        };
    }

    public static Contract.UserProfile ToContractUserProfile(this Entities.UserProfile userProfile)
    {
        return new Contract.UserProfile
        {
            UserProfileId = userProfile.Id,
            UserIdentityGuid = userProfile.UserGuid,
            RealName = userProfile.RealName,
            DateOfBirth = userProfile.DateOfBirth,
            Image = userProfile.ImageFileInfo?.ToContractFileInfo()
        };
    }

    public static Contract.Manufacturer ToContractManufacturer(this Entities.Manufacturer manufacturer)
    {
        return new Contract.Manufacturer
        {
            Id = manufacturer.Id,
            Name = manufacturer.Name,
            AdditionalInfo = manufacturer.AdditionalInfo,
        };
    }

    public static Contract.MedicamentReleaseForm ToContractReleaseForm(this Entities.MedicamentReleaseForm releaseForm)
    {
        return new Contract.MedicamentReleaseForm
        {
            Id = releaseForm.Id,
            Name = releaseForm.Name
        };
    }

    public static Contract.Medicament ToContractMedicament(this Entities.Medicament medicament)
    {
        return new Contract.Medicament
        {
            Id = medicament.Id,
            Name = medicament.Name,
            PackQuantity = medicament.PackQuantity,
            Dosage = medicament.Dosage,
            ReleaseForm = medicament.ReleaseForm?.ToContractReleaseForm(),
            Manufacturer = medicament.Manufacturer?.ToContractManufacturer(),
            Images = medicament.Images.Select(f => f.ToContractFileInfo()).ToList()
        };
    }
}