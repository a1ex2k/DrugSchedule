using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using DrugScheduleFill;
using Microsoft.EntityFrameworkCore;


var connectionString = args[0];
var folder = AppDomain.CurrentDomain.BaseDirectory;

await using var medicamentsStream = new FileStream(Path.Combine(folder, "Medicaments.json"), FileMode.Open, FileAccess.Read, FileShare.Read);
await using var medicamentFilesStream = new FileStream(Path.Combine(folder, "MedicamentFiles.json"), FileMode.Open, FileAccess.Read, FileShare.Read);
await using var formStream = new FileStream(Path.Combine(folder, "ReleaseForms.json"), FileMode.Open, FileAccess.Read, FileShare.Read);
await using var fileInfosStream = new FileStream(Path.Combine(folder, "FileInfos.json"), FileMode.Open, FileAccess.Read, FileShare.Read);
await using var manufacturersStream = new FileStream(Path.Combine(folder, "Manufacturers.json"), FileMode.Open, FileAccess.Read, FileShare.Read);

await using var context = new Context(connectionString);
await using var transaction = await context.Database.BeginTransactionAsync();

try
{
    
    var fileInfos = await JsonSerializer.DeserializeAsync<List<DrugScheduleFill.FileInfo>>(fileInfosStream);
    await context.AddRangeAsync(fileInfos!);
    await ClearTableAndSaveAsync(context, nameof(context.FileInfos));

    var forms = await JsonSerializer.DeserializeAsync<List<MedicamentReleaseForm>>(formStream);
    await context.AddRangeAsync(forms!);
    await ClearTableAndSaveAsync(context, nameof(context.ReleaseForms));

    var manufacturers = await JsonSerializer.DeserializeAsync<List<Manufacturer>>(manufacturersStream);
    await context.AddRangeAsync(manufacturers!);
    await ClearTableAndSaveAsync(context, nameof(context.Manufacturers));

    var medicaments = await JsonSerializer.DeserializeAsync<List<Medicament>>(medicamentsStream);
    await context.AddRangeAsync(medicaments!);
    await ClearTableAndSaveAsync(context, nameof(context.Medicaments));

    var medicamentFiles = await JsonSerializer.DeserializeAsync<List<MedicamentFile>>(medicamentFilesStream);
    await context.AddRangeAsync(medicamentFiles!);
    await ClearTableAndSaveAsync(context, nameof(context.MedicamentFiles));

    transaction.Commit();
}
catch
{
    await transaction.RollbackAsync();
    throw;
}



async Task ClearTableAndSaveAsync(DbContext context, string table)
{
    var databaseName = context.Database.GetDbConnection().Database;
    FormattableString command1 = $"SET IDENTITY_INSERT {databaseName}.{table} ON;";
    await context.Database.ExecuteSqlAsync(command1);
    FormattableString command2 = $"delete from {databaseName}.{table};";
    await context.Database.ExecuteSqlAsync(command2);
    await context.SaveChangesAsync();
    FormattableString command3 = $"SET IDENTITY_INSERT {databaseName}.{table} OFF;";
    await context.Database.ExecuteSqlAsync(command3);
}
