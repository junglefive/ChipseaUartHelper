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
using System.Windows.Shapes;

namespace ChipseaUartHelper
{
    /// <summary>
    /// FrequencyFixTestWindow.xaml 的交互逻辑
    /// </summary>
    public partial class FrequencyFixTestWindow : Window
    {
        SerialPortManager spManager;
        private delegate void UpdateEventHander();
        public FrequencyFixTestWindow(SerialPortManager sp)
        {
           
            InitializeComponent();
            spManager = sp;
        }

        byte channel = 0;
        private void radioButton_Checked(object sender, RoutedEventArgs e)
        {
            
            RadioButton radiobtn = (RadioButton)sender;
            try
            {
                channel = byte.Parse(radiobtn.Tag.ToString());
                
            }
            catch (Exception ex) {

                MessageBox.Show(ex.ToString(), "Erro");
            }
            this.Dispatcher.BeginInvoke(new UpdateEventHander(updateChannel));
        }
        byte[] sendBytes = { 0xC5, 0x02, 0xFF, 0x00, 0x00 }; 
        private void button_send_Click(object sender, RoutedEventArgs e)
        {
            sendBytes[0] = 0xC5;
            sendBytes[1] = 0x02;
            sendBytes[2] = 0xFF;
            sendBytes[3] = channel;
            sendBytes[4] = (byte)(sendBytes[0] ^ sendBytes[1] ^ sendBytes[2] ^ sendBytes[3]);
            //send
            spManager.SendDataPacket(sendBytes);
            this.Dispatcher.BeginInvoke(new UpdateEventHander(updatelog));    
            
        }
        void updateChannel() {
            textBlock.Text = "chosed: " + (2402 + 2 * channel +"    ");
        }
        void updatelog() {
            textBlock.Text += "sending: "+ BitConverter.ToString(sendBytes)+"    ";
            scroll_log.ScrollToRightEnd();
        }

        private void button_reset_Click(object sender, RoutedEventArgs e)
        {
            button_reset.Content = "HEHE";

        }
    }
}
