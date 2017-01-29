using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using Settings = nabbEBReal.Config.Modes.LaneClear;

namespace nabbEBReal.Modes
{
    public sealed class LaneClear : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            // Only execute this mode when the orbwalker is on laneclear mode
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear);
        }

        public override void Execute()
        {
            // TODO: Add laneclear logic here Hi im ezreal
            if (Settings.UseQ && Q.IsReady())
            {
                bool lastQ = false;
                var minions = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy,
                    Player.Instance.ServerPosition, SpellManager.Q.Range).OrderBy(h => h.Health);
                {
                    if (minions.Any() && !lastQ)
                    {
                        var getHealthyCs = minions.GetEnumerator();
                        while (getHealthyCs.MoveNext())
                        {
                            Q.Cast(Q.GetPrediction(minions.Last()).CastPosition);
                        }
                    }
                }
            }


            if (Settings.UseW && W.IsReady())
            {
                var heroes = EntityManager.Heroes.Allies;
                var collision = new List<AIHeroClient>();

                var startPos = Player.Instance.Position.To2D();

                foreach (var hero in heroes.Where(hero => !hero.IsDead))
                {
                    if (hero.Position.Distance(Player.Instance.Position.To2D()) <= SpellManager.W.Range)
                    {
                        var endPos = startPos.Extend(hero.Position.To2D(), SpellManager.W.Range);
                        if (Prediction.Position.Collision.LinearMissileCollision(hero, startPos, endPos,
                            SpellManager.W.Speed, SpellManager.W.Width, SpellManager.W.CastDelay))
                        {
                            collision.Add(hero);
                        }
                            W.Cast(W.GetPrediction(hero).CastPosition);
                    }
                }
            }
        }
    }
}
