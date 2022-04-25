Feature: Echo Workflow
	Simple test of a basic workflow which will echo back a response

@CleanTheSystem
Scenario: Echo Specflow Green Path
	Given I have a request to send to the logic app
		| Key 		| Value 		|
		| Name 		| Michael 		|
		| Surname 	| Stephenson 	|
	And the logic app test manager is setup
	When I send the message to the logic app
	Then the logic app will start running
	And the logic app will receive the message    
	And the logic app will send a reply
	And the logic app will complete successfully
	And the response from the logic app will be as expected
		| Name					|
		| Michael Stephenson 	|