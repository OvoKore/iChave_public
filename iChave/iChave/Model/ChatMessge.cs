using System;

namespace iChave.Model
{
    public class ChatMessge
    {
        public string Email { get; set; }
        public string Message { get; set; }
        public string Time { get; set; }
        public ChatMessge() { }
        public ChatMessge(string Email, string Message)
        {
            this.Email = Email;
            this.Message = Message;
            Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
        public ChatMessge(string Email, string Message, string Time)
        {
            this.Email = Email;
            this.Message = Message;
            this.Time = Time;
        }
    }
}
