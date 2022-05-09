This is just a scratchpad with some notes for the functions runtime


# Setup Path for npm when installing functions core tools

```
$env:Path += ';%AppData%\npm'
```

# Installing with Chocolatey

With chocolatey it is the below commands but its preferred to use npm as the unit tests are setup using
a check for the install path of func.exe which assumes you have installed with npm.


```
choco install azure-functions-core-tools-3

or

choco install azure-functions-core-tools-3 --version 3.0.3785 -y --params "'/x64'"  --force
```

You would need to change the paths in this file if you change how functions core tools is installed:

logicapp.testing.unittests\TestFramework\WorkflowTestHost .cs

# Uninstall Chocolatey

If you need to go back from chocolatey to npm install

```
choco uninstall azure-functions-core-tools-3
```



# Build Pipeline

If you are using chocolatey in the devops pipeline you can use the below to install functions core tools rather than npm

```
    #What: Install Chocolatey on the build agent
    #Why: We might want to use some packages in the build so we will install chocolatey
    - task: gep13.chocolatey-azuredevops.chocolatey-tool-installer-azuredevops.ChocolateyToolInstaller@0
      displayName: 'Use Chocolatey'

    #What: Install Func core tools with chocolatey 
    #Why: If we install with npm then it doesnt set the path variable    
    - task: CmdLine@2
      displayName: 'Install Functions Core Tools'
      inputs:
        script: 'choco install azure-functions-core-tools-3 --version 3.0.3785 -y --params "''/x64''"' 

```