using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    bool isResetting = false;
    bool reset = false;
    public void resetScene(bool wait)
    {
        if(wait)
        StartCoroutine(resetSceneWait());
        else
        {
            isResetting = true;
        }
        
        
    }
    IEnumerator resetSceneWait()
    {
        yield return new WaitForSeconds(1.5f);
        isResetting = true;
    }

    private void Update()
    {

        if (isResetting && !reset)
        {
            reset = true;
            isResetting = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void nextScene()
    {
        SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings);
    }
}
