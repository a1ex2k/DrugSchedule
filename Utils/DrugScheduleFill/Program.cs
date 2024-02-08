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
    await SaveAsync(context, nameof(context.FileInfos), false);

    var forms = await JsonSerializer.DeserializeAsync<List<MedicamentReleaseForm>>(formStream);
    await context.AddRangeAsync(forms!);
    await SaveAsync(context, nameof(context.ReleaseForms));

    var manufacturers = await JsonSerializer.DeserializeAsync<List<Manufacturer>>(manufacturersStream);
    await context.AddRangeAsync(manufacturers!);
    await SaveAsync(context, nameof(context.Manufacturers));

    var medicaments = await JsonSerializer.DeserializeAsync<List<Medicament>>(medicamentsStream);
    await context.AddRangeAsync(medicaments!);
    await SaveAsync(context, nameof(context.Medicaments));

    var medicamentFiles = await JsonSerializer.DeserializeAsync<List<MedicamentFile>>(medicamentFilesStream);
    await context.AddRangeAsync(medicamentFiles!);
    await SaveAsync(context, nameof(context.MedicamentFiles));

    transaction.Commit();
}
catch
{
    await transaction.RollbackAsync();
    throw;
}



async Task SaveAsync(DbContext context, string table, bool hasIdentity = true)
{
    var databaseName = context.Database.GetDbConnection().Database;

    if (hasIdentity)
    {
        var command1 = $"SET IDENTITY_INSERT [{databaseName}].dbo.[{table}] ON;";
        await context.Database.ExecuteSqlRawAsync(command1);
    }
   
    await context.SaveChangesAsync();

    if (hasIdentity)
    {
        var command3 = $"SET IDENTITY_INSERT [{databaseName}].dbo.[{table}] OFF;";
        await context.Database.ExecuteSqlRawAsync(command3);
    }
}
