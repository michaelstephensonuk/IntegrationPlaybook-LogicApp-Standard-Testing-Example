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
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace LogicApp.Testing.AcceptanceTests.ShipInstruction.SpecFlow
{
    
    [Binding]
    [Scope(Feature = "Ship Instruction Processor")]
    public class ProcessorSteps
    {
        private readonly ScenarioContext _scenarioContext;
        public CommonTestContext TestContext { get; set; }

        public ProcessorSteps(ScenarioContext scenarioContext, CommonTestContext testContext)
        {
            _scenarioContext = scenarioContext;
            TestContext = testContext;
            TestContext.WorkflowName = "ShipInstruction-Processor";
        }


        [Given(@"the logic app test manager is setup")]
        public void GivenTheLogicAppTestManagerIsSetup()
        {
            //This will build the logic app test manager and configure it for running this workflow
            TestContext.LogicAppTestManager = LogicAppTestManagerBuilder.Build(TestContext.WorkflowName);
        }

        [Given(@"I have a request to dispatch an order")]
        public void GivenIHaveARequestToDispatchAnOrder(Table table)
        {            
            StringBuilder sb = new StringBuilder();  
            XmlWriterSettings xws = new XmlWriterSettings();  
            xws.OmitXmlDeclaration = true;  
            xws.Indent = true;  

            using (XmlWriter xw = XmlWriter.Create(sb, xws)) {  
                XDocument doc = new XDocument();                                
                var deliveryNotifyElement = new XElement("DeliveryNotify");
                doc.Add(deliveryNotifyElement);

                var deliveryElement = new XElement("Delivery");                
                deliveryNotifyElement.Add(deliveryElement);

                //Add items from table
                foreach(var row in table.Rows)
                {
                    var rowKey = row[0];
                    var rowValue = row[1];

                    if(rowValue == "{{Guid}}")
                        rowValue = Guid.NewGuid().ToString();

                    deliveryElement.Add(new XElement(rowKey, rowValue));
                }

                
                
                doc.Save(xw);  
            }

            TestContext.Request = sb.ToString();
        }

        [When(@"I send the message to the logic app")]
        public void WhenISendTheMessageToTheLogicApp()
        {
            var content = new StringContent(TestContext.Request, Encoding.UTF8, "text/xml");
            TestContext.Response = TestContext.LogicAppTestManager.TriggerLogicAppWithPost(content);
        }

        [Then(@"the logic app will start running")]
        public void ThenTheLogicAppWillStartRunning()
        {
            //If we get a run id then we know the logic app got the message
            Assert.IsNotNull(TestContext.Response.WorkFlowRunId);

            //If the logic app started running we can load the run history at this point to start checking it later
            TestContext.LogicAppTestManager.LoadWorkflowRunHistory();
        }

        [Then(@"the logic app will receive the message")]
        public void ThenTheLogicAppWillReceiveTheMessage()
        {
            //We can check the trigger status was successful
            var triggerStatus = TestContext.LogicAppTestManager.GetTriggerStatus();
            Assert.AreEqual(TriggerStatus.Succeeded, triggerStatus);
        }

        [Then(@"the logic app will parse the request message")]
         public void ThenTheLogicAppWillParseTheRequestMessage()
         {
             var actionStatus = TestContext.LogicAppTestManager.GetActionStatus("Parse JSON - Ship Event");
            Assert.AreEqual(ActionStatus.Succeeded, actionStatus);
         }

        [Then(@"the logic app will lookup data from the source system")]
        public void ThenTheLogicAppWillLookupDataFromTheSourceSystem()
        {
            var actionStatus = TestContext.LogicAppTestManager.GetActionStatus("HTTP - Lookup Data");
            Assert.AreEqual(ActionStatus.Succeeded, actionStatus);
        }

        [Then(@"the logic app will transform data to the destination format")]
        public void ThenTheLogicAppWillTransformDataToTheDestinationFormat(Table table)
        {
            var actionStatus = TestContext.LogicAppTestManager.GetActionStatus("Transform JSON To TEXT");
            Assert.AreEqual(ActionStatus.Succeeded, actionStatus);

            //I can get the output action from the message and then make assertions on it
            var actionOutputMessage = TestContext.LogicAppTestManager.GetActionOutputMessage("Transform JSON To TEXT");
            var actionOutputMessageJson = JObject.Parse(actionOutputMessage);
            var actionMessage = actionOutputMessageJson["body"].Value<string>();    
            Assert.IsTrue(actionMessage.Contains("ns0:ShipOrder"));

            var xdoc = XDocument.Parse(actionMessage);    
            foreach(var row in table.Rows)
            {
                var expectedElementName = row[0];
                Assert.IsTrue(xdoc.Descendants(expectedElementName).Any(), $"The element {expectedElementName} does not exist");
            }
        }

        [Then(@"the logic app will convert the message to the flat file format")]
        public void ThenTheLogicAppWillConvertTheMessageToTheFlatFileFormat()
        {
            var actionStatus = TestContext.LogicAppTestManager.GetActionStatus("Flat File Encoding");
            Assert.AreEqual(ActionStatus.Succeeded, actionStatus);

            var actionOutputMessage = TestContext.LogicAppTestManager.GetActionOutputMessage("Flat File Encoding");
            var actionOutputMessageJson = JObject.Parse(actionOutputMessage);
            var actionMessage = actionOutputMessageJson["body"].Value<string>();
            Assert.IsTrue(actionMessage.StartsWith("$BOF"));
            Assert.IsTrue(actionMessage.EndsWith("$EOF\r\n"));
        }

        [Then(@"the logic app will send a reply")]
        public void ThenTheLogicAppWillSendAReply()
        {
            var actionStatus = TestContext.LogicAppTestManager.GetActionStatus("Response");
            Assert.AreEqual(ActionStatus.Succeeded, actionStatus);
        }

        [Then(@"the logic app will complete successfully")]
        public void ThenTheLogicAppWillCompleteSuccessfully()
        {
            var workflowRunStatus = TestContext.LogicAppTestManager.GetWorkflowRunStatus();
            Assert.AreEqual(WorkflowRunStatus.Succeeded, workflowRunStatus);
        }

        [Then(@"the response from the logic app will be as expected")]
        public void ThenTheResponseFromTheLogicAppWillBeAsExpected()
        {            
            var actualResponse = TestContext.Response.HttpResponse.Content.ReadAsStringAsync().Result;
            Assert.IsNotNull(actualResponse);
        }

        [Then(@"the logic app will identify the commodity is not Petrochemical")]
        public void ThenTheLogicAppWillIdentifyTheCommodityIsNotPetrochemical()
        {
            var actionStatus = TestContext.LogicAppTestManager.GetActionStatus("Compose - Log Not Petrochemical");
            Assert.AreEqual(ActionStatus.Succeeded, actionStatus);
        }

        [Then(@"the logic app will terminate")]
        public void ThenTheLogicAppWillTerminate()
        {
            var actionStatus = TestContext.LogicAppTestManager.GetActionStatus("Terminate - Not Petrochemical");
            Assert.AreEqual(ActionStatus.Succeeded, actionStatus);

            var workflowRunStatus = TestContext.LogicAppTestManager.GetWorkflowRunStatus();
            Assert.AreEqual(WorkflowRunStatus.Failed, workflowRunStatus);
        }
    }
}