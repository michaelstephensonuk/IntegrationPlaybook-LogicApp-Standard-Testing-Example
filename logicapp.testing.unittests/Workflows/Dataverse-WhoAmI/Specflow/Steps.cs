using System.Net.Http;
using System.Net;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using TestFramework;
using System.Dynamic;
using System.Text;
using LogicApp.Testing.UnitTests.Helpers;
using TechTalk.SpecFlow;

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
            if (TestContext != null && TestContext.MockHttpHost != null)
            {
                TestContext.MockHttpHost.Dispose();
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
            var workflowTestHostBuilder = new WorkflowTestHostBuilder();
            workflowTestHostBuilder.Workflows.Add(TestContext.WorkFlowToTest);

            TestContext.WorkflowTestHost = workflowTestHostBuilder.LoadAndBuild();
            TestContext.WorkflowTestHelper = new WorkflowTestHelper(TestContext.ManagementClient);
         }

        [When(@"I send the message to the logic app")]
         public void WhenISendTheMessageToTheLogicApp()
         {
            var logicAppCallBackUrl = TestContext.WorkflowTestHelper.GetCallBackUrl(TestContext.WorkFlowToTest);
            var workFlowRequestContent = new StringContent(TestContext.Request, Encoding.UTF8, "application/json");
            var response = TestContext.ManagementClient.PostAsync(logicAppCallBackUrl, workFlowRequestContent).Result;
            TestContext.Response = response;
            
         }

        [Then(@"the logic app will start running")]
         public void ThenTheLogicAppWillStartRunning()
         {
             TestContext.RunId = TestContext.WorkflowTestHelper.GetMostRecentRunId(TestContext.WorkFlowToTest);
         }

        [Then(@"the logic app will receive the message")]
         public void ThenTheLogicAppWillReceiveTheMessage()
         {
             TestContext.WorkflowTestHelper.AssertActionSucceeded(TestContext.WorkFlowToTest, TestContext.RunId, "Compose");
         }

        [Then(@"the logic app will send a reply")]
         public void ThenTheLogicAppWillSendAReply()
         {
             TestContext.WorkflowTestHelper.AssertActionSucceeded(TestContext.WorkFlowToTest, TestContext.RunId, "Response");
             Assert.AreEqual(HttpStatusCode.OK, TestContext.Response.StatusCode); 
         }

        [Then(@"the logic app will complete successfully")]
         public void ThenTheLogicAppWillCompleteSuccessfully()
         {             
             TestContext.WorkflowTestHelper.AssertMostRecentRunWasSuccessful(TestContext.WorkFlowToTest);
         }

        [Then(@"the logic app will call dataverse who am i")]
         public void ThenTheLogicAppWillCallDataverseWhoAmI()
         {
            TestContext.WorkflowTestHelper.AssertActionSucceeded(TestContext.WorkFlowToTest, TestContext.RunId, "HTTP_-_Dataverse_Who_Am_I");
         }
         
    }
}