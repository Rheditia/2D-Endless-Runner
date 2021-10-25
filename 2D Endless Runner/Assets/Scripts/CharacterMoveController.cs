using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMoveController : MonoBehaviour
{
    [Header("Movement")]
    public float moveAccel;
    public float maxSpeed;


    [Header("Jump")]
    public float jumpAccel;


    [Header("Ground Raycast")]
    public float groundRaycastDistance;
    public LayerMask groundLayerMask;

    private bool isJumping;
    private bool isOnGround;

    private Rigidbody2D rb2d;
    private Animator animator;
    private CharacterSoundController sound;
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sound = GetComponent<CharacterSoundController>();
    }

    // Update is called once per frame
    void Update()
    {
        // read input
        if (Input.GetMouseButtonDown(0))
        {
            if (isOnGround)
            {
                isJumping = true;
                sound.PlayJump();
            }
        }
        // change animation
        animator.SetBool("isOnGround", isOnGround);
    }

    void FixedUpdate()
    {
        // raycast ground
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundRaycastDistance, groundLayerMask);
        if (hit)
        {
            if (!isOnGround && rb2d.velocity.y <= 0)
            {
                isOnGround = true;
            }
        }
        else
        {
            isOnGround = false;
        }

        // calculate velocity vector
        Vector2 velocityVector = rb2d.velocity;

        if (isJumping)
        {
            velocityVector.y += jumpAccel;
            isJumping = false;
        }
        
        velocityVector.x = Mathf.Clamp(velocityVector.x + moveAccel * Time.deltaTime, 0.0f, maxSpeed);
        rb2d.velocity = velocityVector;
    }

    private void OnDrawGizmos()
    {
        Debug.DrawLine(transform.position, transform.position + (Vector3.down * groundRaycastDistance), Color.white);
    }
}
