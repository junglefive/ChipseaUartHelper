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
       // private string[] ports;                       //可用串口数组
        SerialPort ComPort = new SerialPort();
        private bool comIsClosing = false;
        ChartWindow chartWindow = null;
        Thread threadChartWindow = null;
        Thread recData = null;
        Thread threadSerialPortDataRecieved = null;
        //缓存数据
        Queue<int> getDataQueueSave = new Queue<int>();
        SerialPortManager spManager = new SerialPortManager();
        FileStream fs;
        StreamWriter writer;
        StreamReader reader;
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
            //
            radioButton_hex.IsChecked = true;
            //
            comboBox_send.ItemsSource = TextSendList;
            //
            textBox_status_tx.Text = "Tx: " + "0" + " Bytes";
            textBox_status_rx.Text = "Rx: " + "0" + " Bytes";
            textBox_status_com.Text = "";
            //---------
            fs = new FileStream(".\\" + "UserSendLog.list", FileMode.OpenOrCreate);
            writer = new StreamWriter(fs, Encoding.UTF8); //得到文件写对象
            reader = new StreamReader(fs, Encoding.UTF8); //读文件对象
            for (int i = 0; i < 100; i++) {
                string str = reader.ReadLine();
                if(str != null){
                    if (!TextSendList.Contains(str)) {

                        //comboBox_send.Items.Add(str);
                        TextSendList.Add(str);
                    }
            }
            }
            
            reader.Close();
            fs.Close();

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

    


        private void AppendStringToLogBox(string str, bool newLine) {
            //if(box_log)
            if (newLine) {
                box_log.AppendText(DateTime.Now.TimeOfDay.ToString().Substring(0, 11) + ": " + str + "\n");
            }
            else
            {
                box_log.AppendText(DateTime.Now.TimeOfDay.ToString().Substring(0, 11) + ":  " + str );
            }
            

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
        private void updateRxByteCount(int Count) {
            textBox_status_rx.Text = "Rx:  " + Count + " Bytes";

        }

        private volatile Queue dataBytesArrQueue = new Queue();
        private volatile Queue<byte> dataBytesQueue = new Queue<byte>();
        private volatile Queue<int> dataIntQueue = new Queue<int>();
        private volatile byte[] dataArrBuffer = new byte[5];
     //   private String hex;
        private int iValue;
        private delegate void DataIntEventHander(int i);
        private delegate void DataProcessEventHander(string str);
        private delegate void SendStringEventHander(string str, bool newline);
        private int RecieveByteCount = 0;
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
                                                                                       ///////////////////////////////////
                            RecieveByteCount += recBuffer.Length;
                        this.Dispatcher.BeginInvoke(new DataIntEventHander(updateRxByteCount), RecieveByteCount);
                            //////////////////////////////////
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
                //Thread.Sleep(10);
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
                            if (dataBytesQueue.Dequeue() == 0xC5)
                            {//0xAB+0xBA+DataH+DataL+DataLL+Parity
                                if (dataBytesQueue.Dequeue() == 0x03)
                                {
                                    dataByteArr[0] = 0;
                                    dataByteArr[1] = dataBytesQueue.Dequeue();
                                    dataByteArr[2] = dataBytesQueue.Dequeue();
                                    dataByteArr[3] = dataBytesQueue.Dequeue();
                                    if (dataBytesQueue.Dequeue() == (byte)(dataByteArr[1] ^ dataByteArr[2] ^ dataByteArr[3] ^ (byte)0xC5 ^ (byte)0x03))
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
           // User_initiate_serialPort();
           // user_initiate_default();


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
            btn_FFT.IsEnabled = false;
            btn_cmd.IsEnabled = false;
            btn_getXor.IsEnabled = false;
            //打开串口相关设置使能
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
            btn_cmd.IsEnabled = true;
            btn_FFT.IsEnabled = true;
       
            btn_getXor.IsEnabled = true;
            
            if (spManager.bComPortIsOpen) {

                this.Dispatcher.BeginInvoke(new SendStringEventHander(AppendStringToLogBox), "is open.", true);
            }
            else
            {
                try
                {
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
            ClearOherFlag();
            comboBox_sendCount.Text = "0";
            RecieveByteCount = 0;
            SendByteCount = 0;
            textBox_status_tx.Text = "Tx: " + "0" + " Bytes";
            textBox_status_rx.Text = "Rx: " + "0" + " Bytes";
        }

        private void btn_reset_Click(object sender, RoutedEventArgs e)
        {
            box_log.Document.Blocks.Clear();
            box_recieve.Document.Blocks.Clear();

        }
        //btn_clearSendLog_Click
        private void btn_clearSendLog_Click(object sender, RoutedEventArgs e)
        {
            fs = new FileStream(".\\" + "UserSendLog.list", FileMode.Create);
            fs.Close();
            TextSendList.Clear();
            comboBox_send.Items.Refresh();
        }

        private void btn_help_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("https://git.coding.net/junglefive/ChipseaUartHelper.git", "GitHub");
        }
        private void btn_Version_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("v1.2", "版本号");
        }
        private void richTextBox1_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void window_main_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        { if (chartWindow != null) {
                chartWindow.Close();
            }
            fs = new FileStream(".\\" + "UserSendLog.list", FileMode.Create);
            writer = new StreamWriter(fs, Encoding.UTF8); //得到文件写对象
          
            foreach(string str in TextSendList) {
                writer.WriteLine(str);
            }
            writer.Close();
            fs.Close();
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
        /// <summary>
        /// 字符串转16进制字节数组
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public  byte[] strToToHexByte(string hexStr)
        {
                string hexString = hexStr;
                hexString = hexString.Replace(" ", "");
               // if ((hexString.Length % 2) != 0)
                  //  hexString += " ";
                byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                try
                {
                    returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
                }
                catch(Exception ex) {
                    AppendStringToLogBox("Parse Input Hex data Erro", true);
                }
                    
                return returnBytes;

          
        }
        //发送按钮
        private string sendStringBuffer = null;
        ////private bool bSendFlag = true;
        DispatcherTimer sendTimer = null;
        public ArrayList TextSendList = new ArrayList();
        /////private delegate void NoParaDelegateEventHander();
        private int SendByteCount = 0;
        private bool bSendingFlag = false;
        private void btn_send_Click(object sender, RoutedEventArgs e)
        {
            //add 
            //this.Dispatcher.BeginInvoke(new NoParaDelegateEventHander(updatateSendList));
           
            if (spManager.bComPortIsOpen) {
                if (btn_send.Content.Equals("发送中") && bSendTimedFlag)
                {

                    btn_send.Content = "发送数据";
            
                    sendTimer.Stop();
                    AppendStringToLogBox("colse sending", true);
                }
                else {
                    if (bSendingFlag == false) {
                        bSendingFlag = true;
                        sendStringBuffer = comboBox_send.Text;
                        sendTimer = new DispatcherTimer();
                        sendTimer.Tick += new EventHandler(sendTimerHander);
                        sendTimer.Interval = TimeSpan.FromMilliseconds(Convert.ToInt32(textBox_time.Text));
                        sendTimer.Start();
                        btn_send.Content = "发送中";
                    }
                 }
              }
            else
            {
                AppendStringToLogBox("Open SerialPort First.", true);
            }
            ///////////////////////////////////////////////////
            string str = comboBox_send.Text;
            if (!TextSendList.Contains(str)) {
                TextSendList.Add(str);
                comboBox_send.ItemsSource = TextSendList;
                comboBox_send.Items.Refresh();
            }
            //////////////////////////////////////////////////

        }
        private int SendStringCounter = 0;
        private string PreviousSendString;
        private byte[] PreviousSendBytes;
        private void sendTimerHander(object sender, System.EventArgs  e) {
            comboBox_send.Items.Refresh();
            if (B_Auto_Enter_Flag)
            {
                sendStringBuffer = comboBox_send.Text+'\n';
            }
            else
            {
                sendStringBuffer = comboBox_send.Text;
            }
            if (!comIsClosing)
                {
                    if (bSendTimedFlag)
                    {
                        byte[] sendBytesBuffer;
                        SendStringCounter++;
                        comboBox_sendCount.Text = SendStringCounter.ToString();
                           
                        if (spManager.ByteMode){
                        //byte[] sendBytesBuffer = Encoding.Default.GetBytes(sendStringBuffer);
                        if (PreviousSendString==null){
                            sendBytesBuffer = strToToHexByte(sendStringBuffer);
                            PreviousSendBytes = sendBytesBuffer;

                        }

                        else if (PreviousSendString.Equals(sendStringBuffer)&&SendDataAutoPlusFlag)
                        {
                            //sendBytesBuffer = strToToHexByte(sendStringBuffer);
                            //PreviousSendBytes  
                            Byte checksum = 0;
                            //inc lastly 4 bytes
                            PreviousSendBytes[PreviousSendBytes.Length - 2]++;
                            if (PreviousSendBytes[PreviousSendBytes.Length - 2] == 0x00) {
                                PreviousSendBytes[PreviousSendBytes.Length - 3]++;
                                if (PreviousSendBytes[PreviousSendBytes.Length - 3] == 0x00)
                                {
                                    PreviousSendBytes[PreviousSendBytes.Length - 4]++;
                                }
                            }
                            
                            for (int i = 0; i < PreviousSendBytes.Length - 1; i++) {
                                checksum ^= PreviousSendBytes[i];
                            }
                            PreviousSendBytes[PreviousSendBytes.Length - 1] = checksum;
                            sendBytesBuffer = PreviousSendBytes;
                        }
                        else {
                            sendBytesBuffer = strToToHexByte(sendStringBuffer);
                            PreviousSendBytes = sendBytesBuffer;

                        }
                        AppendStringToLogBox("send Hex: " + BitConverter.ToString(sendBytesBuffer), true);
                        spManager.SendDataPacket(sendBytesBuffer);
                        ////////////////////////////////////
                        SendByteCount += sendBytesBuffer.Length;
                        textBox_status_tx.Text = "Tx: " + SendByteCount + " Bytes";
                        /////////////////////////////////////
                        PreviousSendString = sendStringBuffer;
                        }
                        else {
                            spManager.SendDataPacket(sendStringBuffer);
                            AppendStringToLogBox("send String: " + sendStringBuffer, true);
                        }
                    }
                    else
                    {
                        if (spManager.ByteMode)
                        {
                        //  byte[] sendBytesBuffer = Encoding.Default.GetBytes(sendStringBuffer);
                        byte[] sendBytesBuffer = strToToHexByte(sendStringBuffer);
                        AppendStringToLogBox("send Hex: " + BitConverter.ToString(sendBytesBuffer), true);
                            spManager.SendDataPacket(sendBytesBuffer);
                        ////////////////////////////////////
                        SendByteCount += sendBytesBuffer.Length;
                        textBox_status_tx.Text = "Tx: " + SendByteCount + " Bytes";
                        /////////////////////////////////////

                    }
                    else
                        {
                            spManager.SendDataPacket(sendStringBuffer);
                            AppendStringToLogBox("send Stirng: " + sendStringBuffer, true);
                        }
                        btn_send.Content = "发送数据";
                            sendTimer.Stop();
                        }
                }
                else
                {
                    btn_send.Content = "发送数据";
                    sendTimer.Stop();
            }
            bSendingFlag = false;
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

                try {
                    double b = Convert.ToInt32(textBox_time.Text);
                    if (b >= 1)
                    {
                        sendTimer.Interval = TimeSpan.FromMilliseconds(b);
                    }
                    else {
                        b = 1;
                        MessageBox.Show("输入大于1的数字/ms","提示");
                       // sendTimer.Interval = TimeSpan.FromMilliseconds(b);
                    }
                }
                catch {

                }
            }
             
        }
        private bool SendDataAutoPlusFlag = false;
        private void checkBox_AutoPlus_Checked(object sender, RoutedEventArgs e)
        {
            SendDataAutoPlusFlag = true;
            bSendTimedFlag = true;
            checkBox_timed.IsChecked = true;
        }

        private void checkBox_AutoPlus_Unchecked(object sender, RoutedEventArgs e)
        {
            SendDataAutoPlusFlag = false;
        }
        private void ClearOherFlag() {
            SendStringCounter = 0;
            comboBox_sendCount.Clear();

        }

        private void btn_configSerialPort_Click(object sender, RoutedEventArgs e)
        {
            spManager.CloseSerialPort();
            comIsClosing = true;
            btn_close.IsEnabled = false;
            btn_clear.IsEnabled = true;
            btn_send.IsEnabled = false;
            btn_chart.IsEnabled = false;
            btn_FFT.IsEnabled = false;
            btn_cmd.IsEnabled = false;
            btn_getXor.IsEnabled = false;
            InitSerialport initSerialPortWindow = new InitSerialport(ComPort);
            initSerialPortWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            if ((bool)initSerialPortWindow.ShowDialog()) {
                ComPort = initSerialPortWindow.ComPort;
               
            }
            //btn_configSerialPort.Content = ComPort.BaudRate;
           // AppendStringToLogBox("Chosed:  "+ "SerialPort: " + ComPort.PortName + "  " + "Baudrate: " + ComPort.BaudRate + "  " + "Databit: " + ComPort.DataBits + "  " + "Parity: " + ComPort.Parity + "  " + "Stopbit: " + ComPort.StopBits, true);
            btn_open.IsEnabled = true;
            textBox_status_com.Text = "SerialPort: " + ComPort.PortName + "  " + "Baudrate: " + ComPort.BaudRate + "  " + "Databit: " + ComPort.DataBits + "  " + "Parity: " + ComPort.Parity + "  " + "Stopbit: " + ComPort.StopBits;
            //  btn_clear.IsEnabled = true;
        }

        private void btn_FFT_Click(object sender, RoutedEventArgs e)
        {
            try {
                FrequencyFixTestWindow fftWindow = new FrequencyFixTestWindow(spManager);
                fftWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                if (fftWindow != null)
                {
                    fftWindow.Show();
                }
                else {
                    MessageBox.Show("NULL", "Exception");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Exception");
            }
            
        }

        private void btn_cmd_Click(object sender, RoutedEventArgs e)
        {
            spManager.CloseSerialPort();
            UartCmdWindow uartCmdWindow = new UartCmdWindow(spManager, ComPort);
            uartCmdWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            uartCmdWindow.Show();
        }

        private void ScrollViewer_TextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {

        }

        private void comboBox_send_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    
        private void comboBox_send_TextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            comboBox_send.Items.Refresh();
            sendStringBuffer = comboBox_send.Text;
        }

        private void btn_getXor_Click(object sender, RoutedEventArgs e)
        {
            GetXorWindow getXorWindow = new GetXorWindow();
            getXorWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            getXorWindow.Show();
        }
        private void ClearSendLog_Click(object sender, RoutedEventArgs e) {
            try
            {
                fs = new FileStream(".\\" + "UserSendLog.list", FileMode.Truncate);
                writer = new StreamWriter(fs, Encoding.UTF8); //得到文件写对象 
                writer.Close();
                fs.Close();
                TextSendList.Clear();
                TextSendList.Add("C5 01 81 45");
                comboBox_send.Items.Refresh();
            }
            catch {
                MessageBox.Show("Application Erro","Warning");

            }
            


        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        public bool B_Auto_Enter_Flag = false;
        private void checkBox_auto_enter_Checked(object sender, RoutedEventArgs e)
        {
            B_Auto_Enter_Flag = true;
        }

        private void checkBox_auto_enter_Unchecked(object sender, RoutedEventArgs e)
        {
            B_Auto_Enter_Flag = false;
        }


    }

}

