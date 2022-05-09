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

    public class WorkflowTestHostBuilder
    {
        public string LogicAppFolder{ get; set; }
        public string AppSettingsPath{ get; set; }
        public string ConnectionsPath{ get; set; }
        public string ParametersPath{ get; set; }

        public string AppSettingsJson{ get; set; }
        public string ConnectionsJson{ get; set; }
        public string ParametersJson{ get; set; }

        public string HostJson{ get; set; }

        public string HostPath{ get; set; }

        public List<WorkflowTestInput> WorkflowDefinitions { get; set; }

        public List<string> Workflows{ get; set; }

        public WorkflowTestHostBuilder()
        {
            LogicAppFolder = @"..\..\..\..\logicapp\";
            AppSettingsPath = Path.Combine(LogicAppFolder, "local.settings.json");
            ConnectionsPath = Path.Combine(LogicAppFolder, "connections.json");
            HostPath = Path.Combine(LogicAppFolder, "host.json");
            ParametersPath = Path.Combine(LogicAppFolder, "parameters.json");
            Workflows = new List<string>();
            WorkflowDefinitions = new List<WorkflowTestInput>();
        }

        private void ValidatePath(string relativePath)
        {
            var fullPath = Path.GetFullPath(relativePath);
            if(File.Exists(fullPath))
            {
                Console.WriteLine($"The file {fullPath} does exist");
            }
            else
            {
                Console.WriteLine($"The file {fullPath} does not exist");
                throw new Exception($"The file {fullPath} does not exist");
            }
        }
        
        public void Load()
        {
            ValidatePath(ConnectionsPath);
            ValidatePath(AppSettingsPath);
            ValidatePath(HostPath);
            ValidatePath(ParametersPath);

            //Read the various files for the logic app
            ConnectionsJson = File.ReadAllText(ConnectionsPath);            
            AppSettingsJson = File.ReadAllText(AppSettingsPath);
            HostJson = File.ReadAllText(HostPath);
            ParametersJson = File.ReadAllText(ParametersPath);
            
            //Iterate over the loaded workflows and load them too
            foreach(var workflowName in Workflows)
            {
                var workflowPath = @$"..\..\..\..\logicapp\{workflowName}\workflow.json";
                var workflowDefinitionJson = File.ReadAllText(workflowPath);
                
                WorkflowDefinitions.Add(new WorkflowTestInput(functionName: workflowName, flowDefinition: workflowDefinitionJson));
            }
        }

        public WorkflowTestHost Build()
        {            
            //Load the workflow test host and return it
            var workflowTestHost = new WorkflowTestHost(
                WorkflowDefinitions.ToArray(), 
                localSettings: AppSettingsJson, 
                connectionDetails: ConnectionsJson,
                parameters: ParametersJson,
                host: HostJson);

            return workflowTestHost;
        }

        public WorkflowTestHost LoadAndBuild()
        {
            Load();
            return Build();
        }
    }
}

