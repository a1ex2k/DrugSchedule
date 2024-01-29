using System.Diagnostics;
using System.Reflection;
using DrugSchedule.BusinessLogic.Models;
using DrugSchedule.StorageContract.Data;
using Mapster;
using OneOf;

namespace Mapper;

public class MyRegister : ICodeGenerationRegister
{
    public void Register(CodeGenerationConfig config)
    {
        config.AdaptTo("[name]Dto")
            .ForTypes(typeof(ManufacturerCollection),
                      typeof(Manufacturer),
                      typeof(MedicamentSimple),
                      typeof(MedicamentSimpleCollection),
                      typeof(MedicamentExtended),
                      typeof(MedicamentReleaseForm),
                      typeof(ReleaseFormCollection),
                      typeof(MedicamentExtendedCollection),
                      typeof(MedicamentFilter),
                      typeof(MedicamentReleaseFormFilter),
                      typeof(StringFilter),
                      typeof(ManufacturerFilter),
                      typeof(MedicamentExtendedCollection)
                );
    }


}