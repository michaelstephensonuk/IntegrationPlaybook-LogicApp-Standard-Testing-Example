// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace TestFramework
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    /// <summary>
    /// The function test host.
    /// </summary>
    public class WorkflowTestHost : IDisposable
    {
        public static string GetFuncPath()
        {
            var funcPath = GetFuncPathOnLocalMachine();
            if(File.Exists(funcPath))
                return funcPath;
            else
            {
                funcPath = GetFuncPathOnBuildAgent();
                if(File.Exists(funcPath))
                    return funcPath;
                else
                {
                    throw new Exception("The func.exe does not exist at the expected paths");
                }
            }
        }

        ///When func is installed by npm on your dev machine it does into this path by default
        public static string GetFuncPathOnLocalMachine()
        {
            var funcPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\npm\\node_modules\\azure-functions-core-tools\\bin\\func.exe";
            return funcPath;
        }

        ///When npm installs func.exe on the build agent it does into this path by default
        public static string GetFuncPathOnBuildAgent()
        {
            return "C:\\npm\\prefix\\node_modules\\azure-functions-core-tools\\bin\\func.exe";
        }

        /// <summary>
        /// Get or sets the Output Data.
        /// </summary>
        public List<string> OutputData { get; private set; }

        /// <summary>
        /// Gets or sets the error data.
        /// </summary>
        public List<string> ErrorData { get; private set; }

        /// <summary>
        /// Gets or sets the Function process.
        /// </summary>
        public Process Process { get; set; }

        /// <summary>
        /// Gets or sets the Working directory.
        /// </summary>
        public string WorkingDirectory { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkflowTestHost"/> class.
        /// </summary>
        public WorkflowTestHost(WorkflowTestInput[] inputs = null, string localSettings = null, string parameters = null, string connectionDetails = null, string host = null, DirectoryInfo artifactsDirectory = null)
        {
            //TODO: Note I have made a change here so there is a sub folder with all of the workflow runs in it so its easy to remove them
            //Original code:
            //this.WorkingDirectory = Path.Combine(Directory.GetCurrentDirectory(), Guid.NewGuid().ToString());

            var baseTestDirectory = Path.Combine(Directory.GetCurrentDirectory(), "LogicApp-Tests-Dir");
            if(Directory.Exists(baseTestDirectory) == false)
                Directory.CreateDirectory(baseTestDirectory);

            this.WorkingDirectory = Path.Combine(baseTestDirectory, Guid.NewGuid().ToString());

            this.OutputData = new List<string>();
            this.ErrorData = new List<string>();

            this.StartFunctionRuntime(inputs, localSettings, parameters, connectionDetails, host, artifactsDirectory);
        }

        /// <summary>
        /// Starts the function runtime.
        /// </summary>
        protected void StartFunctionRuntime(WorkflowTestInput[] inputs, string localSettings, string parameters, string connectionDetails, string host, DirectoryInfo artifactsDirectory)
        {
            try
            {
                var processes = Process.GetProcessesByName("func");
                foreach (var process in processes)
                {
                    process.Kill();
                }

                Directory.CreateDirectory(this.WorkingDirectory);

                if (inputs != null && inputs.Length > 0)
                {
                    foreach (var input in inputs)
                    {
                        if (!string.IsNullOrEmpty(input.FunctionName))
                        {
                            Directory.CreateDirectory(Path.Combine(this.WorkingDirectory, input.FunctionName));
                            File.WriteAllText(Path.Combine(this.WorkingDirectory, input.FunctionName, input.Filename), input.FlowDefinition);
                        }
                    }
                }

                if (artifactsDirectory != null)
                {
                    if (!artifactsDirectory.Exists)
                    {
                        throw new DirectoryNotFoundException(artifactsDirectory.FullName);
                    }

                    var artifactsWorkingDirectory = Path.Combine(this.WorkingDirectory, "Artifacts");
                    Directory.CreateDirectory(artifactsWorkingDirectory);
                    WorkflowTestHost.CopyDirectory(source: artifactsDirectory, destination: new DirectoryInfo(artifactsWorkingDirectory));
                }

                if (!string.IsNullOrEmpty(parameters))
                {
                    File.WriteAllText(Path.Combine(this.WorkingDirectory, "parameters.json"), parameters);
                }

                if (!string.IsNullOrEmpty(connectionDetails))
                {
                    File.WriteAllText(Path.Combine(this.WorkingDirectory, "connections.json"), connectionDetails);
                }

                if (!string.IsNullOrEmpty(localSettings))
                {
                    File.WriteAllText(Path.Combine(this.WorkingDirectory, "local.settings.json"), localSettings);
                }
                else
                {
                    File.Copy(Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\TestFiles\\local.settings.json"), Path.Combine(this.WorkingDirectory, "local.settings.json"));
                }

                if (!string.IsNullOrEmpty(host))
                {
                    File.WriteAllText(Path.Combine(this.WorkingDirectory, "host.json"), host);
                }
                else
                {
                    File.Copy(Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\TestFiles\\host.json"), Path.Combine(this.WorkingDirectory, "host.json"));
                }

                ListDirectoryContents(this.WorkingDirectory);
                var funcExePath = GetFuncPath();
                this.Process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        WorkingDirectory = this.WorkingDirectory,
                        FileName = funcExePath,
                        Arguments = "start --verbose",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true,
                    }
                };

                var processStarted = new TaskCompletionSource<bool>();

                this.Process.OutputDataReceived += (sender, args) =>
                {
                    var outputData = args.Data;
                    Console.WriteLine(outputData);
                    if (outputData != null && outputData.Contains("Host started") && !processStarted.Task.IsCompleted)
                    {
                        processStarted.SetResult(true);
                    }

                    lock (this)
                    {
                        this.OutputData.Add(args.Data);
                    }
                };

                var errorData = string.Empty;
                this.Process.ErrorDataReceived += (sender, args) =>
                {
                    errorData = args.Data;
                    Console.Write(errorData);

                    lock (this)
                    {
                        this.ErrorData.Add(args.Data);
                    }
                };

                this.Process.Start();

                this.Process.BeginOutputReadLine();
                this.Process.BeginErrorReadLine();

                var result = Task.WhenAny(processStarted.Task, Task.Delay(TimeSpan.FromMinutes(2))).Result;

                if (result != processStarted.Task)
                {
                    throw new InvalidOperationException("Runtime did not start properly. Please make sure you have the latest Azure Functions Core Tools installed and available on your PATH environment variable, and that Azurite is up and running.");
                }

                if (this.Process.HasExited)
                {
                    throw new InvalidOperationException($"Runtime did not start properly. The error is '{errorData}'. Please make sure you have the latest Azure Functions Core Tools installed and available on your PATH environment variable, and that Azurite is up and running.");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Directory.Delete(this.WorkingDirectory, recursive: true);

                throw;
            }
        }

        /// <summary>
        /// Copies the directory.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        protected static void CopyDirectory(DirectoryInfo source, DirectoryInfo destination)
        {
            if (!destination.Exists)
            {
                destination.Create();
            }

            // Copy all files.
            var files = source.GetFiles();
            foreach (var file in files)
            {
                file.CopyTo(Path.Combine(destination.FullName, file.Name));
            }

            // Process subdirectories.
            var dirs = source.GetDirectories();
            foreach (var dir in dirs)
            {
                // Get destination directory.
                var destinationDir = Path.Combine(destination.FullName, dir.Name);

                // Call CopyDirectory() recursively.
                CopyDirectory(dir, new DirectoryInfo(destinationDir));
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            try
            {
                this.Process?.Close();
            }
            finally
            {
                var i = 0;
                while (i < 5)
                {
                    try
                    {
                        Directory.Delete(this.WorkingDirectory, recursive: true);
                        break;
                    }
                    catch
                    {
                        i++;
                        Task.Delay(TimeSpan.FromSeconds(5)).Wait();
                    }
                }
            }
        }


        public static void ListDirectoryContents(string path)
        {       
            Console.WriteLine("Listing files in directory");        
            Console.WriteLine($"Directory: {path}");        
            var filePaths = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
            foreach (var filePath in filePaths)
            {
                Console.WriteLine(filePath);
            }
        }
    }
}