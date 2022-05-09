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
            
            //We will load and build the workflow test host in 2 seperate steps so we can inject the mock response
            workflowTestHostBuilder.Load();

            //We will update some of the app settings to inject the mock host
            dynamic appSettings = JsonConvert.DeserializeObject(workflowTestHostBuilder.AppSettingsJson);
            appSettings.Values.dataverse_url = TestEnvironment.FlowV2MockTestHostUri;
            workflowTestHostBuilder.AppSettingsJson = JsonConvert.SerializeObject(appSettings);

            using (var workflowTestHost = workflowTestHostBuilder.Build())
            {                
                using (var mockHost = new MockHttpHost2())
                using (var client = new HttpClient())
                {           
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

                    //Setup Test Helper for running the workflow
                    var workflowTestHelper = new WorkflowTestHelper(client);

                    // Get workflow callback URL.                    
                    var logicAppCallBackUrl = workflowTestHelper.GetCallBackUrl(workflowToTestName);

                    // Run the workflow.
                    var workFlowRequestContent = new StringContent(inputMessage, Encoding.UTF8, "application/json");
                    var response = client.PostAsync(logicAppCallBackUrl, workFlowRequestContent).Result;
                    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);                

                    // Check workflow run status.
                    // Note this makes an assumption its the most recent run (need to check on this)
                    workflowTestHelper.AssertMostRecentRunWasSuccessful(workflowToTestName);

                    //Get the run id for the run
                    var runId = workflowTestHelper.GetMostRecentRunId(workflowToTestName);
                    
                    //Check Actions run
                    workflowTestHelper.AssertActionSucceeded(workflowToTestName, runId, "Compose");    
                    workflowTestHelper.AssertActionSucceeded(workflowToTestName, runId, "HTTP_-_Dataverse_Who_Am_I");    
                    workflowTestHelper.AssertActionSucceeded(workflowToTestName, runId, "Response");  
                        
                    var responseText = response!.Content!.ReadAsStringAsync()!.Result;
                    dynamic logicAppResponse = JsonConvert.DeserializeObject(responseText);
                    Assert.IsNotNull(logicAppResponse!.UserId, "The response did not contain a user id");
                }
            }
        }
    }
}