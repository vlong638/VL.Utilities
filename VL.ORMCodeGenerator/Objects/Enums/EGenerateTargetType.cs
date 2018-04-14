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
        Objects,
        Entities,
        DomainEntities,
        EntityProperties,
        EntityOperators,
        References,
        ReferenceFetchers,
        Enums,
        Manual,
    }
    public static class EGenerateTargetTypeEx
    {
        public static string GetDirectoryPath(this EGenerateTargetType targetType, string rootPath, string tableName)
        {
            switch (targetType)
            {
                case EGenerateTargetType.DomainEntities:
                    return Path.Combine(rootPath, EDirectoryNames.Business.ToString(), "Domain");
                case EGenerateTargetType.EntityOperators:
                case EGenerateTargetType.ReferenceFetchers:
                    return Path.Combine(rootPath, EDirectoryNames.Business.ToString(),"DAL", tableName);
                case EGenerateTargetType.Entities:
                case EGenerateTargetType.EntityProperties:
                case EGenerateTargetType.References:
                    return Path.Combine(rootPath, EDirectoryNames.Objects.ToString(), EGenerateTargetType.Entities.ToString(), tableName);
                case EGenerateTargetType.Enums:
                    return Path.Combine(rootPath, EDirectoryNames.Objects.ToString(), EGenerateTargetType.Enums.ToString());
                case EGenerateTargetType.Manual:
                    return Path.Combine(rootPath, EDirectoryNames.Objects.ToString(), EGenerateTargetType.Manual.ToString(), tableName);
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
                    filePath = Path.Combine(directoryPath, tableName);
                    break;
                case EGenerateTargetType.DomainEntities:
                    filePath = Path.Combine(directoryPath, tableName + CGenerate.FileNameSuffixOfDomain);
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
        public const string RootCommonNamespace = "VL.Common.Core.Object";
        public static string GetNamespace(this EGenerateTargetType targetType, string rootNamespace)
        {
            switch (targetType)
            {
                case EGenerateTargetType.Entities:
                case EGenerateTargetType.EntityProperties:
                case EGenerateTargetType.References:
                case EGenerateTargetType.Enums:
                    //return RootCommonNamespace + "." + rootNamespace;
                case EGenerateTargetType.DomainEntities:
                case EGenerateTargetType.EntityOperators:
                case EGenerateTargetType.ReferenceFetchers:
                    return rootNamespace + ".DomainModel";
                    //return rootNamespace + "." + "Business";
                //return rootNamespace + "." + EGenerateTargetType.Objects.ToString() + "." + EGenerateTargetType.Entities.ToString();
                //return rootNamespace + "." + EGenerateTargetType.Objects.ToString() + "." + EGenerateTargetType.Enums.ToString();
                default:
                    throw new NotImplementedException();
            }
        }
        /// <summary>啊
        /// ORM模型Entity的基类命名空间
        /// </summary>
        public static string NamespaceOfDAS = "VL.Common.Core.DAS";
        public static string NamespaceOfORM = "VL.Common.Core.ORM";
        public static string NamespaceOfProtocol = "VL.Common.Core.Protocol";
        public static List<string> GetReferences(this EGenerateTargetType targetType, GenerateConfig config)
        {
            var result = new List<string>();
            switch (targetType)
            {
                case EGenerateTargetType.DomainEntities:
                    result.Add("System");
                    result.Add("System.Collections.Generic");
                    result.Add("System.Linq");
                    result.Add(NamespaceOfDAS);
                    result.Add(NamespaceOfORM);
                    //result.Add(NamespaceOfProtocol);
                    //result.Add(EGenerateTargetType.Entities.GetNamespace(config.RootNamespace));
                    //result.Add(GetNamespace(EGenerateTargetType.Enums, config.RootNamespace));
                    break;
                case EGenerateTargetType.Entities:
                    result.Add("System");
                    result.Add("System.Collections.Generic");
                    result.Add("System.Data");
                    //if (config.IsSupportWCF)
                    //{
                    //    result.Add("System.Runtime.Serialization");
                    //}
                    result.Add("System.Runtime.Serialization");
                    result.Add(NamespaceOfORM);
                    break;
                case EGenerateTargetType.EntityProperties:
                    result.Add("System");
                    result.Add(NamespaceOfORM);
                    break;
                case EGenerateTargetType.EntityOperators:
                    result.Add("System");
                    result.Add("System.Collections.Generic");
                    result.Add("System.Linq");
                    result.Add(NamespaceOfDAS);
                    result.Add(NamespaceOfORM);
                    //result.Add(NamespaceOfProtocol);
                    //result.Add(EGenerateTargetType.Entities.GetNamespace(config.RootNamespace));
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
                    //result.Add(NamespaceOfProtocol);
                    //result.Add(EGenerateTargetType.Entities.GetNamespace(config.RootNamespace));
                    break;
                case EGenerateTargetType.Enums:
                    if (config.IsSupportWCF)
                    {
                        result.Add("System.Runtime.Serialization");
                    }
                    break;
                case EGenerateTargetType.Manual:
                    break;
                default:
                    throw new NotImplementedException();
            } 
            return result;
        }
    }
}
