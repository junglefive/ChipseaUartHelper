   M S B A M L   `   `   `  ���� L  HChipseaUartHelper, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:ChipseaUartHelperChipseaUartHelper  $    ChipseaUartHelper.GetXorWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     1localclr-namespace:ChipseaUartHelper       � Title$  GetXorWindow��$��197.334��$��659.751�� � 
ResizeMode$ NoResize=� � ShowInTaskbar .  .���  � RowDefinitions �� �� $��0* �.+�� ���	#FFFBF5F5 � ColumnDefinitions �� $	��22* ��� $
��207* ��� $
��218* ��� $
��192* ��� $	��16* � �� $��5* ��� $��8* ��� $	��45* ��� $	��55* ��� $	��48* ��� $��9* �.+��� -    ��Name$ 	txt_input��$��40��$��0,4,4,0q�$��2�� �� TextWrapping$
 Wrap=�$	��Top=�$��Center=����	#FF97B0FC$��3��$��1��$��1q��� -   $ 
txt_result��$
��Left=�$��36��$��
0,10.5,0,0q�$
 Wrap=�$��Result��$	��Top=�$	��176��$��Center=�$��Center=����	#FFB3ADAB$��3��$��1�����	#FFF93509$��1q� �� TextDecorations Z� �� -   $��Clear��$��Right=�$	��Top=�$	��100�� J� RenderTransformOrigin$ 0.131,1.122&����	#FFF9C0C0$��36��$��0,10.5,61.5,0q�$��3��$��2���� -   $��Bitxor��$��Right=�$	��Top=�$	��156��$ 0.131,1.122&����	#FFF9C0C0$��36��$��
0,10.5,4,0q�$��3��$��3���� -   $ 
txt_output��$��40��$��0,3,4,0q�$��4��$
 Wrap=�$	��Top=�$��Center=����	#FF97B0FC$��3��$��1q�$��1��