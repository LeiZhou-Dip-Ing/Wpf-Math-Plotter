using OxyPlot;
using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Test.Commen.Enum;
using Test.Commen.ViewModels;
using Test.Functions;
using Test.Services.Export;
using Test.Services.Persistence;

namespace Test.ViewModels
{
    public class MainWindowViewModel : ValidationViewModelBase
    {

        private readonly IPlotModelFactory _iplotModelFactory;
        private FunctionSettings _functionSettings;
        private PlotModel _selectedPlotModel;
        private ISettingsService _settingsService;
        private IExportService _exportService;
        private readonly IDialogService _dialogService;
        public DelegateCommand ExportCommand { get; }
        public DelegateCommand ResetSettingParamsCommand { get; }
        public MainWindowViewModel(IPlotModelFactory iplotModelFactory, 
                                   ISettingsService settingsService, 
                                   IExportService exportService,
                                   IDialogService dialogService)
        {
            _iplotModelFactory = iplotModelFactory;
            _settingsService = settingsService;
            _exportService = exportService;
            _dialogService = dialogService;

            ExportCommand = new DelegateCommand(OpenExportDialog);

            ResetSettingParamsCommand = new DelegateCommand(ResetFunctionSettings);

            // loading parameters from App.Config
            LoadSettings();
            FunctionTypes = Enum.GetValues(typeof(FunctionType)).Cast<FunctionType>().ToList();
            // Command to update PlotModel
            UpdatePlotCommand = new DelegateCommand(UpdatePlot);          

            // Subscribe to PropertyChanged event of FunctionSettings
            FunctionSettings.PropertyChanged += (s, e) => UpdatePlot();
            UpdatePlot();
        }

        private void ResetFunctionSettings()
        {
            FunctionSettings = new FunctionSettings
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
            };

            RaisePropertyChanged(nameof(FunctionSettings));
            SetValue(FunctionSettings);

        }
        private void OpenExportDialog()
        {
            var parameters = new DialogParameters
        {
            { "SelectedPlotModel", _selectedPlotModel }
        };

            _dialogService.ShowDialog("ExportDialog", parameters, result =>
            {
                if (result.Result == ButtonResult.OK)
                {
                    // Retrieve parameters from the dialog result
                    var savePath = result.Parameters.GetValue<string>("SavePath");
                    var imageWidth = result.Parameters.GetValue<int>("ImageWidth");
                    var imageHeight = result.Parameters.GetValue<int>("ImageHeight");

                    // Handle the export result
                    HandleExportResult(savePath, imageWidth, imageHeight);
                }
                else if (result.Result == ButtonResult.Cancel)
                {
                    // Handle the cancel result if necessary
                }
            });
        }

        private void HandleExportResult(string savePath, int width, int height)
        {
            MessageBox.Show($"Export Details:\nPath: {savePath}\nWidth: {width}\nHeight: {height}");
            ExportSVGImage(width,height,savePath);
        }

        private double _amplitude;
        public double Amplitude
        {
            get => _amplitude;
            set
            {
                ValidateProperty(_amplitude, value, nameof(Amplitude));
                if (SetProperty(ref _amplitude, value))
                {
                    if (_functionSettings != null)
                    {
                        _functionSettings.Amplitude = value;
                    }    
                }               
            }
        }
        private double _frequency;
        public double Frequency
        {
            get => _frequency; 
            set 
            {
                ValidateProperty(_frequency, value, nameof(Frequency));
                if (SetProperty(ref _frequency, value))
                {
                    if (_functionSettings != null)
                    {
                        _functionSettings.Frequency = value;
                    }
                }
            }
        }

        private double _shift;
        public double Shift
        {
            get => _shift;
            set
            {
                ValidateProperty(_shift, value, nameof(Shift));
                if (SetProperty(ref _shift, value))
                {
                    if (_functionSettings != null)
                    {
                        _functionSettings.Shift = value;
                    }
                }
            }
        }

        private double _step;
        public double Step
        {
            get => _step;
            set
            {
                ValidateProperty(_step, value, nameof(Step));
                if (SetProperty(ref _step, value))
                {
                    if (_functionSettings != null)
                    {
                        _functionSettings.Step = value;
                    }
                }
            }
        }

        private double _yMax;
        public double YMax
        {
            get => _yMax;
            set
            {
                ValidateProperty(_yMax, value, nameof(YMax));
                if (SetProperty(ref _yMax, value))
                {
                    if (_functionSettings != null)
                    {
                        _functionSettings.YMax = value;
                    }
                }
            }
        }

