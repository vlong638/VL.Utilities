using System;
using System.Collections.Generic;
using System.Xml.Linq;
using VL.Common.Configurator.Objects.ConfigEntities;
using VL.Common.DAS.Objects;
using VL.ORMCodeGenerator.Utilities;

namespace VL.ORMCodeGenerator.Objects.Entities
{
    ///// <summary>
    ///// 生成相关参数
    ///// </summary>
    //public class GenerateConfig
    //{
    //    /// <summary>
    //    /// .pdm文件位置
    //    /// </summary>
    //    public string PDMFilePath { set; get; }
    //    /// <summary>
    //    /// 目标文件夹目录
    //    /// </summary>
    //    public string RootPath { set; get; }
    //    /// <summary>
    //    /// 目标命名空间
    //    /// </summary>
    //    public string RootNamespace { set; get; }
    //    /// <summary>
    //    /// 目标数据库类型
    //    /// </summary>
    //    public EDatabaseType DatabaseType { set; get; }
    //    /// <summary>
    //    /// 是否支持WCF
    //    /// </summary>
    //    public bool IsSupportWCF { set; get; }

    //    public GenerateConfig()
    //    {
    //    }
    //    public GenerateConfig(string contentString)
    //    {
    //        var result = contentString.SerializeClassFromString<GenerateConfig>();
    //        Init(result);
    //    }
    //    public void Init(GenerateConfig generateConfig)
    //    {
    //        this.PDMFilePath = generateConfig.PDMFilePath;
    //        this.RootPath = generateConfig.RootPath;
    //        this.RootNamespace = generateConfig.RootNamespace;
    //        this.DatabaseType = generateConfig.DatabaseType;
    //        this.IsSupportWCF = generateConfig.IsSupportWCF;
    //    }

    //    public override string ToString()
    //    {
    //        return this.SerializeClassToString();
    //    }

    //    #region Constraits

    //    //#region Entity
    //    //public string GetDirectoryPath()
    //    //{
    //    //    return Path.Combine(RootPath, EDirectoryNames.Objects.ToString(), EDirectoryNames.Entities.ToString());
    //    //}
    //    //public string GetEntityFilePath(string entityName)
    //    //{
    //    //    return Path.Combine(GetDirectoryPath(), entityName + ".cs");
    //    //}
    //    //public string GetEntityNamespace()
    //    //{
    //    //    return RootNamespace + "." + EDirectoryNames.Objects.ToString();// + "." + GeneratePaths.Entities.ToString();
    //    //}
    //    //#endregion

    //    //#region Enum
    //    //public string GetEnumDirectoryPath()
    //    //{
    //    //    return Path.Combine(RootPath, EDirectoryNames.Objects.ToString(), EDirectoryNames.Enums.ToString());
    //    //}
    //    //public string GetEnumFilePath(string enumName)
    //    //{
    //    //    return Path.Combine(GetEnumDirectoryPath(), enumName + ".cs");
    //    //}
    //    //public string GetEnumNamespace()
    //    //{
    //    //    return RootNamespace + "." + EDirectoryNames.Objects.ToString();// + "." + GeneratePaths.Enums.ToString();
    //    //}
    //    //#endregion

    //    #endregion
    ////}

    /// <summary>
    /// 生成相关参数
    /// </summary>
    public class GenerateConfigs : XMLConfigEntity
    {
        public List<GenerateConfig> Items = new List<GenerateConfig>();

        public GenerateConfigs(string fileName) : base(fileName)
        {
        }
        public GenerateConfigs(string fileName, string directoryPath) : base(fileName, directoryPath)
        {
        }

        public override IEnumerable<XElement> GetXElements()
        {
            List<XElement> elements = new List<XElement>();
            foreach (var item in Items)
            {
                XElement configItems = new XElement(nameof(GenerateConfig)
                    , new XAttribute(nameof(GenerateConfig.PDMFilePath), item.PDMFilePath)
                    , new XAttribute(nameof(GenerateConfig.RootPath), item.RootPath)
                    , new XAttribute(nameof(GenerateConfig.RootNamespace), item.RootNamespace)
                    , new XAttribute(nameof(GenerateConfig.DatabaseType), (int)item.DatabaseType)
                    , new XAttribute(nameof(GenerateConfig.IsSupportWCF), item.IsSupportWCF));
                elements.Add(configItems);
            }
            return elements;
        }
        protected override void Load(XDocument doc)
        {
            Items = new List<GenerateConfig>();
            var configItems = doc.Descendants(nameof(GenerateConfig));
            foreach (var configItem in configItems)
            {
                Items.Add(new GenerateConfig()
                {
                    PDMFilePath = configItem.Attribute(nameof(GenerateConfig.PDMFilePath)).Value,
                    RootPath = configItem.Attribute(nameof(GenerateConfig.RootPath)).Value,
                    RootNamespace = configItem.Attribute(nameof(GenerateConfig.RootNamespace)).Value,
                    DatabaseType = (EDatabaseType)Enum.Parse(typeof(EDatabaseType), configItem.Attribute(nameof(GenerateConfig.DatabaseType)).Value),
                    IsSupportWCF = Convert.ToBoolean(configItem.Attribute(nameof(GenerateConfig.IsSupportWCF)).Value.ToLower()),
                });
            }
        }
    }
    public class GenerateConfig
    {
        /// <summary>
        /// .pdm文件位置
        /// </summary>
        public string PDMFilePath { set; get; } = "";
        /// <summary>
        /// 目标文件夹目录
        /// </summary>
        public string RootPath { set; get; } = "";
        /// <summary>
        /// 目标命名空间
        /// </summary>
        public string RootNamespace { set; get; } = "";
        /// <summary>
        /// 目标数据库类型
        /// </summary>
        public EDatabaseType DatabaseType { set; get; } = EDatabaseType.None;
        /// <summary>
        /// 是否支持WCF
        /// </summary>
        public bool IsSupportWCF { set; get; } = false;
    }
}
