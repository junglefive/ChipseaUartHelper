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
    /// Interaction logic for TextWindow.xaml
    /// </summary>
    public partial class TextWindow : Window
    {
        public TextWindow()
        {
            InitializeComponent();
        }


        private delegate void UpdateTextEventHander(int i);
        public  void updateSource(int idata) {
            this.Dispatcher.Invoke(new UpdateTextEventHander(updateSourceCallBack), idata);
            
        }
        private void updateSourceCallBack(int idata)
        {
            if (richTextBox_hex != null && richTextBox_dec != null)
            {
                richTextBox_hex.AppendText("\n");
                richTextBox_hex.AppendText((DateTime.Now.ToLongTimeString() + "：  " + Convert.ToString(idata, 10)));
                richTextBox_dec.AppendText("\n");
                richTextBox_dec.AppendText(DateTime.Now.ToLongTimeString() + ":   " + Convert.ToString(idata, 16) + "H");

            }

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            richTextBox_dec.Document.Blocks.Clear();
            richTextBox_hex.Document.Blocks.Clear();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            richTextBox_dec = null;
            richTextBox_hex = null;
        }
    }
}
