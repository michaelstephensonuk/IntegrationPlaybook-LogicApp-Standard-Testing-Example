@DEVOPS_WI:53
Feature: Dataverse Who Am I
	Simple test of a basic workflow which will connect to Dataverse


@CleanTheSystem
Scenario: Dataverse Who Am I Green Path
	Given I have a request to send to the logic app
	And the logic app test manager is setup
	When I send the message to the logic app
	Then the logic app will start running
	And the logic app will receive the message
    And the logic app will call the dataverse api
	And the logic app will send a reply
	And the logic app will complete successfully