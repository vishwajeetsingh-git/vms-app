using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace VMS.Infrastructure
{
    public static class NativeMethods
    {
        public const int GWL_EXSTYLE      = -20;
        public const int WS_EX_APPWINDOW  = 0x40000;
        public const int WS_EX_TOOLWINDOW = 0x80;
        public const int WM_GETMINMAXINFO = 0x0024;
        public const uint MONITOR_DEFAULTTONEAREST = 2;

        [DllImport("user32.dll")] public static extern int  GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")] public static extern int  SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        [DllImport("user32.dll")] public static extern IntPtr MonitorFromWindow(IntPtr hwnd, uint dwFlags);
        [DllImport("user32.dll")] public static extern bool   GetMonitorInfo(IntPtr hMonitor, ref MONITORINFO lpmi);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT { public int Left, Top, Right, Bottom; }

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT { public int X, Y; }

        [StructLayout(LayoutKind.Sequential)]
        public struct MINMAXINFO
        {
            public POINT ptReserved, ptMaxSize, ptMaxPosition, ptMinTrackSize, ptMaxTrackSize;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct MONITORINFO
        {
            public int cbSize;
            public RECT rcMonitor, rcWork;
            public uint dwFlags;
        }

        public static void HandleMaximize(Window window, IntPtr lParam)
        {
            var mmi = (MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(MINMAXINFO))!;
            var hwnd = new WindowInteropHelper(window).Handle;
            var monitor = MonitorFromWindow(hwnd, MONITOR_DEFAULTTONEAREST);
            if (monitor != IntPtr.Zero)
            {
                var info = new MONITORINFO { cbSize = Marshal.SizeOf<MONITORINFO>() };
                GetMonitorInfo(monitor, ref info);
                var work = info.rcWork;
                var mon  = info.rcMonitor;
                mmi.ptMaxPosition = new POINT { X = Math.Abs(work.Left - mon.Left), Y = Math.Abs(work.Top - mon.Top) };
                mmi.ptMaxSize     = new POINT { X = Math.Abs(work.Right - work.Left), Y = Math.Abs(work.Bottom - work.Top) };
            }
            Marshal.StructureToPtr(mmi, lParam, true);
        }
    }
}
