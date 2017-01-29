using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using Settings = nabbEBReal.Config.Modes.Harass;
// Using the config like this makes your life easier, trust me

namespace nabbEBReal.Modes
{
    public sealed class Harass : ModeBase
    {
        public static AIHeroClient Ezreal;
        public override bool ShouldBeExecuted()
        {
            // Only execute this mode when the orbwalker is on harass mode
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass);
        }

        public override void Execute()
        {
            if (Ezreal.ManaPercent > Settings.ManaHarras)
            {
                // Q simple
                if (Settings.UseQ && Q.IsReady())
                {
                    var target = Q.GetTarget();
                    if (target != null)
                    {
                        var prediction = Q.GetPrediction(target);
                        if (prediction.HitChance >= HitChance.High)
                        {
                            if (Q.Cast(prediction.CastPosition))
                            {
                                return;
                            }
                        }
                    }
                }

                // W simple
                if (Settings.UseW && W.IsReady())
                {
                    var target = W.GetTarget();
                    if (target != null)
                    {
                        var prediction = W.GetPrediction(target);
                        if (prediction.HitChance >= HitChance.High)
                        {
                            if (W.Cast(prediction.CastPosition))
                            {
                                return;
                            }
                        }
                    }
                }
            }
        }
    }
}
