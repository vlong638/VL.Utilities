using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using VL.Common.Configurator.Objects.ConfigEntities;

namespace VL.NugetHelper.Entities.ConfigEntities
{
    /// <summary>
    /// 项目配置对象
    /// 负责记录项目和其路径信息
    /// </summary>
    public class ProjectsConfigEntity : XMLConfigEntity
    {
        public string NugetServer { set; get; } = "";
        public string APIKey { set; get; } = "";
        public Dictionary<string, string> Dependences { set; get; } = new Dictionary<string, string>();
        public List<ProjectDetail> Projects { set; get; } = new List<ProjectDetail>();

        public ProjectsConfigEntity(string fileName) : base(fileName)
        {
        }

        public ProjectsConfigEntity(string fileName, string directoryPath) : base(fileName, directoryPath)
        {
        }

        public override IEnumerable<XElement> GetXElements()
        {
            List<XElement> xElements = new List<XElement>();
            //Server
            XElement xServer = new XElement("Server"
                , new XAttribute(nameof(NugetServer), NugetServer)
                , new XAttribute(nameof(APIKey), APIKey));
            xElements.Add(xServer);
            //Project
            foreach (var project in Projects)
            {
                XElement xProject = new XElement("Project", 
                    new XAttribute(nameof(ProjectDetail.Name), project.Name),
                    new XAttribute(nameof(ProjectDetail.Author), project.Author),
                    new XAttribute(nameof(ProjectDetail.References), project.GetDependencesString()));
                xProject.Value = project.RootPath;
                xElements.Add(xProject);
            }
            return xElements;
        }

        protected override void Load(XDocument doc)
        {
            //Server
            var xServer = doc.Descendants("Server");
            if (xServer != null)
            {
                NugetServer = xServer.First().Attribute(nameof(NugetServer)).Value;
                APIKey = xServer.First().Attribute(nameof(APIKey)).Value;
            }
            //Project
            var configItems = doc.Descendants("Project");
            foreach (var configItem in configItems)
            {
                var detail = new ProjectDetail(configItem.Attribute(nameof(ProjectDetail.Name)).Value,
                    configItem.Attribute(nameof(ProjectDetail.Author)).Value,
                    configItem.Value);
                if (configItem.Attribute(nameof(ProjectDetail.References)) != null)
                {
                    detail.SetDependencesString(configItem.Attribute(nameof(ProjectDetail.References)).Value);
                }
                Projects.Add(detail);
            }
        }

        //public ProjectsConfigEntity(string fileName, string directoryPath, bool isInitFromFile = false) : base(fileName, directoryPath, isInitFromFile)
        //{
        //}

        //public override XElement ToXElement()
        //{
        //    XElement xProjects = new XElement("Projects");
        //    //Server
        //    XElement xServer = new XElement("Server"
        //        ,new XAttribute(nameof(NugetServer), NugetServer)
        //        , new XAttribute(nameof(APIKey), APIKey));
        //    xProjects.Add(xServer);
        //    //Project
        //    foreach (var project in Projects)
        //    {
        //        XElement xProject = new XElement("Project", new XAttribute(nameof(ProjectDetail.Name), project.Name), new XAttribute(nameof(ProjectDetail.Author), project.Author));
        //        xProject.Value = project.RootPath;
        //        xProjects.Add(xProject);
        //    }
        //    return xProjects;
        //}
        //protected override void Load(XDocument doc)
        //{
        //    //Server
        //    var xServer = doc.Descendants("Server");
        //    if (xServer!=null)
        //    {
        //        NugetServer = xServer.First().Attribute(nameof(NugetServer)).Value;
        //        APIKey = xServer.First().Attribute(nameof(APIKey)).Value;
        //    }
        //    //Project
        //    var configItems = doc.Descendants("Project");
        //    foreach (var configItem in configItems)
        //    {
        //        Projects.Add(new ProjectDetail(configItem.Attribute(nameof(ProjectDetail.Name)).Value, configItem.Attribute(nameof(ProjectDetail.Author)).Value, configItem.Value));
        //    }
        //}
    }
    /// <summary>
    /// 项目信息
    /// 项目总是有着(项目名,作者,路径,备注等信息)
    /// </summary>
    public class ProjectDetail
    {
        public string Name { set; get; }
        public string Author { set; get; }
        public string RootPath { set; get; }
        public string Notes { set; get; }
        public List<string> References { set; get; }

        public ProjectDetail(string name,string author, string rootPath)
        {
            Name = name;
            Author = author;
            RootPath = rootPath;
            References = new List<string>();
        }

        public void SetDependencesString(string referencesString)
        {
            if (string.IsNullOrEmpty(referencesString))
            {
                References = new List<string>();
                return;
            }
            var references= referencesString.Split('\r');
            References = new List<string>();
            foreach (string reference in references)
            {
                var value = reference.Trim('\n').Trim('\r');
                if (!string.IsNullOrEmpty(value))
                {
                    if (!value.EndsWith(".dll"))
                    {
                        value = value + ".dll";
                    }
                    value = value.Replace('/', '\\');
                    References.Add(value);
                }
            }
        }
        public string GetDependencesString()
        {
            return string.Join(System.Environment.NewLine, References);
        }
    }

}
