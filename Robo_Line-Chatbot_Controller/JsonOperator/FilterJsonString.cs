using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Robo_Line_Chatbot_Controller.JsonOperator
{
    internal static class FilterJsonString
    {
        public static string GetJsonStringFromRandomString(string RandomStringWithJson) {
            // Extract the JSON result
            //filter
            string pattern = "[^a-zA-Z0-9\\{\\}\\(\\)\\,\\:\\[\\]\"'\"_' |\\s{2,}]";
            RandomStringWithJson = Regex.Replace(RandomStringWithJson, pattern, "");
            //cut string 
            int firstCurlyBraceIdx = RandomStringWithJson.IndexOf('{');
            int lastCurlyBraceIdx = RandomStringWithJson.LastIndexOf('}');
            string jsonString = RandomStringWithJson.Substring(firstCurlyBraceIdx, lastCurlyBraceIdx - firstCurlyBraceIdx + 1);
            //jsonString = jsonString.Replace("[", "[{")
            //.Replace("]", "}]");

            return jsonString;
        }
    }
}
