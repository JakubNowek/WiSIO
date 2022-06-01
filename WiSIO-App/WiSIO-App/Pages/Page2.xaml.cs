﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using Renci.SshNet;

namespace WiSIO_App.Pages
{
    /// <summary>
    /// Interaction logic for Page2.xaml
    /// </summary>
    public partial class Page2 : Page
    {
        public Page2()
        {
            InitializeComponent();
        }

        private void ButtonMakePhoto_OnClick(object sender, RoutedEventArgs e)
        {
            using (var client = new SshClient("192.168.1.14", "pi", "hehexdxd"))
            {
                client.Connect();
                client.RunCommand("python3 -u make_photo.py");
                client.Disconnect();
            }
            var path = ProjectSourcePath.Value + "tmp\\picture" + DateTime.Now.ToString("h-mm-ss") +".jpg";
            using (var client = new ScpClient("192.168.1.14", "pi", "hehexdxd"))
            {
                client.Connect();
                using (Stream localFile = File.Create(path))
                {
                    client.Download("/tmp/picture.jpg", localFile);
                }
                client.Disconnect();
            }
            var bi3 = new BitmapImage();
            bi3.BeginInit();
            bi3.UriSource = new Uri(path, UriKind.Absolute);
            bi3.EndInit();
            BoardImage.Source = bi3;
            BoardImage.Visibility = Visibility.Visible;
        }

        private void ButtonFileSelect_OnClick(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Multiselect = true,
                Filter = "Png files (*.png)|*.png|All files (*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };
            if (openFileDialog.ShowDialog() == true)
            {
                var bi3 = new BitmapImage();
                bi3.BeginInit();
                bi3.UriSource = new Uri(openFileDialog.FileName, UriKind.Absolute);
                bi3.EndInit();
                BoardImage.Source = bi3;
                BoardImage.Visibility = Visibility.Visible;
            }
        }
    }
}