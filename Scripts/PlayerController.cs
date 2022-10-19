using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float friction;
    float actualSpeed;
    [SerializeField] float jumpSpeed;
    [SerializeField] GameObject shot;
    private Rigidbody rb;
    bool doubleJump;
    bool firstJump = false;
    bool shooting = false;
    bool grounded = false;
    public bool canMove = true;
    float lastOri = 1;
    [SerializeField] PhysicMaterial bouncy;
    bool changeG = false;
    float moveHorizontal = 0;
    float jumpAmount = 0;
    Vector3 down;
    ChangePlayerColor lastUsed;
    BlockTypes.TYPES type = BlockTypes.TYPES.NORMAL;
    // Use this for initialization
    void Start()
    {
        Physics.gravity = new Vector3(0, -Mathf.Abs(Physics.gravity.y), 0);
        rb = GetComponent<Rigidbody>();
        actualSpeed = speed;
        GetComponent<BoxCollider>().material = null;

    }

    void Update()
    {
        down = transform.TransformDirection(Vector3.down * -Mathf.Sign(Physics.gravity.y));
        moveHorizontal = Input.GetAxis("Horizontal");
        groundedCheck();
        shoot();
        //changeGravity();
        jump();
        

    }

    private void FixedUpdate()
    {
        move();
        moveClamp();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Ground"))
        {

            changeColor(collision.gameObject, collision);

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Boundry"))
        {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().resetScene(false);
        }




    }

    public void protection()
    {
        StartCoroutine(protectionCoroutine());
    }
    public IEnumerator protectionCoroutine()
    {
        if (GetComponent<BoxCollider>().enabled)
        {
            GetComponent<BoxCollider>().enabled = false;
            yield return new WaitForEndOfFrame();
            yield return new WaitForFixedUpdate();
            GetComponent<BoxCollider>().enabled = true;
        }
    }
    void shoot()
    {
        if (shooting && Input.GetButtonUp("Fire1") && GameObject.FindGameObjectsWithTag("Shot").Length == 0)
        {
            if (lastOri > 0)
                Instantiate(shot, transform.position + Vector3.right, Quaternion.identity);
            else if (lastOri < 0)
                Instantiate(shot, transform.position - Vector3.right, Quaternion.Euler(0, 0, 180));
        }
    }
    void changeGravity()
    {
        if (changeG)
        {
            Physics.gravity = new Vector3(0, -Physics.gravity.y, 0);

        }
    }

    void groundedCheck()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, down, out hit, GetComponent<BoxCollider>().bounds.extents.y + 0.1f) || Physics.Raycast(transform.position + new Vector3(transform.localScale.x / 2, 0, 0), down, out hit, GetComponent<BoxCollider>().bounds.extents.y + 0.1f) || Physics.Raycast(transform.position - new Vector3(transform.localScale.x / 2, 0, 0), down, out hit, GetComponent<BoxCollider>().bounds.extents.y + 0.1f))
        {
            if (hit.transform.gameObject.tag.Equals("Ground") && hit.transform.gameObject.GetComponent<ChangePlayerColor>().getType() != BlockTypes.TYPES.BOUNCE)
            {
                grounded = true;
                return;

            }
        }

        StartCoroutine(removeGrounded());

    }

    IEnumerator removeGrounded()
    {
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();

        RaycastHit hit;
        if (Physics.Raycast(transform.position, down, out hit, GetComponent<BoxCollider>().bounds.extents.y + 0.1f) || Physics.Raycast(transform.position + new Vector3(transform.localScale.x / 2, 0, 0), down, out hit, GetComponent<BoxCollider>().bounds.extents.y + 0.1f) || Physics.Raycast(transform.position - new Vector3(transform.localScale.x / 2, 0, 0), down, out hit, GetComponent<BoxCollider>().bounds.extents.y + 0.1f))
        {
            if (hit.transform.gameObject.tag.Equals("Ground") && hit.transform.gameObject.GetComponent<ChangePlayerColor>().getType() != BlockTypes.TYPES.BOUNCE)
            {
                grounded = true;

            }
            else
            {
                grounded = false;
            }
        }
        else
        {
            grounded = false;
        }

    }
    void jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            
            if (grounded)
            {
                
                grounded = false;
                jumpAmount = jumpSpeed * -Mathf.Sign(Physics.gravity.y);

                firstJump = true;
            }
            else if (doubleJump && firstJump)
            {
                grounded = false;
                jumpAmount = 1.3f * jumpSpeed * -Mathf.Sign(Physics.gravity.y);
                firstJump = false;

            }
           



        }
    }

    private void moveClamp()
    {
        if (rb.velocity.y > 0 && rb.velocity.y < 0.1 || rb.velocity.y < 0 && rb.velocity.y > -0.1)
            rb.velocity = new Vector3(rb.velocity.x, 0, 0);
    }

    private void move()
    {
        if (canMove)
        {
            
            // if ( Input.GetButton("Horizontal"))
            //  {

            Vector3 movement = new Vector3(moveHorizontal * actualSpeed, rb.velocity.y, 0);
            if (Mathf.Abs(rb.velocity.x) < Mathf.Abs(movement.x) || movement.x * rb.velocity.x < 0)
                rb.velocity = movement;
            if (moveHorizontal != 0)
                lastOri = moveHorizontal;
            // }
            if (Mathf.Abs(jumpAmount) > 0 && rb.velocity.y != Mathf.Sign(jumpAmount))
            {
                rb.velocity = new Vector3(rb.velocity.x, 0);
                rb.AddForce(Vector2.up * jumpAmount, ForceMode.Impulse);
            }
            
           
          
            jumpAmount = 0;
            RaycastHit hit;
            

            if (Physics.Raycast(transform.position, down, out hit, GetComponent<BoxCollider>().bounds.extents.y + 0.1f) || Physics.Raycast(transform.position + new Vector3(transform.localScale.x / 2, 0, 0), down, out hit, GetComponent<BoxCollider>().bounds.extents.y + 0.1f) || Physics.Raycast(transform.position - new Vector3(transform.localScale.x / 2, 0, 0), down, out hit, GetComponent<BoxCollider>().bounds.extents.y + 0.1f))
            {

                if (hit.transform.gameObject.tag.Equals("Ground"))
                {

                    if (hit.transform.gameObject.GetComponent<ChangePlayerColor>().getType() != BlockTypes.TYPES.BOUNCE)
                        if (Mathf.Abs(rb.velocity.x) > 0 && !Input.GetButton("Horizontal"))
                        {
                            rb.velocity = new Vector3(rb.velocity.x - (1 / friction) * Mathf.Abs(rb.velocity.x) * (rb.velocity.x / Mathf.Abs(rb.velocity.x)), rb.velocity.y, 0);

                        }


                }
            }
        }
    }
    // Update is called once per frame




    void changeColor(GameObject g, Collision collision)
    {

        
        ChangePlayerColor cpc = g.GetComponent<ChangePlayerColor>();
        
        g.GetComponent<AudioSource>().PlayOneShot(g.GetComponent<AudioSource>().clip);
        gameObject.GetComponentInChildren<Renderer>().material.SetColor("_Color", cpc.getColor());

        RaycastHit hit;


        actualSpeed = speed;
        doubleJump = false;
        shooting = false;
        changeG = false;

        if (cpc.getType() == BlockTypes.TYPES.GRAVITY)
        {
            if (cpc != lastUsed)
            {
                changeG = true;
                changeGravity();
            }
        }
        else if (cpc.getType() == BlockTypes.TYPES.DOUBLE_JUMP)
        {
            doubleJump = true;
        }
        else if (cpc.getType() == BlockTypes.TYPES.BOUNCE)
        {
            if (Physics.Raycast(transform.position, down, out hit, GetComponent<BoxCollider>().bounds.extents.y + 0.1f) || Physics.Raycast(transform.position + new Vector3(transform.localScale.x / 2, 0, 0), down, out hit, GetComponent<BoxCollider>().bounds.extents.y + 0.1f) || Physics.Raycast(transform.position - new Vector3(transform.localScale.x / 2, 0, 0), down, out hit, GetComponent<BoxCollider>().bounds.extents.y + 0.1f))
            {
                if (hit.transform.gameObject.tag.Equals("Ground"))
                {

                    if (hit.transform.gameObject.GetComponent<ChangePlayerColor>().getType() == BlockTypes.TYPES.BOUNCE)
                    {

                        rb.velocity = new Vector3(-collision.relativeVelocity.x, collision.relativeVelocity.y + 0.1962f * -Mathf.Abs(Physics.gravity.y) / Physics.gravity.y, 0);

                    }

                }
            }

        }
        else if (cpc.getType() == BlockTypes.TYPES.SPEED)
        {
            actualSpeed = speed * 2f;

        }
        else if (cpc.getType() == BlockTypes.TYPES.SHOOT)
        {
            shooting = true;
        }
        else if (cpc.getType() == BlockTypes.TYPES.ROTATE)
        {
            if (cpc != lastUsed)
            {
                StartCoroutine(RotateWorld());
            }
        }
        if (cpc.getType() != BlockTypes.TYPES.BOUNCE)
        {
            if (rb.velocity.x > 0)
                rb.velocity = new Vector3(1, 0, 0);
            else if (rb.velocity.x < 0)
                rb.velocity = new Vector3(-1, 0, 0);
        }

        type = cpc.getType();
        lastUsed = cpc;


    }

    IEnumerator RotateWorld()
    {

        GameObject MainContainer = GameObject.FindGameObjectWithTag("MainContainer");
        //Physics.gravity = new Vector3(0, -Physics.gravity.y, 0);
        for (int i = 0; i < 45; i++)
        {

            MainContainer.transform.RotateAround(GameObject.FindGameObjectWithTag("MainCamera").transform.position, new Vector3(0, 0, 1), -2);
            yield return new WaitForFixedUpdate();
        }



    }

}
