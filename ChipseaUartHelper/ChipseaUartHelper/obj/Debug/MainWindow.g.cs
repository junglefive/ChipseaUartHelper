﻿#pragma checksum "..\..\MainWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "A111D24029769162BC7419DB6E7BC4A86A0EA397"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using ChipseaUartHelper;
using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Research.DynamicDataDisplay.Charts;
using Microsoft.Research.DynamicDataDisplay.Charts.Axes;
using Microsoft.Research.DynamicDataDisplay.Charts.Navigation;
using Microsoft.Research.DynamicDataDisplay.Charts.Shapes;
using Microsoft.Research.DynamicDataDisplay.Common.Palettes;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using Microsoft.Research.DynamicDataDisplay.Navigation;
using Microsoft.Research.DynamicDataDisplay.PointMarkers;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace ChipseaUartHelper {
    
    
    /// <summary>
    /// MainWindow
    /// </summary>
    public partial class MainWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 7 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal ChipseaUartHelper.MainWindow window_main;
        
        #line default
        #line hidden
        
        
        #line 46 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ScrollViewer scroll_log;
        
        #line default
        #line hidden
        
        
        #line 47 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RichTextBox box_log;
        
        #line default
        #line hidden
        
        
        #line 57 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ScrollViewer scroll_recieve;
        
        #line default
        #line hidden
        
        
        #line 58 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RichTextBox box_recieve;
        
        #line default
        #line hidden
        
        
        #line 66 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox comboBox_send;
        
        #line default
        #line hidden
        
        
        #line 68 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_open;
        
        #line default
        #line hidden
        
        
        #line 69 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_clear;
        
        #line default
        #line hidden
        
        
        #line 70 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_cmd;
        
        #line default
        #line hidden
        
        
        #line 71 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_FFT;
        
        #line default
        #line hidden
        
        
        #line 72 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_save;
        
        #line default
        #line hidden
        
        
        #line 74 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Menu menu;
        
        #line default
        #line hidden
        
        
        #line 87 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock textBox_status_tx;
        
        #line default
        #line hidden
        
        
        #line 88 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock textBox_status_rx;
        
        #line default
        #line hidden
        
        
        #line 89 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock textBox_status_com;
        
        #line default
        #line hidden
        
        
        #line 95 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton radioButton_ascii;
        
        #line default
        #line hidden
        
        
        #line 96 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton radioButton_hex;
        
        #line default
        #line hidden
        
        
        #line 99 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.GridSplitter gridSplitter3;
        
        #line default
        #line hidden
        
        
        #line 106 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_close;
        
        #line default
        #line hidden
        
        
        #line 107 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_send;
        
        #line default
        #line hidden
        
        
        #line 108 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_getXor;
        
        #line default
        #line hidden
        
        
        #line 109 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_chart;
        
        #line default
        #line hidden
        
        
        #line 119 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox checkBox_AutoPlus;
        
        #line default
        #line hidden
        
        
        #line 120 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox checkBox_auto_enter;
        
        #line default
        #line hidden
        
        
        #line 121 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox checkBox_timed;
        
        #line default
        #line hidden
        
        
        #line 122 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox textBox_time;
        
        #line default
        #line hidden
        
        
        #line 123 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox comboBox_sendCount;
        
        #line default
        #line hidden
        
        
        #line 132 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox checkBox_decode;
        
        #line default
        #line hidden
        
        
        #line 134 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_configSerialPort;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/ChipseaUartHelper;component/mainwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\MainWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.window_main = ((ChipseaUartHelper.MainWindow)(target));
            
            #line 9 "..\..\MainWindow.xaml"
            this.window_main.Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            
            #line 9 "..\..\MainWindow.xaml"
            this.window_main.Closing += new System.ComponentModel.CancelEventHandler(this.window_main_Closing);
            
            #line default
            #line hidden
            return;
            case 2:
            this.scroll_log = ((System.Windows.Controls.ScrollViewer)(target));
            return;
            case 3:
            this.box_log = ((System.Windows.Controls.RichTextBox)(target));
            return;
            case 4:
            this.scroll_recieve = ((System.Windows.Controls.ScrollViewer)(target));
            return;
            case 5:
            this.box_recieve = ((System.Windows.Controls.RichTextBox)(target));
            return;
            case 6:
            
            #line 65 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.ScrollViewer)(target)).TextInput += new System.Windows.Input.TextCompositionEventHandler(this.ScrollViewer_TextInput);
            
            #line default
            #line hidden
            return;
            case 7:
            this.comboBox_send = ((System.Windows.Controls.ComboBox)(target));
            
            #line 66 "..\..\MainWindow.xaml"
            this.comboBox_send.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.comboBox_send_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 8:
            this.btn_open = ((System.Windows.Controls.Button)(target));
            
            #line 68 "..\..\MainWindow.xaml"
            this.btn_open.Click += new System.Windows.RoutedEventHandler(this.btn_open_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.btn_clear = ((System.Windows.Controls.Button)(target));
            
            #line 69 "..\..\MainWindow.xaml"
            this.btn_clear.Click += new System.Windows.RoutedEventHandler(this.btn_clear_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            this.btn_cmd = ((System.Windows.Controls.Button)(target));
            
            #line 70 "..\..\MainWindow.xaml"
            this.btn_cmd.Click += new System.Windows.RoutedEventHandler(this.btn_cmd_Click);
            
            #line default
            #line hidden
            return;
            case 11:
            this.btn_FFT = ((System.Windows.Controls.Button)(target));
            
            #line 71 "..\..\MainWindow.xaml"
            this.btn_FFT.Click += new System.Windows.RoutedEventHandler(this.btn_FFT_Click);
            
            #line default
            #line hidden
            return;
            case 12:
            this.btn_save = ((System.Windows.Controls.Button)(target));
            
            #line 72 "..\..\MainWindow.xaml"
            this.btn_save.Click += new System.Windows.RoutedEventHandler(this.btn_save_Click);
            
            #line default
            #line hidden
            return;
            case 13:
            this.menu = ((System.Windows.Controls.Menu)(target));
            return;
            case 14:
            
            #line 76 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.btn_clearSendLog_Click);
            
            #line default
            #line hidden
            return;
            case 15:
            
            #line 77 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.btn_getXor_Click);
            
            #line default
            #line hidden
            return;
            case 16:
            
            #line 80 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.MenuItem_Click);
            
            #line default
            #line hidden
            return;
            case 17:
            
            #line 81 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.btn_Version_Click);
            
            #line default
            #line hidden
            return;
            case 18:
            
            #line 82 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.btn_help_Click);
            
            #line default
            #line hidden
            return;
            case 19:
            this.textBox_status_tx = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 20:
            this.textBox_status_rx = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 21:
            this.textBox_status_com = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 22:
            this.radioButton_ascii = ((System.Windows.Controls.RadioButton)(target));
            
            #line 95 "..\..\MainWindow.xaml"
            this.radioButton_ascii.Checked += new System.Windows.RoutedEventHandler(this.radioButton_ascii_Checked);
            
            #line default
            #line hidden
            return;
            case 23:
            this.radioButton_hex = ((System.Windows.Controls.RadioButton)(target));
            
            #line 96 "..\..\MainWindow.xaml"
            this.radioButton_hex.Checked += new System.Windows.RoutedEventHandler(this.radioButton_hex_Checked);
            
            #line default
            #line hidden
            return;
            case 24:
            this.gridSplitter3 = ((System.Windows.Controls.GridSplitter)(target));
            return;
            case 25:
            this.btn_close = ((System.Windows.Controls.Button)(target));
            
            #line 106 "..\..\MainWindow.xaml"
            this.btn_close.Click += new System.Windows.RoutedEventHandler(this.btn_close_Click);
            
            #line default
            #line hidden
            return;
            case 26:
            this.btn_send = ((System.Windows.Controls.Button)(target));
            
            #line 107 "..\..\MainWindow.xaml"
            this.btn_send.Click += new System.Windows.RoutedEventHandler(this.btn_send_Click);
            
            #line default
            #line hidden
            return;
            case 27:
            this.btn_getXor = ((System.Windows.Controls.Button)(target));
            
            #line 108 "..\..\MainWindow.xaml"
            this.btn_getXor.Click += new System.Windows.RoutedEventHandler(this.btn_getXor_Click);
            
            #line default
            #line hidden
            return;
            case 28:
            this.btn_chart = ((System.Windows.Controls.Button)(target));
            
            #line 109 "..\..\MainWindow.xaml"
            this.btn_chart.Click += new System.Windows.RoutedEventHandler(this.btn_chart_Click);
            
            #line default
            #line hidden
            return;
            case 29:
            this.checkBox_AutoPlus = ((System.Windows.Controls.CheckBox)(target));
            
            #line 119 "..\..\MainWindow.xaml"
            this.checkBox_AutoPlus.Checked += new System.Windows.RoutedEventHandler(this.checkBox_AutoPlus_Checked);
            
            #line default
            #line hidden
            
            #line 119 "..\..\MainWindow.xaml"
            this.checkBox_AutoPlus.Unchecked += new System.Windows.RoutedEventHandler(this.checkBox_AutoPlus_Unchecked);
            
            #line default
            #line hidden
            return;
            case 30:
            this.checkBox_auto_enter = ((System.Windows.Controls.CheckBox)(target));
            
            #line 120 "..\..\MainWindow.xaml"
            this.checkBox_auto_enter.Unchecked += new System.Windows.RoutedEventHandler(this.checkBox_auto_enter_Unchecked);
            
            #line default
            #line hidden
            
            #line 120 "..\..\MainWindow.xaml"
            this.checkBox_auto_enter.Checked += new System.Windows.RoutedEventHandler(this.checkBox_auto_enter_Checked);
            
            #line default
            #line hidden
            return;
            case 31:
            this.checkBox_timed = ((System.Windows.Controls.CheckBox)(target));
            
            #line 121 "..\..\MainWindow.xaml"
            this.checkBox_timed.Checked += new System.Windows.RoutedEventHandler(this.checkBox_timed_Checked);
            
            #line default
            #line hidden
            
            #line 121 "..\..\MainWindow.xaml"
            this.checkBox_timed.Unchecked += new System.Windows.RoutedEventHandler(this.checkBox_timed_Unchecked);
            
            #line default
            #line hidden
            return;
            case 32:
            this.textBox_time = ((System.Windows.Controls.TextBox)(target));
            
            #line 122 "..\..\MainWindow.xaml"
            this.textBox_time.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.textBox_time_TextChanged);
            
            #line default
            #line hidden
            return;
            case 33:
            this.comboBox_sendCount = ((System.Windows.Controls.TextBox)(target));
            
            #line 123 "..\..\MainWindow.xaml"
            this.comboBox_sendCount.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.textBox_time_TextChanged);
            
            #line default
            #line hidden
            return;
            case 34:
            this.checkBox_decode = ((System.Windows.Controls.CheckBox)(target));
            
            #line 132 "..\..\MainWindow.xaml"
            this.checkBox_decode.Checked += new System.Windows.RoutedEventHandler(this.checkBox_decode_Checked);
            
            #line default
            #line hidden
            
            #line 132 "..\..\MainWindow.xaml"
            this.checkBox_decode.Unchecked += new System.Windows.RoutedEventHandler(this.checkBox_decode_Unchecked);
            
            #line default
            #line hidden
            return;
            case 35:
            this.btn_configSerialPort = ((System.Windows.Controls.Button)(target));
            
            #line 134 "..\..\MainWindow.xaml"
            this.btn_configSerialPort.Click += new System.Windows.RoutedEventHandler(this.btn_configSerialPort_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
