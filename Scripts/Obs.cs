using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obs : MonoBehaviour
{
    [SerializeField] GameObject explosion;
    [SerializeField] float speed;
    Vector3 pos;
    // Use this for initialization
    void Start()
    {
        pos = transform.position;
        GetComponent<Rigidbody>().AddForce(Vector3.right * speed);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Boundry"))
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            transform.position = pos;
            GetComponent<Rigidbody>().AddForce(Vector3.right * speed);

        }




    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            GameObject.FindGameObjectWithTag("MainCamera").transform.parent = null;
            print(gameObject.name + " hit the player");
            explosion.transform.parent = null;
            explosion.SetActive(true);
            Destroy(other.gameObject);
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().resetScene(true);
            Destroy(gameObject);
        }
    }
}
