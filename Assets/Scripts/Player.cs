using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpSpeed = 15f;
    [SerializeField] float climbingSpeed = 8f;

    float controlThrow;
    Rigidbody2D myRigidBody2D;
    Animator myAnimator;
    BoxCollider2D myBoxCollider2D;
    PolygonCollider2D myPlayersFeet;

    float startingGravityScale;

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody2D = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBoxCollider2D = GetComponent<BoxCollider2D>();
        myPlayersFeet = GetComponent<PolygonCollider2D>();

        startingGravityScale = myRigidBody2D.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        Run();
        Jump();
        Climb();
    }

    private void Climb()
    {
        if(myBoxCollider2D.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            float controlThrow = CrossPlatformInputManager.GetAxis("Vertical");
            Vector2 climbingVelocity = new Vector2(myRigidBody2D.velocity.x, controlThrow * climbingSpeed);
            
            myRigidBody2D.velocity = climbingVelocity;

            myAnimator.SetBool("Climbing", true);

            myRigidBody2D.gravityScale = 0f;
        }
        else
        {
            myAnimator.SetBool("Climbing", false);
            myRigidBody2D.gravityScale = startingGravityScale;
        }
    }

    private void Jump()
    {
        if(!myPlayersFeet.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return; }

        bool isJumping = CrossPlatformInputManager.GetButtonDown("Jump");
        if(isJumping)
        {
            Vector2 jumpVelocity = new Vector2(myRigidBody2D.velocity.x, jumpSpeed);
            myRigidBody2D.velocity = jumpVelocity;
        }
    }

    private void Run()
    {
        controlThrow = CrossPlatformInputManager.GetAxis("Horizontal");

        Vector2 PlayerVelocity = new Vector2(controlThrow * runSpeed, myRigidBody2D.velocity.y);
        myRigidBody2D.velocity = PlayerVelocity;

        FlipSprite();
        ChangingToRunningState();

    }

    private void ChangingToRunningState()
    {
        bool runningHorizontally = Mathf.Abs(myRigidBody2D.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("Running", runningHorizontally);
    }

    private void FlipSprite()
    {
        bool runningHorizontally = Mathf.Abs(myRigidBody2D.velocity.x) > Mathf.Epsilon;

        if(runningHorizontally)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidBody2D.velocity.x), 1f);
        }
    }

}
