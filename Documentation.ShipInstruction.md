This is example documentation for the ship instruction interface.

# Ship Instruction
The ship instruction interface is triggered by events from the energy system when data changes.  This interface is interested in events related to orders which need to be sent to the transport system to instruct movements of railcars to fulfil customer orders.

## Component View

If we look at the systems involved in this interface it looks like the below.

```mermaid
  erDiagram
    Energy-System }|..|{ Integration-Platform : ""
    Transport-System }|..|{ Integration-Platform : ""
```

## Sequence View
If we look at the primary flow of this interface it looks like the below diagram.

```mermaid
  sequenceDiagram
    User->>Energy System: Update Order
    Energy System->>Event API: Publish event
    Event API->>Service Bus: Publish event
    Logic App->>Service Bus: Get message
    Logic App->>Energy System: Get additional data
    Logic App->>Logic App: Transform Data
    Logic App->>Logic App: Encode in flat file format
    Logic App->>Transport System: Deliver over SFTP
    Logic App->>Service Bus: Complete Message
            
```