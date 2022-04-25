Feature: Ship Instruction Processor
	As a transport manager
    So that I can asssociate railcars to orders
    I want the transport system to be notified when an order is ready to be dispatched

@CleanTheSystem
Scenario: Ship Instruction Processor Green Path
	Given I have a request to dispatch an order
		| Key 		        | Value 		    |
		| DeliveryId 		| {{Guid}} 		    |		
        | DeliveryStatus 	| Scheduled 	    |
        | Commodity 	    | Petrochemical 	|
        | CargoId 	        | 28300 	        |
        | OrderPriority 	| 1 	            |
        | BillOfLading 	    |  	                |
	And the logic app test manager is setup
    When I send the message to the logic app
	Then the logic app will start running
	And the logic app will receive the message 	 
	And the logic app will parse the request message
    And the logic app will lookup data from the source system
    And the logic app will transform data to the destination format
		| Expected Notes 	|
		| order_no			|
		| plant_id			|
		| customer_no		|
    And the logic app will convert the message to the flat file format  
	And the logic app will send a reply
	And the logic app will complete successfully
	And the response from the logic app will be as expected		

@CleanTheSystem
Scenario: Ship Instruction Processor Not PetroChemical Event
	Given I have a request to dispatch an order
		| Key 		        | Value 		    	|
		| DeliveryId 		| {{Guid}} 		    	|		
        | DeliveryStatus 	| Scheduled 	    	|
        | Commodity 	    | Not-PetroChemical 	|
        | CargoId 	        | 28300 	        	|
        | OrderPriority 	| 1 	            	|
        | BillOfLading 	    |  	                	|
	And the logic app test manager is setup
    When I send the message to the logic app
	Then the logic app will start running
	And the logic app will receive the message 	 
	And the logic app will parse the request message
	And the logic app will identify the commodity is not Petrochemical    
	And the logic app will terminate	