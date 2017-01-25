using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Rendering;
using SharpDX;

namespace nabbEBRyanChoi
{
    // HELLSING SETUP
    //TODO Check autoattack cancels etc
    public static class Program
    {
        public static bool HasSmite { get; private set; }

        public static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoadingComplete;
        }

        private static void OnLoadingComplete(EventArgs args)
        {
            // Validate champ
            if (Player.Instance.Hero != Champion.Rengar)
            {
                return;
            }
            // Initialize classes
            SpellManager.Initialize();
            Config.Initialize();
            ModeManager.Initialize();
            // Check if the player has smite
            /*
             *             Spell.Targeted blueSmite = new Spell.Targeted(Rengar.GetSpellSlotFromName("S5_SummonerSmitePlayerGanker"), 500, DamageType.True);
                        Spell.Targeted redSmite = new Spell.Targeted(Rengar.GetSpellSlotFromName("S5_SummonerSmiteDuel"), 500, DamageType.True);
                        Spell.Targeted Smite = new Spell.Targeted(Rengar.GetSpellSlotFromName("SummonerSmite"), 500, DamageType.True);
             * */
            // Initialize damage indicator
            DamageIndicator.Initialize(Damages.GetTotalDamage);
            DamageIndicator.DrawingColor = System.Drawing.Color.Goldenrod;

            // Listend to required events
            Drawing.OnDraw += OnDraw;
            Gapcloser.OnGapcloser += OnGapcloser;
        }
        private static void OnDraw(EventArgs args)
        {
            // All circles
            foreach (var spell in SpellManager.Spells)
            {
                switch (spell.Slot)
                {
                    case SpellSlot.Q:
                        if (!Config.Drawing.DrawQ)
                        {
                            continue;
                        }
                        break;
                    case SpellSlot.W:
                        if (!Config.Drawing.DrawW)
                        {
                            continue;
                        }
                        break;
                    case SpellSlot.E:
                        if (!Config.Drawing.DrawE)
                        {
                            continue;
                        }
                        break;
                }
                Circle.Draw(spell.GetColor(), spell.Range, Player.Instance);
            }

            // Damage on healthbar
            DamageIndicator.HealthbarEnabled = Config.Drawing.IndicatorHealthbar;
            DamageIndicator.PercentEnabled = Config.Drawing.IndicatorPercent;
        }
        private static void OnGapcloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs args)
        {
            if (sender.IsEnemy && Config.Misc.GapcloserE && SpellManager.E.IsReady() && SpellManager.E.IsInRange(args.End))
            {
                // Cast E on the gapcloser caster
                SpellManager.E.Cast(sender);
            }
        }
    }
}
