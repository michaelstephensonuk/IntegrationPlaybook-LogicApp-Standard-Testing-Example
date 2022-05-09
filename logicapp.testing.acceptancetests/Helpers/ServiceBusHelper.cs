using Azure.Identity;
using Azure.Messaging.ServiceBus;
using IPB.LogicApp.Standard.Testing;
using IPB.LogicApp.Standard.Testing.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace logicapp.testing.acceptancetests.Helpers
{
    public class ServiceBusHelper
    {
        public static Task SendMessage(string queueName, string messageBody, string contentType, Dictionary<string, object> customProperties = null, string correlationId = "")
        {
            var config = ConfigHelper.GetConfiguration();
            var namespaceName = config["servicebus_namespace"];
            var credential = AzureADHelper.GetDefaultServicePrincipalToken();
            var client = new ServiceBusClient(namespaceName, credential);
            var sender = client.CreateSender(queueName);

            var sbMessage = new ServiceBusMessage(messageBody);
            sbMessage.ContentType = contentType;
            sbMessage.CorrelationId = correlationId;

            if(customProperties != null)
            {
                foreach(var item in customProperties)
                    sbMessage.ApplicationProperties.Add(item.Key, item.Value);
            }

            //sender.SendMessageAsync(sbMessage).ConfigureAwait(false);     
            var task = sender.SendMessageAsync(sbMessage);
            task.Wait();
            return task;
        }        
    }
}