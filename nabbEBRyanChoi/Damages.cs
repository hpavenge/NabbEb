using EloBuddy;
using EloBuddy.SDK;

namespace nabbEBRyanChoi
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
                    //ACTIVE: Rengar slashes all enemies in an arc in the target direction before piercing all enemies in line, dealing physical damage per strike.
                    //「 PHYSICAL DAMAGE PER STRIKE: 35 / 55 / 75 / 95 / 115(+30 / 40 / 50 / 60 / 70 % bonus AD) 」
                    //Empowered Savagery
                    //TODO EMPOWERED ACTIVE: Savagery's total physical damage is increased to 120 - 392 (based on level)= 104+(16*level) (+ 240% bonus AD).
                    damage = new float[] {35, 55, 75, 95, 115}[spellLevel] +
                             new float[] {0.3f, 0.4f, 0.5f, 0.6f, 0.7f}[spellLevel] * Player.Instance.TotalAttackDamage;
                    break;

                case SpellSlot.W:
                    /*
                     *ACTIVE: Rengar lets out a battle roar, dealing magic damage to nearby enemies and healing Rengar for his Grey Health.
                        MAGIC DAMAGE: 50 / 80 / 110 / 140 / 170 (+ 80% AP)
                        Empowered Battle Roar	
                        //TODO EMPOWERED ACTIVE: Battle Roar's base magic damage is modified to 50 - 220 (based on level) = 40+10 per level, will remove all active crowd control effects
                    */
                    damage = new float[] {50, 80, 110, 140, 170}[spellLevel] + 0.8f * Player.Instance.TotalMagicalDamage;
                    break;

                case SpellSlot.E:
                    /*
                     * ACTIVE: Rengar throws a bola in the target direction, dealing physical damage to the first enemy hit and Slow icon slowing them for 1.75 seconds.
                        PHYSICAL DAMAGE: 50 / 95 / 140 / 185 / 230 (+ 70% bonus AD)
                        SLOW: 30 / 45 / 60 / 75 / 90%
                        Empowered Bola Strike	
                        //TODO EMPOWERED ACTIVE: Bola Strike's base physical damage is increased to 50 - 305 (based on level) and using it Root icon roots its target for 1.75 seconds.
                     * */
                    damage = new float[] { 50, 95, 140, 185, 230 }[spellLevel] + 0.50f * Player.Instance.TotalMagicalDamage;
                    break;

                    // ulti MANUAL
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