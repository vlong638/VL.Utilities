using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VL.ORMCodeGenerator.Objects.Constraits;

namespace VL.ORMCodeGenerator.Utilities
{
    public static class CodeBuilder
    {
        #region Utilities Methods
        internal static void AppendFormatLine(this StringBuilder sb, string format, params object[] args)
        {
            try
            {
                sb.AppendLine(string.Format(format, args));
            }
            catch (Exception)
            {
            }
        }
        internal static void AppendRegion(this StringBuilder sb, string regionName, Action appendRegionContents)
        {
            sb.AppendFormatLine(CGenerate.MethodLS + "#region {0}", regionName);
            appendRegionContents();
            sb.AppendLine(CGenerate.MethodLS + "#endregion");
        }
        internal static void AppendCommend(this StringBuilder sb, string leadingSpace, string commend, bool isForMethod)
        {
            if (isForMethod)
            {
                sb.AppendLine(leadingSpace + "/// <summary>");
                sb.AppendLine(leadingSpace + "/// " + commend);
                sb.AppendLine(leadingSpace + "/// </summary>");
            }
            else
            {
                sb.AppendLine(leadingSpace + "//" + commend);
            }
        }
        internal static void AppendCommend(this StringBuilder sb, string leadingSpace, List<string> commends)
        {
            sb.AppendLine(leadingSpace + "/// <summary>");
            foreach (var commend in commends)
            {
                sb.AppendLine(leadingSpace + "/// " + commend);
            }
            sb.AppendLine(leadingSpace + "/// </summary>");
        }
        internal static void AppendContent(this StringBuilder sb, string content)
        {
            sb.AppendLine(content);
        }
        internal static void AppendLine(this StringBuilder sb, string pattern, params object[] args)
        {
            sb.AppendLine(string.Format(pattern, args));
        }
        #endregion

        #region Using
        static string UsingPattern = "using {0};" + System.Environment.NewLine;
        static List<string> BasicUsings { set; get; } = new List<string>()
        {
            "System",
            "System.Collections.Generic",
            "System.Data"
        };
        public static void AppendUsings(this StringBuilder sb, List<string> usings = null, params string[] exUsings)
        {
            List<string> Usings = new List<string>();
            //添加主要命名空间引用
            if (usings != null)
            {
                Usings.AddRange(usings);
                Usings = Usings.Distinct().ToList();
            }
            //else
            //{
            //    Usings.AddRange(BasicUsings);
            //}
            //添加额外命名空间引用
            foreach (string exUsing in exUsings)
            {
                if (!string.IsNullOrEmpty(exUsing))
                    Usings.Add(exUsing);
            }
            //消重
            foreach (string Using in Usings.Distinct())
            {
                sb.AppendFormat(UsingPattern, Using);
            }
            //排序
            Usings.Sort();
        }
        #endregion

        #region NameSpace
        public static void AppendNameSpace(this StringBuilder sb, string nameSpace, Action appentNameSpaceContents)
        {
            sb.AppendFormatLine("namespace {0}", nameSpace);
            sb.AppendLine("{");
            appentNameSpaceContents();
            sb.AppendLine("}");
        }
        #endregion

        #region Class
        public static void AppendClass(this StringBuilder sb, bool isSupportWCF, string prefix, string className, string suffix, Action appentClassContents)
        {
            if (isSupportWCF)
            {
                sb.AppendLine(CGenerate.ClassLS + CGenerate.WCFClassContract);
            }
            sb.AppendFormatLine(CGenerate.ClassLS + "{0} class {1}{2}", prefix, className, suffix);
            sb.AppendLine(CGenerate.ClassLS + "{");
            appentClassContents();
            sb.AppendLine(CGenerate.ClassLS + "}");
        }
        #endregion

