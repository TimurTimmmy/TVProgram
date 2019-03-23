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

        private string IdChannel;
        private string directory = AppDomain.CurrentDomain.BaseDirectory;
        public MainWindow()
        {
            InitializeComponent();
            DG1.Items.Clear();
            ChannelBox.Items.Clear();
            ChannelBox.ItemsSource = (Pack.LoadChannels());


        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            (sender as Slider).Value = Math.Round(e.NewValue, 0);
            Pack.ChangeTimeZone(Convert.ToInt16(Math.Round(e.NewValue, 0)));
            TimeLabel.Content = Pack.vartimezone;
            DG1.ItemsSource = Pack.UpdateGrid(IdChannel);
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            Channel channel = (Channel)ChannelBox.SelectedValue;
            IdChannel = channel.Id;
            DG1.ItemsSource = Pack.UpdateGrid(IdChannel);
            ChannelLogo.Source = Logo.LoadLogo(channel.Logo);
        }

        private void DLButton_Click(object sender, RoutedEventArgs e)
        {
            Pack.DownLoad();
            System.Threading.Thread.Sleep(1000);
            Pack.UnPack();
            System.Threading.Thread.Sleep(1000);
            MessageBox.Show("Загрузка завершена", "Обновление программы");
            Pack.LoadChannels();
        }
    }
}
