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

namespace LogicApp.Testing.UnitTests.Workflows.Echo_Postman.MsTest
{
    
    [TestClass]
    public class Tests
    {
        
        [TestMethod, Priority(1), TestCategory("UnitTests")]
        public void Echo_Postman_GreenPath()
        {
            //This is the value returned by postman
            var expectedUrl = "https://postman-echo.com/get";

            //Create an object for the input to my workflow
            dynamic testInput = new ExpandoObject();            
            var inputMessage = JsonConvert.SerializeObject(testInput);

            //Setup the workflow test host
            var workflowToTestName = "Echo-Postman";
            var workflowTestHostBuilder = new WorkflowTestHostBuilder();
            workflowTestHostBuilder.Workflows.Add(workflowToTestName);
            
            //We will load and build the workflow test host in 2 seperate steps so we can inject the mock response if we want
            workflowTestHostBuilder.Load();

            using (var workflowTestHost = workflowTestHostBuilder.Build())
            {                
                using (var client = new HttpClient())
                {
                    //Note: There is no mock used here, we will just let the logic app ping postman as usual

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
                    workflowTestHelper.AssertActionSucceeded(workflowToTestName, runId, "HTTP_-_Postman_Echo");    
                    workflowTestHelper.AssertActionSucceeded(workflowToTestName, runId, "Response");  
                        
                    //Assert the response
                    var responseText = response!.Content!.ReadAsStringAsync()!.Result;
                    var logicAppResponse = JToken.Parse(responseText);
                    Assert.AreEqual(expectedUrl, logicAppResponse["url"], "The url value is not as expected");
                }
            }
        }
    }
}