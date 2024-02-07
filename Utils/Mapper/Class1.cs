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
            .ForTypes(typeof(NewUserMedicament),
                      typeof(UserMedicamentExtendedModel),
                      typeof(UserMedicamentSimpleModel),
                      typeof(MedicamentSimpleCollection),
                      typeof(UserMedicamentExtendedCollection),
                      typeof(UserMedicamentUpdate),
                      typeof(UserMedicamentFilter)
                );
    }


}