using System.Net.Http;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Linq;
using System.Collections.Generic;  
using IPB.LogicApp.Standard.Testing.Local;
using IPB.LogicApp.Standard.Testing.Local.Host;
using IPB.LogicApp.Standard.Testing.Model.WorkflowRunActionDetails;
using IPB.LogicApp.Standard.Testing.Model.WorkflowRunOverview;
using IPB.LogicApp.Standard.Testing.Model;

namespace LogicApp.Testing.UnitTests.Helpers
{
    public class CommonTestContext
    {
        public CommonTestContext()
        {
            ManagementClient = new HttpClient();
            MockHttpHost = new MockHttpHost();
        }


        public HttpClient ManagementClient { get; set; }
        public MockHttpHost MockHttpHost { get; set; }

        public WorkflowTestHost WorkflowTestHost { get; set; }

        public LogicAppTestManager LogicAppTestManager { get; set; }
        
        public string Request { get; set; }

        public string RunId { get; set; }

        public WorkFlowResponse Response { get; set; }

        public string WorkFlowToTest { get; set; }


    }
    
}

