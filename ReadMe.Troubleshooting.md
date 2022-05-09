


## Error connecting to Functions Runtime

Check you have started azurite

- F1
- Azurite: Start


## Error with Function Core Tools

1) 
To use vscode debugging with logic app standard we need the 64 bit extension for function core tools

The error we get is

```
You can only debug 64 bit processes
```

Fix is:
```
choco install azure-functions-core-tools-3 --version 3.0.3785 -y --params "'/x64'" 
```

More Info:
(see comment in this)
https://community.chocolatey.org/packages/azure-functions-core-tools-3#install 

https://stackoverflow.com/questions/43343721/net-core-debugging-with-vs-code-only-64-bit-processes-can-be-debugged

2) How is functions core tools installed

There are a few different ways you may have installed functions core tools.  This can cause issues if your not running the func.exe version your expecting.

See below article for more info:
https://www.mikestephenson.me/2021/12/28/logic-app-standard-f5-issue-with-functions-core-tools/


# Issue with Function Bundles

In both places we needed to override the bundle versions which had been auto updated and broke stuff

1)
Folder = logicapp
File = host.json

```
{
  "version": "2.0",
  "extensionBundle": {
    "id": "Microsoft.Azure.Functions.ExtensionBundle.Workflows",
    //"version": "[1.*, 2.0.0)"
    "version": "[1.1.32, 1.1.33)"
  },
  "extensions": {
    "workflow": {
       "settings": {
         "Runtime.FlowRetentionThreshold":"7.00:00:00",
         "Runtime.FlowRunRetryableActionJobCallback.ActionJobExecutionTimeout":"00:10:00",
         "Runtime.Backend.Stateless.FlowDefaultForeachItemsLimit":"10000",
         "Runtime.Backend.HttpOperation.RequestTimeout" : "00:04:00",
         "Runtime.Backend.HttpOperation.DefaultRetryCount":"4",
         "Runtime.Backend.HttpOperation.DefaultRetryInterval":"00:00:10"
       }
    }
 }
}

```

2) 
Folder = logicapp\workflow-designtime
File = host.json

```
{
  "version": "2.0",
  "extensionBundle": {
    "id": "Microsoft.Azure.Functions.ExtensionBundle.Workflows",
    //"version": "[1.*, 2.0.0)"
    "version": "[1.1.32, 1.1.33)"
  },
  "extensions": {
    "workflow": {
      "settings": {
        "Runtime.WorkflowOperationDiscoveryHostMode": "true"
      }
    }
  }
}

```


##

.vscode\tasks.json



# Storage Issue

In early may there was an issue with azurite which was causing 412 http errors.  Until this is resolved i have changed to use a storage account in azure.  This is simply a change of the setting in the local.settings.json

logicapp\local.settings.json

```
"AzureWebJobsStorage": "UseDevelopmentStorage=true",
```