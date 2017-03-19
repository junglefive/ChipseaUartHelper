using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;
using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using Microsoft.Research.DynamicDataDisplay.PointMarkers;
using Microsoft.Research.DynamicDataDisplay.Charts.Navigation;

namespace ChipseaUartHelper
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        //串口相关
        public SerialPort ComPort = new SerialPort();//声明一个串口      
        private string[] ports;                       //可用串口数组
        //private bool recStaus = true;                 //接收状态字
        private bool ComPortIsOpen = false;           //COM口开启状态字，在打开/关闭串口中使用，这里没有使用自带的ComPort.IsOpen，因为在串口突然丢失的时候，ComPort.IsOpen会自动false，逻辑混乱
        private bool comIsClosing  =  false;
        DispatcherTimer autoSendTick = new DispatcherTimer();//定时发送
        //private static bool Sending = false;//正在发送数据状态字
        //private static Thread _ComSend;//发送数据线程
        //Queue recQueue = new Queue();//接收数据过程中，接收数据线程与数据处理线程直接传递的队列，先进先出
                                                      //Window
                  
        ChartWindow chartWindow = null;
        TextWindow textWindow = null;
        Thread threadChartWindow = null;
        Thread threadTextWindow = null;
        Thread recData = null;
        Thread threadSerialPortDataRecieved = null;
        //缓存数据
        Queue<int> getDataQueueSave = new Queue<int>();
        public MainWindow()
        {
            InitializeComponent();
            
            threadChartWindow = new Thread(new ThreadStart(startChartWindow));
            threadChartWindow.SetApartmentState(ApartmentState.STA);
            threadChartWindow.IsBackground = true;
            threadChartWindow.Start();

            threadTextWindow = new Thread(new ThreadStart(startTextWindow));
            threadTextWindow.SetApartmentState(ApartmentState.STA);
            threadTextWindow.IsBackground = true;
            threadTextWindow.Start();


            threadSerialPortDataRecieved = new Thread(new ThreadStart(ComPort_DataReceived));
            threadSerialPortDataRecieved.SetApartmentState(ApartmentState.MTA);
            threadSerialPortDataRecieved.IsBackground = true;
          
            threadSerialPortDataRecieved.Start();

            recData = new Thread(new ThreadStart(DecodeBytes));
            recData.SetApartmentState(ApartmentState.MTA);
            recData.IsBackground = true;
            recData.Start();
        }
        /**
        *function:初始化串口部分
        *by:junglefive
        **/
        Dictionary<string, int> dicParity = new Dictionary<string, int>();
        private void User_initiate_serialPort() {
            //serialPortNames
            ports = SerialPort.GetPortNames(); //获取当前可用串口
            if (ports.Length > 0) {
                foreach (string name in ports) {
                    box_portName.Items.Add(name);
                }

            }
            else {
                //MessageBox.Show("There is no availble serialport");
                // this.Dispatcher.BeginInvoke(new SendStringEventHander(AppendStringToLogBox), "There is no availble serialport", true);
            }

            //baudRate
            string[] baudRates = { "1200", "2400", "4800", "9600", "14400", "19200", "28800", "57600", "115200" };
            foreach (string baudrate in baudRates)
            {
                box_baudRate.Items.Add(baudrate);
            }

            //databits
            string[] strDataBits = { "5", "6", "7", "8" };
            foreach (string str in strDataBits) {
                box_dataBits.Items.Add(str);
            }
            //parity
            //Dictionary<string, int> dicParity = new Dictionary<string, int>();
            dicParity.Add("none", 0);
            dicParity.Add("odd", 1);
            dicParity.Add("even", 2);
            dicParity.Add("mark", 3);
            dicParity.Add("space", 4);
            foreach (KeyValuePair<string, int> dic in dicParity) {
                box_parityBits.Items.Add(dic.Key);
            }
            //StopBits
            String[] strStopBits = { "1", "1.5", "2" };
            foreach (string str in strStopBits) {
                box_stopBits.Items.Add(str);
            }
            //
            ComPort.ReadBufferSize = 1024;
           

        }
        private void user_initiate_default() {
            //box_default
            box_portName.Text = "COM3";
            box_baudRate.Text = "9600";
            box_dataBits.Text = "8";
            box_stopBits.Text = "1";
            box_parityBits.Text = "none";
            //btn_default
            //取消事件触发，改用新线程处理
            //ComPort.DataReceived += new SerialDataReceivedEventHandler(ComPort_DataReceived);
           
            //box
            box_log.Document.Blocks.Clear();
            box_recieve.Document.Blocks.Clear();
            
        }
    
        private void AppendStringToLogBox(string str, bool newLine) {
            //if(box_log)
            if (newLine) {
                box_log.AppendText("\n");
            }
            box_log.AppendText(DateTime.Now.ToString()+":  "+str);

            if (box_log.Document.Blocks.Count > 10) {

                box_log.Document.Blocks.Remove(box_log.Document.Blocks.FirstBlock);
            }
            //box_log.Selection.Start;
            //保持lScroll在底部
            scroll_log.ScrollToBottom();
         
            
        }
        private void AppendStringToRecieveBox(string str, bool newline) {
            if (box_recieve.Document.Blocks.Count > 100) {
                box_recieve.Document.Blocks.Remove(box_recieve.Document.Blocks.FirstBlock);
            }
            if (newline)
            {
                box_recieve.AppendText("\n");
            }
            box_recieve.AppendText(DateTime.Now.ToString() + ":  " + str);
            //保持lScroll在底部
            scroll_recieve.ScrollToBottom();
        }
        private delegate void SendSting2EventHander(string hex, string dec);
        private void AppendDecHex(int iValue, string hex) {

            this.Dispatcher.BeginInvoke(new SendSting2EventHander(AppendDecHexHander), hex, iValue.ToString()); 

        }

        private void AppendDecHexHander(string hex, string iValue)
        {
            
            //box_hex.AppendText("\n");
            //box_dec.AppendText("\n");
            //box_dec.AppendText("" + iValue);       
            //box_hex.AppendText("" + hex);
            ////保持lScroll在底部
            //scroll_dec.ScrollToBottom();
            //scroll_hex.ScrollToBottom();
        }


        private volatile Queue dataBytesArrQueue = new Queue();
        private volatile Queue<byte> dataBytesQueue = new Queue<byte>();
        private volatile Queue<int> dataIntQueue = new Queue<int>();
        private volatile byte[] dataArrBuffer = new byte[5];
        private String hex;
        private int iValue;
        private delegate void DataProcessEventHander(string str);
        private delegate void SendStringEventHander(string str, bool newline);
        private void ComPort_DataReceived()
        {
            while (true) {
                System.Threading.Thread.Sleep(20);
                byte[] recBuffer;//接收缓冲区  
                if (ComPort.IsOpen)
                {
                    try
                    {
                        recBuffer = new byte[ComPort.BytesToRead];//接收数据缓存大小
                      
                      //  this.Dispatcher.BeginInvoke(new SendStringEventHander(AppendStringToLogBox), recBuffer.Length.ToString(), true);
                        ComPort.Read(recBuffer, 0, recBuffer.Length);//读取数据  
                        dataBytesArrQueue.Enqueue(recBuffer);//读取数据入列Enqueue（全局）  
                    }
                    catch
                    {
                        // MessageBox.Show("无法接收数据，原因未知！");
                        this.Dispatcher.BeginInvoke(new SendStringEventHander(AppendStringToLogBox), "can't recieve data", true);
                    }

                }
                //else { MessageBox.Show("COM is Closed", "warning"); }
            }
        }
        byte[] dataByteArr = new byte[4];
        private void DecodeBytes() {
            while (true) {
                if (comIsClosing == false)
                {

                    //recive =adoh+adol+adoll+parity(xor)+0xAA
                    if (dataBytesArrQueue.Count > 0)
                    {
                        byte[] tmpByteArr = (byte[])dataBytesArrQueue.Dequeue();
                        if (tmpByteArr != null)
                        {
                            for (int k = 0; k < tmpByteArr.Length; k++)
                            {
                                dataBytesQueue.Enqueue(tmpByteArr[k]);
                            }
                        }

                    }
                    if (dataBytesQueue.Count >7)
                    {

                     
                        if (dataBytesQueue.Dequeue() == 0xAB)
                        {//0xAB+0xBA+DataH+DataL+DataLL+Parity
                            if (dataBytesQueue.Dequeue() == 0xBA) {
                                dataByteArr[0] = 0;
                                dataByteArr[1] = dataBytesQueue.Dequeue();
                                dataByteArr[2] = dataBytesQueue.Dequeue();
                                dataByteArr[3] = dataBytesQueue.Dequeue();
                                if (dataBytesQueue.Dequeue() == (byte)(dataByteArr[1] ^ dataByteArr[2] ^ dataByteArr[3])) {
                                   // if (dataBytesQueue.Dequeue() == 0x55) {
                                        //说明得到正确的数据
                                        hex = BitConverter.ToString(dataByteArr).Replace("-", string.Empty);
                                        iValue = Convert.ToInt32(hex, 16);
                                       // this.Dispatcher.Invoke(new DataProcessEventHander(DataProcessUpdate), hex);
                                        if (chartWindow != null&& bChartWindowIsOpen) { chartWindow.AddData(iValue); }
                                        getDataQueueSave.Enqueue(iValue);
                                        if (getDataQueueSave.Count > 1000) {
                                            getDataQueueSave.Dequeue();
                                        }
                                    this.Dispatcher.BeginInvoke(new SendStringEventHander(AppendStringToRecieveBox),iValue.ToString(), true);
                                        //if (textWindow != null&& bTextWindowIsOpen) { textWindow.updateSource(iValue); }
                                        // AppendDecHex(iValue, hex);
                                        // this.Dispatcher.BeginInvoke(new SendStringEventHander(AppendHex), hex, true);
                                        //Thread.Sleep(1);
                                    //}
                                   // Thread.Sleep(1);
                                }
                               // Thread.Sleep(1);
                            }
                          //  Thread.Sleep(1);
                        }
                       // Thread.Sleep(1);

                    }
                  // Thread.Sleep(1);

                }
                else {
                    Thread.Sleep(1000);
                }
            }

        }
        //call back
        private delegate void DisplayEventHander(string str);
        private void DataProcessUpdate(string hex) {
            //box_data.Add(new LineBreak());
            //box_data.Add("0X"+hex);
            //ChartPlotterDisplay(hex);
            //box_data.AppendText("\n");
            //string str = hex.Remove(0, 1);
            //box_data.AppendText(str + "H");
            //btn_adoh.Content = hex.Substring(0, 1);
            //btn_adol.Content = hex.Substring(2, 3);
            //btn_adoll.Content = hex.Substring(4, 5);

        }
        //ChartPlotterDisplay
        //private EnumerableDataSource<int> dataSource = null;
        Queue<int> dataSourceQueue = new Queue<int>();

        private void ChartPlotterDisplay(string str) {
            int i = Convert.ToInt32(str, 16);
            dataSourceQueue.Enqueue(i);
            //chartPlotter.Viewport.FitToView();
        }



        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            User_initiate_serialPort();
            user_initiate_default();
          

        }

        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            ComPort.Close();
            comIsClosing = true;

            this.Dispatcher.BeginInvoke(new SendStringEventHander(AppendStringToLogBox), "is closed.", true);
        }

        private void btn_open_Click(object sender, RoutedEventArgs e)
        {

            if (ComPort.IsOpen) {

                this.Dispatcher.BeginInvoke(new SendStringEventHander(AppendStringToLogBox), "is open.", true);
            }
            else
            {

                if (ComPort == null) { ComPort = new SerialPort(); }

                try
                {
                    ComPort.PortName = box_portName.SelectedValue.ToString();
                    ComPort.BaudRate = Convert.ToInt32(box_baudRate.SelectedValue.ToString());
                    ComPort.DataBits = Convert.ToInt32(box_dataBits.SelectedValue.ToString());
                    ComPort.StopBits = (StopBits)Convert.ToDouble(box_stopBits.SelectedValue.ToString());
                    ComPort.Parity = (Parity)Convert.ToInt32(dicParity[box_parityBits.SelectedValue.ToString()]);
                    ComPort.Open();
                    comIsClosing = false;
                    this.Dispatcher.BeginInvoke(new SendStringEventHander(AppendStringToLogBox), "Open Successful.", true);
                }
                catch
                {
                   // MessageBox.Show("Can't Open " + ComPort.PortName, "Warnning");
                    this.Dispatcher.BeginInvoke(new SendStringEventHander(AppendStringToLogBox),"Can't Open " + ComPort.PortName, true);
                }

            }
               
        }

        private void btn_Send_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_clear_Click(object sender, RoutedEventArgs e)
        {
            box_log.Document.Blocks.Clear();
            box_recieve.Document.Blocks.Clear();
        }

        private void btn_reset_Click(object sender, RoutedEventArgs e)
        {
            box_log.Document.Blocks.Clear();
            box_recieve.Document.Blocks.Clear();
        }

        private void btn_help_Click(object sender, RoutedEventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void window_main_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {   if (chartWindow != null) {
                chartWindow.Close();
            }
            if (textWindow != null) {
                textWindow.Close();
            }
          
        }
     
        private void btn_chart_Click(object sender, RoutedEventArgs e)
        {
           
          
        }
        private delegate void DoTask();
        private void startChartWindow() {

            //this.Dispatcher.Invoke(new DoTask(DoMyTask));
            //this.Dispatcher.Invoke(new DoTask(showChartWindow));
            System.Windows.Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new DoTask(showChartWindow));
            System.Windows.Threading.Dispatcher.Run();

        }


        private void showChartWindow()
        {
            //在此执行你的代码
            chartWindow = new ChartWindow();
           // chartWindow.ShowDialog();
            //System.Windows.Threading.Dispatcher.Run();
        }
        private void showTextWindow()
        {
            //在此执行你的代码
            textWindow = new TextWindow();
            // textWindow.ShowDialog();
            System.Windows.Threading.Dispatcher.Run();
        }

     
        private void btn_text_Click(object sender, RoutedEventArgs e)
        {
        
            
        }
        private void startTextWindow() {
            //this.Dispatcher.Invoke(new DoTask(showTextWindow));
            System.Windows.Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new DoTask(showTextWindow));
            System.Windows.Threading.Dispatcher.Run();

        }
        ~MainWindow() {

            ComPort = null;
            threadChartWindow.Abort();
            threadTextWindow.Abort();


        }
        private bool bChartWindowIsOpen = false;
        private void checkBox_Checked(object sender, RoutedEventArgs e)
        {
            if (!bChartWindowIsOpen)
            {
                bChartWindowIsOpen = true;
                chartWindow.ShowDialog();

            }
            else {
                //chartWindow.Close();
                //chartWindow.IsVisible = true;
            }

            
        }
        private bool bTextWindowIsOpen = false;
   

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (!bTextWindowIsOpen)
            {
                bTextWindowIsOpen = true;
                textWindow.updateSource(getDataQueueSave);
                textWindow.ShowDialog();

            }
        }

        private void button_Copy_Click(object sender, RoutedEventArgs e)
        {
            if (!bChartWindowIsOpen)
            {
                bChartWindowIsOpen = true;
                chartWindow.ShowDialog();

            }
        }

        private void button_Copy2_Click(object sender, RoutedEventArgs e)
        {

        }

        private void box_portName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ports = SerialPort.GetPortNames(); //获取当前可用串口
            if (ports.Length > 0)
            {
                foreach (string name in ports)
                {

                    if (!box_portName.Items.Contains(name) ){
                        box_portName.Items.Add(name);
                    }
                }

            }

        }
    }

}

