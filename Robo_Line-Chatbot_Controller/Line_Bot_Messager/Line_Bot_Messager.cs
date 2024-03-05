using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Robo_Line_Chatbot_Controller.Line_Bot_Messager
{
    internal static class Line_Bot_Messager
    {
        //Line identity
        public static string line_webhook = "https://webhook";
        public static string line_access_token = "0000000000000000....0000000000000000000000=";
        public static string line_api_key = "00000000-0000-0000-0000-000000000000";

        private static string line_api_endpoint = "https://api.line.me/v2"; 
        private static string retryKey = ""; // Generate a unique UUID for the retry key

        private static string response_msg = " ";


        public static string Set_Address(string _Line_webhook, string _line_access_token, string _Line_api_key)
        {
            try
            {
                line_webhook = _Line_webhook;
                line_access_token = _line_access_token;
                line_api_key = _Line_api_key;
                response_msg = "Set address successfully";
            }
            catch (Exception e)
            {
                response_msg = $"\nException Caught!\n Message :{e.Message}";
                Console.WriteLine($"\nException Caught!\n Message :{e.Message}");
            }
            return response_msg;
        }
        public static string change_line_api_endpoint(string _line_api_endpoint)
        {
            try
            {
                line_api_endpoint = _line_api_endpoint;
                response_msg = "Set line api endpoint successfully";
            }
            catch (Exception e)
            {
                response_msg = $"\nException Caught!\n Message :{e.Message}";
                Console.WriteLine($"\nException Caught!\n Message :{e.Message}");
            }
            return response_msg;
        }
        public static void Show_Line_Bot_Address()
        {
            Console.WriteLine(line_webhook);
            Console.WriteLine(line_access_token);
            Console.WriteLine(line_api_key);
        }
        public static async Task<String> Get_Msg()
        {
            using (HttpClient httpClient = new HttpClient())
            {           
                // Set the base address if you're going to reuse the HttpClient for requests to the same domain
                httpClient.BaseAddress = new Uri(line_webhook);

                // Add headers to HttpClient instance
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json")); // Set Accept header
                httpClient.DefaultRequestHeaders.Add("api-key", $"{line_api_key}"); // Add your API key header

                try
                {
                    // Make the GET request
                    HttpResponseMessage response = await httpClient.GetAsync($"token/{line_api_key}/request/latest/raw");
                    response.EnsureSuccessStatusCode(); // Throw if not a success code.

                    // Get the response from Line async
                    response_msg = await response.Content.ReadAsStringAsync();
                }
                catch (HttpRequestException e)
                {
                    response_msg = $"\nException Caught!\n Message :{e.Message}";
                    Console.WriteLine($"\nException Caught!\n Message :{e.Message}");
                }
            }
            return response_msg;
        }

        public static async Task<String> Send_Msg(string message= "Hello, LINE!",string user_id= "U00000000000000000000000000000000")
        {
            string endpoint = $"{line_api_endpoint}/bot/message/push";
            //Console.WriteLine(endpoint);
            using (HttpClient httpClient = new HttpClient())
            {
                retryKey = Guid.NewGuid().ToString();

                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {line_access_token}");
                httpClient.DefaultRequestHeaders.Add("X-Line-Retry-Key", retryKey);

                string jsonBody = $"{{\"to\":\"{user_id}\",\"messages\":[{{\"type\":\"text\",\"text\":\"{message}\"}}]}}";

                using (HttpContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json"))
                {
                    // Send the response from Line async
                    HttpResponseMessage response = await httpClient.PostAsync(endpoint, content);

                    if (response.IsSuccessStatusCode)
                    {
                        response_msg= $"Message successfully send to {user_id}!";
                    }
                    else
                    {
                        response_msg=$"Error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}";
                    }
                }
            }
            return response_msg;
        }
    }
}
