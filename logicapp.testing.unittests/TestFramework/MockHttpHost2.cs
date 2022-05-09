
namespace TestFramework
{
    using System;
    using WireMock.Logging;
    using WireMock.Server;
    using WireMock.Settings;

    /// <summary>
    /// The mock HTTP host.
    /// </summary>
    public class MockHttpHost2 : IDisposable
    {
        public WireMockServer Server { get; set; }
        
        public MockHttpHost2()
        {            
            var settings = new WireMockServerSettings();
            settings.Port = 7075;
            settings.MaxRequestLogCount = 100;
            settings.Logger = new WireMockConsoleLogger();
            Server = WireMockServer.Start(settings);
        }

        public MockHttpHost2(int port)
        {
            var settings = new WireMockServerSettings();
            settings.Port = port;
            settings.MaxRequestLogCount = 100;
            settings.Logger = new WireMockConsoleLogger();
            Server = WireMockServer.Start(settings);
        }

        /// <summary>
        /// Disposes the resources.
        /// </summary>
        public void Dispose()
        {
            Server.Stop();
        }

    }
}