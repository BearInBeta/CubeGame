using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReguShot : MonoBehaviour {

    [SerializeField] GameObject explosion;
    [SerializeField] float speed;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(Vector3.right * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            explosion.SetActive(true);

            other.gameObject.GetComponent<BoxCollider>().enabled = false;
            other.GetComponentInChildren<MeshRenderer>().enabled = false;
            other.gameObject.GetComponent<PlayerController>().enabled = false;

            StartCoroutine(resetScene());
        }
    }

    IEnumerator resetScene()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
