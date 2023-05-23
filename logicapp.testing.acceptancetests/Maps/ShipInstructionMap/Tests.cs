using IPB.LogicApp.Standard.Testing.Model.WorkflowRunActionDetails;
using IPB.LogicApp.Standard.Testing.Model.WorkflowRunOverview;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using TechTalk.SpecFlow;
using System.IO;
using logicapp.testing.acceptancetests.Helpers;
using IPB.LogicApp.Standard.Testing;

namespace logicapp.testing.acceptancetests.Maps.ShipInstructionMap
{
    
    [TestClass]
    public class Tests
    {
        
        [TestMethod, Priority(1), TestCategory("UnitTests")]
        public void ShipInstruction_Map_GreenPath()
        {
            var workflowName = "Test-Map-Liquid-JsonToText";

            var inputContent = File.ReadAllText(@"..\..\..\Maps\ShipInstructionMap\Input.json");
            var expectedContent = File.ReadAllText(@"..\..\..\Maps\ShipInstructionMap\Output.Expected.xml");

            //And the logic app test manager is setup
            var logicAppTestManager = LogicAppTestManagerBuilder.Build(workflowName);

	        //When I send the message to the logic app
            var content = new StringContent(inputContent, Encoding.UTF8, "application/json");
            content.Headers.Add("mapname", "ShipInstructionMap.liquid");
            var response = logicAppTestManager.TriggerLogicAppWithPost(content);

            //Then the HTTP response from the workflow will be successful
	        var actualStatusCode = response.HttpResponse.StatusCode;
            Assert.AreEqual(System.Net.HttpStatusCode.OK, actualStatusCode);

            //And the response from the logic app will be as expected            
            var actualResponse = response.HttpResponse.Content.ReadAsStringAsync().Result;
            var compareResult = XmlCompareHelper.CompareXmlStringsWithXDoc(expectedContent, actualResponse);
            Assert.IsTrue(compareResult, "The result from the map is not as expected");            
        }
    }
}