using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Mapper
{
    internal class Program
    {

        public static void Main()
        {
            foreach (var VARIABLE in GetClasses("DrugSchedule.BusinessLogic.Models"))
            {
                Console.WriteLine(VARIABLE);
            }

        }
        static List<string> GetClasses(string nameSpace)
        {
            Assembly asm = Assembly.GetAssembly(typeof(DrugSchedule.BusinessLogic.Models.FileData));

            List<string> namespacelist = new List<string>();
            List<string> classlist = new List<string>();

            foreach (Type type in asm.GetTypes())
            {
                if (type.Namespace == nameSpace)
                    namespacelist.Add(type.Name);
            }

            foreach (string classname in namespacelist)
                classlist.Add(classname);

            return classlist;
        }
    }


}
