using Microsoft.WindowsAPICodePack.Dialogs;
using System;

class FileDialogSample
{
    public static void Main()
    {
        // Microsoft.WindowsAPICodePack.Dialogs
        using (var dialog = new CommonOpenFileDialog { IsFolderPicker = true }) {
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok) {
                Console.WriteLine(dialog.FileName);
            } else {
                return;
            }
        }

        //  System.Windows.Forms
        using (var dialog = new System.Windows.Forms.FolderBrowserDialog()) {
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                Console.WriteLine(dialog.SelectedPath);
            }
        }
    }
}
