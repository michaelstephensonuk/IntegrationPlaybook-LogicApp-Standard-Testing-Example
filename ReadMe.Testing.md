



# How Do I

## Run Tests normally

You can either run in solution directory to run all tests
or
Run in the acceptance test or unit test sub directory to focus on specific test projects

```
dotnet test
```

## Turn off Debugging for Tests

```
$env:VSTEST_HOST_DEBUG=0
```

## Run Tests in Debug

Add a breakpoint as needed before running the test.

1) In terminal run the following:

```
$env:VSTEST_HOST_DEBUG=1
```

2) Then run the tests with the appropriate dotnet test command

Note you might want to run an individual test as shown below if you want to focus on one test.

```
dotnet test --filter FullyQualifiedName=logicapp.testing.acceptancetests.Features.Dataverse_WhoAmI_Tests.GreenPath
```

```
dotnet test --filter FullyQualifiedName=LogicApp.Testing.UnitTests.Echo.MsTest.Tests.Echo_GreenPath
```

3) You will now attach the debugger and continue



## Run Tests with logger to get more info on tests that failed

```
dotnet test -v n
```

## Run Tests with a category
Running tests with a category will allow me to run just the mstest or just the specflow tests.

1) MsTest unit tests
If we use an attribute on the test like below then the test category allows us to specify a category for a test

```
[TestMethod, Priority(1), TestCategory("UnitTests")]
```

We can then run specific tests using a filter

```
dotnet test --filter TestCategory=UnitTests
```


2) Just Specflow Tests
If we put a tag on specflow tests which is @SpecflowTests then we can use the below command to run just the specflow tests

```
dotnet test --filter TestCategory=SpecflowTests
```

## Run Tests with a filter on priority

```
dotnet test --filter Priority=1
```