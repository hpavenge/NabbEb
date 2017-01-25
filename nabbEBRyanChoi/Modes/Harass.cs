// Using the config like this makes your life easier, trust me

using EloBuddy;
using EloBuddy.SDK;
using Settings = nabbEBRyanChoi.Config.Modes.Harass;

namespace nabbEBRyanChoi.Modes
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
            // TODO: Add harass logic here
            // See how I used the Settings.UseQ and Settings.Mana here, this is why I love
            // my way of using the menu in the Config class!
            
        }
    }
}
