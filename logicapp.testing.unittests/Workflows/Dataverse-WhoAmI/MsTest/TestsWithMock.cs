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
using IPB.LogicApp.Standard.Testing.Local;
using IPB.LogicApp.Standard.Testing.Local.Host;
using IPB.LogicApp.Standard.Testing.Model.WorkflowRunActionDetails;
using IPB.LogicApp.Standard.Testing.Model.WorkflowRunOverview;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace logicapp.testing.unittests.Workflows.Dataverse_WhoAmI.MsTest
{
    
    [TestClass]
    public class TestsWithMock
    {
        
        [TestMethod, Priority(1), TestCategory("UnitTests")]
        public void DataverseWhoAmI_GreenPath_WithMock()
        {
            //Create the mock response we want
            dynamic mockedResponseObject = new ExpandoObject();  
            mockedResponseObject.BusinessUnitId = "BusinessUnitId";
            mockedResponseObject.UserId = "UserId";
            mockedResponseObject.OrganizationId = "OrganizationId";
            var mockedResponseMessage = JsonConvert.SerializeObject(mockedResponseObject);
            
            //Create an object for the input to my workflow
            dynamic testInput = new ExpandoObject();            
            var inputMessage = JsonConvert.SerializeObject(testInput);

            //Setup the workflow test host
            var workflowToTestName = "Dataverse-WhoAmI";
            var workflowTestHostBuilder = new WorkflowTestHostBuilder();
            workflowTestHostBuilder.Workflows.Add(workflowToTestName);
            
            workflowTestHostBuilder.Load();

            //Spin up the mock http host to redirect calls from the logic app
            using (var mockHost = new MockHttpHost())
            {
                //We will update some of the app settings to inject the mock host url
                dynamic appSettings = JsonConvert.DeserializeObject(workflowTestHostBuilder.AppSettingsJson);
                appSettings.Values.dataverse_url = mockHost.HostUri;
                workflowTestHostBuilder.AppSettingsJson = JsonConvert.SerializeObject(appSettings);

                //Setup the mock server to return responses
                mockHost.Server.Given(
                    Request.Create().WithPath("/api/data/v9.0/WhoAmI()")
                    .UsingGet()                                                
                )
                .RespondWith(
                    Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBody(mockedResponseMessage)
                );

                //Spin up the workflow host wrapper to run the workflows locally
                using (var workflowTestHost = workflowTestHostBuilder.Build())
                {
                    //Create the test manager to act as the client for testing the logic app
                    var logicAppTestManager = new LogicAppTestManager(new LogicAppTestManagerArgs
                    {
                        WorkflowName = workflowToTestName
                    });
                    logicAppTestManager.Setup();

                    //Trigger the workflow
                    var content = new StringContent("{}", Encoding.UTF8, "application/json");
                    var response = logicAppTestManager.TriggerLogicAppWithPost(content);

                    //Check you have a run id
                    Assert.IsNotNull(response.WorkFlowRunId);

                    //If the workflow started running we can load the run history at this point to start checking it later
                    logicAppTestManager.LoadWorkflowRunHistory();

                    //We can check the trigger status was successful
                    var triggerStatus = logicAppTestManager.GetTriggerStatus();
                    Assert.AreEqual(triggerStatus, TriggerStatus.Succeeded);

                    //Check the response action worked
                var actionStatus = logicAppTestManager.GetActionStatus("Compose");
                Assert.AreEqual(actionStatus, ActionStatus.Succeeded);

                actionStatus = logicAppTestManager.GetActionStatus("HTTP_-_Dataverse_Who_Am_I");
                Assert.AreEqual(actionStatus, ActionStatus.Succeeded);

                actionStatus = logicAppTestManager.GetActionStatus("Response");
                Assert.AreEqual(actionStatus, ActionStatus.Succeeded);

                //Check the run status completed successfully
                var workflowRunStatus = logicAppTestManager.GetWorkflowRunStatus();
                Assert.AreEqual(WorkflowRunStatus.Succeeded, workflowRunStatus);

                //Assert the response
                var responseText = response!.HttpResponse.Content!.ReadAsStringAsync()!.Result;
                dynamic logicAppResponse = JsonConvert.DeserializeObject(responseText);
                Assert.IsNotNull(logicAppResponse!.UserId, "The response did not contain a user id");
                }
            }
        }
    }
}