using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenBIM.ServiceClass
{
    using System.IO;
    public static class ConstantClass
    {
        public static string ParentDirectory  = System.Reflection.Assembly.GetExecutingAssembly()
            .Location.Remove(System.Reflection.Assembly.GetExecutingAssembly()
            .Location.LastIndexOf(@"\", StringComparison.Ordinal));

        public static string CommonDirectory = Path.Combine(ParentDirectory, "AppDirrectory");

        public static string LogFilePath = Path.Combine(CommonDirectory, "logFile.txt");
    }
}
