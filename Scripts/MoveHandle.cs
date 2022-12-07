using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveHandle : MonoBehaviour
{
    public float min = 3f;
    public float max = 3f;
    public float speed = 10;
    // Use this for initialization
    void Start()
    {

        min = transform.position.y + min;
        max = transform.position.y + max;

    }

    // Update is called once per frame
    void Update()
    {


        transform.position = new Vector3(transform.position.x, Mathf.PingPong(Time.time * speed, max - min) + min, transform.position.z);

    }
}
