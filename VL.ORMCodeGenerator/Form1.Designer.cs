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
            this.cb_source = new System.Windows.Forms.ComboBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cb_dbType = new System.Windows.Forms.ComboBox();
            this.btnCSharpEntityBrowse = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.cb_wcf = new System.Windows.Forms.CheckBox();
            this.btnCSharpEntityGenerate = new System.Windows.Forms.Button();
            this.lblCSharpEntityNameSpace = new System.Windows.Forms.Label();
            this.tb_namespace = new System.Windows.Forms.TextBox();
            this.tb_target = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // cb_source
            // 
            this.cb_source.FormattingEnabled = true;
            this.cb_source.Location = new System.Drawing.Point(157, 34);
            this.cb_source.Name = "cb_source";
            this.cb_source.Size = new System.Drawing.Size(673, 20);
            this.cb_source.TabIndex = 12;
            this.cb_source.TextChanged += new System.EventHandler(this.ctlSourcePdmFile_TextChanged);
            this.cb_source.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ctlSourcePdmFile_KeyDown);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(836, 32);
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
            this.label1.Location = new System.Drawing.Point(82, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 10;
            this.label1.Text = "源PDM文件";
            // 
            // cb_dbType
            // 
            this.cb_dbType.FormattingEnabled = true;
            this.cb_dbType.Location = new System.Drawing.Point(399, 107);
            this.cb_dbType.Name = "cb_dbType";
            this.cb_dbType.Size = new System.Drawing.Size(156, 20);
            this.cb_dbType.TabIndex = 18;
            // 
            // btnCSharpEntityBrowse
            // 
            this.btnCSharpEntityBrowse.Location = new System.Drawing.Point(836, 66);
            this.btnCSharpEntityBrowse.Name = "btnCSharpEntityBrowse";
            this.btnCSharpEntityBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnCSharpEntityBrowse.TabIndex = 17;
            this.btnCSharpEntityBrowse.Text = "浏览";
            this.btnCSharpEntityBrowse.UseVisualStyleBackColor = true;
            this.btnCSharpEntityBrowse.Click += new System.EventHandler(this.btnCSharpEntityBrowse_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(82, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 16;
            this.label2.Text = "生成地址";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(280, 174);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(101, 23);
            this.button1.TabIndex = 28;
            this.button1.Text = "保存配置";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.SaveConfig_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(328, 110);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 27;
            this.label3.Text = "数据库类型";
            // 
            // cb_wcf
            // 
            this.cb_wcf.AutoSize = true;
            this.cb_wcf.Checked = true;
            this.cb_wcf.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_wcf.Location = new System.Drawing.Point(157, 142);
            this.cb_wcf.Name = "cb_wcf";
            this.cb_wcf.Size = new System.Drawing.Size(66, 16);
            this.cb_wcf.TabIndex = 25;
            this.cb_wcf.Text = "支持Wcf";
            this.cb_wcf.UseVisualStyleBackColor = true;
            // 
            // btnCSharpEntityGenerate
            // 
            this.btnCSharpEntityGenerate.Location = new System.Drawing.Point(154, 174);
            this.btnCSharpEntityGenerate.Name = "btnCSharpEntityGenerate";
            this.btnCSharpEntityGenerate.Size = new System.Drawing.Size(101, 23);
            this.btnCSharpEntityGenerate.TabIndex = 24;
            this.btnCSharpEntityGenerate.Text = "生成";
            this.btnCSharpEntityGenerate.UseVisualStyleBackColor = true;
            this.btnCSharpEntityGenerate.Click += new System.EventHandler(this.btnCSharpEntityGenerate_Click);
            // 
            // lblCSharpEntityNameSpace
            // 
            this.lblCSharpEntityNameSpace.AutoSize = true;
            this.lblCSharpEntityNameSpace.Location = new System.Drawing.Point(82, 110);
            this.lblCSharpEntityNameSpace.Name = "lblCSharpEntityNameSpace";
            this.lblCSharpEntityNameSpace.Size = new System.Drawing.Size(53, 12);
            this.lblCSharpEntityNameSpace.TabIndex = 22;
            this.lblCSharpEntityNameSpace.Text = "项目代号";
            // 
            // tb_namespace
            // 
            this.tb_namespace.Location = new System.Drawing.Point(157, 106);
            this.tb_namespace.Name = "tb_namespace";
            this.tb_namespace.Size = new System.Drawing.Size(149, 21);
            this.tb_namespace.TabIndex = 29;
            // 
            // tb_target
            // 
            this.tb_target.Location = new System.Drawing.Point(157, 68);
            this.tb_target.Name = "tb_target";
            this.tb_target.Size = new System.Drawing.Size(673, 21);
            this.tb_target.TabIndex = 30;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 342);
            this.Controls.Add(this.tb_target);
            this.Controls.Add(this.tb_namespace);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.cb_dbType);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnCSharpEntityBrowse);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cb_wcf);
            this.Controls.Add(this.btnCSharpEntityGenerate);
            this.Controls.Add(this.cb_source);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.lblCSharpEntityNameSpace);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "VL代码生成器";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cb_source;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cb_dbType;
        private System.Windows.Forms.Button btnCSharpEntityBrowse;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox cb_wcf;
        private System.Windows.Forms.Button btnCSharpEntityGenerate;
        private System.Windows.Forms.Label lblCSharpEntityNameSpace;
        private System.Windows.Forms.TextBox tb_namespace;
        private System.Windows.Forms.TextBox tb_target;
    }
}