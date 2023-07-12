using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FindCursor
{
    internal static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (RunningInstance())
            {
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        public static bool RunningInstance()
        {
            // 判断进程，只能启动一个实例
            Process current = Process.GetCurrentProcess();
            var p = Process.GetProcessesByName(current.ProcessName).FirstOrDefault(x => x.Id != current.Id);
            if (p != null)
            {
                SetForegroundWindow(p.MainWindowHandle);
                SendMessage(p.MainWindowHandle, WM_SYSCOMMAND, SC_RESTORE, 0);
                return true;
            }

            return false;
        }

        [DllImport("user32.dll", EntryPoint = "SetForegroundWindow")]
        public static extern int SetForegroundWindow(IntPtr hwnd);
        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        public const int WM_SYSCOMMAND = 0x112;
        public const int SC_RESTORE = 0xF120;
    }
}
