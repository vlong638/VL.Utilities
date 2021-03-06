﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using VL.Common.Object.Protocol;

namespace VL.ToolKit
{
    public partial class ECodeParser : Form
    {
        public ECodeParser()
        {
            InitializeComponent();
        }

        private void Parse_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tb_enum.Text))
                return;

            StringBuilder result = new StringBuilder();
            result.AppendLine("            AddMessages(result.Code, new KeyValueCollection {");
            result.AppendLine(string.Format("                            new KeyValue({0}, \"{1}\") ,", CProtocol.CReport.CError, "系统内部异常"));
            var reader = new StringReader(tb_enum.Text);
            string line;
            int index = -1;
            while ((line = reader.ReadLine())!=null )
            {
                if (line=="" || line.StartsWith("public" )|| line.StartsWith("{" )|| line.StartsWith("}"))
                {
                    continue;
                }
                if (index==-1)
                {
                    var regex = new Regex(@"CProtocol\.CReport\.CManualStart");
                    if (regex.IsMatch(line))
                    {
                        index = 11;
                    }
                    continue;
                }
                else
                {
                    var regex = new Regex(@"([\w]+)(\s?=\s?(\d+))?,?");
                    if (regex.IsMatch(line))
                    {
                        var match = regex.Match(line);
                        if (match.Groups[3].Value !="")
                        {
                            index = Convert.ToInt32(match.Groups[3].Value);
                        }
                        result.AppendLine(string.Format("                            new KeyValue({0}, \"{1}\") ,", index, match.Groups[1]));
                    }
                    index += 1;
                }
            }
            result.AppendLine("                        });");
            tb_KeyValueCollection.Text = result.ToString();
        }

        public Keys Pre { set; get; } = Keys.None;
        private void tb_KeyValueCollection_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyData&Keys.Control)>0)
            {
                Pre = Keys.Control;
            }
            if ((e.KeyData&Keys.C)>0)
            {
                if (Pre!=Keys.Control)
                    return;
                Clipboard.SetDataObject(tb_KeyValueCollection.Text);
            }
        }

        private void tb_KeyValueCollection_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Control)
            {
                Pre = Keys.None;
            }
        }
    }
}
//public enum ECode_Edit
//{
//    Default = CProtocol.CReport.CManualStart,
//    用户名不可为空,
//    标题不可为空,
//    内容不可少于十个字符,
//    内容新建失败 = 15,
//    主体新建失败,
//    内容更新失败,
//    主体更新失败,
//}