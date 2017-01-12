using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PickFilename
{
    public partial class ChangeImageSize : Form
    {
        public ChangeImageSize()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 3;
            width = 320;
            height = 180;
            checkBox1.Checked = true;
        }
        int width, height;
        string extention = ".jpg";
        System.Drawing.Imaging.ImageFormat imageformat = System.Drawing.Imaging.ImageFormat.Jpeg;
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "All Image Files|*.bmp;*.ico;*.gif;*.jpeg;*.jpg;*.png;*.tif;*.tiff|" +
                                "Windows Bitmap(*.bmp)|*.bmp|" +
                                "Windows Icon(*.ico)|*.ico|" +
                                "Graphics Interchange Format (*.gif)|(*.gif)|" +
                                "JPEG File Interchange Format (*.jpg)|*.jpg;*.jpeg|" +
                                "Portable Network Graphics (*.png)|*.png|" +
                                "Tag Image File Format (*.tif)|*.tif;*.tiff";
            ofd.Multiselect = true;
            ofd.FilterIndex = 0;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Bitmap bmp = null;
                string destfileanme, movefilename;
                pbar.Maximum = ofd.FileNames.Length;
                pbar.Minimum = 0;
                pbar.Value = 0;
                lblpbar.Text = "共计选择了" + ofd.FileNames.Length + "个图片";
                List<string> mfslst = new List<string>();
                mfslst.Clear();
                foreach (string filename in ofd.FileNames)
                {
                    pbar.Value++;
                    lblpbar.Text = pbar.Value.ToString() + "/" + "共计选择了" + ofd.FileNames.Length + "个图片";
                    Application.DoEvents();
                    destfileanme = System.IO.Path.ChangeExtension(filename, extention);

                    movefilename = System.IO.Path.GetDirectoryName(filename) + "\\" + System.IO.Path.GetFileNameWithoutExtension(filename) + "_bak" + System.IO.Path.GetExtension(filename);
                    if (destfileanme == filename && checkBox1.Checked)
                    {
                        if (System.IO.File.Exists(movefilename)) System.IO.File.Delete(movefilename);

                    }
                    else movefilename = filename;
                    System.IO.File.Move(filename, movefilename);
                    System.Drawing.Image image = System.Drawing.Image.FromFile(movefilename);
                    bmp = new Bitmap(width, height);
                    Graphics gp = Graphics.FromImage(bmp);
                    gp.DrawImage(image, 0, 0, width, height);
                    if (System.IO.File.Exists(destfileanme)) System.IO.File.Delete(destfileanme);
                    bmp.Save(destfileanme, imageformat);
                    mfslst.Add(movefilename);
                }
                foreach (string f in mfslst)
                {
                    try
                    {
                        if (System.IO.File.Exists(f)) System.IO.File.Delete(f);
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
        }

        private void txtWidth_Leave(object sender, EventArgs e)
        {
            if (!int.TryParse(txtWidth.Text, out width))
            {
                txtWidth.SelectAll();
                txtWidth.Focus();
            }
        }

        private void txtHeight_Leave(object sender, EventArgs e)
        {
            if (!int.TryParse(txtHeight.Text, out height))
            {
                txtHeight.SelectAll();
                txtHeight.Focus();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                imageformat = System.Drawing.Imaging.ImageFormat.Bmp;
                extention = ".bmp";
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                imageformat = System.Drawing.Imaging.ImageFormat.Icon;
                extention = ".ico";
            }
            else if (comboBox1.SelectedIndex == 2)
            {
                imageformat = System.Drawing.Imaging.ImageFormat.Gif;
                extention = ".gif";
            }
            else if (comboBox1.SelectedIndex == 3)
            {
                imageformat = System.Drawing.Imaging.ImageFormat.Jpeg;
                extention = ".jpg";
            }
            else if (comboBox1.SelectedIndex == 4)
            {
                imageformat = System.Drawing.Imaging.ImageFormat.Png;
                extention = ".png";
            }
            else if (comboBox1.SelectedIndex == 5)
            {
                imageformat = System.Drawing.Imaging.ImageFormat.Tiff;
                extention = ".tif";
            }
        }
    }
}
