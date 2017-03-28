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

        private void SpManager_OnSerialPortMissing(object sender, SerialPortMissingEventArgs e)
        {
            //comIsClosing = true;
            AppendStringToLogBox(e.msg, true);//wuixao
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
        private void AppendStringToRecieveBox(string hex, bool newline) {
            if (box_recieve.Document.Blocks.Count > 100) {
                box_recieve.Document.Blocks.Remove(box_recieve.Document.Blocks.FirstBlock);
            }
            if (newline) { 
                //box_recieve.AppendText(DateTime.Now.ToString() + ":  " + Convert.ToInt32(hex));
                if (bHexDisplayFlag)
                {
                    if (bDecodeFlag)
                    {

                        box_recieve.AppendText("\n" + "Binary:  " + Convert.ToString(Convert.ToInt32(hex, 16), 2).PadLeft(24, '0') + "    Hex:  " + hex);
                    }
                    else {

                        box_recieve.AppendText("\n" + "Binary:  " + Convert.ToString(Convert.ToInt32(hex, 16), 2).PadLeft(24, '0') + "    Hex:  " + hex);
                    }

                }
            }

            //保持lScroll在底部
            scroll_recieve.ScrollToBottom();
        }


        private delegate void SendSting2EventHander(string hex, string dec);
        private void AppendDecHex(int iValue, string hex) {

            this.Dispatcher.BeginInvoke(new SendSting2EventHander(AppendDecHexHander), hex, iValue.ToString());
            //保持lScroll在底部
            scroll_recieve.ScrollToBottom();

        }

        private void AppendDecHexHander(string hex, string iValue)
        {

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
                System.Threading.Thread.Sleep(50);
                if (spManager.bComPortIsOpen) {
                       if (spManager.ByteMode && spManager.receiveArrByteQueue.Count > 0)
                        {
                            byte[] recBuffer = spManager.receiveArrByteQueue.Dequeue();//接收缓冲区  
                            if (bDecodeFlag)
                            {
                                dataBytesArrQueue.Enqueue(recBuffer);//读取数据入列Enqueue（全局）
                            }
                            else
                            {

                                this.Dispatcher.Invoke(new DisplayEventHander(DataTextUpdate), BitConverter.ToString(recBuffer).Replace('-', ' ')+" ");
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
                                this.Dispatcher.Invoke(new DisplayEventHander(DataTextUpdate),new string(charBuffer));
                        }
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
                if (comIsClosing == false)
                {
                    if (dataBytesArrQueue.Count > 0)
                    {
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
                                        hex = BitConverter.ToString(dataByteArr).Replace("-", string.Empty);
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

        //call back
        private delegate void DisplayEventHander(string str);
        private void DataTextUpdate(string str) {
            box_recieve.AppendText(str);
            //保持lScroll在底部
            scroll_recieve.ScrollToBottom();

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
            // ComPort.Close();
            spManager.CloseSerialPort();
            comIsClosing = true;

            this.Dispatcher.BeginInvoke(new SendStringEventHander(AppendStringToLogBox), "is closed.", true);
        }

        private void btn_open_Click(object sender, RoutedEventArgs e)
        {
            comIsClosing = false;
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
        }

        private void btn_text_Click(object sender, RoutedEventArgs e)
        {


        }
        ~MainWindow() {

            spManager = null;
            threadChartWindow.Abort();
            //threadTextWindow.Abort();
        }

        //private bool bTextWindowIsOpen = false;


        private void button_Click(object sender, RoutedEventArgs e)
        {

            spManager.CloseSerialPort();
            comIsClosing = true;
            //if (!bTextWindowIsOpen)
            //{
            //    bTextWindowIsOpen = true;
            //    textWindow.updateSource(getDataQueueSave);
            //    textWindow.ShowDialog();

            //}
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
            Thread.Sleep(1);
        }

        private void checkBox_decode_Unchecked(object sender, RoutedEventArgs e)
        {
            bDecodeFlag = false;
            Thread.Sleep(1);
        }


        private bool bChartWindowIsOpen = false;

        private void btn_chart_Click(object sender, RoutedEventArgs e)
        {
            if (!chartWindow.IsVisible)
            {
                bChartWindowIsOpen = true;
                chartWindow = new ChartWindow();
                // chartWindow.
                // this.window_main.AddChild(chartWindow);
                //chartWindow.Owner = window_main;
                //chartWindow.Show();
                // btn_chart.Content = "Close";
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
                   // AppendStringToLogBox("send Hex: " + sendStringBuffer, true);
                    AppendStringToLogBox("send Hex: " + sendStringBuffer, true);
                    }
                    else
                    {
                        spManager.SendDataPacket(sendStringBuffer);
                       // AppendStringToLogBox("send Hex: " + sendStringBuffer, true);
                        btn_send.Content = "send";
                        sendTimer.Stop();
                    }
                }
                else
                {
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
    }

}

