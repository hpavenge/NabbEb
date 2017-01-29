using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using Settings = nabbEBReal.Config.Misc;

namespace nabbEBReal.Modes
{
    public sealed class PermaActive : ModeBase
    {
        private int _lastAltert;

        public override bool ShouldBeExecuted()
        {
            // Since this is permaactive mode, always execute the loop
            return true;
        }

        public override void Execute()
        {
            // TODO: Add permaactive logic here
            if (Settings.StackTear)
            {
                StackTear();
            }
            if (Settings.QMinions)
            {
                QMinions();
            }
            if (Settings.AutoQCC)
            {
                _AutoQCC();
            }
            // Qss on buff that is snare or anything
            Obj_AI_Base.OnBuffGain += OnBuffGain;

            // Alerter for ultimate Hellsing
            if (Settings.Alerter && R.IsReady() && Environment.TickCount - _lastAltert > 5000)
            {
                // Get targets that can die with R
                var killableTargets = EntityManager.Heroes.Enemies
                    .Where(x => x.IsValidTarget(R.Range) && x.TotalHealth() < R.GetRealDamage(x));
                if (killableTargets.Any())
                {
                    _lastAltert = Environment.TickCount;
                    Chat.Print("Targets Killable:" + killableTargets.Select(x => x.ChampionName));
                    //Chat.Print(string.Format("[{0}:{1:D2}] Targets killable: {2}", Math.Floor(time.TotalMinutes), time.Seconds, string.Join(", ", killableTargets.Select(t => t.ChampionName))));
                }
            }
        }

        private static void OnBuffGain(Obj_AI_Base sender, Obj_AI_BaseBuffGainEventArgs args)
        {
            if (!sender.IsMe)
            {
                return;
            }
            if (args != null && (args.Buff.Type == BuffType.Taunt || args.Buff.Type == BuffType.Stun || args.Buff.Type == BuffType.Snare || args.Buff.Type == BuffType.Polymorph || args.Buff.Type == BuffType.Blind || args.Buff.Type == BuffType.Fear || args.Buff.Type == BuffType.Charm || args.Buff.Type == BuffType.Suppression || args.Buff.Type == BuffType.Silence))
            {
                if (Settings.UseQss)
                {
                    Qss();
                }

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

        private static void StackTear()
        {
            if (Player.Instance.IsInShopRange())
            {
                if (ItemManager.Tear.IsOwned() || ItemManager.Manamumu.IsOwned())
                {
                    if (SpellManager.Q.Cast(Game.CursorPos))
                    {
                        return;
                    }
                }
            }
        }
        // Hi im Ezreal
        private static void QMinions()
        {
            foreach (var minions in EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy,Player.Instance.ServerPosition, SpellManager.Q.Range))
            {
                if (Prediction.Health.GetPrediction(minions, (int)(Player.Instance.AttackDelay * 1000)) <= 0 &&
                        (!Orbwalker.CanAutoAttack || !Player.Instance.IsInAutoAttackRange(minions)))
                {
                    if (Player.Instance.GetSpellDamage(minions, SpellSlot.Q) >= minions.Health &&
                        (Orbwalker.LastTarget == null || Orbwalker.LastTarget.NetworkId != minions.NetworkId))
                    {
                        SpellManager.Q.Cast(SpellManager.Q.GetPrediction(minions).CastPosition);
                    }
                        
                }
            }
        }

        private static void _AutoQCC()
        {
            foreach (var enemy in EntityManager.Heroes.Enemies)
            {
                if (enemy.IsStunned || enemy.IsTaunted || enemy.IsFeared || enemy.IsCharmed)
                {
                    if (SpellManager.Q.IsReady())
                    {
                        if (SpellManager.Q.Cast(enemy))
                        {
                            return;
                        }
                    }
                }
            }
        }
    }
}
