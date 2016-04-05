namespace VL.ORMCodeGenerator
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ctlSourcePdmFile = new System.Windows.Forms.ComboBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.button1 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.ctlDbType = new System.Windows.Forms.ComboBox();
            this.ctlCSharpEntityWcfEnabled = new System.Windows.Forms.CheckBox();
            this.btnCSharpEntityGenerate = new System.Windows.Forms.Button();
            this.ctlCSharpEntityNameSpace = new System.Windows.Forms.ComboBox();
            this.ctlCSharpEntityOutputFile = new System.Windows.Forms.ComboBox();
            this.lblCSharpEntityNameSpace = new System.Windows.Forms.Label();
            this.btnCSharpEntityBrowse = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.myProgressBar1 = new VL.ORMCodeGenerator.MyProgressBar();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ctlSourcePdmFile
            // 
            this.ctlSourcePdmFile.FormattingEnabled = true;
            this.ctlSourcePdmFile.Location = new System.Drawing.Point(66, 12);
            this.ctlSourcePdmFile.Name = "ctlSourcePdmFile";
            this.ctlSourcePdmFile.Size = new System.Drawing.Size(673, 20);
            this.ctlSourcePdmFile.TabIndex = 12;
            this.ctlSourcePdmFile.SelectedIndexChanged += new System.EventHandler(this.ctlSourcePdmFile_SelectedIndexChanged_1);
            this.ctlSourcePdmFile.TextChanged += new System.EventHandler(this.ctlSourcePdmFile_TextChanged);
            this.ctlSourcePdmFile.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ctlSourcePdmFile_KeyDown);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(745, 10);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 11;
            this.btnBrowse.Text = "浏览";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnPDMBrowse_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 10;
            this.label1.Text = "源PDM文件";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(-1, 38);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(825, 395);
            this.tabControl1.TabIndex = 13;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.ctlDbType);
            this.tabPage1.Controls.Add(this.ctlCSharpEntityWcfEnabled);
            this.tabPage1.Controls.Add(this.btnCSharpEntityGenerate);
            this.tabPage1.Controls.Add(this.ctlCSharpEntityNameSpace);
            this.tabPage1.Controls.Add(this.ctlCSharpEntityOutputFile);
            this.tabPage1.Controls.Add(this.lblCSharpEntityNameSpace);
            this.tabPage1.Controls.Add(this.btnCSharpEntityBrowse);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(817, 369);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "C# Entity";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(247, 233);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(101, 23);
            this.button1.TabIndex = 21;
            this.button1.Text = "保存配置";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.SaveConfig_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 134);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 20;
            this.label3.Text = "数据库类型";
            // 
            // ctlDbType
            // 
            this.ctlDbType.FormattingEnabled = true;
            this.ctlDbType.ItemHeight = 12;
            this.ctlDbType.Items.AddRange(new object[] {
            "Oracle",
            "MSSQL",
            "MySQL"});
            this.ctlDbType.Location = new System.Drawing.Point(140, 131);
            this.ctlDbType.Name = "ctlDbType";
            this.ctlDbType.Size = new System.Drawing.Size(184, 20);
            this.ctlDbType.TabIndex = 19;
            // 
            // ctlCSharpEntityWcfEnabled
            // 
            this.ctlCSharpEntityWcfEnabled.AutoSize = true;
            this.ctlCSharpEntityWcfEnabled.Checked = true;
            this.ctlCSharpEntityWcfEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ctlCSharpEntityWcfEnabled.Location = new System.Drawing.Point(140, 100);
            this.ctlCSharpEntityWcfEnabled.Name = "ctlCSharpEntityWcfEnabled";
            this.ctlCSharpEntityWcfEnabled.Size = new System.Drawing.Size(66, 16);
            this.ctlCSharpEntityWcfEnabled.TabIndex = 18;
            this.ctlCSharpEntityWcfEnabled.Text = "支持Wcf";
            this.ctlCSharpEntityWcfEnabled.UseVisualStyleBackColor = true;
            // 
            // btnCSharpEntityGenerate
            // 
            this.btnCSharpEntityGenerate.Location = new System.Drawing.Point(140, 233);
            this.btnCSharpEntityGenerate.Name = "btnCSharpEntityGenerate";
            this.btnCSharpEntityGenerate.Size = new System.Drawing.Size(101, 23);
            this.btnCSharpEntityGenerate.TabIndex = 17;
            this.btnCSharpEntityGenerate.Text = "生成";
            this.btnCSharpEntityGenerate.UseVisualStyleBackColor = true;
            this.btnCSharpEntityGenerate.Click += new System.EventHandler(this.btnCSharpEntityGenerate_Click);
            // 
            // ctlCSharpEntityNameSpace
            // 
            this.ctlCSharpEntityNameSpace.FormattingEnabled = true;
            this.ctlCSharpEntityNameSpace.Items.AddRange(new object[] {
            "Lottery.IServices",
            "Football.IService"});
            this.ctlCSharpEntityNameSpace.Location = new System.Drawing.Point(140, 60);
            this.ctlCSharpEntityNameSpace.Name = "ctlCSharpEntityNameSpace";
            this.ctlCSharpEntityNameSpace.Size = new System.Drawing.Size(322, 20);
            this.ctlCSharpEntityNameSpace.TabIndex = 16;
            // 
            // ctlCSharpEntityOutputFile
            // 
            this.ctlCSharpEntityOutputFile.FormattingEnabled = true;
            this.ctlCSharpEntityOutputFile.Location = new System.Drawing.Point(140, 24);
            this.ctlCSharpEntityOutputFile.Name = "ctlCSharpEntityOutputFile";
            this.ctlCSharpEntityOutputFile.Size = new System.Drawing.Size(586, 20);
            this.ctlCSharpEntityOutputFile.TabIndex = 15;
            // 
            // lblCSharpEntityNameSpace
            // 
            this.lblCSharpEntityNameSpace.AutoSize = true;
            this.lblCSharpEntityNameSpace.Location = new System.Drawing.Point(22, 63);
            this.lblCSharpEntityNameSpace.Name = "lblCSharpEntityNameSpace";
            this.lblCSharpEntityNameSpace.Size = new System.Drawing.Size(53, 12);
            this.lblCSharpEntityNameSpace.TabIndex = 14;
            this.lblCSharpEntityNameSpace.Text = "命名空间";
            // 
            // btnCSharpEntityBrowse
            // 
            this.btnCSharpEntityBrowse.Location = new System.Drawing.Point(732, 22);
            this.btnCSharpEntityBrowse.Name = "btnCSharpEntityBrowse";
            this.btnCSharpEntityBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnCSharpEntityBrowse.TabIndex = 13;
            this.btnCSharpEntityBrowse.Text = "浏览";
            this.btnCSharpEntityBrowse.UseVisualStyleBackColor = true;
            this.btnCSharpEntityBrowse.Click += new System.EventHandler(this.btnTargetDirectoryBrowse_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 12);
            this.label2.TabIndex = 12;
            this.label2.Text = "c#代码文件创建到";
            // 
            // myProgressBar1
            // 
            this.myProgressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.myProgressBar1.Location = new System.Drawing.Point(-1, 439);
            this.myProgressBar1.Name = "myProgressBar1";
            this.myProgressBar1.Size = new System.Drawing.Size(825, 21);
            this.myProgressBar1.TabIndex = 14;
            this.myProgressBar1.Visible = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(822, 459);
            this.Controls.Add(this.myProgressBar1);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.ctlSourcePdmFile);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "VL代码生成器";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox ctlSourcePdmFile;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.ComboBox ctlCSharpEntityNameSpace;
        private System.Windows.Forms.ComboBox ctlCSharpEntityOutputFile;
        private System.Windows.Forms.Label lblCSharpEntityNameSpace;
        private System.Windows.Forms.Button btnCSharpEntityBrowse;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnCSharpEntityGenerate;
        private System.Windows.Forms.CheckBox ctlCSharpEntityWcfEnabled;
        private MyProgressBar myProgressBar1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox ctlDbType;
        private System.Windows.Forms.Button button1;
    }
}