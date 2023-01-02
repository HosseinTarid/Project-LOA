using UnityEngine;

namespace Utility.TimeUtility
{
    public static class TimeManager
    {
        public static bool IsPause => Time.timeScale == 0;

        public static void ResumeGame() => Time.timeScale = 1;
        public static void PauseGame() => Time.timeScale = 0;

        public static void ChangeState()
        {
            if (IsPause) ResumeGame();
            else PauseGame();
        }
    }
}