// Using the config like this makes your life easier, trust me

using System.Collections.Generic;
using System.Linq;
using System.Xml;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using Settings = nabbEBRyanChoi.Config.Modes.Combo;

namespace nabbEBRyanChoi.Modes
{
    public sealed class Combo : ModeBase
    {
        public static AIHeroClient Rengar;
        public override bool ShouldBeExecuted()
        {
            // Only execute this mode when the orbwalker is on combo mode
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo);
        }

        public override void Execute()
        {
            //Use items when off CD 
            var itemTarget = TargetSelector.GetTarget(400, DamageType.True);
            if (itemTarget != null)
            {
                //TODO Use Youmu when in ult (somthing with checkbuff i guess)
                ItemManager.UseYoumuu(itemTarget);
                ItemManager.UseHydra(itemTarget);
                ItemManager.UseBotrk(itemTarget);
            }

            List<Obj_AI_Base> enemies = EntityManager.Heroes.Enemies.ToList().ToObj_AI_BaseList();
            // Empowered hit Spacebar to win mode -> use empowered W if having pain otherwise go for kill combo
            // TODO use Empowered E if target isnt low health and Rengar cant get rooted
            if (Rengar.Mana <= 5)
            {
                if (Rengar.IsRooted || Rengar.IsCharmed || Rengar.IsFleeing || Rengar.IsFeared || Rengar.IsStunned)
                {
                    if (W.Cast())
                    {
                        return;
                    }
                }
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
                if (target!= null)
                {
                    if (enemies.Any(x => x.IsInRange(Rengar,SpellManager.W.Range)))
                    {
                        if (W.Cast())
                        {
                            return;
                        }
                    }
                }
            }
            // E simple
            if (Settings.UseE && E.IsReady())
            {
                var target = E.GetTarget();
                if (target != null)
                {
                    var prediction = E.GetPrediction(target);
                    if (prediction.HitChance >= HitChance.High)
                    {
                        // not sure what the diff is between E.Cast(target) or the prediction thingy
                        if (E.Cast(prediction.CastPosition))
                        {
                            return;
                        }
                    }
                }
            }
        }
    }
}
