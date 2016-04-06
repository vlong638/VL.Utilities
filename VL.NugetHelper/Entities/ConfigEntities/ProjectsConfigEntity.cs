﻿using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using VL.Common.Configurator.Objects;

namespace VL.NugetHelper.Entities.ConfigEntities
{
    /// <summary>
    /// 项目配置对象
    /// 负责记录项目和其路径信息
    /// </summary>
    public class ProjectsConfigEntity : XConfigEntity
    {
        public string NugetServer { set; get; } = "";
        public string APIKey { set; get; } = "";
        public List<ProjectDetail> Projects { set; get; } = new List<ProjectDetail>();

        public ProjectsConfigEntity(string fileName, string directoryPath, bool isInitFromFile = false) : base(fileName, directoryPath, isInitFromFile)
        {
        }

        public override XElement ToXElement()
        {
            XElement xProjects = new XElement("Projects");
            //Server
            XElement xServer = new XElement("Server"
                ,new XAttribute(nameof(NugetServer), NugetServer)
                , new XAttribute(nameof(APIKey), APIKey));
            xProjects.Add(xServer);
            //Project
            foreach (var project in Projects)
            {
                XElement xProject = new XElement("Project", new XAttribute(nameof(ProjectDetail.Name), project.Name), new XAttribute(nameof(ProjectDetail.Author), project.Author));
                xProject.Value = project.RootPath;
                xProjects.Add(xProject);
            }
            return xProjects;
        }
        protected override void Load(XDocument doc)
        {
            //Server
            var xServer = doc.Descendants("Server");
            if (xServer!=null)
            {
                NugetServer = xServer.First().Attribute(nameof(NugetServer)).Value;
                APIKey = xServer.First().Attribute(nameof(APIKey)).Value;
            }
            //Project
            var configItems = doc.Descendants("Project");
            foreach (var configItem in configItems)
            {
                Projects.Add(new ProjectDetail(configItem.Attribute(nameof(ProjectDetail.Name)).Value, configItem.Attribute(nameof(ProjectDetail.Author)).Value, configItem.Value));
            }
        }
    }
    public class ProjectDetail
    {
        public string Name { set; get; }
        public string Author { set; get; }
        public string RootPath { set; get; }
        public string Notes { set; get; }
        public ProjectDetail(string name,string author, string rootPath)
        {
            Name = name;
            Author = author;
            RootPath = rootPath;
        }
    }
}
