using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    GameObject shotObject;
    [SerializeField] particleAttractorSpherical telekenisis;
    [SerializeField] float speed, maxSpeed;
    [SerializeField] GameObject voidObject;
    [SerializeField] float friction;
    float actualSpeed;
    [SerializeField] float jumpSpeed;
    [SerializeField] GameObject shot;
    private Rigidbody rb;
    bool doubleJump;
    bool firstJump = false;
    bool shooting = false;
    bool grounded = false;
    bool bouncy = false;
    public bool canMove = true;
    float lastOri = 1;
    [SerializeField] PhysicMaterial bouncyMaterial;
    bool changeG = false;
    float moveHorizontal = 0;
    float jumpAmount = 0;
    Vector3 down;
    ChangePlayerColor lastUsed;
    bool horBounce = false;
    BlockTypes.TYPES type = BlockTypes.TYPES.NORMAL;
    Rigidbody selectedRigidbody;
    Vector3 originalScreenTargetPosition;
    Vector3 originalRigidbodyPos;
    float selectionDistance;
    public float forceAmount = 500;
    string[] groundTags = new string[] {"Ground", "Shootable", "Drag"};

    // Use this for initialization
    void Start()
    {
        Physics.IgnoreLayerCollision(3, 6);
        Physics.gravity = new Vector3(0, -Mathf.Abs(Physics.gravity.y), 0);
        rb = GetComponent<Rigidbody>();
        actualSpeed = speed;
        GetComponent<BoxCollider>().material = null;

    }

    void Update()
    {
        if(rb.velocity.x != 0)
            lastOri = Math.Sign(rb.velocity.x);

        getMouseInput();
        down = transform.TransformDirection(Vector3.down * -Mathf.Sign(Physics.gravity.y));
        moveHorizontal = Input.GetAxis("Horizontal");
        if (groundedCheck(down))
        {
            grounded = true;

        }
        else
        {
            StartCoroutine(removeGrounded());
        }
        
        //changeGravity();
        if(Time.timeScale != 0)
        jump();
        

    }

    void getMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Check if we are hovering over Rigidbody, if so, select it
            selectedRigidbody = GetRigidbodyFromMouseClick();
        }
        if ((Input.GetMouseButtonUp(0) || Time.timeScale == 0) && selectedRigidbody)
        {
            selectedRigidbody.gameObject.GetComponent<ChangePlayerColor>().makeGround();
            rb.isKinematic = false;
            //Release selected Rigidbody if there any
            selectedRigidbody = null;
            telekenisis.gameObject.SetActive(false);
            GetComponent<AudioSource>().Stop();
        }
        else
        {
            if(Time.timeScale != 0)
            shoot();
        }

    }

    void moveBody()
    {

        if (selectedRigidbody)
        {
            rb.isKinematic = true;
            telekenisis.target = selectedRigidbody.gameObject.transform;
            if(!GetComponent<AudioSource>().isPlaying)
            GetComponent<AudioSource>().Play();
            telekenisis.gameObject.SetActive(true);
            selectedRigidbody.gameObject.layer = 3;
            selectedRigidbody.constraints = RigidbodyConstraints.None;
            selectedRigidbody.freezeRotation = true;
            selectedRigidbody.gameObject.transform.rotation = Quaternion.identity;
            
            Camera targetCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

            Vector3 mousePositionOffset = targetCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, selectionDistance)) - originalScreenTargetPosition;
            selectedRigidbody.velocity = (originalRigidbodyPos + mousePositionOffset - selectedRigidbody.transform.position) * forceAmount * Time.deltaTime;
        }

    }


    Rigidbody GetRigidbodyFromMouseClick()
    {
        
        Camera targetCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        RaycastHit hitInfo;
        Ray ray = targetCamera.ScreenPointToRay(Input.mousePosition);
        bool hit = Physics.Raycast(ray, out hitInfo);
        if (hit)
        {
            if (hitInfo.collider.gameObject.GetComponent<Rigidbody>())
            {
                if (hitInfo.collider.gameObject.tag.Equals("Drag"))
                {
                    selectionDistance = Vector3.Distance(ray.origin, hitInfo.point);
                    originalScreenTargetPosition = targetCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, selectionDistance));
                    originalRigidbodyPos = hitInfo.collider.transform.position;
                    return hitInfo.collider.gameObject.GetComponent<Rigidbody>();
                }
            }
        }


        return null;

    }

    private void FixedUpdate()
    {
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
        
        moveBody();
        move();
        moveClamp();
    }
    private void bounce(Collision collision)
    {
        horBounce = false;

        if (groundedCheck(down) || groundedCheck(-down))
        {
            rb.velocity = new Vector3(-collision.relativeVelocity.x, collision.relativeVelocity.y + 0.1962f * -Mathf.Sign(Physics.gravity.y), 0);
        }
        else
        {
            horBounce = true;
            StartCoroutine(removeHorbounce());
            rb.velocity = new Vector3(collision.relativeVelocity.x, -collision.relativeVelocity.y + 0.1962f * -Mathf.Sign(Physics.gravity.y), 0);

        }
    }

    IEnumerator removeHorbounce()
    {
        yield return new WaitForSeconds(0.2f);
        horBounce = false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (Array.IndexOf(groundTags, collision.gameObject.tag) > -1)
        {
            if (bouncy)
            {
                bounce(collision);
            }

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
        if (selectedRigidbody == null && shooting && Input.GetButtonUp("Fire1") && shotObject == null)
        {
            if (lastOri > 0)
                shotObject = Instantiate(shot, transform.position + Vector3.right, Quaternion.identity);
            else if (lastOri < 0)
                shotObject = Instantiate(shot, transform.position - Vector3.right, Quaternion.Euler(0, 0, 180));
        }
    }
    void changeGravity()
    {
        if (changeG)
        {
            Physics.gravity = new Vector3(0, -Physics.gravity.y, 0);

        }
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


    IEnumerator removeGrounded()
    {
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();

        grounded = groundedCheck(down);
     
        

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
        if (canMove && !horBounce)
        {
            
            // if ( Input.GetButton("Horizontal"))
            //  {

            Vector3 movement = new Vector3(moveHorizontal * actualSpeed, rb.velocity.y, 0);
            if (rb.isKinematic == false && ( Mathf.Abs(rb.velocity.x) < Mathf.Abs(movement.x) || movement.x * rb.velocity.x < 0))
                rb.velocity = movement;

            // }
            if (Mathf.Abs(jumpAmount) > 0 && rb.velocity.y != Mathf.Sign(jumpAmount))
            {
                rb.velocity = new Vector3(rb.velocity.x, 0);
                rb.AddForce(Vector2.up * jumpAmount, ForceMode.Impulse);
               
            }
            
           
          
            jumpAmount = 0;
            


                if (groundedCheck(down))
                {

                    
                        if (Mathf.Abs(rb.velocity.x) > 0 && !Input.GetButton("Horizontal"))
                        {
                            rb.velocity = new Vector3(rb.velocity.x - (1 / friction) * Mathf.Abs(rb.velocity.x) * (rb.velocity.x / Mathf.Abs(rb.velocity.x)), rb.velocity.y, 0);

                        }


                }
            
        }
    }
    // Update is called once per frame




    void changeColor(GameObject g, Collision collision)
    {


        ChangePlayerColor cpc = g.GetComponent<ChangePlayerColor>();
        if(cpc == null)
        {
            return;
        }
        if(g.GetComponent<AudioSource>().clip != null)
        g.GetComponent<AudioSource>().PlayOneShot(g.GetComponent<AudioSource>().clip);
        BlockTypes.TYPES cpcType = cpc.getType();



        if (cpc.gameObject.tag == "Drag")
        {
            return;
        }
        actualSpeed = speed;
        doubleJump = false;
        shooting = false;
        changeG = false;
        bouncy = false;

        gameObject.GetComponentInChildren<Renderer>().material.SetColor("_Color", cpc.getColor());

        if (cpcType == BlockTypes.TYPES.VOIDOPEN)
        {
            cpc.changeType(BlockTypes.TYPES.NORMAL, false);
            voidObject.GetComponent<Void>().voidShow();
        }
        else if (cpcType == BlockTypes.TYPES.GRAVITY)
        {
            if (cpc != lastUsed)
            {
                changeG = true;
                changeGravity();
            }
        }
        else if (cpcType == BlockTypes.TYPES.DOUBLE_JUMP)
        {
            doubleJump = true;
        }
        else if (cpcType == BlockTypes.TYPES.BOUNCE)
        {
            bouncy = true;
            bounce(collision);
        }
        else if (cpcType == BlockTypes.TYPES.SPEED)
        {
            actualSpeed = speed * 2f;

        }
        else if (cpcType == BlockTypes.TYPES.SHOOT)
        {
            shooting = true;
        }
        else if (cpcType == BlockTypes.TYPES.ROTATE)
        {
            if (cpc != lastUsed)
            {
                StartCoroutine(RotateWorld());
            }
        }
    

        type = cpcType;
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
