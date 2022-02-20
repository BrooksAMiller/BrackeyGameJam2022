using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    public float multiplier;
    private float origin, length;

    // Start is called before the first frame update
    void Start()
    {
        origin = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float temp = (Camera.main.transform.position.x * (1 - multiplier));
        float distance = (Camera.main.transform.position.x * multiplier);
        transform.position = new Vector3(origin + distance, transform.position.y, transform.position.z);

        if (temp > origin + length) origin += length;
        else if (temp < origin - length) origin -= length;
    }
}
