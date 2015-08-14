/*
* Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license. See full license at the bottom of this file.
*/

using UseIdentityTokenService.Models;
using Microsoft.Exchange.WebServices.Auth.Validation;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace UseIdentityTokenService.Controllers
{
  public class IdentityTokenController : ApiController
  {
    static Dictionary<string, string> idCache;

    // Static constructor
    static IdentityTokenController()
    {
      idCache = new Dictionary<string, string>();
    }

    public ServiceResponse PostIdentityToken(ServiceRequest serviceRequest)
    {
      ServiceResponse response = new ServiceResponse();

      AppIdentityToken token = (AppIdentityToken)AuthToken.Parse(serviceRequest.token);

      try
      {
        // Validate the user identity token. 
        token.Validate(new Uri(Config.Audience));
        // If the token is invalid, Validate will throw an exception. If the service reaches
        // this line, the token is valid.
        response.isValidToken = true;

        // Check to see if the uniqued ID is in the cache.
        if (idCache.ContainsKey(token.UniqueUserIdentification))
        {
          response.isKnown = true;
          response.message = string.Format(
            "User ID found in cache. Response returned for {0} without requesting credentials.",
            idCache[token.UniqueUserIdentification]);
        }
        // If the unique ID is not found, check to see if the request contains credentials.
        else if (!string.IsNullOrEmpty(serviceRequest.serviceUserName) && !string.IsNullOrEmpty(serviceRequest.password))
        {
          response.isKnown = true;
          idCache.Add(token.UniqueUserIdentification, serviceRequest.serviceUserName);
          response.message = string.Format("Unique ID cached for {0}.", serviceRequest.serviceUserName);
        }
        else
        {
          response.isKnown = false;
          response.message = "Unknown identifier.";
        }
      }
      catch (TokenValidationException ex)
      {
        response.isKnown = false;
        response.isValidToken = false;
        response.message = ex.Message;
      }

      return response;
    }
  }
}


// *********************************************************
//
// Outlook-Add-in-JavaScript-UseIdentityToken, https://github.com/OfficeDev/Outlook-Add-in-JavaScript-UseIdentityToken
//
// Copyright (c) Microsoft Corporation
// All rights reserved.
//
// MIT License:
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
// *********************************************************