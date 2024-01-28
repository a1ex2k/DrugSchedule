namespace DrugSchedule.Api.FileAccessProvider;

public interface IFileAccessService
{
    bool Validate(FileAccessParams accessParams);

    FileAccessParams Generate(Guid fileGuid);
}