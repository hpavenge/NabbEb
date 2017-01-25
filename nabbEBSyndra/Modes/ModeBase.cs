using EloBuddy;
using EloBuddy.SDK;

namespace nabbEBSyndra.Modes
{
    public abstract class ModeBase
    {
        // Change the spell type to whatever type you used in the SpellManager
        // here to have full features of that spells, if you don't need that,
        // just change it to Spell.SpellBase, this way it's dynamic with still
        // the most needed functions
        public static AIHeroClient Player
        {
            get { return EloBuddy.Player.Instance; }
        }

        protected Spell.Skillshot Q
        {
            get { return SpellManager.Q; }
        }
        protected Spell.Skillshot W
        {
            get { return SpellManager.W; }
        }
        protected Spell.Skillshot E
        {
            get { return SpellManager.E; }
        }
        protected Spell.Targeted R
        {
            get { return SpellManager.R; }
        }

        protected static Spell.Skillshot QE
        {
            get { return SpellManager.QE; }
        }

        public abstract bool ShouldBeExecuted();

        public abstract void Execute();
    }
}
