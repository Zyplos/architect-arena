using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlatformMover : NetworkBehaviour
{
    public float speed = 2.0f;     // Speed of the movement
    public float height = 1.0f;    // Height of the sine wave
    private float initialY;        // Initial Y position of the platform

    void Start()
    {
        initialY = transform.position.y;
    }

    void Update()
    {
        // Calculate the new Y position using a sine wave
        float newY = initialY + Mathf.Sin(Time.time * speed) * height;

        // Update the platform's position
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
