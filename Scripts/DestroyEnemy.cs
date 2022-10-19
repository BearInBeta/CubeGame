using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DestroyEnemy : MonoBehaviour {
    [SerializeField] GameObject explosion;
    [SerializeField] GameObject Void;
    [SerializeField] GameObject spikes;
    [SerializeField] AudioClip Zap;
    [SerializeField] GameObject electricity;
    [SerializeField] Slider slide;
    [SerializeField] GameObject[] grounds;
    bool first = true;
    public int hp = 0;
    float waitTime = 0.5f;
    bool wait = false;
    // Use this for initialization
    void Start () {
        slide.maxValue = hp;
        slide.value = hp;

    }

    // Update is called once per frame
    void FixedUpdate () {

        if (!wait)
        {
            RaycastHit hit;
            Vector3 up = transform.TransformDirection(Vector3.up * -Physics.gravity.y / Mathf.Abs(Physics.gravity.y));

            if (Physics.Raycast(transform.position, up, out hit, GetComponent<BoxCollider>().bounds.extents.y + 0.1f) || Physics.Raycast(transform.position + new Vector3(transform.localScale.x / 2, 0, 0), up, out hit, GetComponent<BoxCollider>().bounds.extents.y + 0.1f) || Physics.Raycast(transform.position - new Vector3(transform.localScale.x / 2, 0, 0), up, out hit, GetComponent<BoxCollider>().bounds.extents.y + 0.1f))
            {

                if (hit.transform.gameObject.tag.Equals("Player"))
                {
                    if(first)
                    {
                        if(Random.Range(0,1) == 1)
                        {
                            grounds[0].SetActive(false);
                            grounds[1].SetActive(false);
                        }
                        else
                        {
                            grounds[2].SetActive(false);
                            grounds[3].SetActive(false);
                        }

                        first = false;
                    }
                    else
                    {
                        foreach(GameObject g in grounds)
                        {
                            g.SetActive(!g.activeInHierarchy);
                        }
                    }


                    transform.parent.gameObject.GetComponent<Boss1>().gothit = true;
                    hit.transform.gameObject.GetComponent<PlayerController>().protection();
                    hit.transform.gameObject.GetComponent<PlayerController>().canMove = false;
                    wait = true;
                    float f;

                    if (grounds[0].activeInHierarchy)
                        f = 1000;
                    else
                        f = -1000;

                    
                    hit.transform.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(f, 800));
                    hp--;
                    slide.value = hp;
                    if (hp < 0)
                    {
                        hit.transform.gameObject.GetComponent<PlayerController>().canMove = true;

                        Instantiate(explosion, transform.position, Quaternion.identity);
                        Void.SetActive(true);
                        Destroy(slide.gameObject);
                        Destroy(gameObject.transform.parent.gameObject);
                    }
                    else
                    {
                        GetComponent<AudioSource>().PlayOneShot(Zap);
                        if (transform.parent.gameObject.GetComponent<Boss1>().rotating && !transform.parent.gameObject.GetComponent<Boss1>().hitWhileRotating)
                        {
                            transform.parent.gameObject.GetComponent<Boss1>().hitWhileRotating = true;


                        }
                        else if (transform.parent.gameObject.GetComponent<Boss1>().rotating && transform.parent.gameObject.GetComponent<Boss1>().hitWhileRotating)
                        {
                            transform.parent.gameObject.GetComponent<Boss1>().waitTime /= 1.5f;

                        }
                        else if (transform.parent.gameObject.GetComponent<Boss1>().waiting)
                        {
                            transform.parent.gameObject.GetComponent<Boss1>().waitTime /= 1.1f;

                            waitTime *= 3;
                            transform.parent.gameObject.GetComponent<Boss1>().waitingTime /= 3f;
                            transform.parent.gameObject.GetComponent<Boss1>().detectChance -= 1;
                        }

                    }

                    StartCoroutine(Waiting(hit.transform.gameObject));


                }


            }
        }
        
    }

    IEnumerator Waiting(GameObject p)
    {
        electricity.SetActive(true);
        spikes.SetActive(true);
        while (spikes.transform.position.y < 0)
        {
            spikes.transform.position += new Vector3(0, 0.01f, 0);
            yield return new WaitForFixedUpdate();

        }
        if (p != null)
        {
            p.GetComponent<PlayerController>().canMove = true;
        }

        spikes.transform.position = new Vector3(0, 0, 0);
        yield return new WaitForSeconds(waitTime);
        while (transform.parent.gameObject.GetComponent<Boss1>().rotating)
        {
            yield return new WaitForEndOfFrame();
        }
        while (spikes.transform.position.y > -0.8)
        {
            spikes.transform.position -= new Vector3(0, 0.01f, 0);
            yield return new WaitForSeconds(0.005f);
        }

        spikes.transform.position = new Vector3(0, -0.8f, 0);
        spikes.SetActive(false);
        electricity.SetActive(false);
        wait = false;

        


    }
}
