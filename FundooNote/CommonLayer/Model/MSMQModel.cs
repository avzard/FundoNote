using Experimental.System.Messaging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace CommonLayer.Model
{
    public class MSMQModel
    {
        MessageQueue messageQ = new MessageQueue();
        public void sendData2Queue(string token)
        {

            messageQ.Path = @".\private$\Tokens";
            if (!MessageQueue.Exists(messageQ.Path))
            {
                MessageQueue.Create(messageQ.Path);
                //Exists
            }
            messageQ.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });
            messageQ.ReceiveCompleted += MessageQ_ReceiveCompleted1;
            messageQ.Send(token);
            messageQ.BeginReceive();
            messageQ.Close();
        }

        private void MessageQ_ReceiveCompleted1(object sender, ReceiveCompletedEventArgs e)
        {
            var msg = messageQ.EndReceive(e.AsyncResult);
            string token = msg.Body.ToString();
            string Subject = "FundoNotes Token Link";
            string Body = token;
            var SMTP = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("prodbygangadhar501@gmail.com", "qevkhnidhahpddzt"),
                EnableSsl = true,

            };
            SMTP.Send("prodbygangadhar501@gmail.com", "prodbygangadhar501@gmail.com", Subject, Body);
            messageQ.BeginReceive();

        }

    }
}