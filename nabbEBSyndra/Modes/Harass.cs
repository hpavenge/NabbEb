using EloBuddy;
using EloBuddy.SDK;

// Using the config like this makes your life easier, trust me
using Settings = nabbEBSyndra.Config.Modes.Harass;

namespace nabbEBSyndra.Modes
{
    public sealed class Harass : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            // Only execute this mode when the orbwalker is on harass mode
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass);
        }

        public override void Execute()
        {
            // TODO: Add harass logic here + w
            // Q logic
            var target = TargetSelector.GetTarget(Q.Range, DamageType.Magical);
            if (target == null || target.IsZombie || target.HasUndyingBuff()) return;
            if (Q.IsReady() && Settings.UseQ && Player.ManaPercent > Settings.ManaUsage)
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
        }
    }
}
