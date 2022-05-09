# Prerequsites

## Chocolatey
To make it easier to install things we will use chocolatey so we will need to install it.

From the following page: https://chocolatey.org/install#individual

Run the below command in an elevated powershell

```
Set-ExecutionPolicy Bypass -Scope Process -Force; [System.Net.ServicePointManager]::SecurityProtocol = [System.Net.ServicePointManager]::SecurityProtocol -bor 3072; iex ((New-Object System.Net.WebClient).DownloadString('https://community.chocolatey.org/install.ps1'))
```

## VS Code
VS Code is used for the development environment so we need that installed.

```
choco install vscode

```

## Node 
Node is used by Logic Apps so we need that installed too.

```
choco install nodejs --version=12.22.8
```

You might need to restart vs code if you had it open

# .net Core 3.1 SDK

For logic app development we need .net core 3.1 to be installed

```
choco install dotnetcore-sdk
```


## Function Core Tools
Make sure function core tools is installed.  This is used for running the tests locally on your developer machine.
More info:
https://github.com/Azure/azure-functions-core-tools#installing

With npm in vs code it is

```
npm install -g azure-functions-core-tools@3 --unsafe-perm true
```

W





# Install VS Code Extensions

## Cuecumber
This allows you to use specflow.

```
code --install-extension ms alexkrechik.cucumberautocomplete
```

## Azurite
This is used by the functions runtime for running logic apps locally and the local unit testing

```
code --install-extension Azurite.azurite
```

Check the info here:
https://docs.microsoft.com/en-us/azure/storage/common/storage-use-azurite?tabs=visual-studio-code


## Test Explorer for dotnet

To install the vs code test explorer for vscode run the below.

```
code --install-extension formulahendry.dotnet-test-explorer
```



