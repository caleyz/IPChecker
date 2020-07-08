using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace IPChecker
{
    public class FlashWindow
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool FlashWindowEx(ref FLASHWINFO pwfi);

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [StructLayout(LayoutKind.Sequential)]
        public struct FLASHWINFO
        {
            public UInt32 cbSize;
            public IntPtr hwnd;
            public UInt32 dwFlags;
            public UInt32 uCount;
            public Int32 dwTimeout;
        }

        public const UInt32 FLASHW_ALL = 3;
        public const UInt32 FLASHW_TIMERNOFG = 12;
        public const UInt32 FLASHW_STOP = 0x00000004;

        public void FlashWindowNow(IntPtr hWnd)
        {
            FLASHWINFO fInfo = new FLASHWINFO();

            fInfo.cbSize = Convert.ToUInt32(Marshal.SizeOf(fInfo));
            fInfo.hwnd = hWnd;
            fInfo.dwFlags = FLASHW_ALL | FLASHW_TIMERNOFG;
            fInfo.uCount = UInt32.MaxValue;
            fInfo.dwTimeout = 0;

            FlashWindowEx(ref fInfo);
        }

        private static FLASHWINFO GetFLASHWINFO(UInt32 flashwConstant, UInt32 uCount = UInt32.MaxValue, UInt32 dwTimeout = 0)
        {
            FLASHWINFO fInfo = new FLASHWINFO
            {
                hwnd = Process.GetCurrentProcess().MainWindowHandle,
                dwFlags = flashwConstant,
                uCount = uCount
            };
            fInfo.cbSize = Convert.ToUInt32(Marshal.SizeOf(fInfo));
            return fInfo;
        }

        public void StopFlash()
        {
            FLASHWINFO fInfo = GetFLASHWINFO(FLASHW_STOP);
            FlashWindowEx(ref fInfo);
        }
    }
}
