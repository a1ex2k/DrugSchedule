using System.Collections.Immutable;
using OneOf.Types;
using System.IO;
using DrugSchedule.Services.Models;
using DrugSchedule.Services.Utils;

namespace DrugSchedule.Services.Services;

public interface IFileProcessor
{
    public InvalidInput? GetInputFileErrors(InputFile inputFile, AwaitableFileParams awaitableFileParams);
}