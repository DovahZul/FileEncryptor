using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
namespace ProjectDiplom
{
    static class ComCtrlExtensions
    {
        private const string EXPLORER = "EXPLORER";
 
        [DllImport("uxtheme.dll", ExactSpelling = true, PreserveSig = false, CharSet = CharSet.Auto)]
        public static extern void SetWindowTheme(IntPtr hWnd, string subAppName, string subIdList);
 
        public static void WindowExplorerTheme(this ListView ctrl, bool enable)
        {
            string appName = enable ? EXPLORER : null;
            SetWindowTheme(ctrl.Handle, appName, null);
        }
    }
}
