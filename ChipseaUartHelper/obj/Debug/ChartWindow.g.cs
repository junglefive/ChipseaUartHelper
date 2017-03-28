﻿#pragma checksum "..\..\ChartWindow.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "C666C46A6915F1CE9FAAC0D42CB320BA"
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
using D3PaletteControl;
using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Research.DynamicDataDisplay.Charts;
using Microsoft.Research.DynamicDataDisplay.Charts.Axes;
using Microsoft.Research.DynamicDataDisplay.Charts.Maps;
using Microsoft.Research.DynamicDataDisplay.Charts.Maps.Network;
using Microsoft.Research.DynamicDataDisplay.Charts.Navigation;
using Microsoft.Research.DynamicDataDisplay.Charts.Shapes;
using Microsoft.Research.DynamicDataDisplay.Common.Palettes;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using Microsoft.Research.DynamicDataDisplay.Maps.Servers;
using Microsoft.Research.DynamicDataDisplay.Maps.Servers.FileServers;
using Microsoft.Research.DynamicDataDisplay.Maps.Servers.Network;
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
    /// ChartWindow
    /// </summary>
    public partial class ChartWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 7 "..\..\ChartWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal ChipseaUartHelper.ChartWindow chart_window;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\ChartWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Microsoft.Research.DynamicDataDisplay.ChartPlotter plotter;
        
        #line default
        #line hidden
        
        
        #line 38 "..\..\ChartWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox checkBox_origin;
        
        #line default
        #line hidden
        
        
        #line 42 "..\..\ChartWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox checkBox_shake;
        
        #line default
        #line hidden
        
        
        #line 45 "..\..\ChartWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox checkBox_moving;
        
        #line default
        #line hidden
        
        
        #line 48 "..\..\ChartWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox checkBox_iir;
        
        #line default
        #line hidden
        
        
        #line 51 "..\..\ChartWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox checkBox_average1;
        
        #line default
        #line hidden
        
        
        #line 62 "..\..\ChartWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_pause;
        
        #line default
        #line hidden
        
        
        #line 63 "..\..\ChartWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_reset;
        
        #line default
        #line hidden
        
        
        #line 64 "..\..\ChartWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock textBlock_max;
        
        #line default
        #line hidden
        
        
        #line 65 "..\..\ChartWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock textBlock_min;
        
        #line default
        #line hidden
        
        
        #line 66 "..\..\ChartWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock textBlock_delta;
        
        #line default
        #line hidden
        
        
        #line 67 "..\..\ChartWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label label;
        
        #line default
        #line hidden
        
        
        #line 68 "..\..\ChartWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label label_Copy;
        
        #line default
        #line hidden
        
        
        #line 69 "..\..\ChartWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label label_Copy1;
        
        #line default
        #line hidden
        
        
        #line 71 "..\..\ChartWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox textBox_origin;
        
        #line default
        #line hidden
        
        
        #line 78 "..\..\ChartWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox textBox_shake_threshold;
        
        #line default
        #line hidden
        
        
        #line 87 "..\..\ChartWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox textBox_shake_count;
        
        #line default
        #line hidden
        
        
        #line 101 "..\..\ChartWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox textBox_average_times;
        
        #line default
        #line hidden
        
        
        #line 112 "..\..\ChartWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox textBox_moving_length;
        
        #line default
        #line hidden
        
        
        #line 127 "..\..\ChartWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox textBox_iir_order;
        
        #line default
        #line hidden
        
        
        #line 136 "..\..\ChartWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox textBox_iir_threshold;
        
        #line default
        #line hidden
        
        
        #line 146 "..\..\ChartWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock textBlock_log;
        
        #line default
        #line hidden
        
        
        #line 147 "..\..\ChartWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_change;
        
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
            System.Uri resourceLocater = new System.Uri("/ChipseaUartHelper;component/chartwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\ChartWindow.xaml"
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
            this.chart_window = ((ChipseaUartHelper.ChartWindow)(target));
            
            #line 9 "..\..\ChartWindow.xaml"
            this.chart_window.Loaded += new System.Windows.RoutedEventHandler(this.chart_window_Loaded);
            
            #line default
            #line hidden
            
            #line 9 "..\..\ChartWindow.xaml"
            this.chart_window.Closing += new System.ComponentModel.CancelEventHandler(this.chart_window_Closing);
            
            #line default
            #line hidden
            
            #line 9 "..\..\ChartWindow.xaml"
            this.chart_window.Closed += new System.EventHandler(this.chart_window_Closed);
            
            #line default
            #line hidden
            return;
            case 2:
            this.plotter = ((Microsoft.Research.DynamicDataDisplay.ChartPlotter)(target));
            return;
            case 3:
            this.checkBox_origin = ((System.Windows.Controls.CheckBox)(target));
            
            #line 38 "..\..\ChartWindow.xaml"
            this.checkBox_origin.Checked += new System.Windows.RoutedEventHandler(this.checkBox_origin_Checked);
            
            #line default
            #line hidden
            
            #line 38 "..\..\ChartWindow.xaml"
            this.checkBox_origin.Unchecked += new System.Windows.RoutedEventHandler(this.checkBox_origin_Unchecked);
            
            #line default
            #line hidden
            return;
            case 4:
            this.checkBox_shake = ((System.Windows.Controls.CheckBox)(target));
            
            #line 42 "..\..\ChartWindow.xaml"
            this.checkBox_shake.Checked += new System.Windows.RoutedEventHandler(this.checkBox_shake_Checked);
            
            #line default
            #line hidden
            
            #line 42 "..\..\ChartWindow.xaml"
            this.checkBox_shake.Unchecked += new System.Windows.RoutedEventHandler(this.checkBox_shake_Unchecked);
            
            #line default
            #line hidden
            return;
            case 5:
            this.checkBox_moving = ((System.Windows.Controls.CheckBox)(target));
            
            #line 45 "..\..\ChartWindow.xaml"
            this.checkBox_moving.Checked += new System.Windows.RoutedEventHandler(this.checkBox_moving_Checked);
            
            #line default
            #line hidden
            
            #line 45 "..\..\ChartWindow.xaml"
            this.checkBox_moving.Unchecked += new System.Windows.RoutedEventHandler(this.checkBox_moving_Unchecked);
            
            #line default
            #line hidden
            return;
            case 6:
            this.checkBox_iir = ((System.Windows.Controls.CheckBox)(target));
            
            #line 48 "..\..\ChartWindow.xaml"
            this.checkBox_iir.Checked += new System.Windows.RoutedEventHandler(this.checkBox_iir_Checked);
            
            #line default
            #line hidden
            
            #line 48 "..\..\ChartWindow.xaml"
            this.checkBox_iir.Unchecked += new System.Windows.RoutedEventHandler(this.checkBox_iir_Unchecked);
            
            #line default
            #line hidden
            return;
            case 7:
            this.checkBox_average1 = ((System.Windows.Controls.CheckBox)(target));
            
            #line 51 "..\..\ChartWindow.xaml"
            this.checkBox_average1.Checked += new System.Windows.RoutedEventHandler(this.checkBox_average_Checked);
            
            #line default
            #line hidden
            
            #line 51 "..\..\ChartWindow.xaml"
            this.checkBox_average1.Unchecked += new System.Windows.RoutedEventHandler(this.checkBox_average_Unchecked);
            
            #line default
            #line hidden
            return;
            case 8:
            this.btn_pause = ((System.Windows.Controls.Button)(target));
            
            #line 62 "..\..\ChartWindow.xaml"
            this.btn_pause.Click += new System.Windows.RoutedEventHandler(this.btn_pause_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.btn_reset = ((System.Windows.Controls.Button)(target));
            
            #line 63 "..\..\ChartWindow.xaml"
            this.btn_reset.Click += new System.Windows.RoutedEventHandler(this.btn_reset_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            this.textBlock_max = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 11:
            this.textBlock_min = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 12:
            this.textBlock_delta = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 13:
            this.label = ((System.Windows.Controls.Label)(target));
            return;
            case 14:
            this.label_Copy = ((System.Windows.Controls.Label)(target));
            return;
            case 15:
            this.label_Copy1 = ((System.Windows.Controls.Label)(target));
            return;
            case 16:
            this.textBox_origin = ((System.Windows.Controls.TextBox)(target));
            
            #line 71 "..\..\ChartWindow.xaml"
            this.textBox_origin.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.textBox_origin_DataContextChanged);
            
            #line default
            #line hidden
            return;
            case 17:
            this.textBox_shake_threshold = ((System.Windows.Controls.TextBox)(target));
            
            #line 78 "..\..\ChartWindow.xaml"
            this.textBox_shake_threshold.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.textBox_shake_threshold_DataContextChanged);
            
            #line default
            #line hidden
            return;
            case 18:
            this.textBox_shake_count = ((System.Windows.Controls.TextBox)(target));
            
            #line 87 "..\..\ChartWindow.xaml"
            this.textBox_shake_count.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.textBox_shake_count_DataContextChanged);
            
            #line default
            #line hidden
            return;
            case 19:
            this.textBox_average_times = ((System.Windows.Controls.TextBox)(target));
            
            #line 101 "..\..\ChartWindow.xaml"
            this.textBox_average_times.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.textBox_average_times_DataContextChanged);
            
            #line default
            #line hidden
            return;
            case 20:
            this.textBox_moving_length = ((System.Windows.Controls.TextBox)(target));
            
            #line 112 "..\..\ChartWindow.xaml"
            this.textBox_moving_length.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.textBox_moving_length_DataContextChanged);
            
            #line default
            #line hidden
            return;
            case 21:
            this.textBox_iir_order = ((System.Windows.Controls.TextBox)(target));
            
            #line 127 "..\..\ChartWindow.xaml"
            this.textBox_iir_order.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.textBox_iir_order_DataContextChanged);
            
            #line default
            #line hidden
            return;
            case 22:
            this.textBox_iir_threshold = ((System.Windows.Controls.TextBox)(target));
            
            #line 136 "..\..\ChartWindow.xaml"
            this.textBox_iir_threshold.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.textBox_iir_threshold_DataContextChanged);
            
            #line default
            #line hidden
            return;
            case 23:
            this.textBlock_log = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 24:
            this.btn_change = ((System.Windows.Controls.Button)(target));
            
            #line 147 "..\..\ChartWindow.xaml"
            this.btn_change.Click += new System.Windows.RoutedEventHandler(this.btn_change_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

