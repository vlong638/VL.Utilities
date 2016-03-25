namespace VL.ORMCodeGenerator.Objects.Constraits
{
    public class CGenerate
    {
        /// <summary>
        /// Class LeadingSpaces
        /// </summary>
        public static string ClassLS = "    ";
        /// <summary>
        /// NestedContent LeadingSpaces
        /// </summary>
        public static string MethodLS = "        ";
        /// <summary>
        /// MethodContent LeadingSpaces
        /// </summary>
        public static string ContentLS = "            ";
        /// <summary>
        /// Tab LeadingSpaces
        /// </summary>
        public static string TabLS = "    ";
        /// <summary>
        /// 输出文件名称后缀
        /// </summary>
        public static string FileNameSuffixOfReference = "Reference";
        /// <summary>
        /// 输出文件名称后缀
        /// </summary>
        public static string FileNameSuffixOfOperator = "Operator";
        /// <summary>
        /// 输出文件名称后缀
        /// </summary>
        public static string FileNameSuffixOfProperties = "Properties";
        /// <summary>
        /// 输出文件名称后缀
        /// </summary>
        public static string FileNameSuffixOfFetcher = "Fetcher";
        /// <summary>
        /// 输出类名称
        /// </summary>
        public static string ClassNameOfEntityOperator = "EntityOperator";
        /// <summary>
        /// 输出类名称
        /// </summary>
        public static string ClassNameOfEntityFetcher = "EntityFetcher";
        #region WCF
        /// <summary>
        /// WCF Class Contract
        /// </summary>
        public static string WCFClassContract = "[DataContract]";
        /// <summary>
        /// WCF Property Contract
        /// </summary>
        public static string WCFPropertyContract = "[DataMember]";
        /// <summary>
        /// WCF Enum Contract
        /// </summary>
        public static string WCFEnumContract = "[EnumMember]";
        #endregion

        #region PDM
        public static string PDMNameNotationOfTable = "T";
        public static string PDMNameNotationOfEnum = "E";
        public static string PDMNameNotationOfRelationMapper = "R";
        #endregion
    }
}
