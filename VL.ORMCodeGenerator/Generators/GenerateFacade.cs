using PdCommon;
using PdPDM;
using VL.Common.Core.DAS;
using VL.ORMCodeGenerator.Objects.Entities;

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
                case EDatabaseType.SQLite:
                    new PDMModelGenerator().Generate(config, model);
                    model.Close();
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
