using System;
using System.Collections.Generic;
using System.IO;
using VL.ORMCodeGenerator.Objects.Constraits;

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
                    return rootNamespace + EGenerateTargetType.Entities.ToString();
                case EGenerateTargetType.Enums:
                    return rootNamespace + EGenerateTargetType.Enums.ToString();
                default:
                    throw new NotImplementedException();
            }
        }
        /// <summary>
        /// ORM模型Entity的基类命名空间
        /// </summary>
        public static string NamespaceOfDbSessionObjects = "VL.Common.DbSession.Objects";
        public static string NamespaceOfORMObjects = "VL.Common.ORM.Objects";
        public static string NamespaceOfORMQueryBuilders = "VL.ORM.DbOperateLib.Utilities.QueryBuilders";
        public static string NamespaceOfORMQueryOperators = "VL.ORM.DbOperateLib.Utilities.QueryOperators";
        public static List<string> GetReferences(this EGenerateTargetType targetType, bool isSupportWCF = false)
        {
            var result= new List<string>();
            if (isSupportWCF)
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
                    break;
                case EGenerateTargetType.EntityProperties:
                    result.Add(NamespaceOfORMObjects);
                    break;
                case EGenerateTargetType.EntityOperators:
                    result.Add("System.Collections.Generic");
                    result.Add("System.Linq");
                    result.Add("System.DbSession.Objects");
                    result.Add(NamespaceOfORMQueryBuilders);
                    result.Add(NamespaceOfORMQueryOperators);
                    break;
                case EGenerateTargetType.References:
                    result.Add("System");
                    result.Add("System.Collections.Generic");
                    result.Add(NamespaceOfORMObjects);
                    break;
                case EGenerateTargetType.ReferenceFetchers:
                    result.Add("System.Collections.Generic");
                    result.Add(NamespaceOfDbSessionObjects);
                    result.Add(NamespaceOfORMQueryBuilders);
                    result.Add(NamespaceOfORMQueryOperators);
                    break;
                case EGenerateTargetType.Enums:
                    result.Add("System.Runtime.Serialization");
                    break;
                default:
                    throw new NotImplementedException();
            }
            return result;
        }
    }
}
