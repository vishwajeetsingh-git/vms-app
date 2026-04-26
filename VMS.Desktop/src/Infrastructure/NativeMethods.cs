using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace VMS.Infrastructure
{
    public static class NativeMethods
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT { public int Left, Top, Right, Bottom; }

        [StructLayout(LayoutKind.Sequential)]
        public struct MINMAXINFO
        {
            public System.Drawing.Point ptReserved, ptMaxSize, ptMaxPosition, ptMinTrackSize, ptMaxTrackSize;
        }

        [DllImport("user32.dll")] public static extern IntPtr MonitorFromWindow(IntPtr hwnd, uint dwFlags);
        [DllImport("user32.dll")] public static extern bool GetMonitorInfo(IntPtr hMonitor, ref MONITORINFO lpmi);

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
            var monitor = MonitorFromWindow(hwnd, 0x00000002);
            if (monitor != IntPtr.Zero)
            {
                var info = new MONITORINFO { cbSize = Marshal.SizeOf<MONITORINFO>() };
                GetMonitorInfo(monitor, ref info);
                var work = info.rcWork;
                mmi.ptMaxPosition = new System.Drawing.Point(work.Left, work.Top);
                mmi.ptMaxSize = new System.Drawing.Point(work.Right - work.Left, work.Bottom - work.Top);
            }
            Marshal.StructureToPtr(mmi, lParam, true);
        }
    }
}
