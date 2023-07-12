using FindCursor.InputHelp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FindCursor
{
    public partial class Form1
    {
        readonly MouseHook mouseHook = MouseHook.GetMouseHook();

        readonly System.Timers.Timer timer = new System.Timers.Timer();

        // 采样间隔
        readonly int Sampling_Interval = 100;

        // 停止间隔阈值( (n * Sampling_Interval) ms)
        readonly int Stop_Interval_Threshold = 300;

        readonly string Cursor_Path = GetFullPath("keli.cur");
        readonly string Icon_Path = GetFullPath("keqing.ico");

        readonly int Cursor_Width = 100;

        readonly int Cursor_Height = 100;
    }
}
