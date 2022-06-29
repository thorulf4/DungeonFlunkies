using Microsoft.EntityFrameworkCore;
using Server.Model.Items;
using Server.Model.Skills;
using Shared.Descriptors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Application.Combat
{
    public class GetSkills : IApplicationLogic
    {
        private readonly GameDb context;

        public GetSkills(GameDb context, Mediator mediator)
        {
            this.context = context;
        }

        public List<Skill> GetFromPlayer(int playerId)
        {
            return context.Equipped.Where(e => e.PlayerId == playerId).Include(e => e.Item).ThenInclude(i => ((Equipment)i).Skills).SelectMany(o => ((Equipment)o.Item).Skills).ToList();
        }

        public List<LoadedSkill> GetLoadedFromPlayer(int playerId)
        {
            return context.Equipped.Where(e => e.PlayerId == playerId).Include(e => e.Item).ThenInclude(i => ((Equipment)i).Skills).SelectMany(o => ((Equipment)o.Item).Skills).Select(s => new LoadedSkill(s)).ToList();
        }

        public Skill Get(SkillDescriptor skill)
        {
            return context.Skills.Find(skill.Id);
        }

        public Skill Get(int skillId)
        {
            return context.Skills.Find(skillId);
        }
    }
}
