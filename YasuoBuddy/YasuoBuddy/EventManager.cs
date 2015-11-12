using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK.Rendering;
using SharpDX;

namespace YasuoBuddy
{
    internal static class EventManager
    {
        public static bool CanDash(this Obj_AI_Base unit)
        {
            return !unit.HasBuff("YasuoDashWrapper");
        }
    }
}