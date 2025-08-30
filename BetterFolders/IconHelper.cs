using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

public static class IconHelper
{
    [DllImport("Shell32.dll", CharSet = CharSet.Auto)]
    private static extern IntPtr ExtractIcon(IntPtr hInst, string lpszExeFileName, int nIconIndex);

    public static BitmapSource GetIcon(string filePath, int index = 0)
    {
        IntPtr hIcon = ExtractIcon(IntPtr.Zero, filePath, index);

        if (hIcon == IntPtr.Zero)
            return null;

        var bitmapSource = Imaging.CreateBitmapSourceFromHIcon(
            hIcon,
            Int32Rect.Empty,
            BitmapSizeOptions.FromEmptyOptions());

        // Cleanup unmanaged handle
        DestroyIcon(hIcon);

        return bitmapSource;
    }

    [DllImport("User32.dll", SetLastError = true)]
    private static extern bool DestroyIcon(IntPtr hIcon);
}
