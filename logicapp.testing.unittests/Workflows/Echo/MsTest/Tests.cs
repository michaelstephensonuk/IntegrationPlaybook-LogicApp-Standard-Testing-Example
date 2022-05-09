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
using System;

namespace logicapp.testing.unittests.Workflows.Echo.MsTest
{
    
    [TestClass]
    public class Tests
    {        
        
        [TestMethod, Priority(1), TestCategory("UnitTests")]
        public void Echo_GreenPath()
        {
            //Create an object for the input to my workflow
            dynamic testInput = new ExpandoObject();
            testInput.Name = "Michael";
            testInput.Surname = "Stephenson";
            var inputMessage = JsonConvert.SerializeObject(testInput);

            //Setup the workflow test host
            var workflowToTestName = "Echo";
            var workflowTestHostBuilder = new WorkflowTestHostBuilder();
            workflowTestHostBuilder.Workflows.Add(workflowToTestName);

            using (var workflowTestHost = workflowTestHostBuilder.LoadAndBuild())
            {                
                using (var client = new HttpClient())
                {
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
                    workflowTestHelper.AssertActionSucceeded(workflowToTestName, runId, "Response");      
                }
            }
        }
    }
}