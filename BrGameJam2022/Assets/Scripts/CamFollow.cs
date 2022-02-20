using UnityEngine;

public class CamFollow : MonoBehaviour
{
    public Transform target;

    public float smoothSpeed = 0.125f;

    public Vector3 offset;

    void FixedUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        desiredPosition.z = -10f;
        Vector3 smoothedPosition = Vector3.Lerp(new Vector3(transform.position.x, transform.position.y, -10f), desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

    }

}