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
        private ObservableDataSource<Point> dataSource_Y1_Origin = new ObservableDataSource<Point>();
        private ObservableDataSource<Point> dataSource_Y2_Shake = null;
        private ObservableDataSource<Point> dataSource_Y3_Average = null;
        private ObservableDataSource<Point> dataSource_Y4_Moving = null;
        private ObservableDataSource<Point> dataSource_Y5_IIR = null;
        private Queue<Point> dataQueue_Origin = new Queue<Point>();
        private Queue<Point> dataQueue_Shake = new Queue<Point>();
        private Queue<Point> dataQueue_Average = new Queue<Point>();
        private Queue<Point> dataQueue_Moving = new Queue<Point>();
        private Queue<Point> dataQueue_IIR = new Queue<Point>();
        DispatcherTimer refreshPlotterTimer = new DispatcherTimer();
        private double x1_origin = 0;
        private double x2_average = 0;
        private double x3_shake = 0;
        private double x4_moving = 0;
        private double x5_iir = 0;
        private bool isClosingFlag = false;
        LineGraph lineOrigin = new LineGraph();
        LineGraph lineAverage = new LineGraph();
        LineGraph lineShake = new LineGraph();
        LineGraph lineMoving = new LineGraph();
        LineGraph lineIIR = new LineGraph();
        //
        private bool plotterDisplayOrigin = true;
        private bool plotterDisplayShake = false;
        private bool plotterDisplayAverage = false;
        private bool plotterDisplayMoving = false;
        private bool plotterDisplayIIR = false;
        //
        public ChartWindow()
        {
            InitializeComponent();
            //
            checkBox_origin.IsChecked = true;
            checkBox_moving.IsChecked = false;
            checkBox_shake.IsChecked = false;
            checkBox_iir.IsChecked = false;
            //
            textBox.Text = PlotterDataNumber.ToString();
            //
            refreshPlotterTimer.Interval = TimeSpan.FromMilliseconds(2);
            refreshPlotterTimer.Tick += new EventHandler(RefreshPlotterEventHander);
            refreshPlotterTimer.Start();
        }

        private bool bNewDataComeFlag = false;
        private void RefreshPlotterEventHander(object sender, EventArgs e) {
            int iShakeData = 0;
            int iAverageData = 0;
            int iMovingData = 0;
            int iIIRData = 0;
            if (x1_origin== PlotterDataNumber) {
                //dataQueue_Origin.Dequeue();
                //dataQueue_Shake.Dequeue();
                //dataQueue_Average.Dequeue();
                //dataQueue_Moving.Dequeue();
                //dataQueue_IIR.Dequeue();
            }
            //IEnumerable<Point> numerator = new 
            if (bNewDataComeFlag) {
                bNewDataComeFlag = false;
                if (plotterDisplayOrigin && dataSource_Y1_Origin != null)
                {
                    dataSource_Y1_Origin.AppendMany(dataQueue_Origin);
                }
                //get anti-shake  
                if (plotterDisplayShake && dataSource_Y2_Shake != null)
                {
                    dataSource_Y2_Shake.AppendMany(dataQueue_Shake);
                }
                //iAverageData = AverageFilter(iShakeData, 4);
                if (plotterDisplayAverage && dataSource_Y3_Average != null)
                {
                    dataSource_Y3_Average.AppendMany(dataQueue_Average);
                }
                //iMovingData = MovingFilter(iAverageData, 8);
                if (plotterDisplayMoving && dataSource_Y4_Moving != null)
                {
                    dataSource_Y4_Moving.AppendMany(dataQueue_Moving);
                }
                //iIIRData = IIRFilter(iMovingData, 3, 3);

                if (plotterDisplayIIR && dataSource_Y5_IIR != null)
                {
                    dataSource_Y5_IIR.AppendMany(dataQueue_IIR);
                }

            }


        }


        private delegate void UpdateDataSourceEventHander(int i);
        public void AddData(int idata) {


            if (isClosingFlag == false)
            {
                this.Dispatcher.BeginInvoke(new UpdateDataSourceEventHander(updateDataSource), idata);
            }

        }
        private void updateDataSource(int idata) {
           
            x1_origin++;
            //if (x1_origin > PlotterDataNumber) {
            //    x1_origin = 0;
            //}
            if (!btn_pause_click) {
                bNewDataComeFlag = true;
                dataQueue_Origin.Enqueue(new Point(x1_origin, idata));
                dataQueue_Shake.Enqueue(new Point(x1_origin, idata+ 100));
                dataQueue_Average.Enqueue(new Point(x1_origin, idata+ 200));
                dataQueue_Moving.Enqueue(new Point(x1_origin, idata + 300));
                dataQueue_IIR.Enqueue(new Point(x1_origin, idata + 400));
 
            }//btn_pause_click


        }
        private int AntiShakeFilter(int idata) {

            return idata+100;
        }

        private int AverageFilter(int idata, int times) {


            return idata+200;
        }
        private int MovingFilter(int idata, int bufferLength) {

            return idata+300;
        }
        private int IIRFilter(int idata, int order, int threshold) {

            return idata+400;
        }



        
        private void updatePlotter(object sender, EventArgs e) {
            

        }

        private void chart_window_Loaded(object sender, RoutedEventArgs e)
        {

          //  lineOrigin = plotter.AddLineGraph(dataSource_Y1_Origin, Colors.Red, 2, "Origin");
            plotter.Viewport.FitToView();
            
            
        }

        private void chart_window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
         
            isClosingFlag = true;
        }

        private void chart_window_Closed(object sender, EventArgs e)
        {
            dataSource_Y1_Origin = null;
        }
        private bool btn_pause_click = false;
     
        private void btn_pause_Click(object sender, RoutedEventArgs e)
        {
            if (btn_pause_click)
            {
                btn_pause_click = false;
                btn_pause.Content = "pausing";
            }
            else {
                btn_pause_click = true;
                btn_pause.Content = "pause";
            }
                
        }

        private void btn_reset_Click(object sender, RoutedEventArgs e)
        {
            x1_origin = 0;
            dataSource_Y1_Origin = new ObservableDataSource<Point>();
            plotter.Children.Remove(lineOrigin);
            lineOrigin = plotter.AddLineGraph(dataSource_Y1_Origin, Colors.Red, 2, "Origin");
        }
        private int PlotterDataNumber = 1000;//默认1000
        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            PlotterDataNumber = Convert.ToInt32(textBox.Text.ToString());
               
        }


        private void checkBox_origin_Checked(object sender, RoutedEventArgs e)
        {
            plotter.Children.Remove(lineOrigin);
            //x1_origin = 0;
            dataSource_Y1_Origin = new ObservableDataSource<Point>();
            lineOrigin = plotter.AddLineGraph(dataSource_Y1_Origin, Colors.Red, 2, "Origin");
            plotterDisplayOrigin = true;
        }

        private void checkBox_origin_Unchecked(object sender, RoutedEventArgs e)
        {
            //x1_origin = 0;
            dataSource_Y1_Origin = new ObservableDataSource<Point>();
            plotter.Children.Remove(lineOrigin);
            plotterDisplayOrigin = false;
        }
        
        private void checkBox_shake_Checked(object sender, RoutedEventArgs e)
        {
           // if (dataSource_Y2_Shake == null)
            {
              dataSource_Y2_Shake = new ObservableDataSource<Point>();
              lineShake = plotter.AddLineGraph(dataSource_Y2_Shake, Colors.HotPink, 2, "Shake");
            }
         //  plotter.Children.Remove(lineShake);
            plotterDisplayShake = true;

        }

        private void checkBox_shake_Unchecked(object sender, RoutedEventArgs e)
        {
           // x3_shake = 0;
            dataSource_Y2_Shake = new ObservableDataSource<Point>();
            plotter.Children.Remove(lineShake);
            plotterDisplayShake = false;
        }

        private void checkBox_average_Checked(object sender, RoutedEventArgs e)
        {
           // x2_average = 0;
           // if (dataSource_Y3_Average ==null) 
            {
                dataSource_Y3_Average = new ObservableDataSource<Point>();
                lineAverage = plotter.AddLineGraph(dataSource_Y3_Average, Colors.Aqua, 2, "Average");
            }
           // plotter.Children.Remove(lineAverage);
            plotterDisplayAverage = true;
        }

        private void checkBox_average_Unchecked(object sender, RoutedEventArgs e)
        {
           // x2_average = 0;
            plotter.Children.Remove(lineAverage);
            plotterDisplayAverage = false;
        }

        private void checkBox_moving_Checked(object sender, RoutedEventArgs e)
        {
           //   x4_moving = 0;
            //if (dataSource_Y4_Moving ==null)
            {
                dataSource_Y4_Moving = new ObservableDataSource<Point>();
                lineMoving = plotter.AddLineGraph(dataSource_Y4_Moving, Colors.Brown, 2, "Moving");
            }
            //plotter.Children.Remove(lineMoving);
            plotterDisplayMoving = true;    
        }

        private void checkBox_moving_Unchecked(object sender, RoutedEventArgs e)
        {
           // x4_moving = 0;
            plotter.Children.Remove(lineMoving);
            plotterDisplayMoving = false;
        }

        private void checkBox_iir_Checked(object sender, RoutedEventArgs e)
        {
            x5_iir = 0;
            //if (dataSource_Y5_IIR == null)
            {
                dataSource_Y5_IIR = new ObservableDataSource<Point>();
                lineIIR = plotter.AddLineGraph(dataSource_Y5_IIR, Colors.Cyan, 2, "IIR");
            }
            //plotter.Children.Remove(lineIIR);
            plotterDisplayIIR = true;

        }

        private void checkBox_iir_Unchecked(object sender, RoutedEventArgs e)
        {
            x5_iir= 0;
            plotter.Children.Remove(lineIIR);
            plotterDisplayIIR = false;

        }

    }
}
