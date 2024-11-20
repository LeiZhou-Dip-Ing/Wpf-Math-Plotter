using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Commen.Enum;

namespace Test.Functions
{
   public interface IPlotModelFactory
    {
        PlotModel CreatePlotModel(FunctionSettings settings);
    }
}
