using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace TVProgram
{

    public partial class MainWindow : Window
    {

        public int VarTimeZone = 0;
        public string IdChannel;
        public string directory = AppDomain.CurrentDomain.BaseDirectory;
        public MainWindow()
        {
            InitializeComponent();
            DG1.Items.Clear();
            ChannelBox.Items.Clear();
        }
        private void grid_Loaded(object sender, RoutedEventArgs e)
        {
            //LoadChannels();
        }

        public String StrToDate(String str, int hours)
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

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            (sender as Slider).Value = Math.Round(e.NewValue, 0);
            VarTimeZone = Convert.ToInt16(Math.Round(e.NewValue, 0));
            TimeLabel.Content = VarTimeZone;
            UpdateGrid();
        }

        public void LoadChannels()
        {
            List<Channel> TvChnl = new List<Channel>();
            XDocument xdoc = XDocument.Load(directory + "xmltv.xml");
            var items = from xChnl in xdoc.Element("tv").Elements("channel")
                        select new Channel(xChnl.Attribute("id").Value, xChnl.Element("display-name").Value);
            foreach (var it in items) TvChnl.Add(it);
            ChannelBox.ItemsSource = TvChnl;
        }

        public void UpdateGrid()
        {
            List<TvProgramm> TV = new List<TvProgramm>();
            XDocument xdoc = XDocument.Load(directory + "xmltv.xml");
            var items = from xProg in xdoc.Element("tv").Elements("programme")
                        where xProg.Attribute("channel").Value == IdChannel
                        select new TvProgramm(StrToDate((xProg.Attribute("start").Value), VarTimeZone), xProg.Element("title").Value);
            foreach (var it in items) TV.Add(it);
            DG1.ItemsSource = TV;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            Channel ch1 = (Channel)ChannelBox.SelectedValue;
            IdChannel = ch1.Id;
            UpdateGrid();
        }

        public void DownLoad()
        {
            DirectoryInfo directorySelected = new DirectoryInfo(directory);
            foreach (FileInfo fileToDel in directorySelected.GetFiles("*.xml"))
            fileToDel.Delete();
            WebClient WebCln = new WebClient();
            string URL = "http://programtv.ru/xmltv.xml.gz";
            WebCln.DownloadFile(URL, directory + "xmltv.xml.gz");
        }

        public void UnPack()
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

        private void DLButton_Click(object sender, RoutedEventArgs e)
        {
            DownLoad();
            System.Threading.Thread.Sleep(5000);
            UnPack();
            System.Threading.Thread.Sleep(5000);
            LoadChannels();
        }
    }
}
