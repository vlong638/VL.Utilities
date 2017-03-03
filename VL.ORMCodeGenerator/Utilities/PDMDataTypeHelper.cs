using PdPDM;
using System;
using System.Text.RegularExpressions;
using VL.Common.Core.ORM;

namespace VL.ORMCodeGenerator.Utilities
{
    public static class DataTypeHelper
    {
        public static PDMDataType GetPDMDataType(this Column column)
        {
            var pdmDataTypeString = column.DataType;
            Regex regex = new Regex(@"(\w+)(\((\d+))?(,(\d+)\))?");
            Match match = regex.Match(pdmDataTypeString);
            string pdmDataType = match.Groups[1].Value;
            int length, precision;
            int.TryParse(match.Groups[3].Value, out length);
            int.TryParse(match.Groups[5].Value, out precision);
            switch (pdmDataType)
            {
                case "boolean":
                case "numeric":
                case "varchar":
                case "nvarchar":
                case "datetime":
                case "uniqueidentifier":
                    return (PDMDataType)Enum.Parse(typeof(PDMDataType), pdmDataType);
                default:
                    throw new NotImplementedException();
            }
        }
        public static string GetEmptyValue(this Column column)
        {
            switch (column.GetPDMDataType())
            {
                case PDMDataType.varchar:
                case PDMDataType.nvarchar:
                    return "\"\"";
                case PDMDataType.numeric:
                    return "0";
                case PDMDataType.datetime:
                    return "DateTime.MinValue";
                case PDMDataType.uniqueidentifier:
                    return "Guid.Empty";
                case PDMDataType.boolean:
                default:
                    throw new NotImplementedException();
            }
        }
        public static string GetCSharpDataType(this Column column)
        {
            PDMDataType pdmDataType = column.GetPDMDataType();
            int length=column.Length ;
            int precision=column.Precision ;
            switch (pdmDataType)
            {
                case PDMDataType.varchar:
                case PDMDataType.nvarchar:
                    return nameof(String);
                case PDMDataType.numeric:
                    if (precision > 0)
                    {
                        return nameof(Decimal);
                    }
                    if (length > 32)
                    {
                        return nameof(Int64);
                    }
                    else if (length > 16 || length == 0)
                    {
                        return nameof(Int32);
                    }
                    else if (length > 1)
                    {
                        return nameof(Int16);
                    }
                    else
                    {
                        return nameof(Boolean);
                    }
                case PDMDataType.datetime:
                    return nameof(DateTime);
                case PDMDataType.uniqueidentifier:
                    return nameof(Guid);
                case PDMDataType.boolean:
                    return nameof(Boolean);
                default:
                    throw new NotImplementedException("该PDM字段类型未设置对应的C#类型");
            }
        }
        public static string GetCSharpValue(this Column column)
        {
            switch (column.GetCSharpDataType())
            {
                case nameof(String):
                case nameof(DateTime):
                    if (string.IsNullOrEmpty(column.DefaultValue))
                    {
                        return "";
                    }
                    return "\"" + column.DefaultValue + "\"";
                case nameof(Decimal):
                case nameof(Int64):
                case nameof(Int32):
                case nameof(Int16):
                case nameof(Guid):
                    if (string.IsNullOrEmpty(column.DefaultValue))
                    {
                        return "";
                    }
                    return column.DefaultValue;
                case nameof(Boolean):
                    if (string.IsNullOrEmpty(column.DefaultValue))
                    {
                        return "";
                    }
                    if ( column.DefaultValue == "0")
                    {
                        return "false";
                    }
                    return "true";
                default:
                    throw new NotImplementedException("该PDM字段类型未设置对应的C#类型");
            }
        }
        public static string GetCSharpDataTypeConvertString(this Column column)
        {
            PDMDataType pdmDataType = column.GetPDMDataType();
            int length = column.Length;
            int precision = column.Precision;
            string value = "reader[nameof(this." + column.Name + ")]";
            switch (pdmDataType)
            {
                case PDMDataType.varchar:
                case PDMDataType.nvarchar:
                case PDMDataType.numeric:
                case PDMDataType.datetime:
                case PDMDataType.boolean:
                    return string.Format("Convert.To{0}({1})", column.GetCSharpDataType(), value);
                case PDMDataType.uniqueidentifier:
                    return string.Format("new Guid({0}.ToString())", value);
                default:
                    throw new NotImplementedException("该PDM字段类型未设置对应的C#类型");
            }
        }
        /// <summary>
        /// 是否可以以T?的形式构建可空类型
        /// </summary>
        /// <param name="dataType"></param>
        /// <returns></returns>
        public static bool IsNullableType(this PDMDataType dataType)
        {
            switch (dataType)
            {
                case PDMDataType.varchar:
                case PDMDataType.nvarchar:
                    return false;
                case PDMDataType.numeric:
                case PDMDataType.datetime:
                case PDMDataType.uniqueidentifier:
                case PDMDataType.boolean:
                    return true;
                default:
                    throw new NotImplementedException("未实现该类型的空类型检测" + dataType.ToString());
            }
        }
    }
}
