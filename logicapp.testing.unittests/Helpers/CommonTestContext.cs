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
    public class CommonTestContext
    {
        public CommonTestContext()
        {
            ManagementClient = new HttpClient();
            MockHttpHost = new MockHttpHost2();
        }


        public HttpClient ManagementClient { get; set; }
        public MockHttpHost2 MockHttpHost { get; set; }

        public WorkflowTestHost WorkflowTestHost { get; set; }

        public WorkflowTestHelper WorkflowTestHelper { get; set; }
        
        public string Request { get; set; }

        public string RunId { get; set; }

        public HttpResponseMessage Response { get; set; }

        public string WorkFlowToTest { get; set; }


    }
    
}

