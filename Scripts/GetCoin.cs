using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;

public class GetCoin : MonoBehaviour {
    [SerializeField] GameObject particle;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag.Equals("Player"))
        {
            if (SceneManager.GetActiveScene().name == "Main_Menu")
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

                int level = Array.IndexOf(Levels.levels, SceneManager.GetActiveScene().name);


                if (!CoinList.Contains(level))
                {
                    CoinList.Add(level);
                    string coinJson = JsonConvert.SerializeObject(CoinList);
                    PlayerPrefs.SetString("CoinList", coinJson);
                    print(coinJson);
                    GameObject.FindGameObjectWithTag("MainMenu").GetComponent<MenuController>().updateCoins();
                }
            }
            else
            {
                GameObject.FindGameObjectWithTag("Void").GetComponent<Void>().coinCollected = true;
                print("coin collected");
            }
            GameObject.Instantiate(particle, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
