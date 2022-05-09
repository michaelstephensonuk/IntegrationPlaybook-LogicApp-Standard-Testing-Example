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
using System.Diagnostics;
using System.Threading.Tasks;

namespace logicapp.testing.unittests
{    
    ///Note: Ive had some issues with the functions runtime when installed via npm and chocolatey for it being on different paths and when running in 
    ///unit tests.  This test will let you test the func.exe is at the right place and the runtime can be started.    
    [TestClass]
    public class FunctionRuntimeTests
    {                

        public void KillFunc()
        {
            //Kill any func.exe before running test
            var processes = Process.GetProcessesByName("func");
            foreach (var funcProcess in processes)
            {
                funcProcess.Kill();
            }
        }

        [TestInitialize]
        public void TestInitialize()
        {
            KillFunc();
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            KillFunc();
        }

        [TestMethod]
        public void FuncFileExists()
        {
            var expectedPath = TestFramework.WorkflowTestHost.GetFuncPath();
            var exists = File.Exists(expectedPath);
            Assert.IsTrue(exists);
        }

        [TestMethod]
        public void CanStartFunctionRuntime()
        {
            var outputData = new List<string>();
            var errorData = new List<string>();

            var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = TestFramework.WorkflowTestHost.GetFuncPath(),
                        Arguments = "start --verbose",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true,
                    }
                };

            
            var processStarted = new TaskCompletionSource<bool>();

            process.OutputDataReceived += (sender, args) =>
                {
                    var outputData = args.Data;
                    Console.WriteLine(outputData);
                    if (outputData != null && outputData.Contains("Host started") && !processStarted.Task.IsCompleted)
                    {
                        processStarted.SetResult(true);
                    }
                };

            process.ErrorDataReceived += (sender, args) =>
                {
                    var errorData = args.Data;
                    Console.Write(errorData);
                };

            var started = process.Start();


            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            var processes = Process.GetProcessesByName("func");
            Assert.IsTrue(processes.Length == 1, "There should be 1 func.exe instance");
        }        
    }
}