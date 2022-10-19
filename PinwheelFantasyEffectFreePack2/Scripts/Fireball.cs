using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour {

    public Rigidbody rgbd;
    public GameObject fieryParticle;
    public GameObject smokeParticle;
    public GameObject explosionParticle;
    Vector3 oT;
    private void Start()
    {
        GetComponent<Rigidbody>().AddForce(transform.right * 1000);


        StartCoroutine(DestroyWithTime());
    }
    private void FixedUpdate()
    {
       
    }
    private void OnCollisionEnter(Collision collision)
    {
        GetComponent<SphereCollider>().enabled = false;
        fieryParticle.SetActive(false);
        smokeParticle.SetActive(false);
        explosionParticle.SetActive(true);
        rgbd.constraints = RigidbodyConstraints.FreezeAll;
        if (collision.gameObject.tag.Equals("Shootable"))
        {
            if(collision.gameObject.GetComponent<ChangePlayerColor>().getType() == BlockTypes.TYPES.SHOOTABLE)
            Destroy(collision.gameObject);
        }
    }

    IEnumerator DestroyWithTime()
    {
        yield return new WaitForSeconds(1.7f);
        Destroy(gameObject);
    }

    


}
