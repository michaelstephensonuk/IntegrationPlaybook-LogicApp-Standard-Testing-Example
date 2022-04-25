
If you want to query log analytics for any of the data to help in your test then below is an example of how you can do it

https://docs.microsoft.com/en-us/dotnet/api/overview/azure/monitor.query-readme-pre


```

    [Then(@"the workflow will log a message to Log Analytics")]
    public void ThenTheWorkflowWillLogAMessageToLogAnalytics()
    {
        var config = Helpers.ConfigHelper.GetConfiguration();

        var deliveryId = _scenarioContext.Get<string>("DeliveryId");
        //TODO: Lookup run id from log analytics
        var runId = "";
        var workspaceId = config["loganalytics_workspace_id"];

        var query = new StringBuilder();
        query.AppendLine("logicapps_CL");
        query.AppendLine("| where logicapp_name_s == \"ShipInstruction-Processor\"");
        query.AppendLine($"and key1_value_s  == \"{deliveryId}\"");
        query.AppendLine("| order by TimeGenerated desc");
        query.AppendLine("| project logicapp_name_s, logicapp_runid_s, DeliveryId=key1_value_s");

        var timespan = TimeSpan.FromMinutes(60);

        var logAnalyticsHelper = new Helpers.LogAnalyticsQueryHelper();
        var logResults = logAnalyticsHelper.ExecuteQuery(workspaceId, query.ToString(), timespan);
        foreach(var row in logResults.Result.Rows)
        {
            runId = row.GetString("logicapp_runid_s");
        }

        Assert.IsFalse(string.IsNullOrEmpty(runId), "The run id is not found");
        _scenarioContext.Add("RunId", runId);            
    }

```