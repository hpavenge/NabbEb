using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace nabbEBCait
{
    public static class SpellManager
    {
        public static Spell.Skillshot Q { get; private set; }
        public static Spell.Skillshot W { get; private set; }
        public static Spell.Skillshot E { get; private set; }
        public static Spell.Targeted R { get; private set; }

        public static bool IsCastingUlt
        {
            get { return Player.Instance.Buffs.Any(b => b.Caster.IsMe && b.IsValid() && b.DisplayName == "CaitlynR"); }
        }

        public static void Initialize()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 1250, SkillShotType.Linear, 625, 2200, 90);
            W = new Spell.Skillshot(SpellSlot.W, 800, SkillShotType.Circular, 500, int.MaxValue, 20);
            E = new Spell.Skillshot(SpellSlot.E, 800, SkillShotType.Linear, 150, 1600, 80);
            // add 500 range for each leveled R
            R = new Spell.Targeted(SpellSlot.R, (uint) (1500 + Player.Instance.Level * 500));

            //Finetune spells
            Q.AllowedCollisionCount = int.MaxValue;
            W.AllowedCollisionCount = int.MaxValue;
            E.AllowedCollisionCount = 0;
        }
    }
}
