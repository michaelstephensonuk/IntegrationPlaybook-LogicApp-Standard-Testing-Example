# Useful Commands

Build the code:
```
dotnet build
```

Run tests with logger enabled to see all details:
```
dotnet test --logger:"console;verbosity=detailed"
```

Add a package to a project:
```
dotnet add package "Microsoft.AspNetCore.App.Ref"
```

Add an mstest project to the solution:
```
dotnet new mstest --name logicapp.testing.acceptancetests
```


# Running Tests
Make sure Azurite is running before you run the unit tests locally.

- F1
- Azurite: Start


# View Markdown Files

CTRL+K then V

More Info:
https://code.visualstudio.com/docs/languages/markdown