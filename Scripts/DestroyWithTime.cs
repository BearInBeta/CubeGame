using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWithTime : MonoBehaviour {
    GameObject g;
    // Use this for initialization
    void Start () {
        StartCoroutine(destroyWTime());
	}
	
	// Update is called once per frame
	void Update () {

	}

    IEnumerator destroyWTime()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
