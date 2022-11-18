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
using System;
using IPB.LogicApp.Standard.Testing.Local;
using IPB.LogicApp.Standard.Testing.Local.Host;
using IPB.LogicApp.Standard.Testing.Model.WorkflowRunActionDetails;
using IPB.LogicApp.Standard.Testing.Model.WorkflowRunOverview;

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
            
            //Spin up the workflow host wrapper to run the workflows locally
            using (var workflowTestHost = workflowTestHostBuilder.LoadAndBuild())
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
                var actionStatus = logicAppTestManager.GetActionStatus("Response");
                Assert.AreEqual(actionStatus, ActionStatus.Succeeded);

                //Check the run status completed successfully
                var workflowRunStatus = logicAppTestManager.GetWorkflowRunStatus();
                Assert.AreEqual(WorkflowRunStatus.Succeeded, workflowRunStatus);
            }
        }
    }
}