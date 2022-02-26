using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienEscape : MonoBehaviour
{
    private void FixedUpdate()
    {
        GetComponent<Rigidbody2D>().AddForce(new Vector2(100f, 0) * Time.deltaTime);
    }
}
