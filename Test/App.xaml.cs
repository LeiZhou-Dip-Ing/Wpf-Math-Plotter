using Prism.DryIoc;
using Prism.Ioc;
using System.Windows;
using Test.Dialogs.ViewModels;
using Test.Dialogs.Views;
using Test.Functions;
using Test.Services.Export;
using Test.Services.Persistence;
using Test.ViewModels;
using Test.Views;

namespace Test
{
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<Views.MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<MainWindow, MainWindowViewModel>();
            containerRegistry.Register<IPlotModelFactory, PlotModelFactory>();
            containerRegistry.Register<ISettingsService, FileSettingService>();
            containerRegistry.Register<IExportService, ExportService>();
            containerRegistry.RegisterDialog<ExportDialog, ExportDialogViewModel>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            var mainWindowViewModel = Container.Resolve<MainWindowViewModel>();
            mainWindowViewModel.SaveSettings();
            base.OnExit(e);
        }
    }
}
