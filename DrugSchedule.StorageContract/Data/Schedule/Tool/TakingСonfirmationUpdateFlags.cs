using System.Diagnostics.Contracts;
using System;

namespace DrugSchedule.StorageContract.Data;

public class TakingСonfirmationUpdateFlags
{
    public bool Text { get; set; }

    public bool Images { get; set; }

    public bool ForDate { get; set; }

    public bool ForTime { get; set; }

    public bool ForTimeOfDay { get; set; }
}