using System;
using System.Configuration;
using System.IO;
using System.Text.Json;
using Test.Commen.Enum;
using Test.Functions;

namespace Test.Services.Persistence
{
    public class FileSettingService : ISettingsService
    {
        private static string GetSettingsFilePath()
        {
            var filePath = ConfigurationManager.AppSettings["SettingsFilePath"];
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            return Path.Combine(appDataPath, filePath);
        }
        public FunctionSettings LoadSettings()
        {
            var filePath = GetSettingsFilePath();
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<FunctionSettings>(json);
            }
            return  new FunctionSettings 
                         {
                          Amplitude = 1,
                          Frequency = 1,
                          Phase = 0,
                          Shift = 0,
                          Step = 0.1,
                          XMin = -10,
                          XMax = 10,
                          YMin = -2,
                          YMax = 2,
                          FunctionType = FunctionType.Sine
            }; // If there are no documents, return to the default settings
        }

        public void SaveSettings(FunctionSettings settings)
        {
            var json = JsonSerializer.Serialize(settings);
            var filePath = GetSettingsFilePath();
            Directory.CreateDirectory(Path.GetDirectoryName(filePath)); // ensure that filePath exists
            File.WriteAllText(filePath, json);
        }
    }
}
