using System.IO;
using VL.Common.DAS.Objects;
using VL.ORMCodeGenerator.Objects.Enums;
using VL.ORMCodeGenerator.Utilities;

namespace VL.ORMCodeGenerator.Objects.Entities
{
    /// <summary>
    /// 生成相关参数
    /// </summary>
    public class GenerateConfig
    {
        /// <summary>
        /// .pdm文件位置
        /// </summary>
        public string PDMFilePath { set; get; }
        /// <summary>
        /// 目标文件夹目录
        /// </summary>
        public string TargetDirectoryPath { set; get; }
        /// <summary>
        /// 目标命名空间
        /// </summary>
        public string TargetNamespace { set; get; }
        /// <summary>
        /// 目标数据库类型
        /// </summary>
        public EDatabaseType TargetDatabaseType { set; get; }
        /// <summary>
        /// 是否支持WCF
        /// </summary>
        public bool IsSupportWCF { set; get; }

        public GenerateConfig()
        {
        }
        public GenerateConfig(string contentString)
        {
            var result = contentString.SerializeClassFromString<GenerateConfig>();
            Init(result);
        }
        public void Init(GenerateConfig generateConfig)
        {
            this.PDMFilePath = generateConfig.PDMFilePath;
            this.TargetDirectoryPath = generateConfig.TargetDirectoryPath;
            this.TargetNamespace = generateConfig.TargetNamespace;
            this.TargetDatabaseType = generateConfig.TargetDatabaseType;
            this.IsSupportWCF = generateConfig.IsSupportWCF;
        }

        public override string ToString()
        {
            return this.SerializeClassToString();
        }

        #region Constraits

        #region Entity
        public string GetEntityDirectoryPath()
        {
            return Path.Combine(TargetDirectoryPath, EGeneratePath.Objects.ToString(), EGeneratePath.Entities.ToString());
        }
        public string GetEntityFilePath(string entityName)
        {
            return Path.Combine(GetEntityDirectoryPath(), entityName + ".cs");
        }
        public string GetEntityNamespace()
        {
            return TargetNamespace + "." + EGeneratePath.Objects.ToString();// + "." + GeneratePaths.Entities.ToString();
        }
        #endregion

        #region Enum
        public string GetEnumDirectoryPath()
        {
            return Path.Combine(TargetDirectoryPath, EGeneratePath.Objects.ToString(), EGeneratePath.Enums.ToString());
        }
        public string GetEnumFilePath(string enumName)
        {
            return Path.Combine(GetEnumDirectoryPath(), enumName + ".cs");
        }
        public string GetEnumNamespace()
        {
            return TargetNamespace + "." + EGeneratePath.Objects.ToString();// + "." + GeneratePaths.Enums.ToString();
        }
        #endregion

        #endregion
    }
}
