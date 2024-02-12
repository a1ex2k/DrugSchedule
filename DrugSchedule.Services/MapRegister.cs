using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Mapster;

namespace DrugSchedule.Services
{
    public class MapRegister : ICodeGenerationRegister
    {
        public void Register(CodeGenerationConfig config)
        {
            config.AdaptTo("[name]Dto")
                .ForAllTypesInNamespace(Assembly.GetExecutingAssembly(), nameof(DrugSchedule.Services.Models));

        }
    }
}
