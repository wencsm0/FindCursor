using System;
using System.Runtime.InteropServices;

namespace FindCursor.InputHelp
{
    internal static class CursorControl
    {
        [DllImport("user32.dll")]
        public static extern bool SetSystemCursor(IntPtr hcur, uint id);
        public const uint OCR_NORMAL = 32512;
        public const uint OCR_IBEAM = 32513;

        [DllImport("user32.dll")]
        public static extern IntPtr LoadCursorFromFile(string fileName);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr LoadImage(IntPtr hinst, string lpszName, uint uType, int cxDesired, int cyDesired, uint fuLoad);
        public const uint IMAGE_CURSOR = 2;
        public const uint LR_LOADFROMFILE = 0x10;

        [DllImport("user32.dll")]
        public static extern bool SystemParametersInfo(uint uiAction, uint uiParam, IntPtr pvParam, uint fWinIni);
        public const uint SPI_SETCURSORS = 87;
        public const uint SPIF_SENDWININICHANGE = 2;

        [DllImport("user32.dll")]
        public static extern IntPtr CopyIcon(IntPtr cursor);

        [DllImport("user32.dll")]
        public static extern bool DestroyCursor(IntPtr cursor);

        static IntPtr loadImage;
        static IntPtr copyForIbeam;
        static IntPtr copyForNormal;

        public static void SetCursor(string path, int width, int heigh)
        {
            loadImage = LoadImage(IntPtr.Zero, path, IMAGE_CURSOR, width, heigh, LR_LOADFROMFILE);
            copyForIbeam = CopyIcon(loadImage);
            copyForNormal = CopyIcon(loadImage);

            SetSystemCursor(copyForIbeam, OCR_IBEAM);
            SetSystemCursor(copyForNormal, OCR_NORMAL);
        }

        public static void ResetCursor()
        {
            DestroyCursor(copyForIbeam);
            DestroyCursor(copyForNormal);
            DestroyCursor(loadImage);

            //恢复为系统默认图标
            SystemParametersInfo(SPI_SETCURSORS, 0, IntPtr.Zero, SPIF_SENDWININICHANGE);
        }
    }
}
