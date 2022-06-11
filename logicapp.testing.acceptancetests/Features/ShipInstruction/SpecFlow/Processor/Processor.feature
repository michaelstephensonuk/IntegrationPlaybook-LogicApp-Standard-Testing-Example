#This tag will create a hyperlink in the documentation to a work item with the devops task
@DEVOPS_WI:55
Feature: Ship Instruction Processor
	As a transport manager
    So that I can asssociate railcars to orders
    I want the transport system to be notified when an order is ready to be dispatched

<a href="https://tfscsc.visualstudio.com/_git/IntegrationPlaybook?path=/vs-code-logicapp-testing/Documentation.ShipInstruction.md">Click here for more info</a>


@DEVOPS_WI:56
@CleanTheSystem
Scenario: Ship Instruction Processor Green Path

This is the default use case where we get a valid event and then lookup data from the energy system
and transform it to the destination flat file format and deliver it to the transport system.

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

@DEVOPS_WI:57
@CleanTheSystem
Scenario: Ship Instruction Processor Not PetroChemical Event

This is the case where the event is not related to the PetroChemical commodity so we will ignore the
event and not process it to other systems

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