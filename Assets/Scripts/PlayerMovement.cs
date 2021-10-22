using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private const float WALKING_VELOCITY = 2f;
    private const float JUMPING_VELOCITY = 7f;
    
    private const string GEM_TAG = "Gem";
    private const string MONSTER_TAG = "Monster";
    private const string GROUND_BLOCKS_TAG = "HardBlock";

    [SerializeField] GameObject groundBlock;

    private Rigidbody2D rigidbody;
    private AudioSource audioSource;

    private int grounded = 0;
    private bool touchingBlockSide = false;

    void Start()
    {
        audioSource = FindObjectOfType<AudioSource>();
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        BuildVerticalMovement();
    }

    void FixedUpdate()
    {
        BuildSurfaceMovement();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == GROUND_BLOCKS_TAG)
            grounded++;
        else if (collision.gameObject.tag == GEM_TAG)
        {
            collision.gameObject.SetActive(false);
            audioSource.PlayOneShot(SoundManager.Instance.GemCollected);
        }
        else if (collision.gameObject.tag == MONSTER_TAG)
        {
            gameObject.SetActive(false);
            AudioSource.PlayClipAtPoint(SoundManager.Instance.PlayerKilled, gameObject.transform.position);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == GROUND_BLOCKS_TAG)
            grounded--;
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == GROUND_BLOCKS_TAG && grounded <= 0)
            touchingBlockSide = true;   
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        touchingBlockSide = false;
    }

    private void BuildSurfaceMovement()
    {
        float input = Input.GetAxisRaw("Horizontal");
        float velocity = 0f;

        if (!touchingBlockSide)
        {
            if (input >= 0.1f)
                velocity = WALKING_VELOCITY;
            else if (input <= -0.1f)
                velocity = -WALKING_VELOCITY;
        }

        rigidbody.velocity = new Vector2(velocity, rigidbody.velocity.y);
    }

    private void BuildVerticalMovement()
    {
        if (Input.GetButtonDown("Fire1") && grounded > 0)
        {
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, JUMPING_VELOCITY);
            audioSource.PlayOneShot(SoundManager.Instance.PlayerJump);
        }
    }
}
