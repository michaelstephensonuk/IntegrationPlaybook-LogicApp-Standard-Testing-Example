

# Useful Commands Cheatsheet

```
cd logicapp.testing.acceptancetests
cd ..\logicapp.testing.acceptancetests
```

## Run tests to see logs
```
dotnet test --logger:"console;verbosity=normal"
dotnet test --logger:"console;verbosity=detailed"
```

## Echo Test - MsTest

```
dotnet test --filter Name=MsTest_Echo_GreenPath
```

## Echo Test - Specflow

```
dotnet test --filter Name=EchoSpecflowGreenPath
```


## Dataverse Who Am I Test

```
dotnet test --filter Name=DataverseWhoAmIGreenPath
```

## Ship Instruction Processor

```
dotnet test --filter Name=ShipInstructionProcessorGreenPath --logger:"console;verbosity=detailed"

dotnet test --filter Name=ShipInstructionProcessorNotPetroChemicalEvent --logger:"console;verbosity=detailed"


```

## Ship Instruction Receiver

```
dotnet test --filter Name=ShipInstructionReceiverGreenPath --logger:"console;verbosity=detailed"

```

## Map Test

```
dotnet test --filter Name=ShipInstruction_Map_GreenPath --logger:"console;verbosity=detailed"

```

## Schema Test

```
dotnet test --filter Name=ShipInstruction_Schema_Encode_GreenPath --logger:"console;verbosity=detailed"

```