        private double _yMin;
        public double YMin
        {
            get => _yMin;
            set
            {
                ValidateProperty(_yMin, value, nameof(YMin));
                if (SetProperty(ref _yMin, value))
                {
                    if (_functionSettings != null)
                    {
                        _functionSettings.YMin = value;
                    }
                }
            }
        }

        private double _xMin;
        public double XMin
        {
            get => _xMin;
            set
            {
                ValidateProperty(_xMin, value, nameof(XMin));
                if (SetProperty(ref _xMin, value))
                {
                    if (_functionSettings != null)
                    {
                        _functionSettings.XMin = value;
                    }
                }
            }
        }

        private double _xMax;
        public double XMax
        {
            get => _xMax;
            set
            {
                ValidateProperty(_xMax, value, nameof(XMax));
                if (SetProperty(ref _xMax, value))
                {
                    if (_functionSettings != null)
                    {
                        _functionSettings.XMax = value;
                    }
                }
            }
        }

        private double _phase;
        public double Phase
        {
            get => _phase;
            set
            {
                ValidateProperty(_phase, value, nameof(Phase));
                if (SetProperty(ref _phase, value))
                {
                    if (_functionSettings != null)
                    {
                        _functionSettings.Phase = value;
                    }
                }
            }
        }

        public FunctionSettings FunctionSettings
        {
            get => _functionSettings;
            set
            {
                if (SetProperty(ref _functionSettings, value))
                {
                    // Unsubscribe from the old settings PropertyChanged event
                    if (_functionSettings != null)
                    {
                        _functionSettings.PropertyChanged -= (s, e) => UpdatePlot();
                    }

                    _functionSettings = value;
                    _functionSettings.PropertyChanged += (s, e) => UpdatePlot();
                    UpdatePlot();
                }
            }
        }


        public PlotModel SelectedPlotModel
        {
            get => _selectedPlotModel;
            set => SetProperty(ref _selectedPlotModel, value);
        }

        public List<FunctionType> FunctionTypes { get; }

        public ICommand UpdatePlotCommand { get; }


        private void UpdatePlot()
        {
            if (!HasErrors)
            {
                SelectedPlotModel = _iplotModelFactory.CreatePlotModel(FunctionSettings);
                RaisePropertyChanged(nameof(FunctionSettings));
                SaveSettings();
            }              
        }

        private void SetValue(FunctionSettings functionSettings) 
        {
            Amplitude = functionSettings.Amplitude;
            Frequency = functionSettings.Frequency;
            Phase = functionSettings.Phase;
            Shift = functionSettings.Shift;
            Step = functionSettings.Step;
            YMax = functionSettings.YMax;
            YMin = functionSettings.YMin;
            XMin = functionSettings.XMin;
            XMax = functionSettings.XMax;
        }

        private void LoadSettings()
        {          
            FunctionSettings = _settingsService.LoadSettings();
            SetValue(FunctionSettings);
        }

        public void SaveSettings()
        {
            _settingsService.SaveSettings(FunctionSettings);
        }

        private void ExportSVGImage(int width, int height, string savePath)
        {
            _exportService.ExportToSvg(SelectedPlotModel, savePath,width, height);
        }

        public override string ValidateProperty<T>(T value, string propertyName)
        {
            switch (propertyName)
            {
                case nameof(Amplitude) when  value is double amplitudeValue && amplitudeValue <= 0:
                    return "Amplitude should not be Zero or smaller als zero.";
                case nameof(Amplitude) when value is double amplitudeValue && amplitudeValue > FunctionSettings.YMax:
                    return "Amplitude should not exceed YMax.";
                case nameof(Frequency) when value is double frequency && frequency ==0:
                    return "Frequency should not Zero.";
                case nameof(Shift) when value is double shift && shift > FunctionSettings.YMax:
                    return "Shift should not greater as YMax.";
                case nameof(Shift) when value is double shift && shift < FunctionSettings.YMin:
                    return "Shift should not small as YMin.";
                case nameof(Step) when value is double step && step <= 0.001:
                    return "Step should not be smaller als 0.001.";
                case nameof(YMax) when value is double ymax && ymax <= FunctionSettings.YMin:
                    return "YMax should be greater than YMin.";
                case nameof(YMin) when value is double ymin && ymin >= FunctionSettings.YMax:
                    return "YMin should not be greater than YMax.";
                case nameof(XMin) when value is double xmin && xmin >= FunctionSettings.XMax:
                    return "XMin should not be greater than XMax.";
                case nameof(XMax) when value is double xmax && xmax <= FunctionSettings.XMin:
                    return "XMin should not be greater than XMax.";
                default:
                    return null;
            }
        }
    }
}
