using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using CommandLine;
using DrugScheduleFill;
using DrugScheduleFill.Models;
using Microsoft.EntityFrameworkCore;

const string MedicamentImageDirectory = "MedicamentImage";

Options options = default!;
Parser.Default.ParseArguments<Options>(args)
    .WithNotParsed(x => { Console.ReadLine(); Environment.Exit(0); })
    .WithParsed(o => { options = o; });

var sourceDirectory = options.SourceDirectory ?? Environment.CurrentDirectory;

await using var medicamentsStream = new FileStream(Path.Combine(sourceDirectory, "Medicaments.json"), FileMode.Open, FileAccess.Read, FileShare.Read);
await using var medicamentFilesStream = new FileStream(Path.Combine(sourceDirectory, "MedicamentFiles.json"), FileMode.Open, FileAccess.Read, FileShare.Read);
await using var formStream = new FileStream(Path.Combine(sourceDirectory, "ReleaseForms.json"), FileMode.Open, FileAccess.Read, FileShare.Read);
await using var fileInfosStream = new FileStream(Path.Combine(sourceDirectory, "FileInfos.json"), FileMode.Open, FileAccess.Read, FileShare.Read);
await using var manufacturersStream = new FileStream(Path.Combine(sourceDirectory, "Manufacturers.json"), FileMode.Open, FileAccess.Read, FileShare.Read);

var newPath = Path.Combine(options.OutputDirectory, MedicamentImageDirectory);
var fromPath = Path.Combine(sourceDirectory, MedicamentImageDirectory);
await using var context = new Context(options.ConnectionString);
await using var transaction = await context.Database.BeginTransactionAsync();

Console.WriteLine("Started");

try
{
    var fileInfos = await JsonSerializer.DeserializeAsync<List<DrugScheduleFill.Models.FileInfo>>(fileInfosStream);
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

    Directory.CreateDirectory(Path.Combine(options.OutputDirectory, MedicamentImageDirectory));
    var allFiles = Directory.GetFiles(fromPath, "*.*", SearchOption.TopDirectoryOnly);
    foreach (string filePath in allFiles)
    {
        File.Copy(filePath, newPath.Replace(fromPath, newPath), true);
    }
    
    transaction.Commit();
}
catch
{
    await transaction.RollbackAsync();
    throw;
}

Console.WriteLine("Finished successfully");


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
