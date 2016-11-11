﻿using System;
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
        Objects,
        Entities,
        DomainEntities,
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
                case EGenerateTargetType.DomainEntities:
                    return Path.Combine(rootPath, EDirectoryNames.Objects.ToString(), EGenerateTargetType.DomainEntities.ToString());
                case EGenerateTargetType.Entities:
                case EGenerateTargetType.EntityProperties:
                case EGenerateTargetType.EntityOperators:
                case EGenerateTargetType.References:
                case EGenerateTargetType.ReferenceFetchers:
                    return Path.Combine(rootPath, EDirectoryNames.Objects.ToString(), EGenerateTargetType.Entities.ToString(), tableName);
                case EGenerateTargetType.Enums:
                    return Path.Combine(rootPath, EDirectoryNames.Objects.ToString(), EGenerateTargetType.Enums.ToString());
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
                case EGenerateTargetType.DomainEntities:
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
                case EGenerateTargetType.DomainEntities:
                case EGenerateTargetType.EntityProperties:
                case EGenerateTargetType.EntityOperators:
                case EGenerateTargetType.References:
                case EGenerateTargetType.ReferenceFetchers:
                    return rootNamespace + "." + EGenerateTargetType.Objects.ToString() + "." + EGenerateTargetType.Entities.ToString();
                case EGenerateTargetType.Enums:
                    return rootNamespace + "." + EGenerateTargetType.Objects.ToString() + "." + EGenerateTargetType.Enums.ToString();
                default:
                    throw new NotImplementedException();
            }
        }
        /// <summary>啊
        /// ORM模型Entity的基类命名空间
        /// </summary>
        public static string NamespaceOfDAS = "VL.Common.DAS";
        public static string NamespaceOfORM = "VL.Common.ORM";
        public static string NamespaceOfProtocol = "VL.Common.Protocol";
        public static List<string> GetReferences(this EGenerateTargetType targetType, GenerateConfig config=null)
        {
            var result= new List<string>();
            if (config!=null&& config.IsSupportWCF)
            {
                result.Add("System.Runtime.Serialization");
            }
            switch (targetType)
            {
                case EGenerateTargetType.DomainEntities:
                    result.Add("System");
                    result.Add(NamespaceOfDAS);
                    result.Add(GetNamespace(EGenerateTargetType.Enums, config.RootNamespace));
                    result.Add(NamespaceOfORM);
                    result.Add(NamespaceOfProtocol);
                    break;
                case EGenerateTargetType.Entities:
                    result.Add("System");
                    result.Add("System.Collections.Generic");
                    result.Add("System.Data");
                    result.Add(NamespaceOfORM);
                    result.Add(GetNamespace(EGenerateTargetType.Enums, config.RootNamespace));
                    break;
                case EGenerateTargetType.EntityProperties:
                    result.Add(NamespaceOfORM);
                    break;
                case EGenerateTargetType.EntityOperators:
                    result.Add("System");
                    result.Add("System.Collections.Generic");
                    result.Add("System.Linq");
                    result.Add(NamespaceOfDAS);
                    result.Add(NamespaceOfORM);
                    result.Add(NamespaceOfProtocol);
                    break;
                case EGenerateTargetType.References:
                    result.Add("System");
                    result.Add("System.Collections.Generic");
                    result.Add(NamespaceOfORM);
                    break;
                case EGenerateTargetType.ReferenceFetchers:
                    result.Add("System");
                    result.Add("System.Collections.Generic");
                    result.Add(NamespaceOfDAS);
                    result.Add(NamespaceOfORM);
                    result.Add(NamespaceOfProtocol);
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
