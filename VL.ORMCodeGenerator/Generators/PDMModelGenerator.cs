using PdPDM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VL.Common.ORM.Objects;
using VL.Common.ORM.Utilities.QueryBuilders;
using VL.ORM.DbOperateLib.Utilities.QueryOperators;
using VL.ORMCodeGenerator.Objects.Constraits;
using VL.ORMCodeGenerator.Objects.Entities;
using VL.ORMCodeGenerator.Utilities;

namespace VL.ORMCodeGenerator.Generators
{
    /// <summary>
    /// 基于PDM的MSSQL代码生成器
    /// </summary>
    public class PDMModelGenerator : IPDMGenerator
    {
        #region Utilities

        #endregion

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
                string entityDirectoryPath = config.GetEntityDirectoryPath();
                string entityFilePath = config.GetEntityFilePath(tableName + CGenerate.FileNameSuffixOfReference);
                string entityNamespace = config.GetEntityNamespace();
                StringBuilder sb = new StringBuilder();
                sb.AppendUsings(new List<string>() { "System", "System.Collections.Generic", CGenerate.NamespaceOfEntitiesBase });
                sb.AppendLine();
                sb.AppendNameSpace(entityNamespace, () =>
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
                if (!Directory.Exists(entityDirectoryPath))
                {
                    Directory.CreateDirectory(entityDirectoryPath);
                }
                File.WriteAllText(entityFilePath, sb.ToString());
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
                string entityDirectoryPath = config.GetEntityDirectoryPath();
                string entityFilePath = config.GetEntityFilePath(tableName + CGenerate.FileNameSuffixOfFetcher);
                string entityNamespace = config.GetEntityNamespace();
                StringBuilder sb = new StringBuilder();
                sb.AppendUsings(new List<string>() { "System.Collections.Generic", "VL.Common.DbSession.Objects", "VL.ORM.DbOperateLib.Utilities.QueryBuilders", "VL.ORM.DbOperateLib.Utilities.QueryOperators" });
                sb.AppendLine();
                sb.AppendNameSpace(entityNamespace, () =>
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
                                                sb.AppendLine(CGenerate.ContentLS + "var query = " + nameof(IDbQueryBuilder) + ".GetDbQueryBuilder(session);");
                                                sb.AppendLine(CGenerate.ContentLS + "SelectBuilder builder = new SelectBuilder();");
                                                var childTable = reference.ChildTable as Table;
                                                foreach (Column column in childTable.Columns)
                                                {
                                                    if (column.Primary)
                                                    {
                                                        sb.AppendLine(CGenerate.ContentLS + "builder.ComponentWhere.Wheres.Add(new PDMDbPropertyOperateValue(" + childTableName + "Properties." + column.Name + ", OperatorType.Equal, " + parentTableToParameter + "." + column.Name + "));");
                                                    }
                                                }
                                                sb.AppendLine(CGenerate.ContentLS + "query.SelectBuilders.Add(builder);");
                                                sb.AppendLine(CGenerate.ContentLS + parentTableToParameter + "." + childTableToProperty + " = IDbQueryOperator.GetQueryOperator(session)."
                                                    + nameof(IDbQueryOperator.Select) + "<" + childTableName + ">(session, query);");
                                                sb.AppendLine(CGenerate.ContentLS + "return " + parentTableToParameter + "." + childTableToProperty + " != null;");
                                            });
                                            break;
                                        case "1..1":
                                            sb.AppendMethod("public static bool", "Fetch" + childTableName, "this " + parentTableName + " " + parentTableToParameter + ", DbSession session", () =>
                                            {
                                                sb.AppendLine(CGenerate.ContentLS + "var query = " + nameof(IDbQueryBuilder) + ".GetDbQueryBuilder(session);");
                                                sb.AppendLine(CGenerate.ContentLS + "SelectBuilder builder = new SelectBuilder();");
                                                var childTable = reference.ChildTable as Table;
                                                foreach (Column column in childTable.Columns)
                                                {
                                                    if (column.Primary)
                                                    {
                                                        sb.AppendLine(CGenerate.ContentLS + "builder.ComponentWhere.Wheres.Add(new PDMDbPropertyOperateValue(" + childTableName + "Properties." + column.Name + ", OperatorType.Equal, " + parentTableToParameter + "." + column.Name + "));");
                                                    }
                                                }
                                                sb.AppendLine(CGenerate.ContentLS + "query.SelectBuilders.Add(builder);");
                                                sb.AppendLine(CGenerate.ContentLS + parentTableToParameter + "." + childTableToProperty + " = IDbQueryOperator.GetQueryOperator(session)."
                                                    + nameof(IDbQueryOperator.Select) + "<" + childTableName + ">(session, query);");
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
                                                sb.AppendLine(CGenerate.ContentLS + "var query = " + nameof(IDbQueryBuilder) + ".GetDbQueryBuilder(session);");
                                                sb.AppendLine(CGenerate.ContentLS + "SelectBuilder builder = new SelectBuilder();");
                                                var parentTable = reference.ParentTable as Table;
                                                foreach (Column column in parentTable.Columns)
                                                {
                                                    if (column.Primary)
                                                    {
                                                        sb.AppendLine(CGenerate.ContentLS + "builder.ComponentWhere.Wheres.Add(new PDMDbPropertyOperateValue(" + childTableName + "Properties." + column.Name + ", OperatorType.Equal, " + parentTableToParameter + "." + column.Name + "));");
                                                    }
                                                }
                                                sb.AppendLine(CGenerate.ContentLS + "query.SelectBuilders.Add(builder);");
                                                sb.AppendLine(CGenerate.ContentLS + parentTableToParameter + "." + childTableName.ToPropertyFormat().ToPluralFormat() + " = IDbQueryOperator.GetQueryOperator(session)."
                                                         + nameof(IDbQueryOperator.SelectAll) + "<" + childTableName + ">(session, query);");
                                                sb.AppendLine(CGenerate.ContentLS + "return " + parentTableToParameter + "." + childTableName.ToPropertyFormat().ToPluralFormat() + ".Count > 0;");
                                            });
                                            break;
                                        case "1..*":
                                            sb.AppendMethod("public static bool", "Fetch" + childTableToProperty.ToPluralFormat(), "this " + parentTableName + " " + parentTableToParameter + ", DbSession session", () =>
                                            {
                                                sb.AppendLine(CGenerate.ContentLS + "var query = " + nameof(IDbQueryBuilder) + ".GetDbQueryBuilder(session);");
                                                sb.AppendLine(CGenerate.ContentLS + "SelectBuilder builder = new SelectBuilder();");
                                                var childTable = reference.ChildTable as Table;
                                                foreach (Column column in childTable.Columns)
                                                {
                                                    if (column.Primary)
                                                    {
                                                        sb.AppendLine(CGenerate.ContentLS + "builder.ComponentWhere.Wheres.Add(new PDMDbPropertyOperateValue(" + childTableName + "Properties." + column.Name + ", OperatorType.Equal, " + parentTableToParameter + "." + column.Name + "));");
                                                    }
                                                }
                                                sb.AppendLine(CGenerate.ContentLS + "query.SelectBuilders.Add(builder);");
                                                sb.AppendLine(CGenerate.ContentLS + parentTableToParameter + "." + childTableName.ToPropertyFormat().ToPluralFormat() + " = IDbQueryOperator.GetQueryOperator(session)."
                                                    + nameof(IDbQueryOperator.SelectAll) + "<" + childTableName + ">(session, query);");
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
                                                sb.AppendLine(CGenerate.ContentLS + "var query = " + nameof(IDbQueryBuilder) + ".GetDbQueryBuilder(session);");
                                                sb.AppendLine(CGenerate.ContentLS + "SelectBuilder builder = new SelectBuilder();");
                                                sb.AppendLine(CGenerate.ContentLS + "SelectBuilder subSelect = new SelectBuilder();");
                                                var subChildTable = subReference.ChildTable as Table;
                                                foreach (Column column in subChildTable.Columns)
                                                {
                                                    //TODO 多主键
                                                    if (column.Primary)
                                                    {

                                                        sb.AppendLine(CGenerate.ContentLS + "subSelect.ComponentFieldAliases.FieldAliases.Add(" + subChildTableName + "Properties." + column.Name + ");");
                                                        break;
                                                    }
                                                }
                                                var childTable = reference.ChildTable as Table;
                                                foreach (Column column in childTable.Columns)
                                                {
                                                    //TODO 多主键
                                                    if (column.Primary)
                                                    {
                                                        sb.AppendLine(CGenerate.ContentLS + "subSelect.ComponentWhere.Wheres.Add(new PDMDbPropertyOperateValue(" + parentTableName + "Properties." + column.Name + ", OperatorType.Equal, " + childTableName.ToParameterFormat() + "." + column.Name + "));");
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
                                                //        sb.AppendLine(GConstraints.ContentLS + "builder.ComponentWhere.Wheres.Add(new PDMDbPropertyOperateValue(" + subChildTableName + "Properties." + column.Name + ", OperatorType.In, subSelect");
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
                                                //        sb.AppendLine(GConstraints.ContentLS + GConstraints.TabLS + ", new PDMDbPropertyOperateValue(" + parentTableName + "Properties." + column.Name + ", OperatorType.Equal, " + childTableName.ToParameterFormat() + "." + column.Name + ")));");
                                                //        break;
                                                //    }
                                                //}
                                                sb.AppendLine(CGenerate.ContentLS + "query.SelectBuilders.Add(builder);");
                                                sb.AppendLine(CGenerate.ContentLS + childTableName.ToParameterFormat() + "." + subChildTableToProperty.ToPluralFormat() + " = IDbQueryOperator.GetQueryOperator(session).SelectMany<" + subChildTableName + ">(session, query);");
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
                                            //            sb.AppendLine(GConstraints.ContentLS + "query.SelectBuilder.ComponentWhere.Wheres.Add(new PDMDbPropertyOperateValue(" + childTableName + "Properties." + column.Name + ", OperatorType.Equal, " + parentTableToParameter + "." + column.Name + "));");
                                            //        }
                                            //    }
                                            //    sb.AppendLine(GConstraints.ContentLS + parentTableToParameter + "." + childTableName.ToPropertyFormat().ToPluralFormat() + " = IDbQueryOperator.GetQueryOperator(session)."
                                            //        + nameof(IDbQueryOperator.SelectAll) + "<" + childTableName + ">(session, query);");
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
                if (!Directory.Exists(entityDirectoryPath))
                {
                    Directory.CreateDirectory(entityDirectoryPath);
                }
                File.WriteAllText(entityFilePath, sb.ToString());
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
                if (table.Name.StartsWith(CGenerate.PDMNameNotationOfRelationMapper))
                {
                    result = result && GenerateEntity(config, table);
                    result = result && GenerateEntityOperator(config, table, OperatorType.C | OperatorType.R | OperatorType.U | OperatorType.D);
                    result = result && GenerateEntityProperties(config, table);
                }
                if (table.Name.StartsWith(CGenerate.PDMNameNotationOfTable))
                {
                    result = result && GenerateEntity(config, table);
                    result = result && GenerateEntityOperator(config, table, OperatorType.C | OperatorType.R | OperatorType.U | OperatorType.D);
                    result = result && GenerateEntityProperties(config, table);
                }
            }
            return result;
        }
        [Flags]
        public enum OperatorType
        {
            C = 1,
            R = 2,
            U = 4,
            D = 8,
        }
        bool GenerateEntity(GenerateConfig config, Table table)
        {
            //代码生成
            string targetDirectoryPath = config.GetEntityDirectoryPath();
            string targetFilePath = config.GetEntityFilePath(table.Name);
            string targetNamespace = config.GetEntityNamespace();
            StringBuilder sb = new StringBuilder();
            var usings = new List<string>() { "System", "System.Collections.Generic", "System.Data", CGenerate.NamespaceOfEntitiesBase };// "System.Linq", "System.Reflection", 
            if (config.IsSupportWCF)
            {
                usings.Add("System.Runtime.Serialization");
            }
            sb.AppendUsings(usings);
            sb.AppendLine();
            sb.AppendNameSpace(targetNamespace, () =>
            {
                sb.AppendClass(config.IsSupportWCF, "public partial", table.Name, " : " + nameof(IPDMTBase), () =>
                {
                    sb.AppendProperties(() =>
                    {
                        foreach (Column column in table.Columns)
                        {
                            if (config.IsSupportWCF)
                            {
                                sb.AppendLine(CGenerate.MethodLS + CGenerate.WCFPropertyContract);
                            }
                            //TODO 如果字段是Enum类型的
                            string dataType;
                            if (column.IsEnumField())
                            {
                                dataType = column.GetEnumType();
                            }
                            else
                            {
                                dataType = DataTypeHelper.GetCSharpDataType(DataTypeHelper.GetPDMDataType(column.DataType), column.Length, column.Precision);
                            }
                            sb.AppendLine(CGenerate.MethodLS + "public" + " " + dataType + (column.IsNullableField() ? "?" : "") + " " + column.Name + " { get; set; }");
                        }
                    });
                    sb.AppendLine();
                    sb.AppendConstructors(() =>
                    {
                        //sb.AppendConstructor("static", table.Name, "", "", () =>
                        //{
                        //    sb.AppendCommend(GConstraints.ContentLS, "字段", false);
                        //    foreach (Column column in table.Columns)
                        //    {
                        //        sb.AppendLine(GConstraints.ContentLS + "{0}.Properties.Add(new PDMDbProperty(nameof({1}), \"{2}\", \"{3}\", {4}, \"{5}\", {6}, {7}, {8}, \"{9}\"));"//, {9}
                        //            , table.Name, column.Name, column.Code, column.Comment, column.Primary.ToString().ToLower(), column.DataType, column.Length, column.Precision, column.Mandatory.ToString().ToLower(), column.DefaultValue);//, column.DefaultValue
                        //    }
                        //});
                        sb.AppendConstructor("public", table.Name, "", "", () =>
                        {
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
                                        sb.AppendLine(CGenerate.ContentLS + "this." + column.Name + " = Convert.To" + DataTypeHelper.GetCSharpDataType(DataTypeHelper.GetPDMDataType(column.DataType), column.Length, column.Precision)
                                        + "(reader[nameof(this." + column.Name + ")]);");
                                    }
                                    else
                                    {
                                        sb.AppendLine(CGenerate.ContentLS + "if (reader[nameof(this." + column.Name + ")] != DBNull.Value)");
                                        sb.AppendLine(CGenerate.ContentLS + "{");
                                        sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + "this." + column.Name + " = Convert.To" + DataTypeHelper.GetCSharpDataType(DataTypeHelper.GetPDMDataType(column.DataType), column.Length, column.Precision)
                                        + "(reader[nameof(this." + column.Name + ")]);");
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
                                        sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + "this." + column.Name + " = Convert.To" + DataTypeHelper.GetCSharpDataType(DataTypeHelper.GetPDMDataType(column.DataType), column.Length, column.Precision)
                                        + "(reader[nameof(this." + column.Name + ")]);");
                                    }
                                    else
                                    {
                                        sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + "if (reader[nameof(this." + column.Name + ")] != DBNull.Value)");
                                        sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + "{");
                                        sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + CGenerate.TabLS + "this." + column.Name + " = Convert.To" + DataTypeHelper.GetCSharpDataType(DataTypeHelper.GetPDMDataType(column.DataType), column.Length, column.Precision)
                                        + "(reader[nameof(this." + column.Name + ")]);");
                                        sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + "}");
                                    }
                                }
                                sb.AppendLine(CGenerate.ContentLS + "}");
                            }
                        });
                        sb.AppendMethod("public override string", "GetTableName", "", () =>
                        {
                            sb.AppendLine(CGenerate.ContentLS + "return nameof(" + table.Name + ");");
                        });
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
        bool GenerateEntityOperator(GenerateConfig config, Table table, OperatorType operatorType)
        {
            //代码生成
            string targetDirectoryPath = config.GetEntityDirectoryPath();
            string targetFilePath = config.GetEntityFilePath(table.Name + CGenerate.FileNameSuffixOfOperator);
            string targetNamespace = config.GetEntityNamespace();
            StringBuilder sb = new StringBuilder();
            var usings = new List<string>() { "System.Collections.Generic", "System.Linq", "VL.Common.DbSession.Objects", "VL.ORM.DbOperateLib.Utilities.QueryBuilders", "VL.ORM.DbOperateLib.Utilities.QueryOperators" };
            sb.AppendUsings(usings);
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
                            if ((operatorType & OperatorType.D) > 0)
                            {
                                sb.AppendMethod("public static bool", "Delete", "this " + table.Name + " entity, DbSession session", () =>
                                {
                                    sb.AppendLine(CGenerate.ContentLS + "var query = " + nameof(IDbQueryBuilder) + ".GetDbQueryBuilder(session);");
                                    foreach (Column column in table.Columns)
                                    {
                                        if (column.Primary)
                                        {
                                            sb.AppendLine(CGenerate.ContentLS + "query.DeleteBuilder.ComponentWhere.Wheres.Add(new PDMDbPropertyOperateValue(" + table.Name + "Properties." + column.Name + ", OperatorType.Equal, entity." + column.Name + "));");
                                        }
                                    }
                                    sb.AppendLine(CGenerate.ContentLS + "return IDbQueryOperator.GetQueryOperator(session).Delete<" + table.Name + ">(session, query);");
                                });
                                sb.AppendMethod("public static bool", "Delete", "this List<" + table.Name + "> entities, DbSession session", () =>
                                {
                                    sb.AppendLine(CGenerate.ContentLS + "var query = " + nameof(IDbQueryBuilder) + ".GetDbQueryBuilder(session);");
                                    foreach (Column column in table.Columns)
                                    {
                                        if (column.Primary)
                                        {
                                            //TODO 这里应该支持多键主键
                                            sb.AppendLine(CGenerate.ContentLS + "var Ids = entities.Select(c =>c." + column.Name + " );");
                                            sb.AppendLine(CGenerate.ContentLS + "query.DeleteBuilder.ComponentWhere.Wheres.Add(new PDMDbPropertyOperateValue(" + table.Name + "Properties." + column.Name + ", OperatorType.In, Ids));");
                                            break;
                                        }
                                    }
                                    sb.AppendLine(CGenerate.ContentLS + "return IDbQueryOperator.GetQueryOperator(session).Delete<" + table.Name + ">(session, query);");
                                });
                            }
                            #endregion

                            #region C
                            if ((operatorType & OperatorType.C) > 0)
                            {
                                sb.AppendMethod("public static bool", "Insert", "this " + table.Name + " entity, DbSession session", () =>
                                {
                                    sb.AppendLine(CGenerate.ContentLS + "var query = " + nameof(IDbQueryBuilder) + ".GetDbQueryBuilder(session);");
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
                                            sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + "builder.ComponentValue.Values.Add(new PDMDbPropertyValue(" + table.Name + "Properties." + column.Name + ", entity." + column.Name + ".Value));");
                                            sb.AppendLine(CGenerate.ContentLS + "}");
                                        }
                                        else
                                        {
                                            sb.AppendLine(CGenerate.ContentLS + "builder.ComponentValue.Values.Add(new PDMDbPropertyValue(" + table.Name + "Properties." + column.Name + ", entity." + column.Name + "));");
                                        }
                                    }
                                    sb.AppendLine(CGenerate.ContentLS + "query.InsertBuilders.Add(builder);");
                                    sb.AppendLine(CGenerate.ContentLS + "return IDbQueryOperator.GetQueryOperator(session).Insert<" + table.Name + ">(session, query);");
                                });
                                sb.AppendMethod("public static bool", "Insert", "this List<" + table.Name + "> entities, DbSession session", () =>
                                {
                                    sb.AppendLine(CGenerate.ContentLS + "var query = " + nameof(IDbQueryBuilder) + ".GetDbQueryBuilder(session);");
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
                                            sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + CGenerate.TabLS + "builder.ComponentValue.Values.Add(new PDMDbPropertyValue(" + table.Name + "Properties." + column.Name + ", entity." + column.Name + ".Value));");
                                            sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + "}");
                                        }
                                        else
                                        {
                                            sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + "builder.ComponentValue.Values.Add(new PDMDbPropertyValue(" + table.Name + "Properties." + column.Name + ", entity." + column.Name + "));");
                                        }
                                    }
                                    sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + "query.InsertBuilders.Add(builder);");
                                    sb.AppendLine(CGenerate.ContentLS + "}");
                                    sb.AppendLine(CGenerate.ContentLS + "return IDbQueryOperator.GetQueryOperator(session).InsertAll<" + table.Name + ">(session, query);");
                                });
                            }
                            #endregion

                            #region U
                            if ((operatorType & OperatorType.U) > 0)
                            {
                                sb.AppendMethod("public static bool", "Update", "this " + table.Name + " entity, DbSession session, List<string> fields", () =>
                                {
                                    sb.AppendLine(CGenerate.ContentLS + "var query = " + nameof(IDbQueryBuilder) + ".GetDbQueryBuilder(session);");
                                    sb.AppendLine(CGenerate.ContentLS + "UpdateBuilder builder = new UpdateBuilder();");
                                    foreach (Column column in table.Columns)
                                    {
                                        //Wheres
                                        if (column.Primary)
                                        {
                                            sb.AppendLine(CGenerate.ContentLS + "builder.ComponentWhere.Wheres.Add(new PDMDbPropertyOperateValue(" + table.Name + "Properties." + column.Name + ", OperatorType.Equal, entity." + column.Name + "));");
                                        }
                                    }
                                    foreach (Column column in table.Columns)
                                    {
                                        //Values
                                        if (!column.Primary)
                                        {
                                            sb.AppendLine(CGenerate.ContentLS + "if (fields.Contains(" + table.Name + "Properties." + column.Name + ".Title))");
                                            sb.AppendLine(CGenerate.ContentLS + "{");
                                            sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + "builder.ComponentValue.Values.Add(new PDMDbPropertyValue(" + table.Name + "Properties." + column.Name + ", entity." + column.Name + "));");
                                            sb.AppendLine(CGenerate.ContentLS + "}");
                                        }
                                    }
                                    sb.AppendLine(CGenerate.ContentLS + "query.UpdateBuilders.Add(builder);");
                                    sb.AppendLine(CGenerate.ContentLS + "return IDbQueryOperator.GetQueryOperator(session).Update<" + table.Name + ">(session, query);");
                                });
                                sb.AppendMethod("public static bool", "Update", "this List<" + table.Name + "> entities, DbSession session, List<string> fields", () =>
                                {
                                    sb.AppendLine(CGenerate.ContentLS + "var query = " + nameof(IDbQueryBuilder) + ".GetDbQueryBuilder(session);");
                                    sb.AppendLine(CGenerate.ContentLS + "foreach (var entity in entities)");
                                    sb.AppendLine(CGenerate.ContentLS + "{");
                                    sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + "UpdateBuilder builder = new UpdateBuilder();");
                                    foreach (Column column in table.Columns)
                                    {
                                        if (column.Primary)
                                        {
                                            sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + "builder.ComponentWhere.Wheres.Add(new PDMDbPropertyOperateValue(" + table.Name + "Properties." + column.Name + ", OperatorType.Equal, entity." + column.Name + "));");
                                        }
                                    }
                                    foreach (Column column in table.Columns)
                                    {
                                        if (!column.Primary)
                                        {
                                            sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + "if (fields.Contains(" + table.Name + "Properties." + column.Name + ".Title))");
                                            sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + "{");
                                            sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + CGenerate.TabLS + "builder.ComponentValue.Values.Add(new PDMDbPropertyValue(" + table.Name + "Properties." + column.Name + ", entity." + column.Name + "));");
                                            sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + "}");
                                        }
                                    }
                                    sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + "query.UpdateBuilders.Add(builder);");
                                    sb.AppendLine(CGenerate.ContentLS + "}");
                                    sb.AppendLine(CGenerate.ContentLS + "return IDbQueryOperator.GetQueryOperator(session).UpdateAll<" + table.Name + ">(session, query);");
                                });
                            }
                            #endregion
                        });
                        #endregion
                        #region 读
                        sb.AppendRegion("读", () =>
                        {
                            #region R
                            if ((operatorType & OperatorType.R) > 0)
                            {
                                sb.AppendMethod("public static " + table.Name, "Select", "this " + table.Name + " entity, DbSession session, List<string> fields", () =>
                                {
                                    sb.AppendLine(CGenerate.ContentLS + "var query = " + nameof(IDbQueryBuilder) + ".GetDbQueryBuilder(session);");
                                    sb.AppendLine(CGenerate.ContentLS + "SelectBuilder builder = new SelectBuilder();");
                                    sb.AppendLine(CGenerate.ContentLS + "foreach (var field in fields)");
                                    sb.AppendLine(CGenerate.ContentLS + "{");
                                    sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + "builder.ComponentFieldAliases.FieldAliases.Add(new FieldAlias(field));");
                                    sb.AppendLine(CGenerate.ContentLS + "}");
                                    foreach (Column column in table.Columns)
                                    {
                                        if (column.Primary)
                                        {
                                            sb.AppendLine(CGenerate.ContentLS + "builder.ComponentWhere.Wheres.Add(new PDMDbPropertyOperateValue(" + table.Name + "Properties." + column.Name + ", OperatorType.Equal, entity." + column.Name + "));");
                                        }
                                    }
                                    sb.AppendLine(CGenerate.ContentLS + "query.SelectBuilders.Add(builder);");
                                    sb.AppendLine(CGenerate.ContentLS + "return IDbQueryOperator.GetQueryOperator(session).Select<" + table.Name + ">(session, query);");
                                });
                                sb.AppendMethod("public static List<" + table.Name + ">", "Select", "this List<" + table.Name + "> entities, DbSession session, List<string> fields", () =>
                                {
                                    sb.AppendLine(CGenerate.ContentLS + "var query = " + nameof(IDbQueryBuilder) + ".GetDbQueryBuilder(session);");
                                    sb.AppendLine(CGenerate.ContentLS + "SelectBuilder builder = new SelectBuilder();");
                                    sb.AppendLine(CGenerate.ContentLS + "foreach (var field in fields)");
                                    sb.AppendLine(CGenerate.ContentLS + "{");
                                    sb.AppendLine(CGenerate.ContentLS + CGenerate.TabLS + "builder.ComponentFieldAliases.FieldAliases.Add(new FieldAlias(field));");
                                    sb.AppendLine(CGenerate.ContentLS + "}");
                                    foreach (Column column in table.Columns)
                                    {
                                        if (column.Primary)
                                        {
                                            //TODO 这里应该支持多键主键
                                            sb.AppendLine(CGenerate.ContentLS + "var Ids = entities.Select(c =>c." + column.Name + " );");
                                            sb.AppendLine(CGenerate.ContentLS + "builder.ComponentWhere.Wheres.Add(new PDMDbPropertyOperateValue(" + table.Name + "Properties." + column.Name + ", OperatorType.In, Ids));");
                                            break;
                                        }
                                    }
                                    sb.AppendLine(CGenerate.ContentLS + "query.SelectBuilders.Add(builder);");
                                    sb.AppendLine(CGenerate.ContentLS + "return IDbQueryOperator.GetQueryOperator(session).SelectMany<" + table.Name + ">(session, query);");
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
            string targetDirectoryPath = config.GetEntityDirectoryPath();
            string targetFilePath = config.GetEntityFilePath(table.Name + CGenerate.FileNameSuffixOfProperties);
            string targetNamespace = config.GetEntityNamespace();
            StringBuilder sb = new StringBuilder();
            var usings = new List<string>() { CGenerate.NamespaceOfEntitiesBase };// "System.Linq", "System.Reflection", 
            sb.AppendUsings(usings);
            sb.AppendLine();
            sb.AppendNameSpace(targetNamespace, () =>
            {
                sb.AppendClass(false, "public", table.Name + CGenerate.FileNameSuffixOfProperties, "", () =>
                {
                    sb.AppendProperties(() =>
                    {
                        foreach (Column column in table.Columns)
                        {
                            sb.AppendLine(CGenerate.MethodLS + "public static PDMDbProperty " + column.Name + " { get; set; } = new PDMDbProperty(nameof(" + column.Name + "), \"" + column.Code + "\", \"" + column.Comment + "\", " + column.Primary.ToString().ToLower() + ", \"" + DataTypeHelper.GetPDMDataType(column.DataType) + "\", " + column.Length + ", " + column.Precision + ", " + column.Mandatory.ToString().ToLower() + ", " + (string.IsNullOrEmpty(column.DefaultValue) ? "null" : column.DefaultValue) + ");");
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
            string enumDirectoryPath = config.GetEnumDirectoryPath();
            string enumFilePath = config.GetEnumFilePath(table.Name);
            string enumNamespace = config.GetEnumNamespace();
            StringBuilder sb = new StringBuilder();
            sb.AppendUsings(new List<string>() { "System.Runtime.Serialization" });
            sb.AppendLine();
            sb.AppendNameSpace(enumNamespace, () =>
            {
                sb.AppendEnum(config.IsSupportWCF, table.Name, () =>
                {
                    sb.AppendEnumItems(config.IsSupportWCF, table.GetExtendedAttributeText("EnumData"));
                });
            });
            //输出代码
            if (!Directory.Exists(enumDirectoryPath))
            {
                Directory.CreateDirectory(enumDirectoryPath);
            }
            File.WriteAllText(enumFilePath, sb.ToString());
            return true;
        }
        #endregion
    }
}
