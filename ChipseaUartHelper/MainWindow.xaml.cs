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

        public MainWindow()
        {
            InitializeComponent();
            Thread threadChartWindow=null;
            threadChartWindow = new Thread(new ThreadStart(startChartWindow));
            threadChartWindow.SetApartmentState(ApartmentState.STA);
            threadChartWindow.Start();
            Thread threadTextWindow=null;
            threadTextWindow = new Thread(new ThreadStart(startTextWindow));
            threadTextWindow.SetApartmentState(ApartmentState.STA);
            threadTextWindow.Start();
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
                MessageBox.Show("There is no availble serialport");
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
            ComPort.ReadBufferSize = 128;
            //dataSource = new EnumerableDataSource<int>(dataSourceQueue);
            //dataSource = new EnumerableDataSource<int>(dataSourceQueue);
            // chartPlotter.AddLineGraph(dataSource, Colors.Red, 1, "data");

        }
        private void user_initiate_default() {
            //box_default
            box_portName.Text = "COM10";
            box_baudRate.Text = "9600";
            box_dataBits.Text = "8";
            box_stopBits.Text = "1";
            box_parityBits.Text = "none";
            //btn_default
            ComPort.DataReceived += new SerialDataReceivedEventHandler(ComPort_DataReceived);
            Thread recData = new Thread(new ThreadStart(DecodeBytes));
            recData.Start();
            //window
     
        }

        private Queue dataBytesArrQueue = new Queue();
        private Queue<byte> dataBytesQueue = new Queue<byte>();
        private Queue<int> dataIntQueue = new Queue<int>();
        private byte[] dataArrBuffer = new byte[4];
        private String hex;
        private int iValue;
        private delegate void DataProcessEventHander(string str);
        private delegate void TipsSendEventHander(string str);
        private void ComPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            System.Threading.Thread.Sleep(100);
            byte[] recBuffer;//接收缓冲区  
            if (ComPort.IsOpen)
            {
                try
                {
                    recBuffer = new byte[ComPort.BytesToRead];//接收数据缓存大小  
                    ComPort.Read(recBuffer, 0, recBuffer.Length);//读取数据  
                    dataBytesArrQueue.Enqueue(recBuffer);//读取数据入列Enqueue（全局）  
                }
                catch
                {
                    MessageBox.Show("无法接收数据，原因未知！");
                }

            }
            else { MessageBox.Show("COM is Closed", "warning"); }

        }

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
                    if (dataBytesQueue.Count > 0)
                    {

                        byte tmpByte = dataBytesQueue.Dequeue();
                        if (tmpByte == 0xAA && dataArrBuffer[0] == (byte)(dataArrBuffer[1] ^ dataArrBuffer[2] ^ dataArrBuffer[3]))
                        {
                            //说明得到正确的数据
                            byte[] bytesValue = new byte[4];
                            bytesValue[0] = 0x00;
                            for (int i = 1; i < 4; i++)
                            {
                                bytesValue[i] = dataArrBuffer[4 - i];
                            }
                            hex = BitConverter.ToString(bytesValue).Replace("-", string.Empty);
                            iValue = Convert.ToInt32(hex, 16);
                            this.Dispatcher.Invoke(new DataProcessEventHander(DataProcessUpdate), hex);
                            //Display
                            //this.Dispatcher.Invoke(new DisplayEventHander(DataProcessUpdate), iValue);
                            // ChartPlotterDisplay(iValue);
                            if (chartWindow != null) { chartWindow.AddData(iValue); }
                            if (textWindow != null) { textWindow.updateSource(iValue); }

                        }
                        //moving one  byte
                        dataArrBuffer[3] = dataArrBuffer[2];
                        dataArrBuffer[2] = dataArrBuffer[1];
                        dataArrBuffer[1] = dataArrBuffer[0];
                        dataArrBuffer[0] = tmpByte;
                    }

                    Thread.Sleep(10);
                }
                else { Thread.Sleep(10); }
            }

        }
        //call back
        private delegate void DisplayEventHander(string str);
        private void DataProcessUpdate(string hex) {
            //box_data.Add(new LineBreak());
            //box_data.Add("0X"+hex);
            ChartPlotterDisplay(hex);
            //box_data.AppendText("\n");
            string str = hex.Remove(0, 1);
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
        }

        private void btn_open_Click(object sender, RoutedEventArgs e)
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
                }
                catch
                {
                    MessageBox.Show("Can't Open " + ComPort.PortName, "Warnning");
                }
        }

        private void btn_Send_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_clear_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_reset_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_help_Click(object sender, RoutedEventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void window_main_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            chartWindow.Close();
            textWindow.Close();
        }
     
        private void btn_chart_Click(object sender, RoutedEventArgs e)
        {
           
          
        }
        private delegate void DoTask();
        private void startChartWindow() {

            //this.Dispatcher.Invoke(new DoTask(DoMyTask));
            //this.Dispatcher.Invoke(new DoTask(showChartWindow));
            System.Windows.Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new DoTask(showChartWindow));
            System.Windows.Threading.Dispatcher.Run();

        }


        private void showChartWindow()
        {
            //在此执行你的代码
            chartWindow = new ChartWindow();
            chartWindow.Show();
            //System.Windows.Threading.Dispatcher.Run();
        }
        private void showTextWindow()
        {
            //在此执行你的代码
            textWindow = new TextWindow();
            textWindow.Show();
           //System.Windows.Threading.Dispatcher.Run();
        }

     
        private void btn_text_Click(object sender, RoutedEventArgs e)
        {
        
            
        }
        private void startTextWindow() {
            //this.Dispatcher.Invoke(new DoTask(showTextWindow));
            System.Windows.Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new DoTask(showTextWindow));
            System.Windows.Threading.Dispatcher.Run();

        }

    }
    //public partial class ChartWindow : Window
    //{

    //    public ChartWindow() { }

    //}


    //public partial class LogWindow : Window {



    //}

}

