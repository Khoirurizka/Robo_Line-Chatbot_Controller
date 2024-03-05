using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Robo_Line_Chatbot_Controller.Line_Bot_Messager;
using Robo_Line_Chatbot_Controller.JsonOperator;
using Robo_Line_Chatbot_Controller.Azure_OpenAI;

namespace Robo_Line_Chatbot_Controller.JsonOperator
{
    internal static class JsonOperator
    {
        //Line
        private static Line_Recived_Message line_recived_msg;
        private static OutputJson output_json;
        //Azure Open AI
        private static string Azure_openAI_msg_to_user="";

        public static Line_Recived_Message Deserialize_Line_Recived_msg(string jsonString,bool debug=false)
        {
            line_recived_msg = new Line_Recived_Message();
            dynamic jsonData = JsonConvert.DeserializeObject(jsonString); // Deserialize JSON string to a dynamic object


            if (jsonData != null)
            {

                line_recived_msg.destination = jsonData.destination;

                if (jsonData.events != null)
                {
                    line_recived_msg.events.type = jsonData.events[0].type;
                    string msg = ($"{jsonData.events[0].message}");
                    //message
                    if (jsonData.events[0].message != null)
                    {
                        line_recived_msg.events.message.type = (string)jsonData.events[0].message["type"];
                        line_recived_msg.events.message.id = Convert.ToUInt64(jsonData.events[0].message["id"]);
                        line_recived_msg.events.message.quoteToken = (string)jsonData.events[0].message["quoteToken"];
                        line_recived_msg.events.message.text = (string)jsonData.events[0].message["text"];
                    }
                    //
                    line_recived_msg.events.webhookEventId = jsonData.events[0].webhookEventId;
                    line_recived_msg.events.deliveryContext.isRedelivery = Convert.ToBoolean(jsonData.events[0].deliveryContext["isRedelivery"]);
                    line_recived_msg.events.timestamp = Convert.ToUInt64(jsonData.events[0].timestamp);

                    //source
                    if (jsonData.events[0].source != null)
                    {
                        line_recived_msg.events.source.type = jsonData.events[0].source["type"];
                        line_recived_msg.events.source.userId = jsonData.events[0].source["userId"];
                    }
                    //
                    line_recived_msg.events.replyToken = jsonData.events[0].replyToken;
                    line_recived_msg.events.mode = jsonData.events[0].mode;
                }
                if (debug)
                    Console.WriteLine("JSON Deserialize successfully.");

            }
            else
            {
                Console.WriteLine("Invalid JSON structure.");
            }
            return line_recived_msg;
        }

        public static OutputJson Deserialize_Azure_OpenAI_Recived_msg(string chatFromAI)
        {
            output_json = new OutputJson();
            string jsonString = FilterJsonString.GetJsonStringFromRandomString(chatFromAI);
            dynamic jsonData = JsonConvert.DeserializeObject(jsonString); // Deserialize JSON string to a dynamic object

            output_json.id = jsonData.id;
            output_json.type = jsonData.type;
            if (jsonData.command_output[0] != null)
            {
                for (int i = 0; i < jsonData.command_output[0].Count; i++)
                {
                    output_json.command_output.Add(jsonData.command_output[0][$"[{i}]"]);
                }
            }
            output_json.message_to_user = jsonData.message_to_user;

            return output_json;
        }
    }
}
