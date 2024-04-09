using CommandLine;

namespace DrugScheduleFill;

public class Options
{
    [Option('s', "source", Required = false, HelpText = "Path to sample data source directory. Omit to use current directory")]
    public string? SourceDirectory { get; set; }

    [Option('c', "connection", Required = true, HelpText = "Connection string to your MS SQL DrugSchedule database")]
    public string ConnectionString { get; set; } = default!;

    [Option('o', "output", Required = true, HelpText = "Output directory that used to store files uploaded to DrugSchedule WebAPI")]
    public string OutputDirectory { get; set; } = default!;
}