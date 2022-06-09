---
page_type: sample
products:
- office-outlook
- office-365
languages:
- javascript
description: Este exemplo mostra como usar um token de cliente do Exchange Server para fornecer autenticação para usuários do suplemento de e-mail do Outlook.
extensions:
  contentType: samples
  technologies:
  - Add-ins
  createdDate: 8/14/2015 12:52:42 PM
---
# Suplemento do Outlook: Usar um token de identidade de cliente

**Sumário**

* [Resumo](#summary)
* [Pré-requisitos](#prerequisites)
* [Componentes principais do exemplo](#components)
* [Descrição do código](#codedescription)
* [Criar e depurar](#build)
* [Solução de problemas](#troubleshooting)
* [Perguntas e comentários](#questions)
* [Recursos adicionais](#additional-resources)

<a name="summary"></a>
## Resumo
Este exemplo mostra como usar um token de cliente do servidor Exchange para fornecer autenticação a usuários do seu suplemento de e-mail do Outlook. 

<a name="prerequisites"></a>
## Pré-requisitos ##

Esse exemplo requer o seguinte:  

  - Visual Studio 2013 (Atualização 5) ou Visual Studio 2015 com as ferramentas de desenvolvedor do Microsoft Office. 
  - Um computador executando o Exchange 2013 com pelo menos uma conta de email ou uma conta do Office 365. Você pode [Participar do Programa de Desenvolvedores do Office 365 e obter uma assinatura gratuita 1 ano do Office 365](https://aka.ms/devprogramsignup).
  - Qualquer navegador que ofereça suporte a ECMAScript 5.1, HTML5 e CSS3, como o Internet Explorer 9, o Chrome 13, o Firefox 5, o Safari 5.0.6 ou uma versão posterior desses navegadores.
  - Microsoft.Exchange.WebServices.Auth.dll, Microsoft.IdentityModel.dll, and Microsoft.IdentityModel.Extensions.dll. Você pode instalar os pacotes necessários no console do Gerenciador de pacotes (**Ferramentas > Gerenciador de pacotes do NuGet >**):-Install-Package EWS-API-2,1 
	- Instalar-Package Microsoft.IdentityModel  
	- Instalar-Package Microsoft.Identity.Model.Extensions  
  - [ASP.NET MVC 4](http://www.asp.net/mvc/mvc4).
  - Familiaridade com programação em JavaScript e serviços Web.

<a name="components"></a>
## Componentes principais do exemplo
A solução de exemplo contém os seguintes arquivos chave:

Projeto **UseIdentityToken** 

- [```UseIdentityToken.xml```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-UseIdentityToken/blob/master/UseIdentityToken/UseIdentityTokenManifest/UseIdentityToken.xml): O arquivo de manifesto do suplemento de e-mail do Outlook.

Projeto **UseIdentityTokenWeb** 

- [```AppRead/Home/Home.html```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-UseIdentityToken/blob/master/UseIdentityTokenWeb/AppRead/Home/Home.html): A interface do usuário HTML para o suplemento.
- [```AppRead/Home/Home.js```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-UseIdentityToken/blob/master/UseIdentityTokenWeb/AppRead/Home/Home.js): A lógica que manipula a solicitação e o uso do token de identidade.

Projeto **UseIdentityTokenService**

- [```App_Start/WebApiConfig.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-UseIdentityToken/blob/master/UseIdentityTokenService/App_Start/WebApiConfig.cs): Vincula o roteamento padrão para o serviço de API da Web.
- [```Controllers/IdentityTokenController.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-UseIdentityToken/blob/master/UseIdentityTokenService/Controllers/IdentityTokenController.cs): O objeto de serviço que fornece a lógica de negócios para o exemplo de serviço da API Web.
- [```Models/ServiceRequest.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-UseIdentityToken/blob/master/UseIdentityTokenService/Models/ServiceRequest.cs): O objeto que representa uma solicitação da Web. O conteúdo do objeto é criado a partir de um objeto de solicitação JSON enviado do suplemento.
- [```Models/ServiceResponse.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-UseIdentityToken/blob/master/UseIdentityTokenService/Models/ServiceResponse.cs): O objeto que representa uma resposta do serviço Web. O conteúdo do objeto é serializado em um objeto JSON quando eles são enviados de volta ao suplemento.

<a name="codedescription"></a>
##Description do código Este exemplo mostra como usar um token de cliente do Exchange Server para fornecer autenticação para usuários do seu suplemento de e-mail.
O servidor do Exchange emite um token exclusivo para a caixa de correio no servidor.
Você pode usar esse token para associar uma caixa de correio com serviços fornecidos a um suplemento de e-mail.

O exemplo é dividido em duas partes:  
-Um suplemento de e-mail do Outlook que é executado em seu cliente de e-mail. Ele solicita um token de identidade do servidor Exchange e envia esse token para o serviço Web.
-Um serviço Web que processa a solicitação do cliente.

O serviço Web usa as seguintes etapas para processar o token:

- Valida o token para verificar se ele foi enviado de um servidor Exchange e se o token destina-se a esse suplemento de e-mail.
- Pesquisa um dicionário local para determinar se o identificador exclusivo foi usado anteriormente. Se o identificador exclusivo não tiver sido usado, o serviço solicitará credenciais (nome de usuário e senha do serviço) do cliente. Se o identificador exclusivo estiver presente no cache de tokens, o serviço enviará uma resposta.
- Se a solicitação contiver credenciais (ou seja, é uma resposta a uma solicitação de credenciais), o serviço armazena o nome de usuário do serviço no cache de token com o identificador exclusivo do token como sua chave.

Este exemplo não valida o nome de usuário e senha do serviço de qualquer forma. Uma solicitação de credenciais será considerada válida se contiver um nome de usuário e uma senha. As credenciais não expiram no cache neste exemplo. no entanto, todos os identificadores armazenados em cache e os nomes de usuário são perdidos quando você pára de executar o aplicativo de exemplo.

Este exemplo exige um certificado de servidor válido no servidor Exchange. Se o Exchange Server estiver usando seu certificado auto-assinado padrão, você precisará adicionar o certificado ao seu repositório de certificados confiável local. Você pode encontrar [instruções para exportar e instalar um certificado auto-assinado](http://social.technet.microsoft.com/wiki/contents/articles/13898.how-to-export-a-self-signed-server-certificate-and-import-it-on-a-another-server-in-windows-server-2008-r2.aspx) no TechNet.


<a name="build"></a>
## Criar e depurar ##
O suplemento será ativado em qualquer mensagem de e-mail na caixa de entrada do usuário. Você pode facilitar o teste do suplemento enviando uma ou mais mensagens de e-mail para a sua conta de teste antes de executar o exemplo.

1. Abra a solução no Visual Studio e pressione F5 para criar o exemplo. 
2. Conecte-se a uma conta do Exchange fornecendo o endereço de e-mail e a senha de um servidor do Exchange 2013 e permita que o servidor configure a conta de e-mail.  
3. No navegador, faça logon com a conta de e-mail, digitando o nome e a senha da conta.  
4. Selecione uma mensagem na caixa de entrada e clique em **Usar Token de Identidade** na barra de suplementos que é renderizada acima da mensagem.  
5. Clique no botão **Enviar ID exclusiva do Exchange** para enviar uma solicitação para o servidor Exchange.  
6. O servidor solicitará que você faça logon. Você pode digitar algo nas caixas nome de usuário e senha do serviço. Este exemplo não valida o conteúdo das caixas de texto.  
7. Clique no botão **Enviar identificação exclusiva do Exchange** novamente. Desta vez, uma resposta será retornada do servidor sem a solicitação de um nome de usuário e senha.  

Caso tenha outra mensagem de e-mail na caixa de entrada, você pode alternar para essa mensagem de e-mail, mostrar o suplemento **Usar Token de Identidade** e clicar no botão novamente. A resposta será retornada do servidor sem a solicitação de um nome de usuário ou senha.


<a name="troubleshooting"></a>
## Solução de problemas
Você pode encontrar os seguintes problemas ao usar o Outlook Web App para testar um suplemento de e-mail do Outlook:

- A barra de suplementos não aparece quando uma mensagem é selecionada. Se isso ocorrer, reinicie o suplemento selecionando **Debug-Stop Debugging** na janela do Visual Studio, em seguida, pressione F5 para recriar e implantar o suplemento.  
- As alterações no código JavaScript podem não ser selecionadas quando você implanta e executa o suplemento. Se as alterações não forem selecionadas, limpe o cache do navegador da Web selecionando **Ferramentas-opções da Internet** e selecionando o botão **Excluir**. Exclua os arquivos temporários da Internet e reinicie o suplemento.

Se o suplemento carregar mas não funcionar, tente criar a solução no Visual Studio (**Build > solução**). Verifique se faltam dependências na lista de erros e adicione-as conforme necessário.

<a name="questions"></a>
## Perguntas e comentários

- Se você tiver problemas para executar este exemplo, [relate um problema](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-UseIdentityToken/issues).
- Perguntas sobre o desenvolvimento de Suplementos do Office em geral devem ser postadas no [Stack Overflow](http://stackoverflow.com/questions/tagged/office-addins). Não deixe de marcar as perguntas ou comentários com [office-addins].

<a name="additional-resources"></a>
## Recursos adicionais
- Documentação de [Suplementos do Office](https://msdn.microsoft.com/library/office/jj220060.aspx) no MSDN
- [API Web: The Official Microsoft ASP.NET Site](http://www.asp.net/web-api)  
- [Autenticação de um suplemento de e-mail usando tokens de identidade do Exchange 2013](http://msdn.microsoft.com/library/c0520a1e-d9ba-495a-a99f-6816d7d2a23e)  
- [Mais exemplos de Suplementos](https://github.com/OfficeDev?utf8=%E2%9C%93&query=-Add-in)

## Direitos autorais
Copyright © 2015 Microsoft. Todos os direitos reservados.


Este projeto adotou o [Código de Conduta do Código Aberto da Microsoft](https://opensource.microsoft.com/codeofconduct/). Para saber mais, confira [Perguntas frequentes sobre o Código de Conduta](https://opensource.microsoft.com/codeofconduct/faq/) ou contate [opencode@microsoft.com](mailto:opencode@microsoft.com) se tiver outras dúvidas ou comentários.
