using Robo_Line_Chatbot_Controller.Line_Bot_Messager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robo_Line_Chatbot_Controller.Azure_OpenAI
{
    internal class OutputJson
    {

        public string id { get; set; } = "";
        public string type { get; set; } = "";
        public List<string> command_output { get; set; }=new List<string>();    
        public string message_to_user { get; set; } = "";

    }
}
