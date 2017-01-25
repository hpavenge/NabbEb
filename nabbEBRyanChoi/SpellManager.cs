using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using SharpDX;

namespace nabbEBRyanChoi
{
    public static class SpellManager
    {
        #region Fields
        public static Spell.Skillshot Q { get; private set; }

        public static Spell.Skillshot Q2 { get; private set; }
        public static Spell.Active W { get; private set; }
        public static Spell.Skillshot E { get; private set; }

        public static Spell.SpellBase[] Spells { get; private set; }

        public static Dictionary<SpellSlot, Color> ColorTranslation { get; private set; }
        #endregion

        public static void Initialize()
        {
            //TODO actually use Q2 in combo
            Q = new Spell.Skillshot(SpellSlot.Q, 326, SkillShotType.Cone, 250, 3000, 150);
            Q2 = new Spell.Skillshot(SpellSlot.Q,450,SkillShotType.Linear,500,3000,150);
            W = new Spell.Active(SpellSlot.W,450);
            E = new Spell.Skillshot(SpellSlot.E, 1000, SkillShotType.Linear, 250, 1500, 70);
            //Finetune spells
            Q.ConeAngleDegrees = 180;
            Q.AllowedCollisionCount = int.MaxValue;
            Q.MinimumHitChance = HitChance.High; ;
            Q2.AllowedCollisionCount = int.MaxValue;
            E.AllowedCollisionCount = 0;
            E.MinimumHitChance = HitChance.High;
            // Colors
            Spells = (new Spell.SpellBase[] { Q, W, E}).OrderByDescending(o => o.Range).ToArray();
            ColorTranslation = new Dictionary<SpellSlot, Color>
            {
                { SpellSlot.Q, Color.IndianRed.ToArgb(150) },
                { SpellSlot.W, Color.PaleVioletRed.ToArgb(150) },
                { SpellSlot.E, Color.IndianRed.ToArgb(150) }
            };
        }

        // HELLSINGERU
        public static AIHeroClient GetTarget(this Spell.SpellBase spell)
        {
            return TargetSelector.GetTarget(spell.Range, DamageType.Physical);
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
