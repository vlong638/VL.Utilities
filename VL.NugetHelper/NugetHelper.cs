﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using VL.NugetHelper.Entities.ConfigEntities;

namespace VL.NugetHelper
{
    public partial class NugetHelper : Form
    {
        ProjectsConfigEntity ProjectsConfigEntity { set; get; }
        AssemlyConfigEntity AssemlyConfigEntity { set; get; }

        public NugetHelper()
        {
            InitializeComponent();
            //ProjectsConfigEntity = new ProjectsConfigEntity("", System.Environment.CurrentDirectory);
            //ProjectsConfigEntity.Save();

            ProjectsConfigEntity = new ProjectsConfigEntity("Projects.config", System.Environment.CurrentDirectory);
            LoadProjectsConfigEntity();
            //WriteText("结束初始化配置");
        }

        private void LoadProjectsConfigEntity()
        {
            try
            {
                ProjectsConfigEntity.Load();
                //NugetServer配置
                tb_NugetServer.Text = ProjectsConfigEntity.NugetServer;
                tb_APIKey.Text = ProjectsConfigEntity.APIKey;
                //项目名称下拉列表
                cb_projects.DataSource = ProjectsConfigEntity.Projects.Select(c => c.Name).OrderBy(c => c).ToList();
                cb_projects.DisplayMember = nameof(ProjectDetail.Name);
                cb_projects.Refresh();
            }
            catch (Exception ex)
            {
                ProjectsConfigEntity.Save();
            }
        }

