using System;
using EloBuddy;
using EloBuddy.SDK;

namespace nabbEBCait.Modes
{
    public sealed class PermaActive : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            // Since this is permaactive mode, always execute the loop
            return true;
        }

        public override void Execute()
        {
            // TODO: Add permaactive logic here, good for spells like Ignite or Smite OR items
            Orbwalker.DisableMovement = SpellManager.IsCastingUlt;
            Obj_AI_Base.OnBuffGain += OnBuffGain;
            // if snared or anything use qss
        }

        private static void OnBuffGain(Obj_AI_Base sender, Obj_AI_BaseBuffGainEventArgs args)
        {
            if (!sender.IsMe)
            {
                return;
            }
            if (args != null && (args.Buff.Type == BuffType.Taunt || args.Buff.Type == BuffType.Stun || args.Buff.Type == BuffType.Snare || args.Buff.Type == BuffType.Polymorph || args.Buff.Type == BuffType.Blind || args.Buff.Type == BuffType.Fear || args.Buff.Type == BuffType.Charm || args.Buff.Type == BuffType.Suppression || args.Buff.Type == BuffType.Silence))
            {
                Qss();
            }
        }

        private static void Qss()
        {
            // items
            var Qss = new Item((int)ItemId.Quicksilver_Sash);
            var Mercurial = new Item((int)ItemId.Mercurial_Scimitar);
            
            if (Qss.IsOwned() && Qss.IsReady())
            {
                Qss.Cast();
            }

            if (Mercurial.IsOwned() && Mercurial.IsReady())
            {
                Mercurial.Cast();
            }
        }
    }
}
