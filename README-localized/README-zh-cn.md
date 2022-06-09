---
page_type: sample
products:
- office-outlook
- office-365
languages:
- javascript
description: 本示例介绍了如何使用 Exchange 服务器中的客户端标识令牌，对 Outlook 邮件外接程序用户进行身份验证。
extensions:
  contentType: samples
  technologies:
  - Add-ins
  createdDate: 8/14/2015 12:52:42 PM
---
# Outlook 外接程序：使用客户端标识令牌

**目录**

* [摘要](#summary)
* [先决条件](#prerequisites)
* [示例主要组件](#components)
* [代码说明](#codedescription)
* [构建和调试](#build)
* [疑难解答](#troubleshooting)
* [问题和意见](#questions)
* [其他资源](#additional-resources)

<a name="summary"></a>
## 摘要
本示例介绍了如何使用 Exchange 服务器中的客户端标识令牌，对 Outlook 邮件外接程序用户进行身份验证。 

<a name="prerequisites"></a>
## 先决条件 ##

此示例要求如下：  

  - Visual Studio 2013（更新 5）或 Visual Studio 2015，具有 Microsoft Office 开发人员工具。 
  - 运行至少具有一个电子邮件帐户或 Office 365 帐户的 Exchange 2013 的计算机。你可以[参加 Office 365 开发人员计划并获取为期 1 年的免费 Office 365 订阅](https://aka.ms/devprogramsignup)。
  - 任何支持 ECMAScript 5.1、HTML5 和 CSS3 的浏览器，如 Internet Explorer 9、Chrome 13、Firefox 5、Safari 5.0.6 以及这些浏览器的更高版本。
  - Microsoft.Exchange.WebServices.Auth.dll、Microsoft.IdentityModel.dll 和 Microsoft.IdentityModel.Extensions.dll。可从程序包管理器控制台安装所需程序包（**工具 > NuGet 包管理器 > 程序包管理器控制台**）：- 安装程序包 EWS-Api-2.1 
	- 安装程序包 Microsoft.IdentityModel  
	- 安装程序包 Microsoft.Identity.Model.Extensions  
  - [ASP.NET MVC 4](http://www.asp.net/mvc/mvc4).
  - 熟悉 JavaScript 编程和 Web 服务。

<a name="components"></a>
## 示例主要组件
本示例解决方案包含以下主要文件：

**UseIdentityToken** 项目

- [```UseIdentityToken.xml```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-UseIdentityToken/blob/master/UseIdentityToken/UseIdentityTokenManifest/UseIdentityToken.xml)：Outlook 邮件外接程序的清单文件。

**UseIdentityTokenWeb** 项目

- [```AppRead/Home/Home.html```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-UseIdentityToken/blob/master/UseIdentityTokenWeb/AppRead/Home/Home.html)：外接程序的 HTML 用户界面。
- [```AppRead/Home/Home.js```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-UseIdentityToken/blob/master/UseIdentityTokenWeb/AppRead/Home/Home.js)：用于处理请求和使用标识令牌的逻辑。

**UseIdentityTokenService** 项目

- [```App_Start/WebApiConfig.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-UseIdentityToken/blob/master/UseIdentityTokenService/App_Start/WebApiConfig.cs)：为 Web API 服务绑定默认路由。
- [```Controllers/IdentityTokenController.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-UseIdentityToken/blob/master/UseIdentityTokenService/Controllers/IdentityTokenController.cs)：为示例 Web API 服务提供业务逻辑的服务对象。
- [```Models/ServiceRequest.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-UseIdentityToken/blob/master/UseIdentityTokenService/Models/ServiceRequest.cs)：表示 Web 请求的对象。对象的内容通过从外接程序发送的 JSON 请求对象创建。
- [```Models/ServiceResponse.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-UseIdentityToken/blob/master/UseIdentityTokenService/Models/ServiceResponse.cs)：表示 Web 服务响应的对象。将对象的内容发送回外接程序时，会将其序列化为 JSON 对象。

<a name="codedescription"></a>
##代码描述 本示例介绍了如何使用 Exchange 服务器中的客户端标识令牌，对邮件外接程序用户进行身份验证。
Exchange 服务器签发一个对服务器上的邮箱具有唯一性的令牌。
你可以使用此令牌将邮箱与为邮件外接程序提供的服务关联起来。

本示例分为两个部分：  
- 在电子邮件客户端运行的 Outlook 邮件外接程序。它需要来自 Exchange 服务器中的标识令牌，并将此令牌发送至 Web 服务。
- Web 服务用于处理来自客户端的请求。

Web 服务使用以下步骤处理令牌：

- 验证令牌，确保它是从 Exchange 服务器发送的以及令牌专用于此邮件外接程序。
- 搜索本地字典，以确定唯一的标识符以前是否使用过。如果唯一标识符未被使用过，则服务将要求来自客户端的凭据（服务用户名和密码）。如果唯一标识符已存在于令牌缓存中，则服务将发送响应。
- 如果请求包含凭据（即，它是凭据请求的响应），则服务会将服务用户名存储于令牌缓存中并将令牌中的唯一标识符另存为其密钥。

本示例不会以任何方式验证服务用户名和密码。如果凭据请求包含用户名和密码，则该请求将被视为有效。本示例缓存中的凭据不会过期；但是，在停止运行示例应用程序时，所有缓存的标识符和用户名均会丢失。

本示例需要 Exchange 服务器上的有效服务器证书。如果 Exchange 服务器使用默认的自签名证书，则你需要将此证书添加到本地受信任的证书存储区域。你可以在 TechNet 上查找[导出和安装自签名证书的说明](http://social.technet.microsoft.com/wiki/contents/articles/13898.how-to-export-a-self-signed-server-certificate-and-import-it-on-a-another-server-in-windows-server-2008-r2.aspx)。


<a name="build"></a>
## 构建和调试 ##
用户收件箱中的任何电子邮件均会激活外接程序。在运行本示例之前，可以向测试帐户发送一封或多封电子邮件，以此更轻松地测试外接程序。

1. 在 Visual Studio 中打开解决方案，按 F5 构建示例。 
2. 通过为 Exchange 2013 服务器提供电子邮件地址和密码连接至 Exchange 帐户，然后允许服务器配置电子邮件帐户。  
3. 在浏览器中，通过输入帐户名称和密码登录电子邮件帐户。  
4. 选择收件箱中的一封邮件，然后在呈现上述邮件的外接程序栏中单击**使用标识令牌**。  
5. 单击**向服务发送唯一 Exchange ID** 按钮，向 Exchange 服务器发送请求。  
6. 服务器将会提示你登录。你可以在服务用户名和密码框中键入任何内容。本示例不会验证文本框的内容。  
7. 再次单击**向服务发送唯一 Exchange ID** 按钮。服务器这次将会返回响应，但没有用户名和密码请求。  

如果收件箱中具有其他电子邮件，则你可以切换至该电子邮件，显示**使用标识令牌**外接程序，然后再次单击该按钮。服务器将会返回响应，但没有用户名或密码请求。


<a name="troubleshooting"></a>
## 疑难解答
使用 Outlook Web App 测试 Outlook 邮件外接程序时，你可能会遇到以下问题：

- 选中邮件后，不会显示外接程序栏。如果发生此情况，请在 Visual Studio 窗口中选择**调试 - 停止调试**重启外接程序，然后按 F5 重建并部署外接程序。  
- 部署和运行外接程序时，可能不会记录对 JavaScript 代码的更改。如果更改未记录，请清除 Web 浏览器上的缓存，方法是选择**工具 - Internet 选项**并选择**删除**按钮。删除临时 Internet 文件，然后重启外接程序。

如果外接程序已加载但未运行，请尝试在 Visual Studio 中构建解决方案（**构建 > 构建解决方案**）。查看错误列表中是否存在缺失的依赖项，并视需要添加它们。

<a name="questions"></a>
## 问题和意见

- 如果你在运行此示例时遇到任何问题，请[记录问题](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-UseIdentityToken/issues)。
- 与 Office 外接程序开发相关的问题一般应发布到 [Stack Overflow](http://stackoverflow.com/questions/tagged/office-addins)。确保你的问题或意见使用 [Office 外接程序] 进行了标记。

<a name="additional-resources"></a>
## 其他资源
- MSDN 上的 [Office 外接程序](https://msdn.microsoft.com/library/office/jj220060.aspx)文档
- [Web API：官方 Microsoft ASP.NET 网站](http://www.asp.net/web-api)  
- [使用 Exchange 2013 标识令牌对邮件外接程序进行身份验证](http://msdn.microsoft.com/library/c0520a1e-d9ba-495a-a99f-6816d7d2a23e)  
- [更多外接程序示例](https://github.com/OfficeDev?utf8=%E2%9C%93&query=-Add-in)

## 版权信息
版权所有 (c) 2015 Microsoft。保留所有权利。


此项目已采用 [Microsoft 开放源代码行为准则](https://opensource.microsoft.com/codeofconduct/)。有关详细信息，请参阅[行为准则 FAQ](https://opensource.microsoft.com/codeofconduct/faq/)。如有其他任何问题或意见，也可联系 [opencode@microsoft.com](mailto:opencode@microsoft.com)。
