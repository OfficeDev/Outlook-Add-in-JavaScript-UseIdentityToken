---
page_type: sample
products:
- office-outlook
- office-365
languages:
- javascript
description: このサンプルでは、Exchange サーバーからのクライアント トークンを使用して Outlook のメール アドインのユーザーの認証を行う方法を示します。Outlook.
extensions:
  contentType: samples
  technologies:
  - Add-ins
  createdDate: 8/14/2015 12:52:42 PM
---
# Outlook アドイン:クライアント ID トークンを使用する

**目次**

* [概要](#summary)
* [前提条件](#prerequisites)
* [サンプルの主要なコンポーネント](#components)
* [コードの説明](#codedescription)
* [ビルドとデバッグ](#build)
* [トラブルシューティング](#troubleshooting)
* [質問とコメント](#questions)
* [その他のリソース](#additional-resources)

<a name="summary"></a>
## 概要
このサンプルでは、Exchange サーバーからのクライアント トークンを使用して、Outlook のメール アドインのユーザーの認証を行う方法を示します。 

<a name="prerequisites"></a>
## 前提条件 ##

このサンプルを実行するには次のものが必要です。  

  - Visual Studio 2013 (Update 5) または Visual Studio 2015 およびh Microsoft Office Developer Tools。 
  - 少なくとも 1 つのメール アカウントまたは Office 365 アカウントがある Exchange 2013 を実行するコンピューター。[Office 365 Developer プログラムに参加すると、Office 365 の 1 年間無料のサブスクリプションを取得](https://aka.ms/devprogramsignup)できます。
  - Internet Explorer 9、Chrome 13、Firefox 5、Safari 5.0.6、またはこれらのブラウザーの以降のバージョンなど、 ECMAScript 5.1、HTML5、および CSS3 をサポートする任意のブラウザー。
  - Microsoft.Exchange.WebServices.Auth.dll、Microsoft.IdentityModel.dll、および Microsoft.IdentityModel.Extensions.dll。必要なパッケージは、パッケージ マネージャー コンソールからインストールすることができます ([**ツール] > [NuGet パッケージ マネージャー] > パッケージ マネージャー コンソール**)。- Install-Package EWS-Api-2.1 
	- Install-Package Microsoft.IdentityModel  
	- Install-Package Microsoft.Identity.Model.Extensions  
  - [ASP.NET MVC 4](http://www.asp.net/mvc/mvc4)
  - JavaScript プログラミングと Web サービスに精通していること。

<a name="components"></a>
## サンプルの主要なコンポーネント
サンプル ソリューションに含まれる主なファイルは次のとおりです。

**UseIdentityToken** プロジェクト

- [```UseIdentityToken.xml```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-UseIdentityToken/blob/master/UseIdentityToken/UseIdentityTokenManifest/UseIdentityToken.xml):Outlook 用メール アドインのマニフェスト ファイル。

**UseIdentityTokenWeb** プロジェクト

- [```AppRead/Home/Home.html```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-UseIdentityToken/blob/master/UseIdentityTokenWeb/AppRead/Home/Home.html):アドインの HTML ユーザー インターフェイス。
- [```AppRead/Home/Home.js```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-UseIdentityToken/blob/master/UseIdentityTokenWeb/AppRead/Home/Home.js):ID トークンの要求と使用を処理するロジック。

**UseIdentityTokenService** プロジェクト

- [```App_Start/WebApiConfig.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-UseIdentityToken/blob/master/UseIdentityTokenService/App_Start/WebApiConfig.cs):Web API サービスの既定のルーティングをバインドします。
- [```Controllers/IdentityTokenController.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-UseIdentityToken/blob/master/UseIdentityTokenService/Controllers/IdentityTokenController.cs):サンプル Web API サービスのビジネス ロジックを提供するサービス オブジェクト。
- [```Models/ServiceRequest.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-UseIdentityToken/blob/master/UseIdentityTokenService/Models/ServiceRequest.cs):Web 要求を表すオブジェクト。オブジェクトの内容は、アドインから送信された JSON 要求オブジェクトから作成されます。
- [```Models/ServiceResponse.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-UseIdentityToken/blob/master/UseIdentityTokenService/Models/ServiceResponse.cs):Web サービスからの応答を表すオブジェクト。オブジェクトの内容は、アドインに送り返された際に、JSON オブジェクトにシリアル化されます。

<a name="codedescription"></a>
##コードの説明
このサンプルでは、Exchange サーバーからのクライアント トークンを使用して、Outlook のメール アドインのユーザーの認証を行う方法を示します。サーバーのメールボックスに一意のトークンが Exchange サーバーにより発行されます。
このトークンを使用して、メールボックスを、メール アドインに提供するサービスに関連付けることができます。

このサンプルは 2つの部分に分かれています。  
- メール クライアントで実行される Outlook のメール アドイン。Exchange サーバーからの ID トークンを要求し、このトークンを Web サービスに送信します。
- クライアントからの要求を処理する Web サービス。

Web サービスは、次の手順を使用してトークンを処理します。

- トークンを検証し、それが Exchange サーバーから送信されたものであり、このメール アドインを対象としているトークンであることを確認します。
- ローカルのディクショナリを検索して、一意の識別子が以前に使用されたことがあるかどうかを判断します。一意の識別子が使用されたことがない場合、サービスは資格情報 (サービス ユーザー名とパスワード) をクライアントに要求します。トークン キャッシュに一意の識別子が存在する場合、サービスは応答を送信します。
- 要求に資格情報が含まれている場合 (つまり、資格情報の要求に対する応答である場合)、サービスは、トークンからの一意の識別子をキーとして使用して、サービス ユーザー名をトークン キャッシュに格納します。

このサンプルでは、サービス ユーザー名とパスワードの検証は一切行われません。資格情報の要求は、ユーザー名とパスワードの両方がそれに含まれている場合は有効であるとみなされます。このサンプルでは、キャッシュの資格情報に有効期限はありません。ただし、サンプル アプリケーションの実行を停止すると、キャッシュされた識別子とユーザー名はすべて失われます。

このサンプルでは、Exchange サーバーが有効なサーバー証明書を持っている必要があります。サーバーで既定の自己署名証明書が使用されている場合は、証明書をローカルの信頼された証明書ストアに追加する必要があります。[自己署名証明書をエクスポートしてインストールする方法](http://social.technet.microsoft.com/wiki/contents/articles/13898.how-to-export-a-self-signed-server-certificate-and-import-it-on-a-another-server-in-windows-server-2008-r2.aspx)については、TechNet を参照してください。


<a name="build"></a>
## ビルドとデバッグ ##
アドインは、ユーザーの受信トレイのすべてのメール メッセージで有効になります。サンプルを実行する前に、1 つまたは複数のメール メッセージをテスト アカウントに送信しておくと、アドインを簡単にテストできます。

1. ソリューションを Visual Studio で開き、F5 キーを押してサンプルをビルドします。 
2. Exchange 2013 サーバー用のメール アドレスとパスワードを入力して Exchange アカウントに接続し、メール アカウントを構成することをサーバーに許可します。  
3. ブラウザーで、アカウント名とパスワードを入力して、メール アカウントでログオンします。  
4. 受信トレイでメッセージを選択し、メッセージの上に表示されているアドイン バーにある [**Use Identity Token (ID トークンを使用する)**] をクリックします。  
5. [**Send unique Exchange ID to service (一意の Exchange ID をサービスに送信)**] ボタンをクリックして、Exchange サーバーに要求を送信します。  
6. サーバーからログオンが求められます。[サービス ユーザー名] と [パスワード] ボックスに入力する内容は自由です。このサンプルでは、テキスト ボックスの内容は検証されません。  
7. [**Send unique Exchange ID to service (一意の Exchange ID をサービスに送信)**] ボタンを再度クリックします。今度は、ユーザー名とパスワードを求められることなくサーバーから応答が返されます。  

受信トレイに別のメール メッセージがある場合は、そのメール メッセージに切り替えて [**Use Identity Token (ID トークンを使用する)**] アドインを表示させ、ボタンをもう一度クリックすることができます。ユーザー名とパスワードを求められることなくサーバーから応答が返されます。


<a name="troubleshooting"></a>
## トラブルシューティング
Outlook Web App を使用して Outlook のメール アドインをテストするときに、次の問題が発生する場合があります。

- メッセージが選択されているときに、アドイン バーが表示されない。この問題が発生した場合、Visual Studio のウィンドウで **[デバッグ]、[デバッグの停止]** の順に選択してアドインを再起動し、次に F5 キーを押してアドインを再ビルドして展開します。  
- アドインの展開と実行時に JavaScript コードの変更が認識されないことがある。変更が認識されない場合は、**[ツール]、[インターネット オプション]** の順に選択し、[**削除**] ボタンを選択して Web ブラウザーのキャッシュをクリアします。インターネット一時ファイルを削除してからアドインを再起動します。

アドインが読み込まれるものの動作しない場合、ソリューションを Visual Studio でビルドしてみます (**[ビルド] > [ソリューションのビルド]**)。[エラー一覧] を確認して欠落している依存関係がないかどうかを確認し、必要に応じて依存関係を追加します。

<a name="questions"></a>
## 質問とコメント

- このサンプルの実行について問題がある場合は、[問題をログに記録](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-UseIdentityToken/issues)してください。
- Office アドイン開発全般の質問については、「[Stack Overflow](http://stackoverflow.com/questions/tagged/office-addins)」に投稿してください。質問やコメントには、必ず "office-addins" のタグを付けてください。

<a name="additional-resources"></a>
## その他のリソース
- MSDN 上の[Office アドイン](https://msdn.microsoft.com/library/office/jj220060.aspx) ドキュメント
- [Web API:Microsoft ASP.NET の公式サイト](http://www.asp.net/web-api)  
- [Exchange 2013 ID トークンを使用してメール アプリを認証する](http://msdn.microsoft.com/library/c0520a1e-d9ba-495a-a99f-6816d7d2a23e)  
- [その他のアドイン サンプル](https://github.com/OfficeDev?utf8=%E2%9C%93&query=-Add-in)

## 著作権
Copyright (c) 2015 Microsoft.All rights reserved.


このプロジェクトでは、[Microsoft オープン ソース倫理規定](https://opensource.microsoft.com/codeofconduct/)が採用されています。詳細については、「[倫理規定の FAQ](https://opensource.microsoft.com/codeofconduct/faq/)」を参照してください。また、その他の質問やコメントがあれば、[opencode@microsoft.com](mailto:opencode@microsoft.com) までお問い合わせください。
