using IPB.LogicApp.Standard.Testing;
using IPB.LogicApp.Standard.Testing.Model.WorkflowRunActionDetails;
using IPB.LogicApp.Standard.Testing.Model.WorkflowRunOverview;
using logicapp.testing.acceptancetests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using TechTalk.SpecFlow;


namespace logicapp.testing.acceptancetests.Features.Echo.MsTest
{
    
    [TestClass]
    public class Tests
    {
        
        [TestMethod, Priority(1), TestCategory("UnitTests")]
        public void MsTest_Echo_GreenPath()
        {
            var testContext = new CommonTestContext();            
            testContext.WorkflowName = "Echo";

            //Given I have an input test message
            var message = new Dictionary<string, object>();
            message.Add("Name", "Michael");
            message.Add("Surname", "Stephenson");
            testContext.Request = JsonConvert.SerializeObject(message);

            //And the logic app test manager is setup
            testContext.LogicAppTestManager = LogicAppTestManagerBuilder.Build(testContext.WorkflowName);

	        //When I send the message to the logic app
            var content = new StringContent(testContext.Request, Encoding.UTF8, "application/json");
            testContext.Response = testContext.LogicAppTestManager.TriggerLogicAppWithPost(content);

	        //Then the logic app will start running and we can check the run id was created
            Assert.IsNotNull(testContext.Response.WorkFlowRunId);

            //Then we will load the workflow run history
            testContext.LogicAppTestManager.LoadWorkflowRunHistory();

            //Then we will check the trigger ran successfully
            var triggerStatus = testContext.LogicAppTestManager.GetTriggerStatus();
            Assert.AreEqual(triggerStatus, TriggerStatus.Succeeded);

	        //Then we will check the response action ran so we know the workflow sent a reply
            var actionStatus = testContext.LogicAppTestManager.GetActionStatus("Response");
            Assert.AreEqual(actionStatus, ActionStatus.Succeeded);

	        //And the logic app will complete successfully by checking the run status
            var workflowRunStatus = testContext.LogicAppTestManager.GetWorkflowRunStatus();
            Assert.AreEqual(WorkflowRunStatus.Succeeded, workflowRunStatus);

            //Then the HTTP response from the workflow will be successful
	        var actualStatusCode = testContext.Response.HttpResponse.StatusCode;
            Assert.AreEqual(System.Net.HttpStatusCode.OK, actualStatusCode);

            //And the response from the logic app will be as expected
            var expectedName = "Michael Stephenson";
            var actualResponse = testContext.Response.HttpResponse.Content.ReadAsStringAsync().Result;
            Assert.AreEqual(actualResponse, expectedName);		
        }
    }
}