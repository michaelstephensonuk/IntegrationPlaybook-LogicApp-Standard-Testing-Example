using IPB.LogicApp.Standard.Testing.Model.WorkflowRunActionDetails;
using IPB.LogicApp.Standard.Testing.Model.WorkflowRunOverview;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using TechTalk.SpecFlow;
using LogicApp.Testing.AcceptanceTests.Helpers;
using System.IO;

namespace LogicApp.Testing.AcceptanceTests.Schemas.ShipInstructionFlatFile
{
    
    [TestClass]
    public class Tests
    {
        
        [TestMethod, Priority(1), TestCategory("UnitTests")]
        public void ShipInstruction_Schema_Encode_GreenPath()
        {
            var workflowName = "Test-FlatFile-Encode";

            var inputContent = File.ReadAllText(@"..\..\..\Schemas\ShipInstructionFlatFile\Input.xml");
            var expectedContent = File.ReadAllText(@"..\..\..\Schemas\ShipInstructionFlatFile\Output.Expected.txt");

            //And the logic app test manager is setup
            var logicAppTestManager = LogicAppTestManagerBuilder.Build(workflowName);

	        //When I send the message to the logic app
            var content = new StringContent(inputContent, Encoding.UTF8, "text/xml");
            content.Headers.Add("schemaname", "ShipInstructionFlatFile.xsd");
            var response = logicAppTestManager.TriggerLogicAppWithPost(content);

            //Then the HTTP response from the workflow will be successful
	        var actualStatusCode = response.HttpResponse.StatusCode;
            Assert.AreEqual(System.Net.HttpStatusCode.OK, actualStatusCode);

            //And the response from the logic app will be as expected            
            var actualResponse = response.HttpResponse.Content.ReadAsStringAsync().Result;
            Assert.AreEqual(expectedContent, actualResponse);
        }
    }
}