﻿using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Rendering;
using nabbEBSyndra.Modes;
using SharpDX;

namespace nabbEBSyndra
{
    // HELLSING SETUP
    public static class Program
    {
        public static bool HasIgnite { get; private set; }

        public static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoadingComplete;
        }

        private static void OnLoadingComplete(EventArgs args)
        {
            // Validate champ
            if (Player.Instance.Hero != Champion.Syndra)
            {
                return;
            }
            // Initialize classes
            SpellManager.Initialize();
            Config.Initialize();
            ModeManager.Initialize();
            // Check if the player has ignite
            HasIgnite = Player.Instance.GetSpellSlotFromName("SummonerDot") != SpellSlot.Unknown;
            // Initialize damage indicator
            DamageIndicator.Initialize(Damages.GetTotalDamage);
            DamageIndicator.DrawingColor = System.Drawing.Color.Goldenrod;

            // Listend to required events
            Drawing.OnDraw += OnDraw;
            Gapcloser.OnGapcloser += OnGapcloser;
            Interrupter.OnInterruptableSpell += OnInterruptableSpell;
        }
        private static void OnDraw(EventArgs args)
        {
            // test balls are called Seeds
            foreach (var ball in ObjectManager.Get<Obj_AI_Base>().Where(a => a.Name == "Seed" && a.IsValid))
            {
                Circle.Draw(Color.Purple, 60, 6f, ball.Position);
            }
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
                    case SpellSlot.R:
                        if (!Config.Drawing.DrawR)
                        {
                            continue;
                        }
                        break;
                }
                Circle.Draw(spell.GetColor(), spell.Range, Player.Instance);
            }

            // E damage on healthbar
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
        private static void OnInterruptableSpell(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs args)
        {
            if (sender.IsEnemy && args.DangerLevel == DangerLevel.High && Config.Misc.InterruptE && SpellManager.E.IsReady() && SpellManager.E.IsInRange(sender))
            {
                // Cast E on the unit casting the interruptable spell
                SpellManager.E.Cast(sender);
            }
        }
    }
}
