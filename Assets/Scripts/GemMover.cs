using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemMover : MonoBehaviour
{
    private float offSet;
    private float direction;
    private Vector3 basePosition;
    // Start is called before the first frame update
    void Start()
    {
        basePosition = transform.position;

        offSet = Random.Range(1f, 19f);

        direction = Random.Range(-1f, 1f);
        if (direction < 0f) direction = -1f;
        else direction = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        offSet += (Time.deltaTime * direction * 25f);

        if (offSet > 20f)
        {
            offSet = 20f;
            direction = -1f;
        }
        else if (offSet < 0f)
        {
            offSet = 0f;
            direction = 1f;
        }

        transform.position = basePosition + (Vector3.up * (offSet / 100f));
    }
}
