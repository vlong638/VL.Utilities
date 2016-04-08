//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using VL.ORMCodeGenerator.Objects.Entities;

//namespace VL.ORMCodeGenerator.Utilities
//{
//    class CacheHelper
//    {
//        public static string ConfigDirectoryPath = Path.Combine(Environment.CurrentDirectory, "Config");
//        public static string GenerateConfigPath = Path.Combine(ConfigDirectoryPath, nameof(GenerateConfig));

//        public static void SaveGenerateConfig(GenerateConfig config)
//        {
//            if (!Directory.Exists(ConfigDirectoryPath))
//            {
//                Directory.CreateDirectory(ConfigDirectoryPath);
//            }
//            File.WriteAllText(GenerateConfigPath, config.ToString());
//        }
//        public static GenerateConfig LoadGenerateConfig()
//        {
//            if (!File.Exists(GenerateConfigPath))
//            {
//                return new GenerateConfig(); ;
//            }
//            return new GenerateConfig(File.ReadAllText(GenerateConfigPath));
//        }
//    }
//}
