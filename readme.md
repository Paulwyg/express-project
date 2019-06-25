## 数字化城市照明监控管理系统

> 城市照明监控系统，与相应的硬件系统相配合，实现了城市灯光照明系统的实时监测、实时控制。该系统是一个以中央控制室为核心，以分站控制箱为基础，以通讯设备和网络为纽带，以先进的计算机技术为保障的程序化、模块化、网络化、智能化的整体控制系统。其主要特点是智能化程度高、人工干涉环节少、实时性和安全性好、操作简单方便、记录的各种历史数据完备。系统功能涵盖了测量、控制、数据采集、传输、安全监视、通讯、设备管理、路灯资料实时动态、智能化管理等方方面面；实现了城市灯光的遥控、遥测、遥信等功能；可以对灯光系统的开关状态、报警状态、亮灯率、电流和电压等数据进行采集和监视；并对灯光系统进行远程的实时远程控制，而实时掌握照明系统运行状况，快速发现路灯故障、盗窃等并能主动报警，确保照明系统的可靠运行，提高路灯运行质量。

#### 开发环境

<table border="1px" >
    <tr>
        <th>序号</th>
        <th>软件</th>
        <th>版本号</th>
        <th>下载地址</th>
        <th>版权</th>
    </tr>
    <tr>
        <td>1</td>
        <td>Microsoft Visual Studio 2010</td>
        <td>10.0.40219.1 SP1Rel</td>
        <td>https://visualstudio.microsoft.com/zh-hans/</td>
        <td>需要授权</td>
    </tr>
    <tr>
        <td>2</td>
        <td>ArcGIS Runtime for WPF</td>
        <td>10.2</td>
        <td>https://developers.arcgis.com/downloads/</td>
        <td>需要授权</td>
    </tr>
    <tr>
        <td>3</td>
        <td>Telerik UI for WPF</td>
        <td>2015.3.2015.40</td>
        <td>https://www.telerik.com</td>
        <td>需要授权</td>
    </tr>
</table>

#### 安装运行

##### 编译
使用Microsoft Visual Studio编译程序。
>默认编译输出文件名为CETC50.exe

##### 运行
将编译后动态链接库文件（*.dll） 复制到项目发布的 bin 目录中即可。

#### 其他说明

- 运行日志存放在 bin 父目录下的 log 文件夹中。
