using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceObject : MonoBehaviour
{
    string[] groundTags = new string[] { "Ground", "Shootable", "Drag" };
    Vector3 down;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        down = transform.TransformDirection(Vector3.down * -Mathf.Sign(Physics.gravity.y));

    }
    bool groundedCheck(Vector3 down)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, down, out hit, GetComponent<BoxCollider>().bounds.extents.y + 0.1f) || Physics.Raycast(transform.position + new Vector3(transform.localScale.x / 2, 0, 0), down, out hit, GetComponent<BoxCollider>().bounds.extents.y + 0.1f) || Physics.Raycast(transform.position - new Vector3(transform.localScale.x / 2, 0, 0), down, out hit, GetComponent<BoxCollider>().bounds.extents.y + 0.1f))
        {
            if (Array.IndexOf(groundTags, hit.transform.gameObject.tag) > -1)
            {
                return true;

            }
        }

        return false;

    }
    private void bounce(Collision collision)
    {

        if (groundedCheck(down) || groundedCheck(-down))
        {
            rb.velocity = new Vector3(-collision.relativeVelocity.x, collision.relativeVelocity.y + 0.1962f * -Mathf.Sign(Physics.gravity.y), 0);
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (Array.IndexOf(groundTags, collision.gameObject.tag) > -1)
        {
            
                bounce(collision);
            
        }
    }
}
