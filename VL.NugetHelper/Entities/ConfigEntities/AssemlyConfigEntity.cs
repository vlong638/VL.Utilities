using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using VL.Common.Configurator.Objects;

namespace VL.NugetHelper.Entities.ConfigEntities
{
    /// <summary>
    /// 装配配置
    /// 负责记录项目的装配信息
    /// </summary>
    public class AssemlyConfigEntity : TextConfigEntity
    {
        #region 源数据
        //[assembly: AssemblyTitle("VL.NugetHelper")]
        //[assembly: AssemblyDescription("VL.NugetHelper")]
        //[assembly: AssemblyConfiguration("")]
        //[assembly: AssemblyCompany("VL")]
        //[assembly: AssemblyProduct("VL.NugetHelper")]
        //[assembly: AssemblyCopyright("Copyright ©  2016")]
        //[assembly: AssemblyTrademark("")]
        //[assembly: AssemblyCulture("")]
        //[assembly: AssemblyVersion("1.0.0.0")]
        //[assembly: AssemblyFileVersion("1.0.0.0")] 
        #endregion

        public string Title { set; get; }
        public string Description { set; get; }
        public string Configuration { set; get; }
        public string Company { set; get; }
        public string Product { set; get; }
        public string Copyright { set; get; }
        public string Trademark { set; get; }
        public string Culture { set; get; }
        public string Version { set; get; }
        public string FileVersion { set; get; }
        public List<string> Contents { set; get; }

        public AssemlyConfigEntity(string fileName, string directoryPath, bool isInitFromFile = false) : base(fileName, directoryPath, isInitFromFile)
        {
            Title = "";
            Description = "";
            Configuration = "";
            Company = "";
            Product = "";
            Copyright = "";
            Trademark = "";
            Culture = "";
            Version = "";
            FileVersion = "";
            Contents = new List<string>();
        }

        public override string ToContent()
        {
            StringBuilder newContent = new StringBuilder();
            foreach (var content in Contents)
            {
                string text = content;
                //[assembly: AssemblyTitle("VL.NugetHelper")]
                if (text.StartsWith("[assembly: AssemblyTitle"))
                {
                    text = "[assembly: AssemblyTitle(\"" + Title + "\")]";
                }
                //[assembly: AssemblyDescription("VL.NugetHelper")]
                if (text.StartsWith("[assembly: AssemblyDescription"))
                {
                    text = "[assembly: AssemblyDescription(\"" + Description + "\")]";
                }
                //[assembly: AssemblyConfiguration("")]
                if (text.StartsWith("[assembly: AssemblyConfiguration"))
                {
                    text = "[assembly: AssemblyConfiguration(\"" + Configuration + "\")]";
                }
                //[assembly: AssemblyCompany("VL")]
                if (text.StartsWith("[assembly: AssemblyCompany"))
                {
                    text = "[assembly: AssemblyCompany(\"" + Company + "\")]";
                }
                //[assembly: AssemblyProduct("VL.NugetHelper")]
                if (text.StartsWith("[assembly: AssemblyProduct"))
                {
                    text = "[assembly: AssemblyProduct(\"" + Product + "\")]";
                }
                //[assembly: AssemblyCopyright("Copyright ©  2016")]
                if (text.StartsWith("[assembly: AssemblyCopyright"))
                {
                    text = "[assembly: AssemblyCopyright(\"" + Copyright + "\")]";
                }
                //[assembly: AssemblyTrademark("")]
                if (text.StartsWith("[assembly: AssemblyTrademark"))
                {
                    text = "[assembly: AssemblyTrademark(\"" + Trademark + "\")]";
                }
                //[assembly: AssemblyCulture("")]
                if (text.StartsWith("[assembly: AssemblyCulture"))
                {
                    text = "[assembly: AssemblyCulture(\"" + Culture + "\")]";
                }
                //[assembly: AssemblyVersion("1.0.0.0")]
                if (text.StartsWith("[assembly: AssemblyVersion"))
                {
                    text = "[assembly: AssemblyVersion(\"" + Version + "\")]";
                }
                //[assembly: AssemblyFileVersion("1.0.0.0")] 
                if (text.StartsWith("[assembly: AssemblyFileVersion"))
                {
                    text = "[assembly: AssemblyFileVersion(\"" + FileVersion + "\")]";
                }
                newContent.AppendLine(text);
            }
            return newContent.ToString();
        }

