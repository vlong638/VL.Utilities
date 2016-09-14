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
            ////Reference
            if (cb_addReference.IsChecked.HasValue && cb_addReference.IsChecked.Value)
            {
                result.AppendLine("import PrintHelper");
            }
            ////Title
            if (!string.IsNullOrEmpty(tb_Title.Text))
            {
                result.AppendLine("PrintHelper.PrintTitle('" + tb_Title.Text + "')");
            }
            ////Definitions
            //Oriented
            var definitions = tb_definitions.Text;
            if (cb_keepOrient.IsChecked.HasValue && cb_keepOrient.IsChecked.Value)
            {
                reader = new StringReader(definitions);
                while (!string.IsNullOrEmpty(currentText = reader.ReadLine()))
                {
                    result.AppendLine(currentText);
                }
            }
            //Print
            reader = new StringReader(definitions);
            while (!string.IsNullOrEmpty(currentText = reader.ReadLine()))
            {
                if (currentText.Contains("  "))
                {
                    var texts = currentText.Split(new string[1] { "  " }, StringSplitOptions.RemoveEmptyEntries);
                    result.AppendLine("PrintHelper.PrintSampleWithDescription('" + GetEscapeSequencedText(texts[0]) + "','" + GetEscapeSequencedText(texts[1]) + "')");
                }
                else
                {
                    result.AppendLine(tb_prefixForDefinitions.Text + GetEscapeSequencedText(currentText) + tb_suffixForDefinitions.Text);
                }
                //AppendPrintText(tb_prefixForDefinitions.Text, tb_suffixForDefinitions.Text, result, currentText);
            }
            ////Samples
            var samples = tb_samples.Text;
            reader = new StringReader(samples);
            currentText = reader.ReadLine();
            string previousText = "";
            List<string> previousTexts = new List<string>();
            MultilineType currentMultilineType = MultilineType.None;
            if (!string.IsNullOrEmpty(currentText))
            {
                do
                {
                    if (currentText.StartsWith(" ") || currentMultilineType != MultilineType.None)
                    {
                        switch (currentMultilineType)
                        {
                            case MultilineType.None:
                                break;
                            case MultilineType.Backslash:
                                if (!currentText.EndsWith("\\"))
                                {
                                    currentMultilineType = MultilineType.None;
                                }
                                break;
                            case MultilineType.TripleDoubleQuote:
                                if (currentText.Contains("\"\"\""))
                                {
                                    currentMultilineType = MultilineType.None;
                                }
                                break;
                            case MultilineType.TripleSingleQuote:
                                if (currentText.Contains("\'\'\'"))
                                {
                                    currentMultilineType = MultilineType.None;
                                }
                                break;
                            default:
                                throw new NotImplementedException("not implemented with type " + currentMultilineType.ToString());
                        }
                        previousTexts.Add(currentText);
                    }
                    else
                    {
                        PrintPreviousTexts(result, previousTexts);
                        previousTexts = new List<string>() { currentText };
                        if (currentText.EndsWith("\\"))
                        {
                            currentMultilineType = MultilineType.Backslash;
                        }
                        else if (currentText.Contains("\"\"\""))
                        {
                            currentMultilineType = MultilineType.TripleDoubleQuote;
                        }
                        else if (currentText.Contains("\'\'\'"))
                        {
                            currentMultilineType = MultilineType.TripleSingleQuote;
                        }
                    }
                    previousText = currentText;
                }
                while (!string.IsNullOrEmpty(currentText = reader.ReadLine()));
                if (previousTexts.Count > 0)
                {
                    PrintPreviousTexts(result, previousTexts);
                }
            }
            tb_output.Text = result.ToString();
        }

        private static void AppendPrintText(string prefix,string suffix,StringBuilder result, string currentText)
        {
            if (currentText.Contains("  "))
            {
                var texts = currentText.Split(new string[1] { "  " }, StringSplitOptions.RemoveEmptyEntries);
                result.AppendLine("PrintHelper.PrintSampleWithDescription('" + GetEscapeSequencedText(texts[0]) + "','" + GetEscapeSequencedText(texts[1]) + "')");
            }
            else
            {
                result.AppendLine(prefix+GetEscapeSequencedText(currentText) + suffix);
            }
        }

        private static void PrintPreviousTexts(StringBuilder result, List<string> previousTexts)
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

        private static string GetEscapeSequencedText(string previousText)
        {
            previousText = previousText.Replace("\\", "\\\\");
            previousText = previousText.Replace("'", "\\'");
            previousText = previousText.Replace("\"", "\\\"");
            return previousText;
        }
    }
}
