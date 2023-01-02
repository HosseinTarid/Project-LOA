using UnityEngine;

namespace UI.Core
{
    public static class RouterBase
    {
        public static ScreenState Current { private set; get; }

        public static void ChangeState(ScreenState newState)
        {
            Current?.Hide();
            Current = newState;
            Current?.Show();
        }
    }
}
