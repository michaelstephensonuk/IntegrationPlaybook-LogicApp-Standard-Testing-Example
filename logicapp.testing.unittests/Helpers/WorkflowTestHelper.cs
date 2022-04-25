using System.Net.Http;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Linq;
using TestFramework;
using System.Collections.Generic;                 

namespace LogicApp.Testing.UnitTests.Helpers
{

    public class WorkflowTestHelper
    {
        private HttpClient _client;
        public WorkflowTestHelper(HttpClient client)
        {
            _client = client;
        }
        public System.Uri GetCallBackUrl(string workflowName)
        {
            var response = _client.PostAsync(TestEnvironment.GetTriggerCallbackRequestUri(flowName: workflowName, triggerName: "manual"), null).Result;
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var logicAppCallBackUrl = response.Content.ReadAsAsync<CallbackUrlDefinition>().Result.Value;
            return logicAppCallBackUrl;
        }

        public void AssertMostRecentRunWasSuccessful(string workflowName)
        {
            var checkRunsUrl = TestEnvironment.GetRunsRequestUriWithManagementHost(flowName: workflowName);
            var response = _client.GetAsync(checkRunsUrl).Result;
            var responseContent = response.Content.ReadAsAsync<JToken>().Result;
            Assert.AreEqual("Succeeded", responseContent["value"][0]["properties"]["status"].ToString());            
        }

        public string GetMostRecentRunId(string workflowName)
        {
            var checkRunsUrl = TestEnvironment.GetRunsRequestUriWithManagementHost(flowName: workflowName);
            var response = _client.GetAsync(checkRunsUrl).Result;
            var responseContent = response.Content.ReadAsAsync<JToken>().Result;
            Assert.AreEqual("Succeeded", responseContent["value"][0]["properties"]["status"].ToString());
            var runId = responseContent["value"].FirstOrDefault()["name"].ToString();
            return runId;
        }

        public void AssertActionResult(string workflowName, string runId, string actionName, string expectedResult = "Succeeded")
        {
            var getActionUrl = TestEnvironment.GetRunActionsRequestUri(flowName: workflowName, runName: runId);
            var response = _client.GetAsync(getActionUrl).Result;
            var responseContent = response.Content.ReadAsAsync<JToken>().Result;

            Assert.AreEqual(expectedResult, responseContent["value"].Where(actionResult => actionResult["name"].ToString().Equals(actionName)).FirstOrDefault()["properties"]["status"]);
        }

        public void AssertActionSucceeded(string workflowName, string runId, string actionName)
        {
            var getActionUrl = TestEnvironment.GetRunActionsRequestUri(flowName: workflowName, runName: runId);
            var response = _client.GetAsync(getActionUrl).Result;
            var responseContent = response.Content.ReadAsAsync<JToken>().Result;

            Assert.AreEqual("Succeeded", responseContent["value"].Where(actionResult => actionResult["name"].ToString().Equals(actionName)).FirstOrDefault()["properties"]["status"]);
        }
    }
}

