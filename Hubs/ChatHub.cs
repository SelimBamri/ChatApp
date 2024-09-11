using ChatApp.Models;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IDictionary<string, UserConnection> _connection;

        public ChatHub(IDictionary<string, UserConnection> connection)
        {
            this._connection = connection;
        }
        public async Task JoinRoom(UserConnection userConnection)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userConnection.Room!);
            _connection[Context.ConnectionId] = userConnection;
            await Clients.Group(userConnection.Room!)
                .SendAsync("ReceiveMessage", "Let's program Bot", $"{userConnection.User} has joined the Group.");
            await SendConnectedUsers(userConnection.Room!);

        }
        public async Task SendMessage(string message)
        {
            if(_connection.TryGetValue(Context.ConnectionId, out UserConnection userConnection))
            {
                await Clients.Group(userConnection.Room!)
                    .SendAsync("ReceiveMessage", userConnection.User, message, DateTime.Now); 
            }
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            if(!_connection.TryGetValue(Context.ConnectionId, out UserConnection userConnection))
            {
                return base.OnDisconnectedAsync(exception);
            }
            Clients.Group(userConnection.Room!)
                .SendAsync("ReceiveMessage", "Let's program Bot", $"{userConnection.User} has left the Group.");
            SendConnectedUsers(userConnection.Room!);
            return base.OnDisconnectedAsync(exception);
        }   

        public Task SendConnectedUsers(string room)
        {
            var users = _connection.Values
                .Where(x => x.Room == room)
                .Select(x => x.User);
            return Clients.Group(room).SendAsync("ConnectedUsers", users);
        }

    }
}
