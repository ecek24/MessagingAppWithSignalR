using Azure.Messaging;
using MessagingApp.Core.Entities;
using MessagingApp.DAL.DbContext;
using MessagingApp.Services.DTOs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingApp.Services.Hubs
{
    public class ChatHub :Hub
    {
        private readonly MessagingAppDbContext _context;

        public ChatHub(MessagingAppDbContext context)
        {
            _context = context;
        }

        public override async Task OnConnectedAsync()
        {
            string userName = Context.User.Identity.Name;
            var user = _context.Users.FirstOrDefault(u => u.UserName == userName);

            if (user != null)
            {
                UserConnectionManager.AddConnection(user.Id, Context.ConnectionId);
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            UserConnectionManager.RemoveConnection(Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessageAsync(string receiverEmail, string message)
        {
            var sender= _context.Users.FirstOrDefault(s => s.UserName== Context.User.Identity.Name);
            var receiver=_context.Users.FirstOrDefault(r => r.Email== receiverEmail);

            if (receiver == null)
            {
                throw new ArgumentException("Receiver not found.");
            }

            var newMessage = new Message
            {
                Content = message,
                SentAt = DateTime.UtcNow,
                SenderId = sender.Id,
                ReceiverId = receiver.Id
            };

            await _context.Messages.AddAsync(newMessage);
            await _context.SaveChangesAsync();

            string receiverConnectionId = UserConnectionManager.GetConnectionId(receiver.Id);
            
            if (!string.IsNullOrEmpty(receiverConnectionId))
                {
                    await Clients.Client(receiverConnectionId).SendAsync("receiveMessage", message, sender.UserName);
                }
           
        }
    }
}
