using Server.Application.Alerts;
using Server.Model;
using Shared.Alerts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Application.Character
{
    public class KillCharacter : IApplicationLogic
    {
        private readonly GameDb context;
        private readonly Mediator mediator;
        private readonly Authenticator authenticator;

        public KillCharacter(GameDb context, Mediator mediator, Authenticator authenticator)
        {
            this.context = context;
            this.mediator = mediator;
            this.authenticator = authenticator;
        }

        public void Kill(int playerId)
        {
            Player player = mediator.GetHandler<GetPlayer>().Get(playerId);

            context.RemoveRange(context.Equipped.Where(e => e.PlayerId == playerId));
            context.RemoveRange(context.OwnedBys.Where(e => e.OwnerId == playerId));
            context.Remove(player);
            context.SaveChanges();

            authenticator.CloseSession(player.Name);
            mediator.GetHandler<PlayerDiedAlerter>().Alert(player.Name);
        }
    }
}
