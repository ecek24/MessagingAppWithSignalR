using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingApp.Services.Models
{
    public static class UserConnectionManager
    {
        private static readonly Dictionary<string, string> _connections = new Dictionary<string, string>();

        public static void AddConnection(string userId, string connectionId)
        {
            _connections[userId] = connectionId;
        }

        public static void RemoveConnection(string connectionId)
        {
            var item = _connections.FirstOrDefault(c => c.Value == connectionId);
            if (item.Key != null)
            {
                _connections.Remove(item.Key);
            }
        }

        public static string GetConnectionId(string userId)
        {
            return _connections.TryGetValue(userId, out var connectionId) ? connectionId : null;
        }
    }
}
