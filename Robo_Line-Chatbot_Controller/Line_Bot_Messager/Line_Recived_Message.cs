using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robo_Line_Chatbot_Controller.Line_Bot_Messager
{
    internal class Line_Recived_Message
    {

        public string destination { get; set; }
        public Events events { get; set; } = new Events();
    }

    internal class Events
    {
        public string type { get; set; }
        public Message message { get; set; } = new Message();
        public string prev_webhookEventId { get; set; }//history for update
        public string webhookEventId { get; set; }
        public DeliveryContext deliveryContext { get; set; } = new DeliveryContext();
        public UInt64 timestamp { get; set; }
        public Source source { get; set; } = new Source();
        public string replyToken { get; set; }
        public string mode { get; set; }
    }
    internal class Message
    {
        public string type { get; set; }
        public UInt64 id { get; set; }
        public string quoteToken { get; set; }
        public string text { get; set; }
    }
    internal class DeliveryContext
    {
        public bool isRedelivery { get; set; }
    }
    internal class Source
    {
        public string type { get; set; }
        public string userId { get; set; }
    }

}
