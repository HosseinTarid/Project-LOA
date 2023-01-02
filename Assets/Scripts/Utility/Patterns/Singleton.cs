using UnityEngine;

namespace Utility.Patterns
{
    public class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;
        public static T Instance { get { if (!instance) instance = GameObject.FindObjectOfType<T>(); return instance; } }

        protected virtual void Awake() => instance = this as T;
    }
}