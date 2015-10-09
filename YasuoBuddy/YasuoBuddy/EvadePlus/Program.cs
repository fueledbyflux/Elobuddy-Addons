using EloBuddy.SDK.Events;

namespace YasuoBuddy.EvadePlus
{
    internal class Program
    {
        private static SkillshotDetector _skillshotDetector;
        public static EvadePlus Evade;

        public static void Main(string[] args)
        {
            _skillshotDetector = new SkillshotDetector();
            Evade = new EvadePlus(_skillshotDetector);
            EvadeMenu.CreateMenu();
        }
    }
}
