using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

namespace nabbEBSyndra
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
            const DamageType damageType = DamageType.Magical;
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
                    //ACTIVE: Syndra conjures a Dark Sphere at the target location, dealing magic damage to all enemies hit, which then remains for 6 seconds.
                    //MAGIC DAMAGE: 50 / 95 / 140 / 185 / 230(+75 % AP)
                    damage = new float[] { 50, 95, 140, 185, 230 }[spellLevel] + 0.75f * Player.Instance.TotalMagicalDamage;
                    break;

                case SpellSlot.W:
                    /*
                     * First cast : pickup, Second cast : drop it
                    MAGIC DAMAGE: 70 / 110 / 150 / 190 / 230(+70 % AP)
                    // TODO TRANSCENDENT BONUS: Force of Will deals 「 20% bonus 」 true damage. 
                    */
                    damage = new float[] {70, 110, 150, 190, 230}[spellLevel] + 0.7f * Player.Instance.TotalMagicalDamage;
                    break;

                case SpellSlot.E:
                    //MAGIC DAMAGE: 70 / 115 / 160 / 205 / 250(+50 % AP)
                    // TODO TRANSCENDENT BONUS: Scatter the Weak's area of effect is 50% wider.
                    damage = new float[] { 70, 115, 160, 205, 250 }[spellLevel] + 0.50f * Player.Instance.TotalMagicalDamage;
                    break;

                case SpellSlot.R:
                    /*
                     * ACTIVE: Syndra hurls all of her Dark Sphere.png Dark Spheres at the target enemy champion, dealing magic damage per sphere. This also utilizes the three spheres orbiting her.
                        MAGIC DAMAGE PER SPHERE: 90 / 135 / 180 (+ 20% AP)
                        「 MINIMUM DAMAGE: 270 / 405 / 540 (+ 60% AP) 」
                        //TODO TRANSCENDENT BONUS: Unleashed Power's cast range is increased by 75.
                     */
                    float baseUltDamage = (new float[] {270, 405, 540}[spellLevel] +
                                            0.6f * Player.Instance.TotalMagicalDamage);

                    float ballsUltDamage = (new float[] {90, 135, 180}[spellLevel] +
                                            0.2f * Player.Instance.TotalMagicalDamage) * (3+ SpellManager.SpheresCount());
                    damage = baseUltDamage + ballsUltDamage;
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