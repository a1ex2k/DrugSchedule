namespace DrugSchedule.SqlServer.Data.Entities;

public class TakingRule
{
    public long Id { get; set; }

    public required string Text { get; set; }
}