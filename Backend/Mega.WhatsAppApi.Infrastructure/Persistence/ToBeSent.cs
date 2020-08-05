using System;

namespace Mega.SmsAutomator.Objects
{
    public class ToBeSent
    {
        public string Id { get; set; }
        
        public Message Message { get; set; }
        
        public DateTime EntryTime { get; set; }
    }
}