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
        private User_initiate_serialPort(){
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
            foreach( string baudrate in baudRates){
                box_baudRate.Items.Add(baudrate);
            }
            //parity
            Hashtable htParity = new Hashtable();
            htParity.add("none" , 0);
            htParity.add("odd"  , 1);
            htParity.add("even" , 2);
            htParity.add("mark" , 3);
            htParity.add("space", 4);
            string[] strParity = null;
            ;//此处如果能够遍历Key，就可不用数组中转
            htParity.Keys.CopyTo(strParity, 0);
            foreach(string parity in strParity){
                box_Parity.Items.add(parity);
            }



        }
    }
}
