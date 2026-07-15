using Microsoft.AspNetCore.SignalR;

namespace EmployeeAPI.Hubs;

public class LiveStreamHub : Hub
{
    private static readonly Dictionary<string, string> RoomMembers = new();
    private static readonly Dictionary<string, string> Broadcasters = new();

    public async Task JoinRoom(string roomName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
        RoomMembers[Context.ConnectionId] = roomName;
    }

    public async Task StartBroadcast(string roomName)
    {
        Broadcasters[roomName] = Context.ConnectionId;
        
        await Clients.OthersInGroup(roomName).SendAsync("broadcastStarted", roomName);

        Console.WriteLine($"Broadcaster registered: {Context.ConnectionId}");
    }

    // public async Task JoinAsViewer(string roomName)
    // {
    //     if (Broadcasters.TryGetValue(roomName, out var broadcasterId))
    //     {
    //         await Clients.Caller.SendAsync("broadcastStarted", roomName);
    //         await Clients.Caller.SendAsync("viewerJoined", broadcasterId);
    //     }
    // }
    public async Task JoinAsViewer(string roomName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
        Console.WriteLine($"Viewer joined: {Context.ConnectionId}");
        
        if (Broadcasters.TryGetValue(roomName, out var broadcasterId))
        {
            // Notify the viewer that a broadcast exists
            await Clients.Caller.SendAsync("broadcastStarted", roomName);

            // Notify the broadcaster that a viewer joined
            await Clients.Client(broadcasterId)
                .SendAsync("viewerJoined", Context.ConnectionId);
        }
        else
        {
            await Clients.Caller.SendAsync("error", "No active broadcaster found.");
        }
    }

    // public async Task SendOffer(string roomName, string offer)
    // {
    //     if (Broadcasters.TryGetValue(roomName, out var broadcasterId))
    //     {
    //         await Clients.OthersInGroup(roomName).SendAsync("receiveOffer", offer, Context.ConnectionId);
    //     }
    //     else
    //     {
    //         await Clients.Caller.SendAsync("error", "No active broadcaster found.");
    //     }
    // }

    public async Task SendOffer(
    string roomName,
    string offer,
    string viewerConnectionId)
    {
        await Clients.Client(viewerConnectionId)
            .SendAsync("receiveOffer", offer, Context.ConnectionId);
    }

    public async Task SendAnswer(string roomName, string answer, string senderConnectionId)
    {
        await Clients.Client(senderConnectionId).SendAsync("receiveAnswer", answer, Context.ConnectionId);
    }

    public async Task SendIceCandidate(string roomName, string candidate, string targetConnectionId)
    {
        await Clients.Client(targetConnectionId).SendAsync("receiveIceCandidate", candidate, Context.ConnectionId);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        RoomMembers.Remove(Context.ConnectionId);
        foreach (var broadcaster in Broadcasters.Where(x => x.Value == Context.ConnectionId).ToList())
        {
            Broadcasters.Remove(broadcaster.Key);
        }

        await base.OnDisconnectedAsync(exception);
    }
}
