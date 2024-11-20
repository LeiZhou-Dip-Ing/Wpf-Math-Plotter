using OxyPlot;

namespace Test.Services.Export
{
    public interface IExportService
    {
        void ExportToSvg(PlotModel plotModel, string filePath, int width, int height);
        void ExportToPdf(PlotModel plotModel, string filePath, int width, int height);
        
    }
}
