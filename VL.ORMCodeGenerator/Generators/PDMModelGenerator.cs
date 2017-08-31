using PdPDM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VL.Common.Core.ORM;
using VL.ORMCodeGenerator.Objects.Constraits;
using VL.ORMCodeGenerator.Objects.Entities;
using VL.ORMCodeGenerator.Objects.Enums;
using VL.ORMCodeGenerator.Utilities;

namespace VL.ORMCodeGenerator.Generators
{
    /// <summary>
    /// 基于PDM的MSSQL代码生成器
    /// </summary>
    public class PDMModelGenerator : IPDMGenerator
    {
        public override void Generate(GenerateConfig config, Model model)
        {
            bool result = true;
            //生成 Table
            result = result && GenerateTables(config, model.Tables);
            //生成 Reference
            result = result && GenerateReferences(config, model);
            //生成 Enum
            result = result && GenerateEnums(config, model.Tables);
        }

        #region References
        /// <summary>
        ///  生成 Reference 模板
        /// </summary>
        /// <param name="config"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        protected override bool GenerateReferences(GenerateConfig config, Model model)
        {
            bool result = true;
            //目标是生成TableReference关系部分
            foreach (Table table in model.Tables)
            {
                if (!table.Name.StartsWith(CGenerate.PDMNameNotationOfTable))
                {
                    continue;
                }
                result = result && GenerateReference(config, table.Name, model.References);
                result = result && GenerateFetcher(config, table.Name, model.References);
            }
            return result;
        }
        bool GenerateReference(GenerateConfig config, string tableName, ObjectCol references)
        {
            bool result = true;
            bool hasReference = false;
            //目标是生成TableReference关系部分
            foreach (Reference reference in references)
            {
                string parentTableName = reference.ParentTable.GetAttributeText("Name");
                string childTableName = reference.ChildTable.GetAttributeText("Name");
                if (parentTableName == tableName)
                {
                    hasReference = true;
                    break;
                }
                if (parentTableName.StartsWith(CGenerate.PDMNameNotationOfRelationMapper) && childTableName == tableName)
                {
                    hasReference = true;
                    break;
                }
            }
            if (hasReference)
            {
                //代码生成
                string targetDirectoryPath = EGenerateTargetType.References.GetDirectoryPath(config.RootPath, tableName);
                string targetFilePath = EGenerateTargetType.References.GetFilePath(targetDirectoryPath, tableName);
                string targetNamespace = EGenerateTargetType.References.GetNamespace(config.RootNamespace);
                StringBuilder sb = new StringBuilder();
                sb.AppendUsings(EGenerateTargetType.References.GetReferences(config));
                sb.AppendLine();
                sb.AppendNameSpace(targetNamespace, () =>
                {
                    sb.AppendClass(false, "public partial", tableName, " : " + nameof(IPDMTBase), () =>
                    {
                        foreach (Reference reference in references)
                        {
                            string parentTableName = reference.ParentTable.GetAttributeText("Name");
                            string childTableName = reference.ChildTable.GetAttributeText("Name");
                            //关系A-B n..m表示A对B的拥有关系
                            if (parentTableName == tableName)
                            {
                                switch (reference.Cardinality)
                                {
                                    case "0..1":
                                        sb.AppendProperties(true, false, childTableName);
                                        break;
                                    case "0..*":
                                        sb.AppendProperties(true, true, childTableName);
                                        break;
                                    case "1..1":
                                        sb.AppendProperties(false, false, childTableName);
                                        break;
                                    case "1..*":
                                        sb.AppendProperties(false, true, childTableName);
                                        break;
                                    default:
                                        break;
                                }
                            }
                            //关系A-B-C B-A,B-C发现存在中间关联时传递关联
                            if (parentTableName.StartsWith(CGenerate.PDMNameNotationOfRelationMapper) && childTableName == tableName)
                            {
                                foreach (Reference subReference in references)
                                {
                                    string subParentTableName = subReference.ParentTable.GetAttributeText("Name");
                                    string subChildTableName = subReference.ChildTable.GetAttributeText("Name");
                                    if (subParentTableName == parentTableName && subChildTableName != childTableName)
                                    {
                                        sb.AppendProperties(true, true, subChildTableName);
                                    }
                                }
                            }
                        }
                    });
                });
                //输出代码
                if (!Directory.Exists(targetDirectoryPath))
                {
                    Directory.CreateDirectory(targetDirectoryPath);
                }
                File.WriteAllText(targetFilePath, sb.ToString());
            }
            return result;
        }
        bool GenerateFetcher(GenerateConfig config, string tableName, ObjectCol references)
        {
            bool result = true;
            bool hasReference = false;
            //目标是生成TableReference关系部分
            foreach (Reference reference in references)
            {
                string parentTableName = reference.ParentTable.GetAttributeText("Name");
                string childTableName = reference.ChildTable.GetAttributeText("Name");
                if (parentTableName == tableName)
                {
                    hasReference = true;
                    break;
                }
                if (parentTableName.StartsWith(CGenerate.PDMNameNotationOfRelationMapper) && childTableName == tableName)
                {
                    hasReference = true;
                    break;
                }
            }
            if (hasReference)
            {
                //代码生成
                string targetDirectoryPath = EGenerateTargetType.ReferenceFetchers.GetDirectoryPath(config.RootPath, tableName);
                string targetFilePath = EGenerateTargetType.ReferenceFetchers.GetFilePath(targetDirectoryPath, tableName);
                string targetNamespace = EGenerateTargetType.ReferenceFetchers.GetNamespace(config.RootNamespace);
                StringBuilder sb = new StringBuilder();
                sb.AppendUsings(EGenerateTargetType.ReferenceFetchers.GetReferences(config));
                sb.AppendLine();
                sb.AppendNameSpace(targetNamespace, () =>
                {
                    sb.AppendClass(false, "public static partial", CGenerate.ClassNameOfEntityFetcher, "", () =>
                    {
                        sb.AppendMethods(() =>
                        {
                            foreach (Reference reference in references)
                            {
                                string parentTableName = reference.ParentTable.GetAttributeText("Name");
                                string childTableName = reference.ChildTable.GetAttributeText("Name");
                                string parentTableToParameter = parentTableName.ToParameterFormat();
                                string childTableToProperty = childTableName.ToPropertyFormat();
                                //关系A-B n..m表示A对B的拥有关系
                                if (parentTableName == tableName)
                                {
                                    switch (reference.Cardinality)
                                    {
                                        case "0..1"://1...1或0..1关联以直联形式存在, 比如xia挂靠在了class1下,那么xia有着class1的Id
                                            sb.AppendMethod("public static bool", "Fetch" + childTableName, "this " + parentTableName + " " + parentTableToParameter + ", DbSession session", () =>
                                            {
                                                sb.AppendLine(CGenerate.ContentLS + "var query = session.GetDbQueryBuilder();");
                                                sb.AppendLine(CGenerate.ContentLS + "SelectBuilder builder = new SelectBuilder();");
                                                var childTable = reference.ChildTable as Table;
                                                foreach (Column column in childTable.Columns)
                                                {
                                                    if (column.Primary)
                                                    {
                                                        sb.AppendLine(CGenerate.ContentLS + "builder.ComponentWhere.Add(new ComponentValueOfWhere(" + childTableName + "Properties." + column.Name + ", " + parentTableToParameter + "." + column.Name + ", LocateType.Equal));");
                                                    }
                                                }
                                                sb.AppendLine(CGenerate.ContentLS + "query.SelectBuilders.Add(builder);");
                                                sb.AppendLine(CGenerate.ContentLS + parentTableToParameter + "." + childTableToProperty + " = session.GetQueryOperator()."
                                                    + nameof(IDbQueryOperator.Select) + "<" + childTableName + ">(query);");
                                                sb.AppendLine(CGenerate.ContentLS + "return " + parentTableToParameter + "." + childTableToProperty + " != null;");
                                            });
                                            break;
                                        case "1..1":
                                            sb.AppendMethod("public static bool", "Fetch" + childTableName, "this " + parentTableName + " " + parentTableToParameter + ", DbSession session", () =>
                                            {
                                                sb.AppendLine(CGenerate.ContentLS + "var query = session.GetDbQueryBuilder();");
                                                sb.AppendLine(CGenerate.ContentLS + "SelectBuilder builder = new SelectBuilder();");
                                                //0715修正 TEvent.FetchTask()时TEvent可能没有TaskId.可以以EventId嵌套Subselect的形式获取
                                                var parentTable = reference.ParentTable as Table;
                                                var childTable = reference.ChildTable as Table;
                                                Column parentIdentifier = null, childIdentifier = null;
                                                foreach (Column column in parentTable.Columns)
                                                {
                                                    if (column.Primary)
                                                    {
                                                        parentIdentifier = column;
                                                    }
                                                }
                                                foreach (Column column in childTable.Columns)
                                                {
                                                    if (column.Primary)
                                                    {
                                                        childIdentifier = column;
                                                    }
                                                }
                                                sb.AppendLine(CGenerate.ContentLS + "if (" + parentTableToParameter + "." + childIdentifier.Name + " == " + childIdentifier.GetEmptyValue() + ")");
                                                sb.AppendLine(CGenerate.ContentLS + "{");
                                                sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + "var subselect = new SelectBuilder();");
                                                sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + "subselect.TableName = nameof(" + parentTableName + ");");
                                                sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + "subselect.ComponentSelect.Add(" + parentTableName + "Properties." + childIdentifier.Name + ");");
                                                sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + "subselect.ComponentWhere.Add(new ComponentValueOfWhere(" + parentTableName + "Properties." + parentIdentifier.Name + ", " + parentTableToParameter + "." + parentIdentifier.Name + ", LocateType.Equal));");
                                                sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + "builder.ComponentWhere.Add(new ComponentValueOfWhere(" + childTableName + "Properties." + childIdentifier.Name + ", subselect, LocateType.Equal));");
                                                sb.AppendLine(CGenerate.ContentLS + "}");
                                                sb.AppendLine(CGenerate.ContentLS + "else");
                                                sb.AppendLine(CGenerate.ContentLS + "{");
                                                sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + "builder.ComponentWhere.Add(new ComponentValueOfWhere(" + childTableName + "Properties." + childIdentifier.Name + ", " + parentTableToParameter + "." + childIdentifier.Name + ", LocateType.Equal));");
                                                sb.AppendLine(CGenerate.ContentLS + "}");
                                                sb.AppendLine(CGenerate.ContentLS + "query.SelectBuilders.Add(builder);");
                                                sb.AppendLine(CGenerate.ContentLS + parentTableToParameter + "." + childTableToProperty + " = session.GetQueryOperator()."
                                                    + nameof(IDbQueryOperator.Select) + "<" + childTableName + ">(query);");
                                                sb.AppendLine(CGenerate.ContentLS + "if (" + parentTableToParameter + "." + childTableToProperty + " == null)");
                                                sb.AppendLine(CGenerate.ContentLS + "{");
                                                sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + "throw new NotImplementedException(string.Format(\"1..* 关联未查询到匹配数据, Parent:{0}; Child: {1}\", nameof(" + parentTableName + "), nameof(" + childTableName + ")));");
                                                sb.AppendLine(CGenerate.ContentLS + "}");
                                                sb.AppendLine(CGenerate.ContentLS + "return true;");
                                            });
                                            break;
                                        case "0..*"://1..*或0..*关联则是以反向关联形式存在 即Class对应的Students 从Student表获取ClassId对应的一批数据
                                            sb.AppendMethod("public static bool", "Fetch" + childTableToProperty.ToPluralFormat(), "this " + parentTableName + " " + parentTableToParameter + ", DbSession session", () =>
                                            {
                                                sb.AppendLine(CGenerate.ContentLS + "var query = session.GetDbQueryBuilder();");
                                                sb.AppendLine(CGenerate.ContentLS + "SelectBuilder builder = new SelectBuilder();");
                                                var parentTable = reference.ParentTable as Table;
                                                foreach (Column column in parentTable.Columns)
                                                {
                                                    if (column.Primary)
                                                    {
                                                        sb.AppendLine(CGenerate.ContentLS + "builder.ComponentWhere.Add(new ComponentValueOfWhere(" + childTableName + "Properties." + column.Name + ", " + parentTableToParameter + "." + column.Name + ", LocateType.Equal));");
                                                    }
                                                }
                                                sb.AppendLine(CGenerate.ContentLS + "query.SelectBuilders.Add(builder);");
                                                sb.AppendLine(CGenerate.ContentLS + parentTableToParameter + "." + childTableName.ToPropertyFormat().ToPluralFormat() + " = session.GetQueryOperator()."
                                                         + nameof(IDbQueryOperator.SelectAll) + "<" + childTableName + ">(query);");
                                                sb.AppendLine(CGenerate.ContentLS + "return " + parentTableToParameter + "." + childTableName.ToPropertyFormat().ToPluralFormat() + ".Count > 0;");
                                            });
                                            break;
                                        case "1..*":
                                            sb.AppendSummary(new List<string>() {
                                                "return false when fetch.Count()==0",
                                                "return true when fetch.Count()>0" });
                                            sb.AppendMethod("public static bool", "Fetch" + childTableToProperty.ToPluralFormat(), "this " + parentTableName + " " + parentTableToParameter + ", DbSession session", () =>
                                            {
                                                sb.AppendLine(CGenerate.ContentLS + "var query = session.GetDbQueryBuilder();");
                                                sb.AppendLine(CGenerate.ContentLS + "SelectBuilder builder = new SelectBuilder();");
                                                var parentTable = reference.ParentTable as Table;
                                                foreach (Column column in parentTable.Columns)
                                                {
                                                    if (column.Primary)
                                                    {
                                                        sb.AppendLine(CGenerate.ContentLS + "builder.ComponentWhere.Add(new ComponentValueOfWhere(" + childTableName + "Properties." + column.Name + ", " + parentTableToParameter + "." + column.Name + ", LocateType.Equal));");
                                                    }
                                                }
                                                sb.AppendLine(CGenerate.ContentLS + "query.SelectBuilders.Add(builder);");
                                                sb.AppendLine(CGenerate.ContentLS + parentTableToParameter + "." + childTableName.ToPropertyFormat().ToPluralFormat() + " = session.GetQueryOperator()."
                                                    + nameof(IDbQueryOperator.SelectAll) + "<" + childTableName + ">(query);");
                                                sb.AppendLine(CGenerate.ContentLS + "if (" + parentTableToParameter + "." + childTableName.ToPropertyFormat().ToPluralFormat() + ".Count == 0)");
                                                sb.AppendLine(CGenerate.ContentLS + "{");
                                                sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + "throw new NotImplementedException(string.Format(\"1..* 关联未查询到匹配数据, Parent:{0}; Child: {1}\", nameof(" + parentTableName + "), nameof(" + childTableName + ")));");
                                                sb.AppendLine(CGenerate.ContentLS + "}");
                                                sb.AppendLine(CGenerate.ContentLS + "return true;");
                                            });
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                //关系A-B-C B-A,B-C发现存在中间关联时传递关联,传递关联只支持0..*关联
                                if (parentTableName.StartsWith(CGenerate.PDMNameNotationOfRelationMapper) && childTableName == tableName)
                                {
                                    foreach (Reference subReference in references)
                                    {
                                        string subParentTableName = subReference.ParentTable.GetAttributeText("Name");
                                        string subChildTableName = subReference.ChildTable.GetAttributeText("Name");
                                        string subChildTableToProperty = subChildTableName.ToPropertyFormat();
                                        if (subParentTableName == parentTableName && subChildTableName != childTableName)
                                        {
                                            sb.AppendMethod("public static bool", "Fetch" + subChildTableName.ToPluralFormat(), "this " + childTableName + " " + childTableName.ToParameterFormat() + ", DbSession session", () =>
                                            {
                                                //A<-B->C(Student) 0..*
                                                //childTableName Student
                                                //parentTableName R
                                                //subChildTableName TCourses
                                                sb.AppendLine(CGenerate.ContentLS + "var query = session.GetDbQueryBuilder();");
                                                sb.AppendLine(CGenerate.ContentLS + "SelectBuilder builder = new SelectBuilder();");
                                                sb.AppendLine(CGenerate.ContentLS + "SelectBuilder subSelect = new SelectBuilder();");
                                                var subChildTable = subReference.ChildTable as Table;
                                                foreach (Column column in subChildTable.Columns)
                                                {
                                                    //TODO 多主键
                                                    if (column.Primary)
                                                    {

                                                        sb.AppendLine(CGenerate.ContentLS + "subSelect.ComponentSelect.Add(" + subChildTableName + "Properties." + column.Name + ");");
                                                        break;
                                                    }
                                                }
                                                var childTable = reference.ChildTable as Table;
                                                foreach (Column column in childTable.Columns)
                                                {
                                                    //TODO 多主键
                                                    if (column.Primary)
                                                    {
                                                        sb.AppendLine(CGenerate.ContentLS + "subSelect.ComponentWhere.Add(new ComponentValueOfWhere(" + parentTableName + "Properties." + column.Name + ", " + childTableName.ToParameterFormat() + "." + column.Name + ", LocateType.Equal));");
                                                        break;
                                                    }
                                                }
                                                ////FieldAlias
                                                //var subChildTable = subReference.ChildTable as Table;
                                                //foreach (Column column in subChildTable.Columns)
                                                //{
                                                //    //TODO 多主键
                                                //    if (column.Primary)
                                                //    {
                                                //        sb.AppendLine(GConstraints.ContentLS + "builder.ComponentWhere.Add(new PDMDbPropertyOperateValue(" + subChildTableName + "Properties." + column.Name + ", LocateType.In, subSelect");
                                                //        break;
                                                //    }
                                                //}
                                                ////Wheres
                                                //var childTable = reference.ChildTable as Table;
                                                //foreach (Column column in childTable.Columns)
                                                //{
                                                //    //TODO 多主键
                                                //    if (column.Primary)
                                                //    {
                                                //        sb.AppendLine(GConstraints.ContentLS + GConstraints.TabLS + ", new PDMDbPropertyOperateValue(" + parentTableName + "Properties." + column.Name + ", LocateType.Equal, " + childTableName.ToParameterFormat() + "." + column.Name + ")));");
                                                //        break;
                                                //    }
                                                //}
                                                sb.AppendLine(CGenerate.ContentLS + "query.SelectBuilders.Add(builder);");
                                                sb.AppendLine(CGenerate.ContentLS + childTableName.ToParameterFormat() + "." + subChildTableToProperty.ToPluralFormat() + " = session.GetQueryOperator().SelectAll<" + subChildTableName + ">(query);");
                                                sb.AppendLine(CGenerate.ContentLS + "return " + childTableName.ToParameterFormat() + "." + subChildTableToProperty.ToPluralFormat() + ".Count == 0;");
                                            });
                                            //TODO
                                            ////传递关联只支持0..*关联
                                            //sb.AppendMethod("public static bool", "Fetch" + subChildTableToProperty.ToPluralFormat(), "this " + parentTableToParameter + ", DbSession session", () =>
                                            //{
                                            //    sb.AppendLine(GConstraints.ContentLS + "var query = "+nameof(IDbQueryBuilder)+".GetDbQueryBuilder(session);");
                                            //    sb.AppendLine(GConstraints.ContentLS + "query.SelectBuilder.ComponentFieldAliases.FieldAliases.AddRange(" + childTableName + ".Properties);");
                                            //    var childTable = reference.ChildTable as Table;
                                            //    foreach (Column column in childTable.Columns)
                                            //    {
                                            //        if (column.Primary)
                                            //        {
                                            //            sb.AppendLine(GConstraints.ContentLS + "query.Selectbuilder.ComponentWhere.Add(new PDMDbPropertyOperateValue(" + childTableName + "Properties." + column.Name + ", " + parentTableToParameter + "." + column.Name + ", LocateType.Equal));");
                                            //        }
                                            //    }
                                            //    sb.AppendLine(GConstraints.ContentLS + parentTableToParameter + "." + childTableName.ToPropertyFormat().ToPluralFormat() + " = session.GetQueryOperator()."
                                            //        + nameof(IORMProvider.SelectAll) + "<" + childTableName + ">(query);");
                                            //    sb.AppendLine(GConstraints.ContentLS + "return " + parentTableToParameter + "." + childTableName.ToPropertyFormat().ToPluralFormat() + ".Count > 0;");
                                            //});
                                        }
                                    }
                                }
                            }
                        });
                    });
                });
                //输出代码
                if (!Directory.Exists(targetDirectoryPath))
                {
                    Directory.CreateDirectory(targetDirectoryPath);
                }
                File.WriteAllText(targetFilePath, sb.ToString());
            }
            return result;
        }
        #endregion

        #region Tables
        /// <summary>
        /// 生成 Table 模板
        /// </summary>
        /// <param name="config"></param>
        /// <param name="tables"></param>
        /// <returns></returns>
        protected override bool GenerateTables(GenerateConfig config, ObjectCol tables)
        {
            bool result = true;
            ////EntityBase
            //result = result && GenerateTableBase(config);
            //Entities
            foreach (Table table in tables)
            {
                if (table.Name.StartsWith(CGenerate.PDMNameNotationOfEnum))
                {
                    continue;
                }
                if (table.Name.StartsWith(CGenerate.PDMNameNotationOfRelationMapper)
                    || table.Name.StartsWith(CGenerate.PDMNameNotationOfTable))
                {
                    result = result && GenerateEntity(config, table);
                    result = result && GenerateDomainEntity(config, table);
                    result = result && GenerateEntityOperator(config, table, LocateType.C | LocateType.R | LocateType.U | LocateType.D);
                    result = result && GenerateEntityProperties(config, table);
                }
            }
            return result;
        }
        [Flags]
        public enum LocateType
        {
            C = 1,
            R = 2,
            U = 4,
            D = 8,
        }
        bool GenerateEntity(GenerateConfig config, Table table)
        {
            //代码生成
            string targetDirectoryPath = EGenerateTargetType.Entities.GetDirectoryPath(config.RootPath, table.Name);
            string targetFilePath = EGenerateTargetType.Entities.GetFilePath(targetDirectoryPath, table.Name);
            string targetNamespace = EGenerateTargetType.Entities.GetNamespace(config.RootNamespace);
            StringBuilder sb = new StringBuilder();
            sb.AppendUsings(EGenerateTargetType.Entities.GetReferences(config));
            sb.AppendLine();
            CodeBuilder.AppendNameSpace(sb, targetNamespace, () =>
             {
                 sb.AppendClass(config.IsSupportWCF, "public partial", table.Name, " : " + nameof(IPDMTBase), () =>
                 {
                     AppendClassContent(config, table, sb);
                 });
             });
            //输出代码
            if (!Directory.Exists(targetDirectoryPath))
            {
                Directory.CreateDirectory(targetDirectoryPath);
            }
            File.WriteAllText(targetFilePath, sb.ToString());
            return true;
        }
        bool GenerateDomainEntity(GenerateConfig config, Table table)
        {
            //代码生成
            string targetDirectoryPath = EGenerateTargetType.DomainEntities.GetDirectoryPath(config.RootPath, table.Name);
            string targetFilePath = EGenerateTargetType.DomainEntities.GetFilePath(targetDirectoryPath, table.Name);
            string targetNamespace = EGenerateTargetType.DomainEntities.GetNamespace(config.RootNamespace);
            StringBuilder sb = new StringBuilder();
            sb.AppendUsings(EGenerateTargetType.DomainEntities.GetReferences(config));
            sb.AppendLine();
            CodeBuilder.AppendNameSpace(sb, targetNamespace, () =>
            {
                //sb.AppendClass(false, "public static", table.Name, "Ex", () =>
                //{
                //    sb.AppendMethod("//public static void", "Sample", "this " + table.Name + " " + table.Name.ToParameterFormat(), () =>
                //    {
                //    }, true);
                //});
                sb.AppendClass(false, "public static", table.Name + CGenerate.FileNameSuffixOfDomain, "", () =>
                {
                    sb.AppendMethod("//public void", "Create", "DbSession session, " + table.Name + " " + table.Name.ToParameterFormat(), () =>
                    {
                    });
                });
            });
            //输出代码
            if (!Directory.Exists(targetDirectoryPath))
            {
                Directory.CreateDirectory(targetDirectoryPath);
            }
            File.WriteAllText(targetFilePath, sb.ToString());
            return true;
        }

        private static void AppendClassContent(GenerateConfig config, Table table, StringBuilder sb)
        {
            sb.AppendProperties(() =>
            {
                foreach (Column column in table.Columns)
                {
                    //注释
                    if (!string.IsNullOrEmpty(column.Comment))
                    {
                        sb.AppendLine(CGenerate.MethodLS + "/// <summary>");
                        sb.AppendLine(CGenerate.MethodLS + "/// " + column.Comment);
                        sb.AppendLine(CGenerate.MethodLS + "/// </summary>");
                    }
                    //WCF特性
                    if (config.IsSupportWCF)
                    {
                        sb.AppendLine(CGenerate.MethodLS + CGenerate.WCFPropertyContract);
                    }
                    //Enum转换
                    string dataType;
                    if (column.IsEnumField())
                    {
                        dataType = column.GetEnumType();
                    }
                    else
                    {
                        dataType = column.GetCSharpDataType();
                    }
                    sb.AppendLine(CGenerate.MethodLS + "public" + " " + dataType + (column.IsNullableField() ? "?" : "") + " " + column.Name + " { get; set; }");
                }
            });
            sb.AppendLine();
            sb.AppendConstructors(() =>
            {
                sb.AppendConstructor("public", table.Name, "", "", () =>
                {
                });
                List<string> parameters = new List<string>();
                foreach (Column column in table.Columns)
                {
                    if (column.Primary)
                    {
                        string dataType;
                        if (column.IsEnumField())
                        {
                            dataType = column.GetEnumType();
                        }
                        else
                        {
                            dataType = column.GetCSharpDataType();
                        }
                        parameters.Add(dataType + " " + column.Name.ToParameterFormat());
                    }
                }
                sb.AppendConstructor("public", table.Name, string.Join(", ", parameters), "", () =>
                {
                    foreach (Column column in table.Columns)
                    {
                        if (column.Primary)
                        {
                            sb.AppendLine(CGenerate.ContentLS + column.Name + " = " + column.Name.ToParameterFormat() + ";");
                        }
                    }
                });
                sb.AppendConstructor("public", table.Name, "IDataReader reader", " : base(reader)", () =>
                {
                });
            });
            sb.AppendLine();
            sb.AppendMethods(() =>
            {
                sb.AppendMethod("public override void", "Init", "IDataReader reader", () =>
                {
                    foreach (Column column in table.Columns)
                    {
                        if (column.IsEnumField())
                        {
                            if (column.Mandatory)
                            {
                                sb.AppendLine(CGenerate.ContentLS + "this." + column.Name + " = (" + column.GetEnumType() + ")Enum.Parse(typeof(" + column.GetEnumType() + "), reader[nameof(this." + column.Name + ")].ToString());");
                            }
                            else
                            {
                                sb.AppendLine(CGenerate.ContentLS + "if (reader[nameof(this." + column.Name + ")] != DBNull.Value)");
                                sb.AppendLine(CGenerate.ContentLS + "{");
                                sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + "this." + column.Name + " = (" + column.GetEnumType() + ")Enum.Parse(typeof(" + column.GetEnumType() + "), reader[nameof(this." + column.Name + ")].ToString());");
                                sb.AppendLine(CGenerate.ContentLS + "}");
                            }

                        }
                        else
                        {
                            if (column.Mandatory)
                            {
                                sb.AppendLine(CGenerate.ContentLS + "this." + column.Name + " = "
                                    + column.GetCSharpDataTypeConvertString()
                                    + ";");
                            }
                            else
                            {
                                sb.AppendLine(CGenerate.ContentLS + "if (reader[nameof(this." + column.Name + ")] != DBNull.Value)");
                                sb.AppendLine(CGenerate.ContentLS + "{");
                                sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + "this." + column.Name + " = "
                                    + column.GetCSharpDataTypeConvertString()
                                    + ";");
                                sb.AppendLine(CGenerate.ContentLS + "}");
                            }
                        }
                    }
                });
                sb.AppendMethod("public override void", "Init", "IDataReader reader, List<string> fields", () =>
                {
                    foreach (Column column in table.Columns)
                    {
                        sb.AppendLine(CGenerate.ContentLS + "if (fields.Contains(nameof(" + column.Name + ")))");
                        sb.AppendLine(CGenerate.ContentLS + "{");
                        if (column.IsEnumField())
                        {
                            if (column.Mandatory)
                            {
                                sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + "this." + column.Name + " = (" + column.GetEnumType() + ")Enum.Parse(typeof(" + column.GetEnumType() + "), reader[nameof(this." + column.Name + ")].ToString());");
                            }
                            else
                            {
                                sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + "if (reader[nameof(this." + column.Name + ")] != DBNull.Value)");
                                sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + "{");
                                sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + CGenerate.TabLS + "this." + column.Name + " = (" + column.GetEnumType() + ")Enum.Parse(typeof(" + column.GetEnumType() + "), reader[nameof(this." + column.Name + ")].ToString());");
                                sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + "}");
                            }

                        }
                        else
                        {
                            if (column.Mandatory)
                            {
                                sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + "this." + column.Name + " = "
                                    + column.GetCSharpDataTypeConvertString()
                                    + ";");
                            }
                            else
                            {
                                sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + "if (reader[nameof(this." + column.Name + ")] != DBNull.Value)");
                                sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + "{");
                                sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + CGenerate.TabLS + "this." + column.Name + " = "
                                    + column.GetCSharpDataTypeConvertString()
                                    + ";");
                                sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + "}");
                            }
                        }
                        sb.AppendLine(CGenerate.ContentLS + "}");
                    }
                });
                sb.AppendLine(CGenerate.MethodLS + CGenerate.WCFPropertyContract);
                sb.AppendLine(CGenerate.MethodLS + "public override string TableName");
                sb.AppendLine(CGenerate.MethodLS + "{");
                sb.AppendLine(CGenerate.ContentLS + "get");
                sb.AppendLine(CGenerate.ContentLS + "{");
                sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + "return nameof(" + table.Name + ");");
                sb.AppendLine(CGenerate.ContentLS + "}");
                sb.AppendLine(CGenerate.MethodLS + "}");
            });
            sb.AppendLine();
            sb.AppendRegion("Manual", () =>
            {
            });
        }

        bool GenerateEntityOperator(GenerateConfig config, Table table, LocateType LocateType)
        {
            //代码生成
            string targetDirectoryPath = EGenerateTargetType.EntityOperators.GetDirectoryPath(config.RootPath, table.Name);
            string targetFilePath = EGenerateTargetType.EntityOperators.GetFilePath(targetDirectoryPath, table.Name);
            string targetNamespace = EGenerateTargetType.EntityOperators.GetNamespace(config.RootNamespace);
            StringBuilder sb = new StringBuilder();
            sb.AppendUsings(EGenerateTargetType.EntityOperators.GetReferences(config));
            sb.AppendLine();
            sb.AppendNameSpace(targetNamespace, () =>
            {
                sb.AppendClass(false, "public static partial", CGenerate.ClassNameOfEntityOperator, "", () =>
                {
                    sb.AppendMethods(() =>
                    {
                        #region 写
                        sb.AppendRegion("写", () =>
                        {
                            #region D
                            if ((LocateType & LocateType.D) > 0)
                            {
                                sb.AppendMethod("public static bool", "DbDelete", "this " + table.Name + " entity, DbSession session", () =>
                                {
                                    sb.AppendLine(CGenerate.ContentLS + "var query = session.GetDbQueryBuilder();");
                                    foreach (Column column in table.Columns)
                                    {
                                        if (column.Primary)
                                        {
                                            sb.AppendLine(CGenerate.ContentLS + "query.DeleteBuilder.ComponentWhere.Add(new ComponentValueOfWhere(" + table.Name + "Properties." + column.Name + ", entity." + column.Name + ", LocateType.Equal));");
                                        }
                                    }
                                    sb.AppendLine(CGenerate.ContentLS + "return session.GetQueryOperator().Delete<" + table.Name + ">(query);");
                                });
                                sb.AppendMethod("public static bool", "DbDelete", "this List<" + table.Name + "> entities, DbSession session", () =>
                                {
                                    sb.AppendLine(CGenerate.ContentLS + "var query = session.GetDbQueryBuilder();");
                                    foreach (Column column in table.Columns)
                                    {
                                        if (column.Primary)
                                        {
                                            //TODO 这里应该支持多键主键
                                            sb.AppendLine(CGenerate.ContentLS + "var Ids = entities.Select(c =>c." + column.Name + " );");
                                            sb.AppendLine(CGenerate.ContentLS + "query.DeleteBuilder.ComponentWhere.Add(new ComponentValueOfWhere(" + table.Name + "Properties." + column.Name + ", Ids, LocateType.In));");
                                            break;
                                        }
                                    }
                                    sb.AppendLine(CGenerate.ContentLS + "return session.GetQueryOperator().Delete<" + table.Name + ">(query);");
                                });
                            }
                            #endregion

                            #region C
                            if ((LocateType & LocateType.C) > 0)
                            {
                                sb.AppendMethod("public static bool", "DbInsert", "this " + table.Name + " entity, DbSession session", () =>
                                {
                                    sb.AppendLine(CGenerate.ContentLS + "var query = session.GetDbQueryBuilder();");
                                    sb.AppendLine(CGenerate.ContentLS + "InsertBuilder builder = new InsertBuilder();");
                                    foreach (Column column in table.Columns)
                                    {
                                        if (column.Identity)
                                        {
                                            continue;
                                            //TODO 这里存在两种方案  一种是设置了标识增量的自增型, 一种是支持并发的预分配UId机制
                                        }
                                        if (column.IsNullableField())
                                        {
                                            sb.AppendLine(CGenerate.ContentLS + "if (entity." + column.Name + ".HasValue)");
                                            sb.AppendLine(CGenerate.ContentLS + "{");
                                            sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + "builder.ComponentInsert.Add(new ComponentValueOfInsert(" + table.Name + "Properties." + column.Name + ", entity." + column.Name + ".Value));");
                                            sb.AppendLine(CGenerate.ContentLS + "}");
                                        }
                                        else
                                        {
                                            switch (column.GetPDMDataType())
                                            {
                                                case PDMDataType.varchar:
                                                case PDMDataType.nvarchar:
                                                    sb.AppendLine(CGenerate.ContentLS + "if (entity." + column.Name + " == null)");
                                                    sb.AppendLine(CGenerate.ContentLS + "{");
                                                    sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + "throw new NotImplementedException(\"缺少必填的参数项值, 参数项: \" + nameof(entity." + column.Name + "));");
                                                    sb.AppendLine(CGenerate.ContentLS + "}");
                                                    if (column.Length > 0)
                                                    {
                                                        sb.AppendLine(CGenerate.ContentLS + "if (entity." + column.Name + ".Length > " + column.Length + ")");
                                                        sb.AppendLine(CGenerate.ContentLS + "{");
                                                        sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + "throw new NotImplementedException(string.Format(\"参数项:{0}长度:{1}超过额定限制:{2}\", nameof(entity." + column.Name + "), entity." + column.Name + ".Length, " + column.Length + "));");
                                                        sb.AppendLine(CGenerate.ContentLS + "}");
                                                    }
                                                    break;
                                                case PDMDataType.numeric:
                                                case PDMDataType.datetime:
                                                case PDMDataType.uniqueidentifier:
                                                case PDMDataType.boolean:
                                                    break;
                                                default:
                                                    break;
                                            }
                                            sb.AppendLine(CGenerate.ContentLS + "builder.ComponentInsert.Add(new ComponentValueOfInsert(" + table.Name + "Properties." + column.Name + ", entity." + column.Name + "));");
                                        }
                                    }
                                    sb.AppendLine(CGenerate.ContentLS + "query.InsertBuilders.Add(builder);");
                                    sb.AppendLine(CGenerate.ContentLS + "return session.GetQueryOperator().Insert<" + table.Name + ">(query);");
                                });
                                sb.AppendMethod("public static bool", "DbInsert", "this List<" + table.Name + "> entities, DbSession session", () =>
                                {
                                    sb.AppendLine(CGenerate.ContentLS + "var query = session.GetDbQueryBuilder();");
                                    sb.AppendLine(CGenerate.ContentLS + "foreach (var entity in entities)");
                                    sb.AppendLine(CGenerate.ContentLS + "{");
                                    sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + "InsertBuilder builder = new InsertBuilder();");
                                    foreach (Column column in table.Columns)
                                    {
                                        if (column.Identity)
                                        {
                                            continue;
                                            //TODO 这里存在两种方案  
                                            //一种是设置了标识增量的自增型 无需处理
                                            //一种是支持并发的预分配UId机制 通用的预分配UId获取
                                        }
                                        if (column.IsNullableField())
                                        {
                                            sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + "if (entity." + column.Name + ".HasValue)");
                                            sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + "{");
                                            sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + CGenerate.TabLS + "builder.ComponentInsert.Add(new ComponentValueOfInsert(" + table.Name + "Properties." + column.Name + ", entity." + column.Name + ".Value));");
                                            sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + "}");
                                        }
                                        else
                                        {
                                            switch (column.GetPDMDataType())
                                            {
                                                case PDMDataType.varchar:
                                                case PDMDataType.nvarchar:
                                                    sb.AppendLine(CGenerate.ContentLS + "if (entity." + column.Name + " == null)");
                                                    sb.AppendLine(CGenerate.ContentLS + "{");
                                                    sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + "throw new NotImplementedException(\"缺少必填的参数项值, 参数项: \" + nameof(entity." + column.Name + "));");
                                                    sb.AppendLine(CGenerate.ContentLS + "}");
                                                    if (column.Length > 0)
                                                    {
                                                        sb.AppendLine(CGenerate.ContentLS + "if (entity." + column.Name + ".Length > " + column.Length + ")");
                                                        sb.AppendLine(CGenerate.ContentLS + "{");
                                                        sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + "throw new NotImplementedException(string.Format(\"参数项:{0}长度:{1}超过额定限制:{2}\", nameof(entity." + column.Name + "), entity." + column.Name + ".Length, " + column.Length + "));");
                                                        sb.AppendLine(CGenerate.ContentLS + "}");
                                                    }
                                                    break;
                                                case PDMDataType.numeric:
                                                case PDMDataType.datetime:
                                                case PDMDataType.uniqueidentifier:
                                                case PDMDataType.boolean:
                                                    break;
                                                default:
                                                    break;
                                            }
                                            sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + "builder.ComponentInsert.Add(new ComponentValueOfInsert(" + table.Name + "Properties." + column.Name + ", entity." + column.Name + "));");
                                        }
                                    }
                                    sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + "query.InsertBuilders.Add(builder);");
                                    sb.AppendLine(CGenerate.ContentLS + "}");
                                    sb.AppendLine(CGenerate.ContentLS + "return session.GetQueryOperator().InsertAll<" + table.Name + ">(query);");
                                });
                            }
                            #endregion

                            #region U
                            if ((LocateType & LocateType.U) > 0)
                            {
                                sb.AppendMethod("public static bool", "DbUpdate", "this " + table.Name + " entity, DbSession session, params PDMDbProperty[] fields", () =>
                                {
                                    sb.AppendLine(CGenerate.ContentLS + "var query = session.GetDbQueryBuilder();");
                                    sb.AppendLine(CGenerate.ContentLS + "UpdateBuilder builder = new UpdateBuilder();");
                                    //Wheres
                                    foreach (Column column in table.Columns)
                                    {
                                        if (column.Primary)
                                        {
                                            sb.AppendLine(CGenerate.ContentLS + "builder.ComponentWhere.Add(new ComponentValueOfWhere(" + table.Name + "Properties." + column.Name + ", entity." + column.Name + ", LocateType.Equal));");
                                        }
                                    }
                                    //Values
                                    sb.AppendLine(CGenerate.ContentLS + "if (fields==null|| fields.Length==0)");
                                    sb.AppendLine(CGenerate.ContentLS + "{");
                                    foreach (Column column in table.Columns)
                                    {
                                        if (!column.Primary)
                                        {
                                            sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + "builder.ComponentSet.Add(new ComponentValueOfSet(" + table.Name + "Properties." + column.Name + ", entity." + column.Name + "));");
                                        }
                                    }
                                    sb.AppendLine(CGenerate.ContentLS + "}");
                                    sb.AppendLine(CGenerate.ContentLS + "else");
                                    sb.AppendLine(CGenerate.ContentLS + "{");
                                    foreach (Column column in table.Columns)
                                    {
                                        if (!column.Primary)
                                        {
                                            sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + "if (fields.Contains(" + table.Name + "Properties." + column.Name + "))");
                                            sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + "{");
                                            sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + CGenerate.TabLS + "builder.ComponentSet.Add(new ComponentValueOfSet(" + table.Name + "Properties." + column.Name + ", entity." + column.Name + "));");
                                            sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + "}");
                                        }
                                    }
                                    sb.AppendLine(CGenerate.ContentLS + "}");
                                    sb.AppendLine(CGenerate.ContentLS + "query.UpdateBuilders.Add(builder);");
                                    sb.AppendLine(CGenerate.ContentLS + "return session.GetQueryOperator().Update<" + table.Name + ">(query);");
                                });
                                sb.AppendMethod("public static bool", "DbUpdate", "this List<" + table.Name + "> entities, DbSession session, params PDMDbProperty[] fields", () =>
                                {
                                    sb.AppendLine(CGenerate.ContentLS + "var query = session.GetDbQueryBuilder();");
                                    sb.AppendLine(CGenerate.ContentLS + "foreach (var entity in entities)");
                                    sb.AppendLine(CGenerate.ContentLS + "{");
                                    sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + "UpdateBuilder builder = new UpdateBuilder();");
                                    //Wheres
                                    foreach (Column column in table.Columns)
                                    {
                                        if (column.Primary)
                                        {
                                            sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + "builder.ComponentWhere.Add(new ComponentValueOfWhere(" + table.Name + "Properties." + column.Name + ", entity." + column.Name + ", LocateType.Equal));");
                                        }
                                    }
                                    //Values
                                    sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + "if (fields==null|| fields.Length==0)");
                                    sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + "{");
                                    foreach (Column column in table.Columns)
                                    {
                                        if (!column.Primary)
                                        {
                                            sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + CGenerate.TabLS + "builder.ComponentSet.Add(new ComponentValueOfSet(" + table.Name + "Properties." + column.Name + ", entity." + column.Name + "));");
                                        }
                                    }
                                    sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + "}");
                                    sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + "else");
                                    sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + "{");
                                    foreach (Column column in table.Columns)
                                    {
                                        if (!column.Primary)
                                        {
                                            sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + CGenerate.TabLS + "if (fields.Contains(" + table.Name + "Properties." + column.Name + "))");
                                            sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + CGenerate.TabLS + "{");
                                            sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + CGenerate.TabLS + CGenerate.TabLS + "builder.ComponentSet.Add(new ComponentValueOfSet(" + table.Name + "Properties." + column.Name + ", entity." + column.Name + "));");
                                            sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + CGenerate.TabLS + "}");
                                        }
                                    }
                                    sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + "}");
                                    sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + "query.UpdateBuilders.Add(builder);");
                                    sb.AppendLine(CGenerate.ContentLS + "}");
                                    sb.AppendLine(CGenerate.ContentLS + "return session.GetQueryOperator().UpdateAll<" + table.Name + ">(query);");
                                });
                            }
                            #endregion
                        });
                        #endregion
                        #region 读
                        sb.AppendRegion("读", () =>
                        {
                            #region R
                            if ((LocateType & LocateType.R) > 0)
                            {
                                sb.AppendCommend(true, "未查询到数据时返回 null");
                                sb.AppendMethod("public static " + table.Name, "DbSelect", "this " + table.Name + " entity, DbSession session, SelectBuilder select", () =>
                                {
                                    sb.AppendLine(CGenerate.ContentLS + "var query = session.GetDbQueryBuilder();");
                                    sb.AppendLine(CGenerate.ContentLS + "query.SelectBuilder = select;");
                                    sb.AppendLine(CGenerate.ContentLS + "return session.GetQueryOperator().Select<" + table.Name + ">(query);");
                                });
                                sb.AppendCommend(true, "未查询到数据时返回 null");
                                sb.AppendMethod("public static " + table.Name, "DbSelect", "this " + table.Name + " entity, DbSession session, params PDMDbProperty[] fields", () =>
                                {
                                    sb.AppendLine(CGenerate.ContentLS + "var query = session.GetDbQueryBuilder();");
                                    sb.AppendLine(CGenerate.ContentLS + "SelectBuilder builder = new SelectBuilder();");
                                    sb.AppendLine(CGenerate.ContentLS + "if (fields.Count() == 0)");
                                    sb.AppendLine(CGenerate.ContentLS + "{");
                                    foreach (Column column in table.Columns)
                                    {
                                        sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + "builder.ComponentSelect.Add(" + table.Name + "Properties." + column.Name + ");");
                                    }
                                    sb.AppendLine(CGenerate.ContentLS + "}");
                                    sb.AppendLine(CGenerate.ContentLS + "else");
                                    sb.AppendLine(CGenerate.ContentLS + "{");
                                    foreach (Column column in table.Columns)
                                    {
                                        if (column.Primary)
                                        {
                                            sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + "builder.ComponentSelect.Add(" + table.Name + "Properties." + column.Name + ");");
                                        }
                                    }
                                    sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + "foreach (var field in fields)");
                                    sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + "{");
                                    sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + CGenerate.TabLS + "builder.ComponentSelect.Add(field);");
                                    sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + "}");
                                    sb.AppendLine(CGenerate.ContentLS + "}");
                                    foreach (Column column in table.Columns)
                                    {
                                        if (column.Primary)
                                        {
                                            sb.AppendLine(CGenerate.ContentLS + "builder.ComponentWhere.Add(new ComponentValueOfWhere(" + table.Name + "Properties." + column.Name + ", entity." + column.Name + ", LocateType.Equal));");
                                        }
                                    }
                                    sb.AppendLine(CGenerate.ContentLS + "query.SelectBuilders.Add(builder);");
                                    sb.AppendLine(CGenerate.ContentLS + "return session.GetQueryOperator().Select<" + table.Name + ">(query);");
                                });
                                sb.AppendCommend(true, "未查询到数据时返回 new List<T>()");
                                sb.AppendMethod("public static List<" + table.Name + ">", "DbSelect", "this List<" + table.Name + "> entities, DbSession session, params PDMDbProperty[] fields", () =>
                                {
                                    sb.AppendLine(CGenerate.ContentLS + "var query = session.GetDbQueryBuilder();");
                                    sb.AppendLine(CGenerate.ContentLS + "SelectBuilder builder = new SelectBuilder();");
                                    sb.AppendLine(CGenerate.ContentLS + "if (fields.Count() == 0)");
                                    sb.AppendLine(CGenerate.ContentLS + "{");
                                    foreach (Column column in table.Columns)
                                    {
                                        sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + "builder.ComponentSelect.Add(" + table.Name + "Properties." + column.Name + ");");
                                    }
                                    sb.AppendLine(CGenerate.ContentLS + "}");
                                    sb.AppendLine(CGenerate.ContentLS + "else");
                                    sb.AppendLine(CGenerate.ContentLS + "{");
                                    foreach (Column column in table.Columns)
                                    {
                                        if (column.Primary)
                                        {
                                            sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + "builder.ComponentSelect.Add(" + table.Name + "Properties." + column.Name + ");");
                                        }
                                    }
                                    sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + "foreach (var field in fields)");
                                    sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + "{");
                                    sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + CGenerate.TabLS + "builder.ComponentSelect.Add(field);");
                                    sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + "}");
                                    sb.AppendLine(CGenerate.ContentLS + "}");
                                    foreach (Column column in table.Columns)
                                    {
                                        if (column.Primary)
                                        {
                                            //TODO 这里应该支持多键主键
                                            sb.AppendLine(CGenerate.ContentLS + "var Ids = entities.Select(c =>c." + column.Name + " );");
                                            sb.AppendLine(CGenerate.ContentLS + "if (Ids.Count() != 0)");
                                            sb.AppendLine(CGenerate.ContentLS + "{");
                                            sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + "builder.ComponentWhere.Add(new ComponentValueOfWhere(" + table.Name + "Properties." + column.Name + ", Ids, LocateType.In));");
                                            sb.AppendLine(CGenerate.ContentLS + "}");
                                            break;
                                        }
                                    }
                                    sb.AppendLine(CGenerate.ContentLS + "query.SelectBuilders.Add(builder);");
                                    sb.AppendLine(CGenerate.ContentLS + "return session.GetQueryOperator().SelectAll<" + table.Name + ">(query);");
                                });
                                sb.AppendCommend(true, "存在相应对象时返回true,缺少对象时返回false");
                                sb.AppendMethod("public static bool", "DbLoad", "this " + table.Name + " entity, DbSession session, params PDMDbProperty[] fields", () =>
                                {
                                    sb.AppendLine(CGenerate.ContentLS + "var result = entity.DbSelect(session, fields);");
                                    sb.AppendLine(CGenerate.ContentLS + "if (result == null)");
                                    sb.AppendLine(CGenerate.ContentLS + "{");
                                    sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + "return false;");
                                    sb.AppendLine(CGenerate.ContentLS + "}");
                                    sb.AppendLine(CGenerate.ContentLS + "if (fields.Count() == 0)");
                                    sb.AppendLine(CGenerate.ContentLS + "{");
                                    foreach (Column column in table.Columns)
                                    {
                                        if (!column.Primary)
                                        {
                                            sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + "entity." + column.Name + " = result." + column.Name + ";");
                                        }
                                    }
                                    sb.AppendLine(CGenerate.ContentLS + "}");
                                    sb.AppendLine(CGenerate.ContentLS + "else");
                                    sb.AppendLine(CGenerate.ContentLS + "{");
                                    foreach (Column column in table.Columns)
                                    {
                                        if (!column.Primary)
                                        {
                                            sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + "if (fields.Contains(" + table.Name + "Properties." + column.Name + "))");
                                            sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + "{");
                                            sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + CGenerate.TabLS + "entity." + column.Name + " = result." + column.Name + ";");
                                            sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + "}");
                                        }
                                    }
                                    sb.AppendLine(CGenerate.ContentLS + "}");
                                    sb.AppendLine(CGenerate.ContentLS + "return true;");
                                });
                                sb.AppendCommend(true, "存在相应对象时返回true,缺少对象时返回false");
                                sb.AppendMethod("public static bool", "DbLoad", "this List<" + table.Name + "> entities, DbSession session, params PDMDbProperty[] fields", () =>
                                {
                                    sb.AppendLine(CGenerate.ContentLS + "bool result = true;");
                                    sb.AppendLine(CGenerate.ContentLS + "foreach (var entity in entities)");
                                    sb.AppendLine(CGenerate.ContentLS + "{");
                                    sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + "result = result && entity.DbLoad(session, fields);");
                                    sb.AppendLine(CGenerate.ContentLS + "}");
                                    sb.AppendLine(CGenerate.ContentLS + "return result;");
                                });
                            }
                            #endregion
                        });
                        #endregion
                    });
                });
            });
            //输出代码
            if (!Directory.Exists(targetDirectoryPath))
            {
                Directory.CreateDirectory(targetDirectoryPath);
            }
            File.WriteAllText(targetFilePath, sb.ToString());
            return true;
        }
        bool GenerateEntityProperties(GenerateConfig config, Table table)
        {
            //代码生成
            string targetDirectoryPath = EGenerateTargetType.EntityProperties.GetDirectoryPath(config.RootPath, table.Name);
            string targetFilePath = EGenerateTargetType.EntityProperties.GetFilePath(targetDirectoryPath, table.Name);
            string targetNamespace = EGenerateTargetType.EntityProperties.GetNamespace(config.RootNamespace);
            StringBuilder sb = new StringBuilder();
            sb.AppendUsings(EGenerateTargetType.EntityProperties.GetReferences(config));
            sb.AppendLine();
            sb.AppendNameSpace(targetNamespace, () =>
            {
                sb.AppendClass(false, "public", table.Name + CGenerate.FileNameSuffixOfProperties, "", () =>
                {
                    sb.AppendProperties(() =>
                    {
                        foreach (Column column in table.Columns)
                        {
                            var pClass = "PDMDbProperty<" + column.GetCSharpDataType() + ">";
                            var cValue = column.GetCSharpValue();
                            sb.AppendLine(CGenerate.MethodLS + "public static " + pClass + " " + column.Name + " { get; set; } = new " + pClass + "(nameof(" + column.Name + "), \"" + column.Code
                                + "\", \"" + column.Comment + "\", " + column.Primary.ToString().ToLower() + ", " + nameof(PDMDataType) + "." + column.GetPDMDataType() + ", " + column.Length
                                + ", " + column.Precision + ", " + column.Mandatory.ToString().ToLower() + (string.IsNullOrEmpty(cValue) || cValue == "null" ? "" : ", " + cValue) + ");");
                        }
                    });
                });
            });
            //输出代码
            if (!Directory.Exists(targetDirectoryPath))
            {
                Directory.CreateDirectory(targetDirectoryPath);
            }
            File.WriteAllText(targetFilePath, sb.ToString());
            return true;
        }
        #endregion

        #region Enums
        protected override bool GenerateEnums(GenerateConfig config, ObjectCol tables)
        {
            bool result = true;
            foreach (Table table in tables)
            {
                if (!table.Name.StartsWith(CGenerate.PDMNameNotationOfEnum))
                {
                    continue;
                }
                result = result && GenerateEnum(config, table);
            }
            return result;
        }
        bool GenerateEnum(GenerateConfig config, Table table)
        {
            //代码生成
            string targetDirectoryPath = EGenerateTargetType.Enums.GetDirectoryPath(config.RootPath, table.Name);
            string targetFilePath = EGenerateTargetType.Enums.GetFilePath(targetDirectoryPath, table.Name);
            string targetNamespace = EGenerateTargetType.Enums.GetNamespace(config.RootNamespace);
            StringBuilder sb = new StringBuilder();
            sb.AppendUsings(EGenerateTargetType.Enums.GetReferences(config));
            sb.AppendLine();
            sb.AppendNameSpace(targetNamespace, () =>
            {
                sb.AppendEnum(config.IsSupportWCF, table.Name, () =>
                {
                    sb.AppendEnumItems(config.IsSupportWCF, table.GetExtendedAttributeText("EnumData"));
                });
            });
            //输出代码
            if (!Directory.Exists(targetDirectoryPath))
            {
                Directory.CreateDirectory(targetDirectoryPath);
            }
            File.WriteAllText(targetFilePath, sb.ToString());
            return true;
        }
        #endregion
    }
}
