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
        private static long lCounterX = 0;
        List<long> dataX = new List<long>();
        List<int> dataListOriginY = new List<int>();
        CompositeDataSource compositeDataSourceOriginY=null;
        EnumerableDataSource<long> enumerableDataSourceX =null;
        EnumerableDataSource<int> enumerableDataSourceOriginY = null;
        //private DispatcherTimer timer = new DispatcherTimer();
        private bool iClosingFlag = false; 
        public ChartWindow()
        {
            InitializeComponent();
        }
        private delegate void UpdateDataSourceEventHander(int i);
        public void AddData(int idata) {


            if (iClosingFlag == false)
            {
                this.Dispatcher.Invoke(new UpdateDataSourceEventHander(updateDataSource), idata);
            }
            
        }
        private int tmpData;
        private void updateDataSource(int idata) {
            if (dataListOriginY != null && dataX != null && enumerableDataSourceX != null && enumerableDataSourceOriginY != null) {

                //tmpData = idata;
                dataListOriginY.Add(idata);
                //DateTime tempDateTime = new DateTime();
                lCounterX++;
                //tempDateTime = DateTime.Now;
                dataX.Add(lCounterX);
                enumerableDataSourceX.RaiseDataChanged();
                enumerableDataSourceOriginY.RaiseDataChanged();


            }
            
        }
        
        private void updatePlotter(object sender, EventArgs e) {
            //dataListOriginY.Add(tmpData);
            ////DateTime tempDateTime = new DateTime();
            //lCounterX++;
            ////tempDateTime = DateTime.Now;
            //dateTimeX.Add(lCounterX);
            //enumerableDataSourceX.RaiseDataChanged();
            //enumerableDataSourceOriginY.RaiseDataChanged();

        }

        private void chart_window_Loaded(object sender, RoutedEventArgs e)
        {
            //DateTime tempDateTime = new DateTime();
            //tempDateTime = DateTime.Now;
            dataX.Add(0);
            dataListOriginY.Add(0);
            enumerableDataSourceX = new EnumerableDataSource<long>(dataX);
            enumerableDataSourceX.SetXMapping(x => Convert.ToDouble(x));
            enumerableDataSourceOriginY = new EnumerableDataSource<int>(dataListOriginY);
            enumerableDataSourceOriginY.SetYMapping(y => y);
            //con
            compositeDataSourceOriginY = new CompositeDataSource(enumerableDataSourceX, enumerableDataSourceOriginY);
            plotter.AddLineGraph(compositeDataSourceOriginY, Colors.Red, 1,"Origin");
            plotter.FitToView();

            //timer.Interval = TimeSpan.FromMilliseconds(2);
            //timer.Tick += new EventHandler(updatePlotter);
            //timer.IsEnabled = true;
        }

        private void chart_window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
         
            iClosingFlag = true;
        }

        private void chart_window_Closed(object sender, EventArgs e)
        {
            dataX.Clear();
            dataListOriginY.Clear();
            compositeDataSourceOriginY = null;
            enumerableDataSourceX = null;
            enumerableDataSourceOriginY = null;
        }
    }
}
