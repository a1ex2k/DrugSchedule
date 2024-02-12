using System.Collections.Immutable;
using DrugSchedule.BusinessLogic.Models;
using DrugSchedule.BusinessLogic.Utils;
using OneOf.Types;
using System.IO;

namespace DrugSchedule.BusinessLogic.Services;

public interface IFileProcessor
{
    public InvalidInput? GetInputFileErrors(InputFile inputFile, AwaitableFileParams awaitableFileParams);
}