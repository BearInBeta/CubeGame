using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1 : MonoBehaviour {
    [SerializeField] GameObject shot;
    [SerializeField] GameObject Body;
    [SerializeField] AudioClip Off;
    [SerializeField] GameObject player;
    public float waitTime = 2f;
    float speed;
    public bool rotating = false;
    public bool hitWhileRotating = false;
    public bool gothit = false;
    public bool waiting = false;
    public float waitingTime = 3;
    public int noShots = 20;
    public int detectChance = 6;
    public int maxSpeed = 80;
    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {

        transform.Rotate(new Vector3(0, 1, 0) * speed);

        if(!rotating && !waiting)
        {
           StartCoroutine(Shoot());

        }
    }
    IEnumerator Shoot()
    {
        waiting = true;
        yield return new WaitForSeconds(waitingTime);

        GetComponent<AudioSource>().Play();

        for (int i = 0; i < maxSpeed; i+=10)
        {
            Body.GetComponent<Renderer>().material.SetColor("_Color", Color.Lerp(Body.GetComponent<Renderer>().material.color, Color.red, 0.1f));
            speed = i;
            yield return new WaitForSeconds(0.2f);

        }

        rotating = true;
        waiting = false;
        gothit = false;

        for (int i = 0; i < noShots; i++)
        {

            if (gothit)
            {
                break;
            }
            if(Random.Range(0, detectChance) == 0 && player != null)
             {
                if(player.transform.position.x != 0 || player.transform.position.y != 0)
                    GameObject.Instantiate(shot, transform.position, Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(player.transform.position.y, player.transform.position.x)));
                else
                    GameObject.Instantiate(shot, transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360)));

            }
            else
             GameObject.Instantiate(shot, transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360)));

            if (hitWhileRotating)
                GameObject.Instantiate(shot, transform.position, Quaternion.Euler(0, 0, 90));


            yield return new WaitForSeconds(waitTime);
        }
        rotating = false;
        waiting = true;
        gothit = false;
        for (int i = 50; i > 0; i -= 10)
        {
            speed = i;
            Body.GetComponent<Renderer>().material.SetColor("_Color", Color.Lerp(Body.GetComponent<Renderer>().material.color, Color.white, 0.2f));

            yield return new WaitForSeconds(0.2f);

        }
        speed = 0;
        Body.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
        GetComponent<AudioSource>().Stop();

        GetComponent<AudioSource>().PlayOneShot(Off);

        transform.rotation = Quaternion.identity;
        waiting = false;


    }
}
