using System;
using System.Runtime.InteropServices;

namespace RYCBEditorX.Utils;
public class Win32Helper
{
    [DllImport("user32.dll", EntryPoint = "SetForegroundWindow", SetLastError = true)]
    public static extern void SetForegroundWindow(IntPtr hwnd);
}
