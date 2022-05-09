using System.Net.Http;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Linq;
using TestFramework;
using System.Collections.Generic;
using System;

namespace LogicApp.Testing.UnitTests.Helpers
{

    public class WorkflowTestHostHelper
    {
        public static void LogHostInfo(WorkflowTestHost host)
        {
            Console.WriteLine("Test Host Errors:");
            foreach(var error in host.ErrorData)
            {
                Console.WriteLine(error);
            }

            Console.WriteLine("Test Host Log:");
            foreach(var msg in host.OutputData)
            {
                Console.WriteLine(msg);
            }
        }
    }
}

