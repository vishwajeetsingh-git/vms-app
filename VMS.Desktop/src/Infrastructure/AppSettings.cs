using System.IO;
using System.Text.Json;

namespace VMS.Infrastructure
{
    public class AppSettings
    {
        public string ApiBaseUrl { get; set; } = "http://localhost:8081";
        public string Theme { get; set; } = "Dark";

        private static AppSettings? _instance;
        public static AppSettings Instance => _instance ??= Load();

        private static AppSettings Load()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
            if (!File.Exists(path)) return new AppSettings();
            var json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<AppSettings>(json) ?? new AppSettings();
        }
    }
}
