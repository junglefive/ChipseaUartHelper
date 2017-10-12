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
    /// GetXorWindow.xaml 的交互逻辑
    /// </summary>
    public partial class GetXorWindow : Window
    {
        private int i;

        public GetXorWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            txt_input.Clear();
            txt_result.Clear();
            txt_output.Clear();
        }

        private string bytesToString(byte[] bytes) {
            string result = null;

            for (i = 0; i < bytes.Length; i++) {
                result += bytes[i].ToString("x2")+" ";
            }
            return result;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            byte[] arrByte = strToToHexByte(txt_input.Text);
            string strResult = getXor(arrByte).ToString("X2");
            txt_result.Text ="0x"+ strResult;
           txt_output.Text = bytesToString(arrByte) + strResult;
        }
        public byte getXor(byte[] xorBytes) {
            byte result = 0x00;
            foreach (byte tmp in xorBytes) {
                result = (byte)(result ^ tmp);
            }
            return result;
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
                for (int i = 0; i < returnBytes.Length; i++) {
                    returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
                }   
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Parse Input Hex data Erro", "Erro");
                  
                }

            return returnBytes;


        }
    }
}
