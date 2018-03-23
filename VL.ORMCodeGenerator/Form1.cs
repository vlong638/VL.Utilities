using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using VL.Common.Core.DAS;
using VL.ORMCodeGenerator.Generators;
using VL.ORMCodeGenerator.Objects.Entities;

namespace VL.ORMCodeGenerator
{
    public partial class Form1 : Form
    {
        private GenerateConfig _generateConfig = new GenerateConfig();
        public GenerateConfig CurrentGenerateConfig
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

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var dbTypes = Enum.GetNames(typeof(EDatabaseType));
            cb_dbType.DataSource = dbTypes;
            GenerateConfigs.Load();
            cb_source.DataSource = GenerateConfigs.Items.Select(c => c.PDMFilePath).ToList();
            cb_source.Refresh();
            var lastestItem = GenerateConfigs.Items.FirstOrDefault(c => c.PDMFilePath == GenerateConfigs.LastestPDMFilePath);
            if (lastestItem != null)
            {
                cb_source.SelectedItem = lastestItem;
                CurrentGenerateConfig = lastestItem;
            }
            else
            {
                GenerateConfigs.Items.FirstOrDefault(c => c.PDMFilePath == cb_source.Text);
            }
            if (CurrentGenerateConfig != null)
            {
                cb_source.Text = CurrentGenerateConfig.PDMFilePath;
                tb_target.Text = CurrentGenerateConfig.RootPath;
                tb_namespace.Text = CurrentGenerateConfig.RootNamespace;
                cb_dbType.Text = CurrentGenerateConfig.DatabaseType.ToString();
                cb_wcf.Checked = CurrentGenerateConfig.IsSupportWCF;
            }
        }
        private void btnPDMBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.FileName = this.cb_source.Text;
            dialog.Filter = "PowerDesigner PDM 文件(*.pdm)|*.pdm";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.cb_source.Text = dialog.FileName;
                CurrentGenerateConfig.PDMFilePath = this.cb_source.Text;
            }
        }
        private void btnCSharpEntityBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.FileName = this.tb_target.Text;
            dialog.Filter = "PowerDesigner PDM 文件(*.pdm)|*.pdm";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.tb_target.Text = dialog.FileName;
                CurrentGenerateConfig.PDMFilePath = this.tb_target.Text;
            }
        }
        private void ctlSourcePdmFile_TextChanged(object sender, EventArgs e)
        {
            CurrentGenerateConfig = GenerateConfigs.Items.FirstOrDefault(c => c.PDMFilePath == cb_source.Text);
            if (CurrentGenerateConfig == null)
            {
                CurrentGenerateConfig = new GenerateConfig();
                //新pdm地址 保留
                CurrentGenerateConfig.RootPath = tb_target.Text;
                CurrentGenerateConfig.RootNamespace = tb_namespace.Text;
                CurrentGenerateConfig.DatabaseType = (EDatabaseType)Enum.Parse(typeof(EDatabaseType), cb_dbType.Text);
                CurrentGenerateConfig.IsSupportWCF = cb_wcf.Checked;
            }
            //新pdm地址 清空
            tb_target.Text = CurrentGenerateConfig.RootPath;
            tb_namespace.Text = CurrentGenerateConfig.RootNamespace;
            cb_dbType.Text = CurrentGenerateConfig.DatabaseType.ToString();
            cb_wcf.Checked = CurrentGenerateConfig.IsSupportWCF;
        }
        private void ctlSourcePdmFile_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Back||e.KeyData==Keys.Delete)
            {
                GenerateConfigs.Items.Remove(CurrentGenerateConfig);
                cb_source.DataSource = GenerateConfigs.Items.Select(c => c.PDMFilePath).ToList();
                cb_source.Refresh();
                CurrentGenerateConfig = GenerateConfigs.Items.FirstOrDefault(c => c.PDMFilePath == cb_source.Text);
                if (CurrentGenerateConfig == null)
                {
                    CurrentGenerateConfig = new GenerateConfig();
                    cb_source.Text = "";
                }
                tb_target.Text = CurrentGenerateConfig.RootPath;
                tb_namespace.Text = CurrentGenerateConfig.RootNamespace;
                cb_dbType.Text = CurrentGenerateConfig.DatabaseType.ToString();
                cb_wcf.Checked = CurrentGenerateConfig.IsSupportWCF;
            }
            if (e.KeyData == Keys.Enter)
            {
                var generateConfig = GenerateConfigs.Items.FirstOrDefault(c => c.PDMFilePath == cb_source.Text);
                if (generateConfig != null && !string.IsNullOrEmpty(generateConfig.PDMFilePath))
                {
                    CurrentGenerateConfig = generateConfig;
                }
                else if (generateConfig != null)
                {
                    CurrentGenerateConfig = new GenerateConfig();
                }
                else
                {
                    CurrentGenerateConfig = new GenerateConfig();
                    CurrentGenerateConfig.PDMFilePath = cb_source.Text;
                    GenerateConfigs.Items.Add(CurrentGenerateConfig);
                }
                //cb_source.Text = GenerateConfig.PDMFilePath;
                tb_target.Text = CurrentGenerateConfig.RootPath;
                tb_namespace.Text = CurrentGenerateConfig.RootNamespace;
                cb_dbType.Text = CurrentGenerateConfig.DatabaseType.ToString();
                cb_wcf.Checked = CurrentGenerateConfig.IsSupportWCF;
            }
        }
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
            var result = GenerateFacade.Generate(CurrentGenerateConfig);
            //保存Config
            if (result)
            {
                GenerateConfigs.Save();
            }
            MessageBox.Show("生成执行" + (result ? "成功" : "失败"));
        }
        private void SaveConfig_Click(object sender, EventArgs e)
        {
            if (!UpdateConfig())
            {
                return;
            }
            if (!CheckConfig())
            {
                return;
            }
            GenerateConfigs.Save();
            MessageBox.Show("保存配置成功");
        }
        private bool UpdateConfig()
        {
            if (string.IsNullOrEmpty(cb_source.Text))
            {
                MessageBox.Show("请输入pdm文件路径");
                return false;
            }
            if (GenerateConfigs.Items.FirstOrDefault(c=>c.PDMFilePath==CurrentGenerateConfig.PDMFilePath)==null)
            {
                CurrentGenerateConfig = new GenerateConfig();
                GenerateConfigs.Items.Add(CurrentGenerateConfig);
            }
            CurrentGenerateConfig.PDMFilePath = cb_source.Text;
            CurrentGenerateConfig.RootPath = tb_target.Text;
            CurrentGenerateConfig.RootNamespace = tb_namespace.Text;
            CurrentGenerateConfig.IsSupportWCF = cb_wcf.Checked;
            if (!string.IsNullOrEmpty(cb_dbType.Text))
            {
                CurrentGenerateConfig.DatabaseType = (EDatabaseType)Enum.Parse(typeof(EDatabaseType), cb_dbType.Text);
            }
            GenerateConfigs.LastestPDMFilePath = CurrentGenerateConfig.PDMFilePath;
            return true;
        }
        private bool CheckConfig()
        {
            if (string.IsNullOrEmpty(CurrentGenerateConfig.PDMFilePath) || !File.Exists(CurrentGenerateConfig.PDMFilePath))
            {
                MessageBox.Show("请输入有效的" + nameof(CurrentGenerateConfig.PDMFilePath));
                return false;
            }
            if (string.IsNullOrEmpty(CurrentGenerateConfig.RootNamespace))
            {
                MessageBox.Show("请输入有效的" + nameof(CurrentGenerateConfig.RootNamespace));
                return false;
            }
            if (CurrentGenerateConfig.DatabaseType == EDatabaseType.None)
            {
                MessageBox.Show("选择输入有效的" + nameof(CurrentGenerateConfig.DatabaseType));
                return false;
            }
            return true;
        }









        //private void DisplaySubconfig()
        //{
        //    cb_source.Text = GenerateConfig.PDMFilePath;
        //    tb_target.Text = GenerateConfig.RootPath;
        //    tb_namespace.Text = GenerateConfig.RootNamespace;
        //    cb_dbType.Text = GenerateConfig.DatabaseType.ToString();
        //    cb_wcf.Checked = GenerateConfig.IsSupportWCF;
        //}


        //private void DisplayConfig()
        //{
        //    string currentPdmFile = cb_source.Text;
        //    cb_source.DataSource = GenerateConfigs.Items.Select(c => c.PDMFilePath).ToList();
        //    cb_source.Refresh();
        //    cb_source.Text = currentPdmFile;
        //    GenerateConfig = GenerateConfigs.Items.FirstOrDefault(c => c.PDMFilePath == currentPdmFile);
        //    if (GenerateConfig != null)
        //    {
        //        DisplaySubconfig();
        //    }
        //}
        //private void AddSubConfig()
        //{
        //    var generateConfig = GenerateConfigs.Items.FirstOrDefault(c => c.PDMFilePath == cb_source.Text);
        //    if (generateConfig != null && !string.IsNullOrEmpty(generateConfig.PDMFilePath))
        //    {
        //        GenerateConfig = generateConfig;
        //    }
        //    else if (generateConfig != null)
        //    {
        //        GenerateConfig = new GenerateConfig();
        //    }
        //    else
        //    {
        //        GenerateConfig = new GenerateConfig();
        //        GenerateConfig.PDMFilePath = cb_source.Text;
        //        GenerateConfigs.Items.Add(GenerateConfig);
        //    }
        //}
    }
}
