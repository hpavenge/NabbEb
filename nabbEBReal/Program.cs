using System;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Rendering;
using SharpDX;

namespace nabbEBReal
{
    public static class Program
    {

        public static void Main(string[] args)
        {
            // Wait till the loading screen has passed
            Loading.OnLoadingComplete += OnLoadingComplete;
        }

        private static void OnLoadingComplete(EventArgs args)
        {
            // Verify the champion
            if (Player.Instance.Hero != Champion.Ezreal)
            {
                return;
            }

            // Initialize the classes
            Config.Initialize();
            SpellManager.Initialize();
            ModeManager.Initialize();
            // Initialize damage indicator
            DamageIndicator.Initialize(Damages.GetTotalDamage);
            DamageIndicator.DrawingColor = System.Drawing.Color.Goldenrod;

            // Listen to events we need
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
            // TODO improve Gapcloseru 
            if(sender.IsEnemy && sender.GetAutoAttackRange() >= ObjectManager.Player.Distance(args.End))
            {
                var diffGapCloser = args.End - args.Start;
                SpellManager.E.Cast(ObjectManager.Player.ServerPosition + diffGapCloser);
            }
        }
    }
}
