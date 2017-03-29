using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;


namespace ChipseaUartHelper
{
    public class SerialPortManager
    {
        public bool bComPortIsOpen;
        public bool ByteMode { get; set; }
        public SerialPort CurrentSerialPort { get; set; } = new SerialPort();
        public Queue<byte[]> receiveArrByteQueue { get; } = new Queue<byte[]>();
        public Queue<char[]> receiveArrCharQueue { get; } = new Queue<char[]>();
        public delegate void OnSerialPortMissingHander(object sender, SerialPortMissingEventArgs e); //串口丢失事件触发委托
        public event OnSerialPortMissingHander OnSerialPortMissing;//串口丢失事件委托对象
        /*
         *串口丢失事件触发函数
          **/
        private void OnSerialPortMiss(SerialPortMissingEventArgs e)
        {
            if (this.OnSerialPortMissing != null)
            {
                this.OnSerialPortMissing(this, e);
            }
        }
        /*
         */
        public bool OpenSerialPort(SerialPort serialPortPara)
        {
            CurrentSerialPort = serialPortPara;
            if (bComPortIsOpen == false) //ComPortIsOpen == false当前串口为关闭状态，按钮事件为打开串口
            {
                try //尝试打开串口
                {
                    CurrentSerialPort.ReadTimeout = 8000; //串口读超时8秒
                    CurrentSerialPort.WriteTimeout = 8000; //串口写超时8秒，在1ms自动发送数据时拔掉串口，写超时5秒后，会自动停止发送，如果无超时设定，这时程序假死
                    CurrentSerialPort.ReadBufferSize = 1024; //数据读缓存
                    CurrentSerialPort.WriteBufferSize = 1024; //数据写缓存
                    CurrentSerialPort.DataReceived += ComReceive; //串口接收中断
                    CurrentSerialPort.Open();
                    bComPortIsOpen = true; //串口打开状态字改为true                 
                }
                catch (Exception e) //如果串口被其他占用，则无法打开
                {
                    bComPortIsOpen = false;
                    bReceiveCompleted = false;
                    //throw new Exception("unable open serial port" + exception.Message);
                    OnSerialPortMiss(new SerialPortMissingEventArgs("unable open serial port"));
                }
                return true;
            }
            return true;
        }
        private bool bRecStaus = true;//接收状态字
        private byte[] ReceivedDataPacket { get; set; }
        private char[] ReceivedDataPacketChar { get; set; }
        private bool bReceiveCompleted { get; set; }
        private void ComReceive(object sender, SerialDataReceivedEventArgs e)
        {
            bReceiveCompleted = false;
            if (bRecStaus) //如果已经开启接收
            {
                try
                {
                    Thread.Sleep(50);
                    ReceivedDataPacket = new byte[CurrentSerialPort.BytesToRead];
                    ReceivedDataPacketChar = new char[CurrentSerialPort.BytesToRead];
                    // change to char datas 
                    if (ByteMode)
                    {
                        
                        CurrentSerialPort.Read(ReceivedDataPacket, 0, ReceivedDataPacket.Length);
                        receiveArrByteQueue.Enqueue(ReceivedDataPacket);
                        receiveArrCharQueue.Clear();//清空Char接收缓存
                        //OnSerialPortMiss(new SerialPortMissingEventArgs("test event byte"));
                    }
                    else
                    {
                        CurrentSerialPort.Read(ReceivedDataPacketChar, 0, ReceivedDataPacketChar.Length);
                        receiveArrCharQueue.Enqueue(ReceivedDataPacketChar);
                        receiveArrByteQueue.Clear();//清空Byte接收缓存
                       // OnSerialPortMiss(new SerialPortMissingEventArgs("test event char"));
                    }
                    bReceiveCompleted = true;
                }
                catch (Exception)
                {
                    if (CurrentSerialPort.IsOpen == false) //如果ComPort.IsOpen == false，说明串口已丢失
                    {
                        SetComLose(); //串口丢失后相关设置
                    }
                    else
                    {
                        // throw new Exception("unable to receive data");
                        OnSerialPortMiss(new SerialPortMissingEventArgs("unable close serial port"));
                    }
                }
            }
            else //暂停接收
            {
                CurrentSerialPort.DiscardInBuffer(); //清接收缓存
            }
        }
        public bool SendDataPacket(string dataPacket)
        {
            char[] dataPacketChar = dataPacket.ToCharArray();
            return SendDataPacket(dataPacketChar);
        }

        public bool SendDataPacket(byte[] dataPackeg)
        {
            try
            {
                ByteMode = true;
                CurrentSerialPort.Write(dataPackeg, 0, dataPackeg.Length);

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                OnSerialPortMiss(new SerialPortMissingEventArgs("send exception"));
                return false;
            }
            return true;
        }

        public bool CloseSerialPort()
        {
            try//尝试关闭串口
            {
                CurrentSerialPort.DiscardOutBuffer();//清发送缓存
                CurrentSerialPort.DiscardInBuffer();//清接收缓存
                //WaitClose = true;//激活正在关闭状态字，用于在串口接收方法的invoke里判断是否正在关闭串口
                CurrentSerialPort.Close();//关闭串口
                                          // WaitClose = false;//关闭正在关闭状态字，用于在串口接收方法的invoke里判断是否正在关闭串口
                SetAfterClose();//成功关闭串口或串口丢失后的设置
                bComPortIsOpen = false;
            }
            catch//如果在未关闭串口前，串口就已丢失，这时关闭串口会出现异常
            {
                if (CurrentSerialPort.IsOpen == false)//判断当前串口状态，如果ComPort.IsOpen==false，说明串口已丢失
                {
                    SetComLose();
                }
                else//未知原因，无法关闭串口
                {
                    //throw new Exception("unable close serial port");
                    OnSerialPortMiss(new SerialPortMissingEventArgs("unable close serial port"));
                }
            }
            return true;
        }


        public bool SendDataPacket(char[] senddata)
        {
            try
            {
                ByteMode = false;
                CurrentSerialPort.Write(senddata, 0, senddata.Length);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return false;
            }
            return true;
        }

        private void SetAfterClose()//成功关闭串口或串口丢失后的设置
        {
            bComPortIsOpen = false;//串口状态设置为关闭状态 
        }
        private void SetComLose()//成功关闭串口或串口丢失后的设置
        {
            SetAfterClose();//成功关闭串口或串口丢失后的设置
        }

        //0.定义事件传递的参数
    }
    //0.定义事件传递的参数
    public class SerialPortMissingEventArgs : EventArgs
    {
        public string msg;
        public SerialPortMissingEventArgs(string msgString)
        {
            msg = msgString;
        }
    }
}
