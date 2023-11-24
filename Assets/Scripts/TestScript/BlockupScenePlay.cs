using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlockupScenePlay : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(setPlay(1.5f));
    }
    IEnumerator setPlay(float num) 
    {
        Debug.Log("This in BlockupScenePlay");
        yield return new WaitForSeconds(num);
        SceneManager.LoadScene("Location_basedGameplay");
    }
}
