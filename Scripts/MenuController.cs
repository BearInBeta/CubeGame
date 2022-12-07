using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] string[] levels;
    [SerializeField] Transform levelHolder;
    [SerializeField] GameObject levelButtonGO;
    [SerializeField] GameObject backButton, menuPanel;
    [SerializeField] Button continueButton;
    public void newGame()
    {
        SceneManager.LoadScene(levels[0]);
        PlayerPrefs.DeleteKey("levelReached");
    }
    public void selectLevels()
    {
        levelHolder.gameObject.SetActive(true);
        backButton.SetActive(true);
        menuPanel.SetActive(false);
    }

    public void backToMenu()
    {
        levelHolder.gameObject.SetActive(false);
        backButton.SetActive(false);
        menuPanel.SetActive(true);
    }

    public void Continue()
    {
        if(PlayerPrefs.HasKey("levelReached"))
            SceneManager.LoadScene(levels[PlayerPrefs.GetInt("levelReached")]);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    private void Start()
    {
        string levelReached = levels[0];

        if (PlayerPrefs.HasKey("levelReached"))
        {
            
            levelReached = levels[PlayerPrefs.GetInt("levelReached")];
 
        }
        bool reached = true;
        foreach(string level in levels)
        {
            GameObject levelButton = Instantiate(levelButtonGO, levelHolder);
         
            levelButton.GetComponent<Button>().interactable = reached;
            
            levelButton.GetComponent<LevelButton>().level = level;
            if(levelReached.Equals(level))
            {
                reached = false;
            }
        }

        
            continueButton.interactable = PlayerPrefs.HasKey("levelReached");
        
    }
}
