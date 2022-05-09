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

namespace logicapp.testing.acceptancetests.Features.Echo.SpecFlow
{
    
    [Binding]
    [Scope(Feature = "Echo Workflow")]
    public class Steps
    {
        public CommonTestContext TestContext { get; set; }

        public Steps(CommonTestContext testContext)
        {
            TestContext = testContext;
            TestContext.WorkflowName = "Echo";
        }

        [Given(@"I have a request to send to the logic app")]
        public void GivenIHaveARequestToSendToTheLogicApp(Table table)
        {
            //This would be how to send an example message
            var message = new Dictionary<string, object>();
            foreach(var row in table.Rows)
            {
                message.Add(row[0], row[1]);
            }
            TestContext.Request = JsonConvert.SerializeObject(message);
        }

        [Given(@"the logic app test manager is setup")]
        public void GivenTheLogicAppTestManagerIsSetup()
        {
            //This will build the logic app test manager and configure it for running this workflow
            TestContext.LogicAppTestManager = LogicAppTestManagerBuilder.Build(TestContext.WorkflowName);
        }


        [When(@"I send the message to the logic app")]
        public void WhenISendTheMessageToTheLogicApp()
        {            
            var content = new StringContent(TestContext.Request, Encoding.UTF8, "application/json");
            TestContext.Response = TestContext.LogicAppTestManager.TriggerLogicAppWithPost(content);
        }

        [Then(@"the logic app will start running")]
        public void ThenTheLogicAppWillStartRunning()
        {
            //If we get a run id then we know the logic app got the message
            Assert.IsNotNull(TestContext.Response.WorkFlowRunId);

            //If the logic app started running we can load the run history at this point to start checking it later
            TestContext.LogicAppTestManager.LoadWorkflowRunHistory();
        }

        [Then(@"the logic app will receive the message")]
        public void ThenTheLogicAppWillReceiveTheMessage()
        {
            //We can check the trigger status was successful
            var triggerStatus = TestContext.LogicAppTestManager.GetTriggerStatus();
            Assert.AreEqual(triggerStatus, TriggerStatus.Succeeded);
        }

        [Then(@"the logic app will send a reply")]
        public void ThenTheLogicAppWillSendAReply()
        {
            var actionStatus = TestContext.LogicAppTestManager.GetActionStatus("Response");
            Assert.AreEqual(actionStatus, ActionStatus.Succeeded);
        }

        [Then(@"the logic app will complete successfully")]
        public void ThenTheLogicAppWillCompleteSuccessfully()
        {
            var workflowRunStatus = TestContext.LogicAppTestManager.GetWorkflowRunStatus();
            Assert.AreEqual(WorkflowRunStatus.Succeeded, workflowRunStatus);
        }

        [Then(@"the response from the logic app will be as expected")]
        public void ThenTheResponseFromTheLogicAppWillBeAsExpected(Table table)
        {
            var expectedName = table.Rows[0][0];
            var actualResponse = TestContext.Response.HttpResponse.Content.ReadAsStringAsync().Result;
            Assert.AreEqual(actualResponse, expectedName);		
        }
    }
}