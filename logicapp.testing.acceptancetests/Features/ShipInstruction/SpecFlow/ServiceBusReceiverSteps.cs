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
using System.Threading;
using Newtonsoft.Json.Linq;

namespace LogicApp.Testing.AcceptanceTests.ShipInstruction.SpecFlow
{
    
    [Binding]
    [Scope(Feature = "Ship Instruction Service Bus Receiver")]
    public class ServiceBusReceiverSteps
    {
        private readonly ScenarioContext _scenarioContext;
        public CommonTestContext TestContext { get; set; }

        public ServiceBusReceiverSteps(ScenarioContext scenarioContext, CommonTestContext testContext)
        {
            _scenarioContext = scenarioContext;
            TestContext = testContext;
            TestContext.WorkflowName = "ShipInstruction-Receiver";
        }


        [Given(@"the logic app test manager is setup")]
        public void GivenTheLogicAppTestManagerIsSetup()
        {
            //This will build the logic app test manager and configure it for running this workflow
            TestContext.LogicAppTestManager = LogicAppTestManagerBuilder.Build(TestContext.WorkflowName!);
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

                    if(rowKey == "DeliveryId")
                        _scenarioContext.Add("DeliveryId", rowValue);

                    deliveryElement.Add(new XElement(rowKey, rowValue));
                }

                
                
                doc.Save(xw);  
            }

            TestContext.Request = sb.ToString();            
        }

        [When(@"I send the message to the service bus")]
        public void WhenISendTheMessageToTheServiceBus()
        {
            var deliveryId = _scenarioContext.Get<string>("DeliveryId");
            _scenarioContext.Add("TestStartTime", DateTime.UtcNow);

            var messageBody = TestContext.Request;
            
            var customProperties = new Dictionary<string, object>();
            customProperties.Add("DeliveryId", deliveryId);

            var task = Helpers.ServiceBusHelper.SendMessage("ms-la-testing-dev-shipinstruction", messageBody, "text/xml", customProperties, deliveryId);
            Assert.IsTrue(task.IsCompletedSuccessfully);
        }

        [When(@"I wait a short period to let the logic app complete")]
        public void WhenIWaitAShortPeriodToLetTheLogicAppComplete()
        {
            //Delay to allow the logic app to process the message from the queue
            Thread.Sleep(new TimeSpan(0, 0, 30));
        }

        //Then we will check for the most recent logic app run
        [Then(@"we will check for the most recent logic app run")]
        public void ThenWeWillCheckForTheMostRecentLogicAppRun()
        {
            var startTime = _scenarioContext.Get<DateTime>("TestStartTime");
            var run = TestContext.LogicAppTestManager.GetMostRecentRunSince(startTime);            
            _scenarioContext.Add("RunId", run.name);
        }


        [Then(@"the logic app will start running")]
        public void ThenTheLogicAppWillStartRunning()
        {
            var runId = _scenarioContext.Get<string>("RunId");
            TestContext.LogicAppTestManager.LoadWorkflowRunHistory(runId);
        }

        [Then(@"the logic app will receive the message")]
        public void ThenTheLogicAppWillReceiveTheMessage()
        {
            //We can check the trigger status was successful
            var triggerStatus = TestContext.LogicAppTestManager.GetTriggerStatus();
            Assert.AreEqual(triggerStatus, TriggerStatus.Succeeded);
        }

        [Then(@"the logic app will track the delivery id")]
        public void ThenTheLogicAppWillTrackTheDeliveryId()
        {
            var expectedDeliveryId = _scenarioContext.Get<string>("DeliveryId");

            JToken action = TestContext.LogicAppTestManager.GetActionJson("Initialize variable - Delivery ID");
            var actualDeliveryId = action["trackedProperties"]?["DeliveryId"]?.Value<string>();
            Assert.AreEqual(expectedDeliveryId, actualDeliveryId);
        }

        [Then(@"the logic app will call the child processor workflow")]
        public void ThenTheLogicAppWillCallTheChildProcessorWorkflow()
        {
            var actionStatus = TestContext.LogicAppTestManager.GetActionStatus("Call Ship Instruction Processor");
            Assert.AreEqual(ActionStatus.Succeeded, actionStatus);
        }

        [Then(@"the logic app will complete successfully")]
        public void ThenTheLogicAppWillCompleteSuccessfully()
        {
            var workflowRunStatus = TestContext.LogicAppTestManager.GetWorkflowRunStatus();
            Assert.AreEqual(WorkflowRunStatus.Succeeded, workflowRunStatus);
        }
    }
}