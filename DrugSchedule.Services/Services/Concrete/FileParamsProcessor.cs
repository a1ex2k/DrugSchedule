using DrugSchedule.Services.Errors;
using DrugSchedule.Services.Models;
using DrugSchedule.Services.Utils;
using MimeDetective;

namespace DrugSchedule.Services.Services;

public class FileParamsProcessor : IFileProcessor
{
    private readonly ContentInspector _inspector = new ContentInspectorBuilder()
    {
        Definitions = MimeDetective.Definitions.Default.All()
    }.Build();


    public InvalidInput? GetInputFileErrors(InputFile inputFile, AwaitableFileParams awaitableFileParams)
    {
        var error = new InvalidInput();
        if (string.IsNullOrWhiteSpace(inputFile.MediaType))
        {
            error.Add(ErrorMessages.MimeTypeNotSet);
        }

        if (string.IsNullOrWhiteSpace(inputFile.NameWithExtension))
        {
            error.Add(ErrorMessages.FilenameNotSet);
        }

        if (!inputFile.Stream.CanRead || inputFile.Stream.Length == 0)
        {
            error.Add(ErrorMessages.StreamCannotBeRead);
        }

        if (inputFile.Stream.Length > awaitableFileParams.MaxSize)
        {
            error.Add($"File size must be less than {awaitableFileParams} bytes");
        }

        if (!CheckMetadata(inputFile))
        {
            error.Add($"Media type doesn't match real file metadata");
        }

        if (!CheckExtension(inputFile, awaitableFileParams.FileExtensions))
        {
            error.Add($"File must have one of the following extensions: {string.Join(", ", awaitableFileParams.FileExtensions)}. And must have corresponding media type");
        }

        return error.HasMessages ? error : null;
    }

    private bool CheckMetadata(InputFile inputFile)
    {
        var match = _inspector.Inspect(inputFile.Stream).FirstOrDefault();
        if (match == null)
        {
            return false;
        }

        if (!string.Equals(match.Definition.File.MimeType, inputFile.MediaType, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        return true;
    }

    private bool CheckExtension(InputFile inputFile, params string[] extensions)
    {
        foreach (var ext in extensions)
        {
            if (inputFile.NameWithExtension.EndsWith(ext, StringComparison.OrdinalIgnoreCase))
                return true;
        }

        return false;
    }
}