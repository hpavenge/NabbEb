using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using Settings = nabbEBCait.Config.Modes.Combo;

namespace nabbEBCait.Modes
{
    public sealed class Combo : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            // Only execute this mode when the orbwalker is on combo mode
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo);
        }

        public override void Execute()
        {
            // use q when target in range and cant be aad
            if (Settings.UseQ && Q.IsReady())
            {
                var target = TargetSelector.GetTarget(Q.Range, DamageType.Physical);
                if (Q.CanCast(target) && !ObjectManager.Player.IsInAutoAttackRange(target))
                {
                    return;
                }
            }
            // use W when target is stunned or rooted
            if (Settings.UseW && W.IsReady())
            {
                var target = W.GetTarget();
                if (target!= null && target.IsStunned)
                {
                    if (W.Cast())
                    {
                        return;
                    }
                }
            }
            // use R if killable
            if (Settings.UseR && R.IsReady())
            {
                var target = R.GetTarget();
                if (target != null && R.GetRealDamage(target) > target.Health)
                {
                    R.Cast(target);
                }
            }
            // use EQ combo if killable thanks mr troll 
            if (E.IsReady() && Q.IsReady())
            {
                var target = TargetSelector.GetTarget(E.Range, DamageType.Physical);
                if (target!= null && (Q.GetRealDamage(target) + E.GetRealDamage(target) > target.Health))
                {
                    var predEq = E.GetPrediction(target);
                    if (predEq.HitChance >= HitChance.Medium)
                    {
                        E.Cast(predEq.CastPosition);
                        Player.IssueOrder(GameObjectOrder.AttackUnit, target);
                    }
                    var predQ = Q.GetPrediction(target);
                    if (predQ.HitChance >= HitChance.High)
                    {
                        Q.Cast(predQ.CastPosition);
                        Player.IssueOrder(GameObjectOrder.AttackUnit, target);
                    }
                }
            }
        }
    }
}
