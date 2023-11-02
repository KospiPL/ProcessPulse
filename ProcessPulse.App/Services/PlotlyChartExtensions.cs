using Plotly.Blazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessPulse.App.Services
{
    public static class PlotlyChartExtensions
    {
        public static void Replot(this PlotlyChart chart, Plotly.Blazor.Layout newLayout, IList<ITrace> newData)
        {
            chart.Layout = newLayout;  
            chart.Data = newData;      
            chart.Update();            
        }
    }

}
