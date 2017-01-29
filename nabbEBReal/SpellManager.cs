using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using SharpDX;

namespace nabbEBReal
{
    public static class SpellManager
    {
        public static Spell.Skillshot Q { get; private set; }
        public static Spell.Skillshot W { get; private set; }
        public static Spell.Skillshot E { get; private set; }
        public static Spell.Skillshot R { get; private set; }

        public static Spell.SpellBase[] Spells { get; private set; }

        public static Dictionary<SpellSlot, Color> ColorTranslation { get; private set; }

        public static bool IsCastingUlt
        {
            get { return Player.Instance.Buffs.Any(b => b.Caster.IsMe && b.IsValid() && b.DisplayName == "EzrealR"); }
        }

        public static void Initialize()
        {
            //RANGE: 1150 SPEED: 2000 COST: 28 / 31 / 34 / 37 / 40 MANA COOLDOWN: 6.5 / 6 / 5.5 / 5 / 4.5
            Q = new Spell.Skillshot(SpellSlot.Q, 1150, SkillShotType.Linear, 250, 2000, 60);
            //RANGE: 1000 SPEED: 1550 COST: 50 / 60 / 70 / 80 / 90 MANA COOLDOWN: 9
            W = new Spell.Skillshot(SpellSlot.W, 1000, SkillShotType.Linear, 250, 1600, 80);
            //RANGE: 475 / 750 COST: 90 MANA COOLDOWN: 19 / 17.5 / 16 / 14.5 / 13
            E = new Spell.Skillshot(SpellSlot.E, 475, SkillShotType.Linear, 250, 2000, 80);
            //RANGE: Global SPEED: 2000 COST: 100 MANA COOLDOWN: 120
            R = new Spell.Skillshot(SpellSlot.R, 3000, SkillShotType.Linear, 1000, 2000, 160);

            //Finetune spells
            Q.AllowedCollisionCount = 0;
            // W & R can go trough units
            W.AllowedCollisionCount = int.MaxValue;
            R.AllowedCollisionCount = int.MaxValue;

            // Colors
            Spells = (new Spell.SpellBase[] { Q, W, E }).OrderByDescending(o => o.Range).ToArray();
            ColorTranslation = new Dictionary<SpellSlot, Color>
            {
                { SpellSlot.Q, Color.Yellow.ToArgb(150) },
                { SpellSlot.W, Color.PaleVioletRed.ToArgb(150) },
                { SpellSlot.E, Color.IndianRed.ToArgb(150) }
            };
        }
        // HELLSINGERU
        public static AIHeroClient GetTarget(this Spell.SpellBase spell)
        {
            return TargetSelector.GetTarget(spell.Range, DamageType.Mixed);
        }

        private static Color ToArgb(this Color color, byte a)
        {
            return new ColorBGRA(color.R, color.G, color.B, a);
        }

        public static Color GetColor(this Spell.SpellBase spell)
        {
            return ColorTranslation.ContainsKey(spell.Slot) ? ColorTranslation[spell.Slot] : Color.Wheat;
        }
    }
}
