using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using SharpDX;

namespace nabbEBSyndra
{
    public static class SpellManager
    {
        #region Fields
        public static Spell.Skillshot Q { get; private set; }
        public static Spell.Skillshot W { get; private set; }
        public static Spell.Skillshot E { get; private set; }
        public static Spell.Targeted R { get; private set; }
        public static Spell.Skillshot QE { get; private set; }

        public static Spell.SpellBase[] Spells { get; private set; }

        public static Dictionary<SpellSlot, Color> ColorTranslation { get; private set; }
        #endregion

        public static void Initialize()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 800, SkillShotType.Circular, 625, Int32.MaxValue, 90);
            W = new Spell.Skillshot(SpellSlot.W, 925, SkillShotType.Circular, 500, 2500, 20);
            E = new Spell.Skillshot(SpellSlot.E, 700, SkillShotType.Cone, 150, Int32.MaxValue, 80);
            R = new Spell.Targeted(SpellSlot.R, 700);
            QE = new Spell.Skillshot(SpellSlot.W, 980, SkillShotType.Linear, 600, 2400, 18);
            //Finetune spells
            Q.AllowedCollisionCount = Int32.MaxValue;
            W.AllowedCollisionCount = Int32.MaxValue;
            W.MinimumHitChance = HitChance.High;
            E.AllowedCollisionCount = 0;
            QE.AllowedCollisionCount = Int32.MaxValue;
            // Colors
            Spells = (new Spell.SpellBase[] { Q, W, E, R }).OrderByDescending(o => o.Range).ToArray();
            ColorTranslation = new Dictionary<SpellSlot, Color>
            {
                { SpellSlot.Q, Color.IndianRed.ToArgb(150) },
                { SpellSlot.W, Color.PaleVioletRed.ToArgb(150) },
                { SpellSlot.E, Color.IndianRed.ToArgb(150) },
                { SpellSlot.R, Color.DarkRed.ToArgb(150) }
            };
        }

        // HELLSINGERU
        public static AIHeroClient GetTarget(this Spell.SpellBase spell, params AIHeroClient[] excludeTargets)
        {
            var targets = EntityManager.Heroes.Enemies.Where(o => o.IsValidTarget() && !excludeTargets.Contains(o) && spell.IsInRange(o)).ToArray();
            return TargetSelector.GetTarget(targets, DamageType.Magical);
        }

        public static bool CastOnBestTarget(this Spell.SpellBase spell)
        {
            var target = spell.GetTarget();
            return target != null && spell.Cast(target);
        }

        private static Color ToArgb(this Color color, byte a)
        {
            return new ColorBGRA(color.R, color.G, color.B, a);
        }

        public static Color GetColor(this Spell.SpellBase spell)
        {
            return ColorTranslation.ContainsKey(spell.Slot) ? ColorTranslation[spell.Slot] : Color.Wheat;
        }

        // KA-Syndra leggo
        public static int SpheresCount()
        {
            return ObjectManager.Get<Obj_AI_Base>().Count(x => x.Name == "Seed" && x.IsValid);
        }

        //TODO test and debug
        public static void QECast(Vector3 position)
        {
            if (Q.IsReady() && E.IsReady())
            {
                Q.Cast(Player.Instance.Position.Extend(position, E.Range - 10).To3D());
                E.Cast(Player.Instance.Position.Extend(position, E.Range - 10).To3D());
            }
        }

        //TODO test
        public static Vector3 GrabWPost(bool onlyQ)
        {
            var sphere =
                ObjectManager.Get<Obj_AI_Base>().FirstOrDefault(a => a.Name == "Seed" && a.IsValid);
            if (sphere != null)
                return sphere.Position;
            if (!onlyQ)
            {
                var minion = EntityManager.MinionsAndMonsters.GetLaneMinions()
                    .OrderByDescending(m => m.Health)
                    .FirstOrDefault(m => m.IsValidTarget(W.Range) && m.IsEnemy);
                if (minion != null)
                    return minion.Position;
            }
            return new Vector3();
        }
    }
}
