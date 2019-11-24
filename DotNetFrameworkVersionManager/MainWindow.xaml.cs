using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace DotNetFrameworkVersionManager
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string CSPROJ = "*.csproj";


        /// <summary> .NET Framework 버전을 변경할 솔루션 위치 입력 </summary>
        private string m_path;
        private string[] m_filePaths;
        private Config config = new DefaultConfig();
        public MainWindow()
        {
            InitializeComponent();
            var dataPath = Directory.GetCurrentDirectory() + "\\Config.xml";
            //config = Util.Load<Config>(dataPath);
            //if (config == null) {
            //    ();
            //    Util.Save(config, dataPath);
            //}
        }

        private void Button_SetPath(object sender, RoutedEventArgs e)
        {
            // FolderBrowserDialog
            var dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true; if (dialog.ShowDialog() == CommonFileDialogResult.Ok) {
                m_path = dialog.FileName;
                pathLabel.Content = "PATH:" + m_path;
            }

            // Directory.GetCurrentDirectory
            try {
                m_filePaths = Directory.GetFiles(m_path, CSPROJ, SearchOption.AllDirectories);
            } catch {
                MessageBox.Show("경로가 이상함.");
            }

            foreach (var filePath in m_filePaths) {
                CreateCheckBox(filePath);
            }
        }

        private void CreateCheckBox(string path)
        {
            var split = path.Split('\\');
            var cb = new CheckBox {
                Content = split[split.Length - 1],
            };
            CheckItems.Children.Add(cb);
        }

        private void CheckBox_AllClick(object sender, RoutedEventArgs e)
        {
            var cb = e.Source as CheckBox;
            var isChecked = cb.IsChecked ?? false;
            foreach (var item in CheckItems.Children) {
                if (item is CheckBox child) {
                    child.IsChecked = isChecked;
                }
            }
        }

        private void Button_ChangeProjectData(object sender, RoutedEventArgs e)
        {
            if (m_filePaths == null) {
                return;
            }

            int.TryParse(Console.ReadLine(), out var selectVersion);

            foreach (string filePath in m_filePaths) {
                var readTexts = File.ReadAllLines(filePath);
                for (int i = 0; i < readTexts.Length; i++) {
                    foreach (var changeData in config.ChangeDatas) {
                        if (readTexts[i].Contains(changeData.Start)) {
                            var result = $"{changeData.Start}{changeData.Items[changeData.SelectedIndex]}{changeData.End}";
                            readTexts[i] = result;

                            var temp = readTexts[i];
                            var startIndex = temp.IndexOf('>');
                            var endIndex = temp.IndexOf('<', startIndex);
                            var v = temp.Substring(startIndex, endIndex - startIndex);
                        }
                    }
                }

                File.WriteAllLines(filePath, readTexts);
            }
        }
    }
}
