using Server.Application.Character;
using Server.Model;
using Shared.Descriptors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Application.Combat
{
    public class CombatPlayer : CombatEntity
    {
        private readonly Mediator mediator;

        public int playerId;

        public bool hasAction;
        public bool hasBonusAction;

        public CombatPlayer(Mediator mediator, Player player)
        {
            this.mediator = mediator;

            playerId = player.Id;
            name = player.Name;
            maxHealth = 100;
            health = player.Health;
            skills = mediator.GetHandler<GetSkills>().GetLoadedFromPlayer(player.Id);

            hasAction = true;
            hasBonusAction = true;

            alive = true;
        }

        public override void TakeDamage(int damage)
        {
            base.TakeDamage(damage);

            mediator.GetHandler<PlayerStats>().DecreaseHealth(playerId, damage);
        }
    }
}
