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
using System.Windows.Shapes;
using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using Microsoft.Research.DynamicDataDisplay.PointMarkers;
using Microsoft.Research.DynamicDataDisplay.Charts.Navigation;
using System.Windows.Threading;

namespace ChipseaUartHelper
{
    /// <summary>
    /// Interaction logic for ChartWindow.xaml
    /// </summary>
    public partial class ChartWindow : Window
    {
        private ObservableDataSource<Point> dataSource_Y1 = new ObservableDataSource<Point>();
        private double x1 = 0;
        private bool isClosingFlag = false; 
        public ChartWindow()
        {
            InitializeComponent();
        }
        private delegate void UpdateDataSourceEventHander(int i);
        public void AddData(int idata) {


            if (isClosingFlag == false)
            {
                this.Dispatcher.Invoke(new UpdateDataSourceEventHander(updateDataSource), idata);
            }
            
        }
        private void updateDataSource(int idata) {
          
            if (dataSource_Y1 != null) { 
                dataSource_Y1.AppendAsync(base.Dispatcher, new Point(x1++, idata));

            }
            
        }
        
        private void updatePlotter(object sender, EventArgs e) {
            

        }

        private void chart_window_Loaded(object sender, RoutedEventArgs e)
        {
           
            plotter.AddLineGraph(dataSource_Y1, Colors.Red, 2, "Origin");
            plotter.Viewport.FitToView();
            
        }

        private void chart_window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
         
            isClosingFlag = true;
        }

        private void chart_window_Closed(object sender, EventArgs e)
        {
            dataSource_Y1 = null;
        }
    }
}
