using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Void : MonoBehaviour {
    [SerializeField] GameObject effect;
    [SerializeField] AudioClip AC;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate()
    {
        GetComponent<Rigidbody>().AddTorque(new Vector3(0, 0, 1), ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
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
