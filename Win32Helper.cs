using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RYCBEditorX.Utils;
public class Win32Helper
{
    [DllImport("user32.dll", EntryPoint = "SetForegroundWindow", SetLastError = true)]
    public static extern void SetForegroundWindow(IntPtr hwnd);
}
