using DrugSchedule.Services.Models;
using DrugSchedule.Services.Services.Abstractions;
using DrugSchedule.Services.Utils;
using DrugSchedule.StorageContract.Abstractions;
using DrugSchedule.StorageContract.Data;
using TakingScheduleExtended = DrugSchedule.Services.Models.TakingScheduleExtended;

namespace DrugSchedule.Services.Services;

public class ScheduleReadService : IScheduleReadService
{
    private readonly IScheduleSpecialRepository _scheduleSpecialRepository;
    private readonly IScheduleRepository _scheduleRepository;
    private readonly IScheduleConfirmationRepository _confirmationRepository;
    private readonly IScheduleRepeatRepository _repeatRepository;
    private readonly IScheduleShareRepository _shareRepository;
    private readonly IDownloadableFileConverter _downloadableFileConverter;
    private readonly ICurrentUserIdentifier _currentUserIdentifier;

    public ScheduleReadService(IScheduleSpecialRepository scheduleRepository, IScheduleRepository scheduleRepository1, IScheduleConfirmationRepository confirmationRepository, IScheduleRepeatRepository repeatRepository, IScheduleShareRepository shareRepository, IDownloadableFileConverter downloadableFileConverter, ICurrentUserIdentifier currentUserIdentifier)
    {
        _scheduleSpecialRepository = scheduleRepository;
        _scheduleRepository = scheduleRepository1;
        _confirmationRepository = confirmationRepository;
        _repeatRepository = repeatRepository;
        _shareRepository = shareRepository;
        _downloadableFileConverter = downloadableFileConverter;
        _currentUserIdentifier = currentUserIdentifier;
    }


    public async Task<OneOf<ScheduleSimpleCollection, InvalidInput>> SearchForScheduleAsync(string searchString, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(searchString) || searchString.Trim().Length < 3)
        {
            return new InvalidInput("Search value must be at least 3 not whitespace characters long");
        }

        var foundSchedules =
            await _scheduleSpecialRepository.SearchForOwnedOrSharedAsync(_currentUserIdentifier.UserProfileId,
                searchString, cancellationToken);

        return toScheduleSimpleCollection(foundSchedules);
    }


    public async Task<OneOf<ScheduleSimple, NotFound>> GetScheduleSimpleAsync(long id, CancellationToken cancellationToken = default)
    {
        var schedule = await _scheduleSpecialRepository.GetScheduleSimpleAsync(id,
            _currentUserIdentifier.UserProfileId, cancellationToken);

        if (schedule == null)
        {
            return new NotFound("Schedule was not found or current user doesn't have permissions to access");
        }

        return ToScheduleSimple(schedule!);
    }


    public async Task<ScheduleSimpleCollection> GetSchedulesSimpleAsync(TakingScheduleFilter filter, CancellationToken cancellationToken = default)
    {
        var schedules = await _scheduleSpecialRepository.GetSchedulesSimpleAsync(
            filter, _currentUserIdentifier.UserProfileId, cancellationToken);

        return toScheduleSimpleCollection(schedules);
    }


    public async Task<OneOf<TakingScheduleExtended, NotFound>> GetScheduleExtendedAsync(long id, CancellationToken cancellationToken = default)
    {
        var schedule = await _scheduleSpecialRepository.GetScheduleExtendedAsync(id,
            _currentUserIdentifier.UserProfileId, cancellationToken);

        if (schedule == null)
        {
            return new NotFound("Schedule was not found or current user doesn't have permissions to access");
        }

        return ToScheduleExtended(schedule!);
    }


    public Task<ScheduleExtendedCollection> GetSchedulesExtendedAsync(TakingScheduleFilter filter, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }


    public async Task<OneOf<TakingСonfirmationCollection, NotFound>> GetTakingConfirmationsAsync(long confirmationId, long scheduleId,
        CancellationToken cancellationToken = default)
    {
        var checkResult =
            await _scheduleSpecialRepository.GetOwnOrSharedSchedulesIdsAsync(scheduleId, _currentUserIdentifier.UserProfileId,
                cancellationToken);
        
        if (checkResult == null)
        {
            return new NotFound("Schedule was not found or current user doesn't have permissions to access");
        }

        var confirmation = await _scheduleSpecialRepository.GetTakingConfirmationAsync()


    }


    public async Task<TakingСonfirmationCollection> GetTakingConfirmationsAsync(TakingConfirmationFilter filter, CancellationToken cancellationToken = default)
    {
        var 
    }


    private TakingСonfirmation ToConfirmation(StorageContract.Data. schedule)
    {
        return new ScheduleSimple
        {
            Id = schedule.Id,
            MedicamentName = schedule.MedicamentName,
            MedicamentReleaseFormName = schedule.MedicamentReleaseFormName,
            ThumbnailUrl = _downloadableFileConverter.ToThumbLink(schedule.MedicamentImage,
                FileCategory.DrugConfirmation.IsPublic(), true),
            CreatedAt = schedule.CreatedAt,
            Enabled = schedule.Enabled
        };
    }

    private ScheduleSimple ToScheduleSimple(StorageContract.Data.TakingScheduleSimple schedule)
    {
        return new ScheduleSimple
        {
            Id = schedule.Id,
            MedicamentName = schedule.MedicamentName,
            MedicamentReleaseFormName = schedule.MedicamentReleaseFormName,
            ThumbnailUrl = _downloadableFileConverter.ToThumbLink(schedule.MedicamentImage,
                FileCategory.DrugConfirmation.IsPublic(), true),
            CreatedAt = schedule.CreatedAt,
            Enabled = schedule.Enabled
        };
    }

    private TakingScheduleExtended ToScheduleExtended(StorageContract.Data.TakingScheduleSimple schedule)
    {
        return new TakingScheduleExtended
        {
            Id = schedule.Id,
            ContactOwnerProfile = null,
            GlobalMedicament = null,
            UserMedicament = null,
            Information = null,
            CreatedAt = schedule.CreatedAt,
            Enabled = schedule.Enabled,
            ScheduleRepeats = null,
            ScheduleShares = null,
        };
    }

    private ScheduleSimpleCollection toScheduleSimpleCollection(List<StorageContract.Data.TakingScheduleSimple> scheduleList)
    {
        return new ScheduleSimpleCollection
        {
            Schedules = scheduleList.ConvertAll(ToScheduleSimple) 
        };
    }
}