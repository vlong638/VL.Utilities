using System;
using System.Windows.Forms;

namespace VL.ORMCodeGenerator
{
    public partial class MyProgressBar : UserControl
    {
        public MyProgressBar()
        {
            InitializeComponent();
        }

        private void MyProgressBar_Load(object sender, EventArgs e)
        {
        }


        public void SetMax(int max)
        {
            this.progressBar1.Maximum = max;
            this.progressBar1.Value = 0;
            Application.DoEvents();
        }
        public void SetCurrent(int value)
        {
            this.progressBar1.Value = value;
            Application.DoEvents();
        }
        public void ShowMe(string text)
        {
            this.label1.Text = text;
            this.Visible = true;
            Application.DoEvents();
        }
        public void HideMe()
        {
            this.Visible = false;
            Application.DoEvents();
        }
    }
}
