using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Void : MonoBehaviour {
    [SerializeField] GameObject effect;
    [SerializeField] AudioClip AC;
    public bool coinCollected = false;
    [SerializeField] bool voidHidden = false;
	// Use this for initialization
	void Start () {
        if (voidHidden)
        {
            voidHide();
        }
        coinCollected = false;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void voidHide()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        GetComponent<MeshRenderer>().enabled = false;
    }
    public void voidShow()
    {
        voidHidden = false;
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
        GetComponent<MeshRenderer>().enabled = true;
    }
    private void FixedUpdate()
    {
        GetComponent<Rigidbody>().AddTorque(new Vector3(0, 0, 1), ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!voidHidden && other.gameObject.tag.Equals("Player"))
        {
            if(other.gameObject.transform.Find("camera") != null)
            other.gameObject.transform.Find("camera").parent = null;
            other.gameObject.SetActive(false);
            Instantiate(effect, transform.position, Quaternion.identity);
            GetComponent<AudioSource>().PlayOneShot(AC);
            foreach(GameObject p in GameObject.FindGameObjectsWithTag("Player"))
            {
                if (p.activeInHierarchy == true)
                    return;
            }
            StartCoroutine(endLvl());

        }
    }
    IEnumerator endLvl()
    {
        if (coinCollected)
        {
            print("coin was collected: " + coinCollected);
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
            }
        }

        yield return new WaitForSeconds(0.5f);
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        Instantiate(effect, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(1.2f);

        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().nextScene();

    }
}
