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

namespace ChipseaUartHelper
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        //串口相关
        public  SerialPort ComPort = new SerialPort();//声明一个串口      
        private string[] ports;                       //可用串口数组
        private bool recStaus = true;                 //接收状态字
        private bool ComPortIsOpen = false;           //COM口开启状态字，在打开/关闭串口中使用，这里没有使用自带的ComPort.IsOpen，因为在串口突然丢失的时候，ComPort.IsOpen会自动false，逻辑混乱
       
        DispatcherTimer autoSendTick = new DispatcherTimer();//定时发送
        private static bool Sending = false;//正在发送数据状态字
        private static Thread _ComSend;//发送数据线程
        Queue recQueue = new Queue();//接收数据过程中，接收数据线程与数据处理线程直接传递的队列，先进先出
        private  SendSetStr SendSet = new SendSetStr();//发送数据线程传递参数的结构体
        private  struct SendSetStr//发送数据线程传递参数的结构体格式
        {
            public string SendSetData;//发送的数据
            public bool   SendSetMode;//发送模式
        } 
        public MainWindow()
        {
            InitializeComponent();
        }
        /**
        *function:初始化串口部分
        *by:junglefive
        **/
        private void User_initiate_serialPort(){
            //serialPortNames
            ports = SerialPort.GetPortNames(); //获取当前可用串口
            if(ports.Length>0){
                foreach ( string name in ports){
                box_portName.Items.Add(name);
                }

            }
            else{
                MessageBox.Show("There is no availble serialport");
            }
           
            //baudRate
            string[] baudRates = {"1200","2400","4800","9600","14400","19200","28800","57600","115200" };
            foreach (string baudrate in baudRates)
            {
                box_baudRate.Items.Add(baudrate);
            }

            //databits
            string[] strDataBits = {"5","6","7","8" };
            foreach (string str in strDataBits) {
                box_dataBits.Items.Add(str);
            }
            //parity
            Dictionary<string, int> dicParity = new Dictionary<string, int>();
            dicParity.Add("none" , 0);
            dicParity.Add("odd"  , 1);
            dicParity.Add("even" , 2);
            dicParity.Add("mark" , 3);
            dicParity.Add("space", 4);
            foreach (KeyValuePair<string, int> dic in dicParity) {
                box_parity.Items.Add(dic.Key);
            }
            //StopBits
            String[] strStopBits = { "1","1.5", "2"};
            foreach (string str in strStopBits) {
                box_stopBits.Items.Add(str);
            }
           

        }
        private void user_initiate_default() {
            //box_default
            box_portName.Text = "COM1";
            box_baudRate.Text = "9600";
            box_dataBits.Text = "8";
            box_stopBits.Text = "1";
            box_parity.Text = "none";
            //btn_default
            ComPort.DataReceived +=new SerialDataReceivedEventHandler( ComPort_DataReceived);
            

        }

        private void ComPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            User_initiate_serialPort();
            user_initiate_default();
        }

        private void btn_open_close_Click(object sender, RoutedEventArgs e)
        {
            if (ComPort.IsOpen)
            {
                
                 
                ComPort.Close();
                btn_open_close.Content = "Open";
            }

            else {
                    try
                    {
                        ComPort.PortName = box_portName.SelectedValue.ToString();
                        ComPort.BaudRate = Convert.ToInt32(box_baudRate.SelectedValue.ToString());
                        ComPort.DataBits = Convert.ToInt32(box_dataBits.SelectedValue.ToString());
                        ComPort.StopBits = (StopBits)Convert.ToDouble(box_stopBits.SelectedValue.ToString());
                        ComPort.Parity = (Parity)Convert.ToInt32(box_parity.SelectedValue.ToString());
                        ComPort.Open();
                        btn_open_close.Content = "Close";
                    }
                    catch{
                        MessageBox.Show("Can't Open "+ComPort.PortName, "Warnning");

                    }    
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
    }
}
