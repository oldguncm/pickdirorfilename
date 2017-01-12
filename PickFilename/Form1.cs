using System;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using flashinfo;
using System.Collections.Generic;
using DaveChambers.FolderBrowserDialogEx;
using System.Drawing;
using System.Text;

namespace PickFilename
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //button5.Visible = true;
            rtb.DetectUrls = true;
            rbEnd.BringToFront();
            rbMiddle.Parent = rbBegin.Parent = rbEnd.Parent = panel4;
            panel1.Parent = splitContainer1.Panel2;
            splitContainer1.SplitterDistance = splitContainer1.Width - 310;
            panel1.BringToFront();
            panel1.Location = new System.Drawing.Point(0, 0);

        }
        int dirNum = 0;
        int fileNum = 0;
        bool includeExtens, includePath, includeFileSize, includeFolder;
        private void button1_Click(object sender, EventArgs e)
        {
            dirNum = 0;
            fileNum = 0;
            if (chkDirAndFile.Checked) includeFolder = true;
            else includeFolder = false;
            if (chkRetainExtention.Checked) includeExtens = true;
            else includeExtens = false;
            if (chkpath.Checked) includePath = true;
            else includePath = false;
            if (chkFileSize.Checked) includeFileSize = true;
            else includeFileSize = false;
            if (includeFolder)
            {
                FolderBrowserDialogEx fbde = new DaveChambers.FolderBrowserDialogEx.FolderBrowserDialogEx();
                fbde.ShowEditbox = true;
                fbde.ShowNewFolderButton = false;
                fbde.StartPosition = FormStartPosition.CenterParent;
                string folderpath = Application.StartupPath + "\\folder.txt";
                if (File.Exists(folderpath))
                {
                    fbde.SelectedPath = File.ReadAllText(folderpath);
                }

                if (fbde.ShowDialog(this) == DialogResult.OK)
                {
                    File.WriteAllText(folderpath, fbde.SelectedPath);
                    ListDirectory(fbde.SelectedPath);
                }
                rtb.AppendText("共有" + dirNum.ToString() + "个文件夹" + "    " + "共有" + fileNum.ToString() + "个文件");

            }
            else
            {

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    rtb.Focus();
                    string s;
                    foreach (string filename in ofd.FileNames)
                    {
                        if (txtInclude.Text.Trim() != "" && !filename.Contains(txtInclude.Text.Trim())) continue;
                        FileInfo file = new FileInfo(filename);
                        fileNum++;
                        //s = file.FullName;
                        if (!includeExtens)
                        {
                            s = System.IO.Path.GetFileNameWithoutExtension(file.Name);
                        }
                        else s = file.Name;
                        if (includePath)
                        {
                            s = System.IO.Path.GetDirectoryName(file.FullName) + "\\" + s;
                        }

                        if (includeFileSize)
                        {
                            s += "\t" + file.Length.ToString("N0", System.Globalization.CultureInfo.InvariantCulture);
                            //s += "\t" + file.Extension;
                        }
                        if (chkExtention.Checked)
                        {
                            s = s + "\t" + file.Extension;
                        }
                        if (file.Extension.ToLower() == ".wmv" || file.Extension.ToLower() == ".avi" || file.Extension.ToLower() == ".mp4" || file.Extension.ToLower() == ".mpg")
                        {
                            if (chkDuration.Checked)
                            {
                                string ss = GetMediaDuration(file.FullName);
                                int ttt = timeToint(ss);
                                s += "\t" + doubletotime(ttt) + "\t" + ttt.ToString();
                            }
                            if (chkFrameWidthAndHeight.Checked)
                            {
                                System.Drawing.Size size = GetMediaFrameWidthAndHeight(file.FullName);
                                s += "\t" + size.Width.ToString() + "\t" + size.Height.ToString();
                            }
                        }
                        else if (file.Extension.ToLower() == ".swf")
                        {
                            FlashInfo flash = new FlashInfo(file.FullName);
                            if (chkDuration.Checked)
                            {
                                s += "\t" + doubletotime(flash.Duration) + "\t" + flash.Duration.ToString();
                            }
                            if (chkFrameWidthAndHeight.Checked)
                            {
                                s += "\t" + flash.Width.ToString() + "\t" + flash.Height.ToString();
                            }
                        }
                        rtb.AppendText(s + Environment.NewLine);
                        //fileN++;
                    }
                }
            }
        }
        public void ListDirectory(string strFullPathName)
        {
            Application.DoEvents();
            DirectoryInfo dir = new DirectoryInfo(strFullPathName);
            DirectoryInfo[] dirSubs = dir.GetDirectories();

            //遍历子目录
            foreach (DirectoryInfo dirSub in dirSubs)
            {
                try
                {
                    // 输出目录名
                    // richTextBox1.AppendText(dirSub.FullName + "\\\x0A");
                    dirNum++;
                    // 递归调用ListDirectory
                    ListDirectory(dirSub.FullName);
                }
                catch
                {

                }
            }

            //获取目录中的文件
            string s = String.Empty;
            FileInfo[] files = null;
            string Include = txtInclude.Text.Trim();
            //int fileN = 0;
            if (Include != "") files = dir.GetFiles("*" + Include + "*");
            else files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                if (extensionTxt.Text.Trim() != "" && file.Extension != extensionTxt.Text.Trim()) continue;
                // 输出文件名
                fileNum++;
                //s = file.FullName;
                if (!includeExtens)
                {
                    s = System.IO.Path.GetFileNameWithoutExtension(file.Name);
                }
                else s = file.Name;
                if (includePath)
                {
                    s = System.IO.Path.GetDirectoryName(file.FullName) + "\\" + s;
                }

                if (includeFileSize)
                {
                    s += "\t" + file.Length.ToString("N0", System.Globalization.CultureInfo.InvariantCulture);
                    //s += "\t" + file.Extension;
                }
                if (chkExtention.Checked)
                {
                    s = s + "\t" + file.Extension;
                }
                if (chkDuration.Checked)
                {
                    if (file.Extension.ToLower() == ".wmv" || file.Extension.ToLower() == ".avi")
                    {

                        string ss = GetMediaDuration(file.FullName);
                        int ttt = timeToint(ss);
                        s += "\t" + doubletotime(ttt) + "\t" + ttt.ToString();
                    }
                    else if (file.Extension.ToLower() == ".swf")
                    {
                        FlashInfo flash = new FlashInfo(file.FullName);
                        s += "\t" + doubletotime(flash.Duration) + "\t" + flash.Duration.ToString();
                    }
                }
                else if (chkFrameWidthAndHeight.Checked)
                {
                    if (file.Extension.ToLower() == ".wmv" || file.Extension.ToLower() == ".avi")
                    {
                        System.Drawing.Size size = GetMediaFrameWidthAndHeight(file.FullName);
                        s += "\t" + size.Width.ToString() + "\t" + size.Height.ToString();
                    }
                    else if (file.Extension.ToLower() == ".swf")
                    {
                        FlashInfo flash = new FlashInfo(file.FullName);
                        s += "\t" + flash.Width.ToString() + "\t" + flash.Height.ToString();
                    }
                }
                rtb.AppendText("\t" + s + Environment.NewLine);
            }


            //fileN++;

            //richTextBox1.AppendText("共有" + fileN.ToString() + "个文件\x0A");
        }
        private string doubletotime(double seconds)
        {
            string time = string.Empty;
            int h = ((int)seconds) / 3600;
            if (h != 0) time = h.ToString().PadLeft(2, '0') + "小时";
            int m = (((int)seconds) % 3600) / 60;
            if (h != 0) time += m.ToString().PadLeft(2, '0') + "分";
            else
            {
                if (m != 0) time += m.ToString().PadLeft(2, '0') + "分";
            }
            int s = ((int)seconds) % 60;
            return time + s.ToString().PadLeft(2, '0') + "秒";
        }
        private void button2_Click(object sender, EventArgs e)
        {
            sfd.DefaultExt = "txt";
            sfd.Filter = "Text files (*.txt)|*.txt";
            sfd.AddExtension = true;
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                string sfn = sfd.FileName;
                rtb.SaveFile(sfn, RichTextBoxStreamType.PlainText);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("制作：李志军，E-mail：oldgun@sina.com");
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Link;
            else e.Effect = DragDropEffects.None;

        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {

            Array files = (Array)e.Data.GetData(DataFormats.FileDrop);
            int fn = files.GetLength(0);
            for (int i = 0; i < fn; i++)
            {
                string s = files.GetValue(i).ToString();
                if (!chkpath.Checked)
                {
                    s = System.IO.Path.GetFileName(s);
                }
                if (!chkRetainExtention.Checked)
                {
                    s = System.IO.Path.GetFileNameWithoutExtension(s);
                }
                if (chkExtention.Checked)
                {
                    s += "\t" + System.IO.Path.GetExtension(files.GetValue(i).ToString());
                }
                rtb.AppendText(s + Environment.NewLine);

            }


        }
        private string space(int l)
        {
            string s = "";
            for (int i = 0; i < l; i++)
                s += " ";
            return s;
        }
        private void delfolder(string sfd)
        {
            DirectoryInfo dir = new DirectoryInfo(sfd);
            DirectoryInfo[] dirSubs = dir.GetDirectories();

            //遍历子目录
            foreach (DirectoryInfo dirSub in dirSubs)
            {

                // 输出目录名

                // 递归调用ListDirectory
                delfolder(dirSub.FullName);

            }

            //获取目录中的文件
            string s, s1;
            //int fileN = 0;
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {

                // 输出文件名

                s = file.Name;
                s1 = file.FullName;
                if (file.Extension != ".swf")
                {
                    File.Delete(s1);
                }
                //if (s.ToLower() != "main.swf" && s.ToLower()!="cover.swf")
                //{ 


                //    //string dsf = dir.FullName + "\\1.swf";
                //    //if (!File.Exists(dsf))
                //    //{
                //    //    File.Move(s1, dsf);
                //    //    File.Delete(s1);
                //    //}
                //}

                //fileN++;
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            List<string> drivelist = getHardDrives();
            string diskname, dir;
            foreach (string str in rtb.Lines)
            {
                dir = str.Trim();
                if (!string.IsNullOrEmpty(dir))
                {
                    diskname = dir.Substring(0, 3);
                    if (drivelist.Contains(diskname))
                    {
                        if (!Directory.Exists(dir))
                            Directory.CreateDirectory(dir);
                    }
                }
            }
        }
        private List<string> getHardDrives()
        {
            List<string> drivelist = new List<string>();
            DriveInfo[] driveinfos = DriveInfo.GetDrives();
            drivelist.Clear();
            foreach (DriveInfo dr in driveinfos)
            {
                if (dr.DriveType == DriveType.Fixed) drivelist.Add(dr.Name);
            }
            return drivelist;
        }
        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();

                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    if (MessageBox.Show("确定要删除该文件夹下的指定的文件", "警告", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        delfolder(fbd.SelectedPath);
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.ToString());
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {

            try
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();

                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    string sfd = fbd.SelectedPath;
                    DirectoryInfo diri = new DirectoryInfo(sfd);
                    DirectoryInfo[] dirr = diri.GetDirectories();
                    foreach (DirectoryInfo di in dirr)
                    {
                        rtb.AppendText(di.Name + "\r");
                    }
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.ToString());
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    if (fbd.ShowDialog() == DialogResult.OK)
                    {

                        foreach (string file in ofd.FileNames)
                        {
                            FileInfo sf = new FileInfo(file);
                            copyfile(fbd.SelectedPath, sf.Name, sf.FullName);
                        }
                    }
                }

            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.ToString());
            }
        }
        private void copyfile(string dirfullname, string filename, string filefullname)
        {

            DirectoryInfo dir = new DirectoryInfo(dirfullname);
            DirectoryInfo[] dirSubs = dir.GetDirectories();
            string dirfn = "";

            //遍历子目录
            foreach (DirectoryInfo dirSub in dirSubs)
            {

                // 输出目录名
                dirfn = dirSub.FullName;
                // 递归调用ListDirectory
                copyfile(dirSub.FullName, filename, filefullname);

            }

            string sfile = dirfn + filename;
            if (!File.Exists(sfile))
                File.Copy(filefullname, sfile);


        }

        private void CutMenuItem_Click(object sender, EventArgs e)
        {
            SendKeys.Send("^X");
        }

        private void CopyMenu_Click(object sender, EventArgs e)
        {
            SendKeys.Send("^C");
        }

        private void PasteMeun_Click(object sender, EventArgs e)
        {
            SendKeys.Send("^V");
        }

        private void DelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SendKeys.Send("{DELETE}");
        }

        private void ClearMenu_Click(object sender, EventArgs e)
        {
            rtb.SelectAll();
            SendKeys.Send("{DELETE}");
        }




        private void button5_Click_1(object sender, EventArgs e)
        {
            if (TxtBeginEnd.Text == "") return;
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            string folderpath = Application.StartupPath + "\\folder.txt";
            if (File.Exists(folderpath))
            {
                fbd.SelectedPath = File.ReadAllText(folderpath);
            }
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(folderpath, fbd.SelectedPath);
                editBiginEnd(fbd.SelectedPath, txtRemoveExt.Text.Trim());
            }
        }
        private void editBiginEnd(string strpath, string extension = "")
        {
            DirectoryInfo dirinfo = new DirectoryInfo(strpath);
            DirectoryInfo[] dirs = dirinfo.GetDirectories();
            string strnewfilename = "";
            foreach (DirectoryInfo dir in dirs)
            {
                editBiginEnd(dir.FullName);
            }
            FileInfo[] files;
            if (string.IsNullOrEmpty(extension)) files = dirinfo.GetFiles();
            else files = dirinfo.GetFiles("*" + extension.Trim('*'));
            foreach (FileInfo file in files)
            {
                strnewfilename = "";
                if (rbBegin.Checked)
                {
                    if (file.Name.StartsWith(TxtBeginEnd.Text.Trim()))
                    {
                        strnewfilename = file.Name.Substring(TxtBeginEnd.Text.Trim().Length);
                        strnewfilename = file.DirectoryName + "\\" + strnewfilename;
                    }
                    else if (TxtBeginEnd.Text.Trim() == "#")
                    {
                        strnewfilename = file.Name;
                        Regex regex = new System.Text.RegularExpressions.Regex(@"^\d*");
                        if (regex.IsMatch(file.Name))
                        {
                            strnewfilename = Regex.Replace(file.Name, @"^\d*", "");
                        }
                        strnewfilename = file.DirectoryName + "\\" + strnewfilename;
                    }
                    if (strnewfilename != "")
                        if (!File.Exists(strnewfilename))
                            File.Move(file.FullName, strnewfilename);
                        else rtb.AppendText(strnewfilename + "文件已存在" + Environment.NewLine);
                }
                else if (rbEnd.Checked)
                {
                    bool flag = false;
                    strnewfilename = Path.GetFileNameWithoutExtension(file.Name).Trim();
                    if (strnewfilename.EndsWith(TxtBeginEnd.Text.Trim()))
                    {
                        strnewfilename = strnewfilename.Substring(0, strnewfilename.Length - TxtBeginEnd.Text.Trim().Length);
                        strnewfilename = file.DirectoryName + "\\" + strnewfilename + file.Extension;
                        flag = true;
                    }
                    else if (TxtBeginEnd.Text.Trim() == "#")
                    {
                        //strnewfilename = strnewfilename.Substring(0, strnewfilename.Length - TxtBeginEnd.Text.Trim().Length);
                        Regex regex = new System.Text.RegularExpressions.Regex(@"\d*$");
                        if (regex.IsMatch(strnewfilename))
                        {
                            strnewfilename = Regex.Replace(strnewfilename, @"\d*$", "");
                            strnewfilename = file.DirectoryName + "\\" + strnewfilename + file.Extension;
                            flag = true;
                        }

                    }
                    if (flag)
                    {
                        try
                        {
                            if (!File.Exists(strnewfilename))
                                File.Move(file.FullName, strnewfilename);
                            else rtb.AppendText(strnewfilename + "文件已存在" + Environment.NewLine);
                        }
                        catch (Exception ex)
                        {
                            rtb.AppendText(strnewfilename + "发送IO错误" + Environment.NewLine + ex.ToString());
                        }
                    }
                }
                else if (rbMiddle.Checked)
                {
                    strnewfilename = Path.GetFileNameWithoutExtension(file.Name).Trim();
                    strnewfilename = strnewfilename.Replace(TxtBeginEnd.Text.Trim(), "");
                    strnewfilename = file.DirectoryName + "\\" + strnewfilename + file.Extension;
                    if (!File.Exists(strnewfilename))
                        File.Move(file.FullName, strnewfilename);
                    else rtb.AppendText(strnewfilename + "文件已存在" + Environment.NewLine);
                }
            }
        }
        private void addBeginEndFilename(string strpath, string extension = "")
        {
            if (txtAddtext.Text.Trim() == "") return;
            string addtext = txtAddtext.Text.Trim();
            DirectoryInfo dirinfo = new DirectoryInfo(strpath);
            DirectoryInfo[] dirs = dirinfo.GetDirectories();
            string strnewfilename = "";
            foreach (DirectoryInfo dir in dirs)
            {
                addBeginEndFilename(dir.FullName, extension);
            }
            FileInfo[] files;
            if (string.IsNullOrEmpty(extension)) files = dirinfo.GetFiles();
            else files = dirinfo.GetFiles("*" + extension.Trim('*'));
            foreach (FileInfo file in files)
            {
                strnewfilename = "";
                if (rbaddbegin.Checked)
                {
                    strnewfilename = file.DirectoryName + "\\" + addtext + file.Name;
                    if (File.Exists(strnewfilename)) rtb.AppendText("已存在：" + strnewfilename + Environment.NewLine);
                    else File.Move(file.FullName, strnewfilename);
                }
                else
                {
                    strnewfilename = file.DirectoryName + "\\" + Path.GetFileNameWithoutExtension(file.FullName) + addtext + file.Extension;
                    if (File.Exists(strnewfilename)) rtb.AppendText("已存在：" + strnewfilename + Environment.NewLine);
                    else File.Move(file.FullName, strnewfilename);
                }
            }
        }
        public static String filterUnNumber(String str)
        { // 只允数字 
            String regEx = "[^0-9]";
            Regex p = new System.Text.RegularExpressions.Regex(regEx);
            if (p.IsMatch(str))
            {

            }
            //Matcher m = p.matcher(str); //替换与模式匹配的所有字符（即非数字的字符将被""替换） 
            return str;
        }
        private int timeToint(string time)
        {
            string[] strs = time.Split(new char[] { ':' });
            return 3600 * int.Parse(strs[0]) + 60 * int.Parse(strs[1]) + int.Parse(strs[2]);

        }
        ///　<summary>
        ///　获取媒体播放时间长度，格式00:00:00。
        ///　</summary>
        ///　<param name="path">媒体路径</param>
        ///　<returns>播放时间长度</returns>
        public static string GetMediaDuration(string path)
        {
            try
            {
                MediaInfo mi = new MediaInfo(path);               
                return mi.DurationStr;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return null;
            }
        }
        /// <summary>
        /// 获得wmv文件的帧宽度和高度
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static System.Drawing.Size GetMediaFrameWidthAndHeight(string path)
        {           
            try
            {
                MediaInfo mi = new MediaInfo(path);               
                return new System.Drawing.Size(mi.Width.Value, mi.Height.Value);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return new System.Drawing.Size(0, 0);
            }
        }

        private void btnReplacefilename_Click(object sender, EventArgs e)
        {
            if (txtSourcestr.Text == "") return;
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            string folderpath = Application.StartupPath + "\\folder.txt";
            if (File.Exists(folderpath))
            {
                fbd.SelectedPath = File.ReadAllText(folderpath);
            }
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(folderpath, fbd.SelectedPath);
                replaceFilename(fbd.SelectedPath);
            }
        }
        private void replaceFilename(string path)
        {
            DirectoryInfo Dir = new DirectoryInfo(path);
            DirectoryInfo[] dirs = Dir.GetDirectories();
            foreach (DirectoryInfo dir in dirs)
            {
                replaceFilename(dir.FullName);
            }
            foreach (string filename in System.IO.Directory.GetFiles(Dir.FullName))
            {
                string desfilename = System.IO.Path.GetDirectoryName(filename) + "\\" + System.IO.Path.GetFileName(filename).Replace(txtSourcestr.Text, txtReplacestr.Text);
                try
                {
                    if (!System.IO.File.Exists(desfilename)) System.IO.File.Move(filename, desfilename);
                }
                catch
                {
                    rtb.AppendText(filename + Environment.NewLine);
                }

            }
            //FileInfo[] files = Dir.GetFiles();
            //foreach (FileInfo file in files)
            //{
            //    string sourcefilename = file.FullName;
            //    string desfilename = sourcefilename.Replace(txtSourcestr.Text, txtReplacestr.Text);
            //    file.MoveTo(desfilename);
            //}
        }

        private void button7_Click_1(object sender, EventArgs e)
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
                addBeginEndFilename(fbd.SelectedPath, txtAddExt.Text.Trim());
            }
        }

        private void button9_Click(object sender, EventArgs e)
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
                compareFIle(fbd.SelectedPath);
            }
        }
        private void compareFIle(string filepath)
        {
            DirectoryInfo DIR = new DirectoryInfo(filepath);
            DirectoryInfo[] dirs = DIR.GetDirectories();
            foreach (DirectoryInfo dir in dirs)
            {
                compareFIle(dir.FullName);
            }
            FileInfo[] files = DIR.GetFiles("*.mp4");
            string p300 = string.Empty;
            foreach (FileInfo file in files)
            {
                p300 = Path.GetFileNameWithoutExtension(file.Name);
                if (file.Name.Length < 9)
                {
                    if (!File.Exists(file.DirectoryName + "\\" + p300 + "_300p.mp4")) rtb.AppendText(file.FullName + ":\t 300p NO Exists" + Environment.NewLine);
                    continue;
                }
                else if (file.Name.Substring(file.Name.Length - 9) == "_300p.mp4") continue;
                if (!File.Exists(file.DirectoryName + "\\" + p300 + "_300p.mp4")) rtb.AppendText(file.FullName + ":\t 300p NO Exists" + Environment.NewLine);
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            CompareFileFrm cff = new CompareFileFrm();
            cff.Owner = this;
            cff.ShowDialog();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            ChangeImageSize cis = new ChangeImageSize();
            cis.ShowDialog();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (txtStart.Text.Trim() == "") return;
            if (txtPattern.Text.Trim() == "") return;
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            string folderpath = Application.StartupPath + "\\folder.txt";
            if (File.Exists(folderpath))
            {
                fbd.SelectedPath = File.ReadAllText(folderpath);
            }
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(folderpath, fbd.SelectedPath);
                filenamelist.Clear();
                getfilenamelist(fbd.SelectedPath, txtPattern.Text);
                foreach (string filename in filenamelist)
                {
                    File.Delete(filename);
                }
            }
        }
        List<string> filenamelist = new List<string>();
        private void getfilenamelist(string path, string pattern)
        {
            DirectoryInfo DIR = new DirectoryInfo(path);
            DirectoryInfo[] dirs = DIR.GetDirectories();
            foreach (DirectoryInfo dir in dirs) getfilenamelist(dir.FullName, pattern);
            if (!pattern.StartsWith("*") && !pattern.StartsWith("?")) pattern = "*" + pattern;
            FileInfo[] files = DIR.GetFiles(pattern);
            foreach (FileInfo file in files)
            {
                if (rbStartYes.Checked)
                {
                    if (file.Name.StartsWith(txtStart.Text))
                        filenamelist.Add(file.FullName);
                }
                else if (rbstartNo.Checked)
                {
                    if (!file.Name.StartsWith(txtStart.Text)) filenamelist.Add(file.FullName);
                }
                else if (rbEndyes.Checked)
                {
                    if (file.Name.EndsWith(txtStart.Text)) filenamelist.Add(file.FullName);
                }
                else if (rbEndno.Checked)
                {
                    if (!file.Name.EndsWith(txtStart.Text)) filenamelist.Add(file.FullName);
                }
                else if (rbContain.Checked)
                {
                    if (file.Name.Contains(txtStart.Text)) filenamelist.Add(file.FullName);
                }
                else if (rbContainNo.Checked)
                {
                    if (!file.Name.Contains(txtStart.Text)) filenamelist.Add(file.FullName);
                }
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            if (txtsourceext.Text.Trim() == "") return;
            if (txtdesext.Text.Trim() == "") return;
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            string folderpath = Application.StartupPath + "\\folder.txt";
            string oldext = txtsourceext.Text.Trim(), newext = txtdesext.Text.Trim();
            if (File.Exists(folderpath))
            {
                fbd.SelectedPath = File.ReadAllText(folderpath);
            }
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(folderpath, fbd.SelectedPath);
                System.IO.DirectoryInfo DIR = new DirectoryInfo(fbd.SelectedPath);
                System.IO.FileInfo[] fileinfos = DIR.GetFiles(oldext.StartsWith("*") ? oldext : ("*" + oldext));
                foreach (System.IO.FileInfo file in fileinfos)
                {
                    string newfilename = System.IO.Path.ChangeExtension(file.FullName, newext.StartsWith("*") ? newext.Substring(1) : newext);
                    if (!File.Exists(newfilename)) File.Move(file.FullName, newfilename);
                }
            }
        }
        List<string> dirlist = new List<string>();
        void changedirname(DirectoryInfo DIR)
        {

            DirectoryInfo[] dirs = DIR.GetDirectories();
            foreach (DirectoryInfo dir in dirs) changedirname(dir);
            if (rbChange.Checked)
            {
                if (rbfirst.Checked)
                {
                    if (DIR.Name.StartsWith(txtSource.Text.Trim()))
                    {
                        int sindex = DIR.FullName.LastIndexOf("\\");
                        string dpath = DIR.FullName.Substring(0, sindex + 1) + txtdest.Text.Trim() + DIR.Name.Substring(txtdest.Text.Trim().Length);
                        if (!Directory.Exists(dpath))
                            try
                            {
                                DIR.MoveTo(dpath);
                            }
                            catch
                            {

                            }
                    }
                }
                else if (rbstrcenter.Checked)
                {

                }
                else if (rbEnd.Checked)
                {

                }
            }
            else if (rbDel.Checked)
            {

            }
            else if (rbadd.Checked)
            {

            }
        }

        private void btnCopyFile_Click(object sender, EventArgs e)
        {
            string[] filenames = rtb.Lines;
            string file, dest;
            DaveChambers.FolderBrowserDialogEx.FolderBrowserDialogEx fbde = new FolderBrowserDialogEx();
            if (fbde.ShowDialog(this) == DialogResult.OK)
            {
                foreach (string filename in filenames)
                {
                    file = txtFilenamePrefix.Text.Trim() + filename.Trim();
                    dest = fbde.SelectedPath + "\\" + Path.GetFileName(file);
                    if (File.Exists(file))
                    {
                        File.Copy(file, dest, true);
                    }
                }
            }
        }

        private void btnChangeDirname_Click(object sender, EventArgs e)
        {
            DaveChambers.FolderBrowserDialogEx.FolderBrowserDialogEx fbde = new FolderBrowserDialogEx();
            fbde.ShowEditbox = true;
            string folderpath = Application.StartupPath + "\\folder.txt";
            if (File.Exists(folderpath))
            {
                fbde.SelectedPath = File.ReadAllText(folderpath);
            }
            if (fbde.ShowDialog(this) == DialogResult.OK)
            {
                File.WriteAllText(folderpath, fbde.SelectedPath);
                System.IO.DirectoryInfo DIR = new DirectoryInfo(fbde.SelectedPath);
                changedirname(DIR);
            }
            if (rbChange.Checked)
            {

            }
            else if (rbDel.Checked)
            {

            }
            else if (rbadd.Checked)
            {

            }
        }

        private void Undo_Click(object sender, EventArgs e)
        {
            SendKeys.Send("^Z");
        }
        StringBuilder sb=new StringBuilder();
        private void getSubDirnames(string dirname)
        {
            sb.AppendLine(dirname);
            foreach (string dir in System.IO.Directory.GetDirectories(dirname))getSubDirnames(dir);
           
        }
        private void button8_Click(object sender, EventArgs e)
        {
            FolderBrowserDialogEx fbde = new DaveChambers.FolderBrowserDialogEx.FolderBrowserDialogEx();
            fbde.ShowEditbox = true;
            fbde.ShowNewFolderButton = false;
            fbde.StartPosition = FormStartPosition.CenterParent;
            fbde.Title = "选择文件夹";
            string folderpath = Application.StartupPath + "\\folder.txt";
            if (File.Exists(folderpath))
            {
                fbde.SelectedPath = File.ReadAllText(folderpath);
            }

            if (fbde.ShowDialog(this) == DialogResult.OK)
            {
                File.WriteAllText(folderpath, fbde.SelectedPath);
                
                if (!chkAllLevel.Checked)
                    foreach (string dir in Directory.GetDirectories(fbde.SelectedPath))
                    {
                        rtb.AppendText(dir + Environment.NewLine);
                    }
                else
                {
                    sb.Clear();
                    getSubDirnames(fbde.SelectedPath);
                    rtb.AppendText(sb.ToString());
                }
            }
        }

        private void btnCopyRename_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string sourcefilename = ofd.FileName;
                FolderBrowserDialogEx fbde = new DaveChambers.FolderBrowserDialogEx.FolderBrowserDialogEx();
                fbde.ShowEditbox = true;
                fbde.ShowNewFolderButton = false;
                fbde.StartPosition = FormStartPosition.CenterParent;
                fbde.Title = "选择文件夹";
                string folderpath = Application.StartupPath + "\\folder.txt";
                if (File.Exists(folderpath))
                {
                    fbde.SelectedPath = File.ReadAllText(folderpath);
                }

                if (fbde.ShowDialog(this) == DialogResult.OK)
                {
                    File.WriteAllText(folderpath, fbde.SelectedPath);
                    copytodir(fbde.SelectedPath, sourcefilename);
                }
            }
        }
        private void copytodir(string DIR, string filename)
        {
            foreach (string dir in Directory.GetDirectories(DIR)) copytodir(dir, filename);
            string[] files = Directory.GetFiles(DIR, "*.swf");
            if (files.Length > 0)
            {
                string file = files[0];
                string newfile = file.Substring(0, file.Length - 4) + "_q.swf";
                if (File.Exists(file) && !File.Exists(newfile)) File.Move(file, newfile);
                if (File.Exists(filename) && !File.Exists(file)) File.Copy(filename, file);
            }
        }

        private void btnMergeImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "*.jpg;*.png;*.bmp|*.jpg;*.png;*.bmp";
            ofd.Title = "请选择图片";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string sourcefilename = ofd.FileName;
                FolderBrowserDialogEx fbde = new DaveChambers.FolderBrowserDialogEx.FolderBrowserDialogEx();
                fbde.ShowEditbox = true;
                fbde.ShowNewFolderButton = false;
                fbde.StartPosition = FormStartPosition.CenterParent;
                fbde.Title = "选择文件夹";
                string folderpath = Application.StartupPath + "\\folder.txt";
                if (File.Exists(folderpath))
                {
                    fbde.SelectedPath = File.ReadAllText(folderpath);
                }

                if (fbde.ShowDialog(this) == DialogResult.OK)
                {
                    File.WriteAllText(folderpath, fbde.SelectedPath);
                    Image img = Bitmap.FromFile(sourcefilename);
                    mergeImage(fbde.SelectedPath, img);
                }
            }
        }
        /// <summary>
        /// 将img画到图片文件的左上角中
        /// </summary>
        /// <param name="DIR">遍历的路径名</param>
        /// <param name="img">图片</param>
        private void mergeImage(string DIR, Image img)
        {
            foreach (string dir in Directory.GetDirectories(DIR)) mergeImage(dir, img);
            string[] files = Directory.GetFiles(DIR);
            string ext, tempfile;
            Image simg;
            int i = 0;
            System.Drawing.Imaging.ImageFormat imgfmt = System.Drawing.Imaging.ImageFormat.Jpeg;
            foreach (string file in files)
            {
                ext = Path.GetExtension(file).ToLower();
                if (ext != ".jpg" && ext != ".png" && ext != ".bmp") continue;
                if (ext == ".png") imgfmt = System.Drawing.Imaging.ImageFormat.Png;
                else if (ext == ".bmp") imgfmt = System.Drawing.Imaging.ImageFormat.Bmp;
                i = 0;
            tem: tempfile = string.Format(Path.GetTempPath() + "\\tempimagefile{0}" + ext, i++);
                try
                {
                    if (File.Exists(tempfile)) File.Delete(tempfile);
                }
                catch
                {
                    goto tem;
                }
                File.Copy(file, tempfile);
                if (ext == ".jpg" || ext == ".png" || ext == ".bmp")
                {
                    simg = Bitmap.FromFile(tempfile);
                    Graphics gp = Graphics.FromImage(simg);
                    gp.DrawImage(img, 0, 0);
                    System.IO.File.Delete(file);
                    simg.Save(file, imgfmt);
                }
            }
        }
    }
}