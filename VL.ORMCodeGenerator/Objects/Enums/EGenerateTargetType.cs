using System;
using System.Collections.Generic;
using System.IO;
using VL.ORMCodeGenerator.Objects.Constraits;
using VL.ORMCodeGenerator.Objects.Entities;

namespace VL.ORMCodeGenerator.Objects.Enums
{
    /// <summary>
    /// 生成目标
    /// </summary>
    public enum EGenerateTargetType
    {
        Entities,
        EntityProperties,
        EntityOperators,
        References,
        ReferenceFetchers,
        Enums,
    }
    public static class EGenerateTargetTypeEx
    {
        public static string GetDirectoryPath(this EGenerateTargetType targetType, string rootPath, string tableName)
        {
            switch (targetType)
            {
                case EGenerateTargetType.Entities:
                case EGenerateTargetType.EntityProperties:
                case EGenerateTargetType.EntityOperators:
                case EGenerateTargetType.References:
                case EGenerateTargetType.ReferenceFetchers:
                    return Path.Combine(rootPath, EDirectoryNames.Objects.ToString(), EDirectoryNames.Entities.ToString(), tableName);
                case EGenerateTargetType.Enums:
                    return Path.Combine(rootPath, EDirectoryNames.Objects.ToString(), EDirectoryNames.Enums.ToString());
                default:
                    throw new NotImplementedException();
            }
        }
        public static string GetFilePath(this EGenerateTargetType targetType, string directoryPath, string tableName)
        {
            string filePath;
            switch (targetType)
            {
                case EGenerateTargetType.Entities:
                    filePath= Path.Combine(directoryPath, tableName);
                    break;
                case EGenerateTargetType.EntityProperties:
                    filePath = Path.Combine(directoryPath, tableName + CGenerate.FileNameSuffixOfProperties);
                    break;
                case EGenerateTargetType.EntityOperators:
                    filePath = Path.Combine(directoryPath, tableName + CGenerate.FileNameSuffixOfOperator);
                    break;
                case EGenerateTargetType.References:
                    filePath = Path.Combine(directoryPath, tableName + CGenerate.FileNameSuffixOfReference);
                    break;
                case EGenerateTargetType.ReferenceFetchers:
                    filePath = Path.Combine(directoryPath, tableName + CGenerate.FileNameSuffixOfFetcher);
                    break;
                case EGenerateTargetType.Enums:
                    filePath = Path.Combine(directoryPath, tableName);
                    break;
                default:
                    throw new NotImplementedException();
            }
            return filePath + ".cs";
        }
        public static string GetNamespace(this EGenerateTargetType targetType, string rootNamespace)
        {
            switch (targetType)
            {
                case EGenerateTargetType.Entities:
                case EGenerateTargetType.EntityProperties:
                case EGenerateTargetType.EntityOperators:
                case EGenerateTargetType.References:
                case EGenerateTargetType.ReferenceFetchers:
                    return rootNamespace + "." + EGenerateTargetType.Entities.ToString();
                case EGenerateTargetType.Enums:
                    return rootNamespace + "." + EGenerateTargetType.Enums.ToString();
                default:
                    throw new NotImplementedException();
            }
        }
        /// <summary>
        /// ORM模型Entity的基类命名空间
        /// </summary>
        public static string NamespaceOfDASObjects = "VL.Common.DAS.Objects";
        public static string NamespaceOfORMObjects = "VL.Common.ORM.Objects";
        public static string NamespaceOfORMQueryBuilders = "VL.Common.ORM.Utilities.QueryBuilders";
        public static string NamespaceOfORMQueryOperators = "VL.Common.ORM.Utilities.QueryOperators";
        public static List<string> GetReferences(this EGenerateTargetType targetType, GenerateConfig config=null)
        {
            var result= new List<string>();
            if (config!=null&& config.IsSupportWCF)
            {
                result.Add("System.Runtime.Serialization");
            }
            switch (targetType)
            {
                case EGenerateTargetType.Entities:
                    result.Add("System");
                    result.Add("System.Collections.Generic");
                    result.Add("System.Data");
                    result.Add(NamespaceOfORMObjects);
                    result.Add(config.RootNamespace + ".Enums");
                    break;
                case EGenerateTargetType.EntityProperties:
                    result.Add(NamespaceOfORMObjects);
                    break;
                case EGenerateTargetType.EntityOperators:
                    result.Add("System.Collections.Generic");
                    result.Add("System.Linq");
                    result.Add(NamespaceOfDASObjects);
                    result.Add(NamespaceOfORMQueryBuilders);
                    result.Add(NamespaceOfORMQueryOperators);
                    break;
                case EGenerateTargetType.References:
                    result.Add("System");
                    result.Add("System.Collections.Generic");
                    result.Add(NamespaceOfORMObjects);
                    break;
                case EGenerateTargetType.ReferenceFetchers:
                    result.Add("System");
                    result.Add("System.Collections.Generic");
                    result.Add(NamespaceOfDASObjects);
                    result.Add(NamespaceOfORMQueryBuilders);
                    result.Add(NamespaceOfORMQueryOperators);
                    break;
                case EGenerateTargetType.Enums:
                    break;
                default:
                    throw new NotImplementedException();
            }
            return result;
        }
    }
}
