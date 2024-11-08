using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingApp.Core.Entities
{
    public class Message
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public DateTime SentAt { get; set; } = DateTime.UtcNow;

        //Nav props

        [ForeignKey("SenderId")]
        public User Sender { get; set; }

        public string SenderId { get; set; }


        [ForeignKey("ReceiverId")]
        public User Receiver { get; set; }

        public string ReceiverId { get; set; }
    }
}

