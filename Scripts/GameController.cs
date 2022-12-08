using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    bool isResetting = false;
    bool reset = false;
    int level;
    private void Start()
    {

        level = Array.IndexOf(Levels.levels, SceneManager.GetActiveScene().name);
        if (!PlayerPrefs.HasKey("levelReached") || PlayerPrefs.GetInt("levelReached") < level)
        {
            PlayerPrefs.SetInt("levelReached", level);
            print("Reached level: " + PlayerPrefs.GetInt("levelReached"));

        }
        else
        {
            print(PlayerPrefs.GetInt("levelReached"));
        }
    }
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
        if(level + 1 >= Levels.levels.Length)
        {
            SceneManager.LoadScene("Prototype");
        }
        else
        SceneManager.LoadScene(Levels.levels[(level + 1)]);
    }
}
