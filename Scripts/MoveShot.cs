using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveShot : MonoBehaviour {
    [SerializeField] GameObject explosion;
    [SerializeField] float speed;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        transform.Translate(Vector3.right * speed) ;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag.Equals("Player"))
        {
            print(gameObject.name + " hit the player");
            explosion.transform.parent = null;
            explosion.SetActive(true);
            Destroy(other.gameObject);
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().resetScene(true);
            Destroy(gameObject);
        }
    }


}
