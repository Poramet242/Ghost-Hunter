using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    public void GoToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        //StartCoroutine(LoadScene(sceneName, objectToMove));
    }
    IEnumerator LoadScene(string sceneName, List<GameObject> objectToMove)
    {
        SceneManager.LoadSceneAsync(sceneName);
        SceneManager.sceneLoaded += (newScene, mode) =>
        {
            SceneManager.SetActiveScene(newScene);
        };

        Scene sceneToLoad = SceneManager.GetSceneByName(sceneName);
        foreach (GameObject item in objectToMove)
        {
            SceneManager.MoveGameObjectToScene(item, sceneToLoad);
        }
        yield return null;
    }
}
