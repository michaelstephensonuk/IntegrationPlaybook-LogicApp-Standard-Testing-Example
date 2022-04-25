using TechTalk.SpecFlow;
using System.Diagnostics;

namespace LogicApp.Testing.UnitTests.Helpers
{
    [Binding]
    public class FunctionHostHelper
    {
        [BeforeScenario(Order = 0)]
        [Scope(Tag = "KillFuncBeforeTest")]
        public void KillFuncBeforeTest()
        {
            //Clean any left over func.exe processes between tests
            var processes = Process.GetProcessesByName("func");
            foreach (var process in processes)
            {
                process.Kill();
            }
        }
    }
    
}

