

cd .\logicapp.testing.unittests
cd ..\logicapp.testing.unittests

## Run tests to see logs
```
dotnet test --logger:"console;verbosity=detailed"
```

## Echo test to show the most basic test
```
dotnet test --filter FullyQualifiedName=LogicApp.Testing.UnitTests.Echo.MsTest.Tests.Echo_GreenPath
```

## Echo Postman Test to simulate mocked response
```

dotnet test --filter FullyQualifiedName=LogicApp.Testing.UnitTests.Workflows.Echo_Postman.MsTest.Tests.Echo_Postman_GreenPath


dotnet test --filter FullyQualifiedName=LogicApp.Testing.UnitTests.Workflows.Echo_Postman.MsTest.TestsWithMock.Echo_Postman_WithMock  --logger:"console;verbosity=detailed"

```

## Dataverse who am i test in MsTest
```
dotnet test --filter FullyQualifiedName=LogicApp.Testing.UnitTests.Workflows.Dataverse_WhoAmI.MsTest.Tests.DataverseWhoAmI_GreenPath
```

## Dataverse who am i test in Specflow
```
dotnet test --filter FullyQualifiedName=LogicApp.Testing.UnitTests.Workflows.Dataverse_WhoAmI.Specflow.Dataverse_WhoAmIFeature.Dataverse_WhoAmI_GreenPath_Specflow
```









