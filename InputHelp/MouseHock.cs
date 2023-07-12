using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace FindCursor.InputHelp
{
    public class MouseHook
    {
        public const int WH_MOUSE_LL = 14; //可以截获整个系统所有模块的鼠标事件。
        public const int WM_MOUSEMOVE = 0x200; // 鼠标移动

        public delegate int HookProc(int nCode, Int32 wParam, IntPtr lParam);
        public delegate void MyMouseEventHandler(Int32 wParam, MouseHookStruct mouseMsg);

        private event MyMouseEventHandler OnMouseActivity;

        private HookProc _mouseHookProcedure;

        // 鼠标钩子句柄
        private static int _hMouseHook = 0;

        // 锁
        private readonly object lockObject = new object();

        // 当前状态,是否已经启动
        private bool isStart = false;

        [StructLayout(LayoutKind.Sequential)]
        public class MouseHookStruct
        {
            public Point p; // 鼠标位置
            public int hWnd;
            public int wHitTestCode;
            public int dwExtraInfo;
        }

        // 装置钩子的函数
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);

        // 卸下钩子的函数
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern bool UnhookWindowsHookEx(int idHook);

        // 下一个钩挂的函数
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int CallNextHookEx(int idHook, int nCode, Int32 wParam, IntPtr lParam);


        private static volatile MouseHook MyMouseHook;
        private readonly static object createLock = new object();
        private MouseHook() { }

        public static MouseHook GetMouseHook()
        {
            if (MyMouseHook == null)
            {
                lock (createLock)
                {
                    if (MyMouseHook == null)
                    {
                        MyMouseHook = new MouseHook();
                    }
                }
            }
            return MyMouseHook;
        }
        ~MouseHook()
        {
            Stop();
        }

        public void Start()
        {
            if (isStart)
            {
                return;
            }
            lock (lockObject)
            {
                if (isStart)
                {
                    return;
                }
                if (OnMouseActivity == null)
                {
                    throw new Exception("Please set handler first!Then run Start");
                }
                // 安装鼠标钩子
                if (_hMouseHook == 0)
                {
                    // 生成一个HookProc的实例.
                    _mouseHookProcedure = new HookProc(MouseHookProc);
                    _hMouseHook = SetWindowsHookEx(WH_MOUSE_LL, _mouseHookProcedure, Marshal.GetHINSTANCE(System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0]), 0);
                    //假设装置失败停止钩子
                    if (_hMouseHook == 0)
                    {
                        Stop();
                        throw new Exception("SetWindowsHookEx failed.");
                    }
                }
                isStart = true;
            }
        }

        public void Stop()
        {
            if (!isStart)
            {
                return;
            }
            lock (lockObject)
            {
                if (!isStart)
                {
                    return;
                }
                bool retMouse = true;
                if (_hMouseHook != 0)
                {
                    retMouse = UnhookWindowsHookEx(_hMouseHook);
                    _hMouseHook = 0;
                }
                // 假设卸下钩子失败
                if (!(retMouse))
                    throw new Exception("UnhookWindowsHookEx failed.");
                // 删除所有事件
                OnMouseActivity = null;
                // 标志位改变
                isStart = false;
            }
        }

        private int MouseHookProc(int nCode, Int32 wParam, IntPtr lParam)
        {
            // 假设正常执行而且用户要监听鼠标的消息
            if ((nCode >= 0) && (OnMouseActivity != null))
            {
                MouseHookStruct MyMouseHookStruct = (MouseHookStruct)Marshal.PtrToStructure(lParam, typeof(MouseHookStruct));
                OnMouseActivity(wParam, MyMouseHookStruct);
            }
            // 启动下一次钩子
            return CallNextHookEx(_hMouseHook, nCode, wParam, lParam);
        }

        public MouseHook AddMouseHandler(MyMouseEventHandler handler)
        {
            OnMouseActivity += handler;
            return this;
        }

        public MouseHook RemoveMouseHandler(MyMouseEventHandler handler)
        {
            if (OnMouseActivity != null)
            {
                OnMouseActivity -= handler;
            }

            return this;
        }
    }
}
