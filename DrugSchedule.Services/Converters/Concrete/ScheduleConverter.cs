using DrugSchedule.Services.Models;
using DrugSchedule.Services.Models.Schedule;
using DrugSchedule.Services.Utils;
using DrugSchedule.StorageContract.Data;
using TakingScheduleExtended = DrugSchedule.StorageContract.Data.TakingScheduleExtended;
using UserContactSimple = DrugSchedule.StorageContract.Data.UserContactSimple;

namespace DrugSchedule.Services.Converters;

internal class ScheduleConverter : IScheduleConverter
{
    private readonly IDownloadableFileConverter _downloadableFileConverter;
    private readonly IGlobalMedicamentConverter _globalMedicamentConverter;
    private readonly IUserMedicamentConverter _userMedicamentConverter;
    private readonly IUserContactConverter _contactConverter;

    public ScheduleConverter(IDownloadableFileConverter downloadableFileConverter, IGlobalMedicamentConverter globalMedicamentConverter, IUserMedicamentConverter userMedicamentConverter, IUserContactConverter contactConverter)
    {
        _downloadableFileConverter = downloadableFileConverter;
        _globalMedicamentConverter = globalMedicamentConverter;
        _userMedicamentConverter = userMedicamentConverter;
        _contactConverter = contactConverter;
    }

    public Models.TakingСonfirmation ToConfirmation(StorageContract.Data.TakingСonfirmation confirmation)
    {
        return new Models.TakingСonfirmation
        {
            Id = confirmation.Id,
            RepeatId = confirmation.RepeatId,
            CreatedAt = confirmation.CreatedAt,
            Images = new FileCollection
            {
                Files = _downloadableFileConverter.ToFilesModels(confirmation.Images, FileCategory.DrugConfirmation.IsPublic())
            },
            Text = null
        };
    }

    public ScheduleSimple ToScheduleSimple(TakingScheduleSimple schedule)
    {
        return new ScheduleSimple
        {
            Id = schedule.Id,
            MedicamentName = schedule.MedicamentName,
            MedicamentReleaseFormName = schedule.MedicamentReleaseFormName,
            ThumbnailUrl = _downloadableFileConverter.ToThumbLink(schedule.MedicamentImage,
                FileCategory.DrugConfirmation.IsPublic(), true),
            CreatedAt = schedule.CreatedAt,
            Enabled = schedule.Enabled,
            OwnerContact = schedule.ContactOwner == null ? null : 
                _contactConverter.ToContactSimple(schedule.ContactOwner)
        };
    }

    public Models.TakingScheduleExtended ToScheduleExtended(TakingScheduleExtended schedule)
    {
        return new Models.TakingScheduleExtended
        {
            Id = schedule.Id,
            ContactOwner = schedule.ContactOwner == null ? null :
                _contactConverter.ToContactSimple(schedule.ContactOwner),
            GlobalMedicament = schedule.GlobalMedicament == null ? null : _globalMedicamentConverter.ToMedicamentSimple(schedule.GlobalMedicament),
            UserMedicament = schedule.UserMedicament == null ? null : _userMedicamentConverter.ToUserMedicamentSimple(schedule.UserMedicament),
            Information = schedule.Information,
            CreatedAt = schedule.CreatedAt,
            Enabled = schedule.Enabled,
            ScheduleRepeats = schedule.ScheduleRepeats,
            ScheduleShares = schedule.ContactOwner == null ? schedule.ScheduleShares : null,
        };
    }

    public ScheduleSimpleCollection ToScheduleSimpleCollection(List<TakingScheduleSimple> scheduleList)
    {
        return new ScheduleSimpleCollection
        {
            Schedules = scheduleList.ConvertAll(ToScheduleSimple)
        };
    }

    public ScheduleExtendedCollection ToScheduleExtendedCollection(List<TakingScheduleExtended> scheduleList)
    {
        return new ScheduleExtendedCollection
        {
            Schedules = scheduleList.ConvertAll(ToScheduleExtended)
        };
    }

    public TakingСonfirmationCollection ToСonfirmationCollection(List<StorageContract.Data.TakingСonfirmation> confirmationsList)
    {
        return new TakingСonfirmationCollection
        {
            Confirmations = confirmationsList.ConvertAll(ToConfirmation)
        };
    }
}