using IPB.LogicApp.Standard.Testing;
using IPB.LogicApp.Standard.Testing.Model;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace logicapp.testing.acceptancetests.Helpers
{
    public static class StringExtensions
    {
        public static T GetMessageBodyFromActionJson<T>(this string actionJsonMessage)
        {
            var bodyKey = "body";

            var actionMessageJson = JObject.Parse(actionJsonMessage);
            if(actionMessageJson.ContainsKey(bodyKey))
                return actionMessageJson[bodyKey].Value<T>();              
            else
                throw new Exception("There is no body for this action body property");
        }

        public static T GetMessageContentFromActionJson<T>(this string actionJsonMessage)
        {
            var contentKey = "content";

            var actionMessageJson = JObject.Parse(actionJsonMessage);
            if(actionMessageJson.ContainsKey(contentKey))
                return actionMessageJson[contentKey].Value<T>();
            else
                throw new Exception("There is no content for this action on the content property");
        }

        public static string GetTextFromActionJsonFirstProperty(this string actionJsonMessage)
        {
            var actionMessageJson = JObject.Parse(actionJsonMessage);            
            return actionMessageJson.First.ToString();
        }

        public static string FormatAsJson(this string me)
        {
            var token = JToken.Parse(me)            ;
            return token.ToString(Newtonsoft.Json.Formatting.Indented);
        }

        public static string FormatAsXml(this string me)
        {
            var xdoc = XDocument.Parse(me);
            return xdoc.ToString();
        }

    }
}