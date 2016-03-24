using PdPDM;
using VL.ORMCodeGenerator.Objects.Entities;

namespace VL.ORMCodeGenerator.Generators
{
    /// <summary>
    /// 基于PowerDesigner的代码生成器
    /// </summary>
    public abstract class IPDMGenerator
    {
        /// <summary>
        /// 生成代码
        /// </summary>
        /// <param name="config"></param>
        public abstract void Generate(GenerateConfig config, Model model);

        /// <summary>
        /// 生成Tables
        /// </summary>
        /// <param name="config"></param>
        /// <param name="tables"></param>
        /// <returns></returns>
        protected abstract bool GenerateTables(GenerateConfig config, ObjectCol tables);
        /// <summary>
        /// 生成References
        /// </summary>
        /// <param name="config"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        protected abstract bool GenerateReferences(GenerateConfig config, Model model);
        /// <summary>
        /// 生成Enums
        /// </summary>
        /// <param name="config"></param>
        /// <param name="tables"></param>
        /// <returns></returns>
        protected abstract bool GenerateEnums(GenerateConfig config, ObjectCol tables);

        //string targetFilePath = GenerateTargetFilePath(config);
        //string targetFileContent = GenerateTargetFilePath(config);
        //File.WriteAllText(targetFilePath, targetFileContent);
        ///// <summary>
        ///// 获取目标文件路径
        ///// </summary>
        ///// <param name="config"></param>
        ///// <returns></returns>
        //protected abstract string GenerateTargetFileContent(GenerateConfig config);
        ///// <summary>
        ///// 获取目标文件内容
        ///// </summary>
        ///// <param name="config"></param>
        ///// <returns></returns>
        //protected abstract string GenerateTargetFilePath(GenerateConfig config);
    }
}
