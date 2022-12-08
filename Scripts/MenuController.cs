using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] Transform levelHolder;
    [SerializeField] GameObject levelButtonGO;
    [SerializeField] GameObject backButton, menuPanel;
    [SerializeField] Button continueButton;
    [SerializeField] TMPro.TMP_Text levelName, coinsText;
    public void newGame()
    {
        SceneManager.LoadScene(Levels.levels[0]);
        PlayerPrefs.DeleteKey("levelReached");
        PlayerPrefs.DeleteKey("CoinList");
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("Main_Menu");
    }
    public void Resume()
    {
        menuPanel.SetActive(false);
        Time.timeScale = 1;
    }
    public void Pause()
    {
        menuPanel.SetActive(true);
        Time.timeScale = 0;
    }
    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

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
            SceneManager.LoadScene(Levels.levels[PlayerPrefs.GetInt("levelReached")]);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void updateCoins()
    {
        List<int> CoinList;
        if (PlayerPrefs.HasKey("CoinList"))
        {
            try
            {
                CoinList = (List<int>)JsonConvert.DeserializeObject<List<int>>(PlayerPrefs.GetString("CoinList"));
            }
            catch (Exception e)
            {
                print(e.Message);
                CoinList = new List<int>();
            }
        }
        else
            CoinList = new List<int>();

        coinsText.text = CoinList.Count + "";
    }
    private void Start()
    {
        updateCoins();
        levelName.text = SceneManager.GetActiveScene().name.Replace("_", " ");
        Time.timeScale = 1;
        string levelReached = Levels.levels[0];

        if (PlayerPrefs.HasKey("levelReached"))
        {
            
            levelReached = Levels.levels[PlayerPrefs.GetInt("levelReached")];
 
        }
        bool reached = true;
        foreach(string level in Levels.levels)
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
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !SceneManager.GetActiveScene().name.Equals("Main_Menu"))
        {
            if(menuPanel.activeInHierarchy)
                Resume();
            else
                Pause();
        }
    }
}

public static class Levels
{
    public static string[] levels = new string[] { "Easy_As_Pie", "Need_A_Boost", "Into_Orbit", "Speedster", "Pong","Mind_The_Gap","Mirror_Mirror","Scary_Go_Round"};
}
