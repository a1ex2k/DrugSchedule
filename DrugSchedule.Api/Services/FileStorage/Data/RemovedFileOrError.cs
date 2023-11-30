using DrugSchedule.Api.Services.FileStorage.Data;
using OneOf;

namespace DrugSchedule.Api.Services.FileStorage;

[GenerateOneOf]
public partial class RemovedFileOrError : OneOfBase<RemovedFile, FileServiceError> { }