using System;
using System.Runtime.InteropServices.WindowsRuntime;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

// Using the config like this makes your life easier, trust me
using Settings = nabbEBSyndra.Config.Modes.Combo;

namespace nabbEBSyndra.Modes
{
    public sealed class Combo : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            // Only execute this mode when the orbwalker is on combo mode
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo);
        }

        private static int lastWCast;

        public override void Execute()
        {
            // TODO: check W and EQ
            // Q logic
            var target = TargetSelector.GetTarget(Q.Range, DamageType.Magical);
            if (target == null || target.IsZombie || target.HasUndyingBuff()) return;
            if (Q.IsReady() && Settings.UseQ)
            {
                var prediction = Q.GetPrediction(target);
                if (prediction.HitChance >= Q.MinimumHitChance)
                {
                    if (Q.Cast(target))
                    {
                        return;
                    }
                }
            }
            // W logic
            target = TargetSelector.GetTarget(W.Range, DamageType.Magical);
            if (target == null || target.IsZombie || target.HasUndyingBuff()) return;
            if (W.IsReady() && Settings.UseW)
            {
                if (Player.Spellbook.GetSpell(SpellSlot.W).ToggleState != 2 &&
                    lastWCast + 700 < Environment.TickCount)
                {
                    W.Cast(SpellManager.GrabWPost(true));
                    lastWCast = Environment.TickCount;
                }
                if (Player.Spellbook.GetSpell(SpellSlot.W).ToggleState >= 1 &&
                    lastWCast + 300 < Environment.TickCount)
                {
                    W.Cast(W.GetPrediction(target).CastPosition);
                }
            }
            // ult logic
            target = TargetSelector.GetTarget(R.Range, DamageType.Magical);
            if (target == null || target.IsZombie || target.HasUndyingBuff()) return;
            if (R.IsReady() && Settings.UseR)
            {
                if (R.Cast(target))
                {
                    return;
                }
            }
            // Q E if possible
            target = TargetSelector.GetTarget(QE.Range, DamageType.Magical);
            if (target == null || target.IsZombie || target.HasUndyingBuff()) return;
            if (Q.IsReady() && E.IsReady() && target.IsValidTarget(QE.Range) && Settings.UseQ && Settings.UseE)
            {
                SpellManager.QECast(QE.GetPrediction(target).CastPosition);
            }

            // own QE incoming 
//            target = TargetSelector.GetTarget(QE.Range, DamageType.Magical);
//            if (target == null || target.IsZombie || target.HasUndyingBuff()) return;
//            if (Q.IsReady() && E.IsReady() && Settings.UseQ && Settings.UseE)
//            {
//                var prediction = QE.GetPrediction(target);
//                if (prediction.HitChance >= HitChance.High)
//                {
//                    if (QE.Cast(target))
//                    {
//                        return;
//                    }
//                }
//            }
        }
    }
}
