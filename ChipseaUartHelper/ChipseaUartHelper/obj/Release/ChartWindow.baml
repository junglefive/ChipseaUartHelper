   M S B A M L   `   `   `  ���� L  HChipseaUartHelper, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:ChipseaUartHelperChipseaUartHelper  #    ChipseaUartHelper.ChartWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     1localclr-namespace:ChipseaUartHelper     Y UDynamicDataDisplay, Version=0.3.0.0, Culture=neutral, PublicKeyToken=5b7d744a7263923f=d34http://research.microsoft.com/DynamicDataDisplay/1.0  -     �Name$  chart_window�� � Title$ ChartWindow��$��419.5��$��1097.833��.��� ���	#FFEAD5D5$��0,0,2,0q� � ColumnDefinitions �� �� $	��165 � � RowDefinitions �� .+�8  2Microsoft.Research.DynamicDataDisplay.ChartPlotter  -   $��plotter�����	#FF6C6868$��5q�$��5q�$��5q�� $��1��$��	7,10,10,5q� �� ��  �� $��35 ��� $��50 ��� $��30 ��� $��34 ��� $��62 ��� $��28 ��� $��27 ��� $��29 ��� $��33 ��� $��38 �.+�� ���	#FFCBC0C0$��0,-1,0,0q�.+�� .+��� -    ��Name$ checkBox_origin��$��Origin�� J� RenderTransformOrigin$ 0.546,0.858&�$��0,6,0,10q�� $��1�����	#FFCFB2B2.+��� -   $ checkBox_shake��$��Shake��$��Center=�$	��Top=�$��0,20,0,0q�$ 
4.789,0.25&�� $��3�����	#FFF3B6B6.+��� -   $ checkBox_moving��$��Moving��$��0,10,0,0q�$	��Top=�$ 3.902,0.375&�� $��4�����	#FFD7DEB6.+��� -   $ checkBox_iir��$	��IIR��$��
0,16.5,0,0q�$	��Top=�� $��2�����	#FFD19E9E.+��� -   $ checkBox_average1��$��Average��$��0,4,0,0q�$	��Top=�$ 3.902,0.375&�$��19�����	#FF72ACACz�V� .~��� �� ��  �� Angle$ -0.547Q�T� �� -    ��Name$ 	btn_pause��$��Pause��$	��Top=�$��21��$��0,10,0,0q�$��9�����	#FFB2AB99�� -	   $ 	btn_reset��$��Reset��$��0,0,0,7q�$��9��$��1�����	#FFB4AD9C$
��Left=�$��74��$��21��$��Bottom=��� -
    ��Name$ textBlock_max��$
��Wrap=�$	��MAX�����	#FFE49B9B$��1��$��5��$��29��$��Center=�	 �� TextAlignment$	 Center=�
 � Cursor$

 None��{�.  �� -   $ textBlock_min��$
��Wrap=�$	��MIN��$��6�����	#FFAD97DC$��1��$��Center=�$��28��$	 Center=��� -   $ textBlock_delta��$��7��$
��Wrap=�$��DELTA�����	#FFC71C1C���	#FFA6CBBF$��1��$��Center=�$��30��$	 Center=��� -    ��Name$ label��$
��MAX:��$��5�����	#FFCF9B9B�� -   $ 
label_Copy��$
��MIN:��$��6�����	#FFA796CB�� -   $ label_Copy1��$��VALUE-D:��$��7�����	#FFA8BFBB� $��1�����	#FFCFC0C0$��0,-1,0,0q�.+��� -    ��Name$ textBox_origin��$��	12,9,12,6q� �� TextWrapping$
 Wrap=�$��0��$ 0.425,0.373&����	#FFB0C7AD
������� �� TextAlignment$ Center=�� $��1�����	#FFD1B2B2$��1�� �� $	��12* ��� $	��13* �.+��� -   $ textBox_shake_threshold��$��	12,0,12,2q�$
 Wrap=�$	��200��$ 0.425,0.373&�$��20��$��Bottom=� �� 	UndoLimit$ 5��$��1��$ Center=�.������  �� EndPoint$
 0,20&� � MappingMode$ Absolute=� �� 
StartPoint$	 0,0&�.���  � Color$ 	#FFABADB3�� � Offset$
 0.05Q�� $ 	#FFE2E3EA��$
 0.07Q�� $ 	#FF8EA4B9��$ 1Q�.���� -   $ textBox_shake_count��$��	12,3,12,0q�$
 Wrap=�$��8��$ 0.425,0.373&�$	 200��$ Center=�.������ $
 0,20&�$ Absolute=�$	 0,0&�.��� $ 	#FFABADB3��$
 0.05Q�� $ 	#FFE2E3EA��$
 0.07Q�� $ 	#FF8A9DB0��$ 1Q�.��� $��1��$��2�����	#FFD69F9F �� .+��� -   $ textBox_average_times��$��	12,6,12,3q�$
 Wrap=�$��4��$ 0.425,0.373&�$ Center=�.������ $
 0,20&�$ Absolute=�$	 0,0&�.��� $ 	#FFABADB3��$
 0.05Q�� $ 	#FFE2E3EA��$
 0.07Q�� $ 	#FF7C8A99��$ 1Q�.��� $��1��$��3�����	#FFF5BFBF.+��� -   $ textBox_moving_length��$��	12,6,12,8q�$
 Wrap=�$��8��$ 0.425,0.373&�$ Center=���.  .������ $
 0,20&�$ Absolute=�$	 0,0&�.��� $ 	#FFABADB3��$
 0.05Q�� $ 	#FFE2E3EA��$
 0.07Q�� $ 	#FF758595��$ 1Q�.��� $��1��$��4�����	#FFD4D1B0 �� �� $��32 �.+��� -   $ textBox_iir_order��$��	12,5,12,0q�$
 Wrap=�$��6��$ 0.425,0.373&�$��23��$	��Top=�$ Center=�.������ $
 0,20&�$ Absolute=�$	 0,0&�.��� $ 	#FFABADB3��$
 0.05Q�� $ 	#FFE2E3EA��$
 0.07Q�� $ 	#FF8496A8��$ 1Q�.���� -   $ textBox_iir_threshold��$��	12,0,12,4q�$
 Wrap=�$��50��$ 0.425,0.373&�$��23��$��1��$��Bottom=�$ Center=�.������ $
 0,20&�$ Absolute=�$	 0,0&�.��� $ 	#FFABADB3��$
 0.05Q�� $ 	#FFE2E3EA��$
 0.07Q�� $ 	#FF748699��$ 1Q�.���� -   $ textBlock_log��$
��Wrap=�$��  �����	#FFD4D1D4$��0,5,0,6q�$��1��$��8��$��Center=�$��22��$	 Center=��� -   $ 
btn_change��$
��Next��$��0,5,0,0q�$��8��$	��Top=����	#FF958F74$��22��