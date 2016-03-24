using PdPDM;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using VL.Common.DAS.Objects;
using VL.ORMCodeGenerator.Generators;
using VL.ORMCodeGenerator.Objects.Entities;
using VL.ORMCodeGenerator.Utilities;

namespace VL.ORMCodeGenerator
{
    public partial class Form1 : Form
    {
        List<Table> Tables { get; set; }
        public GenerateConfig Config
        {
            get
            {
                if (_config == null)
                {
                    _config = CacheHelper.LoadGenerateConfig();
                }
                return _config;
            }
            set
            {
                _config = value;
            }
        }
        GenerateConfig _config;

        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 页面加载 初始化数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            //获取Config
            ctlSourcePdmFile.Text = Config.PDMFilePath;
            ctlCSharpEntityOutputFile.Text = Config.TargetDirectoryPath;
            ctlCSharpEntityNameSpace.Text = Config.TargetNamespace;
            ctlCSharpEntityWcfEnabled.Checked = Config.IsSupportWCF;
            ctlDbType.Text = Config.TargetDatabaseType.ToString();
        }
        /// <summary>
        /// 获取pdm文件路径
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPDMBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.FileName = this.ctlSourcePdmFile.Text;
            dialog.Filter = "PowerDesigner PDM 文件(*.pdm)|*.pdm";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.ctlSourcePdmFile.Text = dialog.FileName;
                Config.PDMFilePath = this.ctlSourcePdmFile.Text;
            }
        }
        /// <summary>
        /// 设置输出文件路径
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTargetDirectoryBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog f = new FolderBrowserDialog();
            if (f.ShowDialog() == DialogResult.OK)
            {
                this.ctlCSharpEntityOutputFile.Text = f.SelectedPath;
                Config.TargetDirectoryPath = this.ctlCSharpEntityOutputFile.Text;
            }
        }
        /// <summary>
        /// 执行生成方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCSharpEntityGenerate_Click(object sender, EventArgs e)
        {
            UpdateConfig();
            //根据Config执行生成
            var result = GenerateFacade.Generate(Config);
            //保存Config
            if (result)
            {
                CacheHelper.SaveGenerateConfig(Config);
            }
            MessageBox.Show("生成执行" + (result ? "成功" : "失败"));
        }

        private void UpdateConfig()
        {
            //获取Config
            Config.PDMFilePath = ctlSourcePdmFile.Text;
            Config.TargetDirectoryPath = ctlCSharpEntityOutputFile.Text;
            Config.TargetNamespace = ctlCSharpEntityNameSpace.Text;
            Config.IsSupportWCF = ctlCSharpEntityWcfEnabled.Checked;
            Config.TargetDatabaseType = (EDatabaseType)Enum.Parse(typeof(EDatabaseType), ctlDbType.Text);
        }

        /// <summary>
        /// 切换数据源pdm文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ctlSourcePdmFile_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        /// <summary>
        /// 切换输出文件路径
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ctlCSharpEntityOutputFile_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void SaveConfig_Click(object sender, EventArgs e)
        {
            UpdateConfig();
            CacheHelper.SaveGenerateConfig(Config);
            MessageBox.Show("保存配置成功");
        }
    }
}
