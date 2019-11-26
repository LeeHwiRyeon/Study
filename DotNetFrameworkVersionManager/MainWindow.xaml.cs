using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
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
        private Config m_config;
        private Dictionary<string, ComboBox> m_dataComboBox = new Dictionary<string, ComboBox>();
        private readonly string DataPath = Directory.GetCurrentDirectory() + "\\Config.xml";
        public MainWindow()
        {
            InitializeComponent();

            m_config = Util.Load<Config>(DataPath);
            if (m_config == null) {
                m_config = Config.DefaultConfig();
                Util.Save(m_config, DataPath);
            }

            foreach (var changeData in m_config.ChangeDatas) {
                var label = new Label {
                    Content = changeData.Start
                };
                ProjectDatas.Children.Add(label);

                var comboBox = new ComboBox();
                foreach (var item in changeData.Items) {
                    comboBox.Items.Add(item);
                }

                comboBox.SelectedIndex = changeData.SelectedIndex;
                ProjectDatas.Children.Add(comboBox);
                if (!m_dataComboBox.ContainsKey(changeData.Start)) {
                    m_dataComboBox.Add(changeData.Start, comboBox);
                }
            }
        }

        private void Button_SetPath(object sender, RoutedEventArgs e)
        {
            // FolderBrowserDialog
            using (var dialog = new CommonOpenFileDialog {
                IsFolderPicker = true
            }) {
                if (dialog.ShowDialog() == CommonFileDialogResult.Ok) {
                    m_path = dialog.FileName;
                    pathLabel.Content = "PATH:" + m_path;
                } else {
                    return;
                }
            }

            // Directory.GetCurrentDirectory
            try {
                m_filePaths = Directory.GetFiles(m_path, CSPROJ, SearchOption.AllDirectories);
            } catch {
                MessageBox.Show("경로가 이상함.");
                return;
            }

            CheckItems.Children.Clear();
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

            foreach (string filePath in m_filePaths) {
                foreach (var checkItem in CheckItems.Children) {
                    if (checkItem is CheckBox cb) {
                        if (!cb.IsChecked ?? true) {
                            continue;
                        }

                        var split = filePath.Split('\\');
                        if (cb.Content.ToString() != split[split.Length - 1]) {
                            continue;
                        }

                        var readTexts = File.ReadAllLines(filePath);
                        for (int i = 0; i < readTexts.Length; i++) {
                            foreach (var changeData in m_config.ChangeDatas) {
                                if (!m_dataComboBox.TryGetValue(changeData.Start, out var comboBox)) {
                                    continue;
                                }

                                var selectedIndex = comboBox.SelectedIndex;
                                if (comboBox.SelectedIndex <= -1) {
                                    selectedIndex = 0;
                                }

                                if (readTexts[i].Contains(changeData.Start)) {
                                    if (changeData.Items.Length <= selectedIndex) {
                                        continue;
                                    }

                                    var result = $"{changeData.Start}{changeData.Items[selectedIndex]}{changeData.End}";
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

        private void ClosedWindow(object sender, EventArgs e)
        {
            foreach (var changeData in m_config.ChangeDatas) {
                if (!m_dataComboBox.TryGetValue(changeData.Start, out var comboBox)) {
                    continue;
                }

                var selectedIndex = comboBox.SelectedIndex;
                if (comboBox.SelectedIndex <= -1) {
                    selectedIndex = 0;
                }

                changeData.SelectedIndex = selectedIndex;
            }
            Util.Save(m_config, DataPath);
        }
    }
}
