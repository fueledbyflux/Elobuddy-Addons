using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;

namespace ProjectRiven
{
    class BurstHandler
    {
        public static void Init()
        {
            Obj_AI_Base.OnSpellCast += Obj_AI_Base_OnSpellCast;
        }

        private static void Obj_AI_Base_OnSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe) return;
        }
    }
}
