using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Windows;
using System.Xml.Linq;

namespace TVProgram
{
    public class Pack
    {
        private static int VarTimeZone = 0;
        public static int vartimezone
        {
            get { return VarTimeZone; }
        }

        private static string directory = AppDomain.CurrentDomain.BaseDirectory;

        public static int ChangeTimeZone(int TimeZone)
        {
            return VarTimeZone = +TimeZone;
        }

        public static List<Channel> LoadChannels()
        {
            List<Channel> TvChnl = new List<Channel>();
            XDocument xdoc = XDocument.Load(directory + "xmltv.xml");
            var items = from xChnl in xdoc.Element("tv").Elements("channel")
                        select new Channel(xChnl.Attribute("id").Value, xChnl.Element("display-name").Value, xChnl.Element("icon").FirstAttribute.Value) ;
            foreach (var it in items) TvChnl.Add(it);
            return TvChnl;
        }

        public static List<TvProgramm> UpdateGrid(string IdChannel)
        {
            List<TvProgramm> TV = new List<TvProgramm>();
            XDocument xdoc = XDocument.Load(directory + "xmltv.xml");
            var items = from xProg in xdoc.Element("tv").Elements("programme")
                        where xProg.Attribute("channel").Value == IdChannel
                        select new TvProgramm(StrToDate((xProg.Attribute("start").Value), VarTimeZone), xProg.Element("title").Value);
            foreach (var it in items) TV.Add(it);            
            return TV;
        }

        public static void DownLoad()
        {
            DirectoryInfo directorySelected = new DirectoryInfo(directory);
            foreach (FileInfo fileToDel in directorySelected.GetFiles("*.xml"))
                fileToDel.Delete();
            WebClient WebCln = new WebClient();
            string URL = "http://programtv.ru/xmltv.xml.gz";
            WebCln.DownloadFile(URL, directory + "xmltv.xml.gz");
        }

        public static void UnPack()
        {
            DirectoryInfo directorySelected = new DirectoryInfo(directory);
            foreach (FileInfo fileToDecompress in directorySelected.GetFiles("*.gz"))
            {
                using (FileStream originalFileStream = fileToDecompress.OpenRead())
                {
                    string currentFileName = fileToDecompress.FullName;
                    string newFileName = currentFileName.Remove(currentFileName.Length - fileToDecompress.Extension.Length);

                    using (FileStream decompressedFileStream = File.Create(newFileName))
                    {
                        using (GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
                        {
                            decompressionStream.CopyTo(decompressedFileStream);
                            Console.WriteLine($"Decompressed: {fileToDecompress.Name}");
                        }
                    }
                }
                fileToDecompress.Delete();
            }
        }

        public static  String StrToDate(String str, int hours)
        {
            str = str.Substring(0, 14);
            DateTime datetime = new DateTime();
            try
            {
                datetime = DateTime.ParseExact(str, "yyyyMMddHHmmss", CultureInfo.GetCultureInfo("ru-RU")).AddHours(hours);
                return datetime.ToString("dd.MM.yyyy HH:mm:ss");
            }
            catch (FormatException)
            {
                MessageBox.Show("Не верный формат даты");
                return str;
            }
        }
    }
}