        #region Projects
        private void UpdateProjectsConfigEntity()
        {
            //更新
            ProjectsConfigEntity.NugetServer = tb_NugetServer.Text;
            ProjectsConfigEntity.APIKey = tb_APIKey.Text;
            //更新当前在选的程序
            ProjectDetail project = null;
            if (!string.IsNullOrEmpty(cb_projects.Text))
            {
                project = ProjectsConfigEntity.Projects.FirstOrDefault(c => c.Name == cb_projects.Text);
            }
            if (project == null)
            {
                project = new ProjectDetail(cb_projects.Text, tb_Author.Text, tb_projectRootPath.Text);
                project.SetDependencesString(tb_Dependences.Text);
                ProjectsConfigEntity.Projects.Add(project);
            }
            else
            {
                project.Author = tb_Author.Text;
                project.RootPath = tb_projectRootPath.Text;
                project.SetDependencesString(tb_Dependences.Text);
            }
            WriteText("已更新" + nameof(ProjectsConfigEntity));
        }
        private void Delete_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cb_projects.Text))
            {
                var project = ProjectsConfigEntity.Projects.FirstOrDefault(c => c.Name == cb_projects.Text);
                if (project != null)
                {
                    ProjectsConfigEntity.Projects.Remove(project);
                    var dataSource = ProjectsConfigEntity.Projects.Select(c => c.Name).ToList();
                    if (dataSource.Count == 0)
                    {
                        cb_projects.SelectedItem = null;
                        tb_Author.Text = null;
                        tb_projectRootPath.Text = null;
                        tb_Dependences.Text = null;
                    }
                    cb_projects.DataSource = dataSource;
                    cb_projects.DisplayMember = nameof(ProjectDetail.Name);
                    cb_projects.Refresh();
                    WriteText("删除成功");
                }
                else
                {
                    WriteText("该项目未创建");
                }
            }
            else
            {
                WriteText("请选择有效的项目项");
            }
        }
        private void SaveProjectsConfig_Click(object sender, EventArgs e)
        {
            SaveAll();
        }
        private void SaveAll()
        {
            UpdateProjectsConfigEntity();
            UpdateAssemlyConfigEntity();
            ProjectsConfigEntity.Save();
            AssemlyConfigEntity.Save();
            WriteText("已保存配置");
        }
        private void cb_projects_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (!string.IsNullOrEmpty(cb_projects.Text))
            //{
            //    LoadProjectDetail();
            //    LoadAssemlyConfigEntity();
            //}
        }

        private void LoadProjectDetail()
        {
            var project = ProjectsConfigEntity.Projects.FirstOrDefault(c => c.Name == cb_projects.Text);
            if (project == null)
            {
                project = new ProjectDetail(cb_projects.Text, "", "");
            }
            tb_Author.Text = project.Author;
            tb_projectRootPath.Text = project.RootPath;
            //引用项目
            tb_Dependences.Text = project.GetDependencesString();
            WriteText("已加载项目配置" + cb_projects.Text);
        }
        #endregion

        #region Assemblies
        private void LoadAssemlyConfigEntity()
        {
            //if (!string.IsNullOrEmpty(cb_projects.Text) && ProjectsConfigEntity.Projects.FirstOrDefault(c => c.Name == cb_projects.Text) != null)
            //{
            //    var project = ProjectsConfigEntity.Projects.FirstOrDefault(c => c.Name == cb_projects.Text);
            //    AssemlyConfigEntity = new AssemlyConfigEntity("AssemblyInfo.cs", Path.Combine(project.RootPath, "Properties"));
            //    AssemlyConfigEntity.Load();
            //}
            //else
            //{
            //    AssemlyConfigEntity = new AssemlyConfigEntity("", "");
            //}
            AssemlyConfigEntity = new AssemlyConfigEntity("AssemblyInfo.cs", Path.Combine(tb_projectRootPath.Text, "Properties"));
            if (File.Exists(AssemlyConfigEntity.InputFilePath))
            {
                AssemlyConfigEntity.Load();

            }
            tb_Title.Text = AssemlyConfigEntity.Title;
            tb_Description.Text = AssemlyConfigEntity.Description;
            tb_Configuration.Text = AssemlyConfigEntity.Configuration;
            tb_Company.Text = AssemlyConfigEntity.Company;
            tb_Product.Text = AssemlyConfigEntity.Product;
            tb_Copyright.Text = AssemlyConfigEntity.Copyright;
            tb_Trademark.Text = AssemlyConfigEntity.Trademark;
            tb_Culture.Text = AssemlyConfigEntity.Culture;
            tb_Version.Text = AssemlyConfigEntity.Version;
            tb_FileVersion.Text = AssemlyConfigEntity.FileVersion;
            WriteText("已加载引用配置" + AssemlyConfigEntity.Title);
        }
        private void UpdateAssemlyConfigEntity()
        {
            if (AssemlyConfigEntity==null)
                AssemlyConfigEntity = new AssemlyConfigEntity("");
            AssemlyConfigEntity.Title = tb_Title.Text;
            AssemlyConfigEntity.Description = tb_Description.Text;
            AssemlyConfigEntity.Configuration = tb_Configuration.Text;
            AssemlyConfigEntity.Company = tb_Company.Text;
            AssemlyConfigEntity.Product = tb_Product.Text;
            AssemlyConfigEntity.Copyright = tb_Copyright.Text;
            AssemlyConfigEntity.Trademark = tb_Trademark.Text;
            AssemlyConfigEntity.Culture = tb_Culture.Text;
            AssemlyConfigEntity.Version = tb_Version.Text;
            AssemlyConfigEntity.FileVersion = tb_FileVersion.Text;
            WriteText("已更新" + nameof(AssemlyConfigEntity));
        }
        #endregion

        #region Nuget Packages
        protected class NugetManager
        {
            public string SourceRootPath { set; get; }
            public string OutputDirectoryPath { set; get; }

            public NugetManager(string sourceRootPath, string projectName)
            {
                SourceRootPath = sourceRootPath;
                OutputDirectoryPath = Path.Combine(Environment.CurrentDirectory, projectName);
                if (!Directory.Exists(OutputDirectoryPath))
                {
                    Directory.CreateDirectory(OutputDirectoryPath);
                }
            }

            public void GenerateNugetSpec(ProjectDetail project, AssemlyConfigEntity assembly)
            {
                var content = GetNugetSpecContent(project, assembly);
                File.WriteAllText(Path.Combine(SourceRootPath, GetProjectName(project.RootPath) + ".nuspec"), content.ToString());
            }
            private static StringBuilder GetNugetSpecContent(ProjectDetail project, AssemlyConfigEntity assembly)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat(@"<?xml version=""1.0""?>" + System.Environment.NewLine);
                sb.AppendFormat(@"<package>" + System.Environment.NewLine);
                sb.AppendFormat(@"  <metadata>" + System.Environment.NewLine);
                sb.AppendFormat(@"    <id>{0}</id>" + System.Environment.NewLine, assembly.Title);
                sb.AppendFormat(@"    <version>{0}</version>" + System.Environment.NewLine, assembly.Version);
                sb.AppendFormat(@"    <authors>{0}</authors>" + System.Environment.NewLine, project.Author);
                sb.AppendFormat(@"    <owners>{0}</owners>" + System.Environment.NewLine, project.Author);
                //sb.AppendFormat(@"    <licenseUrl>{0}</licenseUrl>" + System.Environment.NewLine);
                //sb.AppendFormat(@"    <projectUrl>{0}</projectUrl>" + System.Environment.NewLine);
                //sb.AppendFormat(@"    <iconUrl>{0}</iconUrl>" + System.Environment.NewLine);
                //sb.AppendFormat(@"    <requireLicenseAcceptance>{0}</requireLicenseAcceptance>" + System.Environment.NewLine);
                sb.AppendFormat(@"    <description>{0}</description>" + System.Environment.NewLine, assembly.Description.Trim('\n').Trim('\r'));
                //sb.AppendFormat(@"    <releaseNotes>{0}</releaseNotes>" + System.Environment.NewLine);
                sb.AppendFormat(@"    <copyright>{0}</copyright>" + System.Environment.NewLine, assembly.Copyright);
                sb.AppendFormat(@"    <tags>{0}</tags>" + System.Environment.NewLine, project.Notes);
                //if (project.Dependences.Count() != 0)
                //{
                //    sb.AppendFormat(@"    <dependencies>" + System.Environment.NewLine);
                //    foreach (var references in project.Dependences)
                //    {
                //        sb.AppendFormat("      <dependency id=\"" + references.Key + "\" version=\"" + references.Value + "\" />" + System.Environment.NewLine);
                //    }
                //    sb.AppendFormat(@"    </dependencies>" + System.Environment.NewLine);
                //}
                //if (project.References.Count() != 0)
                //{
                //    sb.AppendFormat(@"    <references>" + System.Environment.NewLine);
                //    foreach (var reference in project.References)
                //    {
                //        sb.AppendFormat("      <reference file=\"" + reference.Substring(reference.IndexOf('\\') + 1) + "\" />" + System.Environment.NewLine);
                //    }
                //    sb.AppendFormat(@"    </references>" + System.Environment.NewLine);
                //}
                sb.AppendFormat(@"  </metadata>" + System.Environment.NewLine);
                if (project.References.Count() != 0)
                {
                    sb.AppendFormat(@"  <files>" + System.Environment.NewLine);
                    foreach (var reference in project.References)
                    {
                        sb.AppendFormat("      <file src=\"lib\\" + reference + "\"  target=\"lib\\" + reference.Substring(0, reference.IndexOf('\\')) + "\"/>" + System.Environment.NewLine);
                        //sb.AppendFormat("      <file src=\"lib\\net452\\" + reference + "\"  target=\"lib\\net452\"/>" + System.Environment.NewLine);
                        //sb.AppendFormat("      <file src=\"lib\\" + reference + "\"  target=\"lib\"/>" + System.Environment.NewLine);
                    }
                    sb.AppendFormat(@"  </files>" + System.Environment.NewLine);
                }
                sb.AppendFormat(@"</package>" + System.Environment.NewLine);
                return sb;
            }
            public string GetProjectFileFullPath(string rootPath)
            {
                var fileFullPaths = Directory.GetFiles(rootPath);
                foreach (var fileFullPath in fileFullPaths)
                {
                    if (fileFullPath.EndsWith(".csproj"))
                    {
                        return fileFullPath;
                    }
                }
                return "";
            }
            public string GetProjectName(string rootPath, bool withSuffix = false)
            {
                var fileFullPaths = Directory.GetFiles(rootPath);
                foreach (var fileFullPath in fileFullPaths)
                {
                    if (fileFullPath.EndsWith(".csproj"))
                    {
                        var result = fileFullPath.Substring(fileFullPath.LastIndexOf("\\") + 1);
                        if (!withSuffix)
                        {
                            result = result.Substring(0, result.LastIndexOf("."));
                        }
                        return result;
                    }
                }
                return "";
            }
            public List<string> GetNugetPackageFileFullPaths()
            {
                List<string> result = new List<string>();
                var fileFullPaths = Directory.GetFiles(OutputDirectoryPath);
                foreach (var fileFullPath in fileFullPaths)
                {
                    if (fileFullPath.EndsWith(".nupkg"))
                    {
                        result.Add(fileFullPath);
                    }
                }
                return result;
            }

            public StringBuilder ExecuteCmdCommand(string commandText)
            {
                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo.FileName = "cmd.exe";
                p.StartInfo.UseShellExecute = false;    //是否使用操作系统shell启动
                p.StartInfo.RedirectStandardInput = true;//接受来自调用程序的输入信息
                p.StartInfo.RedirectStandardOutput = true;//由调用程序获取输出信息
                p.StartInfo.RedirectStandardError = true;//重定向标准错误输出
                p.StartInfo.CreateNoWindow = true;//不显示程序窗口
                p.Start();//启动程序
                p.StandardInput.WriteLine(commandText);//向cmd窗口发送输入信息
                p.StandardInput.Close();//关闭输入,否则p.StandardInput将永远处于等待输入状态,导致p.StandardOutput无法结束获取
                //获取cmd窗口的输出信息
                StringBuilder sb = new StringBuilder();
                string output;
                while ((output = p.StandardOutput.ReadLine()) != null)
                {
                    sb.AppendLine(output);
                }
                while ((output = p.StandardError.ReadLine()) != null)
                {
                    sb.AppendLine(output);
                }
                p.StandardOutput.Close();
                p.WaitForExit();
                return sb;
            }
        }

        private void Generate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tb_Version.Text))
            {
                tb_Version.Text = "1.0.*";
            }
            SaveAll();
            if (!string.IsNullOrEmpty(cb_projects.Text))
            {
                var project = ProjectsConfigEntity.Projects.FirstOrDefault(c => c.Name == cb_projects.Text);
                if (project != null)
                {
                    NugetManager manager = new NugetManager(project.RootPath, project.Name);
                    //Remove Previous Package
                    var filePaths = manager.GetNugetPackageFileFullPaths();
                    foreach (var filePath in filePaths)
                    {
                        File.Delete(filePath);
                    }
                    //Spec
                    string tempVersion = GetTempVersion();
                    AssemlyConfigEntity.Version = tempVersion;
                    AssemlyConfigEntity.Description = tb_Description.Text + Environment.NewLine + tb_DescriptionEx.Text;
                    manager.GenerateNugetSpec(project, AssemlyConfigEntity);
                    AssemlyConfigEntity.Version = tb_Version.Text;
                    AssemlyConfigEntity.Description = tb_Description.Text;
                    //Pack
                    var projectFileFullPath = manager.GetProjectFileFullPath(project.RootPath);
                    if (string.IsNullOrEmpty(projectFileFullPath))
                    {
                        WriteText("未获取到该项目的.csproj项目文件");
                        return;
                    }
                    StringBuilder sb = manager.ExecuteCmdCommand("nuget pack " + projectFileFullPath + " -OutputDirectory " + manager.OutputDirectoryPath);
                    WriteText(sb.ToString());
                }
                else
                {
                    WriteText("该项目未创建");
                }
            }
            else
            {
                WriteText("请选择有效的项目项");
            }
        }

        private string GetTempVersion()
        {
            string tempVersion;
            if (tb_Version.Text.EndsWith("*"))
            {
                Regex regex = new Regex(@"(\d+.\d+.)\*");
                var match = regex.Match(tb_Version.Text);
                tempVersion = match.Groups[1].Value + DateTime.Now.ToString("yyMMddHHmm") + ".1";
            }
            else
            {
                Regex regex = new Regex(@"(\d+.\d+.)\d+(.\d+)");
                var match = regex.Match(tb_Version.Text);
                tempVersion = match.Groups[1].Value + DateTime.Now.ToString("yyMMddHHmm") + match.Groups[2].Value;
            }

            return tempVersion;
        }

        private void Push_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cb_projects.Text))
            {
                var project = ProjectsConfigEntity.Projects.FirstOrDefault(c => c.Name == cb_projects.Text);
                if (project != null)
                {
                    //Push
                    NugetManager manager = new NugetManager(project.RootPath, project.Name);
                    var filePaths = manager.GetNugetPackageFileFullPaths();
                    if (filePaths.Count() == 0)
                    {
                        WriteText("该项目未创建Nuget包");
                        return;
                    }
                    if (filePaths.Count() > 1)
                    {
                        WriteText("该项目存在多个包");
                        return;
                    }
                    StringBuilder sb = manager.ExecuteCmdCommand("nuget push " + filePaths.First() + " -s " + ProjectsConfigEntity.NugetServer + " " + ProjectsConfigEntity.APIKey);
                    WriteText(sb.ToString());
                }
                else
                {
                    WriteText("该项目未创建");
                }
            }
            else
            {
                WriteText("请选择有效的项目项");
            }
        }
        #endregion

        #region Output
        protected void WriteText(string pattern, params object[] args)
        {
            tb_output.AppendText(string.Format(pattern, args) + System.Environment.NewLine);
        }
        #endregion
        private void cb_projects_TextChanged(object sender, EventArgs e)
        {
            LoadProjectDetail();
            //LoadAssemlyConfigEntity();
        }

        private void tb_projectRootPath_TextChanged(object sender, EventArgs e)
        {
            LoadAssemlyConfigEntity();
        }
    }
}
