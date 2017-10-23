using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace chchch.Invoke
{
    public static class Ch3CoroutineManager
    {
        private class _coroutineBehaviour : MonoBehaviour { };

        private static _coroutineBehaviour _monoBehaviour;

        private static _coroutineBehaviour coroutineBehaviour
        {
            get
            {
                if (_monoBehaviour == null)
                {
                    GameObject gameObject = new GameObject(typeof(Ch3CoroutineManager).Name);
                    gameObject.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector | HideFlags.DontSave;
                    Object.DontDestroyOnLoad(gameObject);
                    _monoBehaviour = gameObject.AddComponent<_coroutineBehaviour>();
                }
                
                return _monoBehaviour;
            }
        }
        
        public static Coroutine Start(IEnumerator p_enumerator)
        {
            return coroutineBehaviour.StartCoroutine(p_enumerator);
        }

        public static void Kill(Coroutine p_coroutine)
        {
            coroutineBehaviour.StopCoroutine(p_coroutine);
        }

        public static void KillAll()
        {
            coroutineBehaviour.StopAllCoroutines();
        }
    }
}
