using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Microsoft.Win32;
using System.IO;

namespace ChipseaUartHelper
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {    
        private string[] ports;                       //可用串口数组
        private bool comIsClosing = false;
        ChartWindow chartWindow = null;
        Thread threadChartWindow = null;
        Thread recData = null;
        Thread threadSerialPortDataRecieved = null;
        //缓存数据
        Queue<int> getDataQueueSave = new Queue<int>();
        SerialPortManager spManager = new SerialPortManager();
        public MainWindow()
        {
            InitializeComponent();

            threadChartWindow = new Thread(new ThreadStart(startChartWindow));
            threadChartWindow.SetApartmentState(ApartmentState.MTA);
            threadChartWindow.IsBackground = true;
            threadChartWindow.Start();
            //
            threadSerialPortDataRecieved = new Thread(new ThreadStart(ComPort_DataReceived));
            threadSerialPortDataRecieved.SetApartmentState(ApartmentState.STA);
            threadSerialPortDataRecieved.IsBackground = true;
            threadSerialPortDataRecieved.Start();

            recData = new Thread(new ThreadStart(DecodeBytes));
            recData.SetApartmentState(ApartmentState.STA);
            recData.IsBackground = true;
            recData.Start();
            //
            spManager.OnSerialPortMissing += SpManager_OnSerialPortMissing;
            
        }

        private void ChartWindow_chartWindowClosing(object sender, EventArgs e)
        {
            radioButton_hex.IsChecked = true;
            AppendStringToLogBox("ChartWindow closed", true);
          //  throw new NotImplementedException();
        }

        private void SpManager_OnSerialPortMissing(object sender, SerialPortMissingEventArgs e)
        {
            comIsClosing = true;
            this.Dispatcher.BeginInvoke(new SendStringEventHander(AppendStringToLogBox), e.msg, true);
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
            //    ComPort.ReadBufferSize = 1024;


        }
        private void user_initiate_default() {
            //box_default
            if (box_portName.Items.Count > 0) {
                box_portName.Text = box_portName.Items.GetItemAt(box_portName.Items.Count - 1).ToString();
            }
            box_baudRate.Text = "9600";
            box_dataBits.Text = "8";
            box_stopBits.Text = "1";
            box_parityBits.Text = "none";
            //box
            box_log.Document.Blocks.Clear();
            box_recieve.Document.Blocks.Clear();
            //btn
            radioButton_hex.IsChecked = true;
        }

        private void AppendStringToLogBox(string str, bool newLine) {
            //if(box_log)
            if (newLine) {
                box_log.AppendText("\n");
            }

            box_log.AppendText(DateTime.Now.TimeOfDay.ToString().Substring(0, 11) + "：  " + str);

            if (box_log.Document.Blocks.Count > 100) {

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
                //box_recieve.AppendText(DateTime.Now.ToString() + ":  " + Convert.ToInt32(hex));
                if (bHexDisplayFlag)
                {
                    if (bDecodeFlag)
                    {

                        box_recieve.AppendText("\n" + "#Binary:  " + Convert.ToString(Convert.ToInt32(str, 16), 2).PadLeft(24, '0') + "   #Hex:  " + str+ "   #Dec: "+ Convert.ToInt32(str, 16));
                    }
                }
            }
            else {
                box_recieve.AppendText(str);

            }

            //保持lScroll在底部
            scroll_recieve.ScrollToBottom();
        }

        private volatile Queue dataBytesArrQueue = new Queue();
        private volatile Queue<byte> dataBytesQueue = new Queue<byte>();
        private volatile Queue<int> dataIntQueue = new Queue<int>();
        private volatile byte[] dataArrBuffer = new byte[5];
     //   private String hex;
        private int iValue;
        private delegate void DataProcessEventHander(string str);
        private delegate void SendStringEventHander(string str, bool newline);
        private void ComPort_DataReceived()
        {
            while (true) {
               // System.Threading.Thread.Sleep(5);
                if (spManager.bComPortIsOpen) {
                       if (spManager.ByteMode && spManager.receiveArrByteQueue.Count > 0)
                        {
                            if (spManager.receiveArrByteQueue.Count > 10) {
                            this.Dispatcher.Invoke(new SendStringEventHander(AppendStringToLogBox), "Too manyBytes Buffer" + " ", false);
                        }
                            byte[] recBuffer = spManager.receiveArrByteQueue.Dequeue();//接收缓冲区  
                            if (bDecodeFlag)
                            {
                                dataBytesArrQueue.Enqueue(recBuffer);//读取数据入列Enqueue（全局）
                            }
                            else
                            {

                                this.Dispatcher.Invoke(new SendStringEventHander(AppendStringToRecieveBox), BitConverter.ToString(recBuffer).Replace('-', ' ')+" ", false);
                            }
                        }
                      else if(spManager.ByteMode == false && spManager.receiveArrCharQueue.Count > 0)
                        {
                            char[] charBuffer = spManager.receiveArrCharQueue.Dequeue();
                            if (bDecodeFlag)
                            {
                                //nothing
                            }
                            else
                            {
                                this.Dispatcher.Invoke(new SendStringEventHander(AppendStringToRecieveBox),new string(charBuffer), false);
                            }
                        }
                    else
                    {
                        System.Threading.Thread.Sleep(25);
                    }
 
                }
            }
        }

        /*
         * 
         * DecodeBytes：解码函数
         * 
         * */


        byte[] dataByteArr = new byte[4];
        private void DecodeBytes() {
            while (true) {
                Thread.Sleep(10);
                if (comIsClosing == false)
                {
                    if (dataBytesArrQueue.Count > 0)
                    {
                        if (dataBytesArrQueue.Count > 700) {
                            this.Dispatcher.Invoke(new SendStringEventHander(AppendStringToLogBox), "Too many Byte Buffer" + " ", false);
                        }
                        byte[] tmpByteArr = (byte[])dataBytesArrQueue.Dequeue();
                        if (tmpByteArr != null)
                        {
                            if (bDecodeFlag)
                            {
                                for (int k = 0; k < tmpByteArr.Length; k++)
                                {
                                    dataBytesQueue.Enqueue(tmpByteArr[k]);
                                }
                            }
                        }

                    }
                    //recive = |0xAB+BA+adoh+adol+adoll+parity(xor)|
                    if (dataBytesQueue.Count > 7)
                    {

                        if (bDecodeFlag) {
                            if (dataBytesQueue.Dequeue() == 0xAB)
                            {//0xAB+0xBA+DataH+DataL+DataLL+Parity
                                if (dataBytesQueue.Dequeue() == 0xBA)
                                {
                                    dataByteArr[0] = 0;
                                    dataByteArr[1] = dataBytesQueue.Dequeue();
                                    dataByteArr[2] = dataBytesQueue.Dequeue();
                                    dataByteArr[3] = dataBytesQueue.Dequeue();
                                    if (dataBytesQueue.Dequeue() == (byte)(dataByteArr[1] ^ dataByteArr[2] ^ dataByteArr[3]))
                                    {    
                                        //说明得到正确的数据
                                       string  hex = BitConverter.ToString(dataByteArr).Replace("-", string.Empty);
                                        iValue = Convert.ToInt32(hex, 16);
                                        if (chartWindow != null && bChartWindowIsOpen) { chartWindow.AddData(iValue); }
                                        getDataQueueSave.Enqueue(iValue);
                                        this.Dispatcher.BeginInvoke(new SendStringEventHander(AppendStringToRecieveBox), hex.Substring(2), true);
                                    }
                                }
                            }
                        }
                    }
              
                }
             
            }
        }



/*
 *
 */

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            User_initiate_serialPort();
            user_initiate_default();


        }

        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            // ComPort.Close();
            spManager.CloseSerialPort();
            comIsClosing = true;
            btn_close.IsEnabled = false;
            btn_clear.IsEnabled = true;
            btn_send.IsEnabled = false;
            btn_chart.IsEnabled = false;
           // btn_save.IsEnabled = false;关闭串口，保留save按键
            //getDataQueueSave.Clear();暂时不清
            this.Dispatcher.BeginInvoke(new SendStringEventHander(AppendStringToLogBox), "is closed.", true);
        }

        private void btn_open_Click(object sender, RoutedEventArgs e)
        {
            comIsClosing = false;
            btn_close.IsEnabled = true;
            btn_clear.IsEnabled = true;
            btn_send.IsEnabled = true;
            btn_chart.IsEnabled = true;
            btn_save.IsEnabled = true;
            if (spManager.bComPortIsOpen) {

                this.Dispatcher.BeginInvoke(new SendStringEventHander(AppendStringToLogBox), "is open.", true);
            }
            else
            {

                SerialPort ComPort = new SerialPort();

                try
                {
                    ComPort.PortName = box_portName.SelectedValue.ToString();
                    ComPort.BaudRate = Convert.ToInt32(box_baudRate.SelectedValue.ToString());
                    ComPort.DataBits = Convert.ToInt32(box_dataBits.SelectedValue.ToString());
                    ComPort.StopBits = (StopBits)Convert.ToDouble(box_stopBits.SelectedValue.ToString());
                    ComPort.Parity = (Parity)Convert.ToInt32(dicParity[box_parityBits.SelectedValue.ToString()]);
                   // ComPort.Open();
                    comIsClosing = false;
                   
                    spManager.OpenSerialPort(ComPort);
                    this.Dispatcher.BeginInvoke(new SendStringEventHander(AppendStringToLogBox), "Open Successful.", true);
                }
                catch
                {
                    // MessageBox.Show("Can't Open " + ComPort.PortName, "Warnning");
                    this.Dispatcher.BeginInvoke(new SendStringEventHander(AppendStringToLogBox), "Can't Open " + ComPort.PortName, true);
                }

            }

        }



        private void btn_clear_Click(object sender, RoutedEventArgs e)
        {
            box_log.Document.Blocks.Clear();
            box_recieve.Document.Blocks.Clear();
            dataBytesArrQueue.Clear();
            spManager.receiveArrByteQueue.Clear();
            spManager.receiveArrCharQueue.Clear();
            dataBytesQueue.Clear();
            getDataQueueSave.Clear();


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
        { if (chartWindow != null) {
                chartWindow.Close();
            }
            Application.Current.Shutdown();


        }

        private delegate void DoTask();
        private void startChartWindow() {

            System.Windows.Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new DoTask(showChartWindow));
            System.Windows.Threading.Dispatcher.Run();

        }


        private void showChartWindow()
        {
            //在此执行你的代码

            chartWindow = new ChartWindow();
            if (chartWindow != null) {
                chartWindow.chartWindowClosing += ChartWindow_chartWindowClosing;
            }
           
        }

        private void btn_text_Click(object sender, RoutedEventArgs e)
        {


        }
        ~MainWindow() {

            spManager = null;

            threadChartWindow.Abort();
            getDataQueueSave.Clear();
            //threadTextWindow.Abort();
        }

        //private bool bTextWindowIsOpen = false;


        private void button_Click(object sender, RoutedEventArgs e)
        {

            spManager.CloseSerialPort();
            comIsClosing = true;
  
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

                    if (!box_portName.Items.Contains(name)) {
                        box_portName.Items.Add(name);
                    }
                }

            }

        }
        private bool bDecodeFlag = false;


        private void checkBox_decode_Checked(object sender, RoutedEventArgs e)
        {
            bDecodeFlag = true;
            bHexDisplayFlag = true;
            radioButton_hex.IsChecked = true;
            radioButton_ascii.IsEnabled = false;
            Thread.Sleep(1);
        }

        private void checkBox_decode_Unchecked(object sender, RoutedEventArgs e)
        {
            bDecodeFlag = false;
            radioButton_ascii.IsEnabled = true;
            Thread.Sleep(1);
        }


        private bool bChartWindowIsOpen = false;

        private void btn_chart_Click(object sender, RoutedEventArgs e)
        {
            if (!chartWindow.IsVisible)
            {
                bChartWindowIsOpen = true;
                chartWindow = new ChartWindow();
                chartWindow.chartWindowClosing += ChartWindow_chartWindowClosing;//注册子窗口关闭事件
                checkBox_decode.IsChecked = true;
                bHexDisplayFlag = true;
                chartWindow.ShowDialog();
            }
        }
        //发送按钮
        private string sendStringBuffer = null;
        ////private bool bSendFlag = true;
        DispatcherTimer sendTimer = null;
        private void btn_send_Click(object sender, RoutedEventArgs e)
        {
            if (spManager.bComPortIsOpen) {
                if (btn_send.Content.Equals("sending") && bSendTimedFlag)
                {

                    btn_send.Content = "send";
                    sendTimer.Stop();
                    AppendStringToLogBox("colse sending", true);
                }
                else {
                    sendStringBuffer = textBox_send.Text;
                    sendTimer = new DispatcherTimer();
                    sendTimer.Tick += new EventHandler(sendTimerHander);
                    sendTimer.Interval = TimeSpan.FromMilliseconds(Convert.ToInt32(textBox_time.Text));
                    sendTimer.Start();
                    btn_send.Content = "sending";
                 }
              }
            else
            {
                AppendStringToLogBox("Open SerialPort First.", true);
            }

        }
        private void sendTimerHander(object sender, System.EventArgs  e) {

                if (!comIsClosing)
                {
                    if (bSendTimedFlag)
                    {
                            if (spManager.ByteMode)
                            {
                                byte[] sendBytesBuffer = Encoding.Default.GetBytes(sendStringBuffer);
                                AppendStringToLogBox("send Hex: " + BitConverter.ToString(sendBytesBuffer), true);
                                spManager.SendDataPacket(sendBytesBuffer);
                            }
                            else {
                                spManager.SendDataPacket(sendStringBuffer);
                                AppendStringToLogBox("send Stirng: " + sendStringBuffer, true);
                            }
                    }
                    else
                    {
                        if (spManager.ByteMode)
                        {
                            byte[] sendBytesBuffer = Encoding.Default.GetBytes(sendStringBuffer);
                            AppendStringToLogBox("send Hex: " + BitConverter.ToString(sendBytesBuffer), true);
                            spManager.SendDataPacket(sendBytesBuffer);
                        }
                        else
                        {
                            spManager.SendDataPacket(sendStringBuffer);
                            AppendStringToLogBox("send Stirng: " + sendStringBuffer, true);
                        }
                        btn_send.Content = "send";
                                sendTimer.Stop();
                        }
                }
                else
                {
                    btn_send.Content = "send";
                    sendTimer.Stop();
            }

        }
        private bool bSendTimedFlag = false;
        private void checkBox_timed_Checked(object sender, RoutedEventArgs e)
        {
            bSendTimedFlag = true;
        }

        private void checkBox_timed_Unchecked(object sender, RoutedEventArgs e)
        {
            bSendTimedFlag = false;
        }

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            comIsClosing = true;
            if (bDecodeFlag)
            {
                //getDataQueueSave
                spManager.CloseSerialPort();
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.Title = "Save text Files";
                dlg.DefaultExt = "txt";
                dlg.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                dlg.FilterIndex = 1;
                dlg.RestoreDirectory = true;
                if (dlg.ShowDialog() == true)
                {
                    using (FileStream fs = new FileStream(dlg.FileName, FileMode.Create))
                    {
                        using (StreamWriter writer = new StreamWriter(fs, Encoding.UTF8))
                        {
                            // writer.Write(Textbox1.Content);
                            foreach (int iValue in getDataQueueSave)
                            {
                                writer.WriteLine(iValue);
                            }
                            //MessageBox.Show("Write txt successful.");
                            writer.Close();
                            AppendStringToLogBox("Save successful.", true);
                        }
                    }
                }
            }
            else {

                AppendStringToLogBox("need decode first.",true);
            }
  

        }
        private bool bHexDisplayFlag = true;

        private void radioButton_hex_Checked(object sender, RoutedEventArgs e)
        {
            bHexDisplayFlag = true;
            spManager.ByteMode = true;
            Thread.Sleep(1);

        }

        private void radioButton_ascii_Checked(object sender, RoutedEventArgs e)
        {
            bHexDisplayFlag = false;
            spManager.ByteMode = false;
            Thread.Sleep(1);
        }

        private void textBox_time_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sendTimer != null) {
                sendTimer.Interval = TimeSpan.FromMilliseconds(Convert.ToInt32(textBox_time.Text));
            }
             
        }

        private void textBox_send_TextChanged(object sender, TextChangedEventArgs e)
        {
            sendStringBuffer = textBox_send.Text;
        }
    }

}

