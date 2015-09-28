# Outlook Add-in: Use a client identity token

**Table of contents**

* [Summary](#summary)
* [Prerequisites](#prerequisites)
* [Key components of the sample](#components)
* [Description of the code](#codedescription)
* [Build and debug](#build)
* [Troubleshooting](#troubleshooting)
* [Questions and comments](#questions)
* [Additional resources](#additional-resources)

<a name="summary"></a>
##Summary
This sample shows how to use a client token from the Exchange server to provide authentication for users of your mail add-in for Outlook. 

<a name="prerequisites"></a>
## Prerequisites ##

This sample requires the following:  

  - Visual Studio 2013 (Update 5) or Visual Studio 2015, with Microsoft Office Developer Tools. 
  - A computer running Exchange 2013 with at least one email account, or an Office 365 account. You can [sign up for an Office 365 Developer subscription](http://aka.ms/o365-android-connect-signup) and get an Office 365 account through it.
  - Any browser that supports ECMAScript 5.1, HTML5, and CSS3, such as Internet Explorer 9, Chrome 13, Firefox 5, Safari 5.0.6, or a later version of these browsers.
  - Microsoft.Exchange.WebServices.Auth.dll, Microsoft.IdentityModel.dll, and Microsoft.IdentityModel.Extensions.dll. You can install the required packages from the Package Manager Console (**Tools > NuGet Package Manager > Package Manager Console**): 	- Install-Package EWS-Api-2.1 
	- Install-Package Microsoft.IdentityModel  
	- Install-Package Microsoft.Identity.Model.Extensions  
  - [ASP.NET MVC 4](http://www.asp.net/mvc/mvc4).
  - Familiarity with JavaScript programming and web services.

<a name="components"></a>
## Key components of the sample
The sample solution contains the following key files:

**UseIdentityToken** project

- [```UseIdentityToken.xml```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-UseIdentityToken/blob/master/UseIdentityToken/UseIdentityTokenManifest/UseIdentityToken.xml): The manifest file for the mail add-in for Outlook.

**UseIdentityTokenWeb** project

- [```AppRead/Home/Home.html```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-UseIdentityToken/blob/master/UseIdentityTokenWeb/AppRead/Home/Home.html): The HTML user interface for the add-in.
- [```AppRead/Home/Home.js```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-UseIdentityToken/blob/master/UseIdentityTokenWeb/AppRead/Home/Home.js): The logic that handles requesting and using the identity token.

**UseIdentityTokenService** project

- [```App_Start/WebApiConfig.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-UseIdentityToken/blob/master/UseIdentityTokenService/App_Start/WebApiConfig.cs): Binds the default routing for the Web API service.
- [```Controllers/IdentityTokenController.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-UseIdentityToken/blob/master/UseIdentityTokenService/Controllers/IdentityTokenController.cs): The service object that provides the business logic for the sample Web API service.
- [```Models/ServiceRequest.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-UseIdentityToken/blob/master/UseIdentityTokenService/Models/ServiceRequest.cs): The object that represents a web request. The contents of the object are created from a JSON request object sent from the add-in.
- [```Models/ServiceResponse.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-UseIdentityToken/blob/master/UseIdentityTokenService/Models/ServiceResponse.cs): The object that represents a response from the web service. The contents of the object are serialized to a JSON object when they are sent back to the add-in.

<a name="codedescription"></a>
##Description of the code
This sample shows how to use a client token from the Exchange server to provide authentication for users of your mail add-in. The Exchange server issues a token that is unique to the mailbox 
	on the server. You can use this token to associate a mailbox with services that you provide to a mail add-in.

The sample is divided in two parts:  
- A mail add-in for Outlook that runs in your email client. It requests an identity token from the Exchange server and sends this token to the web service.
- A web service that processes the request from the client.

The web service uses the following steps to process the token:

- Validates the token to make sure that it was sent from an Exchange server, and that the token was intended for this mail add-in.
- Searches a local dictionary to determine whether the unique identifier has been used before. If the unique identifier has not been used, the service requests credentials (service user name and password) from the client. If the unique identifier is present in the token cache, the service sends a response.
- If the request contains credentials (that is, it is a response to a request for credentials), the service stores the service user name in the token cache with the unique identifier from the token as its key.

This sample does not validate the service user name and password in any way. A credential request is considered valid if it contains both a user name and password. Credentials do not expire from the cache in this sample; however, all the cached identifiers and user names are lost when you stop running the sample application.

This sample requires a valid server certificate on the Exchange server. If the Exchange server is using its default self-signed certificate, you will need to add the certificate to your local trusted certificate store. You can find [instructions for exporting and installing a self-signed certificate](http://social.technet.microsoft.com/wiki/contents/articles/13898.how-to-export-a-self-signed-server-certificate-and-import-it-on-a-another-server-in-windows-server-2008-r2.aspx) on TechNet.


<a name="build"></a>
## Build and debug ##
The add-in will be activated on any email message in the user's Inbox. You can make it easier to test the add-in by sending one or more email messages to your test account before you run the sample.

1. Open the solution in Visual Studio, and press F5 to build the sample. 
2. Connect to an Exchange account by providing the email address and password for an Exchange 2013 server, and allow the server to configure the email account.  
3. In the browser, log on with the email account by entering the account name and password.  
4. Select a message in the Inbox, and click **Use Identity Token** in the add-in bar that renders above the message.  
5. Click the **Send unique Exchange ID to service** button to send a request to the Exchange server.  
6. The server will prompt you to log on. You can type anything in the service user name and password boxes. This sample does not validate the contents of the text boxes.  
7. Click the **Send unique Exchange ID to service** button again. This time, a response is returned from the server without a request for a user name and password.  

If you have another email message in your Inbox, you can switch to that email message, show the **Use Identity Token** add-in, and click the button again. The response will be returned from the server without a request for a user name or password.


<a name="troubleshooting"></a>
## Troubleshooting
You might encounter following issues when you use Outlook Web App to test a mail add-in for Outlook:

- The add-in bar does not appear when a message is selected. If this occurs, restart the add-in by selecting **Debug - Stop Debugging** in the Visual Studio window, then press F5 to rebuild and deploy the add-in.  
- Changes to the JavaScript code might not be picked up when you deploy and run the add-in. If the changes are not picked up, clear the cache on the web browser by selecting **Tools - Internet options** and selecting the **Delete** button. Delete the temporary Internet files and then restart the add-in.

If the add-in loads but does not run, try to build the solution in Visual Studio (**Build > Build Solution**). Check the Error List for missing dependencies and add them as needed.

<a name="questions"></a>
## Questions and comments

- If you have any trouble running this sample, please [log an issue](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-UseIdentityToken/issues).
- Questions about Office Add-in development in general should be posted to [Stack Overflow](http://stackoverflow.com/questions/tagged/office-addins). Make sure that your questions or comments are tagged with [office-addins].

<a name="additional-resources"></a>
## Additional resources
- [Office Add-ins](https://msdn.microsoft.com/library/office/jj220060.aspx) documentation on MSDN
- [Web API: The Official Microsoft ASP.NET Site](http://www.asp.net/web-api)  
- [Authenticating a mail add-in by using Exchange 2013 identity tokens](http://msdn.microsoft.com/library/c0520a1e-d9ba-495a-a99f-6816d7d2a23e)  
- [More Add-in samples](https://github.com/OfficeDev?utf8=%E2%9C%93&query=-Add-in)

## Copyright
Copyright (c) 2015 Microsoft. All rights reserved.
