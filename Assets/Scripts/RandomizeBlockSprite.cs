using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeBlockSprite : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] Sprite[] sprites;

    // Start is called before the first frame update
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        int choice = Random.Range(0, sprites.Length - 1);
        spriteRenderer.sprite = sprites[choice];
    }
}
