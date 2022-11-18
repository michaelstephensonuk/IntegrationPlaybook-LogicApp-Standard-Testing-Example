using System.Net.Http;
using System.Net;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Dynamic;
using System.Text;
using LogicApp.Testing.UnitTests.Helpers;
using TechTalk.SpecFlow;
using IPB.LogicApp.Standard.Testing.Local;
using IPB.LogicApp.Standard.Testing.Local.Host;
using IPB.LogicApp.Standard.Testing.Model.WorkflowRunActionDetails;
using IPB.LogicApp.Standard.Testing.Model.WorkflowRunOverview;

namespace logicapp.testing.unittests.Workflows.Test_Stateful_HelloWorld.Specflow
{
    
    [Binding]
    [Scope(Feature = "Dataverse-WhoAmI")]
    public class Steps
    {
        public CommonTestContext TestContext { get; set; }

        public Steps(CommonTestContext testContext)
        {
            TestContext = testContext;
            TestContext.WorkFlowToTest = "Dataverse-WhoAmI";
        }

        [AfterScenario]        
        public void AfterScenario()
        {
            if (TestContext != null)
            {
                if(TestContext.MockHttpHost != null)
                {
                    TestContext.MockHttpHost.Dispose();
                }

                if(TestContext.WorkflowTestHost != null)
                {
                    TestContext.WorkflowTestHost.Dispose();
                }
            }
            
        }

        [Given(@"I have a request to send to the logic app")]
         public void GivenIHaveARequestToSendToTheLogicApp()
         {
            dynamic testInput = new ExpandoObject();            
            var inputMessage = JsonConvert.SerializeObject(testInput);

            TestContext.Request = inputMessage;
         }

        [Given(@"the logic app test manager is setup")]
         public void GivenTheLogicAppTestManagerIsSetup()
         {            
            //Setup the workflow test host and save to context
            var workflowTestHostBuilder = new WorkflowTestHostBuilder();
            workflowTestHostBuilder.Workflows.Add(TestContext.WorkFlowToTest);
            TestContext.WorkflowTestHost = workflowTestHostBuilder.LoadAndBuild();

            //Setup the client to call the workflow runtime and save to context
            var logicAppTestManager = new LogicAppTestManager(new LogicAppTestManagerArgs
            {
                WorkflowName = TestContext.WorkFlowToTest
            });
            logicAppTestManager.Setup();
            TestContext.LogicAppTestManager = logicAppTestManager;
         }

        [When(@"I send the message to the logic app")]
         public void WhenISendTheMessageToTheLogicApp()
         {
            var content = new StringContent("{}", Encoding.UTF8, "application/json");
            var response = TestContext.LogicAppTestManager.TriggerLogicAppWithPost(content);
            TestContext.Response = response;
         }

        [Then(@"the logic app will start running")]
         public void ThenTheLogicAppWillStartRunning()
         {
             TestContext.RunId = TestContext.Response.WorkFlowRunId;
             Assert.IsNotNull(TestContext.RunId);

            //Load the run history
            TestContext.LogicAppTestManager.LoadWorkflowRunHistory();
         }

        [Then(@"the logic app will receive the message")]
         public void ThenTheLogicAppWillReceiveTheMessage()
         {
            var actionStatus = TestContext.LogicAppTestManager.GetActionStatus("Compose");
            Assert.AreEqual(actionStatus, ActionStatus.Succeeded);
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
            var runStatus = TestContext.LogicAppTestManager.GetWorkflowRunStatus();
            Assert.AreEqual(runStatus, WorkflowRunStatus.Succeeded);
         }

        [Then(@"the logic app will call dataverse who am i")]
         public void ThenTheLogicAppWillCallDataverseWhoAmI()
         {
            var actionStatus = TestContext.LogicAppTestManager.GetActionStatus("HTTP_-_Dataverse_Who_Am_I");
            Assert.AreEqual(actionStatus, ActionStatus.Succeeded);
         }
         
    }
}