using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using IWshRuntimeLibrary;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Interop;


namespace BetterFolders;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    // Drag and drop support

    // DragOver event handler
    private void Window_DragOver(object sender, DragEventArgs e)
    {
        // Check if the data being dragged is a file or folder
        if (e.Data.GetDataPresent(DataFormats.FileDrop))
        {
            e.Effects = DragDropEffects.Copy; // Allow copy operation
        }
        else
        {
            e.Effects = DragDropEffects.None; // Disallow drop
        }
        e.Handled = true;
    }

    // Drop event handler
    private void Window_Drop(object sender, DragEventArgs e)
    {
        // Check if the data being dropped is a file or folder
        if (e.Data.GetDataPresent(DataFormats.FileDrop))
        {
            // Get the dropped files/folders
            string[] droppedItems = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string item in droppedItems)
            {
                string appName = Path.GetFileNameWithoutExtension(item);
                string targetPath = item;

                // Resolve shortcut if the target is a .lnk file
                if (item.EndsWith(".lnk", StringComparison.OrdinalIgnoreCase))
                {
                    targetPath = ResolveShortcut(item);
                    appName = Path.GetFileNameWithoutExtension(targetPath);
                }
                
                AddApp(targetPath, appName);
            }
            MessageBox.Show("Shortcuts created successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        else
        {
            MessageBox.Show("Please drop valid files or folders.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        e.Handled = true;
    }

    // Method to add shortcut to application
    private void AddApp(string targetPath, string appName)
    {
        // Add icon to app border
        App1.Background = new ImageBrush(IconHelper.GetIcon(targetPath));

        // Update app name text
        App1Text.Text = appName;
    }

    // Resolve shortcut target path
    private string ResolveShortcut(string shortcutPath)
    {
        WshShell shell = new WshShell();
        IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutPath);
        return shortcut.TargetPath;
    }
}