using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CityLizard.Xml.Utf8
{
    using W = Microsoft.Win32;
    using L = System.Xml.Linq;
    using X = System.Xml;
    using IO = System.IO;
    using R = System.Text.RegularExpressions;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Invoke(Action action)
        {
            this.Dispatcher.Invoke(action);
        }

        private void ThreadAddText(string text)
        {
            this.Invoke(() => this.textBox.AppendText(text));
        }

        private void convertFiles_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new W.OpenFileDialog()
            {
                Multiselect = true,
                Filter = "FictionBook (*.fb2)|*.fb2",
            };
            if (openFileDialog.ShowDialog() == true)
            {
                this.convertFiles.IsEnabled = false;
                this.progressBar.Value = 0;
                this.progressBar.Maximum = openFileDialog.FileNames.Length;
                var thread = new System.Threading.Thread(
                () =>
                {
                    var i = 0;
                    foreach (var file in openFileDialog.FileNames)
                    {
                        try
                        {
                            // detect encoding.
                            string encoding = null;
                            using (var reader = X.XmlReader.Create(file))
                            {
                                if (reader.Read())
                                {
                                    if (reader.NodeType ==
                                        X.XmlNodeType.XmlDeclaration)
                                    {
                                        encoding = reader.GetAttribute("encoding");
                                    }
                                }
                            }
                            //
                            string text;
                            using (var reader =
                                new IO.StreamReader(
                                    file, Encoding.GetEncoding(encoding)))
                            {
                                text = reader.ReadToEnd();
                            }
                            text = R.Regex.Replace(text, "<([^>]*<)", "&lt;$1");
                            // text = R.Regex.Replace(text, "<([^/\\?A-Za-z])", "&lt;$1");
                            //
                            var xml = L.XDocument.Parse(text);
                            xml.Declaration.Encoding = "utf-8";
                            xml.Save(file, L.SaveOptions.DisableFormatting);
                        }
                        catch (Exception ex)
                        {
                            this.ThreadAddText(
                                "file: " + file + "\n" +
                                "error: " + ex.Message + "\n");
                        }
                        ++i;
                        this.Invoke(() => this.progressBar.Value = i);
                    }
                    this.Invoke(() => this.convertFiles.IsEnabled = true);
                });
                thread.Start();
            }
        }

        private void clear_Click(object sender, RoutedEventArgs e)
        {
            this.textBox.Clear();
        }
    }
}
