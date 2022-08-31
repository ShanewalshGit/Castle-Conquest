using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    [SerializeField] float runSpeed = 10f;

    float controlThrow;
    Rigidbody2D myRigidBody2D;

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Run();
    }

    private void Run()
    {
        controlThrow = CrossPlatformInputManager.GetAxis("Horizontal");

        Vector2 PlayerVelocity = new Vector2(controlThrow * runSpeed, myRigidBody2D.velocity.y);
        myRigidBody2D.velocity = PlayerVelocity;

        FlipSprite();
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
