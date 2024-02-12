using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.Services.Models;

public class ManufacturerCollection
{
    public required List<Manufacturer> Manufacturers { get; set; }
}