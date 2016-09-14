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

        private void button_Click(object sender, RoutedEventArgs e)
        {
            StringReader reader;
            StringBuilder result = new StringBuilder();
            var textLine = "";
            //Title
            result.AppendLine("PrintHelper.PrintTitle('" + tb_Title.Text + "')");
            //Definitions
            var definitions = tb_definitions.Text;
            if (cb_keepOrient.IsChecked.HasValue&& cb_keepOrient.IsChecked.Value)
            {
                reader = new StringReader(definitions);
                while (!string.IsNullOrEmpty(textLine = reader.ReadLine()))
                {
                    result.AppendLine(textLine);
                }
            }
            reader = new StringReader(definitions);
            while (!string.IsNullOrEmpty(textLine = reader.ReadLine()))
            {
                result.AppendLine(tb_prefixForDefinitions.Text + textLine + tb_suffixForDefinitions.Text);
            }
            //Samples
            var samples = tb_samples.Text;
            reader = new StringReader(samples);
            while (!string.IsNullOrEmpty(textLine = reader.ReadLine()))
            {
                result.AppendLine("PrintHelper.PrintSubtitle('" + textLine + "')");
                result.AppendLine(textLine);
            }
            tb_output.Text = result.ToString();
        }
    }
}
