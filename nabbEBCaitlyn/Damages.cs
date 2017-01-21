using EloBuddy;
using EloBuddy.SDK;

namespace nabbEBCait
{
    public static class Damages
    {
        // Hellsing the beast really this makes shit much simpler though
        public static float GetTotalDamage(AIHeroClient target)
        {
            // Auto attack
            var damage = Player.Instance.GetAutoAttackDamage(target);

            // Q
            if (SpellManager.Q.IsReady())
            {
                damage += SpellManager.Q.GetRealDamage(target);
            }

            // W
            if (SpellManager.W.IsReady())
            {
                damage += SpellManager.W.GetRealDamage(target);
            }

            // E
            if (SpellManager.E.IsReady())
            {
                damage += SpellManager.E.GetRealDamage(target);
            }

            // R
            if (SpellManager.R.IsReady())
            {
                damage += SpellManager.R.GetRealDamage(target);
            }

            return damage;
        }

        public static float GetRealDamage(this Spell.SpellBase spell, Obj_AI_Base target)
        {
            return spell.Slot.GetRealDamage(target);
        }

        public static float GetRealDamage(this SpellSlot slot, Obj_AI_Base target)
        {
            // Helpers
            var spellLevel = Player.Instance.Spellbook.GetSpell(slot).Level;
            const DamageType damageType = DamageType.Physical;
            float damage = 0;

            // Validate spell level
            if (spellLevel == 0)
            {
                return 0;
            }
            spellLevel--;

            switch (slot)
            {
                case SpellSlot.Q:
                    //「 PHYSICAL DAMAGE: 30 / 70 / 110 / 150 / 190 (+ 130 / 140 / 150 / 160 / 170% AD) 」
                    // after which it expands in width but deals only 67% damage to all enemies it passes through thereafter.
                    damage = (new [] { 30, 70, 110, 150, 190 }[spellLevel] + new[] {1.3f, 1.4f, 1.5f, 1.6f, 1.7f}[spellLevel] * Player.Instance.TotalAttackDamage) * 0.67f;
                    break;

                case SpellSlot.W:
                    // HEADSHOT DAMAGE INCREASE: 30 / 70 / 110 / 150 / 190 (+ 70% AD)
                    // passive not implemented
                    damage = 0;
                    break;

                case SpellSlot.E:
                    //ACTIVE: Caitlyn fires a net and dashes in the opposite direction, dealing magic damage to the first enemy hit and Slow icon slowing them by 50% for 1 second.
                    // MAGIC DAMAGE: 70 / 110 / 150 / 190 / 230(+80 % AP)
                    damage = new float[] { 70, 110, 150, 190, 230 }[spellLevel] + 0.80f * Player.Instance.TotalMagicalDamage;
                    break;

                case SpellSlot.R:
                    // If Caitlyn completes the channel, she fires a homing projectile toward the target that deals physical damage to the first enemy champion it hits.Other enemy champions can intercept the shot.
                    // PHYSICAL DAMAGE: 250 / 475 / 700(+200 % bonus AD)
                    // Xerath calls down a blast of arcane energy to the target area which strikes after 0.5 seconds delay,
                    // dealing magic damage to all enemies within. Each cast has a static cooldown of 0.8 seconds.
                    damage = new float[] { 250, 475, 700 }[spellLevel] + 2.0f * Player.Instance.TotalAttackDamage;
                    break;
            }

            // No damage set
            if (damage <= 0)
            {
                return 0;
            }

            // Calculate damage on target and return (-20 to make it actually more accurate Kappa) Hellsing lord of scriptorz
            return Player.Instance.CalculateDamageOnUnit(target, damageType, damage) - 20;
        }
    }
}