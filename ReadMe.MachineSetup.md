# Prerequsites

## Chocolatey
To make it easier to install things we will use chocolatey so we will need to install it.



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

## Function Core Tools
Make sure function core tools is installed.  This is used for running the tests locally on your developer machine.
More info:
https://github.com/Azure/azure-functions-core-tools#installing

With npm in vs code it is

```
npm install -g azure-functions-core-tools@3 --unsafe-perm true
```

With chocolatey it is:

```
choco install azure-functions-core-tools-3

or

choco install azure-functions-core-tools-3 --version 3.0.3785 -y --params "'/x64'"  --force
```




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







