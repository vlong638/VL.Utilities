using PdCommon;
using PdPDM;
using VL.Common.DAS.Objects;
using VL.ORMCodeGenerator.Objects.Entities;
using VL.ORMCodeGenerator.Objects.Enums;

namespace VL.ORMCodeGenerator.Generators
{
    /// <summary>
    /// 代码生成窗口
    /// </summary>
    public class GenerateFacade
    {
        public static bool Generate(GenerateConfig config)
        {
            ApplicationClass pd = new ApplicationClass();
            //打开模型,并获取模型基类
            Model model = (Model)pd.OpenModel(config.PDMFilePath, OpenModelFlags.omf_Hidden);
            //根据模型生成
            switch (config.DatabaseType)
            {
                case EDatabaseType.MSSQL:
                case EDatabaseType.Oracle:
                case EDatabaseType.MySQL:
                    new PDMModelGenerator().Generate(config, model);
                    break;
                //new PDMGeneratorOfOracle().Generate(config, model);
                //break;
                //new PDMGeneratorOfMySQL().Generate(config, model);
                //break;
                default:
                    break;
            }
            return true;
        }
    }
}
