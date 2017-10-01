using chchch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SceneManager
{
    private const float LOAD_THRESHOLD = 0.9f;
    private const float LOAD_FINISH = 1.0f;

    public delegate void LoadSceneActivateCallback();
    public delegate void LoadSceneSuccessCallback(bool p_success, LoadSceneActivateCallback p_activationCallback);
    public delegate void LoadSceneProgressCallback(float p_progress);

    public static void LoadScene(string p_name, LoadSceneSuccessCallback p_onSuccess, LoadSceneProgressCallback p_onProgress)
    {
        CoroutineManager.Start(LoadSceneEnumerator(p_name, p_onSuccess, p_onProgress));
    }

    private static IEnumerator LoadSceneEnumerator(string p_name, LoadSceneSuccessCallback p_onSuccess, LoadSceneProgressCallback p_onProgress)
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
