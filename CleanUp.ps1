#This is a powershell script which will clean up when we hit a problem with logic apps and sort any of the common issues we have.

#Step 1 - Kill common problem processes
#These processes often cause a problem if they have something locked so 
taskkill /IM dotnet.exe /F
taskkill /IM testhost.exe /F
taskkill /IM func.exe /F

#Step 2 - Clean the extension bundles directory
#When using vscode somehow it seems to occasionally add another version of the workflow bundle folder
#this will remove it
$directoryToCheck = $env:USERPROFILE + '\.azure-functions-core-tools\Functions\ExtensionBundles\Microsoft.Azure.Functions.ExtensionBundle.Workflows\2.8.4'

$pathExists = Test-Path -Path $directoryToCheck
if($pathExists){
    Write-Host 'The 2.8.4 bundle directory exists so removing it'
    Remove-Item $directoryToCheck -Recurse
}
