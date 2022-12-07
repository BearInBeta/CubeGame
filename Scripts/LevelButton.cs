using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelButton : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text levelText;
    public string level;
    public void GoToLevel()
    {
            SceneManager.LoadScene(level);
    }

    private void Update()
    {
        levelText.text = level.Replace("_", " ");
    }
}
