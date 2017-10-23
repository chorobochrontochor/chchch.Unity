using chchch;
using chchch.Invoke;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace chchch.Scene
{
    public delegate void Ch3LoadSceneActivateCallback();
    public delegate void Ch3LoadSceneSuccessCallback(bool p_success, Ch3LoadSceneActivateCallback p_activationCallback);
    public delegate void Ch3LoadSceneProgressCallback(float p_progress);

    public static class Ch3SceneManager
    {
        private const float LOAD_THRESHOLD = 0.9f;
        private const float LOAD_FINISH = 1.0f;

        public static void LoadScene(string p_name, Ch3LoadSceneSuccessCallback p_onSuccess, Ch3LoadSceneProgressCallback p_onProgress)
        {
            Ch3CoroutineManager.Start(LoadSceneEnumerator(p_name, p_onSuccess, p_onProgress));
        }

        private static IEnumerator LoadSceneEnumerator(string p_name, Ch3LoadSceneSuccessCallback p_onSuccess, Ch3LoadSceneProgressCallback p_onProgress)
        {
            AsyncOperation async = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(p_name, UnityEngine.SceneManagement.LoadSceneMode.Single);

            if (async == null)
            {
                p_onSuccess(false, null);

                yield break;
            }

            async.allowSceneActivation = false;

            while (async.progress < LOAD_THRESHOLD)
            {
                p_onProgress(async.progress / LOAD_THRESHOLD);

                yield return null;
            }

            p_onProgress(LOAD_FINISH);

            yield return null;

            p_onSuccess(true, () => { async.allowSceneActivation = true; });

            while (!async.isDone)
            {
                yield return null;
            }

            yield break;
        }
    }
}