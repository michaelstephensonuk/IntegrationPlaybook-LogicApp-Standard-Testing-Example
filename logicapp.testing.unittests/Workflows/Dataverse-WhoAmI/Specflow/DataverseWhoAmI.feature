Feature: Dataverse-WhoAmI
	Simple test of a basic stateful logic app

@SpecflowTests
@KillFuncBeforeTest
Scenario: Dataverse-WhoAmI-Green Path-Specflow
	Given I have a request to send to the logic app
	And the logic app test manager is setup
	When I send the message to the logic app
	Then the logic app will start running
	And the logic app will receive the message
	And the logic app will call dataverse who am i
	And the logic app will send a reply
	And the logic app will complete successfully