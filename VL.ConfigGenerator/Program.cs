using System;
using System.Collections.Generic;
using VL.Common.Configurator.Objects.ConfigEntities;
using VL.Common.Testing.Objects;

namespace VL.ConfigGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            List<ConsoloMenuItem> consoloMenuItems = new List<ConsoloMenuItem>();
            consoloMenuItems.Add(new ConsoloMenuItem("SaveConnection", "生成DbConnectionsConfigEntity配置文件", () =>
            {
                KeyValuesConfigEntity configEntity = new KeyValuesConfigEntity("DbConnectionsConfigEntity.config");
                configEntity.Items.Add(new KeyValueConfigEntity("@ConnectionName", "@ConnectingString"));
                configEntity.Save();
                return true;

                //DbConnectionsConfigEntity configEntity = new DbConnectionsConfigEntity(nameof(DbConnectionsConfigEntity)+".config", @"D:\GitProjects\VL.Utilities\VL.ConfigGenerator\Configs");
                //configEntity.DbConnectionConfigs.Add(new DbConnectionConfigEntity("@Name", "@ConnectingString"));
                //configEntity.Save();
                //return true;
            }));
            consoloMenuItems.Add(new ConsoloMenuItem("LoadConnection", "生成DbConnectionsConfigEntity配置文件", () =>
            {
                KeyValuesConfigEntity configEntity = new KeyValuesConfigEntity("DbConnectionsConfigEntity.config");
                configEntity.Load();
                Console.WriteLine(configEntity.ToDisplayFormat());
                return true;

                //DbConnectionsConfigEntity configEntity = new DbConnectionsConfigEntity(nameof(DbConnectionsConfigEntity) + ".config", @"D:\GitProjects\VL.Utilities\VL.ConfigGenerator\Configs");
                //configEntity.Load();
                //Console.WriteLine(configEntity.ToDisplayFormat());
                //return true;
            }));
            consoloMenuItems.WaitForOperation();
        }
    }
}
