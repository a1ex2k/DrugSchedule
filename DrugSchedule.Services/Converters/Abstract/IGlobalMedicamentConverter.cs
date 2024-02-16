using DrugSchedule.Services.Models;
using DrugSchedule.StorageContract.Data;

namespace DrugSchedule.Services.Converters;

public interface IGlobalMedicamentConverter
{
    ManufacturerCollection ToManufacturerCollection(List<Manufacturer> manufacturers);
    MedicamentExtendedModel ToMedicamentExtended(MedicamentExtended medicament);
    MedicamentExtendedCollection ToMedicamentExtendedCollection(List<MedicamentExtended> medicaments);
    MedicamentSimpleModel ToMedicamentSimple(MedicamentSimple medicament);
    MedicamentSimpleCollection ToMedicamentSimpleCollection(List<MedicamentSimple> medicaments);
    ReleaseFormCollection ToReleaseFormCollection(List<MedicamentReleaseForm> forms);
}