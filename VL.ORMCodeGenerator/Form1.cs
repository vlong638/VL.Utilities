using PdPDM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using VL.Common.DAS.Objects;
using VL.ORMCodeGenerator.Generators;
using VL.ORMCodeGenerator.Objects.Entities;

namespace VL.ORMCodeGenerator
{
    public partial class Form1 : Form
    {
        List<Table> Tables { get; set; }

        public GenerateConfig GenerateConfig
        {
            get
            {
                return _generateConfig;
            }

            set
            {
                _generateConfig = value;
            }
        }

        public GenerateConfigs GenerateConfigs = new GenerateConfigs(nameof(GenerateConfigs) + ".config", Path.Combine(Environment.CurrentDirectory, "Configs"));
        private GenerateConfig _generateConfig = new GenerateConfig();

        public Form1()
        {
            InitializeComponent();
            GenerateConfig = new GenerateConfig();
        }

        private void UpdateSourcePdmFile()
        {
            string currentPdmFile = ctlSourcePdmFile.Text;
            ctlSourcePdmFile.DataSource = GenerateConfigs.Items.Select(c => c.PDMFilePath).ToList();
            ctlSourcePdmFile.Text = currentPdmFile;
            //ctlSourcePdmFile.SelectedItem = currentPdmFile;
            GenerateConfig = GenerateConfigs.Items.FirstOrDefault(c => c.PDMFilePath == currentPdmFile);
            ctlSourcePdmFile.Refresh();
            if (GenerateConfig != null)
            {
                DisplayGenerateConfig();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GenerateConfigs.Load();
            UpdateSourcePdmFile();
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
                GenerateConfig.PDMFilePath = this.ctlSourcePdmFile.Text;
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
                GenerateConfig.RootPath = this.ctlCSharpEntityOutputFile.Text;
            }
        }
        /// <summary>
        /// 执行生成方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCSharpEntityGenerate_Click(object sender, EventArgs e)
        {
            if (!UpdateConfig())
            {
                return;
            }
            if (!CheckConfig())
            {
                return;
            }
            //根据Config执行生成
            var result = GenerateFacade.Generate(GenerateConfig);
            //保存Config
            if (result)
            {
                GenerateConfigs.Save();
            }
            MessageBox.Show("生成执行" + (result ? "成功" : "失败"));
        }
        private bool UpdateConfig()
        {
            if (string.IsNullOrEmpty(ctlSourcePdmFile.Text))
            {
                MessageBox.Show("请输入pdm文件路径");
                return false;
            }
            GenerateConfig.PDMFilePath = ctlSourcePdmFile.Text;
            GenerateConfig.RootPath = ctlCSharpEntityOutputFile.Text;
            GenerateConfig.RootNamespace = ctlCSharpEntityNameSpace.Text;
            GenerateConfig.IsSupportWCF = ctlCSharpEntityWcfEnabled.Checked;
            if (!string.IsNullOrEmpty(ctlDbType.Text))
            {
                GenerateConfig.DatabaseType = (EDatabaseType)Enum.Parse(typeof(EDatabaseType), ctlDbType.Text);
            }
            return true;
        }
        private bool CheckConfig()
        {
            if (string.IsNullOrEmpty(GenerateConfig.PDMFilePath) || !File.Exists(GenerateConfig.PDMFilePath))
            {
                MessageBox.Show("请输入有效的" + nameof(GenerateConfig.PDMFilePath));
                return false;
            }
            if (string.IsNullOrEmpty(GenerateConfig.RootNamespace))
            {
                MessageBox.Show("请输入有效的" + nameof(GenerateConfig.RootNamespace));
                return false;
            }
            if (GenerateConfig.DatabaseType == EDatabaseType.None)
            {
                MessageBox.Show("选择输入有效的" + nameof(GenerateConfig.DatabaseType));
                return false;
            }
            return true;
        }
        private void SaveConfig_Click(object sender, EventArgs e)
        {
            if (!UpdateConfig())
            {
                return;
            }
            GenerateConfigs.Save();
            MessageBox.Show("保存配置成功");
        }
        private void ctlSourcePdmFile_SelectedIndexChanged_1(object sender, EventArgs e)
        {
        }
        private void UpdateGenerateConfig()
        {
            var generateConfig= GenerateConfigs.Items.FirstOrDefault(c => c.PDMFilePath == ctlSourcePdmFile.Text);
            if (generateConfig!=null&& !string.IsNullOrEmpty(generateConfig.PDMFilePath))
            {
                GenerateConfig = generateConfig;

                DisplayGenerateConfig();
            }
            else if (generateConfig != null)
            {
                GenerateConfig = new GenerateConfig();
                DisplayGenerateConfig();
            }
            else
            {
                GenerateConfig = new GenerateConfig();
                GenerateConfig.PDMFilePath = ctlSourcePdmFile.Text;
                GenerateConfigs.Items.Add(GenerateConfig);

                DisplayGenerateConfig();
            }
        }
        private void DisplayGenerateConfig()
        {
            ctlSourcePdmFile.Text = GenerateConfig.PDMFilePath;
            ctlCSharpEntityOutputFile.Text = GenerateConfig.RootPath;
            ctlCSharpEntityNameSpace.Text = GenerateConfig.RootNamespace;
            ctlCSharpEntityWcfEnabled.Checked = GenerateConfig.IsSupportWCF;
            ctlDbType.Text = GenerateConfig.DatabaseType.ToString();
        }
        private void ctlSourcePdmFile_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Delete)
            {
                GenerateConfigs.Items.Remove(GenerateConfig);

                UpdateSourcePdmFile();
            }
            if (e.KeyData == Keys.Enter)
            {
                UpdateGenerateConfig();
                UpdateSourcePdmFile();
            }
        }

        private void ctlSourcePdmFile_TextChanged(object sender, EventArgs e)
        {
            UpdateGenerateConfig();
        }
    }
}
