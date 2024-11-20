using Test.Functions;

namespace Test.Services.Persistence
{
    public interface ISettingsService
    {
        void SaveSettings(FunctionSettings settings);
        FunctionSettings LoadSettings();
    }
}
