
# Create Solution

```
dotnet new sln
```


# Logic App Project

## Create Project

1) Make a directory for the logic app code

```
md logicapp
```

2) Use logic app in azure extension to create project. 


# Acceptance Test Project

Go to solution directory

1) Create Project

```
dotnet new mstest --name logicapp.testing.acceptancetests
```

2) Add Logic App Test Framework

 Change to the acceptance test folder

```
cd logicapp.testing.acceptancetests

```

3) Add the testing framework for testing the deployed code

```
dotnet add package "IPB.LogicApp.Standard.Testing"
dotnet add package "Specflow.MsTest"
dotnet add package "SpecFlow.Plus.LivingDocPlugin"
dotnet add package "Microsoft.Extensions.Configuration"
dotnet add package "Microsoft.Extensions.Configuration.Json"
```

4) Add appsettings.json file
This file is where you add some app settings to allow the test framework to connect to Azure to run your logic app workflows.

Use the below file as a reference.

https://github.com/michaelstephensonuk/IntegrationPlaybook-LogicApp-Standard-Testing/blob/main/LogicApp.Testing.Example/appsettings.json


## Add Helpers to test project

Helpers are for classes which will help you code the project

1) Add a folder for the Helpers

```
md Helpers

```

2) Add LogicAppTestManagerBuilder to project
The LogicAppManagerBuilder is a helper class you can create to encapsulate how you create the logic app manager for use in your tests.  I normally just read some values from the
appsettings file to configure it.

Use the below file as a reference:
https://github.com/michaelstephensonuk/IntegrationPlaybook-LogicApp-Standard-Testing/blob/main/LogicApp.Testing.Example/LogicAppTestManagerBuilder.cs


3) Add Common Test Context class to helpers

https://github.com/michaelstephensonuk/IntegrationPlaybook-LogicApp-Standard-Testing/blob/main/LogicApp.Testing.Example/CommonTestContext.cs


## Add Features to test

1) Add a folder for the features

```
md Features
```

Any new tests you add will go under the features folder.


## Log Analytics Queries

If you want to use the log analytics query approach demonstrated in this sample then add the below reference

```
dotnet add package Azure.Identity
dotnet add package Azure.Monitor.Query --prerelease
```

More info on the identity client
https://docs.microsoft.com/en-us/dotnet/api/overview/azure/identity-readme?view=azure-dotnet

More info on the log analytics:
https://docs.microsoft.com/en-us/dotnet/api/overview/azure/monitor.query-readme-pre



## Service Bus Test

In one of the test cases we are using service bus.  If you want to do this test you would also need these extensions

```
dotnet add package Azure.Messaging.ServiceBus

```

# Unit Test Project
This section describes how to setup the unit test project for use in the solution.


## Unit Test Project File

1) Create the project

```
dotnet new mstest --name logicapp.testing.unittests
```

Then go to the folder for the unit tests.

```
cd logicapp.testing.unittests

```


2) Add the testing framework for testing the deployed code

```
dotnet add package "Newtonsoft.Json"
dotnet add package "Microsoft.AspNet.WebApi.Client"
dotnet add package "Microsoft.AspNetCore"
dotnet add package "Microsoft.AspNetCore.ResponseCompression"
dotnet add package "Microsoft.AspNetCore.Mvc"
dotnet add package "Microsoft.Extensions.Configuration"
dotnet add package "Microsoft.Extensions.Configuration.Json"

```

3) Add Specflow to the project
If you want to use specflow within your unit test project run the below 2 commands

```
dotnet add package "Specflow.MsTest"
dotnet add package "SpecFlow.Plus.LivingDocPlugin"
```

4) Add the logic app test framework nuget

```
dotnet add package IPB.LogicApp.Standard.Testing.Local 
```


# Add Test Projects to Solution

1) Add the unit test project to the solution
```
dotnet sln vs-code-logicapp-testing.sln add .\logicapp.testing.unittests\logicapp.testing.unittests.csproj
```

2) Add the acceptance test project to the solution
```
dotnet sln vs-code-logicapp-testing.sln add .\logicapp.testing.acceptancetests\logicapp.testing.acceptancetests.csproj
```
