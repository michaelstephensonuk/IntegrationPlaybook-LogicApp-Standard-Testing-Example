using Azure.Core;
using Azure.Identity;
using IPB.LogicApp.Standard.Testing;
using IPB.LogicApp.Standard.Testing.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace logicapp.testing.acceptancetests.Helpers
{
    public class AzureADHelper
    {
        //Note: There are a few different ways you could choose to configure the credential.  There is more info and help
        //for troubleshooting on this page
        //https://github.com/Azure/azure-sdk-for-net/blob/main/sdk/identity/Azure.Identity/TROUBLESHOOTING.md#troubleshoot-azure-identity-authentication-issues
        
            
        public static TokenCredential GetDefaultServicePrincipalToken()
        {
            var config = ConfigHelper.GetConfiguration();
            TokenCredential credential = new ClientSecretCredential(config["AZURE_TENANT_ID"], 
                config["AZURE_CLIENT_ID"], 
                config["AZURE_CLIENT_SECRET"]);

            return credential;
        }



    }
}