        #region Enum
        public static void AppendEnum(this StringBuilder sb, bool isSupportWCF, string enumName, Action appentEnumContents)
        {
            if (isSupportWCF)
            {
                sb.AppendLine(CGenerate.ClassLS + CGenerate.WCFClassContract);
            }
            sb.AppendFormatLine(CGenerate.ClassLS + "public enum {0}", enumName);
            sb.AppendLine(CGenerate.ClassLS + "{");
            appentEnumContents();
            sb.AppendLine(CGenerate.ClassLS + "}");
        }
        public static void AppendEnumItems(this StringBuilder sb, bool isSupportWCF, string enumItemsString)
        {
            var enumItems = enumItemsString.Split('\n');//\r\n
            foreach (var enumItem in enumItems)
            {
                if (string.IsNullOrWhiteSpace(enumItem))
                {
                    continue;
                }
                var enumItemValues = enumItem.TrimEnd('\r').Split(',', '.');
                string commend = enumItemValues[1];
                //注释
                if (!string.IsNullOrEmpty(commend))
                {
                    sb.AppendCommend(CGenerate.MethodLS, commend, true);
                }
                //WCF契约
                if (isSupportWCF)
                {
                    sb.AppendLine(CGenerate.MethodLS + CGenerate.WCFEnumContract);
                }
                //枚举项
                AppendEnumItem(sb, enumItemValues[2], enumItemValues[0]);
            }
        }
        private static void AppendEnumItem(this StringBuilder sb, string name, string valueString)
        {
            if (string.IsNullOrEmpty(valueString))
            {
                sb.AppendLine(CGenerate.MethodLS + name + ",");
            }
            else
            {
                int value;
                int.TryParse(valueString, out value);
                sb.AppendLine(CGenerate.MethodLS + name + " = " + value + ",");
            }
        }
        #endregion

        #region Properties
        public static void AppendProperties(this StringBuilder sb, bool isNullable, bool isMultiple, string className)
        {
            string classNameToProperty = className.ToPropertyFormat();
            sb.AppendLine(CGenerate.MethodLS + "public " + (isMultiple ? "List<" : "") + className + (isMultiple ? ">" : "") + " " + (isMultiple ? (classNameToProperty.ToPluralFormat()) : classNameToProperty) + " { get; set; }");
        }
        public static void AppendProperties(this StringBuilder sb, Action appendPropertiesContent)
        {
            sb.AppendRegion("Properties", () =>
            {
                appendPropertiesContent();
            });
        }
        #endregion

        #region Constructors
        internal static void AppendConstructors(this StringBuilder sb, Action appendConstructorsContent)
        {
            sb.AppendRegion("Constructors", () =>
            {
                appendConstructorsContent();
            });
        }
        /// <summary>
        /// 模板:添加构造函数字符串
        /// </summary>
        internal static void AppendConstructor(this StringBuilder sb, string prefix, string className, string parameter, string suffix, Action appendAssignment)
        {
            sb.AppendFormatLine(CGenerate.MethodLS + "{0} {1}({2}){3}", prefix, className, parameter, suffix);
            sb.AppendLine(CGenerate.MethodLS + "{");
            appendAssignment();
            sb.AppendLine(CGenerate.MethodLS + "}");
        }
        #endregion

        #region Methods
        internal static void AppendSummary(this StringBuilder sb, IEnumerable<string> summaries)
        {

            /// <summary>
            /// return false if fetch.count()==0
            /// return true if fetch.Count()>0
            /// </summary>

            sb.AppendFormatLine(CGenerate.MethodLS + "/// <summary>");
            foreach (var summary in summaries)
            {
                sb.AppendFormatLine(CGenerate.MethodLS + "/// "+ summary);
            }
            sb.AppendFormatLine(CGenerate.MethodLS + "/// </summary>");
        }
        internal static void AppendMethods(this StringBuilder sb, Action appendMethodsContent)
        {
            sb.AppendRegion("Methods", () =>
            {
                appendMethodsContent();
            });
        }
        internal static void AppendMethod(this StringBuilder sb, string prefix, string methodName, string parameterString, Action appendMethodContent)
        {
            sb.AppendFormatLine(CGenerate.MethodLS + "{0} {1}({2})", prefix, methodName, parameterString);
            if (prefix.StartsWith("//"))
            {
                sb.AppendLine(CGenerate.MethodLS + "//{");
                appendMethodContent();
                sb.AppendLine(CGenerate.MethodLS + "//}");
            }
            else
            {
                sb.AppendLine(CGenerate.MethodLS + "{");
                appendMethodContent();
                sb.AppendLine(CGenerate.MethodLS + "}");
            }
        }
        #endregion

        #region Manual
        public static void AppendManualField(this StringBuilder sb)
        {
            sb.AppendRegion("ManualCode", () =>
            {
                sb.AppendLine(CGenerate.MethodLS + "//手工添加的内容请于此处添加");
            });
        }
        #endregion
    }
}
