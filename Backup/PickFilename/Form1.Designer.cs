namespace PickFilename
{
  partial class Form1
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
    /// 设计器支持所需的方法 - 不要
    /// 使用代码编辑器修改此方法的内容。
    /// </summary>
    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
        this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
        this.button1 = new System.Windows.Forms.Button();
        this.button2 = new System.Windows.Forms.Button();
        this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
        this.richTextBox1 = new System.Windows.Forms.RichTextBox();
        this.checkBox1 = new System.Windows.Forms.CheckBox();
        this.checkBox2 = new System.Windows.Forms.CheckBox();
        this.panel1 = new System.Windows.Forms.Panel();
        this.button3 = new System.Windows.Forms.Button();
        this.checkBox3 = new System.Windows.Forms.CheckBox();
        this.panel1.SuspendLayout();
        this.SuspendLayout();
        // 
        // openFileDialog1
        // 
        this.openFileDialog1.FileName = "openFileDialog1";
        this.openFileDialog1.Multiselect = true;
        // 
        // button1
        // 
        this.button1.Location = new System.Drawing.Point(11, 23);
        this.button1.Name = "button1";
        this.button1.Size = new System.Drawing.Size(75, 23);
        this.button1.TabIndex = 0;
        this.button1.Text = "打开";
        this.button1.UseVisualStyleBackColor = true;
        this.button1.Click += new System.EventHandler(this.button1_Click);
        // 
        // button2
        // 
        this.button2.Location = new System.Drawing.Point(11, 74);
        this.button2.Name = "button2";
        this.button2.Size = new System.Drawing.Size(75, 23);
        this.button2.TabIndex = 1;
        this.button2.Text = "保存";
        this.button2.UseVisualStyleBackColor = true;
        this.button2.Click += new System.EventHandler(this.button2_Click);
        // 
        // richTextBox1
        // 
        this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
        this.richTextBox1.Location = new System.Drawing.Point(0, 0);
        this.richTextBox1.Name = "richTextBox1";
        this.richTextBox1.Size = new System.Drawing.Size(354, 375);
        this.richTextBox1.TabIndex = 2;
        this.richTextBox1.Text = "";
        // 
        // checkBox1
        // 
        this.checkBox1.AutoSize = true;
        this.checkBox1.Location = new System.Drawing.Point(11, 123);
        this.checkBox1.Name = "checkBox1";
        this.checkBox1.Size = new System.Drawing.Size(72, 16);
        this.checkBox1.TabIndex = 3;
        this.checkBox1.Text = "保留路径";
        this.checkBox1.UseVisualStyleBackColor = true;
        // 
        // checkBox2
        // 
        this.checkBox2.AutoSize = true;
        this.checkBox2.Location = new System.Drawing.Point(11, 162);
        this.checkBox2.Name = "checkBox2";
        this.checkBox2.Size = new System.Drawing.Size(84, 16);
        this.checkBox2.TabIndex = 4;
        this.checkBox2.Text = "保留扩展名";
        this.checkBox2.UseVisualStyleBackColor = true;
        // 
        // panel1
        // 
        this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
        this.panel1.Controls.Add(this.button3);
        this.panel1.Controls.Add(this.checkBox3);
        this.panel1.Controls.Add(this.checkBox2);
        this.panel1.Controls.Add(this.button1);
        this.panel1.Controls.Add(this.checkBox1);
        this.panel1.Controls.Add(this.button2);
        this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
        this.panel1.Location = new System.Drawing.Point(354, 0);
        this.panel1.Name = "panel1";
        this.panel1.Size = new System.Drawing.Size(100, 375);
        this.panel1.TabIndex = 5;
        // 
        // button3
        // 
        this.button3.Location = new System.Drawing.Point(11, 301);
        this.button3.Name = "button3";
        this.button3.Size = new System.Drawing.Size(75, 23);
        this.button3.TabIndex = 5;
        this.button3.Text = "关于...";
        this.button3.UseVisualStyleBackColor = true;
        this.button3.Click += new System.EventHandler(this.button3_Click);
        // 
        // checkBox3
        // 
        this.checkBox3.AutoSize = true;
        this.checkBox3.Location = new System.Drawing.Point(11, 198);
        this.checkBox3.Name = "checkBox3";
        this.checkBox3.Size = new System.Drawing.Size(84, 16);
        this.checkBox3.TabIndex = 4;
        this.checkBox3.Text = "插入超链接";
        this.checkBox3.UseVisualStyleBackColor = true;
        // 
        // Form1
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(454, 375);
        this.Controls.Add(this.richTextBox1);
        this.Controls.Add(this.panel1);
        this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
        this.Name = "Form1";
        this.Text = "提取文件名";
        this.panel1.ResumeLayout(false);
        this.panel1.PerformLayout();
        this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.OpenFileDialog openFileDialog1;
    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.Button button2;
    private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    private System.Windows.Forms.RichTextBox richTextBox1;
    private System.Windows.Forms.CheckBox checkBox1;
    private System.Windows.Forms.CheckBox checkBox2;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.Button button3;
    private System.Windows.Forms.CheckBox checkBox3;
  }
}

