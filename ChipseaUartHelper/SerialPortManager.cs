using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;


namespace ChipseaUartHelper
{
    public class SerialPortManager
    {
        private bool bRecStaus = true;//接收状态字
        public bool bComPortIsOpen;
        private void SetAfterClose()//成功关闭串口或串口丢失后的设置
        {
            bComPortIsOpen = false;//串口状态设置为关闭状态 
        }
        private void SetComLose()//成功关闭串口或串口丢失后的设置
        {
            SetAfterClose();//成功关闭串口或串口丢失后的设置
        }
        public SerialPort CurrentSerialPort { get; set; } = new SerialPort();
        private byte[] ReceivedDataPacket { get; set; }
        public Queue<byte[]> receiveArrByteQueue { get; } = new Queue<byte[]>();
        public Queue<char[]> receiveArrCharQueue { get; } = new Queue<char[]>();
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
                catch (Exception exception) //如果串口被其他占用，则无法打开
                {
                    bComPortIsOpen = false;
                    bReceiveCompleted = false;
                    throw new Exception("unable open serial port" + exception.Message);
                }
                return true;
            }
            return true;
        }

        public char[] ReceivedDataPacketChar { get; set; }
        public bool bReceiveCompleted { get; set; }
        public bool bReceiveException { get; set; }
        private void ComReceive(object sender, SerialDataReceivedEventArgs e)
        {
            bReceiveCompleted = false;
            bReceiveException = false;
            if (bRecStaus) //如果已经开启接收
            {
                try
                {
                    Thread.Sleep(20);
                    ReceivedDataPacket = new byte[CurrentSerialPort.BytesToRead];
                    ReceivedDataPacketChar = new char[CurrentSerialPort.BytesToRead];
                    // change to char datas 
                    if (ByteMode)
                    {
                        CurrentSerialPort.Read(ReceivedDataPacket, 0, ReceivedDataPacket.Length);
                        receiveArrByteQueue.Enqueue(ReceivedDataPacket);
                        receiveArrCharQueue.Clear();//清空Char接收缓存
                    }
                    else
                    {
                        CurrentSerialPort.Read(ReceivedDataPacketChar, 0, CurrentSerialPort.BytesToRead);
                        receiveArrCharQueue.Enqueue(ReceivedDataPacketChar);
                        receiveArrByteQueue.Clear();//清空Byte接收缓存
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
                        bReceiveException = true;
                       // throw new Exception("unable to receive data");
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
                    throw new Exception("unable close serial port");
                }
            }
            return true;
        }

        public bool ByteMode { get; set; }

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
    }
}
