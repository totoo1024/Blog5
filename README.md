# Blog

#  :fire: 注意： **已升级到最新的 .NET 6版本**
## 预览地址
 **[https://www.37czone.com](https://www.37czone.com)** 

 **[后台入口](https://www.37czone.com/Main/Login/Index)** 
后台预览账号/密码：root/123456（请勿修改密码）

#### 介绍
个人博客网站

#### 软件架构
网站已升级到最新的 **.NET6** ，已集成Redis、Autofac、Mapster映射、FluentValidation验证组件（支持自带model验证）、集成极验行为验证、layui开发，由于前端能力有限，所以没有使用前后端分离，博客基本功能已经全部完成（异常日志只记录在了文件中）
相对于[.NET Core 2.2](https://gitee.com/miss_you/Blog/tree/master/)版本简化了，日志组件还没有时间集成，以后有时间会补上，站点已经可以正常运行（已经部署在docker中），点击下方地址即可访问
[博客地址](https://www.37czone.com) https://www.37czone.com
[后台地址](https://[输入链接说明](https://www.37czone.com/main)) https://www.37czone.com/main
后台登录用户名/密码：root/123456
 :grin: 由于博客已经正式上线，所以只开放了预览权限


#### 使用说明

1. 网站使用的sqlsugar ORM开源框架，相关文档请查看官网[sqlsugar框架](https://www.donet5.com/)，数据库使用的是mysql，ORM支持7种数据库（MySql、SqlServer、Sqlite、Oracle、Postgresql、达梦、人大金仓），所以可以随意切换，具体请看sqlsugar官网文档
2. 数据库备份以及脚本放在db目录下，执行任意一项即可，数据库表中仅将所有主键统一成“Id”,项目中后台管理员登录用户名/密码:admin/admin1024
3. 写代码都有详细注释，这里就不一一介绍
4. 创建数据库后记得修改appsettings.json文件中的数据库连接字符串
5. 在Linux部署注意事项，默认是使用的图形验证码，在Linux上部署需要安装相关依赖不然无法正常显示图形验证码，可自行百度解决，图形绘制已经替换为官方的System.Drawing.Common包

6.项目可以选择性使用redis， 默认是没有启用redis（默认使用内置缓存）的和极验验证的，需要启用请先安装redis和注册极验的账号，在appsettings.json文件中更改即可使用（注：极验行为验证免费版只支持滑块验证，还有一些其他限制，个人使用已经足够）
7. 日志模块还没有时间完成，以后有时间会补上，有需要可先参考此博客的另外一个版本
8. 接入QQ授权登录留言评论

如果有什么BUG还希望大家提交到Issues，我看到会及时修复。

#### 开发利器推荐
:point_right: [Resharper](https://www.jetbrains.com/resharper/) https://www.jetbrains.com/resharper/ 

前台预览
![前台预览](https://images.gitee.com/uploads/images/2019/0122/094841_7b096768_967952.png "37℃空间-个人博客.png")

后台预览![后台预览](https://images.gitee.com/uploads/images/2019/0122/095015_2d0d64ad_967952.png "后台管理系统.png")
因为不是经常在线，无法及时回复网友的问题，很多网友想入群交流，特此新建交流群，有需要的可以进群交流，也可以及时回复
![交流群](https://images.gitee.com/uploads/images/2021/1122/151656_044b09d9_967952.png "屏幕截图.png")
