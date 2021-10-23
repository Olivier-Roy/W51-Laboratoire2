using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private const string DECREMENT_METHOD_NAME = "DecrementExitPoints";

    private const string GEM_TAG = "Gem";
    private const string EXIT_TAG = "Exit";
    private const string MONSTER_TAG = "Monster";
    
    private const int NB_POINTS_GEM = 50;
    private const int MAX_NB_POINTS_EXIT = 600;
    private const int NB_POINTS_EXIT_DECREMENT = 5;

    private const int NB_SECONDS_DECREMENT = 1;
    private const int NB_SECONDS_SCALE_DOWN = 1;
    private const int NB_SECONDS_NEXT_SCENE_DELAY = 2;
    private const int NB_SECONDS_RESTART_SCENE_DELAY = 1;

    private bool touchingGem = false;
    private bool touchingExit = false;
    private bool touchingMonster = false;

    private AudioSource audioSource;
    private PlayerMovement playerMovement;

    private int nbPointsExit = MAX_NB_POINTS_EXIT;

    void Start()
    {
        audioSource = FindObjectOfType<AudioSource>();
        playerMovement = gameObject.GetComponent<PlayerMovement>();

        InvokeRepeating(DECREMENT_METHOD_NAME, 0, NB_SECONDS_DECREMENT);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == GEM_TAG && !touchingGem)
        {
            touchingGem = true;
            collision.gameObject.SetActive(false);
            audioSource.PlayOneShot(SoundManager.Instance.GemCollected);
            GameManager.instance.AddScore(NB_POINTS_GEM);
        }
        else if (collision.gameObject.tag == EXIT_TAG && !touchingExit)
        {
            touchingExit = true;
            playerMovement.CanMove(false);
            audioSource.PlayOneShot(SoundManager.Instance.ExitReached);
            GameManager.instance.AddScore(nbPointsExit);
            GameManager.instance.StartNextlevel(NB_SECONDS_NEXT_SCENE_DELAY);
        }
        else if (collision.gameObject.tag == MONSTER_TAG && !touchingMonster)
        {
            touchingMonster = true;
            playerMovement.CanMove(false);
            GameManager.instance.PlayerDie();
            StartCoroutine(ScaleDownToZero());
            audioSource.PlayOneShot(SoundManager.Instance.PlayerKilled);
            GameManager.instance.RestartLevel(NB_SECONDS_RESTART_SCENE_DELAY);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == GEM_TAG && touchingGem)
            touchingGem = false;
    }

    private void DecrementExitPoints()
    {
        if (nbPointsExit >= NB_POINTS_EXIT_DECREMENT)
            nbPointsExit -= NB_POINTS_EXIT_DECREMENT;
    }

    // https://answers.unity.com/answers/805226/view.html
    private IEnumerator ScaleDownToZero()
    {
        float currentTime = 0.0f;
        do
        {
            gameObject.transform.localScale = Vector3.Lerp(
                gameObject.transform.localScale, Vector3.zero, currentTime / NB_SECONDS_SCALE_DOWN);
            currentTime += Time.deltaTime;
            yield return null;
        }
        while (currentTime <= NB_SECONDS_SCALE_DOWN);
    }
}
