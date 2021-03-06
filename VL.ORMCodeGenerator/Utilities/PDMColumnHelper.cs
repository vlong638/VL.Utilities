﻿using PdPDM;
using System.Globalization;

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
            return (!string.IsNullOrEmpty(column.GetExtendedAttributeText("Enum"))) && column.GetExtendedAttributeText("Enum") != "<None>";
        }
        public static string GetEnumType(this Column column)
        {
            return column.GetExtendedAttributeText("Enum");
        }
        public static bool IsNullableField(this Column column)
        {
            if (!column.Mandatory)
            {
                if (column.IsEnumField())
                    return false;
                var dataType = column.GetPDMDataType();
                return dataType.IsNullableType();
            }
            return false;
        }
    }
}
