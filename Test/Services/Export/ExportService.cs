using OxyPlot;
using System;
using System.IO;

namespace Test.Services.Export
{
    public class ExportService : IExportService
    {
        public void ExportToPdf(PlotModel plotModel, string filePath, int width, int height)
        {
            throw new NotImplementedException();
        }


        // Export to Vector Format (Scalable Vector Graphics)
        public void ExportToSvg(PlotModel plotModel, string filePath, int width, int height)
        {
            var exporter = new SvgExporter { Width = width, Height = height };
            using (var stream = File.Create(filePath))
            {
                exporter.Export(plotModel, stream);
            }
        }
    }
}
