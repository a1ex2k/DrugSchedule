using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.BusinessLogic.Models;

public class ManufacturerCollection
{
    public required List<Manufacturer> Manufacturers { get; set; }
}