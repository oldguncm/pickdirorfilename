using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PickFilename
{
  public partial class Form1 : Form
  {
    public Form1()
    {
      InitializeComponent();
    }

    private void button1_Click(object sender, EventArgs e)
    {
      if (openFileDialog1.ShowDialog() == DialogResult.OK)
      {
        string s;
        for (int i = 0; i < openFileDialog1.FileNames.Length; i++)
        {
          s = openFileDialog1.FileNames[i];
          if (!checkBox1.Checked)
          {
           
            int j = s.LastIndexOf("\\");
            s = s.Substring(j + 1);
          }
          if (!checkBox2.Checked)
          {
            int k = s.LastIndexOf(".");
            if (k > 0) s = s.Substring(0, k);
          }
          if (checkBox3.Checked)
          {
            s =  s;
          }
          richTextBox1.AppendText( s+"\x0A");
        }
      }
    }

    private void button2_Click(object sender, EventArgs e)
    {
        saveFileDialog1.DefaultExt = "txt";
        saveFileDialog1.Filter = "Text files (*.txt)|*.txt";
      if (saveFileDialog1.ShowDialog() == DialogResult.OK)
      {
          string sfn = saveFileDialog1.FileName;
          if (sfn.Substring(sfn.Length - 4) != ".txt") sfn = sfn + ".txt";
        richTextBox1.SaveFile(sfn,RichTextBoxStreamType.PlainText);
        
      }
    }

    private void button3_Click(object sender, EventArgs e)
    {
      MessageBox.Show("制作：李志军，E-mail：oldgun@sina.com");
      
    }
  }
}