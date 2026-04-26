using System;
using System.Windows;
using System.Windows.Interop;

namespace VMS.Infrastructure
{
    public class ChromeWindow : Window
    {
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            var helper = new WindowInteropHelper(this);
            var source = HwndSource.FromHwnd(helper.Handle);
            if (source == null || source.Handle == IntPtr.Zero) return;

            var style = NativeMethods.GetWindowLong(source.Handle, NativeMethods.GWL_EXSTYLE);
            style |=  NativeMethods.WS_EX_APPWINDOW;
            style &= ~NativeMethods.WS_EX_TOOLWINDOW;
            NativeMethods.SetWindowLong(source.Handle, NativeMethods.GWL_EXSTYLE, style);

            source.AddHook(WndProc);
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == 0x0024) // WM_GETMINMAXINFO
            {
                NativeMethods.HandleMaximize(this, lParam);
                handled = true;
            }
            return IntPtr.Zero;
        }

        protected void TitleBar_Minimize()         => WindowState = WindowState.Minimized;
        protected void TitleBar_ToggleMaximize()   => WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        protected void TitleBar_Close()            => Close();
    }
}
