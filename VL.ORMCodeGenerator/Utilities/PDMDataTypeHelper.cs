using System;
using System.Text.RegularExpressions;
using VL.Common.ORM.Objects;

namespace VL.ORMCodeGenerator.Utilities
{
    public static class DataTypeHelper
    {
        public static PDMDataType GetPDMDataType(string pdmDataTypeString)
        {
            Regex regex = new Regex(@"(\w+)(\((\d+))?(,(\d+)\))?");
            Match match = regex.Match(pdmDataTypeString);
            string pdmDataType = match.Groups[1].Value;
            int length, precision;
            int.TryParse(match.Groups[3].Value, out length);
            int.TryParse(match.Groups[5].Value, out precision);
            switch (pdmDataType)
            {
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
        public static string GetCSharpDataType(this PDMDataType pdmDataType, int length, int precision)
        {
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
                    if (length > 32 || length == 0)
                    {
                        return nameof(Int64);
                    }
                    else if (length > 16)
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
                default:
                    throw new NotImplementedException("该PDM字段类型未设置对应的C#类型");
            }
        }
        public static string GetCSharpDataTypeConvertString(this PDMDataType pdmDataType, int length, int precision, string value)
        {
            switch (pdmDataType)
            {
                case PDMDataType.varchar:
                case PDMDataType.nvarchar:
                case PDMDataType.numeric:
                case PDMDataType.datetime:
                    return string.Format("Convert.To{0}({1})", pdmDataType.GetCSharpDataType(length, precision), value);
                case PDMDataType.uniqueidentifier:
                    return string.Format("new Guid({0}.ToString())", value);
                default:
                    throw new NotImplementedException("该PDM字段类型未设置对应的C#类型");
            }
        }
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
                    return true;
                default:
                    throw new NotImplementedException("未实现该类型的空类型检测" + dataType.ToString());
            }
        }
    }
}
