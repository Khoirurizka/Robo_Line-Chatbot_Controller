using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Azure.AI.OpenAI;
using Newtonsoft.Json;
using System.Text.Json;
using Robo_Line_Chatbot_Controller.Azure_OpenAI;
using Robo_Line_Chatbot_Controller.Line_Bot_Messager;
using Robo_Line_Chatbot_Controller.JsonOperator;

class main_LCAIC
{
    static async Task Main(string[] args)
    {

        string line_webhook = "https://webhook.site/9580ded1-ca58-426d-ac26-0aa2fde4197f";
        string line_acces_token = "W32MorNaJ6AjsS+cUMc3bWLac95KeU1mXKTqDnNB+tkcTQtUDtTpHAsIJjvXu+EZmfd2XOuzyK4axVCDar5uN5SbRz9oeF0i3qIFkX1QEtEFEWU/YQ5aVhBcl1O7V1srtV2KkbG20+CbZZc7LqxFbQdB04t89/1O/w1cDnyilFU=";
        string line_api_key = "9580ded1-ca58-426d-ac26-0aa2fde4197f";

        //// line bot
        Line_Bot_Messager.Set_Address(line_webhook, line_acces_token, line_api_key);
        while (true)
        {
            // Line Recived Messages
            Line_Recived_Message line_recived_msg = new Line_Recived_Message();
            ////
            //strat taking msg ( Beta Edition)
            string temp_webhookEventId = "";//history for update
            string bot_json_msg = Line_Bot_Messager.Get_Msg().Result;
            line_recived_msg = JsonOperator.Deserialize_Line_Recived_msg(bot_json_msg);
            line_recived_msg.events.prev_webhookEventId = line_recived_msg.events.webhookEventId;
            temp_webhookEventId = line_recived_msg.events.webhookEventId;

            //pooling taking msg
            while (line_recived_msg.events.webhookEventId == line_recived_msg.events.prev_webhookEventId)
            {
                bot_json_msg = Line_Bot_Messager.Get_Msg().Result;
                line_recived_msg = JsonOperator.Deserialize_Line_Recived_msg(bot_json_msg);

                line_recived_msg.events.prev_webhookEventId = temp_webhookEventId;
                temp_webhookEventId = line_recived_msg.events.webhookEventId;
            }

            ////
            // chat gpt
            string chatToLinebot = "We have have robot with json data: \r\n" +
                                   "{Robot[0]:{id: U01,robot_type:uav, var_status: {velocity:50m/s, yaw:70deg},command_list:{take_off(x m), landing(x m), move_forward(x m), turn_right(x m), turn_left(x m),move_back(x m)}}," +
                                   "{Robot[1]:{id: S01,robot_type:rov, var_status: {velocity:50m/s, yaw:70deg},command_list:{sumerge(x m), to_surface(x m), move_forward(x m), turn_right(x m), turn_left(x m),move_back(x m)}}}." +
                                   $"Then, A user has this prompt: \"{line_recived_msg.events.message.text}\".\r\n" +
                                   "please give response in json with format: \r\n" +
                                   "{\"id\": \"id\", \"type\": \"type\",\"command_output\": [{put_the_command_list e.q. \"[0]\":\"take_off\",\"[1]\":\"move_forward\" on command_output}] make sure the [{ on begin and }] on the end, \"message_to_user\": \"put_your_response_here (please use sentences to explain to the user\")}";
            await Azure_OpenAI_Commander.SendMessageToAI(chatToLinebot);
            string chatGpt_recived_msg = Azure_OpenAI_Commander.ShowResponseMsg();
            Console.WriteLine(chatGpt_recived_msg);
            Console.WriteLine(JsonOperator.Deserialize_Azure_OpenAI_Recived_msg(chatGpt_recived_msg).message_to_user);

            // Line Send Messages
            Console.WriteLine(Line_Bot_Messager.Send_Msg(JsonOperator.Deserialize_Azure_OpenAI_Recived_msg(chatGpt_recived_msg).message_to_user, line_recived_msg.events.source.userId).Result);
        }

    }
}

