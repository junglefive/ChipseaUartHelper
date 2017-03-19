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

        private bool isClosingFlag = false;
        private Queue<int> iDataQueue = new Queue<int>();
        private delegate void UpdateTextEventHander(Queue<int> DataQueue);
        public  void updateSource(Queue<int> DataQueue) {
            
            this.Dispatcher.Invoke(new UpdateTextEventHander(updateSourceCallBack),DataQueue);
            //updateSourceCallBack(idata);
        


        }
        private void updateSourceCallBack(Queue<int> iDataQueueBuf)
        {
            iDataQueue = iDataQueueBuf;
            if (!isClosingFlag) {
                int idata;
                for (int j = 0; j < iDataQueue.Count; j++) {

                    idata = iDataQueue.Dequeue();
                    if (richTextBox_hex != null && richTextBox_dec != null)
                    {
                            richTextBox_hex.AppendText("\n");
                            richTextBox_hex.AppendText(Convert.ToString(idata, 10));
                        if (richTextBox_dec.Document.Blocks.Count > 500)
                        {

                            richTextBox_dec.Document.Blocks.Remove(richTextBox_dec.Document.Blocks.FirstBlock);
                            richTextBox_hex.Document.Blocks.Remove(richTextBox_hex.Document.Blocks.FirstBlock);
                        }
                        richTextBox_dec.AppendText("\n");
                        richTextBox_dec.AppendText(Convert.ToString(idata, 16) + "H");
                        
                    }
                }

                //滚动条一直显示在底部
                scroll_dec.ScrollToBottom();
                scroll_hex.ScrollToBottom();

            }
               

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            isClosingFlag = true;
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
