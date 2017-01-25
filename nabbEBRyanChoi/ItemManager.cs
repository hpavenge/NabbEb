using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;

namespace nabbEBRyanChoi
{
    public class ItemManager
    {
        /*
         * Hellsing Legend <3
         * */
        #region Items
        public static readonly Item Tiamat = new Item(ItemId.Tiamat_Melee_Only, 400);
        public static readonly Item Hydra = new Item(ItemId.Ravenous_Hydra_Melee_Only, 400);

        public static readonly Item Cutlass = new Item(ItemId.Bilgewater_Cutlass, 550);
        public static readonly Item Botrk = new Item(ItemId.Blade_of_the_Ruined_King, 550);

        public static readonly Item Youmuu = new Item(ItemId.Youmuus_Ghostblade);
        #endregion

        #region methods
        public static bool UseHydra(Obj_AI_Base target)
        {
            if (target == null)
            {
                return false;
            }
            if (Hydra.IsReady() && target.IsValidTarget(Hydra.Range))
            {
                return Hydra.Cast();
            }
            if (Tiamat.IsReady() && target.IsValidTarget(Tiamat.Range))
            {
                return Tiamat.Cast();
            }
            return false;
        }

        public static bool UseBotrk(AIHeroClient target)
        {
            if (target == null)
            {
                return false;
            }
            if (Botrk.IsReady() && target.IsValidTarget(Botrk.Range) &&
                Player.Instance.Health + Player.Instance.GetItemDamage(target, Botrk.Id) < Player.Instance.MaxHealth)
            {
                return Botrk.Cast(target);
            }
            if (Cutlass.IsReady() && target.IsValidTarget(Cutlass.Range))
            {
                return Cutlass.Cast(target);
            }
            return false;
        }

        public static bool UseYoumuu(Obj_AI_Base target)
        {
            if (target == null)
            {
                return false;
            }
            if (Youmuu.IsReady() && target.IsValidTarget(Player.Instance.GetAutoAttackRange(target)))
            {
                return Youmuu.Cast();
            }
            return false;
        }
        #endregion

    }
}