        protected override void Load(StreamReader stream)
        {
            string text;
            while ((text = stream.ReadLine()) != null)
            {
                //[assembly: AssemblyTitle("VL.NugetHelper")]
                if (text.StartsWith("[assembly: AssemblyTitle"))
                {
                    Regex regex = new Regex(@"\[assembly: AssemblyTitle\(""(.*)""\)\]");
                    var matches = regex.Match(text);
                    if (!string.IsNullOrEmpty(matches.Groups[1].Value))
                    {
                        Title = matches.Groups[1].Value;
                    }
                }
                //[assembly: AssemblyDescription("VL.NugetHelper")]
                if (text.StartsWith("[assembly: AssemblyDescription"))
                {
                    Regex regex = new Regex(@"\[assembly: AssemblyDescription\(""(.*)""\)\]");
                    var matches = regex.Match(text);
                    if (!string.IsNullOrEmpty(matches.Groups[1].Value))
                    {
                        Description = matches.Groups[1].Value;
                    }
                }
                //[assembly: AssemblyConfiguration("")]
                if (text.StartsWith("[assembly: AssemblyConfiguration"))
                {
                    Regex regex = new Regex(@"\[assembly: AssemblyConfiguration\(""(.*)""\)\]");
                    var matches = regex.Match(text);
                    if (!string.IsNullOrEmpty(matches.Groups[1].Value))
                    {
                        Configuration = matches.Groups[1].Value;
                    }
                }
                //[assembly: AssemblyCompany("VL")]
                if (text.StartsWith("[assembly: AssemblyCompany"))
                {
                    Regex regex = new Regex(@"\[assembly: AssemblyCompany\(""(.*)""\)\]");
                    var matches = regex.Match(text);
                    if (!string.IsNullOrEmpty(matches.Groups[1].Value))
                    {
                        Company = matches.Groups[1].Value;
                    }
                }
                //[assembly: AssemblyProduct("VL.NugetHelper")]
                if (text.StartsWith("[assembly: AssemblyProduct"))
                {
                    Regex regex = new Regex(@"\[assembly: AssemblyProduct\(""(.*)""\)\]");
                    var matches = regex.Match(text);
                    if (!string.IsNullOrEmpty(matches.Groups[1].Value))
                    {
                        Product = matches.Groups[1].Value;
                    }
                }
                //[assembly: AssemblyCopyright("Copyright ©  2016")]
                if (text.StartsWith("[assembly: AssemblyCopyright"))
                {
                    Regex regex = new Regex(@"\[assembly: AssemblyCopyright\(""(.*)""\)\]");
                    var matches = regex.Match(text);
                    if (!string.IsNullOrEmpty(matches.Groups[1].Value))
                    {
                        Copyright = matches.Groups[1].Value;
                    }
                }
                //[assembly: AssemblyTrademark("")]
                if (text.StartsWith("[assembly: AssemblyTrademark"))
                {
                    Regex regex = new Regex(@"\[assembly: AssemblyTrademark\(""(.*)""\)\]");
                    var matches = regex.Match(text);
                    if (!string.IsNullOrEmpty(matches.Groups[1].Value))
                    {
                        Trademark = matches.Groups[1].Value;
                    }
                }
                //[assembly: AssemblyCulture("")]
                if (text.StartsWith("[assembly: AssemblyCulture"))
                {
                    Regex regex = new Regex(@"\[assembly: AssemblyCulture\(""(.*)""\)\]");
                    var matches = regex.Match(text);
                    if (!string.IsNullOrEmpty(matches.Groups[1].Value))
                    {
                        Culture = matches.Groups[1].Value;
                    }
                }
                //[assembly: AssemblyVersion("1.0.0.0")]
                if (text.StartsWith("[assembly: AssemblyVersion"))
                {
                    Regex regex = new Regex(@"\[assembly: AssemblyVersion\(""(.*)""\)\]");
                    var matches = regex.Match(text);
                    if (!string.IsNullOrEmpty(matches.Groups[1].Value))
                    {
                        Version = matches.Groups[1].Value;
                    }
                }
                //[assembly: AssemblyFileVersion("1.0.0.0")] 
                if (text.StartsWith("[assembly: AssemblyFileVersion"))
                {
                    Regex regex = new Regex(@"\[assembly: AssemblyFileVersion\(""(.*)""\)\]");
                    var matches = regex.Match(text);
                    if (!string.IsNullOrEmpty(matches.Groups[1].Value))
                    {
                        FileVersion = matches.Groups[1].Value;
                    }
                }
                Contents.Add(text);
            }
        }
    }
}
