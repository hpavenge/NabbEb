using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using Settings = nabbEBReal.Config.Modes.Combo;

namespace nabbEBReal.Modes
{
    public sealed class Combo : ModeBase
    {
        public static AIHeroClient Ezreal;
        public override bool ShouldBeExecuted()
        {
            // Only execute this mode when the orbwalker is on combo mode
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo);
        }

        public override void Execute()
        {
            var itemTarget = TargetSelector.GetTarget(400, DamageType.True);
            if (Settings.UseItems && itemTarget != null)
            {
                ItemManager.UseYoumuu(itemTarget);
                ItemManager.UseHydra(itemTarget);
                ItemManager.UseBotrk(itemTarget);
                // TODO improve muramana
                ItemManager.UseMuramana(itemTarget);
            }               
            castE();
            castQ();
            castW();
            if (Settings.UseR && R.IsReady())
            {
                if (Settings.RCount > 1)
                {
                    castRSeveral();
                }
                else
                {
                    castR();
                }
            }
            

        }

        public void castQ()
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
        }

        public void castW()
        {
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

        public void castR()
        {
            // R simple
                var target = R.GetTarget();
                if (target != null)
                {
                    var prediction = R.GetPrediction(target);
                    if (prediction.HitChance >= HitChance.High)
                    {
                        if (R.Cast(prediction.CastPosition))
                        {
                            return;
                        }
                    }
                }
        }

        public void castE()
        {
            if (Settings.UseE && E.IsReady())
            {
                var target = E.GetTarget();
                if (target != null)
                {
                    if (E.Cast(Game.CursorPos))
                    {
                        return;
                    }
                }
            }
        }

        public void castRSeveral()
        {
            //TODO import several R logic (Hi im Ezreal)
            foreach (var hero in EntityManager.Heroes.Enemies.Where(hero => hero.IsValidTarget(3000)))
            {
                var collision = new List<AIHeroClient>();
                var startPos = Player.Instance.Position.To2D();
                var endPos = hero.Position.To2D();
                collision.Clear();
                foreach (
                    var colliHero in
                    EntityManager.Heroes.Enemies.Where(
                        colliHero =>
                            !colliHero.IsDead && colliHero.IsVisible &&
                            colliHero.IsInRange(hero, 3000) && colliHero.IsValidTarget(3000)))
                {
                    if (Prediction.Position.Collision.LinearMissileCollision(colliHero, startPos, endPos,
                        R.Speed, R.Width, R.CastDelay))
                    {
                        collision.Add(colliHero);
                    }
                    if (collision.Count >= Settings.RCount)
                    {
                        R.Cast(hero);
                    }
                }
            }
        }
    }
}
