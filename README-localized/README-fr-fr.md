---
page_type: sample
products:
- office-outlook
- office-365
languages:
- javascript
description: Cet exemple montre comment utiliser un jeton client à partir du serveur Exchange pour fournir une authentification aux utilisateurs de votre complément courrier pour Outlook.
extensions:
  contentType: samples
  technologies:
  - Add-ins
  createdDate: 8/14/2015 12:52:42 PM
---
# Complément Outlook : Utiliser un jeton d’identité client

**Table des matières**

* [Résumé](#summary)
* [Conditions préalables](#prerequisites)
* [Composants clés de l’exemple](#components)
* [Description du code](#codedescription)
* [Création et débogage](#build)
* [Résolution des problèmes](#troubleshooting)
* [Questions et commentaires](#questions)
* [Ressources supplémentaires](#additional-resources)

<a name="summary"></a>
## Résumé
Cet exemple présente comment utiliser un jeton client à partir du serveur Exchange afin de fournir une authentification pour les utilisateurs de votre complément de messagerie pour Outlook. 

<a name="prerequisites"></a>
## Conditions préalables ##

Cet exemple nécessite les éléments suivants :  

  - Visual Studio 2013 (mise à jour 5) ou Visual Studio 2015, avec les outils de développement Microsoft Office. 
  - Un ordinateur exécutant Exchange 2013 avec au moins un compte de messagerie ou un compte Office 365. Vous pouvez [participer au programme pour les développeurs Office 365 et obtenir un abonnement gratuit d’un an à Office 365](https://aka.ms/devprogramsignup).
  - Tout navigateur qui prend en charge ECMAScript 5.1, HTML5 et CSS3, tel qu’Internet Explorer 9, Chrome 13, Firefox 5, Safari 5.0.6 ou une version ultérieure de ces navigateurs.
  - Microsoft.Exchange.WebServices.Auth.dll, Microsoft.IdentityModel.dll, and Microsoft.IdentityModel.Extensions.dll. Vous pouvez installer les packages requis à partir de la console Package Manager (**Outils >Gestionnaire de package NuGet >Console Gestionnaire de package**) : – package EWS-API-2.1 
	- Install-Package Microsoft.IdentityModel  
	- Install-Package Microsoft.Identity.Model.Extensions  
  - [](http://www.asp.net/mvc/mvc4)ASP.NET MVC 4[.
  - Être familiarisé avec les services web et de programmation JavaScript.

<a name="components"></a>
## Composants clés de l’exemple
La solution de l’exemple contient les fichiers clés suivants :

Projet **UseIdentityToken**

- [```UseIdentityToken.xml```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-UseIdentityToken/blob/master/UseIdentityToken/UseIdentityTokenManifest/UseIdentityToken.xml) : Fichier manifeste pour le complément courrier pour Outlook.

Projet **UseIdentityTokenWeb**

- [```AppRead/Home/Home.html```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-UseIdentityToken/blob/master/UseIdentityTokenWeb/AppRead/Home/Home.html) : Interface utilisateur HTML pour le complément.
- [```AppRead/Home/Home.js```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-UseIdentityToken/blob/master/UseIdentityTokenWeb/AppRead/Home/Home.js) : Logique gérant les demandes et l’utilisation du jeton d’identité.

Projet **UseIdentityTokenService**

- [```App_Start/WebApiConfig.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-UseIdentityToken/blob/master/UseIdentityTokenService/App_Start/WebApiConfig.cs) : Lie le routage par défaut pour le service API web.
- [```Controllers/IdentityTokenController.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-UseIdentityToken/blob/master/UseIdentityTokenService/Controllers/IdentityTokenController.cs) : Objet de service qui fournit la logique métier pour l’exemple de service d'API web.
- [```Models/ServiceRequest.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-UseIdentityToken/blob/master/UseIdentityTokenService/Models/ServiceRequest.cs) : Objet qui représente une requête web. Le contenu de l’objet est créé à partir d’un objet de requête JSON envoyé depuis votre complément de messagerie.
- [```Models/ServiceResponse.cs```](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-UseIdentityToken/blob/master/UseIdentityTokenService/Models/ServiceResponse.cs) : Objet qui représente une réponse du service web. Le contenu de l’objet est sérialisé en objet JSON lorsqu’il est renvoyé au complément.

<a name="codedescription"></a>
##Description
de code Cet exemple présente comment utiliser un jeton client à partir du serveur Exchange afin de fournir une authentification pour les utilisateurs de votre complément de messagerie.
Le serveur Exchange publie un jeton propre à la boîte aux lettres sur le serveur. Vous pouvez utiliser ce jeton pour associer une boîte aux lettres aux services que vous fournissez à un complément de courrier.

L’échantillon se divise en deux parties :  
– Un complément de courrier pour Outlook qui s’exécute dans votre client de messagerie. Il demande un jeton d’identité au serveur Exchange Server et envoie ce jeton au service web.
– Service web qui traite la demande en provenance du client.

Le service suit les étapes suivantes pour traiter le jeton :

- Valide le jeton pour s’assurer qu’il a été envoyé depuis un serveur Exchange Server et que le jeton était destiné à ce complément de courrier.
- Recherche un dictionnaire local pour déterminer si l’identificateur unique a été utilisé précédemment. Si l’identificateur unique n’a pas été utilisé, le service demande les informations d’identification (nom d’utilisateur et mot de passe du service) du client. Si l’identificateur unique est présent dans le cache de jetons, le service envoie une réponse.
- Si la demande contient des informations d’identification (autrement dit, une réponse à une demande d’informations d’identification), le service stocke le nom d’utilisateur du service dans le cache de jetons avec l’identificateur unique du jeton comme clé.

Cet exemple ne valide en aucune façon le nom d’utilisateur et le mot de passe du service. Une demande d’informations d’identification est considérée comme valide si elle contient un nom d’utilisateur et un mot de passe. Les informations d’identification n’expirent pas dans le cache dans cet exemple. Cependant, tous les identificateurs et noms d’utilisateur mis en cache sont perdus lorsque vous stoppez l’exécution de l’exemple d’application.

Cet exemple nécessite un certificat de serveur valide sur le serveur Exchange. Si le serveur Exchange utilise le certificat auto-signé par défaut, vous devez ajouter le certificat à votre magasin de certificats approuvés local. Vous pourrez trouver des [instructions pour l'exportation et l'installation d'un certificat auto-signé](http://social.technet.microsoft.com/wiki/contents/articles/13898.how-to-export-a-self-signed-server-certificate-and-import-it-on-a-another-server-in-windows-server-2008-r2.aspx) sur TechNet.


<a name="build"></a>
## Création et débogage ##
Le complément sera activé sur tout message électronique figurant dans la boîte de réception de l’utilisateur. Vous pouvez simplifier le test du complément en envoyant un ou plusieurs courriers électroniques à votre compte de test avant d’exécuter l’exemple.

1. Ouvrez la solution dans Visual Studio, puis appuyez sur F5 pour créer l’exemple. 
2. Connectez-vous à un compte Exchange en fournissant l’adresse de courrier et le mot de passe d’un serveur Exchange 2013, puis autorisez le serveur à configurer le compte de messagerie.  
3. Dans le navigateur, connectez-vous avec le compte de courrier en entrant le nom du compte et le mot de passe.  
4. Sélectionnez un message dans la boîte de réception, puis cliquez sur **Utiliser un jeton d'identité** dans la barre de complément qui se présente au-dessus du message.  
5. Cliquez sur le bouton **Envoyer un ID Exchange unique au service** pour envoyer une demande au serveur Exchange.  
6. Le serveur vous demandera de vous connecter. Vous pouvez taper n'importe quel élément dans les zones nom d’utilisateur et mot de passe du service. Cet exemple ne valide pas le contenu de zones de texte.  
7. Cliquez de nouveau sur le bouton **Envoyer un ID Exchange unique au service**. Cette fois, une réponse est renvoyée par le serveur sans qu’une demande de nom d’utilisateur et de mot de passe soit demandée.  

Si vous avez un autre message électronique dans votre boîte de réception, vous pouvez basculer vers ce message, affichez le complément **Utiliser un jeton d’identité**, puis cliquer de nouveau sur le bouton. La réponse sera renvoyée par le serveur sans qu’une demande de nom d’utilisateur ou de mot de passe soit demandée.


<a name="troubleshooting"></a>
## Résolution de problèmes
Vous pouvez rencontrer les problèmes suivants lorsque vous utilisez Outlook Web App pour tester un complément courrier pour Outlook :

- La barre de complément n'apparaît pas lorsque le message est sélectionné. Si c’est le cas, redémarrez le complément en sélectionnant **Debug – arrêter le débogage** dans la fenêtre Visual Studio, puis appuyez sur F5 pour regénérer et déployer le complément.  
- Les modifications apportées au code JavaScript peuvent ne pas être prises en compte lors du déploiement et de l’exécution du complément. Si les modifications ne sont pas prises en compte, effacez le cache du navigateur web en sélectionnant **Outils – Options Internet** puis sélectionnez **Supprimer...** Supprimez les fichiers Internet temporaires, puis redémarrez le complément.

Si le complément se charge mais ne s’exécute pas, essayez de générer la solution dans Visual Studio (**Build > Générer une solution**). Recherchez les dépendances manquantes dans la Liste des erreurs et ajoutez-les si nécessaire.

<a name="questions"></a>
## Questions et commentaires

- Si vous rencontrez des difficultés pour exécuter cet exemple, veuillez [consigner un problème](https://github.com/OfficeDev/Outlook-Add-in-JavaScript-UseIdentityToken/issues).
- Si vous avez des questions générales sur le développement de compléments Office, envoyez-les sur [Stack Overflow](http://stackoverflow.com/questions/tagged/office-addins). Posez vos questions ou envoyez vos commentaires en incluant la balise [office-addins].

<a name="additional-resources"></a>
## Ressources supplémentaires
- Documentation pour [Compléments Office](https://msdn.microsoft.com/library/office/jj220060.aspx) sur MSDN.
- [API Web : Le site officiel Microsoft ASP.NET](http://www.asp.net/web-api)  
- [Authentification d’un complément de courrier à l’aide de jetons d’identité Exchange 2013](http://msdn.microsoft.com/library/c0520a1e-d9ba-495a-a99f-6816d7d2a23e)  
- [Autres exemples de compléments](https://github.com/OfficeDev?utf8=%E2%9C%93&query=-Add-in)

## Copyright
Copyright (c) 2015 Microsoft. Tous droits réservés.


Ce projet a adopté le [code de conduite Open Source de Microsoft](https://opensource.microsoft.com/codeofconduct/). Pour en savoir plus, reportez-vous à la [FAQ relative au code de conduite](https://opensource.microsoft.com/codeofconduct/faq/) ou contactez [opencode@microsoft.com](mailto:opencode@microsoft.com) pour toute question ou tout commentaire.
