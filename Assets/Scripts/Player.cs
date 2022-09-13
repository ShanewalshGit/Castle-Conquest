using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpSpeed = 15f;
    [SerializeField] float climbingSpeed = 8f;
    [SerializeField] float attackRadius = 3f;
    [SerializeField] Vector2 hitKick = new Vector2(30f, 30f);
    [SerializeField] Transform hurtBox;
    [SerializeField] AudioClip jumpingSFX, attackingSFX, gettingHitSFX, walkingSFX;

    float controlThrow;
    Rigidbody2D myRigidBody2D;
    Animator myAnimator;
    BoxCollider2D myBoxCollider2D;
    PolygonCollider2D myPlayersFeet;
    AudioSource myAudioSource;  

    float startingGravityScale;
    bool inPain = false;

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody2D = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBoxCollider2D = GetComponent<BoxCollider2D>();
        myPlayersFeet = GetComponent<PolygonCollider2D>();
        myAudioSource = GetComponent<AudioSource>();

        startingGravityScale = myRigidBody2D.gravityScale;

        myAnimator.SetTrigger("DoorOut");

    }

    // Update is called once per frame
    void Update()
    {
        if(!inPain)
        {
            Run();
            Jump();
            Climb();
            Attack();
            ExitLevel();


            if(myBoxCollider2D.IsTouchingLayers(LayerMask.GetMask("Enemy")))
            {
                PlayerHit();
            }

        }
    }

    private void ExitLevel()
    {
        if(!myBoxCollider2D.IsTouchingLayers(LayerMask.GetMask("Interactables"))) { return; }

        if(CrossPlatformInputManager.GetButtonDown("Vertical"))
        {
            myAnimator.SetTrigger("DoorIn");
        }
    }

    public void LoadNextLevel()
    {
        FindObjectOfType<ExitDoor>().StartLoadingNextLevel();
        TurnOffRenderer();
    }

    public void TurnOffRenderer()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }

    private void Attack()
    {
        if(CrossPlatformInputManager.GetButtonDown("Fire1"))
        {
            myAnimator.SetTrigger("Attacking");
            myAudioSource.PlayOneShot(attackingSFX);
            Collider2D[] enemiesToHit = Physics2D.OverlapCircleAll(hurtBox.position, attackRadius, LayerMask.GetMask("Enemy"));

            foreach(Collider2D enemy in enemiesToHit)
            {
                enemy.GetComponent<Enemy>().Dying();
            }
        
        }
    }

    public void PlayerHit()
    {
        myRigidBody2D.velocity = hitKick * new Vector2(-transform.localScale.x, 1f);
        
        myAnimator.SetTrigger("Hitting");
        myAudioSource.PlayOneShot(gettingHitSFX);
        inPain = true;
        FindObjectOfType<GameSession>().ProcessPlayerDeath();

        StartCoroutine(StopHurting());
    }

        IEnumerator StopHurting()
        {
            yield return new WaitForSeconds(2f);

            inPain = false;
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

            myAudioSource.PlayOneShot(jumpingSFX);
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

    void StepsSFX()
    {
        bool playerMovingHorizontally = Mathf.Abs(myRigidBody2D.velocity.x) > Mathf.Epsilon;
        if(playerMovingHorizontally && myBoxCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            myAudioSource.PlayOneShot(walkingSFX);
        }
        else
        {
            myAudioSource.Stop();
        }
    
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(hurtBox.position, attackRadius);
    }


}
