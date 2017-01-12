using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace PickFilename
{
    public partial class CompareFileFrm : Form
    {
        public CompareFileFrm()
        {
            InitializeComponent();
           
        }
        private  List<string> NotPairedlist = new List<string>();
        private int filescount = 0;
        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            string folderpath = Application.StartupPath + "\\folder.txt";
            if (File.Exists(folderpath))
            {
                fbd.SelectedPath = File.ReadAllText(folderpath);
            }
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(folderpath, fbd.SelectedPath);
                NotPairedlist.Clear();
                filescount = 0;
                RichTextBox rich = ((Form1)this.Owner).rtb;
                if (textBox1.Text[0] == '.') textBox1.Text = "*" + textBox1.Text;
                if (textBox2.Text[0] == '.') textBox2.Text = "*" + textBox2.Text;
                compare(fbd.SelectedPath);
                rich.Lines = NotPairedlist.ToArray();
                rich.AppendText(Environment.NewLine+Environment.NewLine+"共计：" + NotPairedlist.Count.ToString() + "个不匹配");
                rich.AppendText(Environment.NewLine + "共计：" + filescount.ToString() + "个\""+textBox1.Text +"\"类型文件");
            }
            
        }
        private void compare(string path)
        {
            DirectoryInfo DIR = new DirectoryInfo(path);
            if (checkBox1.Checked)
            {
                DirectoryInfo[] dirs = DIR.GetDirectories();
                foreach (DirectoryInfo dir in dirs) compare(dir.FullName);
            }
            System.IO.FileInfo[] files = DIR.GetFiles(textBox1.Text);           
            foreach (System.IO.FileInfo file in files)
            {
                filescount++;
                if (!System.IO.File.Exists(System.IO.Path.ChangeExtension(file.FullName,Path.GetExtension(textBox2.Text))))
                {
                    NotPairedlist.Add(file.FullName);
                }
            }
        }
    }
}
