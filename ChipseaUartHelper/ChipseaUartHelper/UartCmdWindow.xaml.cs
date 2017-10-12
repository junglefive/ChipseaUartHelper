using System;
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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ChipseaUartHelper
{
    /// <summary>
    /// UartCmdWindow.xaml 的交互逻辑
    /// </summary>
    public partial class UartCmdWindow : Window
    {
        SerialPortManager spManager = new SerialPortManager();
        public UartCmdWindow(SerialPortManager sp, SerialPort com)
        {
            InitializeComponent();
            spManager = sp;
            spManager.OpenSerialPort(com); ;
            initCmd();
        }
        byte[] bArryCmd1 = { 0x10, 0x00, 0x00, 0xC5, 0x15, 0x96, 0xCA, 0x11, 0x0F, 0x00, 0x01, 0x02, 0x39, 0x55, 0xDB, 0xBC, 0x5A, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x4c, 0x00, 0x00 };
        byte[] bArryCmd2 = { 0x10, 0x00, 0x00, 0xC5, 0x15, 0x96, 0x21, 0x00, 0x00, 0x00, 0x0a, 0x00, 0x02, 0x39, 0x00, 0x00, 0x00, 0x20, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
        byte[] bArryCmd3 = { 0x10, 0x00, 0x00, 0xC5, 0x15, 0x96, 0x01, 0x00, 0x00, 0x00, 0x0a, 0x00, 0x02, 0x39, 0x00, 0x28, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
        byte[] bArryCmd4 = { 0x10, 0x00, 0x00, 0xC5, 0x15, 0x94, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x00, 0x00 };
        byte[] bArryCmd5 = { 0x10, 0x00, 0x00, 0xC5, 0x01, 0x81, 0x45 };
        private void initCmd() {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            DateTime nowTime = DateTime.Now;
            long unixTime = (long)Math.Round((nowTime - startTime).TotalSeconds, MidpointRounding.AwayFromZero);
            byte[] unixtime = BitConverter.GetBytes(unixTime);
            //timeBlock.Text = "timeStamp: "+ Convert.ToString(unixTime,16);
            for (int i = 0; i < 4; i++)
            {
                bArryCmd2 [7 + i] =  unixtime[3 - i];
                bArryCmd3 [7 + i] = unixtime[3 - i];
            }

            for (int i = 0; i< 23; i++)
            {
                bArryCmd1[26] ^= bArryCmd1[3 + i];
                bArryCmd2[26] ^= bArryCmd2[3 + i];
                bArryCmd3[26] ^= bArryCmd3[3 + i];
                bArryCmd4[26] ^= bArryCmd4[3 + i];
            }
            combox_cmd1.Text = BitConverter.ToString(bArryCmd1).Replace('-', ' ') + " ";
            combox_cmd2.Text = BitConverter.ToString(bArryCmd2).Replace('-', ' ') + " ";
            combox_cmd3.Text = BitConverter.ToString(bArryCmd3).Replace('-', ' ') + " ";
            combox_cmd4.Text = BitConverter.ToString(bArryCmd4).Replace('-', ' ') + " ";
            combox_cmd5.Text = BitConverter.ToString(bArryCmd5).Replace('-', ' ') + " ";
        }
        public byte[] strToToHexByte(string hexStr)
        {
            string hexString = hexStr;
            hexString = hexString.Replace(" ", "");
            // if ((hexString.Length % 2) != 0)
            //  hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];

            try
            {
                for (int i = 0; i < returnBytes.Length; i++)
                {
                    returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Parse Input Hex data Erro", "Erro");

            }

            return returnBytes;
        }

        private bool bSendingFlag = false;
        private int t1 = 0;
        private int t2 = 0;
        private int t3 = 0;
        private int t4 = 0;
        private int t5 = 0;
        private void btn_send1_Click(object sender, RoutedEventArgs e)
        {

            spManager.SendDataPacket(strToToHexByte(combox_cmd1.Text.ToString()));
            txt_log.Text = " " + combox_cmd1.Text.ToString();

        }
        private void btn_send2_Click(object sender, RoutedEventArgs e)
        {
            spManager.SendDataPacket(strToToHexByte(combox_cmd2.Text.ToString()));
            txt_log.Text = " " + combox_cmd2.Text.ToString();
        }

        private void btn_send3_Click(object sender, RoutedEventArgs e)
        {
            spManager.SendDataPacket(strToToHexByte(combox_cmd3.Text.ToString()));
            txt_log.Text = " " + combox_cmd3.Text.ToString();
        }

        private void btn_send4_Click(object sender, RoutedEventArgs e)
        {
            spManager.SendDataPacket(strToToHexByte(combox_cmd4.Text.ToString()));
            txt_log.Text = " " + combox_cmd4.Text.ToString();
        }

        private void btn_send5_Click(object sender, RoutedEventArgs e)
        {
            spManager.SendDataPacket(strToToHexByte(combox_cmd5.Text.ToString()));
            txt_log.Text = " " + combox_cmd5.Text.ToString();

        }
        Thread thread = null;
        private string txt_str_t1;
        private string txt_str_t2;
        private string txt_str_t3;
        private string txt_str_t4;
        private string txt_str_t5;
        private void btn_send666_Click(object sender, RoutedEventArgs e)
        {
            t1 = Convert.ToInt32(txt_t1.Text);
            t2 = Convert.ToInt32(txt_t2.Text);
            t3 = Convert.ToInt32(txt_t3.Text);
            t4 = Convert.ToInt32(txt_t4.Text);
            t5 = Convert.ToInt32(txt_t5.Text);

            txt_str_t1 = combox_cmd1.Text;
            txt_str_t2 = combox_cmd2.Text;
            txt_str_t3 = combox_cmd3.Text;
            txt_str_t4 = combox_cmd4.Text;
            txt_str_t5 = combox_cmd5.Text;

            if (btn_send666.Content.Equals("Stop Send"))
            {
                try {
                    btn_send666.Content = "Loop Send All";
                    //sendTimer.Stop();
                    thread.Abort();
                    bSendingFlag = false;
                    txt_log.Text = "closed sending";
                }
                catch {
                    txt_log.Text = "erro";
                }

            }
            else
            {
                if (bSendingFlag == false)
                {
                    if (t1_checked || t2_checked || t3_checked || t4_checked || t5_checked)
                    {
                        bSendingFlag = true;
                        try {
                            thread = new Thread(new ThreadStart(SendCommand));
                            thread.SetApartmentState(ApartmentState.STA);
                            thread.IsBackground = true;
                            thread.Start();
                            btn_send666.Content = "Stop Send";
                        }
                        catch {
                            
                        }
                       
                    }
                    else {
                        MessageBox.Show("Please checked one More CMD","warnnig");
                    }
                    
                }
            }
        }


        private delegate void SingleParaEventHander(string str);

        void SendStringBySerialPort(string str) {
            //spManager.SendDataPacket(strToToHexByte(combox_cmd5.Text.ToString()));
            spManager.SendDataPacket(strToToHexByte(str));
            txt_log.Text = "sending: " + str;
        }
        /// <summary>
        /// /////////////////////////////////////////////
        /// </summary>
        void SendCommand() {
            while (true) {
                if (t1_checked)
                {
                    this.Dispatcher.BeginInvoke(new SingleParaEventHander(SendStringBySerialPort), txt_str_t1);
                    Thread.Sleep(t1);
                }

                if (t2_checked)
                {
                    this.Dispatcher.BeginInvoke(new SingleParaEventHander(SendStringBySerialPort), txt_str_t2);
                    Thread.Sleep(t2);
                }

                if (t3_checked)
                {
                    this.Dispatcher.BeginInvoke(new SingleParaEventHander(SendStringBySerialPort), txt_str_t3);
                    Thread.Sleep(t3);
                }

                if (t4_checked)
                {
                    this.Dispatcher.BeginInvoke(new SingleParaEventHander(SendStringBySerialPort), txt_str_t4);
                    Thread.Sleep(t4);
                }

                if (t5_checked)
                {
                    this.Dispatcher.BeginInvoke(new SingleParaEventHander(SendStringBySerialPort), txt_str_t5);
                    Thread.Sleep(t5);
                }
                bSendingFlag = false;
            }
           
        }
        private bool t1_checked = false;
        private bool t2_checked = false;
        private bool t3_checked = false;
        private bool t4_checked = false;
        private bool t5_checked = false;
        private void check_t1_Checked(object sender, RoutedEventArgs e)
        {
            t1_checked = true;
        }

        private void check_t2_Checked(object sender, RoutedEventArgs e)
        {
            t2_checked = true;
        }

        private void check_t3_Checked(object sender, RoutedEventArgs e)
        {
            t3_checked = true;
        }

        private void check_t4_Checked(object sender, RoutedEventArgs e)
        {
            t4_checked = true;
        }

        private void check_t5_Checked(object sender, RoutedEventArgs e)
        {
            t5_checked = true;
        }

        private void check_t1_Unchecked(object sender, RoutedEventArgs e)
        {
            t1_checked = false;
        }

        private void check_t2_Unchecked(object sender, RoutedEventArgs e)
        {
            t2_checked = false;
        }

        private void check_t3_Unchecked(object sender, RoutedEventArgs e)
        {
            t3_checked = false;
        }

        private void check_t4_Unchecked(object sender, RoutedEventArgs e)
        {
            t4_checked = false;
        }

        private void check_t5_Unchecked(object sender, RoutedEventArgs e)
        {
            t5_checked = false;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try {
                thread.Abort();
            }
            catch {

            }
      
        }
    }
}
