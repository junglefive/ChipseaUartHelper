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
using System.IO;

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
        private FileStream fs = null;
        StreamWriter writer = null;
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
            //textBox.Text = PlotterDataNumber.ToString();
            //
            refreshPlotterTimer.Interval = TimeSpan.FromMilliseconds(10);
            refreshPlotterTimer.Tick += new EventHandler(RefreshPlotterEventHander);
            refreshPlotterTimer.Start();
            //初始化组件值
            initiateDefaultValue();
            //初始化文件，保存表数据
        }
        private void initiateDefaultValue() {
            iOriginRightShiftBits = 0;
            iShakeCount = 10;
            iShakeThreshold = 200;
            iAverageTimes = 4;
            iMovingLength = 8;
            iIIROrder = 6;
            iIIRThreshold = 50;
            textBox_origin.Text = iOriginRightShiftBits.ToString();
            textBox_shake_count.Text = iShakeCount.ToString();
            textBox_shake_threshold.Text = iShakeThreshold.ToString();
            textBox_moving_length.Text = iMovingLength.ToString();
            textBox_average_times.Text =  iAverageTimes.ToString();
            textBox_moving_length.Text =  iMovingLength.ToString();
            textBox_iir_order.Text = iIIROrder.ToString();
            textBox_iir_threshold.Text = iIIRThreshold.ToString();
            //
            if (btnChangeCount == MAXMIN_ORIGIN) { appendTextToTextBoxLog("ORIGIN"); }
        }

        private bool bNewDataComeFlag = false;
        private void RefreshPlotterEventHander(object sender, EventArgs e) {
         
            if (bNewDataComeFlag && bReseting == false) {
                bNewDataComeFlag = false;
                //update text
                textBlock_max.Text   = ""+iMax;
                textBlock_min.Text   = ""+iMin;
                textBlock_delta.Text = ""+(iMax - iMin);
                if (plotterDisplayOrigin && dataSource_Y1_Origin != null)
                {
                //dataSource_Y1_Origin.AppendMany(dataQueue_Origin);
                    dataSource_Y1_Origin.AppendAsync(Dispatcher, dataPoint_Origin);
                }
                //get anti-shake  
                if (plotterDisplayShake && dataSource_Y2_Shake != null)
                {
                    dataSource_Y2_Shake.AppendAsync(Dispatcher, dataPoint_Shake);
                }
                //iAverageData = AverageFilter(iShakeData, 4);
                if (plotterDisplayAverage && dataSource_Y3_Average != null)
                {
                    dataSource_Y3_Average.AppendAsync(Dispatcher, dataPoint_Average);
                }
                //iMovingData = MovingFilter(iAverageData, 8);
                if (plotterDisplayMoving && dataSource_Y4_Moving != null)
                {
                    dataSource_Y4_Moving.AppendAsync(Dispatcher, dataPoint_Moving);
                }
                //iIIRData = IIRFilter(iMovingData, 3, 3);

                if (plotterDisplayIIR && dataSource_Y5_IIR != null)
                {
                    dataSource_Y5_IIR.AppendAsync(Dispatcher, dataPoint_IIR);
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
        Point dataPoint_Origin = new Point();
        Point dataPoint_Shake = new Point();
        Point dataPoint_Average = new Point();
        Point dataPoint_Moving = new Point();
        Point dataPoint_IIR = new Point();
        private int iMax = 0;
        private int iMin = 16777215;
        private void updateDataSource(int idata) {
            
            x1_origin++;
            idata = OriginFilter(idata, iOriginRightShiftBits);
            
            //if (x1_origin > PlotterDataNumber) {
            //    x1_origin = 0;
            //}
            if (btnChangeCount == MAXMIN_ORIGIN) { updateMaxMin(idata); }
            if (btnChangeCount == MAXMIN_SHAKE) { updateMaxMin(AntiShakeFilter(idata, iShakeCount, iShakeThreshold)); }
            if (btnChangeCount == MAXMIN_AVERAGE) { updateMaxMin(AverageFilter(idata, iAverageTimes)); }
            if (btnChangeCount == MAXMIN_MOVING) { updateMaxMin(MovingFilter(idata, iMovingLength)); }
            if (btnChangeCount == MAXMIN_IIR) { updateMaxMin(IIRFilter(idata, iIIROrder, iIIRThreshold)); }

            if (!btn_pause_click) {

                bNewDataComeFlag = true;
                int iAntiShake = AntiShakeFilter(idata, iShakeCount, iShakeThreshold);
                int iAverage = AverageFilter(idata, iAverageTimes);
                int iMoving = MovingFilter(idata, iMovingLength);
                int iIIR = IIRFilter(idata, iIIROrder, iIIRThreshold);
                //
                dataPoint_Origin = (new Point(x1_origin, idata));
                dataPoint_Shake = (new Point(x1_origin, iAntiShake));
                dataPoint_Average = (new Point(x1_origin, iAverage));
                dataPoint_Moving = (new Point(x1_origin, iMoving));
                dataPoint_IIR = (new Point(x1_origin, iIIR));
                //更新数据到文档
                if (writer != null && isClosingFlag == false)
                {
                    //writer.Flush();

                    writer.WriteLine(idata +",  "+iAntiShake+",  "+iAverage + ",  "+iMoving+",  "+iIIR);
                    //writer.Flush();
                }
                else if (writer == null && isClosingFlag == false)
                {
                    //string.Format("{0:yyyy-MM-dd }",System.DateTime.Now)
                    
                    fs = new FileStream(".\\" + "ChartDataAutoSave-" + string.Format("{0:yyyy-MM-dd }", System.DateTime.Now)+ ".txt", FileMode.Create);
                    writer = new StreamWriter(fs, Encoding.UTF8); //得到文件写对象
                    writer.WriteLine("Origin,   Shake,    Average,    Moving,     IIR");
                }

            }//btn_pause_click


        }
        private void updateMaxMin(int idata) {

            if (idata > iMax) { iMax = idata; };
            if (idata < iMin) { iMin = idata; };

        }



        private int OriginFilter(int idata, int rightshift) {

            return idata >> rightshift;
        }


        private int iShakeCounter = 0;
        private int iShakeLastSample = 0;
        private int AntiShakeFilter(int idata, int counter, int threshold) {

            if (Math.Abs(idata - iShakeLastSample) > threshold)
            {
                iShakeCounter++;
                if (iShakeCounter < counter)
                {
                    return iShakeLastSample;
                }
                else
                {
                    iShakeLastSample = idata;
                    return iShakeLastSample;
                }

            }
            else {

                iShakeCounter = 0;
                return idata;
            }


           
        }
        private int iAverageCounter = 0;
        private int iAverageSum = 0;
        private int iAverageLastValue = 0;
        private int AverageFilter(int idata, int times) {
            int iTmp = 0;
            iAverageCounter++;
            iAverageSum += idata;
            if (iAverageCounter == times)
            {
                iTmp =  iAverageSum / times;
                iAverageCounter = 0;
                iAverageSum = 0;
                iAverageLastValue = iTmp;
            }
            else {

                iTmp =  iAverageLastValue;
            }
            
            return iTmp;
        }
        Queue<int> iMovingQueue = new Queue<int>();
        private int iMovingLastValue = 0;
        private int MovingFilter(int idata, int bufferLength) {
            int sum = 0;
            int getData = 0;
            iMovingQueue.Enqueue(idata);
            if (iMovingQueue.Count == bufferLength + 1)
            {
                iMovingQueue.Dequeue();
                foreach (int i in iMovingQueue)
                {
                    sum += i;
                }
                getData = sum / bufferLength;
                iMovingLastValue = getData;
            }
            else {
                getData = idata;
            }

            return getData;
        }
        private  int iLastValue = 0;
        private  int iLastSample = 0;
        private  int iCurValue = 0;
        private  int IIRFilter(int idata, int order, int threshold) {
            int iTmp = 0;

            iTmp = (int)((idata + iLastSample + iLastValue*Math.Pow(2, order) - 2 * iLastValue)/(Math.Pow(2, order)));
            if (Math.Abs(iTmp - iLastValue) > threshold)
            {
                iCurValue = idata;
            }
            else {
                iCurValue = iTmp;
            }

            iLastValue = iCurValue;
            iLastSample = idata;
            return iCurValue;
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
            //writer.Close();
            // writer = null;
            if (writer != null) {
                writer.Close();
                writer = null;
            }
            OnChartWindowClosing();
        }

        private void chart_window_Closed(object sender, EventArgs e)
        {
            dataSource_Y1_Origin = null;
        }
        private bool btn_pause_click = false;
     
        private void btn_pause_Click(object sender, RoutedEventArgs e)
        {
            if (!btn_pause_click)
            {
                btn_pause_click = true;
                btn_pause.Content = "pausing";
            }
            else {
                btn_pause_click = false;
                btn_pause.Content = "pause";
            }
                
        }

  
        //private int PlotterDataNumber = 1000;//默认1000
        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
          //  PlotterDataNumber = Convert.ToInt32(textBox.Text.ToString());
               
        }


        private void checkBox_origin_Checked(object sender, RoutedEventArgs e)
        {
            plotter.Children.Remove(lineOrigin);
            //x1_origin = 0;
            dataSource_Y1_Origin = new ObservableDataSource<Point>();
            lineOrigin = plotter.AddLineGraph(dataSource_Y1_Origin, Colors.Gray, 2, "Origin");
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
                lineAverage = plotter.AddLineGraph(dataSource_Y3_Average, Colors.LimeGreen, 2, "Average");
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
            //x5_iir = 0;
            //if (dataSource_Y5_IIR == null)
            {
                dataSource_Y5_IIR = new ObservableDataSource<Point>();
                lineIIR = plotter.AddLineGraph(dataSource_Y5_IIR, Colors.DodgerBlue, 2, "IIR");
            }
            //plotter.Children.Remove(lineIIR);
            plotterDisplayIIR = true;

        }

        private void checkBox_iir_Unchecked(object sender, RoutedEventArgs e)
        {
            //x5_iir= 0;
            plotter.Children.Remove(lineIIR);
            plotterDisplayIIR = false;

        }
        private bool bReseting = false;
        private void btn_reset_Click(object sender, RoutedEventArgs e)
        {
            bReseting = true;
            x1_origin = 0;
            if(writer!=null){
                writer.Close();
            }
           
            writer = null;
            
            if (plotterDisplayOrigin == true) {
                plotter.Children.Remove(lineOrigin);
                dataSource_Y1_Origin = new ObservableDataSource<Point>();
                lineOrigin = plotter.AddLineGraph(dataSource_Y1_Origin, Colors.Gray, 2, "Origin");
            }

            //plotterDisplayOrigin = true;
            //plotter.Children.Remove(lineShake);
            if (plotterDisplayShake == true) {
                plotter.Children.Remove(lineShake);
                dataSource_Y2_Shake = new ObservableDataSource<Point>();
                lineShake = plotter.AddLineGraph(dataSource_Y2_Shake, Colors.HotPink, 2, "Shake");
            }

            //plotterDisplayShake = true;
            //
            if (plotterDisplayAverage == true) {
                plotter.Children.Remove(lineAverage);
                dataSource_Y3_Average = new ObservableDataSource<Point>();
                lineAverage = plotter.AddLineGraph(dataSource_Y3_Average, Colors.LimeGreen, 2, "Average");
            }


            //plotterDisplayAverage = true;
            //
            if (plotterDisplayMoving == true) {
                plotter.Children.Remove(lineMoving);
                dataSource_Y4_Moving = new ObservableDataSource<Point>();
                lineMoving = plotter.AddLineGraph(dataSource_Y4_Moving, Colors.Brown, 2, "Moving");
            }


            //plotterDisplayMoving = false;
            //if (dataSource_Y5_IIR == null)
            if (plotterDisplayIIR == true) {
                plotter.Children.Remove(lineIIR);
                dataSource_Y5_IIR = new ObservableDataSource<Point>();
                lineIIR = plotter.AddLineGraph(dataSource_Y5_IIR, Colors.DodgerBlue, 2, "IIR");
            }
           
            //plotterDisplayIIR = false;
            iMax = 0;
            iMin = 16777215;
            textBlock_max.Text = "";
            textBlock_min.Text = "";
            textBlock_delta.Text = "";
            bReseting = false;
        }

        private int iOriginRightShiftBits = 0;
        private void textBox_origin_DataContextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {

            iOriginRightShiftBits = Convert.ToInt32(textBox_origin.Text);
            }
            catch { }
        }
        private int iShakeCount = 0;
        private void textBox_shake_count_DataContextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
            iShakeCount = Convert.ToInt32(textBox_shake_count.Text);

            }
            catch { }
        }
        private int iShakeThreshold = 0;
        private void textBox_shake_threshold_DataContextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {

            iShakeThreshold = Convert.ToInt32(textBox_shake_threshold.Text);
            }
            catch { }
        }
        private int iAverageTimes = 0;
        private void textBox_average_times_DataContextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
            iAverageTimes = Convert.ToInt32(textBox_average_times.Text);

            }
            catch { }
        }
        private int iMovingLength = 0;
        private void textBox_moving_length_DataContextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {

            iMovingLength = Convert.ToInt32(textBox_moving_length.Text);
            }
            catch { }
        }
        private int iIIROrder = 0;
        private void textBox_iir_order_DataContextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {

            iIIROrder = Convert.ToInt32(textBox_iir_order.Text);
            }
            catch { }
        }
        private int iIIRThreshold = 0;
        private void textBox_iir_threshold_DataContextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {

            iIIRThreshold = Convert.ToInt32(textBox_iir_threshold);
            }
            catch { }
        }
        private int btnChangeCount = 0;
        private const int MAXMIN_ORIGIN = 0;
        private const int MAXMIN_SHAKE = 1;
        private const int MAXMIN_AVERAGE = 2;
        private const int MAXMIN_MOVING = 3;
        private const int MAXMIN_IIR = 4;

        private void btn_change_Click(object sender, RoutedEventArgs e)
        {
            btnChangeCount++;
            if (btnChangeCount == 5) { btnChangeCount = 0; }
            iMax = 0;
            iMin = 16777215;
            textBlock_max.Text = "";
            textBlock_min.Text = "";
            textBlock_delta.Text = "";
            //
            if (btnChangeCount == MAXMIN_ORIGIN) { appendTextToTextBoxLog("ORIGIN"); }
            if (btnChangeCount == MAXMIN_SHAKE) { appendTextToTextBoxLog("SHAKE"); }
            if (btnChangeCount == MAXMIN_AVERAGE) { appendTextToTextBoxLog("AVERAGE"); }
            if (btnChangeCount == MAXMIN_MOVING) { appendTextToTextBoxLog("MOVING"); }
            if (btnChangeCount == MAXMIN_IIR) { appendTextToTextBoxLog("IIR"); }

        }
        private delegate void appendTextEventHander(string msg);

        private void appendTextToTextBoxLog(string msg) {
            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new appendTextEventHander(appendTextHander), msg);
        }
        private void appendTextHander(string msg) {

            textBlock_log.Text = msg;

        }

        public delegate void ChartWindowClosingHander(object sender, EventArgs e);
        public event ChartWindowClosingHander chartWindowClosing;
        private void OnChartWindowClosing() {

            if (chartWindowClosing != null) {
                chartWindowClosing(this, new EventArgs());
            }
        }
    }
}
