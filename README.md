# Luo.Core
个人官网：WWW.LuoCore.com


通过学习 https://github.com/anjoy8/Blog.Core  项目写的一个基础模板
## 原因是 Blog.Core 结构混乱，耦合度高，对于一个新手入门比较有难度，
所以我重构这个项目，欢迎大家多提意见

## 项目架构参考说明
https://www.cnblogs.com/jackyfei/p/12145569.html

## ViewModel 命名
请求VO以Input/Form/Query结尾,如下图的Colorlnput.cs

响应VO以Output/List/Result结尾,如下图的ColorOutput.cs

## 数据仓库命名规则处理操作
创建（Create、Add、Insert

更新（Update、PUT、Write

读取（Retrieve、SELECT、GET 、Read）  

和删除（Delete、Dispose）

## 使用SqlSugar 版本号为：5.0.6.7
https://www.donet5.com/Home/Doc
