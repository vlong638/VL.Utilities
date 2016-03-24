using PdPDM;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VL.Common.ORM.Objects;

namespace VL.ORMCodeGenerator.Utilities
{
    public static class PDMColumnHelper
    {
        public static string ToPropertyFormat(this string className)
        {
            return className.Substring(1);
        }
        public static string ToParameterFormat(this string className)
        {
            return className.Substring(0, 1).ToLower() + className.Substring(1);
        }
        public static string ToPluralFormat(this string className)
        {
            return System.Data.Entity.Design.PluralizationServices.PluralizationService.CreateService(new CultureInfo("en")).Pluralize(className);
        }
        public static bool IsEnumField(this Column column)
        {
            return column.GetExtendedAttributeText("Enum") != "<None>";
        }
        public static string GetEnumType(this Column column)
        {
            return column.GetExtendedAttributeText("Enum");
        }
        public static bool IsNullableField(this Column column)
        {
            if (!column.Mandatory)
            {
                var dataType = DataTypeHelper.GetPDMDataType(column.DataType);
                switch (dataType)
                {
                    case PDMDataType.varchar:
                        return false;
                    case PDMDataType.numeric:
                    case PDMDataType.datetime:
                    case PDMDataType.uniqueidentifier:
                        return true;
                    default:
                        throw new NotImplementedException("未实现该类型的空类型检测" + dataType.ToString());
                }
            }
            return false;
        }
    }
}
