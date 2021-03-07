using Server.Application;
using Server.Application.Combat;
using Shared;
using Shared.Requests.Combat;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.RequestHandlers.Combat
{
    public class UseSkillHandler : RequestHandler<UseSkillRequest>
    {
        private readonly Mediator mediator;
        private readonly Authenticator authenticator;

        public UseSkillHandler(Mediator mediator, Authenticator authenticator)
        {
            this.mediator = mediator;
            this.authenticator = authenticator;
        }

        public override Response Handle(UseSkillRequest request)
        {
            var playerId = authenticator.VerifySession(request.Name, request.SessionId);

            mediator.GetHandler<CombatManager>().PlayerUseSkill(playerId, request.Skill, request.TargetId).Save();

            return Response.Ok();
        }
    }
}
