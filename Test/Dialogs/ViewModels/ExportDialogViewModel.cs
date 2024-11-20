using Microsoft.Win32;
using OxyPlot;
using OxyPlot.Wpf;
using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Test.Commen.ViewModels;

namespace Test.Dialogs.ViewModels
{
    public class ExportDialogViewModel : ValidationViewModelBase, IDialogAware
    {
        private PlotModel _selectedPlotModel;

        private ImageSource _svgPreviewImage;
        public string Title => "Export SVG Image";

        public event Action<IDialogResult> RequestClose;

        public DelegateCommand ConfirmCommand { get; }
        public DelegateCommand CancelCommand { get; }
        public DelegateCommand BrowseCommand { get; }

        private int _imageWidth;

        public int ImageWidth
        {
            get => _imageWidth;
            set
            {
                ValidateProperty(_imageWidth, value, nameof(ImageWidth));
                SetProperty(ref _imageWidth, value);
            }
        }

        private int _imageHeight;

        public int ImageHeight
        {
            get => _imageHeight;
            set
            {
                ValidateProperty(_imageHeight, value, nameof(ImageHeight));
                SetProperty(ref _imageHeight, value);
            }
        }

        public string SavePath { get; set; }

        public ImageSource SvgPreviewImage
        {
            get => _svgPreviewImage;
            set => SetProperty(ref _svgPreviewImage, value);
        }

        public ExportDialogViewModel()
        {
            ImageHeight = 200; ImageWidth = 300;
            ConfirmCommand = new DelegateCommand(OnConfirm);
            CancelCommand = new DelegateCommand(OnCancel);
            BrowseCommand = new DelegateCommand(OnBrowse);
        }

        // Handles the browsing for a file to save the plot
        private void OnBrowse()
        {
            try
            {
                var dialog = new SaveFileDialog
                {
                    Filter = "SVG Files (*.svg)|*.svg|All Files (*.*)|*.*",
                    DefaultExt = ".svg"
                };

                if (dialog.ShowDialog() == true)
                {
                    SavePath = dialog.FileName;
                    RaisePropertyChanged(nameof(SavePath));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while browsing for a file: {ex.Message}");
            }
        }

        private void OnCancel()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));
        }

        // Handles the confirm action, validates parameters and closes the dialog with results
        private void OnConfirm()
        {
            if (string.IsNullOrEmpty(SavePath) || ImageWidth <= 0 || ImageHeight <= 0)
            {
                // Show a message to the user or handle invalid parameters
                MessageBox.Show("Please provide a valid file path and dimensions.");
                return;
            }

            var parameters = new DialogParameters
        {
            { "SavePath", SavePath },
            { "ImageWidth", ImageWidth },
            { "ImageHeight", ImageHeight }
        };
            RequestClose?.Invoke(new DialogResult(ButtonResult.OK, parameters));
        }

        // Determines if the dialog can be closed (always true in this case)
        public bool CanCloseDialog() => true;

        // Handles actions to be taken when the dialog is closed
        public void OnDialogClosed() { }

        // Initializes the ViewModel with parameters from the dialog
        public void OnDialogOpened(IDialogParameters parameters)
        {
            if (parameters.ContainsKey("SelectedPlotModel"))
            {
                SetPlotModel(parameters.GetValue<PlotModel>("SelectedPlotModel"));
                UpdateSvgPreview();
            }
        }

        // Receives a PlotModel from the MainWindowViewModel
        public void SetPlotModel(PlotModel plotModel)
        {
            _selectedPlotModel = plotModel;
            UpdateSvgPreview();  // Automatically generates a preview when a PlotModel is received
        }

        // Updates the SVG preview image based on the current PlotModel
        private void UpdateSvgPreview()
        {
            if (_selectedPlotModel == null) return;

            try
            {
                using (var stream = new MemoryStream())
                {
                    var exporter = new PngExporter { Width = ImageWidth, Height = ImageHeight };
                    exporter.Export(_selectedPlotModel, stream);
                    stream.Seek(0, SeekOrigin.Begin);

                    SvgPreviewImage = ConvertSvgStreamToImageSource(stream);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while generating the SVG preview: {ex.Message}");
            }
        }

        // Converts an SVG stream to an ImageSource for display
        private ImageSource ConvertSvgStreamToImageSource(Stream svgStream)
        {
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = svgStream;
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.EndInit();
            bitmapImage.Freeze();  // Prevent cross-threading issues

            return bitmapImage;
        }
        public override string ValidateProperty<T>(T value, string propertyName)
        {
            switch (propertyName)
            {
                case nameof(ImageWidth) when value is int imagWidtg && imagWidtg <= 0:
                    return "ImageWidth should not be Zero or smaller als zero.";
                case nameof(ImageHeight) when value is int imagHeight && imagHeight <= 0:
                    return "ImageHeight should not be Zero or smaller als zero.";
                default:
                    return null;
            }
        }
    }
}
