Feature: Ship Instruction Service Bus Receiver
	As a transport manager
    So that I can asssociate railcars to orders
    I want the transport system to be notified when an order is ready to be dispatched

@CleanTheSystem
Scenario: Ship Instruction Receiver Green Path
	Given I have a request to dispatch an order
		| Key 		        | Value 		    |
		| DeliveryId 		| {{Guid}} 		    |		
        | DeliveryStatus 	| Scheduled 	    |
        | Commodity 	    | Petrochemical 	|
        | CargoId 	        | 28300 	        |
        | OrderPriority 	| 1 	            |
        | BillOfLading 	    |  	                |
	And the logic app test manager is setup
	When I send the message to the service bus
	And I wait a short period to let the logic app complete    
	Then we will check for the most recent logic app run
	And the logic app will start running
	And the logic app will receive the message  
	And the logic app will track the delivery id  
	And the logic app will call the child processor workflow
	And the logic app will complete successfully