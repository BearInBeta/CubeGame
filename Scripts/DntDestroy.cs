using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DntDestroy : MonoBehaviour {

    void Awake()
    {

        if (GameObject.FindGameObjectsWithTag("Music").Length > 1)
            foreach(GameObject g in GameObject.FindGameObjectsWithTag("Music"))
            {
                Destroy(GameObject.FindGameObjectsWithTag("Music")[0]);
            }
        
        
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        DontDestroyOnLoad(transform.gameObject);

    }
}
