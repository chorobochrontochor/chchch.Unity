using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace chchch
{
    public static class CoroutineManager
    {
        private class _coroutineBehaviour : MonoBehaviour { };
        private static _coroutineBehaviour _monoBehaviour;

        public static Coroutine Start(IEnumerator p_enumerator)
        {
            if (_monoBehaviour == null)
            {
                GameObject gameObject = new GameObject(typeof(CoroutineManager).Name);
                gameObject.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector | HideFlags.DontSave;
                Object.DontDestroyOnLoad(gameObject);
                _monoBehaviour = gameObject.AddComponent<_coroutineBehaviour>();
            }

            return _monoBehaviour.StartCoroutine(p_enumerator);
        }

        public static void Kill(Coroutine p_coroutine)
        {
            _monoBehaviour.StopCoroutine(p_coroutine);
        }

        public static void KillAll()
        {
            _monoBehaviour.StopAllCoroutines();
        }
    }
}