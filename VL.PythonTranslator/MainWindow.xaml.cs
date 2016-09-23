using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VL.PythonTranslator
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        enum MultilineType
        {
            None,
            Backslash,
            TripleDoubleQuote,
            TripleSingleQuote,
        }
        private void button_Click(object sender, RoutedEventArgs e)
        {
            StringReader reader;
            StringBuilder result = new StringBuilder();
            var currentText = "";
            var definitions = tb_definitions.Text;
            ////Reference
            if (cb_addReference.IsChecked.HasValue && cb_addReference.IsChecked.Value)
            {
                reader = new StringReader(definitions);
                bool isNeedPrintHelper = true;
                for (int i = 0; i < 3; i++)
                {
                    if (!string.IsNullOrEmpty(currentText = reader.ReadLine()))
                    {
                        if (currentText== "import PrintHelper")
                        {
                            isNeedPrintHelper = false;
                        }
                    }
                }
                if (isNeedPrintHelper)
                {
                    result.AppendLine("import PrintHelper");
                }
            }
            ////Definitions
            //Oriented
            if (cb_keepOrient.IsChecked.HasValue && cb_keepOrient.IsChecked.Value)
            {
                reader = new StringReader(definitions);
                while (!string.IsNullOrEmpty(currentText = reader.ReadLine()))
                {
                    result.AppendLine(currentText);
                }
            }
            //Print
            AppendPrintText(tb_prefixForDefinitions.Text, tb_suffixForDefinitions.Text, result, new StringReader(definitions));
            tb_output.Text = result.ToString();
        }

        private static void AppendPrintText(string prefix, string suffix, StringBuilder result, StringReader reader)
        {
            string currentText, text;
            while ((currentText = reader.ReadLine()) != null)
            {
                if (currentText.StartsWith("c-")|| currentText.StartsWith("//"))
                {
                    text = currentText.Substring(2);
                    AppendContent(prefix, suffix, result, text);
                }
                else if (currentText.StartsWith("cm-"))
                {
                    List<string> multipleLineSample = new List<string>();
                    string subCurrentText;
                    while ((subCurrentText = reader.ReadLine()) != null)
                    {
                        if (subCurrentText.StartsWith("cm-"))
                        {
                            break;
                        }
                        multipleLineSample.Add(subCurrentText);
                    }
                    foreach (var line in multipleLineSample)
                    {
                        AppendContent(prefix, suffix, result, line);
                    }
                }
                else if (currentText.StartsWith("t-"))
                {
                    text = currentText.Substring(2);
                    AppendContent("PrintHelper.PrintTitle('", "')", result, text, false);
                }
                else if (currentText.StartsWith("sub-"))
                {
                    text = currentText.Substring(4);
                    AppendContent("PrintHelper.PrintSubtitle('", "')", result, text, false);
                }
                else if (currentText.StartsWith("h-"))
                {
                    text = currentText.Substring(2);
                    AppendContent("PrintHelper.PrintHint('", "')", result, text, false);
                }
                else if (currentText.StartsWith("s-"))
                {
                    text = currentText.Substring(2);
                    result.AppendLine("PrintHelper.PrintCode('" + GetEscapeSequencedText(text) + "')");
                    result.AppendLine(text);
                }
                else if (currentText.StartsWith("ss-"))
                {
                    List<string> multipleLineSample = new List<string>();
                    string subCurrentText;
                    while ((subCurrentText = reader.ReadLine()) != null)
                    {
                        if (subCurrentText.StartsWith("ss-"))
                        {
                            break;
                        }
                        multipleLineSample.Add(subCurrentText);
                    }
                    AppendSample(result, multipleLineSample, false);
                }
                else if (currentText.StartsWith("sm-"))
                {
                    List<string> multipleLineSample = new List<string>();
                    string subCurrentText;
                    while ((subCurrentText = reader.ReadLine()) != null)
                    {
                        if (subCurrentText.StartsWith("sm-"))
                        {
                            break;
                        }
                        multipleLineSample.Add(subCurrentText);
                    }
                    AppendSample(result, multipleLineSample, true);
                }
                else
                {
                    result.AppendLine(currentText);
                }
            }
        }

        private static void AppendContent(string prefix, string suffix, StringBuilder result,  string text,bool isSample=true)
        {
            if (isSample&&text.Contains("  "))
            {
                var texts = text.Split(new string[1] { "  " }, StringSplitOptions.RemoveEmptyEntries);
                if (texts.Count() == 2)
                {
                    result.AppendLine("PrintHelper.PrintSampleWithDescription('" + GetEscapeSequencedText(texts[0]) + "','" + GetEscapeSequencedText(texts[1]) + "')");
                }
                else if (texts.Count() < 2)
                {
                    result.AppendLine(prefix + GetEscapeSequencedText(text) + suffix);
                }
                else
                {
                    throw new NotImplementedException("暂不支持二段以上的多段式");
                }
            }
            else if (text.Contains("  "))
            {
                var texts = text.Split(new string[1] { "  " }, StringSplitOptions.RemoveEmptyEntries);
                result.AppendLine(prefix + string.Join("','", texts) + suffix);
            }
            else
            {
                result.AppendLine(prefix + GetEscapeSequencedText(text) + suffix);
            }
        }

        private static void AppendSample(StringBuilder result, List<string> previousTexts, bool isIndividual = true)
        {
            if (isIndividual)
            {
                foreach (var previousText in previousTexts)
                {
                    result.AppendLine("PrintHelper.PrintCode('" + GetEscapeSequencedText(previousText) + "')");
                }
                foreach (var previousText in previousTexts)
                {
                    result.AppendLine(previousText);
                }
            }
            else
            {
                foreach (var previousText in previousTexts)
                {
                    result.AppendLine("PrintHelper.PrintCode('" + GetEscapeSequencedText(previousText) + "')");
                    result.AppendLine(previousText);
                }
            }
        }

        private static string GetEscapeSequencedText(string previousText)
        {
            previousText = previousText.Replace("\\", "\\\\");
            previousText = previousText.Replace("'", "\\'");
            previousText = previousText.Replace("\"", "\\\"");
            return previousText;
        }
    }
}
