namespace VL.ToolKit
{
    partial class ECodeParser
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
            this.button1 = new System.Windows.Forms.Button();
            this.tb_enum = new System.Windows.Forms.TextBox();
            this.tb_KeyValueCollection = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(398, 179);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(101, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "=>";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Parse_Click);
            // 
            // tb_enum
            // 
            this.tb_enum.Location = new System.Drawing.Point(12, 12);
            this.tb_enum.Multiline = true;
            this.tb_enum.Name = "tb_enum";
            this.tb_enum.Size = new System.Drawing.Size(380, 411);
            this.tb_enum.TabIndex = 1;
            // 
            // tb_KeyValueCollection
            // 
            this.tb_KeyValueCollection.Location = new System.Drawing.Point(505, 12);
            this.tb_KeyValueCollection.Multiline = true;
            this.tb_KeyValueCollection.Name = "tb_KeyValueCollection";
            this.tb_KeyValueCollection.Size = new System.Drawing.Size(380, 411);
            this.tb_KeyValueCollection.TabIndex = 2;
            // 
            // ECodeParser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(897, 435);
            this.Controls.Add(this.tb_KeyValueCollection);
            this.Controls.Add(this.tb_enum);
            this.Controls.Add(this.button1);
            this.Name = "ECodeParser";
            this.Text = "ECodeParser";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox tb_enum;
        private System.Windows.Forms.TextBox tb_KeyValueCollection;
    }
}

