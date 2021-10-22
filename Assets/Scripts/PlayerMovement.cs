using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private const float WALKING_VELOCITY = 2f;
    private const float JUMPING_VELOCITY = 7f;
    private const float HORIZONTAL_SCREEN_LIMIT = 3.87f;
    private const float VERTICAL_SCREEN_LIMIT = 2.15f;

    private AudioSource audioSource;
    private Rigidbody2D rigidbody;
    private int grounded = 0;

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        grounded++;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        grounded--;
    }

    private void BuildSurfaceMovement()
    {
        float input = Input.GetAxisRaw("Horizontal");
        float velocity = 0f;

        if (input >= 0.1f)
            velocity = WALKING_VELOCITY;
        else if (input <= -0.1f)
            velocity = -WALKING_VELOCITY;

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
