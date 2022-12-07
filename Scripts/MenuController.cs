using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] string[] levels;
    [SerializeField] Transform levelHolder;
    [SerializeField] GameObject levelButtonGO;
    public void newGame()
    {
        SceneManager.LoadScene(levels[0]);
    }

    public void Continue()
    {
        if(PlayerPrefs.HasKey("levelReached"))
            SceneManager.LoadScene(PlayerPrefs.GetInt("levelReached"));
    }

    private void Start()
    {
        foreach(string level in levels)
        {
            GameObject levelButton = Instantiate(levelButtonGO, levelHolder);
            levelButton.GetComponent<LevelButton>().level = level;
        }
    }
}
