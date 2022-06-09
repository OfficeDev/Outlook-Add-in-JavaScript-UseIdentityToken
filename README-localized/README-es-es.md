---
page_type: sample
products:
- office-outlook
- office-365
languages:
- javascript
description: Este ejemplo muestra cómo usar un token de cliente del servidor de Exchange para proporcionar autenticación a los usuarios de su complemento de correo para Outlook.
extensions:
  contentType: samples
  technologies:
  - Add-ins
  createdDate: 8/14/2015 12:52:42 PM
---
# Complemento de Outlook: Usar un token de identidad de cliente

**Tabla de contenido**

* [Resumen](#summary)
* [Requisitos previos](#prerequisites)
* [Componentes clave del ejemplo](#components)
* [Descripción del código](#codedescription)
* [Compilar y depurar](#build)
* [Solución de problemas](#troubleshooting)
* [Preguntas y comentarios](#questions)
* [Recursos adicionales](#additional-resources)

<a name="summary"></a>
## Resumen
Este ejemplo muestra cómo usar un token de cliente desde un servidor Exchange para proporcionar autenticación a los usuarios de su complemento de correo de Outlook. 

<a name="prerequisites"></a>
## Requisitos previos ##

Este ejemplo necesita lo siguiente:  

  - Visual Studio 2013 (actualización 5) o Visual Studio 2015, con las herramientas para desarrolladores de Microsoft Office. 
  - Un equipo que ejecute Exchange 2013 y, como mínimo, una cuenta de correo electrónico o una cuenta de Office 365. Puede [participar en el programa para desarrolladores Office 365 y obtener una suscripción gratuita durante 1 año a Office 365](https://aka.ms/devprogramsignup).
  - Cualquier explorador que admita ECMAScript 5.1, HTML5 y CSS3, como Internet Explorer 9, Chrome 13, Firefox 5, Safari 5.0.6 o una versión posterior de estos exploradores.
  - Microsoft.Exchange.WebServices.Auth.dll, Microsoft.IdentityModel.dll y Microsoft.IdentityModel.Extensions.dll. Puede instalar los paquetes necesarios en la consola del administrador de paquetes (**herramientas > administrador de paquetes de NuGet > consola del administrador de paquetes**): - Install-Package EWS-Api-2.1 
	- Install-Package Microsoft.IdentityModel  
	- Install-Package Microsoft.Identity.Model.Extensions  
  - [ASP.NET MVC 4](http://www.asp.net/mvc/mvc4).
  - Familiaridad con los servicios web y la programación de JavaScript.

<a name="components"></a>
## Componentes clave del ejemplo
La solución de ejemplo contiene los archivos clave siguientes:

Proyecto **UseIdentityToken**

- [```UseIdentityToken.xml```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-UseIdentityToken/blob/master/UseIdentityToken/UseIdentityTokenManifest/UseIdentityToken.xml): El archivo de manifiesto para el complemento de correo de Outlook.

Proyecto **UseIdentityTokenWeb** 

- [```AppRead/Home/Home.html```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-UseIdentityToken/blob/master/UseIdentityTokenWeb/AppRead/Home/Home.html): La interfaz de usuario HTML para el complemento.
- [```AppRead/Home/Home.js```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-UseIdentityToken/blob/master/UseIdentityTokenWeb/AppRead/Home/Home.js): La lógica que controla la solicitud y el uso del token de identidad.

Proyecto **UseIdentityTokenService** 

- [```App_Start/WebApiConfig.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-UseIdentityToken/blob/master/UseIdentityTokenService/App_Start/WebApiConfig.cs): Enlaza el enrutamiento predeterminado para el servicio de Web API.
- [```Controllers/IdentityTokenController.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-UseIdentityToken/blob/master/UseIdentityTokenService/Controllers/IdentityTokenController.cs): El objeto de servicio que proporciona la lógica empresarial del servicio Web API de ejemplo.
- [```Models/ServiceRequest.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-UseIdentityToken/blob/master/UseIdentityTokenService/Models/ServiceRequest.cs): El objeto que representa una solicitud web. El contenido del objeto se crea desde un objeto de solicitud JSON enviado desde el complemento.
- [```Models/ServiceResponse.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-UseIdentityToken/blob/master/UseIdentityTokenService/Models/ServiceResponse.cs): El objeto que representa una respuesta del servicio web. El contenido del objeto se serializa en un objeto JSON cuando se vuelve a enviar al complemento.

<a name="codedescription"></a>
##Description
del código Este ejemplo muestra cómo usar un token de cliente desde un servidor Exchange para proporcionar autenticación a los usuarios de su complemento de correo.
El servidor de Exchange envía un token que es único para el buzón del servidor. Puede usar este token para asociar un buzón con servicios que proporciona a un complemento de correo.

El ejemplo se divide en dos partes:  
- Un complemento de correo para Outlook que se ejecuta en su cliente de correo electrónico. Solicita un token de identidad al servidor de Exchange y envía este token al servicio web.
- Un servicio web que procese la solicitud desde el cliente.

El servicio web emplea los siguientes pasos para procesar el token:

- Valida el token para asegurarse de que se ha enviado desde un servidor de Exchange y de que el token está diseñado para este complemento de correo.
- Busca un diccionario local para determinar si se ha usado antes el identificador único. Si no se ha usado el identificador único, el servicio solicita las credenciales (nombre de usuario y contraseña del servicio) del cliente. Si el identificador único está presente en la caché de tokens, el servicio envía una respuesta.
- Si la solicitud contiene credenciales (es decir, se trata de una respuesta a una solicitud de credenciales), el servicio almacena el nombre de usuario del servicio en la caché de tokens con el identificador único del token como su clave.

Este ejemplo no valida el nombre de usuario y la contraseña del servicio de ninguna manera. Una solicitud de credenciales se considera válida si contiene un nombre de usuario y una contraseña. Las credenciales no caducan en la caché de este ejemplo, sin embargo, se pierden todos los nombres de usuario e identificadores almacenados en caché al detener la ejecución de la aplicación de ejemplo.

Este ejemplo requiere un certificado de servidor válido en el servidor de Exchange. Si el servidor de Exchange usa su certificado autofirmado predeterminado, tendrá que agregar el certificado al almacén de certificados de confianza local. Puede encontrar [instrucciones para exportar e instalar un certificado autofirmado](http://social.technet.microsoft.com/wiki/contents/articles/13898.how-to-export-a-self-signed-server-certificate-and-import-it-on-a-another-server-in-windows-server-2008-r2.aspx) en TechNet.


<a name="build"></a>
## Compilar y depurar ##
El complemento se activará en cualquier mensaje de correo electrónico de la bandeja de entrada del usuario. Puede hacer que sea más fácil probar el complemento enviando uno o más mensajes de correo electrónico a la cuenta de prueba antes de ejecutar el ejemplo.

1. Abra la solución en Visual Studio y seleccione F5 para compilar el ejemplo. 
2. Conecte a una cuenta de Exchange proporcionando la dirección de correo electrónico y la contraseña de un servidor de Exchange 2013 y permita que el servidor configure la cuenta de correo electrónico.  
3. En el explorador, inicie sesión con la cuenta de correo electrónico escribiendo el nombre de la cuenta y la contraseña.  
4. Seleccione un mensaje de la bandeja de entrada y haga clic en **usar token de identidad** en la barra de complementos que se representa encima del mensaje.  
5. Haga clic en el botón **Enviar ID. de Exchange único al servicio** para enviar una solicitud al servidor Exchange.  
6. El servidor le pedirá que inicie sesión. Puede escribir lo que sea en los cuadros nombre de usuario y contraseña del servicio. Este ejemplo no valida el contenido de los cuadros de texto.  
7. Haga clic de nuevo en el botón **Enviar ID. de Exchange único al servicio**. Esta vez, el servidor devuelve una respuesta sin una solicitud de nombre de usuario y contraseña.  

Si tiene otro mensaje de correo electrónico en la bandeja de entrada, puede cambiar a ese mensaje de correo electrónico, mostrar el complemento **usar token de identidad** y hacer clic en el botón de nuevo. El servidor devuelve una respuesta sin una solicitud de nombre de usuario y contraseña.


<a name="troubleshooting"></a>
## Solución de problemas
Es posible que se produzcan los siguientes problemas al usar Outlook Web App para probar un complemento de correo para Outlook:

- La barra de complemento no aparece cuando se selecciona un mensaje. Si esto ocurre, vuelva a iniciar el complemento seleccionando **Depuración: detener depuración** en la ventana de Visual Studio y presione F5 para recompilar e implementar el complemento.  
- Es posible que los cambios en el código de JavaScript no se hayan recogido al implementar y ejecutar el complemento. Si no se han añadido los cambios, borre la memoria caché en el explorador web. Para ello, seleccione **herramientas: opciones de Internet** y seleccione el botón **eliminar**. Elimine los archivos temporales de Internet y reinicie el complemento.

Si el complemento se carga, pero no se ejecuta, pruebe a crear la solución en Visual Studio (**compilación > compilación de la solución**). Busque en la lista de errores las dependencias que faltan y agréguelas según sea necesario.

<a name="questions"></a>
## Preguntas y comentarios

- Si tiene algún problema para ejecutar este ejemplo, [registre un problema](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-UseIdentityToken/issues).
- Las preguntas sobre el desarrollo de complementos para Office en general deben enviarse a [Stack Overflow](http://stackoverflow.com/questions/tagged/office-addins). Asegúrese de que sus preguntas o comentarios se etiquetan con [office-addins].

<a name="additional-resources"></a>
## Recursos adicionales
- Documentación de [complementos de Office](https://msdn.microsoft.com/library/office/jj220060.aspx) sobre MSDN
- [API web: El sitio oficial de Microsoft ASP.NET](http://www.asp.net/web-api)  
- [Autenticación de un complemento de correo mediante los tokens de identidad de Exchange 2013](http://msdn.microsoft.com/library/c0520a1e-d9ba-495a-a99f-6816d7d2a23e)  
- [Más complementos de ejemplo](https://github.com/OfficeDev?utf8=%E2%9C%93&query=-Add-in)

## Derechos de autor
Copyright (c) 2015 Microsoft. Todos los derechos reservados.


Este proyecto ha adoptado el [Código de conducta de código abierto de Microsoft](https://opensource.microsoft.com/codeofconduct/). Para obtener más información, vea [Preguntas frecuentes sobre el código de conducta](https://opensource.microsoft.com/codeofconduct/faq/) o póngase en contacto con [opencode@microsoft.com](mailto:opencode@microsoft.com) si tiene otras preguntas o comentarios.
