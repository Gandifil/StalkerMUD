using Microsoft.AspNetCore.SignalR;
using StalkerMUD.Common.Models;

namespace StalkerMUD.Server.Hubs
{
    public static class FightClientProxyExtensions
    {
        public static Task AddActorAsync(this IClientProxy proxy, ActorResponse actor)
        {
            return proxy.SendAsync("addActor", actor);
        }

        public static Task ChangeActorAsync(this IClientProxy proxy, ActorChangeResponse actorChange)
        {
            return proxy.SendAsync("changeActor", actorChange);
        }

        public static Task RemoveActorAsync(this IClientProxy proxy, string actorId)
        {
            return proxy.SendAsync("removeActor", actorId);
        }

        public static Task SelectActionAsync(this IClientProxy proxy)
        {
            return proxy.SendAsync("selectAction");
        }

        public static Task SendMessageAsync(this IClientProxy proxy, string message)
        {
            return proxy.SendAsync("message", message);
        }
    }
}
