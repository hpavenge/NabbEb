using System;
using EloBuddy;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Rendering;
using SharpDX;

namespace nabbEBCait
{
    public static class Program
    {

        public static void Main(string[] args)
        {
            // Wait till the loading screen has passed
            Loading.OnLoadingComplete += OnLoadingComplete;
        }

        private static void OnLoadingComplete(EventArgs args)
        {
            // Verify the champion
            if (Player.Instance.Hero != Champion.Caitlyn)
            {
                return;
            }

            // Initialize the classes
            Config.Initialize();
            SpellManager.Initialize();
            ModeManager.Initialize();

            // Listen to events we need
            Drawing.OnDraw += OnDraw;
            Gapcloser.OnGapcloser += OnGapcloser;
        }

        private static void OnDraw(EventArgs args)
        {
            // Draw range circles of our spells
            Circle.Draw(Color.Red, SpellManager.Q.Range, Player.Instance.Position);
            // TODO: Uncomment if you want those enabled aswell, but remember to enable them
            // TODO: in the SpellManager aswell, otherwise you will get a NullReferenceException
            //Circle.Draw(Color.Red, SpellManager.W.Range, Player.Instance.Position);
            //Circle.Draw(Color.Red, SpellManager.E.Range, Player.Instance.Position);
            //Circle.Draw(Color.Red, SpellManager.R.Range, Player.Instance.Position);
        }

        private static void OnGapcloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs args)
        {
            if (sender.IsEnemy && SpellManager.E.IsReady() && SpellManager.E.IsInRange(args.End))
            {
                // Cast E on the gapcloser caster
                SpellManager.E.Cast(sender);
            }
        }
    }
}
