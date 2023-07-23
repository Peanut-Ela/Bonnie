using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Vector3 offset = new Vector3(0f, 0f, -10f);
    private float smoothTime = 0.25f;
    private Vector3 velocity = Vector3.zero;

    private Transform target;

    // Update is called once per frame
    void LateUpdate()
    {
        if (target == null)
        {
            if (Player.instance != null)
                target = Player.instance.transform;
            else
                return; // Exit the method if the player is still not found
        }

        Vector3 targetPosition = target.position + offset;
        Vector3 smoothPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        transform.position = new Vector3(smoothPosition.x, smoothPosition.y, offset.z);
    }
}
