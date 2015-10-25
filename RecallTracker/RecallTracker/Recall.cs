using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;

namespace RecallTracker
{
    public class Recall
    {
        public Recall(AIHeroClient unit, int recallStart, int recallEnd, int duration)
        {
            Unit = unit;
            RecallStart = recallStart;
            Duration = duration;
            RecallEnd = recallEnd;
            ExpireTime = RecallEnd + 2000;
        }

        public int RecallEnd;
        public int Duration;
        public int RecallStart;
        public int ExpireTime;
        public int CancelT;

        public AIHeroClient Unit;

        public bool IsAborted;

        public void Abort()
        {
            CancelT = Environment.TickCount;
            ExpireTime = Environment.TickCount + 2000;
            IsAborted = true;
        }

        private float Elapsed { get { return ((CancelT > 0 ? CancelT : Environment.TickCount) - RecallStart); } }

        public float PercentComplete()
        {
            return (float) Math.Round((Elapsed / Duration) * 100) > 100 ? 100 : (float)Math.Round((Elapsed / Duration) * 100);
        }
    }
}
