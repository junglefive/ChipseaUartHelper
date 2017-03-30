
## [ChipseaUartHelper](www.chipsea.com)
#  软件说明
## 功能

1. 串口常规功能，hex或char型发送(可以定时 `Timed` ,单位 `ms` )或接收数据。100Hz/帧以内没问题；
2. 解析数据，支持一个通道数据解析：下位机发送数据格式（1帧）：0xAB+0xBA+ADOH+ADOL+ADOLL+CHECKSUM;CheckSum为ADOH^ADOL^ADOLL（异或值）；
3. 上位机 `Chart` 可以 `Decode` 数据，并且绘图，绘图有多种滤波处理可以选择；并且可以观察滤波结果的峰峰值变化；
4. 数据保存，`ChartWindow` 会自动保存最后一次绘图数据与当前目录（免安装版本），安装版本会保存在安装目录。主窗口可以保存当前解析得到的数据（可以另存为任意目录）; 
5. ...
## 记录
#### 问题（功能）增加20170330 by [junglefive](https://github.com/junglefive/ChipseaUartHelper)
- Q1: 多次Reset会出现偶尔线条异常
- Q2: 考虑在成功打开SeirialPort后，COM配置 isEnable = false.
- Q3: ChartWindow 中，某个异常没有Catch,导致程序偶尔奔溃.
- Q4: ...
## 处理记录
- A1:
- A2: 
- A3: ...




