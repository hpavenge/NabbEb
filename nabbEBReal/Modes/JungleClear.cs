using EloBuddy;
using EloBuddy.SDK;
using Settings = nabbEBReal.Config.Modes.JungleClear;

namespace nabbEBReal.Modes
{
    public sealed class JungleClear : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            // Only execute this mode when the orbwalker is on jungleclear mode
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear);
        }

        public override void Execute()
        {
            // TODO: Add jungleclear logic here Hi im ezreal
            foreach (var minion in EntityManager.MinionsAndMonsters.GetJungleMonsters(Player.Instance.Position, Q.Range))
            {
                if (minion.IsValidTarget())
                {
                    Q.Cast(Q.GetPrediction(minion).CastPosition);
                }
            }
        }
    }
}
