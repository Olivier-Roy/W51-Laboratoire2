using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private const string DECREMENT_METHOD_NAME = "DecrementCoyoteTime";

    private const string GROUND_BLOCKS_TAG = "HardBlock";
    private const string PLATFORM_BLOCKS_TAG = "Platform";
    private const string SCREEN_LIMITS_TAG = "ScreenLimits";

    private const int MAX_COYOTE_TIME = 20; // En centièmes de secondes
    private const float WALKING_VELOCITY = 2f;
    private const float NB_SECONDS_DECREMENT = 0.01f;
    private const float HOLD_JUMP_TIME_LIMIT = 0.25f;
    private const float INITIAL_JUMPING_VELOCITY = 3f;
    private const int JUMPING_VELOCITY_MULTIPLIER = 20;

    private Rigidbody2D rigidbody2d;
    private AudioSource audioSource;

    private int grounded = 0;
    private bool canMove = true;
    private bool holdJump = false;
    private float holdJumpTimer = 0f;
    private bool touchingBlockSide = false;
    private bool touchingScreenLimit = false;
    private float coyoteTime = MAX_COYOTE_TIME;

    void Start()
    {
        audioSource = FindObjectOfType<AudioSource>();
        rigidbody2d = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        ManageCoyoteTime();
        ManageJumpButtonDown();
        BuildVerticalMovement();
    }

    void FixedUpdate()
    {
        BuildSurfaceMovement();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == GROUND_BLOCKS_TAG ||
            collision.gameObject.tag == PLATFORM_BLOCKS_TAG)
            grounded++;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == GROUND_BLOCKS_TAG ||
            collision.gameObject.tag == PLATFORM_BLOCKS_TAG)
            grounded--;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.tag == GROUND_BLOCKS_TAG ||
            collision.gameObject.tag == PLATFORM_BLOCKS_TAG)
            && !touchingBlockSide)
        {
            CancelInvoke(DECREMENT_METHOD_NAME);
            coyoteTime = MAX_COYOTE_TIME;
            touchingScreenLimit = false;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (grounded <= 0)
        {
            if (collision.gameObject.tag == GROUND_BLOCKS_TAG)
                touchingBlockSide = true;
            else if (collision.gameObject.tag == SCREEN_LIMITS_TAG)
                touchingScreenLimit = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        touchingBlockSide = false;
    }

    public void CanMove(bool canMove)
    {
        this.canMove = canMove;
    }

    private void BuildSurfaceMovement()
    {
        float input = Input.GetAxisRaw("Horizontal");
        float velocity = 0f;

        if (!touchingBlockSide && !touchingScreenLimit && canMove)
        {
            if (input >= 0.1f)
                velocity = WALKING_VELOCITY;
            else if (input <= -0.1f)
                velocity = -WALKING_VELOCITY;
        }

        rigidbody2d.velocity = new Vector2(velocity, rigidbody2d.velocity.y);
    }

    private void ManageJumpButtonDown()
    {
        if (Input.GetButtonDown("Fire1") && (grounded > 0 || coyoteTime > 0) && canMove)
            holdJump = true;

        if (holdJump)
            holdJumpTimer += Time.deltaTime;
    }

    private void BuildVerticalMovement()
    {
        if ((Input.GetButtonUp("Fire1") && holdJump ||
            holdJumpTimer >= HOLD_JUMP_TIME_LIMIT) &&
            (grounded > 0 || coyoteTime > 0) && canMove)
        {
            coyoteTime = 0f;
            rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x,
                INITIAL_JUMPING_VELOCITY + (holdJumpTimer * JUMPING_VELOCITY_MULTIPLIER));
            audioSource.PlayOneShot(SoundManager.Instance.PlayerJump);
            ResetJump();
        }
        else if (holdJumpTimer >= HOLD_JUMP_TIME_LIMIT)
            ResetJump();
    }

    private void ResetJump()
    {
        holdJumpTimer = 0f;
        holdJump = false;
    }

    private void ManageCoyoteTime()
    {
        if (grounded <= 0 && !holdJump && coyoteTime == MAX_COYOTE_TIME)
            InvokeRepeating(DECREMENT_METHOD_NAME, 0, NB_SECONDS_DECREMENT);
        if (coyoteTime <= 0f)
            CancelInvoke(DECREMENT_METHOD_NAME);
    }

    private void DecrementCoyoteTime()
    {
        coyoteTime--;
    }
}
