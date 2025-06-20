using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneReloaderOnce : MonoBehaviour
{
    private static bool alreadyReloaded = false;

    private void Start()
    {
#if UNITY_WEBGL
        if (!alreadyReloaded)
        {
            alreadyReloaded = true;
            StartCoroutine(ReloadAfterFrames(5)); 
        }
#endif
    }

    IEnumerator ReloadAfterFrames(int frameCount)
    {
        for (int i = 0; i < frameCount; i++)
            yield return null;

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
