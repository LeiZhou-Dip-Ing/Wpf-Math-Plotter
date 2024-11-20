using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Windows;
using Test.Commen.Enum;

namespace Test.Functions
{
    public class PlotModelFactory : IPlotModelFactory
    {
        public PlotModel CreatePlotModel(FunctionSettings settings)
        {
            try
            {
                var model = new PlotModel { Title = GetTitle(settings) };
                var function = CreateFunction(settings);
                model.Series.Add(CreateFunctionSeries(function, settings));

                model.Axes.Add(CreateAxis(AxisPosition.Bottom, settings.XMin, settings.XMax,"X Axis"));
                model.Axes.Add(CreateAxis(AxisPosition.Left, settings.YMin, settings.YMax, "Y Axis"));

                return model;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
                throw;
            }           
        }

        private static string GetTitle(FunctionSettings settings)
        {
            // If Amplitude is 1, omit "1 *". If Amplitude is -1, display "- " (without 1)
            string amplitudePart = settings.Amplitude == 1
                ? ""
                : (settings.Amplitude == -1 ? "- " : $"{settings.Amplitude} * ");

            // If Frequency is 1, omit "1x". If Frequency is -1, display "-x". Otherwise, display "Frequency * x"
            string frequencyPart = settings.Frequency == 1
                ? "x"
                : (settings.Frequency == -1 ? "-x" : $"{settings.Frequency}x");

            // If Phase is 0, omit. If positive, show "+ phase". If negative, show "- phase"
            string phasePart = settings.Phase == 0
                ? ""
                : (settings.Phase > 0 ? $"+ {settings.Phase}" : $"- {Math.Abs(settings.Phase)}");

            // If Shift is 0, omit. If positive, show "+ shift". If negative, show "- shift"
            string shiftPart = settings.Shift == 0
                ? ""
                : (settings.Shift > 0 ? $"+ {settings.Shift}" : $"- {Math.Abs(settings.Shift)}");

            // Build the function expression based on the function type
            string expression;

            switch (settings.FunctionType)
            {
                case FunctionType.Sine:
                    expression = $"{amplitudePart}sin({frequencyPart} {phasePart}) {shiftPart}";
                    break;

                case FunctionType.Cosine:
                    expression = $"{amplitudePart}cos({frequencyPart} {phasePart}) {shiftPart}";
                    break;

                case FunctionType.Sinc:
                    expression = settings.Amplitude == 0
                        ? "0"
                        : $"{amplitudePart}sinc({frequencyPart} {phasePart}) {shiftPart}";
                    break;

                default:
                    throw new ArgumentException("Invalid function type");
            }

            return $"y = {expression.Trim()}";
        }

        private static FunctionSeries CreateFunctionSeries(Func<double, double> function, FunctionSettings settings)
        {
            return new FunctionSeries(function, settings.XMin, settings.XMax, settings.Step);
        }

        private static LinearAxis CreateAxis(AxisPosition position, double min, double max,string title)
        {
            return new LinearAxis
            {
                Position = position,
                Minimum = min,
                Maximum = max,
                Title = title,
                MaximumPadding = 0.1,
                MinimumPadding = 0.1
            };
        }

        private static Func<double, double> CreateFunction(FunctionSettings settings)
        {
            Func<double, double> function;

            switch (settings.FunctionType)
            {
                case FunctionType.Sine:
                    function = x => settings.Amplitude * Math.Sin(settings.Frequency * x + settings.Phase) + settings.Shift;
                    break;

                case FunctionType.Cosine:
                    function = x => settings.Amplitude * Math.Cos(settings.Frequency * x + settings.Phase) + settings.Shift;
                    break;

                case FunctionType.Sinc:
                    function = x => x == 0 ? settings.Amplitude : settings.Amplitude * Math.Sin(settings.Frequency * x + settings.Phase) / x  + settings.Shift;
                    break;

                default:
                    throw new ArgumentException("Invalid function type");
            }

            return function;
        }
    }
}
