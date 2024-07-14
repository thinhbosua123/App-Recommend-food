using LiveCharts.Wpf;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.TextFormatting;
using System.Security.RightsManagement;
using LiveCharts.Defaults;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Project.UserControlXAML
{
    /// <summary>
    /// Interaction logic for PieChartMeal.xaml
    /// </summary>
    public partial class PieChartMeal : UserControl 
    {

        public int breakfast { get; set; }

        public int lunch { get; set; }
        public int dinner { get; set; }
        public PieChartMeal()
        {
            InitializeComponent();
            this.DataContext = this;
            PointLabel = chartPoint =>
                string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);
        }
        public Func<ChartPoint, string> PointLabel { get; set; }

        private void Chart_OnDataClick(object sender, ChartPoint chartpoint)
        {
            var chart = (LiveCharts.Wpf.PieChart)chartpoint.ChartView;

            //clear selected slice.
            foreach (PieSeries series in chart.Series)
                series.PushOut = 0;

            var selectedSeries = (PieSeries)chartpoint.SeriesView;
            selectedSeries.PushOut = 8;
        }

        //private void UserControl_Loaded(object sender, RoutedEventArgs e)
        //{
        //    seriBreakfast.Values = new ChartValues<ObservableValue> { new ObservableValue(breakfast) };
        //    seriLunch.Values = new ChartValues<ObservableValue> { new ObservableValue(lunch) };
        //    seriDinner.Values = new ChartValues<ObservableValue> { new ObservableValue(dinner) };
        //}

        internal new void Loaded()
        {
            seriBreakfast.Values = new ChartValues<ObservableValue> { new ObservableValue(breakfast) };
            seriLunch.Values = new ChartValues<ObservableValue> { new ObservableValue(lunch) };
            seriDinner.Values = new ChartValues<ObservableValue> { new ObservableValue(dinner) };
        }
    }
}
