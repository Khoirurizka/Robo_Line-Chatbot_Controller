using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure;
using Azure.AI.OpenAI;
namespace Robo_Line_Chatbot_Controller.Azure_OpenAI
{
    internal static class Azure_OpenAI_Commander
    {
        //Azure Open AI identity 
        public static string deployment_name = "Test_BPrj_Chat";
        public static string azure_OpenAI_endpoint = "https://testbproject.openai.azure.com/";
        public static string azure_OpenAI_key = "b6acf6c75b8c4da9862ccc9a8b9ef2ff";
        public static string chat_request_system_message = "";

        private static string response_msg = " ";
        private static List<string> response_msgs = new List<string> { };

        private static OpenAIClient client = new OpenAIClient(new Uri(azure_OpenAI_endpoint), new AzureKeyCredential(azure_OpenAI_key));

        public static async Task<List<string>> SendMessageToAI(string chat_request_system_message, int max_token=100)
        {
            response_msgs.Clear();

            try
            {
                ChatCompletionsOptions chatcompletionsoptions = new ChatCompletionsOptions()
                {
                    DeploymentName = deployment_name, //This must match the custom deployment name you chose for your model
                    Messages = {new ChatRequestSystemMessage(chat_request_system_message),
                },
                    MaxTokens = max_token
                };
                await foreach (StreamingChatCompletionsUpdate chatUpdate in client.GetChatCompletionsStreaming(chatcompletionsoptions))
                {
                    if (chatUpdate.Role.HasValue)
                    {
                        response_msgs.Add($"{chatUpdate.Role.Value.ToString().ToUpperInvariant()}: ");
                    }
                    if (!string.IsNullOrEmpty(chatUpdate.ContentUpdate))
                    {
                        response_msgs.Add(chatUpdate.ContentUpdate);
                    }
                }
            }
            catch (Exception e)
            {
                response_msgs.Add($"\nException Caught!\n Message :{e.Message}");
                Console.WriteLine($"\nException Caught!\n Message :{e.Message}");
            }
            return response_msgs;
        }
        public static string ShowResponseMsg()
        {
            response_msg = "";
            foreach (string s in response_msgs)
            {
                response_msg += s;
            }
            return response_msg;
        }
    }
}
