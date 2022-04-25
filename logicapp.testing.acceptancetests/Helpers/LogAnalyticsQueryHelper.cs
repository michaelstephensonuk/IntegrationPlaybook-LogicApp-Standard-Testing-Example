
using Azure;
using Azure.Identity;
using Azure.Monitor.Query;
using Azure.Monitor.Query.Models;
using IPB.LogicApp.Standard.Testing;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace LogicApp.Testing.AcceptanceTests.Helpers
{
    public class LogAnalyticsQueryHelper
    {
        public async Task<LogsTable> ExecuteQuery(string workspaceId, string query, TimeSpan timeSpan)
        {
            
            var credential = AzureADHelper.GetDefaultServicePrincipalToken();
            var client = new LogsQueryClient(credential);
            Response<LogsQueryResult> response = await client.QueryWorkspaceAsync(
                workspaceId,
                query, 
                new QueryTimeRange(timeSpan));

            return response.Value.Table;
        }
    }
}