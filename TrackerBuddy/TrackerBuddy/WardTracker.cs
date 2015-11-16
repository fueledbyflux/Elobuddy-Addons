using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;

namespace TrackerBuddy
{
    class WardTracker
    {
        public static Dictionary<int, Obj_AI_Base> Wards = new Dictionary<int, Obj_AI_Base>(); 

        public static void Init()
        {
            //GameObject.OnCreate += GameObject_OnCreate;
        }

        private static void GameObject_OnCreate(GameObject sender, EventArgs args)
        {
            var ward = sender as Obj_AI_Base;
            if (ward != null && sender.Name.ToLower().Contains("ward") && sender.IsEnemy)
            {

            }
        }
    }
}
