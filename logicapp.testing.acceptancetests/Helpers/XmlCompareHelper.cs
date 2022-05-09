using Azure.Identity;
using Azure.Messaging.ServiceBus;
using IPB.LogicApp.Standard.Testing;
using IPB.LogicApp.Standard.Testing.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace logicapp.testing.acceptancetests.Helpers
{
    public class XmlCompareHelper
    {
        public static bool CompareXmlStringsWithXDoc(string actual, string expected)
        {
            var passed = true;

            //Using the XDoc will remove any formatting issues so they have same indenting etc
            var actualDoc = XDocument.Parse(actual, LoadOptions.None);
            var expectedDoc = XDocument.Parse(expected, LoadOptions.None);
            
            actual = actualDoc.ToString();
            expected = expectedDoc.ToString();

            var actualBuilder = new StringBuilder();
            var expectedBuilder = new StringBuilder();

            if (actual.Length != expected.Length)
            {
                Console.WriteLine($"String lengths dont match, actual:{actual.Length}, expected:{expected.Length}");
                passed = false;
            }

            var index = 0;
            foreach (char ac in actual)
            {
                var ec = expected[index];

                actualBuilder.Append(ac);
                expectedBuilder.Append(ec);

                if (ec != ac)
                {
                    passed = false;
                    break;
                }

                index++;
            }

            if (passed)
            {
                Console.WriteLine("Comparison Success");
                Console.WriteLine(actual);
            }
            else
            {
                Console.WriteLine("Actual until Failure");
                Console.WriteLine(actualBuilder.ToString());
                Console.WriteLine("");
                Console.WriteLine("Expected until failure");
                Console.WriteLine(expectedBuilder.ToString());
            }

            return passed;            
        }       
    }
}