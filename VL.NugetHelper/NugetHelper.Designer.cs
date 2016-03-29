namespace VL.NugetHelper
{
    partial class NugetHelper
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.Generate = new System.Windows.Forms.Button();
            this.cb_projects = new System.Windows.Forms.ComboBox();
            this.Delete = new System.Windows.Forms.Button();
            this.tb_projectRootPath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tb_Author = new System.Windows.Forms.TextBox();
            this.SaveProjects = new System.Windows.Forms.Button();
            this.Push = new System.Windows.Forms.Button();
            this.tb_output = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tb_Title = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tb_Description = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tb_Configuration = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tb_Company = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tb_Product = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tb_FileVersion = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.tb_Version = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.tb_Culture = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.tb_Trademark = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.tb_Copyright = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.tb_APIKey = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.tb_NugetServer = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.tb_Notes = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // Generate
            // 
            this.Generate.Location = new System.Drawing.Point(340, 42);
            this.Generate.Name = "Generate";
            this.Generate.Size = new System.Drawing.Size(75, 23);
            this.Generate.TabIndex = 0;
            this.Generate.Text = "Generate";
            this.Generate.UseVisualStyleBackColor = true;
            this.Generate.Click += new System.EventHandler(this.Generate_Click);
            // 
            // cb_projects
            // 
            this.cb_projects.FormattingEnabled = true;
            this.cb_projects.Location = new System.Drawing.Point(109, 83);
            this.cb_projects.Name = "cb_projects";
            this.cb_projects.Size = new System.Drawing.Size(144, 20);
            this.cb_projects.TabIndex = 1;
            this.cb_projects.SelectedIndexChanged += new System.EventHandler(this.cb_projects_SelectedIndexChanged);
            this.cb_projects.TextChanged += new System.EventHandler(this.cb_projects_TextChanged);
            // 
            // Delete
            // 
            this.Delete.Location = new System.Drawing.Point(102, 42);
            this.Delete.Name = "Delete";
            this.Delete.Size = new System.Drawing.Size(75, 23);
            this.Delete.TabIndex = 2;
            this.Delete.Text = "Delete";
            this.Delete.UseVisualStyleBackColor = true;
            this.Delete.Click += new System.EventHandler(this.Delete_Click);
            // 
            // tb_projectRootPath
            // 
            this.tb_projectRootPath.Location = new System.Drawing.Point(109, 109);
            this.tb_projectRootPath.Name = "tb_projectRootPath";
            this.tb_projectRootPath.Size = new System.Drawing.Size(387, 21);
            this.tb_projectRootPath.TabIndex = 5;
            this.tb_projectRootPath.TextChanged += new System.EventHandler(this.tb_projectRootPath_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 112);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "RootPath";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 88);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "Name";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(269, 86);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 9;
            this.label3.Text = "Author";
            // 
            // tb_Author
            // 
            this.tb_Author.Location = new System.Drawing.Point(352, 81);
            this.tb_Author.Name = "tb_Author";
            this.tb_Author.Size = new System.Drawing.Size(144, 21);
            this.tb_Author.TabIndex = 8;
            // 
            // SaveProjects
            // 
            this.SaveProjects.Location = new System.Drawing.Point(21, 42);
            this.SaveProjects.Name = "SaveProjects";
            this.SaveProjects.Size = new System.Drawing.Size(75, 23);
            this.SaveProjects.TabIndex = 10;
            this.SaveProjects.Text = "Save";
            this.SaveProjects.UseVisualStyleBackColor = true;
            this.SaveProjects.Click += new System.EventHandler(this.SaveProjectsConfig_Click);
            // 
            // Push
            // 
            this.Push.Location = new System.Drawing.Point(421, 42);
            this.Push.Name = "Push";
            this.Push.Size = new System.Drawing.Size(75, 23);
            this.Push.TabIndex = 12;
            this.Push.Text = "Push";
            this.Push.UseVisualStyleBackColor = true;
            this.Push.Click += new System.EventHandler(this.Push_Click);
            // 
            // tb_output
            // 
            this.tb_output.Location = new System.Drawing.Point(12, 409);
            this.tb_output.Multiline = true;
            this.tb_output.Name = "tb_output";
            this.tb_output.Size = new System.Drawing.Size(521, 147);
            this.tb_output.TabIndex = 13;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 141);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 12);
            this.label4.TabIndex = 15;
            this.label4.Text = "Title";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // tb_Title
            // 
            this.tb_Title.Location = new System.Drawing.Point(109, 136);
            this.tb_Title.Name = "tb_Title";
            this.tb_Title.Size = new System.Drawing.Size(144, 21);
            this.tb_Title.TabIndex = 14;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(20, 168);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 12);
            this.label5.TabIndex = 17;
            this.label5.Text = "Description";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // tb_Description
            // 
            this.tb_Description.Location = new System.Drawing.Point(109, 163);
            this.tb_Description.Name = "tb_Description";
            this.tb_Description.Size = new System.Drawing.Size(144, 21);
            this.tb_Description.TabIndex = 16;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(20, 195);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(83, 12);
            this.label6.TabIndex = 19;
            this.label6.Text = "Configuration";
            this.label6.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // tb_Configuration
            // 
            this.tb_Configuration.Location = new System.Drawing.Point(109, 190);
            this.tb_Configuration.Name = "tb_Configuration";
            this.tb_Configuration.Size = new System.Drawing.Size(144, 21);
            this.tb_Configuration.TabIndex = 18;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(20, 222);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(47, 12);
            this.label7.TabIndex = 21;
            this.label7.Text = "Company";
            this.label7.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // tb_Company
            // 
            this.tb_Company.Location = new System.Drawing.Point(109, 217);
            this.tb_Company.Name = "tb_Company";
            this.tb_Company.Size = new System.Drawing.Size(144, 21);
            this.tb_Company.TabIndex = 20;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(20, 249);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(47, 12);
            this.label8.TabIndex = 23;
            this.label8.Text = "Product";
            this.label8.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // tb_Product
            // 
            this.tb_Product.Location = new System.Drawing.Point(109, 244);
            this.tb_Product.Name = "tb_Product";
            this.tb_Product.Size = new System.Drawing.Size(144, 21);
            this.tb_Product.TabIndex = 22;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(263, 249);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(71, 12);
            this.label9.TabIndex = 33;
            this.label9.Text = "FileVersion";
            this.label9.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // tb_FileVersion
            // 
            this.tb_FileVersion.Location = new System.Drawing.Point(352, 244);
            this.tb_FileVersion.Name = "tb_FileVersion";
            this.tb_FileVersion.Size = new System.Drawing.Size(144, 21);
            this.tb_FileVersion.TabIndex = 32;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(263, 222);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(47, 12);
            this.label10.TabIndex = 31;
            this.label10.Text = "Version";
            this.label10.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // tb_Version
            // 
            this.tb_Version.Location = new System.Drawing.Point(352, 217);
            this.tb_Version.Name = "tb_Version";
            this.tb_Version.Size = new System.Drawing.Size(144, 21);
            this.tb_Version.TabIndex = 30;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(263, 195);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(47, 12);
            this.label11.TabIndex = 29;
            this.label11.Text = "Culture";
            this.label11.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // tb_Culture
            // 
            this.tb_Culture.Location = new System.Drawing.Point(352, 190);
            this.tb_Culture.Name = "tb_Culture";
            this.tb_Culture.Size = new System.Drawing.Size(144, 21);
            this.tb_Culture.TabIndex = 28;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(263, 168);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(59, 12);
            this.label12.TabIndex = 27;
            this.label12.Text = "Trademark";
            this.label12.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // tb_Trademark
            // 
            this.tb_Trademark.Location = new System.Drawing.Point(352, 163);
            this.tb_Trademark.Name = "tb_Trademark";
            this.tb_Trademark.Size = new System.Drawing.Size(144, 21);
            this.tb_Trademark.TabIndex = 26;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(263, 141);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(59, 12);
            this.label13.TabIndex = 25;
            this.label13.Text = "Copyright";
            this.label13.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // tb_Copyright
            // 
            this.tb_Copyright.Location = new System.Drawing.Point(352, 136);
            this.tb_Copyright.Name = "tb_Copyright";
            this.tb_Copyright.Size = new System.Drawing.Size(144, 21);
            this.tb_Copyright.TabIndex = 24;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(263, 17);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(47, 12);
            this.label14.TabIndex = 37;
            this.label14.Text = "API Key";
            this.label14.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // tb_APIKey
            // 
            this.tb_APIKey.Location = new System.Drawing.Point(352, 12);
            this.tb_APIKey.Name = "tb_APIKey";
            this.tb_APIKey.Size = new System.Drawing.Size(144, 21);
            this.tb_APIKey.TabIndex = 36;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(20, 17);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(71, 12);
            this.label15.TabIndex = 35;
            this.label15.Text = "NugetServer";
            this.label15.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // tb_NugetServer
            // 
            this.tb_NugetServer.Location = new System.Drawing.Point(109, 12);
            this.tb_NugetServer.Name = "tb_NugetServer";
            this.tb_NugetServer.Size = new System.Drawing.Size(144, 21);
            this.tb_NugetServer.TabIndex = 34;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(20, 274);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(35, 12);
            this.label16.TabIndex = 39;
            this.label16.Text = "Notes";
            // 
            // tb_Notes
            // 
            this.tb_Notes.Location = new System.Drawing.Point(109, 274);
            this.tb_Notes.Multiline = true;
            this.tb_Notes.Name = "tb_Notes";
            this.tb_Notes.Size = new System.Drawing.Size(387, 129);
            this.tb_Notes.TabIndex = 38;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(556, 568);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.tb_Notes);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.tb_APIKey);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.tb_NugetServer);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.tb_FileVersion);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.tb_Version);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.tb_Culture);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.tb_Trademark);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.tb_Copyright);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.tb_Product);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.tb_Company);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tb_Configuration);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tb_Description);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tb_Title);
            this.Controls.Add(this.tb_output);
            this.Controls.Add(this.Push);
            this.Controls.Add(this.SaveProjects);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tb_Author);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tb_projectRootPath);
            this.Controls.Add(this.Delete);
            this.Controls.Add(this.cb_projects);
            this.Controls.Add(this.Generate);
            this.Name = "NugetHelper";
            this.Text = "NugetHelper";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Generate;
        private System.Windows.Forms.ComboBox cb_projects;
        private System.Windows.Forms.Button Delete;
        private System.Windows.Forms.TextBox tb_projectRootPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tb_Author;
        private System.Windows.Forms.Button SaveProjects;
        private System.Windows.Forms.Button Push;
        private System.Windows.Forms.TextBox tb_output;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tb_Title;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tb_Description;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tb_Configuration;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tb_Company;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tb_Product;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tb_FileVersion;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox tb_Version;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox tb_Culture;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox tb_Trademark;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox tb_Copyright;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox tb_APIKey;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox tb_NugetServer;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox tb_Notes;
    }
}

