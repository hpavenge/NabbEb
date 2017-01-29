using EloBuddy;
using EloBuddy.SDK;

namespace nabbEBReal
{
    public static class Damages
    {
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
                    // ACTIVE: Ezreal fires a bolt of energy in a line
                    // PHYSICAL DAMAGE: 35 / 55 / 75 / 95 / 115 (+ 110% AD) (+ 40% AP)
                    damage = new float[] {35, 55, 75, 95, 115}[spellLevel] + (1.1f * Player.Instance.TotalAttackDamage) +
                             (0.4f * Player.Instance.TotalMagicalDamage);
                    break;

                case SpellSlot.W:
                    // ACTIVE: Ezreal fires a wave of energy in a line, dealing magic 
                    // MAGIC DAMAGE: 70 / 115 / 160 / 205 / 250 (+ 80% AP)
                    damage = new float[] {70, 115, 160, 205, 250}[spellLevel] +
                             0.8f * Player.Instance.TotalMagicalDamage;
                    break;

                case SpellSlot.E:
                    // ACTIVE: Ezreal blinks to the target location, firing a homing bolt that deals magic damage to the nearest enemy.
                    // MAGIC DAMAGE: 75 / 125 / 175 / 225 / 275 (+ 50% bonus AD) (+ 75% AP)
                    damage = new float[] {75, 125, 175, 225, 275}[spellLevel] +
                             (0.5f * Player.Instance.TotalMagicalDamage) + (0.75f * Player.Instance.TotalMagicalDamage);
                    break;
                case SpellSlot.R:
                    //ACTIVE: After gathering energy for 1 second, Ezreal fires an Trueshot Barrage Minimap energy projectile in the target direction
                    //  MAGIC DAMAGE: 350 / 500 / 650 (+ 100% bonus AD) (+ 90% AP) 」
                    // TODO Each enemy hit reduces the projectile's damage by 10%, down to a minimum 30% damage. (for now auto reduce dmg to 0.6 instead of 0.9)
                    damage = new float[] {350, 500, 650}[spellLevel] + 0.6f * Player.Instance.TotalMagicalDamage;
                    break;
            }

            // No damage set
            if (damage <= 0)
            {
                return 0;
            }

            // Calculate damage on target and return (-20 to make it actually more accurate Kappa) -> Hellsing
            return Player.Instance.CalculateDamageOnUnit(target, damageType, damage) - 20;
        }
    }
}