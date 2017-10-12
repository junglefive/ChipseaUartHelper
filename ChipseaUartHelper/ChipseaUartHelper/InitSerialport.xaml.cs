using System;
using System.Collections.Generic;
using System.IO.Ports;
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

namespace ChipseaUartHelper
{
    /// <summary>
    /// InitSerialport.xaml 的交互逻辑
    /// </summary>
    public partial class InitSerialport : Window
    {


        private string[] ports;
        public SerialPort ComPort = new SerialPort();
        public InitSerialport(SerialPort com)
        {
            ComPort = com;
            InitializeComponent();
            User_initiate_serialPort();
            user_initiate_default();
        }

        private void user_initiate_default()
        {
            //box_default
            if (box_portName.Items.Count > 0)
            {
                box_portName.Text = box_portName.Items.GetItemAt(box_portName.Items.Count - 1).ToString();
            }
            box_baudRate.Text = "9600";
            box_dataBits.Text = "8";
            box_stopBits.Text = "1";
            box_parityBits.Text = "none";
        }
        /**
*function:初始化串口部分
*by:junglefive
**/
        Dictionary<string, int> dicParity = new Dictionary<string, int>();
        private void User_initiate_serialPort()
        {
            //serialPortNames
            ports = SerialPort.GetPortNames(); //获取当前可用串口
            if (ports.Length > 0)
            {
                foreach (string name in ports)
                {
                    box_portName.Items.Add(name);
                }

            }
            else
            {
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
            foreach (string str in strDataBits)
            {
                box_dataBits.Items.Add(str);
            }
            //parity
            //Dictionary<string, int> dicParity = new Dictionary<string, int>();
            dicParity.Add("none", 0);
            dicParity.Add("odd", 1);
            dicParity.Add("even", 2);
            dicParity.Add("mark", 3);
            dicParity.Add("space", 4);
            foreach (KeyValuePair<string, int> dic in dicParity)
            {
                box_parityBits.Items.Add(dic.Key);
            }
            //StopBits
            String[] strStopBits = { "1", "1.5", "2" };
            foreach (string str in strStopBits)
            {
                box_stopBits.Items.Add(str);
            }
            //
            //    ComPort.ReadBufferSize = 1024;


        }

        private void box_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
           
        }

        private void button_ok_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ComPort.PortName = box_portName.SelectedValue.ToString();
                ComPort.BaudRate = Convert.ToInt32(box_baudRate.SelectedValue.ToString());
                ComPort.DataBits = Convert.ToInt32(box_dataBits.SelectedValue.ToString());
                ComPort.StopBits = (StopBits)Convert.ToDouble(box_stopBits.SelectedValue.ToString());
                ComPort.Parity = (Parity)Convert.ToInt32(dicParity[box_parityBits.SelectedValue.ToString()]);
                this.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show("parse erro: "+ ex.ToString(), "warnnig");
            }
        }
    }
}
