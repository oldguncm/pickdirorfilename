using System;
using System.Text;
using System.IO;
using System.Collections;
using zlib;

namespace flashinfo
{
    public class FlashInfo
    {
        private int width, height, version, frameCount, fileLength;
        private float frameRate;
        private bool isCompressed;
        private System.Drawing.Color bkcolor = System.Drawing.Color.White;
        private string filen = string.Empty;
        public FlashInfo(string filename)
        {
            filen = filename;
            if (!File.Exists(filename))
                throw new FileNotFoundException(filename);
            FileStream stream = File.OpenRead(filename);
            BinaryReader reader = new BinaryReader(stream);
            try
            {
                if (stream.Length < 8)
                    throw new InvalidDataException("不是Flash文件格式");
                string flashMark = new string(reader.ReadChars(3));
                if (flashMark != "FWS" && flashMark != "CWS")
                    throw new InvalidDataException("不是Flash文件格式");
                isCompressed = flashMark == "CWS";
                version = Convert.ToInt32(reader.ReadByte());
                fileLength = reader.ReadInt32();

                if (isCompressed)
                {
                    byte[] dataPart = new byte[stream.Length - 8];
                    reader.Read(dataPart, 0, dataPart.Length);

                    MemoryStream outStream = new MemoryStream();
                    outStream.Write(dataPart, 0, dataPart.Length);
                    outStream.Position = 0;

                    outStream = ZDecompressStream(outStream);
                    ProcessCompressedPart(outStream);
                }
                else
                {
                    byte[] dataPart = new byte[30];
                    reader.Read(dataPart, 0, dataPart.Length);
                    MemoryStream dataStream = new MemoryStream(dataPart);
                    try
                    {
                        ProcessCompressedPart(dataStream);
                    }
                    finally
                    {
                        dataStream.Close();
                    }
                }
            }
            finally
            {
                reader.Close();
                stream.Close();
            }
        }

        private MemoryStream ZDecompressStream(MemoryStream CompressedStream)
        {
            MemoryStream tmpStream = new MemoryStream();
            ZOutputStream outZStream = new ZOutputStream(tmpStream);
            try
            {
                byte[] buffer = new byte[2048];
                int len;
                outZStream.Position = 0;
                CompressedStream.Position = 0;
                while ((len = CompressedStream.Read(buffer, 0, 2048)) > 0)
                {
                    outZStream.Write(buffer, 0, len);
                }
                tmpStream.Position = 0;
                return tmpStream;
            }
            finally
            {
                CompressedStream.Dispose();
            }
        }

        private void ProcessCompressedPart(MemoryStream stream)
        {
            BinaryReader reader = new BinaryReader(stream);
            try
            {
                byte[] rect;
                int nbits, totalBits, totalBytes;
                nbits = reader.ReadByte() >> 3;
                totalBits = nbits * 4 + 5;
                totalBytes = totalBits / 8;
                if (totalBits % 8 != 0)
                    totalBytes++;
                reader.BaseStream.Seek(-1, SeekOrigin.Current);
                rect = reader.ReadBytes(totalBytes);
                Byte xs = reader.ReadByte();
                Byte zs = reader.ReadByte();
                if (xs == 0)
                    frameRate = zs;
                else frameRate = float.Parse(string.Format("{1}.{0}", zs, xs));
                frameCount = Convert.ToInt32(reader.ReadInt16());
                BitArray bits = new BitArray(rect);
                bool[] reversedBits = new bool[bits.Length];
                for (int i = 0; i < totalBytes; i++)
                {
                    int count = 7;
                    for (int j = 8 * i; j < 8 * (i + 1); j++)
                    {
                        reversedBits[j + count] = bits[j];
                        count -= 2;
                    }
                }
                bits = new BitArray(reversedBits);
                StringBuilder sbField = new StringBuilder(bits.Length);
                for (int i = 0; i < bits.Length; i++)
                    sbField.Append(bits[i] ? "1" : "0");
                string result = sbField.ToString();
                string widthBinary = result.Substring(nbits + 5, nbits);
                string heightBinary = result.Substring(3 * nbits + 5, nbits);
                width = Convert.ToInt32(FlashInfo.BinaryToInt64(widthBinary) / 20);
                height = Convert.ToInt32(FlashInfo.BinaryToInt64(heightBinary) / 20);

                bool readcolor = false;
                do
                {
                    byte b = reader.ReadByte();
                    if (Convert.ToInt32(b) == 0x43)
                    {
                        readcolor = (Convert.ToInt32(reader.ReadByte()) == 2);
                        if (readcolor) break;
                    }
                } while (reader.BaseStream.Position != fileLength && reader.BaseStream.Position < reader.BaseStream.Length-1);
                if (readcolor)
                {
                    bkcolor = System.Drawing.Color.FromArgb(Convert.ToInt32(reader.ReadByte()), Convert.ToInt32(reader.ReadByte()), Convert.ToInt32(reader.ReadByte()));
                }
                else bkcolor = System.Drawing.Color.White;
            }
            finally
            {
                reader.Close();
            }
        }

        private static long BinaryToInt64(string binaryString)
        {
            if (string.IsNullOrEmpty(binaryString))
                throw new ArgumentNullException();
            long result = 0;
            for (int i = 0; i < binaryString.Length; i++)
            {
                result = result * 2;
                if (binaryString[i] == '1')
                    result++;
            }
            return result;
        }

        #region 属性定义
        public int Width
        {
            get
            {
                return this.width;
            }
        }

        public int Height
        {
            get
            {
                return this.height;
            }
        }

        public int FileLength
        {
            get
            {
                return this.fileLength;
            }
        }

        public int Version
        {
            get
            {
                return this.version;
            }
        }

        public float FrameRate
        {
            get
            {
                return this.frameRate;
            }
        }

        public int FrameCount
        {
            get
            {
                return this.frameCount;
            }
        }

        public bool IsCompressed
        {
            get
            {
                return this.isCompressed;
            }
        }

        public float Duration
        {
            get
            {
                return frameCount / frameRate;
            }
        }

        public System.Drawing.Color BackColor
        {
            get
            {
                return bkcolor;
            }
        }
        #endregion

        public static void CopyStream(System.IO.Stream input, System.IO.Stream output)
        {
            byte[] buffer = new byte[2000];
            int len;
            while ((len = input.Read(buffer, 0, 2000)) > 0)
            {
                output.Write(buffer, 0, len);
            }
            output.Flush();
        }
    }
}
