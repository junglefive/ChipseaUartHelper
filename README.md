
## [ChipseaUartHelper](www.chipsea.com)
#  软件说明
## 功能

1. 串口常规功能，hex或char型发送(可以定时 `Timed` ,单位 `ms` )或接收数据。100Hz/帧以内没问题；
2. 解析数据，支持一个通道数据解析：C5+03+ADOH+ADOL+ADOLL+CHECKSUM
3. 上位机 `Chart` 可以 `Decode` 数据，并且绘图，绘图有多种滤波处理可以选择；并且可以观察滤波结果的峰峰值变化；
4. 数据保存，`ChartWindow` 会自动保存最后一次绘图数据与当前目录（免安装版本）
5. ...

## 使用

- [需安装framework4.5.2](https://www.microsoft.com/zh-CN/download/details.aspx?id=42642)

## V1.1.3

- 增加多命令循环发送
- 修改主界面布局
- 十六进亦或优化
- 命令串口布局优化

## V1.1.4

 - CMD窗口自动载入OKOK协议数据，带时间戳
 - Main界面允许载入100条记录


## V1.1.5

 - 修复循环发送CMD5显示错误的BUG
 - 修复循环发送Byte[]的BUG

## V1.1.6

 - 增加主界面可伸缩
 - 接收窗口大小可调节

## v1.2
 - @2018/1/14
 - 修改界面
 - 增加字符模式自动增加`\n`
 - 修改界面位中文界面

