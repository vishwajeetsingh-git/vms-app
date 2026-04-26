using System.Windows;
using System.Windows.Media;

namespace VMS.Infrastructure
{
    public static class ThemeManager
    {
        private static string _current = "Dark";

        public static string GetCurrentTheme() => _current;

        public static void ApplyTheme(string theme)
        {
            _current = theme;
            var r = Application.Current.Resources;

            if (theme == "Light")
            {
                r["BackgroundColor"]    = B(0xF5, 0xF5, 0xF5);
                r["SurfaceColor"]       = B(0xFF, 0xFF, 0xFF);
                r["SidebarColor"]       = B(0xEC, 0xEC, 0xEC);
                r["CardBackgroundColor"]= B(0xFF, 0xFF, 0xFF);
                r["BorderColor"]        = B(0xD0, 0xD0, 0xD0);
                r["TopBarColor"]        = B(0xEC, 0xEC, 0xEC);
                r["MenuHoverColor"]     = B(0xE0, 0xE0, 0xE0);
                r["MenuActiveColor"]    = B(0xD0, 0xD0, 0xD0);
                r["TextPrimaryColor"]   = B(0x1C, 0x1C, 0x1E);
                r["TextSecondaryColor"] = B(0x8E, 0x8E, 0x93);
                r["InputBackgroundColor"]=B(0xFF,0xFF,0xFF);
                r["InputTextColor"]     = B(0x1C, 0x1C, 0x1E);
                r["PrimaryColor"]       = B(0x00, 0x7A, 0xFF);
                r["SecondaryColor"]     = B(0x58, 0x56, 0xD6);
                r["SuccessColor"]       = B(0x34, 0xC7, 0x59);
                r["WarningColor"]       = B(0xFF, 0x95, 0x00);
                r["ErrorColor"]         = B(0xFF, 0x3B, 0x30);
                r["SidebarColor"]       = B(0xF0, 0xF0, 0xF0);
            }
            else
            {
                r["BackgroundColor"]    = B(0x1E, 0x1E, 0x1E);
                r["SurfaceColor"]       = B(0x25, 0x25, 0x26);
                r["SidebarColor"]       = B(0x18, 0x18, 0x18);
                r["CardBackgroundColor"]= B(0x2D, 0x2D, 0x30);
                r["BorderColor"]        = B(0x3E, 0x3E, 0x42);
                r["TopBarColor"]        = B(0x18, 0x18, 0x18);
                r["MenuHoverColor"]     = B(0x2A, 0x2D, 0x2E);
                r["MenuActiveColor"]    = B(0x04, 0x4F, 0x7C);
                r["TextPrimaryColor"]   = B(0xD4, 0xD4, 0xD4);
                r["TextSecondaryColor"] = B(0x85, 0x85, 0x85);
                r["InputBackgroundColor"]=B(0x3C,0x3C,0x3C);
                r["InputTextColor"]     = B(0xFF, 0xFF, 0xFF);
                r["PrimaryColor"]       = B(0x00, 0x7A, 0xCC);
                r["SecondaryColor"]     = B(0x56, 0x9C, 0xD6);
                r["SuccessColor"]       = B(0x6A, 0x99, 0x55);
                r["WarningColor"]       = B(0xCE, 0x91, 0x78);
                r["ErrorColor"]         = B(0xF4, 0x87, 0x71);
            }
        }

        private static SolidColorBrush B(byte r, byte g, byte b)
            => new SolidColorBrush(Color.FromRgb(r, g, b));
    }
}
