using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySmallEnemy : MonoBehaviour {
    [SerializeField] GameObject electricity;
    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void Update() {
        RaycastHit hit;
        Vector3 up = transform.TransformDirection(Vector3.up * -Physics.gravity.y / Mathf.Abs(Physics.gravity.y));

        if (Physics.Raycast(transform.position, up, out hit, GetComponent<BoxCollider>().bounds.extents.y + 0.1f) || Physics.Raycast(transform.position + new Vector3(transform.localScale.x / 2, 0, 0), up, out hit, GetComponent<BoxCollider>().bounds.extents.y + 0.1f) || Physics.Raycast(transform.position - new Vector3(transform.localScale.x / 2, 0, 0), up, out hit, GetComponent<BoxCollider>().bounds.extents.y + 0.1f))
        {

            if (hit.transform.gameObject.tag.Equals("Player"))
            {
                Instantiate(electricity, transform.position, Quaternion.identity);

                Destroy(gameObject.transform.parent.gameObject);

            }
        }
    }

   
}

