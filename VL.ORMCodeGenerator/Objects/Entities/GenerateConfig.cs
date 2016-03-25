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
        public string RootPath { set; get; }
        /// <summary>
        /// 目标命名空间
        /// </summary>
        public string RootNamespace { set; get; }
        /// <summary>
        /// 目标数据库类型
        /// </summary>
        public EDatabaseType DatabaseType { set; get; }
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
            this.RootPath = generateConfig.RootPath;
            this.RootNamespace = generateConfig.RootNamespace;
            this.DatabaseType = generateConfig.DatabaseType;
            this.IsSupportWCF = generateConfig.IsSupportWCF;
        }

        public override string ToString()
        {
            return this.SerializeClassToString();
        }

        #region Constraits

        //#region Entity
        //public string GetDirectoryPath()
        //{
        //    return Path.Combine(RootPath, EDirectoryNames.Objects.ToString(), EDirectoryNames.Entities.ToString());
        //}
        //public string GetEntityFilePath(string entityName)
        //{
        //    return Path.Combine(GetDirectoryPath(), entityName + ".cs");
        //}
        //public string GetEntityNamespace()
        //{
        //    return RootNamespace + "." + EDirectoryNames.Objects.ToString();// + "." + GeneratePaths.Entities.ToString();
        //}
        //#endregion

        //#region Enum
        //public string GetEnumDirectoryPath()
        //{
        //    return Path.Combine(RootPath, EDirectoryNames.Objects.ToString(), EDirectoryNames.Enums.ToString());
        //}
        //public string GetEnumFilePath(string enumName)
        //{
        //    return Path.Combine(GetEnumDirectoryPath(), enumName + ".cs");
        //}
        //public string GetEnumNamespace()
        //{
        //    return RootNamespace + "." + EDirectoryNames.Objects.ToString();// + "." + GeneratePaths.Enums.ToString();
        //}
        //#endregion

        #endregion
    }
}
