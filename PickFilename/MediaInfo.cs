using System;
using System.Diagnostics;
using System.Drawing;
using MediaInfoNET;

namespace PickFilename
{
    public class MediaInfo
    {
        public MediaInfo(string filename)
        {
            Filename = filename;
        }
        private string _filename,_durationstr;
        private int?  _duration,_width,_height;
        private long? _durationmillis;
        public string Filename
        {
            get { return _filename; }
            set
            {
                try
                {
                    _filename = value;
                    Size? size = getFrameSize();
                    _width = size.Value.Width;
                    _height = size.Value.Height;
                    _duration = getDuration();
                    _durationstr = getDurationStr();
                    _durationmillis = getDurationMillis();
                }
                catch
                {
                    _width = null;
                    _height = null;
                    _duration = null;
                    _durationstr = null;
                    _durationmillis = null;
                }
            }
        }
        /// <summary>
        /// 帧宽度
        /// </summary>
        public int? Width { get { return _width; } }
        /// <summary>
        /// 帧高度
        /// </summary>
        public int? Height { get { return _height; } }
        /// <summary>
        /// 时长字符串（HH:MM:SS）
        /// </summary>
        public string DurationStr { get { return _durationstr; } }
        /// <summary>
        /// 时长(秒)
        /// </summary>
        public int? Duration { get { return _duration; } }
        /// <summary>
        /// 时长的毫秒数
        /// </summary>
        public long? DurationMillis { get { return _durationmillis; } }
        /// <summary>
        /// 得到视频的帧大小
        /// </summary>
        /// <returns>SIZE</returns>
        private Size? getFrameSize()
        {
            try
            {
                if (string.IsNullOrEmpty(_filename)) return null;
                MediaInfoNET.MediaFile mf = new MediaInfoNET.MediaFile(_filename);
                Size size = new Size();
                MediaInfo_Stream_Video msv=mf.Video[0];
                size.Width = msv.Width;
                size.Height = msv.Height;
                return size;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 得到视频的时长（单位：秒）
        /// </summary>
        /// <returns>时长</returns>
        private int getDuration()
        {
            try
            {
                if (string.IsNullOrEmpty(_filename)) return 0;
                MediaInfoNET.MediaFile mf = new MediaInfoNET.MediaFile(_filename);
                MediaInfo_Stream_Video msv = mf.Video[0];
                return (int)(msv.DurationMillis / 1000);
            }
            catch
            {
                return -1;
            }
        }
        /// <summary>
        /// 得到时长字符串HH:MM:SS
        /// </summary>
        /// <returns>时长字符串</returns>
        private string getDurationStr()
        {
            try
            {
                if (string.IsNullOrEmpty(_filename)) return "";
                MediaInfoNET.MediaFile mf = new MediaInfoNET.MediaFile(_filename);
                MediaInfo_Stream_Video msv = mf.Video[0];
                return msv.DurationString;
                //int du= vs.Duration / 1000;
                //return (du / 3600).ToString() + ":" + ((du % 3600) / 60).ToString() + ":" + (du % 60).ToString();
            }
            catch
            {
                return "";
            }
        }
        /// <summary>
        /// 得到视频时长的毫秒数
        /// </summary>
        /// <returns>毫秒数</returns>
        public long getDurationMillis()
        {
            try
            {
                if (string.IsNullOrEmpty(_filename)) return 0;
                MediaInfoNET.MediaFile mf = new MediaInfoNET.MediaFile(_filename);
                MediaInfo_Stream_Video msv = mf.Video[0];
                return msv.DurationMillis;
            }
            catch
            {
                return -1;
            }
        }
        /// <summary>执行一条command命令
        /// 执行一条command命令
        /// </summary>
        /// <param name="command">需要执行的Command</param>
        /// <param name="output">输出</param>
        /// <param name="error">错误</param>
        public static void ExecuteCommand(string command, out string output, out string error)
        {
            try
            {
                //创建一个进程
                Process pc = new Process();
                pc.StartInfo.FileName = command;
                pc.StartInfo.UseShellExecute = false;
                pc.StartInfo.RedirectStandardOutput = true;
                pc.StartInfo.RedirectStandardError = true;
                pc.StartInfo.CreateNoWindow = true;
                //启动进程
                pc.Start();
                //准备读出输出流和错误流
                string outputData = string.Empty;
                string errorData = string.Empty;
                pc.BeginOutputReadLine();
                pc.BeginErrorReadLine();

                pc.OutputDataReceived += (ss, ee) =>
                {
                    outputData += ee.Data;
                };
                pc.ErrorDataReceived += (ss, ee) =>
                {
                    errorData += ee.Data;
                };

                //等待退出
                pc.WaitForExit();
                //关闭进程
                pc.Close();
                //返回流结果
                output = outputData;
                error = errorData;
            }
            catch (Exception)
            {
                output = null;
                error = null;
            }
        }
        /// <summary>
        /// 获取视频的帧宽度和帧高度
        /// </summary>
        /// <param name="videoFilePath">mov文件的路径</param>
        /// <returns>null表示获取宽度或高度失败</returns>
        public static void GetMovWidthAndHeight(string videoFilePath, out int? width, out int? height, out string durationstring)
        {
            try
            {
                //判断文件是否存在
                if (!System.IO.File.Exists(videoFilePath))
                {
                    width = null;
                    height = null;
                }
                //执行命令获取该文件的一些信息 
                string ffmpegPath = new System.IO.FileInfo(Process.GetCurrentProcess().MainModule.FileName).DirectoryName + @"\ffmpeg\ffmpeg.exe";
                string output;
                string error;
                ExecuteCommand("\"" + ffmpegPath + "\"" + " -i " + "\"" + videoFilePath + "\"", out output, out error);
                if (string.IsNullOrEmpty(error))
                {
                    width = null;
                    height = null;
                    durationstring = string.Empty;
                }
                //通过正则表达式获取信息里面的宽度信息
                System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex("(\\d{2,4})x(\\d{2,4})", System.Text.RegularExpressions.RegexOptions.Compiled);
                System.Text.RegularExpressions.Match m = regex.Match(error);
                if (m.Success)
                {
                    width = int.Parse(m.Groups[1].Value);
                    height = int.Parse(m.Groups[2].Value);
                }
                else
                {
                    width = null;
                    height = null;
                }
                System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("\\d{1,2}:\\d{1,2}:\\d{1,2}.\\d{1,2}", System.Text.RegularExpressions.RegexOptions.Compiled);
                m = reg.Match(error);
                if (m.Success)
                {
                    durationstring = m.Value;
                }
                else
                {
                    durationstring = string.Empty;
                }
            }
            catch (Exception)
            {
                width = null;
                height = null;
                durationstring = string.Empty;
            }
        }
    }
}

